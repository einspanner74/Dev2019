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
    class InspectionLeadTrim
    {
        private CogLeadTrimResult LeadTrimResult = new CogLeadTrimResult();

        private CogImage8Grey SrcImage = new CogImage8Grey();

        private CogBlob                 BlobProc;
        private CogBlobResults          BlobResults;
        private CogBlobResult           BlobResult;
        //private CogBlobReferenceResult  InspBlobReferResults;
        private CogLeadTrimResult       InspLeadTrimResults;

        #region Initialize & Deinitialize
        public InspectionLeadTrim()
        {
            BlobProc = new CogBlob();
            BlobResults = new CogBlobResults();
            BlobResult = new CogBlobResult();
            //InspBlobReferResults = new CogBlobReferenceResult();
            InspLeadTrimResults = new CogLeadTrimResult();

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

        public bool Run(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo, ref CogLeadTrimResult _CogLeadTrimResult, int _NgNumber = 0)
        {
            bool _Result = false;

            CogRectangle _LeadBodyArea = new CogRectangle();
            CogRectangle _MoldChipArea = new CogRectangle();
            CogRectangle _LeadLengthArea = new CogRectangle();
            CogRectangle _ShoulderArea = new CogRectangle();
            CogRectangle _LeadTipArea = new CogRectangle();
            CogRectangle _GateArea = new CogRectangle();

            _LeadBodyArea.SetCenterWidthHeight(_CogLeadTrimAlgo.BodyArea.CenterX, _CogLeadTrimAlgo.BodyArea.CenterY, _CogLeadTrimAlgo.BodyArea.Width, _CogLeadTrimAlgo.BodyArea.Height);
            _MoldChipArea.SetCenterWidthHeight(_CogLeadTrimAlgo.ChipOutArea.CenterX, _CogLeadTrimAlgo.ChipOutArea.CenterY, _CogLeadTrimAlgo.ChipOutArea.Width, _CogLeadTrimAlgo.ChipOutArea.Height);
            _LeadLengthArea.SetCenterWidthHeight(_CogLeadTrimAlgo.LeadMeasurementArea.CenterX, _CogLeadTrimAlgo.LeadMeasurementArea.CenterY, _CogLeadTrimAlgo.LeadMeasurementArea.Width, _CogLeadTrimAlgo.LeadMeasurementArea.Height);
            _ShoulderArea.SetCenterWidthHeight(_CogLeadTrimAlgo.ShoulderInspArea.CenterX, _CogLeadTrimAlgo.ShoulderInspArea.CenterY, _CogLeadTrimAlgo.ShoulderInspArea.Width, _CogLeadTrimAlgo.ShoulderInspArea.Height);
            _LeadTipArea.SetCenterWidthHeight(_CogLeadTrimAlgo.LeadTipInspArea.CenterX, _CogLeadTrimAlgo.LeadTipInspArea.CenterY, _CogLeadTrimAlgo.LeadTipInspArea.Width, _CogLeadTrimAlgo.LeadTipInspArea.Height);
            _GateArea.SetCenterWidthHeight(_CogLeadTrimAlgo.GateRemainingArea.CenterX, _CogLeadTrimAlgo.GateRemainingArea.CenterY, _CogLeadTrimAlgo.GateRemainingArea.Width, _CogLeadTrimAlgo.GateRemainingArea.Height);

            do
            {
                //Lead body search
                if (false == LeadBodySearch(_SrcImage, _LeadBodyArea, _CogLeadTrimAlgo))        break;
                if (false == MoldChipOutInspection(_SrcImage, _MoldChipArea, _CogLeadTrimAlgo)) break;
                if (false == LeadMeasurement(_SrcImage, _LeadLengthArea, _CogLeadTrimAlgo))     break;
                if (false == ShoulderInspection(_SrcImage, _ShoulderArea, _CogLeadTrimAlgo))    break;
                if (false == LeadTipInspection(_SrcImage, _LeadTipArea, _CogLeadTrimAlgo))      break;
                if (false == GateReminingInspection(_SrcImage, _GateArea, _CogLeadTrimAlgo))    break;

                _Result = true;
            } while (false);

            _CogLeadTrimResult = LeadTrimResult;

            return _Result;
        }

        public bool LeadBodySearch(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo)
        {
            LeadTrimResult.IsGood = true;

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadBodySearch - Start", CLogManager.LOG_LEVEL.MID);

            if (false == _CogLeadTrimAlgo.IsUseLeadBody)
            {
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadBodySearch Disable - End", CLogManager.LOG_LEVEL.MID);
                return true;
            }

            bool _Result = true;
            
            try
            {
                int _Index = 0;
                double _BlobAreaMaxmize = 0;

                SetHardFixedThreshold(100);
                SetConnectivityMinimum(100000);
                SetPolarity(false);

                System.Diagnostics.Stopwatch _ProcessWatch = new System.Diagnostics.Stopwatch();
                _ProcessWatch.Reset(); _ProcessWatch.Start();

                #region Step1. Lead 전체의 Center를 구해서 masking 위치를 따라가게 한다.
                //Step1. Lead 전체의 Center를 구해서 masking 위치를 따라가게 한다.
                BlobResults = BlobProc.Execute(_SrcImage, _InspRegion);
                //CogLeadTrimShoulderResult _CogShoulderResult = GetShoulderResult(BlobResults);
                GetResult(true);

                CogLeadTrimResult _CogBlobReferResultTemp = new CogLeadTrimResult();
                _CogBlobReferResultTemp = GetResults();
                
                if (_CogBlobReferResultTemp.BlobCount <= 0)
                {
                    //Blob이 없는 경우 제품 자체가 없는걸로 인식
                    LeadTrimResult.NgType = eNgType.EMPTY;
                    LeadTrimResult.IsGood = false;
                    return false;
                }

                for (int iLoopCount = 0; iLoopCount < _CogBlobReferResultTemp.BlobCount; ++iLoopCount)
                {
                    if (_CogBlobReferResultTemp.BlobArea[iLoopCount] > _BlobAreaMaxmize)
                    {
                        _BlobAreaMaxmize = _CogBlobReferResultTemp.BlobArea[iLoopCount];
                        _Index = iLoopCount;
                    }
                }

                if (_CogLeadTrimAlgo.BodyCenterOrigin.X == 0 && _CogLeadTrimAlgo.BodyCenterOrigin.Y == 0)
                {
                    _CogLeadTrimAlgo.BodyCenterOrigin.X = _CogBlobReferResultTemp.BlobCenterX[_Index];
                    _CogLeadTrimAlgo.BodyCenterOrigin.Y = _CogBlobReferResultTemp.BlobCenterY[_Index];
                }

                LeadTrimResult.LeadBodyOffsetX = _CogBlobReferResultTemp.BlobCenterX[_Index] - _CogLeadTrimAlgo.BodyCenterOrigin.X;
                LeadTrimResult.LeadBodyOffsetY = _CogBlobReferResultTemp.BlobCenterY[_Index] - _CogLeadTrimAlgo.BodyCenterOrigin.Y;
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadBodySearch - Lead Center XY Find End", CLogManager.LOG_LEVEL.MID);
                #endregion

                #region Step2. Masking 처리
                //Step2. Masking 처리
                #region Masking 처리 방법1 주석
                /*
                //Masking 처리 방법1
                CogImage8Grey _CogCopyImage = new CogImage8Grey(_SrcImage);
                for(int iLoopCount = 0; iLoopCount < _CogLeadTrimAlgo.BodyMaskingAreaList.Count; ++iLoopCount)
                {
                    int _StartX = (int)(_CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].CenterX - _CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].Width / 2);
                    int _StartY = (int)(_CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].CenterY - _CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].Height / 2);
                    int _Width = (int)(_CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].Width);
                    int _Height = (int)(_CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].Height);

                    for (int jLoopCount = 0; jLoopCount < _CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].Width; ++jLoopCount)
                    {
                        for (int zLoopCount = 0; zLoopCount < _CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].Height; ++zLoopCount)
                        {
                            _CogCopyImage.SetPixel(_StartX + jLoopCount, _StartY + zLoopCount, 255);
                        }
                    }

                    //Parallel.For(0, _Width, (jLoopCount) =>
                    //{
                    //    Parallel.For(0, _Height, (zLoopCount) =>
                    //    {
                    //        _CogCopyImage.SetPixel(_StartX + jLoopCount, _StartY + zLoopCount, 255);
                    //    });
                    //});
                }
                */
                #endregion

                CogImage8Grey _CogCopyImage = new CogImage8Grey(_SrcImage);
                CogCopyRegionTool _CogCopyRegionTool = new CogCopyRegionTool();
                _CogCopyRegionTool.RunParams.FillRegion = true;
                _CogCopyRegionTool.RunParams.FillRegionValue = 255;
                _CogCopyRegionTool.RunParams.ImageAlignmentEnabled = true;
                _CogCopyRegionTool.RunParams.InputImageAlignmentX = _CogCopyImage.Width / 2;
                _CogCopyRegionTool.RunParams.InputImageAlignmentY = _CogCopyImage.Height / 2;
                _CogCopyRegionTool.RunParams.DestinationImageAlignmentX = _CogCopyImage.Width / 2;
                _CogCopyRegionTool.RunParams.DestinationImageAlignmentY = _CogCopyImage.Height / 2;

                for (int iLoopCount = 0; iLoopCount < _CogLeadTrimAlgo.BodyMaskingAreaList.Count; ++iLoopCount)
                {
                    CogRectangle _CogRegion = new CogRectangle();
                    _CogRegion.SetCenterWidthHeight(_CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].CenterX + LeadTrimResult.LeadBodyOffsetX, 
                                                    _CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].CenterY + LeadTrimResult.LeadBodyOffsetY,
                                                    _CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].Width, _CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].Height);

                    _CogCopyRegionTool.InputImage = _CogCopyImage;
                    _CogCopyRegionTool.DestinationImage = _CogCopyImage;
                    _CogCopyRegionTool.Region = _CogRegion;
                    _CogCopyRegionTool.Run();

                    _CogCopyImage = (CogImage8Grey)_CogCopyRegionTool.OutputImage;
                }
                #endregion

                #region Step3. Lead Pure Body Find
                //Step3. Lead Pure Body Find
                BlobResults = BlobProc.Execute(_CogCopyImage, _InspRegion);
                GetResult(true);

                _CogBlobReferResultTemp = new CogLeadTrimResult();
                _CogBlobReferResultTemp = GetResults();

                if (_CogBlobReferResultTemp.BlobCount <= 0)
                {
                    //Blob이 없는 경우 제품 자체가 없는걸로 인식
                    LeadTrimResult.NgType = eNgType.EMPTY;
                    LeadTrimResult.IsGood = false;
                    return false;
                }

                _BlobAreaMaxmize = 0;
                for (int iLoopCount = 0; iLoopCount < _CogBlobReferResultTemp.BlobCount; ++iLoopCount)
                {
                    if (_CogBlobReferResultTemp.BlobArea[iLoopCount] > _BlobAreaMaxmize)
                    {
                        _BlobAreaMaxmize = _CogBlobReferResultTemp.BlobArea[iLoopCount];
                        _Index = iLoopCount;
                    }
                }

                LeadTrimResult.LeadBodyLeftTop.X = _CogBlobReferResultTemp.BlobMinX[_Index];
                LeadTrimResult.LeadBodyLeftTop.Y = _CogBlobReferResultTemp.BlobMinY[_Index];
                LeadTrimResult.LeadBodyRightTop.X = _CogBlobReferResultTemp.BlobMaxX[_Index];
                LeadTrimResult.LeadBodyRightTop.Y = _CogBlobReferResultTemp.BlobMinY[_Index];
                LeadTrimResult.LeadBodyLeftBottom.X = _CogBlobReferResultTemp.BlobMinX[_Index];
                LeadTrimResult.LeadBodyLeftBottom.Y = _CogBlobReferResultTemp.BlobMaxY[_Index];
                LeadTrimResult.LeadBodyRightBottom.X = _CogBlobReferResultTemp.BlobMaxX[_Index];
                LeadTrimResult.LeadBodyRightBottom.Y = _CogBlobReferResultTemp.BlobMaxY[_Index];

                CogLine _CogLine = new CogLine();
                _CogLine.SetFromStartXYEndXY(LeadTrimResult.LeadBodyLeftTop.X, LeadTrimResult.LeadBodyLeftTop.Y, LeadTrimResult.LeadBodyRightTop.X, LeadTrimResult.LeadBodyRightTop.Y);
                LeadTrimResult.LeadBodyBaseLine = _CogLine;
                #endregion

                _ProcessWatch.Stop();
                string _ProcessTime = String.Format("Lead Body Search Time : {0} ms", _ProcessWatch.Elapsed.TotalSeconds.ToString());
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, _ProcessTime, CLogManager.LOG_LEVEL.LOW);
                
                #region Masking Image Save
                //CogImageFile _CogImageFile = new CogImageFile();
                //_CogImageFile.Open(@"D:\Origin.jpg", CogImageFileModeConstants.Write);
                //_CogImageFile.Append(_SrcImage);
                //_CogImageFile.Close();
                //
                //_CogImageFile.Open(@"D:\Mask.jpg", CogImageFileModeConstants.Write);
                //_CogImageFile.Append(_CogCopyImage);
                //_CogImageFile.Close();
                #endregion
            }

            catch (Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "LeadBodySearch - Inspection Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
                LeadTrimResult.NgType = eNgType.EMPTY;
                LeadTrimResult.IsGood = false;
                _Result = false;
            }

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadBodySearch - End", CLogManager.LOG_LEVEL.MID);
            return _Result;
        }

        public bool MoldChipOutInspection(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo)
        {
            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "MoldChipOutInspection - Start", CLogManager.LOG_LEVEL.MID);

            if (false == _CogLeadTrimAlgo.IsUseMoldChipOut)
            {
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "MoldChipOutInspection Disable - End", CLogManager.LOG_LEVEL.MID);
                return true;
            }

            bool _Result = true;

            try
            {
                System.Diagnostics.Stopwatch _ProcessWatch = new System.Diagnostics.Stopwatch();
                _ProcessWatch.Reset(); _ProcessWatch.Start();

                #region Lead body의 홈 부분을 위한 마스킹 -> BodyMaskingArea를 사용한다. 반전해서 사용
                CogImage8Grey _CogCopyImage = new CogImage8Grey(_SrcImage);
                CogCopyRegionTool _CogCopyRegionTool = new CogCopyRegionTool();
                _CogCopyRegionTool.RunParams.FillRegion = true;
                _CogCopyRegionTool.RunParams.FillRegionValue = 0;
                _CogCopyRegionTool.RunParams.ImageAlignmentEnabled = true;
                _CogCopyRegionTool.RunParams.InputImageAlignmentX = _CogCopyImage.Width / 2;
                _CogCopyRegionTool.RunParams.InputImageAlignmentY = _CogCopyImage.Height / 2;
                _CogCopyRegionTool.RunParams.DestinationImageAlignmentX = _CogCopyImage.Width / 2;
                _CogCopyRegionTool.RunParams.DestinationImageAlignmentY = _CogCopyImage.Height / 2;
                
                for (int iLoopCount = 0; iLoopCount < _CogLeadTrimAlgo.BodyMaskingAreaList.Count; ++iLoopCount)
                {
                    CogRectangle _CogRegion = new CogRectangle();
                    _CogRegion.SetCenterWidthHeight(_CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].CenterX + LeadTrimResult.LeadBodyOffsetX,
                                                    _CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].CenterY + LeadTrimResult.LeadBodyOffsetY,
                                                    _CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].Width, _CogLeadTrimAlgo.BodyMaskingAreaList[iLoopCount].Height);
                
                    _CogCopyRegionTool.InputImage = _CogCopyImage;
                    _CogCopyRegionTool.DestinationImage = _CogCopyImage;
                    _CogCopyRegionTool.Region = _CogRegion;
                    _CogCopyRegionTool.Run();
                
                    _CogCopyImage = (CogImage8Grey)_CogCopyRegionTool.OutputImage;
                }

                #region Masking Image Save
                //CogImageFile _CogImageFile = new CogImageFile();
                //_CogImageFile.Open(@"D:\Origin.jpg", CogImageFileModeConstants.Write);
                //_CogImageFile.Append(_SrcImage);
                //_CogImageFile.Close();
                //
                //_CogImageFile.Open(@"D:\Mask.jpg", CogImageFileModeConstants.Write);
                //_CogImageFile.Append(_CogCopyImage);
                //_CogImageFile.Close();
                #endregion
                #endregion

                SetConnectivityMinimum(2000);
                SetHardFixedThreshold(_CogLeadTrimAlgo.ChipOutThreshold);
                SetPolarity(Convert.ToBoolean(_CogLeadTrimAlgo.ChipOutForeground));

                //BlobResults = BlobProc.Execute(_SrcImage, _InspRegion);
                BlobResults = BlobProc.Execute(_CogCopyImage, _InspRegion);
                GetResult(true);

                CogLeadTrimResult _CogChipOutResult = new CogLeadTrimResult();
                _CogChipOutResult = GetResults();

                if (null == LeadTrimResult.ChipOutNgList) LeadTrimResult.ChipOutNgList = new List<CogRectangle>();

                LeadTrimResult.ChipOutNgList.Clear();

                for (int iLoopCount = 0; iLoopCount < _CogChipOutResult.BlobCount; ++iLoopCount)
                {
                    double _Width = _CogChipOutResult.BlobMaxX[iLoopCount] - _CogChipOutResult.BlobMinX[iLoopCount];
                    double _Height = _CogChipOutResult.BlobMaxY[iLoopCount] - _CogChipOutResult.BlobMinY[iLoopCount];

                    if ((_CogLeadTrimAlgo.ChipOutWidthMax > _Width && _CogLeadTrimAlgo.ChipOutWidthMin < _Width) &&
                        (_CogLeadTrimAlgo.ChipOutHeightMax > _Height && _CogLeadTrimAlgo.ChipOutHeightMin < _Height))
                    {
                        CogRectangle _DefectArea = new CogRectangle();
                        _DefectArea.SetXYWidthHeight(_CogChipOutResult.BlobMinX[iLoopCount], _CogChipOutResult.BlobMinY[iLoopCount], _Width, _Height);
                        LeadTrimResult.ChipOutNgList.Add(_DefectArea);

                        LeadTrimResult.NgType = eNgType.CHIP_OUT;
                        LeadTrimResult.IsGood = false;
                        _Result = false;    
                    }
                }

                _ProcessWatch.Stop();
                string _ProcessTime = String.Format("MoldChipOutInspection Time : {0} ms", _ProcessWatch.Elapsed.TotalSeconds.ToString());
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, _ProcessTime, CLogManager.LOG_LEVEL.LOW);
            }

            catch (Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "MoldChipOutInspection - Inspection Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
                LeadTrimResult.NgType = eNgType.CHIP_OUT;
                LeadTrimResult.IsGood = false;
                _Result = false;
            }

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "MoldChipOutInspection - End", CLogManager.LOG_LEVEL.MID);
            return _Result;
        }

        public bool LeadMeasurement(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo)
        {
            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadMeasurement - Start", CLogManager.LOG_LEVEL.MID);

            if (false == _CogLeadTrimAlgo.IsUseLeadMeasurement)
            {
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadMeasurement Disable - End", CLogManager.LOG_LEVEL.MID);
                return true;
            }

            bool _Result = true;

            try
            {
                System.Diagnostics.Stopwatch _ProcessWatch = new System.Diagnostics.Stopwatch();
                _ProcessWatch.Reset(); _ProcessWatch.Start();


                SetConnectivityMinimum(10000);
                SetHardFixedThreshold(_CogLeadTrimAlgo.LeadThreshold);
                SetPolarity(Convert.ToBoolean(_CogLeadTrimAlgo.LeadForeground));

                BlobResults = BlobProc.Execute(_SrcImage, _InspRegion);
                GetResult(true);

                CogLeadTrimResult _CogLeadTrimResult = new CogLeadTrimResult();
                _CogLeadTrimResult = GetResults();

                if (null == LeadTrimResult.LeadMeasureList) LeadTrimResult.LeadMeasureList = new List<CogRectangle>();
                LeadTrimResult.LeadMeasureList.Clear();

                #region Lead Pitch Point Get
                LeadTrimResult.LeadCount = _CogLeadTrimResult.BlobCount;
                LeadTrimResult.LeadCenterX = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadCenterY = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadWidth = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadLength = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadPitchLength = new double[_CogLeadTrimResult.BlobCount - 1];
                LeadTrimResult.LeadPitchTopX = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadPitchTopY = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadPitchBottomX = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadPitchBottomY = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.Angle = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadLengthResult = new bool[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.IsLeadLengthGood = new bool[_CogLeadTrimResult.BlobCount];

                for (int iLoopCount = 0; iLoopCount < _CogLeadTrimResult.BlobCount; ++iLoopCount)
                {
                    #region Lead Bent Check
                    //double _Angle = _CogLeadMeasureResult.Angle[iLoopCount] * 180 / Math.PI;
                    //if (_Angle > 0) _Angle = 90 - (_CogLeadMeasureResult.Angle[iLoopCount] * 180 / Math.PI);
                    //else _Angle = -(90 + (_CogLeadMeasureResult.Angle[iLoopCount] * 180 / Math.PI));
                    //
                    //_CogLeadMeasureResult.IsLeadBentGood[iLoopCount] = true;
                    ////if ((_Angle > _CogLeadTrimAlgo.LeadBentMax) || (_Angle < -_CogLeadTrimAlgo.LeadBentMin))
                    //{
                    //    _CogLeadMeasureResult.IsLeadBentGood[iLoopCount] = false;
                    //    _CogLeadMeasureResult.IsGood &= _CogLeadMeasureResult.IsLeadBentGood[iLoopCount];
                    //}
                    #endregion

                    #region Pitch Point 구하기
                    CogLineSegment _CenterLine = new CogLineSegment();
                    if (_CogLeadTrimResult.Angle[iLoopCount] > 0)
                    {
                        _CenterLine.SetStartLengthRotation(_CogLeadTrimResult.BlobCenterX[iLoopCount], _CogLeadTrimResult.BlobCenterY[iLoopCount], _CogLeadTrimResult.PrincipalWidth[iLoopCount] / 2, (Math.PI) + _CogLeadTrimResult.Angle[iLoopCount]);
                        LeadTrimResult.LeadPitchTopX[iLoopCount] = _CenterLine.EndX;
                        LeadTrimResult.LeadPitchTopY[iLoopCount] = _CenterLine.EndY;

                        _CenterLine.SetStartLengthRotation(_CogLeadTrimResult.BlobCenterX[iLoopCount], _CogLeadTrimResult.BlobCenterY[iLoopCount], _CogLeadTrimResult.PrincipalWidth[iLoopCount] / 2, _CogLeadTrimResult.Angle[iLoopCount]);
                        LeadTrimResult.LeadPitchBottomX[iLoopCount] = _CenterLine.EndX;
                        LeadTrimResult.LeadPitchBottomY[iLoopCount] = _CenterLine.EndY;
                    }

                    else
                    {
                        _CenterLine.SetStartLengthRotation(_CogLeadTrimResult.BlobCenterX[iLoopCount], _CogLeadTrimResult.BlobCenterY[iLoopCount], _CogLeadTrimResult.PrincipalWidth[iLoopCount] / 2, _CogLeadTrimResult.Angle[iLoopCount]);
                        LeadTrimResult.LeadPitchTopX[iLoopCount] = _CenterLine.EndX;
                        LeadTrimResult.LeadPitchTopY[iLoopCount] = _CenterLine.EndY;

                        _CenterLine.SetStartLengthRotation(_CogLeadTrimResult.BlobCenterX[iLoopCount], _CogLeadTrimResult.BlobCenterY[iLoopCount], _CogLeadTrimResult.PrincipalWidth[iLoopCount] / 2, (Math.PI) + _CogLeadTrimResult.Angle[iLoopCount]);
                        LeadTrimResult.LeadPitchBottomX[iLoopCount] = _CenterLine.EndX;
                        LeadTrimResult.LeadPitchBottomY[iLoopCount] = _CenterLine.EndY;
                    }
                    LeadTrimResult.Angle[iLoopCount] = _CogLeadTrimResult.Angle[iLoopCount];
                    LeadTrimResult.LeadCenterX[iLoopCount] = _CogLeadTrimResult.BlobCenterX[iLoopCount];
                    LeadTrimResult.LeadCenterY[iLoopCount] = _CogLeadTrimResult.BlobCenterY[iLoopCount];
                    //LeadTrimResult.LeadWidth[iLoopCount] = _CogLeadTrimResult.PrincipalWidth[iLoopCount];
                    //LeadTrimResult.LeadLength[iLoopCount] = _CogLeadTrimResult.PrincipalHeight[iLoopCount];
                    LeadTrimResult.LeadWidth[iLoopCount] = _CogLeadTrimResult.PrincipalHeight[iLoopCount] * _CogLeadTrimAlgo.ResolutionX;
                    LeadTrimResult.LeadLength[iLoopCount] = _CogLeadTrimResult.PrincipalWidth[iLoopCount] * _CogLeadTrimAlgo.ResolutionY;
                    #endregion

                    #region Length 구하기
                    CogLine _CogLeadLine = new CogLine();
                    _CogLeadLine.SetFromStartXYEndXY(LeadTrimResult.LeadPitchTopX[iLoopCount], LeadTrimResult.LeadPitchTopX[iLoopCount], LeadTrimResult.LeadPitchBottomX[iLoopCount], LeadTrimResult.LeadPitchBottomY[iLoopCount]);

                    CogIntersectLineLineTool _CogIntersect = new CogIntersectLineLineTool();
                    _CogIntersect.InputImage = _SrcImage;
                    _CogIntersect.LineA = LeadTrimResult.LeadBodyBaseLine;
                    _CogIntersect.LineB = _CogLeadLine;
                    _CogIntersect.Run();

                    LeadTrimResult.LeadPitchBottomX[iLoopCount] = _CogIntersect.X;
                    LeadTrimResult.LeadPitchBottomY[iLoopCount] = _CogIntersect.Y;

                    double _Angle;
                    double _Length = CogMath.DistancePointPoint(LeadTrimResult.LeadPitchTopX[iLoopCount], LeadTrimResult.LeadPitchTopY[iLoopCount], LeadTrimResult.LeadPitchBottomX[iLoopCount], LeadTrimResult.LeadPitchBottomY[iLoopCount], out _Angle);
                    LeadTrimResult.LeadLength[iLoopCount] = _Length * _CogLeadTrimAlgo.ResolutionY;

                    //Length 비교
                    if (LeadTrimResult.LeadLength[iLoopCount] > (_CogLeadTrimAlgo.LeadLengthArray[iLoopCount] - _CogLeadTrimAlgo.LeadLengthSpec) && 
                        LeadTrimResult.LeadLength[iLoopCount] < (_CogLeadTrimAlgo.LeadLengthArray[iLoopCount] + _CogLeadTrimAlgo.LeadLengthSpec))
                    {
                        LeadTrimResult.IsLeadLengthGood[iLoopCount] = true;
                    }

                    else
                    {
                        LeadTrimResult.IsLeadLengthGood[iLoopCount] = false;
                        LeadTrimResult.IsGood = false;
                        LeadTrimResult.NgType = eNgType.LEAD_LENGTH;
                    }

                    #endregion 

                    #region Pitch 구하기
                    if (iLoopCount > 0)
                        LeadTrimResult.LeadPitchLength[iLoopCount - 1] = (LeadTrimResult.LeadPitchTopX[iLoopCount] - LeadTrimResult.LeadPitchTopX[iLoopCount - 1]) * _CogLeadTrimAlgo.ResolutionX;
                    #endregion

                    #region Lead Wdith 구하기
                    //Width는 그대로 사용
                    #endregion
                }
                #endregion

                _ProcessWatch.Stop();
                string _ProcessTime = String.Format("LeadMeasurement Time : {0} ms", _ProcessWatch.Elapsed.TotalSeconds.ToString());
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, _ProcessTime, CLogManager.LOG_LEVEL.LOW);
            }

            catch (Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "LeadMeasurement - Inspection Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
                _Result = false;
            }

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadMeasurement - End", CLogManager.LOG_LEVEL.MID);
            return _Result;
        }

        public bool ShoulderInspection(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo)
        {
            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "ShoulderInspection - Start", CLogManager.LOG_LEVEL.MID);

            if (false == _CogLeadTrimAlgo.IsUseShoulderInspection)
            {
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "ShoulderInspection Disable - End", CLogManager.LOG_LEVEL.MID);
                return true;
            }

            bool _Result = true;

            try
            {
                System.Diagnostics.Stopwatch _ProcessWatch = new System.Diagnostics.Stopwatch();
                _ProcessWatch.Reset(); _ProcessWatch.Start();

                #region Lead Shoulder 영역 추출
                SetConnectivityMinimum(2000);
                SetHardFixedThreshold(_CogLeadTrimAlgo.ShoulderThreshold);
                SetPolarity(Convert.ToBoolean(_CogLeadTrimAlgo.ShoulderForeground));
                BlobResults = BlobProc.Execute(_SrcImage, _InspRegion);
                CogLeadTrimShoulderResult _ShoulderResult = GetShoulderResult(BlobResults);

                if (_ShoulderResult.BlobCount != _CogLeadTrimAlgo.LeadCount)
                {
                    LeadTrimResult.LastErrorMessage = "Shoulder Lead Count Error";
                    return false;
                }

                //Lead Shoulder 영역 추출
                CogAffineTransformTool  _LeadAreaTransTool = new CogAffineTransformTool();
                CogRectangleAffine[]    _LeadArea = new CogRectangleAffine[_CogLeadTrimAlgo.LeadCount];
                CogImage8Grey[]         _ShoulderImage = new CogImage8Grey[_CogLeadTrimAlgo.LeadCount];
                double[]               _LeadRotation = new double[_CogLeadTrimAlgo.LeadCount];
                for (int iLoopCount = 0; iLoopCount < _ShoulderResult.BlobCount; ++iLoopCount)
                {
                    #region Lead Shoulder 부분 분리
                    _LeadArea[iLoopCount] = new CogRectangleAffine();
                    _LeadArea[iLoopCount].CenterX       = _ShoulderResult.BlobCenterX[iLoopCount];
                    _LeadArea[iLoopCount].CenterY       = _ShoulderResult.BlobCenterY[iLoopCount];
                    _LeadArea[iLoopCount].SideXLength   = _ShoulderResult.PrincipalHeight[iLoopCount] + 20;
                    _LeadArea[iLoopCount].SideYLength   = _ShoulderResult.PrincipalWidth[iLoopCount];

                    _LeadAreaTransTool.InputImage = _SrcImage;
                    _LeadAreaTransTool.Region = _LeadArea[iLoopCount];
                    _LeadAreaTransTool.Run();

                    _ShoulderImage[iLoopCount] = new CogImage8Grey();
                    _ShoulderImage[iLoopCount] = (CogImage8Grey)_LeadAreaTransTool.OutputImage;

                    if (_ShoulderResult.Angle[iLoopCount] >= 0) _LeadRotation[iLoopCount] = _ShoulderResult.Angle[iLoopCount] - (Math.PI / 2);
                    else                                        _LeadRotation[iLoopCount] = (Math.PI / 2) + _ShoulderResult.Angle[iLoopCount];

                    //string _FileName = string.Format(@"D:\Lead{0}.jpg", iLoopCount + 1);
                    //CogImageFile _CogImageFile = new CogImageFile();
                    //_CogImageFile.Open(_FileName, CogImageFileModeConstants.Write);
                    //_CogImageFile.Append(_ShoulderImage[iLoopCount]);
                    //_CogImageFile.Close();
                    #endregion
                }
                #endregion

                //Caliper로 측정하여 검사영역 나누기
                #region Caliper Tool Setting
                CogCaliperTool _LeadCaliper = new CogCaliperTool();
                _LeadCaliper.RunParams.EdgeMode = CogCaliperEdgeModeConstants.Pair;
                _LeadCaliper.RunParams.Edge0Polarity = CogCaliperPolarityConstants.LightToDark;
                _LeadCaliper.RunParams.Edge0Position = _CogLeadTrimAlgo.LeadEdgeWidth / 2 * (-1);
                _LeadCaliper.RunParams.Edge1Polarity = CogCaliperPolarityConstants.DarkToLight;
                _LeadCaliper.RunParams.Edge1Position = _CogLeadTrimAlgo.LeadEdgeWidth / 2;
                _LeadCaliper.Region = null;
                #endregion

                LeadTrimResult.ShoulderBurrDefectList.Clear();
                LeadTrimResult.ShoulderNickDefectList.Clear();
                for (int iLoopCount = 0; iLoopCount < _ShoulderResult.BlobCount; ++iLoopCount)
                {
                    _LeadCaliper.InputImage = _ShoulderImage[iLoopCount];
                    _LeadCaliper.InputImage.SelectedSpaceName = "@";
                    _LeadCaliper.Run();
                    //CogSerializer.SaveObjectToFile(_LeadCaliper, string.Format(@"D:\Caliper{0}.vpp", iLoopCount + 1));

                    if (_LeadCaliper.Results != null)
                    {
                        //Edge 추출
                        _ShoulderResult.LeadEdgeCenter[iLoopCount]  = _LeadCaliper.Results[0].PositionX;
                        _ShoulderResult.LeadEdgeWidth[iLoopCount]   = _LeadCaliper.Results[0].Width;
                        _ShoulderResult.LeadEdgeLeft[iLoopCount]    = _LeadCaliper.Results[0].Edge0.PositionX;
                        _ShoulderResult.LeadEdgeRight[iLoopCount]   = _LeadCaliper.Results[0].Edge1.PositionX;

                        //Lead 왼쪽, 오른쪽, Lead 영역 추출
                        GetLeadSectionArea(_LeadCaliper.Results, _LeadArea[iLoopCount], _LeadRotation[iLoopCount], 
                                           ref _ShoulderResult.LeadLeftArea[iLoopCount], ref _ShoulderResult.LeadRightArea[iLoopCount], ref _ShoulderResult.LeadCenterArea[iLoopCount]);

                        LeadBurrCheck(_SrcImage, _ShoulderResult.LeadLeftArea[iLoopCount], _CogLeadTrimAlgo);
                        LeadBurrCheck(_SrcImage, _ShoulderResult.LeadRightArea[iLoopCount], _CogLeadTrimAlgo);
                        LeadNickCheck(_SrcImage, _ShoulderResult.LeadCenterArea[iLoopCount], _CogLeadTrimAlgo);

                        #region Blob 설정값 저장하여 확인하기 위해서 / 주석 처리
                        //CogBlobMeasure _BlobMeasure = new CogBlobMeasure();
                        //_BlobMeasure.Measure = CogBlobMeasureConstants.CenterMassX;
                        //_BlobMeasure.Mode = CogBlobMeasureModeConstants.PreCompute;
                        //_BlobMeasure.FilterMode = CogBlobFilterModeConstants.IncludeBlobsInRange;
                        //
                        //CogBlobTool _Blob = new CogBlobTool();
                        //_Blob.InputImage = _SrcImage;
                        //_Blob.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.ErodeHorizontal);
                        //_Blob.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.DilateHorizontal);
                        //_Blob.RunParams.RunTimeMeasures.Add(_BlobMeasure);
                        //
                        //_Blob.Region = _ShoulderResult.LeadLeftArea[iLoopCount];
                        //_Blob.Run();
                        //CogSerializer.SaveObjectToFile(_Blob, string.Format(@"D:\Blob{0}_Left.vpp", iLoopCount + 1));
                        //
                        //_Blob.Region = _ShoulderResult.LeadRightArea[iLoopCount];
                        //_Blob.Run();
                        //CogSerializer.SaveObjectToFile(_Blob, string.Format(@"D:\Blob{0}_Right.vpp", iLoopCount + 1));
                        //
                        //_Blob.RunParams.SegmentationParams.Mode = CogBlobSegmentationModeConstants.HardFixedThreshold;
                        //_Blob.RunParams.SegmentationParams.Polarity = CogBlobSegmentationPolarityConstants.LightBlobs;
                        //_Blob.RunParams.SegmentationParams.HardFixedThreshold = 90;
                        //_Blob.RunParams.ConnectivityMinPixels = 10;
                        //_Blob.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.DilateHorizontal);
                        //_Blob.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.ErodeHorizontal);
                        //
                        //
                        //_Blob.Region = _ShoulderResult.LeadCenterArea[iLoopCount];
                        //_Blob.Run();
                        //CogSerializer.SaveObjectToFile(_Blob, string.Format(@"D:\Blob{0}_Center.vpp", iLoopCount + 1));
                        #endregion
                    }
                }

                _ProcessWatch.Stop();
                string _ProcessTime = String.Format("ShoulderInspection Time : {0} ms", _ProcessWatch.Elapsed.TotalSeconds.ToString());
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, _ProcessTime, CLogManager.LOG_LEVEL.LOW);
            }

            catch (Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "ShoulderInspection - Inspection Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
                _Result = false;
            }

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "ShoulderInspection - End", CLogManager.LOG_LEVEL.MID);
            return _Result;
        }

        /// <summary>
        /// Lead를 Caliper로 측정 후 세 영역으로 나누어 검사하기
        /// </summary>
        /// <param name="_LeadCaliperResult">Caliper Result</param>
        /// <param name="_LeadArea">Lead 전체 영역</param>
        /// <param name="_Rotation">Lead 기울기</param>
        /// <param name="_Left">Lead 왼쪽 영역</param>
        /// <param name="_Right">Lead 오른쪽 영역</param>
        /// <param name="_Center">Lead 중앙 영역</param>
        private void GetLeadSectionArea(CogCaliperResults _LeadCaliperResult, CogRectangleAffine _LeadArea, double _Rotation, ref CogRectangleAffine _Left, ref CogRectangleAffine _Right, ref CogRectangleAffine _Center)
        {
            double _CX, _CY, _W, _H;

            #region Lead 왼쪽, 오른쪽, Lead 영역 추출
            //Lead의 왼쪽 영역 추출 : Lead Shoulder Burr 검사
            _Left = new CogRectangleAffine();
            _CX = _LeadCaliperResult[0].Edge0.PositionX - 10;
            _CY = _LeadArea.CenterY;
            _W = 20;
            _H = _LeadArea.SideYLength + 10;
            _Left.SetCenterLengthsRotationSkew(_CX, _CY, _W, _H, _Rotation, 0);

            //Lead의 오른쪽 영역 추출 : Lead Shoulder Burr 검사
            _Right = new CogRectangleAffine();
            _CX = _LeadCaliperResult[0].Edge1.PositionX + 10;
            _CY = _LeadArea.CenterY;
            _W = 20;
            _H = _LeadArea.SideYLength + 10;
            _Right.SetCenterLengthsRotationSkew(_CX, _CY, _W, _H, _Rotation, 0);

            //Lead 영역 추출 : Lead Shoulder nick 검사
            _Center = new CogRectangleAffine();
            _CX = _LeadCaliperResult[0].PositionX;
            _CY = _LeadArea.CenterY;
            _W = _LeadCaliperResult[0].Width;
            _H = _LeadArea.SideYLength + 10;
            _Center.SetCenterLengthsRotationSkew(_CX, _CY, _W, _H, _Rotation, 0);
            #endregion
        }

        private void LeadBurrCheck(CogImage8Grey _SrcImage, CogRectangleAffine _BurrArea, CogLeadTrimAlgo _CogLeadTrimAlgo)
        {
            SetPolarity(false);
            SetConnectivityMinimum(50);
            SetHardFixedThreshold(_CogLeadTrimAlgo.ShoulderBurrThreshold);
            SetMorphology(CogBlobMorphologyConstants.ErodeHorizontal);
            SetMorphology(CogBlobMorphologyConstants.DilateHorizontal);

            CogBlobResults _BlobResults = BlobProc.Execute(_SrcImage, _BurrArea);
            CogLeadTrimShoulderResult _LeadBurrResult = GetShoulderResult(_BlobResults);

            if (0 == _LeadBurrResult.BlobCount) return;

            CogRectangle _DefectArea = new CogRectangle();
            for (int iLoopCount = 0; iLoopCount < _LeadBurrResult.BlobCount; ++iLoopCount)
            {
                double _RealWidth = _LeadBurrResult.Width[iLoopCount] * _CogLeadTrimAlgo.ResolutionX;
                double _RealHeight = _LeadBurrResult.Height[iLoopCount] * _CogLeadTrimAlgo.ResolutionY;

                if (_RealWidth > _CogLeadTrimAlgo.ShoulderBurrSpec && _RealHeight > _CogLeadTrimAlgo.ShoulderBurrSpec)
                {
                    _DefectArea = new CogRectangle();
                    _DefectArea.SetCenterWidthHeight(_LeadBurrResult.BlobCenterX[iLoopCount], _LeadBurrResult.BlobCenterY[iLoopCount], _LeadBurrResult.Width[iLoopCount], _LeadBurrResult.Height[iLoopCount]);

                    LeadTrimResult.ShoulderBurrDefectList.Add(_DefectArea);
                }
            }
        }

        private void LeadNickCheck(CogImage8Grey _SrcImage, CogRectangleAffine _NickArea, CogLeadTrimAlgo _CogLeadTrimAlgo)
        {
            SetPolarity(true);
            SetConnectivityMinimum(50);
            SetHardFixedThreshold(_CogLeadTrimAlgo.ShoulderNickThreshold);
            SetMorphology(CogBlobMorphologyConstants.ErodeHorizontal);
            SetMorphology(CogBlobMorphologyConstants.DilateHorizontal);

            CogBlobResults _BlobResults = BlobProc.Execute(_SrcImage, _NickArea);
            CogLeadTrimShoulderResult _LeadNickResult = GetShoulderResult(_BlobResults);

            if (0 == _LeadNickResult.BlobCount) return;

            CogRectangle _DefectArea = new CogRectangle();
            for (int iLoopCount = 0; iLoopCount < _LeadNickResult.BlobCount; ++iLoopCount)
            {
                double _RealWidth = _LeadNickResult.Width[iLoopCount] * _CogLeadTrimAlgo.ResolutionX;
                double _RealHeight = _LeadNickResult.Height[iLoopCount] * _CogLeadTrimAlgo.ResolutionY;

                if (_RealWidth > _CogLeadTrimAlgo.ShoulderNickSpec && _RealHeight > _CogLeadTrimAlgo.ShoulderNickSpec)
                {
                    _DefectArea = new CogRectangle();
                    _DefectArea.SetCenterWidthHeight(_LeadNickResult.BlobCenterX[iLoopCount], _LeadNickResult.BlobCenterY[iLoopCount], _LeadNickResult.Width[iLoopCount], _LeadNickResult.Height[iLoopCount]);

                    //LeadTrimResult.ShoulderBurrDefectList.Add(_DefectArea);
                    LeadTrimResult.ShoulderNickDefectList.Add(_DefectArea);
                }
            }
        }

        public bool LeadTipInspection(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo)
        {
            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadTipInspection - Start", CLogManager.LOG_LEVEL.MID);

            if (false == _CogLeadTrimAlgo.IsUseLeadTipInspection)
            {
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadTipInspection Disable - End", CLogManager.LOG_LEVEL.MID);
                return true;
            }

            bool _Result = true;

            try
            {
                System.Diagnostics.Stopwatch _ProcessWatch = new System.Diagnostics.Stopwatch();
                _ProcessWatch.Reset(); _ProcessWatch.Start();

                #region  Lead Tip 영역 추출
                SetConnectivityMinimum(2000);
                SetHardFixedThreshold(_CogLeadTrimAlgo.ShoulderThreshold);
                SetPolarity(Convert.ToBoolean(_CogLeadTrimAlgo.ShoulderForeground));
                BlobResults = BlobProc.Execute(_SrcImage, _InspRegion);
                CogLeadTrimLeadTipResult _LeadTipResult = GetLeadTipResult(BlobResults);

                if (_LeadTipResult.BlobCount != _CogLeadTrimAlgo.LeadCount)
                {
                    LeadTrimResult.LastErrorMessage = "Lead Tip Count Error";
                    return false;
                }

                #region  Lead Tip 영역 추출
                CogAffineTransformTool  _LeadTipAreaTransTool = new CogAffineTransformTool();
                CogRectangleAffine[]    _LeadTipArea = new CogRectangleAffine[_CogLeadTrimAlgo.LeadCount];
                CogImage8Grey[]         _LeadTipImage = new CogImage8Grey[_CogLeadTrimAlgo.LeadCount];
                double[]                _LeadTipRotation = new double[_CogLeadTrimAlgo.LeadCount];
                for (int iLoopCount = 0; iLoopCount < _LeadTipResult.BlobCount; ++iLoopCount)
                {
                    #region Lead Tip 부분 분리
                    _LeadTipArea[iLoopCount] = new CogRectangleAffine();
                    _LeadTipArea[iLoopCount].CenterX     = _LeadTipResult.BlobCenterX[iLoopCount];
                    _LeadTipArea[iLoopCount].CenterY     = _LeadTipResult.BlobCenterY[iLoopCount];
                    _LeadTipArea[iLoopCount].SideXLength = _LeadTipResult.PrincipalHeight[iLoopCount] + 20;
                    _LeadTipArea[iLoopCount].SideYLength = _LeadTipResult.PrincipalWidth[iLoopCount];

                    _LeadTipAreaTransTool.InputImage = _SrcImage;
                    _LeadTipAreaTransTool.Region = _LeadTipArea[iLoopCount];
                    _LeadTipAreaTransTool.Run();

                    _LeadTipImage[iLoopCount] = new CogImage8Grey();
                    _LeadTipImage[iLoopCount] = (CogImage8Grey)_LeadTipAreaTransTool.OutputImage;

                    if (_LeadTipResult.Angle[iLoopCount] >= 0)  _LeadTipRotation[iLoopCount] = _LeadTipResult.Angle[iLoopCount] - (Math.PI / 2);
                    else                                        _LeadTipRotation[iLoopCount] = (Math.PI / 2) + _LeadTipResult.Angle[iLoopCount];

                    //string _FileName = string.Format(@"D:\Lead{0}.jpg", iLoopCount + 1);
                    //CogImageFile _CogImageFile = new CogImageFile();
                    //_CogImageFile.Open(_FileName, CogImageFileModeConstants.Write);
                    //_CogImageFile.Append(_LeadTipImage[iLoopCount]);
                    //_CogImageFile.Close();
                    #endregion
                }
                #endregion
                #endregion

                //Caliper로 측정하여 검사영역 나누기
                #region Caliper Tool Setting
                CogCaliperTool _LeadCaliper = new CogCaliperTool();
                _LeadCaliper.RunParams.EdgeMode = CogCaliperEdgeModeConstants.Pair;
                _LeadCaliper.RunParams.Edge0Polarity = CogCaliperPolarityConstants.LightToDark;
                _LeadCaliper.RunParams.Edge0Position = _CogLeadTrimAlgo.LeadTipEdgeWidth / 2 * (-1);
                _LeadCaliper.RunParams.Edge1Polarity = CogCaliperPolarityConstants.DarkToLight;
                _LeadCaliper.RunParams.Edge1Position = _CogLeadTrimAlgo.LeadTipEdgeWidth / 2;
                _LeadCaliper.Region = null;
                #endregion

                LeadTrimResult.LeadTipBurrDefectList.Clear();
                for (int iLoopCount = 0; iLoopCount < _LeadTipResult.BlobCount; ++iLoopCount)
                {
                    _LeadCaliper.InputImage = _LeadTipImage[iLoopCount];
                    _LeadCaliper.InputImage.SelectedSpaceName = "@";
                    _LeadCaliper.Run();
                    //CogSerializer.SaveObjectToFile(_LeadCaliper, string.Format(@"D:\Caliper{0}.vpp", iLoopCount + 1));

                    if (_LeadCaliper.Results != null)
                    {
                        //Edge 추출
                        _LeadTipResult.LeadEdgeCenter[iLoopCount] = _LeadCaliper.Results[0].PositionX;
                        _LeadTipResult.LeadEdgeWidth[iLoopCount]  = _LeadCaliper.Results[0].Width;
                        _LeadTipResult.LeadEdgeLeft[iLoopCount]   = _LeadCaliper.Results[0].Edge0.PositionX;
                        _LeadTipResult.LeadEdgeRight[iLoopCount]  = _LeadCaliper.Results[0].Edge1.PositionX;

                        //Lead 왼쪽, 오른쪽, Lead 영역 추출
                        GetLeadSectionArea(_LeadCaliper.Results, _LeadTipArea[iLoopCount], _LeadTipRotation[iLoopCount],
                                           ref _LeadTipResult.LeadTipLeftArea[iLoopCount], ref _LeadTipResult.LeadTipRightArea[iLoopCount], ref _LeadTipResult.LeadTipCenterArea[iLoopCount]);

                        LeadTipBurrCheck(_SrcImage, _LeadTipResult.LeadTipLeftArea[iLoopCount], _CogLeadTrimAlgo);
                        LeadTipBurrCheck(_SrcImage, _LeadTipResult.LeadTipRightArea[iLoopCount], _CogLeadTrimAlgo);

                        #region Blob 설정값 저장하여 확인하기 위해서 / 주석 처리
                        //CogBlobMeasure _BlobMeasure = new CogBlobMeasure();
                        //_BlobMeasure.Measure = CogBlobMeasureConstants.CenterMassX;
                        //_BlobMeasure.Mode = CogBlobMeasureModeConstants.PreCompute;
                        //_BlobMeasure.FilterMode = CogBlobFilterModeConstants.IncludeBlobsInRange;
                        //
                        //CogBlobTool _Blob = new CogBlobTool();
                        //_Blob.InputImage = _SrcImage;
                        //_Blob.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.ErodeHorizontal);
                        //_Blob.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.DilateHorizontal);
                        //_Blob.RunParams.RunTimeMeasures.Add(_BlobMeasure);
                        //
                        //_Blob.Region = _LeadTipResult.LeadTipLeftArea[iLoopCount];
                        //_Blob.Run();
                        //CogSerializer.SaveObjectToFile(_Blob, string.Format(@"D:\Blob{0}_Left.vpp", iLoopCount + 1));
                        //
                        //_Blob.Region = _LeadTipResult.LeadTipRightArea[iLoopCount];
                        //_Blob.Run();
                        //CogSerializer.SaveObjectToFile(_Blob, string.Format(@"D:\Blob{0}_Right.vpp", iLoopCount + 1));
                        #endregion
                    }
                }

                _ProcessWatch.Stop();
                string _ProcessTime = String.Format("LeadTipInspection Time : {0} ms", _ProcessWatch.Elapsed.TotalSeconds.ToString());
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, _ProcessTime, CLogManager.LOG_LEVEL.LOW);
            }

            catch (Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "LeadTipInspection - Inspection Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
                _Result = false;
            }

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadTipInspection - End", CLogManager.LOG_LEVEL.MID);
            return _Result;
        }

        private void LeadTipBurrCheck(CogImage8Grey _SrcImage, CogRectangleAffine _BurrArea, CogLeadTrimAlgo _CogLeadTrimAlgo)
        {
            SetPolarity(false);
            SetConnectivityMinimum(50);
            SetHardFixedThreshold(_CogLeadTrimAlgo.ShoulderBurrThreshold);
            SetMorphology(CogBlobMorphologyConstants.ErodeHorizontal);
            SetMorphology(CogBlobMorphologyConstants.DilateHorizontal);

            CogBlobResults _BlobResults = BlobProc.Execute(_SrcImage, _BurrArea);
            CogLeadTrimLeadTipResult _LeadTipBurrResult = GetLeadTipResult(_BlobResults);

            if (0 == _LeadTipBurrResult.BlobCount) return;

            CogRectangle _DefectArea = new CogRectangle();
            for (int iLoopCount = 0; iLoopCount < _LeadTipBurrResult.BlobCount; ++iLoopCount)
            {
                double _RealWidth = _LeadTipBurrResult.Width[iLoopCount] * _CogLeadTrimAlgo.ResolutionX;
                double _RealHeight = _LeadTipBurrResult.Height[iLoopCount] * _CogLeadTrimAlgo.ResolutionY;

                if (_RealWidth > _CogLeadTrimAlgo.ShoulderNickSpec && _RealHeight > _CogLeadTrimAlgo.ShoulderNickSpec)
                {
                    _DefectArea = new CogRectangle();
                    _DefectArea.SetCenterWidthHeight(_LeadTipBurrResult.BlobCenterX[iLoopCount], _LeadTipBurrResult.BlobCenterY[iLoopCount], _LeadTipBurrResult.Width[iLoopCount], _LeadTipBurrResult.Height[iLoopCount]);

                    LeadTrimResult.LeadTipBurrDefectList.Add(_DefectArea);
                }
            }
        }

        public bool GateReminingInspection(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo)
        {
            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "GateRemainingInspection - Start", CLogManager.LOG_LEVEL.MID);

            if (false == _CogLeadTrimAlgo.IsUseGateRemainingInspection)
            {
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "GateRemainingInspection Disable - End", CLogManager.LOG_LEVEL.MID);
                return true;
            }

            bool _Result = true;

            try
            {
                System.Diagnostics.Stopwatch _ProcessWatch = new System.Diagnostics.Stopwatch();
                _ProcessWatch.Reset(); _ProcessWatch.Start();

                SetConnectivityMinimum(20000);
                SetHardFixedThreshold(_CogLeadTrimAlgo.GateRemainingThreshold);
                SetPolarity(Convert.ToBoolean(_CogLeadTrimAlgo.GateRemainingForeground));

                BlobResults = BlobProc.Execute(_SrcImage, _InspRegion);
                GetResult(true);

                CogLeadTrimGateRemainingResult _GateRemainingResult = GetGateRemainingResult(BlobResults);
                LeadTrimResult.GateRemainingNgList.Clear();
                for (int iLoopCount = 0; iLoopCount < _GateRemainingResult.BlobCount; ++iLoopCount)
                {
                    double _ResultRealWidth = _GateRemainingResult.Width[iLoopCount] * _CogLeadTrimAlgo.ResolutionX;
                    double _ResultRealHeight = _GateRemainingResult.Height[iLoopCount] * _CogLeadTrimAlgo.ResolutionY;

                    double _RealWidth = _CogLeadTrimAlgo.GateRemainingSpec;
                    double _RealHeight = _CogLeadTrimAlgo.GateRemainingSpec;

                    if (_ResultRealWidth > _RealWidth && _ResultRealHeight > _RealHeight)
                    {
                        CogRectangle _DefectArea = new CogRectangle();
                        _DefectArea.SetXYWidthHeight(_GateRemainingResult.BlobMinX[iLoopCount], _GateRemainingResult.BlobMinY[iLoopCount], _GateRemainingResult.Width[iLoopCount], _GateRemainingResult.Height[iLoopCount]);
                        LeadTrimResult.GateRemainingNgList.Add(_DefectArea);
                    }
                }

                _ProcessWatch.Stop();
                string _ProcessTime = String.Format("GateRemainingInspection Time : {0} ms", _ProcessWatch.Elapsed.TotalSeconds.ToString());
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, _ProcessTime, CLogManager.LOG_LEVEL.LOW);
            }

            catch (Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "GateRemainingInspection - Inspection Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
                _Result = false;
            }

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "GateRemainingInspection - End", CLogManager.LOG_LEVEL.MID);
            return _Result;
        }

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
            else                    BlobProc.SegmentationParams.Polarity = CogBlobSegmentationPolarityConstants.DarkBlobs;
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

        public CogLeadTrimResult GetLeadTrimResult()
        {
            return LeadTrimResult;
        }

        private CogLeadTrimLeadTipResult GetLeadTipResult(CogBlobResults _BlobResults)
        {
            CogLeadTrimLeadTipResult _LeadTipResult = new CogLeadTrimLeadTipResult();

            if (null == _BlobResults || _BlobResults.GetBlobs().Count < 0) { _LeadTipResult.BlobCount = 0; return _LeadTipResult; }

            _LeadTipResult.BlobCount = _BlobResults.GetBlobs().Count;
            _LeadTipResult.BlobArea = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.Width = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.Height = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.BlobMinX = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.BlobMinY = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.BlobMaxX = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.BlobMaxY = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.BlobCenterX = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.BlobCenterY = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.PrincipalWidth = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.PrincipalHeight = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.BlobMessCenterX = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.BlobMessCenterY = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.Angle = new double[_BlobResults.GetBlobs().Count];

            _LeadTipResult.LeadEdgeLeft = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.LeadEdgeRight = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.LeadEdgeCenter = new double[_BlobResults.GetBlobs().Count];
            _LeadTipResult.LeadEdgeWidth = new double[_BlobResults.GetBlobs().Count];

            _LeadTipResult.LeadTipLeftArea = new CogRectangleAffine[_BlobResults.GetBlobs().Count];
            _LeadTipResult.LeadTipRightArea = new CogRectangleAffine[_BlobResults.GetBlobs().Count];
            _LeadTipResult.LeadTipCenterArea = new CogRectangleAffine[_BlobResults.GetBlobs().Count];

            for (int iLoopCount = 0; iLoopCount < _BlobResults.GetBlobs().Count; ++iLoopCount)
            {
                _LeadTipResult.BlobArea[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Area, iLoopCount);
                _LeadTipResult.Width[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeWidth, iLoopCount);
                _LeadTipResult.Height[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeHeight, iLoopCount);
                _LeadTipResult.BlobMinX[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinX, iLoopCount);
                _LeadTipResult.BlobMinY[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinY, iLoopCount);
                _LeadTipResult.BlobMaxX[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxX, iLoopCount);
                _LeadTipResult.BlobMaxY[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxY, iLoopCount);
                _LeadTipResult.BlobCenterX[iLoopCount] = (_LeadTipResult.BlobMaxX[iLoopCount] + _LeadTipResult.BlobMinX[iLoopCount]) / 2;
                _LeadTipResult.BlobCenterY[iLoopCount] = (_LeadTipResult.BlobMaxY[iLoopCount] + _LeadTipResult.BlobMinY[iLoopCount]) / 2;
                _LeadTipResult.PrincipalWidth[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPrincipalAxisWidth, iLoopCount);
                _LeadTipResult.PrincipalHeight[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPrincipalAxisHeight, iLoopCount);
                _LeadTipResult.BlobMessCenterX[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.CenterMassX, iLoopCount);
                _LeadTipResult.BlobMessCenterY[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.CenterMassY, iLoopCount);
                _LeadTipResult.Angle[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Angle, iLoopCount);
            }

            return _LeadTipResult;
        }

        private CogLeadTrimShoulderResult GetShoulderResult(CogBlobResults _BlobResults)
        {
            CogLeadTrimShoulderResult _ShoulderResult = new CogLeadTrimShoulderResult();

            if (null == _BlobResults || _BlobResults.GetBlobs().Count < 0) { _ShoulderResult.BlobCount = 0; return _ShoulderResult; }
            _ShoulderResult.BlobCount       = _BlobResults.GetBlobs().Count;
            _ShoulderResult.BlobArea        = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.Width           = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.Height          = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.BlobMinX        = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.BlobMinY        = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.BlobMaxX        = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.BlobMaxY        = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.BlobCenterX     = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.BlobCenterY     = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.PrincipalWidth  = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.PrincipalHeight = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.BlobMessCenterX = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.BlobMessCenterY = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.Angle           = new double[_BlobResults.GetBlobs().Count];

            _ShoulderResult.LeadEdgeLeft    = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.LeadEdgeRight   = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.LeadEdgeCenter  = new double[_BlobResults.GetBlobs().Count];
            _ShoulderResult.LeadEdgeWidth   = new double[_BlobResults.GetBlobs().Count];

            _ShoulderResult.LeadLeftArea    = new CogRectangleAffine[_BlobResults.GetBlobs().Count];
            _ShoulderResult.LeadRightArea   = new CogRectangleAffine[_BlobResults.GetBlobs().Count];
            _ShoulderResult.LeadCenterArea  = new CogRectangleAffine[_BlobResults.GetBlobs().Count];

            for (int iLoopCount = 0; iLoopCount < _BlobResults.GetBlobs().Count; ++iLoopCount)
            {
                _ShoulderResult.BlobArea[iLoopCount]    = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Area, iLoopCount);
                _ShoulderResult.Width[iLoopCount]       = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeWidth, iLoopCount);
                _ShoulderResult.Height[iLoopCount]      = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeHeight, iLoopCount);
                _ShoulderResult.BlobMinX[iLoopCount]    = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinX, iLoopCount);
                _ShoulderResult.BlobMinY[iLoopCount]    = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinY, iLoopCount);
                _ShoulderResult.BlobMaxX[iLoopCount]    = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxX, iLoopCount);
                _ShoulderResult.BlobMaxY[iLoopCount]    = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxY, iLoopCount);
                _ShoulderResult.BlobCenterX[iLoopCount] = (_ShoulderResult.BlobMaxX[iLoopCount] + _ShoulderResult.BlobMinX[iLoopCount]) / 2;
                _ShoulderResult.BlobCenterY[iLoopCount] = (_ShoulderResult.BlobMaxY[iLoopCount] + _ShoulderResult.BlobMinY[iLoopCount]) / 2;
                _ShoulderResult.PrincipalWidth[iLoopCount]  = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPrincipalAxisWidth, iLoopCount);
                _ShoulderResult.PrincipalHeight[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPrincipalAxisHeight, iLoopCount);
                _ShoulderResult.BlobMessCenterX[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.CenterMassX, iLoopCount);
                _ShoulderResult.BlobMessCenterY[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.CenterMassY, iLoopCount);
                _ShoulderResult.Angle[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Angle, iLoopCount);
            }

            return _ShoulderResult;
        }

        private CogLeadTrimGateRemainingResult GetGateRemainingResult(CogBlobResults _BlobResults)
        {
            CogLeadTrimGateRemainingResult _GateRemainingResult = new CogLeadTrimGateRemainingResult();

            if (null == _BlobResults || _BlobResults.GetBlobs().Count < 0) { _GateRemainingResult.BlobCount = 0; return _GateRemainingResult; }
            _GateRemainingResult.BlobCount = _BlobResults.GetBlobs().Count;
            _GateRemainingResult.BlobArea = new double[_BlobResults.GetBlobs().Count];
            _GateRemainingResult.Width = new double[_BlobResults.GetBlobs().Count];
            _GateRemainingResult.Height = new double[_BlobResults.GetBlobs().Count];
            _GateRemainingResult.BlobMinX = new double[_BlobResults.GetBlobs().Count];
            _GateRemainingResult.BlobMinY = new double[_BlobResults.GetBlobs().Count];
            _GateRemainingResult.BlobMaxX = new double[_BlobResults.GetBlobs().Count];
            _GateRemainingResult.BlobMaxY = new double[_BlobResults.GetBlobs().Count];
            _GateRemainingResult.BlobCenterX = new double[_BlobResults.GetBlobs().Count];
            _GateRemainingResult.BlobCenterY = new double[_BlobResults.GetBlobs().Count];
            _GateRemainingResult.PrincipalWidth = new double[_BlobResults.GetBlobs().Count];
            _GateRemainingResult.PrincipalHeight = new double[_BlobResults.GetBlobs().Count];
            _GateRemainingResult.BlobMessCenterX = new double[_BlobResults.GetBlobs().Count];
            _GateRemainingResult.BlobMessCenterY = new double[_BlobResults.GetBlobs().Count];
            _GateRemainingResult.Angle = new double[_BlobResults.GetBlobs().Count];

            for (int iLoopCount = 0; iLoopCount < _BlobResults.GetBlobs().Count; ++iLoopCount)
            {
                _GateRemainingResult.BlobArea[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Area, iLoopCount);
                _GateRemainingResult.Width[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeWidth, iLoopCount);
                _GateRemainingResult.Height[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeHeight, iLoopCount);
                _GateRemainingResult.BlobMinX[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinX, iLoopCount);
                _GateRemainingResult.BlobMinY[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinY, iLoopCount);
                _GateRemainingResult.BlobMaxX[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxX, iLoopCount);
                _GateRemainingResult.BlobMaxY[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxY, iLoopCount);
                _GateRemainingResult.BlobCenterX[iLoopCount] = (_GateRemainingResult.BlobMaxX[iLoopCount] + _GateRemainingResult.BlobMinX[iLoopCount]) / 2;
                _GateRemainingResult.BlobCenterY[iLoopCount] = (_GateRemainingResult.BlobMaxY[iLoopCount] + _GateRemainingResult.BlobMinY[iLoopCount]) / 2;
                _GateRemainingResult.PrincipalWidth[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPrincipalAxisWidth, iLoopCount);
                _GateRemainingResult.PrincipalHeight[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPrincipalAxisHeight, iLoopCount);
                _GateRemainingResult.BlobMessCenterX[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.CenterMassX, iLoopCount);
                _GateRemainingResult.BlobMessCenterY[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.CenterMassY, iLoopCount);
                _GateRemainingResult.Angle[iLoopCount] = _BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Angle, iLoopCount);
            }

            return _GateRemainingResult;
        }

        private CogLeadTrimResult GetResults()
        {
            //return InspBlobReferResults;
            return InspLeadTrimResults;
        }

        private bool GetResult(bool _IsGraphicResult)
        {
            bool _Result = true;

            InspLeadTrimResults = new CogLeadTrimResult();
            if (null == BlobResults || BlobResults.GetBlobs().Count <= 0) return false;

            InspLeadTrimResults.BlobCount = BlobResults.GetBlobs().Count;
            InspLeadTrimResults.BlobArea = new double[BlobResults.GetBlobs().Count];
            InspLeadTrimResults.Width = new double[BlobResults.GetBlobs().Count];
            InspLeadTrimResults.Height = new double[BlobResults.GetBlobs().Count];
            InspLeadTrimResults.BlobMinX = new double[BlobResults.GetBlobs().Count];
            InspLeadTrimResults.BlobMinY = new double[BlobResults.GetBlobs().Count];
            InspLeadTrimResults.BlobMaxX = new double[BlobResults.GetBlobs().Count];
            InspLeadTrimResults.BlobMaxY = new double[BlobResults.GetBlobs().Count];
            InspLeadTrimResults.BlobCenterX = new double[BlobResults.GetBlobs().Count];
            InspLeadTrimResults.BlobCenterY = new double[BlobResults.GetBlobs().Count];
            InspLeadTrimResults.PrincipalWidth = new double[BlobResults.GetBlobs().Count];
            InspLeadTrimResults.PrincipalHeight = new double[BlobResults.GetBlobs().Count];
            InspLeadTrimResults.BlobMessCenterX = new double[BlobResults.GetBlobs().Count];
            InspLeadTrimResults.BlobMessCenterY = new double[BlobResults.GetBlobs().Count];
            InspLeadTrimResults.Angle = new double[BlobResults.GetBlobs().Count];
            if (_IsGraphicResult) InspLeadTrimResults.ResultGraphic = new CogCompositeShape[InspLeadTrimResults.BlobCount];

            InspLeadTrimResults.LeadCount = InspLeadTrimResults.BlobCount;
            InspLeadTrimResults.LeadAngle = new double[InspLeadTrimResults.LeadCount];
            InspLeadTrimResults.LeadPitchTopX = new double[InspLeadTrimResults.LeadCount];
            InspLeadTrimResults.LeadPitchTopY = new double[InspLeadTrimResults.LeadCount];
            InspLeadTrimResults.LeadPitchBottomX = new double[InspLeadTrimResults.LeadCount];
            InspLeadTrimResults.LeadPitchBottomY = new double[InspLeadTrimResults.LeadCount];
            InspLeadTrimResults.LeadLength = new double[InspLeadTrimResults.LeadCount];
            InspLeadTrimResults.LeadPitchLength = new double[InspLeadTrimResults.LeadCount - 1];
            InspLeadTrimResults.LeadWidth = new double[InspLeadTrimResults.LeadCount];
            InspLeadTrimResults.LeadLengthStartX = new double[InspLeadTrimResults.LeadCount];
            InspLeadTrimResults.LeadLengthStartY = new double[InspLeadTrimResults.LeadCount];
            InspLeadTrimResults.IsLeadBentGood = new bool[InspLeadTrimResults.LeadCount];

            for (int iLoopCount = 0; iLoopCount < InspLeadTrimResults.BlobCount; ++iLoopCount)
            {
                BlobResult = BlobResults.GetBlobByID(iLoopCount);

                InspLeadTrimResults.BlobArea[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Area, iLoopCount);
                InspLeadTrimResults.Width[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeWidth, iLoopCount);
                InspLeadTrimResults.Height[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeHeight, iLoopCount);
                InspLeadTrimResults.BlobMinX[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinX, iLoopCount);
                InspLeadTrimResults.BlobMinY[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinY, iLoopCount);
                InspLeadTrimResults.BlobMaxX[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxX, iLoopCount);
                InspLeadTrimResults.BlobMaxY[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxY, iLoopCount);
                InspLeadTrimResults.BlobCenterX[iLoopCount] = (InspLeadTrimResults.BlobMaxX[iLoopCount] + InspLeadTrimResults.BlobMinX[iLoopCount]) / 2;
                InspLeadTrimResults.BlobCenterY[iLoopCount] = (InspLeadTrimResults.BlobMaxY[iLoopCount] + InspLeadTrimResults.BlobMinY[iLoopCount]) / 2;
                InspLeadTrimResults.PrincipalWidth[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPrincipalAxisWidth, iLoopCount);
                InspLeadTrimResults.PrincipalHeight[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPrincipalAxisHeight, iLoopCount);
                InspLeadTrimResults.BlobMessCenterX[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.CenterMassX, iLoopCount);
                InspLeadTrimResults.BlobMessCenterY[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.CenterMassY, iLoopCount);
                InspLeadTrimResults.Angle[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Angle, iLoopCount);
                InspLeadTrimResults.LeadAngle[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Angle, iLoopCount);

                if (_IsGraphicResult) InspLeadTrimResults.ResultGraphic[iLoopCount] = BlobResult.CreateResultGraphics(CogBlobResultGraphicConstants.Boundary);
            }

            return _Result;
        }
    }
}
