using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Dimensioning;

using ParameterManager;
using LogMessageManager;

namespace InspectionSystemManager
{
    class InspectionLeadForm
    {
        private CogLeadFormResult   LeadFormResult = new CogLeadFormResult();

        private CogBlob             BlobProc;
        //private CogBlobResults      BlobResults;
        //private CogBlobResult       BlobResult;

        #region Initialize & Deinitialize
        public InspectionLeadForm()
        {
            BlobProc = new CogBlob();
            //BlobResults = new CogBlobResults();
            //BlobResult = new CogBlobResult();

            BlobProc.SegmentationParams.Mode = CogBlobSegmentationModeConstants.HardFixedThreshold;
            BlobProc.SegmentationParams.Polarity = CogBlobSegmentationPolarityConstants.LightBlobs;
            BlobProc.SegmentationParams.HardFixedThreshold = 100;
            BlobProc.ConnectivityMode = CogBlobConnectivityModeConstants.GreyScale;
            BlobProc.ConnectivityCleanup = CogBlobConnectivityCleanupConstants.Fill;
            BlobProc.ConnectivityMinPixels = 10;
        }

        public void Initialize()
        {

        }

        public void DeInitialize()
        {

        }
        #endregion

        public bool Run(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadFormAlgo _CogLeadFormAlgo, ref CogLeadFormResult _CogLeadFormResult, int _NgNumber = 0)
        {
            bool _Result = false;

            ClearLeadFormResult(_CogLeadFormAlgo.LeadCount);

            CogRectangle _LeadAlignArea = new CogRectangle();

            _LeadAlignArea.SetCenterWidthHeight(_CogLeadFormAlgo.AlignArea.CenterX, _CogLeadFormAlgo.AlignArea.CenterY, _CogLeadFormAlgo.AlignArea.Width, _CogLeadFormAlgo.AlignArea.Height);

            do
            {
                if (false ==  LeadAlignSE(_SrcImage, _LeadAlignArea, _CogLeadFormAlgo)) break;

                _Result = true;
            } while (false);

            _CogLeadFormResult = LeadFormResult;

            return _Result;
        }

        public void ClearLeadFormResult(int _LeadCount)
        {
            LeadFormResult = new CogLeadFormResult();
            LeadFormResult.EachLeadStatusArray = new EachLeadStatus[_LeadCount];
            for (int iLoopCount = 0; iLoopCount < _LeadCount; ++iLoopCount)
                LeadFormResult.EachLeadStatusArray[iLoopCount] = new EachLeadStatus();
        }

        public bool LeadAlignSE(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadFormAlgo _CogLeadFormAlgo)
        {
            if (LeadFormResult.IsGood != true) return true;

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadAlign - Start", CLogManager.LOG_LEVEL.MID);

            if (false == _CogLeadFormAlgo.IsUseAlign)
            {
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadAlign Disable - End", CLogManager.LOG_LEVEL.MID);
                return true;
            }

            bool _Result = true;
            List<double> _AlignPositionX = new List<double>();
            List<double> _AlignPositionY = new List<double>();

            try
            {
                System.Diagnostics.Stopwatch _ProcessWatch = new System.Diagnostics.Stopwatch();
                _ProcessWatch.Reset(); _ProcessWatch.Start();

                SetHardFixedThreshold(_CogLeadFormAlgo.AlignThreshold);
                SetConnectivityMinimum(500);
                SetPolarity(true);

                CogBlobResults _BlobResults = BlobProc.Execute(_SrcImage, _InspRegion);
                List<double> _GuidePositionX = GetLeadGuidePosition(_BlobResults);

                #region Caliper Tool Setting => Y좌표 위치 정보 찾기
                LeadFormResult.LeadCount = _GuidePositionX.Count;

                if (_GuidePositionX.Count != _CogLeadFormAlgo.LeadCount)
                {
                    LeadFormResult.IsGood = false;
                    LeadFormResult.NgType = eNgType.LEAD_CNT;
                    LeadFormResult.SearchArea.SetCenterWidthHeight(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height);
                    LeadFormResult.LeadCountStatus = LeadFormResult.LeadCount.ToString();
                    return false;
                }

                for (int iLoopCount = 0; iLoopCount < _GuidePositionX.Count; ++iLoopCount)
                {
                    CogRectangleAffine _CaliperRegion = new CogRectangleAffine();
                    _CaliperRegion.SetCenterLengthsRotationSkew(_GuidePositionX[iLoopCount], _InspRegion.CenterY,_InspRegion.Height, 170, 1.5708, 0);

                    CogCaliperTool _LeadCaliper = new CogCaliperTool();
                    _LeadCaliper.RunParams.EdgeMode = CogCaliperEdgeModeConstants.SingleEdge;
                    _LeadCaliper.RunParams.Edge0Polarity = CogCaliperPolarityConstants.DarkToLight;
                    _LeadCaliper.RunParams.MaxResults = 2;
                    _LeadCaliper.Region = _CaliperRegion;

                    _LeadCaliper.InputImage = _SrcImage;
                    _LeadCaliper.Run();

                    if (_LeadCaliper.Results.Count == 1)
                        _AlignPositionY.Add(_LeadCaliper.Results[0].PositionY);
                    else if (_LeadCaliper.Results.Count == 2)
                        _AlignPositionY.Add((_LeadCaliper.Results[0].PositionY < _LeadCaliper.Results[1].PositionY) ? _LeadCaliper.Results[0].PositionY : _LeadCaliper.Results[1].PositionY);

                    //CogSerializer.SaveObjectToFile(_LeadCaliper, string.Format(@"D:\GuideCaliper{0}.vpp", iLoopCount + 1));
                }
                #endregion

                #region Blob Tool Setting => X좌표 위치 정보 찾기
                for (int iLoopCount = 0; iLoopCount < _GuidePositionX.Count; ++iLoopCount)
                {
                    SetHardFixedThreshold(_CogLeadFormAlgo.AlignThreshold);
                    SetConnectivityMinimum(500);
                    SetPolarity(true);

                    CogRectangle _BlobRegion = new CogRectangle();
                    _BlobRegion.SetCenterWidthHeight(_GuidePositionX[iLoopCount], _AlignPositionY[iLoopCount] + 18, 170, 36);
                    _BlobResults = BlobProc.Execute(_SrcImage, _BlobRegion);

                    if (_BlobResults.GetBlobs().Count == 0)
                    {
                        LeadFormResult.IsGood = false;
                        LeadFormResult.NgType = eNgType.LEAD_CNT;
                        LeadFormResult.SearchArea.SetCenterWidthHeight(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height);
                        LeadFormResult.LeadCountStatus = "NG";
                        return false;
                    }

                    _AlignPositionX.Add(GetCenterXResult(_BlobResults));
                }
                #endregion

                #region Align XY 좌표 ADD
                if (_CogLeadFormAlgo.LeadCount == _GuidePositionX.Count)
                {
                    LeadFormResult.AlignResultDataList.Clear();
                    for (int iLoopCount = 0; iLoopCount < _GuidePositionX.Count; ++iLoopCount)
                    {
                        LeadFormAlignResultData _AlignResult = new LeadFormAlignResultData();
                        _AlignResult.CenterX = _AlignPositionX[iLoopCount];
                        _AlignResult.CenterY = _AlignPositionY[iLoopCount];
                        _AlignResult.Width = 40;
                        _AlignResult.Height = 40;
                        _AlignResult.IsGood = true;
                        LeadFormResult.AlignResultDataList.Add(_AlignResult);
                    }
                }

                else
                {
                    LeadFormResult.IsGood = false;
                    LeadFormResult.NgType = eNgType.LEAD_CNT;
                    LeadFormResult.LeadCountStatus = _GuidePositionX.Count.ToString();
                    LeadFormResult.SearchArea.SetCenterWidthHeight(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height);
                    _Result = false;
                }
                #endregion 

                //Lead 간격 조건 검사
                LeadFormResult.AlignOffsetDataList.Clear();
                PointD _AlignOffset = new PointD();
                if (_CogLeadFormAlgo.LeadCount == LeadFormResult.LeadCount)
                {
                    for (int iLoopCount = 0; iLoopCount < _CogLeadFormAlgo.LeadCount; ++iLoopCount)
                    {
                        double _RealCenterX = LeadFormResult.AlignResultDataList[iLoopCount].CenterX * _CogLeadFormAlgo.ResolutionX;
                        double _RealCenterY = LeadFormResult.AlignResultDataList[iLoopCount].CenterY * _CogLeadFormAlgo.ResolutionY;

                        #region X 축 Skew 확인
                        //Align Pitch Spec에서 완전히 벗어났는지 확인
                        if (_RealCenterX > _CogLeadFormAlgo.AlignPositionArray[iLoopCount].X - _CogLeadFormAlgo.AlignPitchSpec &&
                            _RealCenterX < _CogLeadFormAlgo.AlignPositionArray[iLoopCount].X + _CogLeadFormAlgo.AlignPitchSpec)
                        {
                            //Align Skew 가능 범위에 들어와 있는지 확인
                            //Skew 범위 안쪽이면 Skew 여부에 상관없이 GOOD
                            if (_RealCenterX > _CogLeadFormAlgo.AlignPositionArray[iLoopCount].X - _CogLeadFormAlgo.AlignSkewSpec &&
                                _RealCenterX < _CogLeadFormAlgo.AlignPositionArray[iLoopCount].X + _CogLeadFormAlgo.AlignSkewSpec)
                            {
                                if (LeadFormResult.NgType == eNgType.GOOD)
                                    LeadFormResult.AlignResultDataList[iLoopCount].IsGood = true;
                                LeadFormResult.EachLeadStatusArray[iLoopCount].SetSkewResult(eLeadStatus.GOOD);
                            }

                            //Skew 범위 < Position < Pitch Err 범위
                            //불량 판정 & Skew 가능 에러로 전달
                            else
                            {
                                LeadFormResult.AlignResultDataList[iLoopCount].IsGood = false;
                                LeadFormResult.IsGood = false;

                                LeadFormResult.EachLeadStatusArray[iLoopCount].SetSkewResult(eLeadStatus.LEAD_SKEW_ENABLE);
                                _Result = false;
                            }
                        }

                        //완전히 벗어나면  Skew 불가능 에러
                        else
                        {
                            LeadFormResult.AlignResultDataList[iLoopCount].IsGood = false;
                            LeadFormResult.IsGood = false;

                            LeadFormResult.EachLeadStatusArray[iLoopCount].SetSkewResult(eLeadStatus.LEAD_SKEW_DISABLE);
                            _Result = false;
                        }
                        #endregion

                        #region Y 축 Skew 확인
                        //Align Pitch Spec에서 완전히 벗어났는지 확인
                        if (_RealCenterY > _CogLeadFormAlgo.AlignPositionArray[iLoopCount].Y - _CogLeadFormAlgo.AlignPitchSpec &&
                            _RealCenterY < _CogLeadFormAlgo.AlignPositionArray[iLoopCount].Y + _CogLeadFormAlgo.AlignPitchSpec)
                        {
                            //Align Skew 가능 범위에 들어와 있는지 확인
                            //Skew 범위 안쪽이면 Skew 여부에 상관없이 GOOD
                            if (_RealCenterY > _CogLeadFormAlgo.AlignPositionArray[iLoopCount].Y - _CogLeadFormAlgo.AlignSkewSpec &&
                                _RealCenterY < _CogLeadFormAlgo.AlignPositionArray[iLoopCount].Y + _CogLeadFormAlgo.AlignSkewSpec)
                            {
                                //if (LeadFormResult.NgType == eNgType.GOOD)
                                //    LeadFormResult.AlignResultDataList[iLoopCount].IsGood = true;
                                LeadFormResult.EachLeadStatusArray[iLoopCount].SetSkewResult(eLeadStatus.GOOD);
                            }

                            //Skew 범위 < Position < Pitch Err 범위
                            //불량 판정 & Skew 가능 에러로 전달
                            else
                            {
                                LeadFormResult.AlignResultDataList[iLoopCount].IsGood = false;
                                LeadFormResult.IsGood = false;

                                LeadFormResult.EachLeadStatusArray[iLoopCount].SetSkewResult(eLeadStatus.LEAD_SKEW_ENABLE);
                                _Result = false;
                            }
                        }

                        else
                        {
                            LeadFormResult.AlignResultDataList[iLoopCount].IsGood = false;
                            LeadFormResult.IsGood = false;

                            LeadFormResult.EachLeadStatusArray[iLoopCount].SetSkewResult(eLeadStatus.LEAD_SKEW_DISABLE);
                            _Result = false;
                        }
                        #endregion

                        _AlignOffset.X = _CogLeadFormAlgo.AlignPositionArray[iLoopCount].X - _RealCenterX;
                        _AlignOffset.Y = _CogLeadFormAlgo.AlignPositionArray[iLoopCount].Y - _RealCenterY;
                        LeadFormResult.AlignOffsetDataList.Add(_AlignOffset);

                        LeadFormResult.EachLeadStatusArray[iLoopCount].SideX = _AlignOffset.X.ToString();
                        LeadFormResult.EachLeadStatusArray[iLoopCount].SideY = _AlignOffset.Y.ToString();
                    }
                }

                else
                {
                    LeadFormResult.IsGood = false;
                    LeadFormResult.NgType = eNgType.LEAD_CNT;
                    LeadFormResult.LeadCountStatus = LeadFormResult.LeadCount.ToString();
                    LeadFormResult.SearchArea.SetCenterWidthHeight(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height);
                    _Result = false;
                }

                _ProcessWatch.Stop();
                string _ProcessTime = String.Format("LeadAlign Time : {0} ms", _ProcessWatch.Elapsed.TotalSeconds.ToString());
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, _ProcessTime, CLogManager.LOG_LEVEL.LOW);
                
            }

            catch (Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "LeadAlign - Inspection Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
                LeadFormResult.NgType = eNgType.EMPTY;
                LeadFormResult.IsGood = false;
                LeadFormResult.LeadBodyStatus = "NG";
                LeadFormResult.SearchArea.SetCenterWidthHeight(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height);
                _Result = false;
            }

            return _Result; 
        }

        public bool LeadAlign(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadFormAlgo _CogLeadFormAlgo)
        {
            if (LeadFormResult.IsGood != true) return true;

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadAlign - Start", CLogManager.LOG_LEVEL.MID);

            if (false == _CogLeadFormAlgo.IsUseAlign)
            {
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadAlign Disable - End", CLogManager.LOG_LEVEL.MID);
                return true;
            }

            bool _Result = true;

            try
            {
                System.Diagnostics.Stopwatch _ProcessWatch = new System.Diagnostics.Stopwatch();
                _ProcessWatch.Reset(); _ProcessWatch.Start();

                SetHardFixedThreshold(_CogLeadFormAlgo.AlignThreshold);
                SetConnectivityMinimum(500);
                SetPolarity(true);

                CogBlobResults _BlobResults = BlobProc.Execute(_SrcImage, _InspRegion);
                LeadFormResult.AlignResultDataList = GetAlignResult(_BlobResults);
                LeadFormResult.LeadCount = LeadFormResult.AlignResultDataList.Count;

                //Lead 간격 조건 검사
                LeadFormResult.AlignOffsetDataList.Clear();
                PointD _AlignOffset = new PointD();
                if (_CogLeadFormAlgo.LeadCount == LeadFormResult.LeadCount)
                {
                    for (int iLoopCount = 0; iLoopCount < _CogLeadFormAlgo.LeadCount; ++iLoopCount)
                    {
                        double _RealCenterX = LeadFormResult.AlignResultDataList[iLoopCount].CenterX * _CogLeadFormAlgo.ResolutionX;
                        double _RealCenterY = LeadFormResult.AlignResultDataList[iLoopCount].CenterY * _CogLeadFormAlgo.ResolutionY;

                        if (_RealCenterX > _CogLeadFormAlgo.AlignPositionArray[iLoopCount].X - _CogLeadFormAlgo.AlignPitchSpec && 
                            _RealCenterX < _CogLeadFormAlgo.AlignPositionArray[iLoopCount].X + _CogLeadFormAlgo.AlignPitchSpec)
                        {
                            //if (LeadFormResult.NgType == eNgType.GOOD)
                            LeadFormResult.AlignResultDataList[iLoopCount].IsGood = true;
                        }

                        else
                        {
                            LeadFormResult.AlignResultDataList[iLoopCount].IsGood = false;
                            LeadFormResult.IsGood = false;
                            _Result = false;
                        }


                        if (_RealCenterY > _CogLeadFormAlgo.AlignPositionArray[iLoopCount].Y - _CogLeadFormAlgo.AlignPitchSpec &&
                            _RealCenterY < _CogLeadFormAlgo.AlignPositionArray[iLoopCount].Y + _CogLeadFormAlgo.AlignPitchSpec)
                        {
                            if (LeadFormResult.NgType == eNgType.GOOD)
                                LeadFormResult.AlignResultDataList[iLoopCount].IsGood = true;
                        }

                        else
                        {
                            LeadFormResult.AlignResultDataList[iLoopCount].IsGood = false;
                            LeadFormResult.IsGood = false;
                            _Result = false;
                        }

                        _AlignOffset.X = _CogLeadFormAlgo.AlignPositionArray[iLoopCount].X - _RealCenterX;
                        _AlignOffset.Y = _CogLeadFormAlgo.AlignPositionArray[iLoopCount].Y - _RealCenterY;
                        LeadFormResult.AlignOffsetDataList.Add(_AlignOffset);
                    }
                }

                else
                {
                    LeadFormResult.IsGood = false;
                    LeadFormResult.NgType = eNgType.LEAD_CNT;
                    LeadFormResult.SearchArea.SetCenterWidthHeight(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height);
                    _Result = false;
                }

                _ProcessWatch.Stop();
                string _ProcessTime = String.Format("LeadAlign Time : {0} ms", _ProcessWatch.Elapsed.TotalSeconds.ToString());
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, _ProcessTime, CLogManager.LOG_LEVEL.LOW);
            }

            catch (Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "LeadAlign - Inspection Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
                LeadFormResult.NgType = eNgType.EMPTY;
                LeadFormResult.IsGood = false;
                LeadFormResult.SearchArea.SetCenterWidthHeight(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height);
                _Result = false;
            }

            return _Result;
        }

        public CogLeadFormResult GetLeadFormResult()
        {
            return LeadFormResult;
        }

        public List<double> GetLeadGuidePosition(CogBlobResults _BlobResults)
        {
            List<double> _GuidePositionList = new List<double>();

            if (null == _BlobResults || _BlobResults.GetBlobs().Count <= 0) return _GuidePositionList;

            for (int iLoopCount = 0; iLoopCount < _BlobResults.GetBlobs().Count; ++iLoopCount)
            {
                double _CenterX = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeCenterX, iLoopCount);

                bool _IsNew = true;
                for (int jLoopCount = 0; jLoopCount < _GuidePositionList.Count; ++jLoopCount)
                {
                    if (_CenterX > _GuidePositionList[jLoopCount] - 100 && _CenterX < _GuidePositionList[jLoopCount] + 100)
                    {
                        _IsNew = false;
                        break;
                    }
                }

                if (true == _IsNew)
                {
                    double _GuidePosTemp;
                    _GuidePosTemp = _CenterX;
                    _GuidePositionList.Add(_GuidePosTemp);
                }
            }

            _GuidePositionList.Sort();

            return _GuidePositionList;
        }

        public double GetCenterXResult(CogBlobResults _BlobResults)
        {
            double _CenterX = 0;
            double _MaxArea = 0;

            if (null == _BlobResults || _BlobResults.GetBlobs().Count <= 0) return _CenterX;

            for (int iLoopCount = 0; iLoopCount < _BlobResults.GetBlobs().Count; ++iLoopCount)
            {
                if (_MaxArea < _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Area, iLoopCount))
                {
                    _MaxArea = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Area, iLoopCount);
                    _CenterX = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeCenterX, iLoopCount);
                }
            }

            return _CenterX;
        }

        public List<LeadFormAlignResultData> GetAlignResult(CogBlobResults _BlobResults)//, int _LeadCount)
        {
            List<LeadFormAlignResultData> _AlignResultDataList = new List<LeadFormAlignResultData>();
            
            if (null == _BlobResults || _BlobResults.GetBlobs().Count <= 0) return _AlignResultDataList;

            for (int iLoopCount = 0; iLoopCount < _BlobResults.GetBlobs().Count; ++iLoopCount)
            {
                double _BlobMinX = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinX, iLoopCount);
                double _BlobMaxX = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxX, iLoopCount);
                double _BlobMinY = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinY, iLoopCount);
                double _BlobMaxY = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxY, iLoopCount);
                double _Width = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeWidth, iLoopCount);
                double _Height = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeHeight, iLoopCount);

                //사이즈가 너무 큰 경우 걸러내기
                if ((_Width < 65 && _Height < 65) && (_Width > 35 && _Height > 35))
                {
                    LeadFormAlignResultData _AlignResultDataTemp = new LeadFormAlignResultData();
                    _AlignResultDataTemp.Area = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Area, iLoopCount);
                    _AlignResultDataTemp.Width = _Width;
                    _AlignResultDataTemp.Height = _Height;
                    _AlignResultDataTemp.Angle = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Angle, iLoopCount);
                    _AlignResultDataTemp.CenterX = (_BlobMaxX + _BlobMinX) / 2;
                    _AlignResultDataTemp.CenterY = (_BlobMaxY + _BlobMinY) / 2;

                    _AlignResultDataList.Add(_AlignResultDataTemp);
                }
            }

            //Y축 정렬
            _AlignResultDataList.Sort(delegate (LeadFormAlignResultData _A, LeadFormAlignResultData _B)
            {
                if (_A.CenterY > _B.CenterY)        return 1;
                else if (_A.CenterY < _B.CenterY)   return -1;
                return 0;
            });

            //Y축 정렬 후 Lead 갯수까지만 자르기
            //if (_AlignResultDataList.Count > _LeadCount)
            //{
            //    for (int iLoopCount = _AlignResultDataList.Count - 1; iLoopCount > _LeadCount - 1; --iLoopCount)
            //        _AlignResultDataList.RemoveAt(iLoopCount);
            //}

            //X축으로 재 정렬
            _AlignResultDataList.Sort(delegate (LeadFormAlignResultData _A, LeadFormAlignResultData _B)
            {
                if (_A.CenterX> _B.CenterX) return 1;
                else if (_A.CenterX < _B.CenterX) return -1;
                return 0;
            });

            return _AlignResultDataList;
        }

        #region Blob Condition Setting
        private void SetHardFixedThreshold(int _ThresholdValue)
        {
            BlobProc.SegmentationParams.Mode = CogBlobSegmentationModeConstants.HardFixedThreshold;
            BlobProc.SegmentationParams.HardFixedThreshold = _ThresholdValue;
        }

        private void SetConnectivityMinimum(int _MinValue)
        {
            BlobProc.ConnectivityMinPixels = _MinValue;
        }

        private void SetPolarity(bool _IsForegroundLight)
        {
            if (_IsForegroundLight) BlobProc.SegmentationParams.Polarity = CogBlobSegmentationPolarityConstants.LightBlobs;
            else BlobProc.SegmentationParams.Polarity = CogBlobSegmentationPolarityConstants.DarkBlobs;
        }
        private void SetMorphology(CogBlobMorphologyConstants _Mode)
        {
            BlobProc.MorphologyOperations.Add(_Mode);
        }

        private void SetMeasurement(CogBlobMeasureConstants _Properties, CogBlobMeasureModeConstants _Filter, CogBlobFilterModeConstants _Range, double _RangeLow, double _RangeHigh, bool _IsNew = true)
        {
            if (_IsNew) BlobProc.RunTimeMeasures.Clear();

            CogBlobMeasure _BlobMeasure = new CogBlobMeasure();
            _BlobMeasure.Measure = _Properties;
            _BlobMeasure.Mode = _Filter;
            _BlobMeasure.FilterMode = _Range;
            _BlobMeasure.FilterRangeLow = _RangeLow;
            _BlobMeasure.FilterRangeHigh = _RangeHigh;
            BlobProc.RunTimeMeasures.Add(_BlobMeasure);
        }
        #endregion
    }
}
