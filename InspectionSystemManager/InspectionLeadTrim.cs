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

using ParameterManager;
using LogMessageManager;

namespace InspectionSystemManager
{
    class InspectionLeadTrim
    {
        private CogLeadTrimResult LeadTrimResult = new CogLeadTrimResult();

        private CogImage8Grey SrcImage = new CogImage8Grey();

        private CogBlob         BlobProc;
        private CogBlobResults  BlobResults;
        private CogBlobResult   BlobResult;
        private CogBlobReferenceResult InspResults;

        #region Initialize & Deinitialize
        public InspectionLeadTrim()
        {
            BlobProc = new CogBlob();
            BlobResults = new CogBlobResults();
            BlobResult = new CogBlobResult();
            InspResults = new CogBlobReferenceResult();

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

        public bool Run(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo, CogLeadTrimResult _CogLeadTrimResult, int _NgNumber = 0)
        {
            bool _Result = false;

            do
            {
                //Lead body search
                if (false == LeadBodySearch(_SrcImage, _InspRegion, _CogLeadTrimAlgo)) break;

                _Result = true;
            } while (false);

            return _Result;
        }

        public bool LeadBodySearch(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo)
        {
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
                SetConnectivityMinimum(20000);
                SetPolarity(false);

                System.Diagnostics.Stopwatch _ProcessWatch = new System.Diagnostics.Stopwatch();
                _ProcessWatch.Reset(); _ProcessWatch.Start();

                #region Step1. Lead 전체의 Center를 구해서 masking 위치를 따라가게 한다.
                //Step1. Lead 전체의 Center를 구해서 masking 위치를 따라가게 한다.
                BlobResults = BlobProc.Execute(_SrcImage, _InspRegion);
                GetResult(true);

                CogBlobReferenceResult _CogBlobReferResultTemp = new CogBlobReferenceResult();
                _CogBlobReferResultTemp = GetResults();

                if (_CogBlobReferResultTemp.BlobCount <= 0) return false;
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

                _CogBlobReferResultTemp = new CogBlobReferenceResult();
                _CogBlobReferResultTemp = GetResults();

                if (_CogBlobReferResultTemp.BlobCount <= 0) return false;

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

                SetConnectivityMinimum(20000);
                SetHardFixedThreshold(_CogLeadTrimAlgo.ChipOutThreshold);
                SetPolarity(Convert.ToBoolean(_CogLeadTrimAlgo.ChipOutForeground));

                BlobResults = BlobProc.Execute(_SrcImage, _InspRegion);
                GetResult(true);

                CogBlobReferenceResult _CogChipOutResult = new CogBlobReferenceResult();
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
                    }
                }

                _ProcessWatch.Stop();
                string _ProcessTime = String.Format("MoldChipOutInspection Time : {0} ms", _ProcessWatch.Elapsed.TotalSeconds.ToString());
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, _ProcessTime, CLogManager.LOG_LEVEL.LOW);
            }

            catch (Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "MoldChipOutInspection - Inspection Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
                _Result = false;
            }


            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "MoldChipOutInspection - End", CLogManager.LOG_LEVEL.MID);
            return _Result;
        }

        public CogLeadTrimResult GetLeadTrimResult()
        {
            return LeadTrimResult;
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

        private CogBlobReferenceResult GetResults()
        {
            return InspResults;
        }

        private bool GetResult(bool _IsGraphicResult)
        {
            bool _Result = true;

            if (null == BlobResults || BlobResults.GetBlobs().Count < 0) return false;

            InspResults.BlobCount = BlobResults.GetBlobs().Count;
            InspResults.BlobArea = new double[BlobResults.GetBlobs().Count];
            InspResults.Width = new double[BlobResults.GetBlobs().Count];
            InspResults.Height = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobMinX = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobMinY = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobMaxX = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobMaxY = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobCenterX = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobCenterY = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobMessCenterX = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobMessCenterY = new double[BlobResults.GetBlobs().Count];
            InspResults.OriginX = new double[BlobResults.GetBlobs().Count];
            InspResults.OriginY = new double[BlobResults.GetBlobs().Count];
            InspResults.IsGoods = new bool[BlobResults.GetBlobs().Count];
            if (_IsGraphicResult) InspResults.ResultGraphic = new CogCompositeShape[InspResults.BlobCount];

            for (int iLoopCount = 0; iLoopCount < InspResults.BlobCount; ++iLoopCount)
            {
                BlobResult = BlobResults.GetBlobByID(iLoopCount);

                InspResults.BlobArea[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Area, iLoopCount);
                InspResults.Width[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeWidth, iLoopCount);
                InspResults.Height[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeHeight, iLoopCount);
                InspResults.BlobMinX[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinX, iLoopCount);
                InspResults.BlobMinY[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinY, iLoopCount);
                InspResults.BlobMaxX[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxX, iLoopCount);
                InspResults.BlobMaxY[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxY, iLoopCount);
                InspResults.BlobCenterX[iLoopCount] = (InspResults.BlobMaxX[iLoopCount] + InspResults.BlobMinX[iLoopCount]) / 2;
                InspResults.BlobCenterY[iLoopCount] = (InspResults.BlobMaxY[iLoopCount] + InspResults.BlobMinY[iLoopCount]) / 2;
                InspResults.BlobMessCenterX[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.CenterMassX, iLoopCount);
                InspResults.BlobMessCenterY[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.CenterMassY, iLoopCount);

                if (_IsGraphicResult) InspResults.ResultGraphic[iLoopCount] = BlobResult.CreateResultGraphics(CogBlobResultGraphicConstants.Boundary);
            }

            return _Result;
        }
    }
}
