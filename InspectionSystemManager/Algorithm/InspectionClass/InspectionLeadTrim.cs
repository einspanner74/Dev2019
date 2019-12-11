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
        private CogLine                 LeadBodyBaseLine = new CogLine();

        private double                  LeadBodyOffsetX = 0;
        private double                  LeadBodyOffsetY = 0;

        #region Initialize & Deinitialize
        public InspectionLeadTrim()
        {
            
            BlobResults = new CogBlobResults();
            BlobResult = new CogBlobResult();
            //InspBlobReferResults = new CogBlobReferenceResult();
            InspLeadTrimResults = new CogLeadTrimResult();

            InitializeBlobProcessor();
        }

        public void Initialize()
        {

        }

        public void DeInitialize()
        {

        }

        private void InitializeBlobProcessor()
        {
            BlobProc = new CogBlob();
            BlobProc.SegmentationParams.Mode = CogBlobSegmentationModeConstants.HardFixedThreshold;
            BlobProc.SegmentationParams.Polarity = CogBlobSegmentationPolarityConstants.LightBlobs;
            BlobProc.SegmentationParams.HardFixedThreshold = 100;
            BlobProc.ConnectivityMode = CogBlobConnectivityModeConstants.GreyScale;
            BlobProc.ConnectivityCleanup = CogBlobConnectivityCleanupConstants.Fill;
            BlobProc.ConnectivityMinPixels = 10;
        }
        #endregion

        public bool Run(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo, ref CogLeadTrimResult _CogLeadTrimResult, int _NgNumber = 0)
        {
            bool _Result = false;

            ClearLeadTrimResult(_CogLeadTrimAlgo.LeadCount);

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
                if (false == LeadBodySearch(_SrcImage, _LeadBodyArea, _CogLeadTrimAlgo, true))        break;
                if (false == MoldChipOutInspection(_SrcImage, _MoldChipArea, _CogLeadTrimAlgo, true)) break;
                if (false == LeadMeasurement(_SrcImage, _LeadLengthArea, _CogLeadTrimAlgo, true))     break;
                if (false == ShoulderInspection(_SrcImage, _ShoulderArea, _CogLeadTrimAlgo, true))    break;
                if (false == LeadTipInspection(_SrcImage, _LeadTipArea, _CogLeadTrimAlgo, true))      break;
                if (false == GateReminingInspection(_SrcImage, _GateArea, _CogLeadTrimAlgo, true))    break;

                _Result = true;
            } while (false);

            _CogLeadTrimResult = LeadTrimResult;

            return _Result;
        }

        public void ClearLeadTrimResult(int _LeadCount)
        {
            LeadTrimResult = new CogLeadTrimResult();
            LeadTrimResult.EachLeadStatusArray = new EachLeadStatus[_LeadCount];
            for (int iLoopCount = 0; iLoopCount < _LeadCount; ++iLoopCount)
                LeadTrimResult.EachLeadStatusArray[iLoopCount] = new EachLeadStatus();
        }

        public bool LeadBodySearch(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo, bool _InInspProcess = false)
        {
            if (LeadTrimResult.IsGood != true) return true;

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadBodySearch - Start", CLogManager.LOG_LEVEL.MID);

            if (false == _CogLeadTrimAlgo.IsUseLeadBody)
            {
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadBodySearch Disable - End", CLogManager.LOG_LEVEL.MID);
                return true;
            }

            bool _Result = true;
            
            try
            {   
                System.Diagnostics.Stopwatch _ProcessWatch = new System.Diagnostics.Stopwatch();
                _ProcessWatch.Reset(); _ProcessWatch.Start();

                int _Index = 0;
                double _BlobAreaMaxmize = 0;

                InitializeBlobProcessor();

                //SetMorphology(CogBlobMorphologyConstants.ErodeHorizontal);
                //SetMorphology(CogBlobMorphologyConstants.DilateHorizontal);
                SetHardFixedThreshold(50);
                SetConnectivityMinimum(100000);
                SetPolarity(false);

                #region Step1. Lead 전체의 Center를 구해서 masking 위치를 따라가게 한다.
                //Step1. Lead 전체의 Center를 구해서 masking 위치를 따라가게 한다.
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadBodySearch - BlobProc.Start", CLogManager.LOG_LEVEL.MID);

                
                BlobResults = BlobProc.Execute(_SrcImage, _InspRegion);
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadBodySearch - BlobProc.End", CLogManager.LOG_LEVEL.MID);
                //CogLeadTrimShoulderResult _CogShoulderResult = GetShoulderResult(BlobResults);
                GetResult(true);

                CogLeadTrimResult _CogBlobReferResultTemp = new CogLeadTrimResult();
                _CogBlobReferResultTemp = GetResults();
                
                if (_CogBlobReferResultTemp.BlobCount <= 0)// && _CogBlobReferResultTemp.BlobArea[0] > 6000000)
                {
                    //Blob이 없는 경우 제품 자체가 없는걸로 인식
                    LeadTrimResult.NgType = eNgType.EMPTY;
                    LeadTrimResult.IsGood = false;
                    LeadTrimResult.LeadBodyStatus = "NG";
                    LeadTrimResult.SearchArea.SetCenterWidthHeight(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height);
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

                if (_BlobAreaMaxmize < 6000000)
                {
                    //Blob이 없는 경우 제품 자체가 없는걸로 인식
                    LeadTrimResult.NgType = eNgType.EMPTY;
                    LeadTrimResult.IsGood = false;
                    LeadTrimResult.LeadBodyStatus = "NG";
                    LeadTrimResult.SearchArea.SetCenterWidthHeight(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height);
                    return false;
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

                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadBodySearch - Masking End", CLogManager.LOG_LEVEL.MID);
                #endregion

                #region Step3. Lead Pure Body Find
                //Step3. Lead Pure Body Find
                SetHardFixedThreshold(42);
                BlobResults = BlobProc.Execute(_CogCopyImage, _InspRegion);
                GetResult(true);

                _CogBlobReferResultTemp = new CogLeadTrimResult();
                _CogBlobReferResultTemp = GetResults();

                #region Blob 설정값 저장하여 확인하기 위해서 / 주석 처리
                //CogBlobMeasure _BlobMeasure = new CogBlobMeasure();
                //_BlobMeasure.Measure = CogBlobMeasureConstants.CenterMassX;
                //_BlobMeasure.Measure = CogBlobMeasureConstants.CenterMassY;
                //_BlobMeasure.Mode = CogBlobMeasureModeConstants.PreCompute;
                //_BlobMeasure.FilterMode = CogBlobFilterModeConstants.IncludeBlobsInRange;
                //
                //CogBlobTool _Blob = new CogBlobTool();
                //_Blob.InputImage = _CogCopyImage;
                //_Blob.RunParams.RunTimeMeasures.Add(_BlobMeasure);
                //_Blob.Region = _InspRegion;
                //_Blob.Run();
                //CogSerializer.SaveObjectToFile(_Blob, string.Format(@"D:\MaskBlob.vpp"));
                #endregion

                if (_CogBlobReferResultTemp.BlobCount <= 0)
                {
                    //Blob이 없는 경우 제품 자체가 없는걸로 인식
                    LeadTrimResult.NgType = eNgType.EMPTY;
                    LeadTrimResult.IsGood = false;
                    LeadTrimResult.LeadBodyStatus = "NG";
                    LeadTrimResult.SearchArea.SetCenterWidthHeight(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height);
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

                //LJH 2019.11.07
                LeadTrimResult.LeadBodyOffsetX = _CogBlobReferResultTemp.BlobCenterX[_Index] - _CogLeadTrimAlgo.BodyCenterOrigin.X;
                LeadTrimResult.LeadBodyOffsetY = _CogBlobReferResultTemp.BlobCenterY[_Index] - _CogLeadTrimAlgo.BodyCenterOrigin.Y;

                LeadBodyOffsetX = LeadTrimResult.LeadBodyOffsetX;
                LeadBodyOffsetY = LeadTrimResult.LeadBodyOffsetY;

                CogLine _CogLine = new CogLine();
                _CogLine.SetFromStartXYEndXY(LeadTrimResult.LeadBodyLeftTop.X, LeadTrimResult.LeadBodyLeftTop.Y, LeadTrimResult.LeadBodyRightTop.X, LeadTrimResult.LeadBodyRightTop.Y);
                //LeadTrimResult.LeadBodyBaseLine = _CogLine;
                //LeadBodyBaseLine = _CogLine;
                #endregion

                #region Step4. Body 정밀 align
                //4-1. Masking 작업
                CogImage8Grey _CogAlignImage = new CogImage8Grey(_SrcImage);
                CogCopyRegionTool _CogAlignImageCopyTool = new CogCopyRegionTool();
                _CogAlignImageCopyTool.RunParams.FillRegion = true;
                _CogAlignImageCopyTool.RunParams.FillRegionValue = 0;
                _CogAlignImageCopyTool.RunParams.ImageAlignmentEnabled = true;
                _CogAlignImageCopyTool.RunParams.InputImageAlignmentX = _CogAlignImage.Width / 2;
                _CogAlignImageCopyTool.RunParams.InputImageAlignmentY = _CogAlignImage.Height / 2;
                _CogAlignImageCopyTool.RunParams.DestinationImageAlignmentX = _CogAlignImage.Width / 2;
                _CogAlignImageCopyTool.RunParams.DestinationImageAlignmentY = _CogAlignImage.Height / 2;

                /* 20191204 주석 처리
                //Mask 주석
                //CogRectangle _CogMaskArea = new CogRectangle();
                ////_CogMaskArea.SetXYWidthHeight(350, 1720, 2864, 80);
                //_CogMaskArea.SetXYWidthHeight(350, LeadTrimResult.LeadBodyLeftTop.Y - 40, 2864, 80);
                //_CogAlignImageCopyTool.InputImage = _CogAlignImage;
                //_CogAlignImageCopyTool.DestinationImage = _CogAlignImage;
                //_CogAlignImageCopyTool.Region = _CogMaskArea;
                //_CogAlignImageCopyTool.Run();
                //_CogAlignImage = (CogImage8Grey)_CogAlignImageCopyTool.OutputImage;

                //4-2 Find line
                //CogFindLineTool _CogFindLineTool = new CogFindLineTool();
                //_CogFindLineTool.RunParams.NumCalipers = 125;
                //_CogFindLineTool.RunParams.CaliperSearchLength = 50;
                //_CogFindLineTool.RunParams.CaliperProjectionLength = 8;
                //_CogFindLineTool.RunParams.CaliperSearchDirection = 1.5708;
                //_CogFindLineTool.RunParams.NumToIgnore = 5;
                //_CogFindLineTool.RunParams.ExpectedLineSegment.StartX = 0;
                //_CogFindLineTool.RunParams.ExpectedLineSegment.StartY = LeadTrimResult.LeadBodyLeftTop.Y;
                //_CogFindLineTool.RunParams.ExpectedLineSegment.EndX = 3647;
                //_CogFindLineTool.RunParams.ExpectedLineSegment.EndY = LeadTrimResult.LeadBodyLeftTop.Y;
                //_CogFindLineTool.RunParams.CaliperRunParams.ContrastThreshold = 10;
                //_CogFindLineTool.RunParams.CaliperRunParams.FilterHalfSizeInPixels = 5;
                //_CogFindLineTool.InputImage = _CogAlignImage;
                //_CogFindLineTool.Run();
                */

                //4-2 신규 Find line
                CogFindLineTool _CogFindLineTool = new CogFindLineTool();
                _CogFindLineTool.RunParams.NumCalipers = 85;
                _CogFindLineTool.RunParams.CaliperSearchLength = 40;
                _CogFindLineTool.RunParams.CaliperProjectionLength = 18;
                _CogFindLineTool.RunParams.CaliperSearchDirection = 1.5708;
                _CogFindLineTool.RunParams.NumToIgnore = 25;
                _CogFindLineTool.RunParams.ExpectedLineSegment.StartX = 213;
                _CogFindLineTool.RunParams.ExpectedLineSegment.StartY = LeadTrimResult.LeadBodyLeftTop.Y;
                _CogFindLineTool.RunParams.ExpectedLineSegment.EndX = 3347;
                _CogFindLineTool.RunParams.ExpectedLineSegment.EndY = LeadTrimResult.LeadBodyLeftTop.Y;
                _CogFindLineTool.RunParams.CaliperRunParams.ContrastThreshold = 8;
                _CogFindLineTool.RunParams.CaliperRunParams.FilterHalfSizeInPixels = 2;
                _CogFindLineTool.InputImage = _CogAlignImage;
                _CogFindLineTool.Run();
                
                //CogSerializer.SaveObjectToFile(_CogFindLineTool, string.Format(@"D:\FindLine.vpp"));

                LeadTrimResult.LeadBodyBaseLine = _CogFindLineTool.Results.GetLine();
                LeadBodyBaseLine = _CogFindLineTool.Results.GetLine();
                LeadTrimResult.LeadBodyBaseLineSegment = _CogFindLineTool.Results.GetLineSegment();

                if (LeadTrimResult.LeadBodyBaseLineSegment == null)
                {
                    //Blob이 없는 경우 제품 자체가 없는걸로 인식
                    LeadTrimResult.NgType = eNgType.EMPTY;
                    LeadTrimResult.IsGood = false;
                    LeadTrimResult.LeadBodyStatus = "NG";
                    LeadTrimResult.SearchArea.SetCenterWidthHeight(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height);
                    return false;
                }
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
                //_CogImageFile.Open(@"D:\Mask.bmp", CogImageFileModeConstants.Write);
                //_CogImageFile.Append(_CogCopyImage);
                //_CogImageFile.Close();
                #endregion
            }

            catch (Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "LeadBodySearch - Inspection Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
                LeadTrimResult.NgType = eNgType.EMPTY;
                LeadTrimResult.IsGood = false;
                LeadTrimResult.SearchArea.SetCenterWidthHeight(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height);
                _Result = false;
            }

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "LeadBodySearch - End", CLogManager.LOG_LEVEL.MID);

            if (false == _InInspProcess) _Result = true;
            return _Result;
        }

        public bool MoldChipOutInspection(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo, bool _InInspProcess = false)
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
                CogRectangle _OffsetInspRegion = new CogRectangle();
                //_OffsetInspRegion.SetCenterWidthHeight(_InspRegion.CenterX + LeadTrimResult.LeadBodyOffsetX, _InspRegion.CenterY + LeadTrimResult.LeadBodyOffsetY, _InspRegion.Width, _InspRegion.Height);
                _OffsetInspRegion.SetCenterWidthHeight(_InspRegion.CenterX + LeadBodyOffsetX, _InspRegion.CenterY + LeadBodyOffsetY, _InspRegion.Width, _InspRegion.Height);
                BlobResults = BlobProc.Execute(_CogCopyImage, _OffsetInspRegion);
                GetResult(true);

                CogLeadTrimResult _CogChipOutResult = new CogLeadTrimResult();
                _CogChipOutResult = GetResults();

                if (null == LeadTrimResult.ChipOutNgList) LeadTrimResult.ChipOutNgList = new List<CogRectangle>();

                bool _IsCurrentGood = true;
                double _MaxDefectSize = 0;
                LeadTrimResult.ChipOutNgList.Clear();
                for (int iLoopCount = 0; iLoopCount < _CogChipOutResult.BlobCount; ++iLoopCount)
                {
                    double _Width = _CogChipOutResult.BlobMaxX[iLoopCount] - _CogChipOutResult.BlobMinX[iLoopCount];
                    double _Height = _CogChipOutResult.BlobMaxY[iLoopCount] - _CogChipOutResult.BlobMinY[iLoopCount];
                    double _RealWidth = _Width * _CogLeadTrimAlgo.ResolutionX;
                    double _RealHeight = _Height * _CogLeadTrimAlgo.ResolutionY;

                    _IsCurrentGood = true;
                    //if ((_CogLeadTrimAlgo.ChipOutWidthMax > _Width && _CogLeadTrimAlgo.ChipOutWidthMin < _Width) &&
                    //(_CogLeadTrimAlgo.ChipOutHeightMax > _Height && _CogLeadTrimAlgo.ChipOutHeightMin < _Height))
                    if (_RealWidth > _CogLeadTrimAlgo.ChipOutSpec && _RealHeight > _CogLeadTrimAlgo.ChipOutSpec)
                    {
                        CogRectangle _DefectArea = new CogRectangle();
                        _DefectArea.SetXYWidthHeight(_CogChipOutResult.BlobMinX[iLoopCount], _CogChipOutResult.BlobMinY[iLoopCount], _Width, _Height);
                        LeadTrimResult.ChipOutNgList.Add(_DefectArea);

                        LeadTrimResult.NgType = eNgType.CHIP_OUT;
                        LeadTrimResult.ChipOutStatus = "NG";
                        LeadTrimResult.IsGood = false;
                        //if (_RealWidth > _RealHeight)   LeadTrimResult.ChipOutStatus = _RealWidth.ToString();
                        //else                            LeadTrimResult.ChipOutStatus = _RealHeight.ToString();

                        _IsCurrentGood = false;

                        _Result = false;    
                    }

                    //LJH 2019.07.26 : 전제 데이터를 다 남기기 위해 if 문 밖으로 이동
                    if ((_RealWidth > _RealHeight) && (_RealWidth > _MaxDefectSize))
                    {
                        if (false == _IsCurrentGood)
                        {
                            _MaxDefectSize = _RealWidth;
                            LeadTrimResult.ChipOutStatus = _MaxDefectSize.ToString();
                        }

                        else
                        {
                            _MaxDefectSize = _RealHeight;
                            LeadTrimResult.ChipOutStatus = _MaxDefectSize.ToString();
                        }
                    }

                    else if ((_RealHeight >= _RealWidth) && (_RealHeight > _MaxDefectSize))
                    {
                        if (false == _IsCurrentGood)
                        {
                            _MaxDefectSize = _RealHeight;
                            LeadTrimResult.ChipOutStatus = _MaxDefectSize.ToString();
                        }

                        else
                        {
                            _MaxDefectSize = _RealWidth;
                            LeadTrimResult.ChipOutStatus = _MaxDefectSize.ToString();
                        }
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

            if (false == _InInspProcess) _Result = true;
            return _Result;
        }

        public bool LeadMeasurement(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo, bool _InInspProcess = false)
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

                SetConnectivityMinimum(5000);
                SetHardFixedThreshold(_CogLeadTrimAlgo.LeadThreshold);
                SetPolarity(Convert.ToBoolean(_CogLeadTrimAlgo.LeadForeground));

                CogRectangle _LeadInspRegion = new CogRectangle();
                //_LeadInspRegion.SetCenterWidthHeight(_InspRegion.CenterX + LeadTrimResult.LeadBodyOffsetX, _InspRegion.CenterY + LeadTrimResult.LeadBodyOffsetY, _InspRegion.Width, _InspRegion.Height);
                _LeadInspRegion.SetCenterWidthHeight(_InspRegion.CenterX + LeadBodyOffsetX, _InspRegion.CenterY + LeadBodyOffsetY, _InspRegion.Width, _InspRegion.Height);
                _InspRegion = _LeadInspRegion;
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
                LeadTrimResult.LeadTipWidth = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.IsLeadBentGood = new bool[_CogLeadTrimResult.BlobCount - 1];
                LeadTrimResult.LeadPitchPointX = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadPitchPointY = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadPitchTopX = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadPitchTopY = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadPitchBottomX = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadPitchBottomY = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.Angle = new double[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.LeadLengthResult = new bool[_CogLeadTrimResult.BlobCount];
                LeadTrimResult.IsLeadLengthGood = new bool[_CogLeadTrimResult.BlobCount];

                if (LeadTrimResult.LeadCount != _CogLeadTrimAlgo.LeadCount)
                {
                    LeadTrimResult.IsGood = false;
                    LeadTrimResult.NgType = eNgType.LEAD_CNT;
                    LeadTrimResult.LeadCountStatus = LeadTrimResult.LeadCount.ToString();
                    LeadTrimResult.SearchArea.SetCenterWidthHeight(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height);
                    return false;
                }

                for (int iLoopCount = 0; iLoopCount < _CogLeadTrimResult.BlobCount; ++iLoopCount)
                {
                    #region Lead Bent Check Not use!!!
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

                    #region Pitch 구하기 (New)
                    //Pitch Point 구하기 Lead 끝단의 Center
                    CogRectangleAffine _CaliperRegion = new CogRectangleAffine();

                    //_CaliperRegion.SetCenterLengthsRotationSkew(LeadTrimResult.LeadPitchTopX[iLoopCount], LeadTrimResult.LeadPitchTopY[iLoopCount] + 55, 70, 50, 0, 0);
                    //_CaliperRegion.SetCenterLengthsRotationSkew(LeadTrimResult.LeadPitchTopX[iLoopCount], LeadTrimResult.LeadPitchTopY[iLoopCount] + 55, 70, 50, _CogLeadTrimResult.Angle[iLoopCount] - 1.5708, 0);

                    //Lead Tip 기준
                    //_CaliperRegion.SetCenterLengthsRotationSkew(LeadTrimResult.LeadPitchTopX[iLoopCount], LeadTrimResult.LeadPitchTopY[iLoopCount] + 70, 70, 50, _CogLeadTrimResult.Angle[iLoopCount] - 1.5708, 0);

                    //Mold Base Line 기준
                    //_CaliperRegion.SetCenterLengthsRotationSkew(LeadTrimResult.LeadPitchTopX[iLoopCount], LeadBodyBaseLine.Y - 740, 70, 50, _CogLeadTrimResult.Angle[iLoopCount] - 1.5708, 0);
                    _CaliperRegion.SetCenterLengthsRotationSkew(LeadTrimResult.LeadPitchTopX[iLoopCount], LeadBodyBaseLine.Y - 740, 140, 50, _CogLeadTrimResult.Angle[iLoopCount] - 1.5708, 0);

                    CogCaliperTool _TipCaliper = new CogCaliperTool();
                    _TipCaliper.RunParams.EdgeMode = CogCaliperEdgeModeConstants.Pair;
                    _TipCaliper.RunParams.Edge0Polarity = CogCaliperPolarityConstants.LightToDark;
                    _TipCaliper.RunParams.Edge0Position = -25;
                    _TipCaliper.RunParams.Edge1Polarity = CogCaliperPolarityConstants.DarkToLight;
                    _TipCaliper.RunParams.Edge1Position = 25;
                    _TipCaliper.RunParams.ContrastThreshold = 10;
                    _TipCaliper.RunParams.FilterHalfSizeInPixels = 5;
                    _TipCaliper.Region = _CaliperRegion;
                    _TipCaliper.InputImage = _SrcImage;
                    _TipCaliper.Run();
                    //CogSerializer.SaveObjectToFile(_TipCaliper, string.Format(@"D:\TipCaliper{0}.vpp", iLoopCount + 1));
                    
                    if (_TipCaliper.Results != null)
                    {
                        LeadTrimResult.LeadPitchPointX[iLoopCount] = _TipCaliper.Results[0].PositionX;
                        LeadTrimResult.LeadPitchPointY[iLoopCount] = _TipCaliper.Results[0].PositionY;
                        LeadTrimResult.LeadTipWidth[iLoopCount] = _TipCaliper.Results[0].Width *_CogLeadTrimAlgo.ResolutionX;
                        LeadTrimResult.EachLeadStatusArray[iLoopCount].TipWidth = (_TipCaliper.Results[0].Width * _CogLeadTrimAlgo.ResolutionX).ToString();
                    }
                    
                    if (iLoopCount > 0)
                    {
                        LeadTrimResult.LeadPitchLength[iLoopCount - 1] = (LeadTrimResult.LeadPitchPointX[iLoopCount] - LeadTrimResult.LeadPitchPointX[iLoopCount - 1]) * _CogLeadTrimAlgo.ResolutionX;
                        LeadTrimResult.EachLeadStatusArray[iLoopCount - 1].Bent = LeadTrimResult.LeadPitchLength[iLoopCount - 1].ToString();
                    
                        //Pitch Spec에서 완전히 벗어났는지 확인
                        if (LeadTrimResult.LeadPitchLength[iLoopCount - 1] > (_CogLeadTrimAlgo.LeadPitchArray[iLoopCount - 1] - _CogLeadTrimAlgo.LeadPitchSpec) &&
                            LeadTrimResult.LeadPitchLength[iLoopCount - 1] < (_CogLeadTrimAlgo.LeadPitchArray[iLoopCount - 1] + _CogLeadTrimAlgo.LeadPitchSpec))
                        {
                            //Align Skew 가능 범위에 들어와 있는지 확인
                            //Skew 범위 안쪽이면 Skew 여부에 상관없이 GOOD
                            if (LeadTrimResult.LeadPitchLength[iLoopCount - 1] > (_CogLeadTrimAlgo.LeadPitchArray[iLoopCount - 1] - _CogLeadTrimAlgo.LeadSkewSpec) &&
                                LeadTrimResult.LeadPitchLength[iLoopCount - 1] < (_CogLeadTrimAlgo.LeadPitchArray[iLoopCount - 1] + _CogLeadTrimAlgo.LeadSkewSpec))
                            {
                                //if (LeadTrimResult.NgType == eNgType.GOOD)
                                LeadTrimResult.IsLeadBentGood[iLoopCount - 1] = true;
                                LeadTrimResult.EachLeadStatusArray[iLoopCount - 1].SetSkewResult(eLeadStatus.GOOD);
                            }
                    
                            //Skew 범위 < Position < Pitch Err 범위
                            //불량 판정 & Skew 가능 에러로 전달
                            else
                            {
                                LeadTrimResult.IsLeadBentGood[iLoopCount - 1] = false;
                                LeadTrimResult.IsGood = false;
                    
                                LeadTrimResult.EachLeadStatusArray[iLoopCount - 1].SetSkewResult(eLeadStatus.LEAD_SKEW_ENABLE);
                    
                                //불량이 발생해도 계속 진행 하도록 설정 _Result = false -> true;
                                //_Result = false;
                                _Result = true;
                            }
                        }
                    
                        //완전히 벗어나면 Skew 불가능 에러
                        else
                        {
                            LeadTrimResult.IsLeadBentGood[iLoopCount - 1] = false;
                            LeadTrimResult.IsGood = false;
                    
                            LeadTrimResult.EachLeadStatusArray[iLoopCount - 1].SetSkewResult(eLeadStatus.LEAD_SKEW_DISABLE);
                    
                            //불량이 발생해도 계속 진행 하도록 설정 _Result = false -> true;
                            //_Result = false;
                            _Result = true;
                        }
                    }
                    #endregion

                    #region Pitch 구하기 (기존)
                    //if (iLoopCount > 0)
                    //{
                    //    LeadTrimResult.LeadPitchLength[iLoopCount - 1] = (LeadTrimResult.LeadPitchTopX[iLoopCount] - LeadTrimResult.LeadPitchTopX[iLoopCount - 1]) * _CogLeadTrimAlgo.ResolutionX;
                    //    LeadTrimResult.EachLeadStatusArray[iLoopCount - 1].Bent = LeadTrimResult.LeadPitchLength[iLoopCount - 1].ToString();
                    //
                    //    //Pitch Spec에서 완전히 벗어났는지 확인
                    //    if (LeadTrimResult.LeadPitchLength[iLoopCount - 1] > (_CogLeadTrimAlgo.LeadPitchArray[iLoopCount - 1] - _CogLeadTrimAlgo.LeadPitchSpec) &&
                    //        LeadTrimResult.LeadPitchLength[iLoopCount - 1] < (_CogLeadTrimAlgo.LeadPitchArray[iLoopCount - 1] + _CogLeadTrimAlgo.LeadPitchSpec))
                    //    {
                    //        //Align Skew 가능 범위에 들어와 있는지 확인
                    //        //Skew 범위 안쪽이면 Skew 여부에 상관없이 GOOD
                    //        if (LeadTrimResult.LeadPitchLength[iLoopCount - 1] > (_CogLeadTrimAlgo.LeadPitchArray[iLoopCount - 1] - _CogLeadTrimAlgo.LeadSkewSpec) &&
                    //            LeadTrimResult.LeadPitchLength[iLoopCount - 1] < (_CogLeadTrimAlgo.LeadPitchArray[iLoopCount - 1] + _CogLeadTrimAlgo.LeadSkewSpec))
                    //        {
                    //            //if (LeadTrimResult.NgType == eNgType.GOOD)
                    //            LeadTrimResult.IsLeadBentGood[iLoopCount - 1] = true;
                    //            LeadTrimResult.EachLeadStatusArray[iLoopCount - 1].SetSkewResult(eLeadStatus.GOOD);
                    //        }
                    //
                    //        //Skew 범위 < Position < Pitch Err 범위
                    //        //불량 판정 & Skew 가능 에러로 전달
                    //        else
                    //        {
                    //            LeadTrimResult.IsLeadBentGood[iLoopCount - 1] = false;
                    //            LeadTrimResult.IsGood = false;
                    //
                    //            LeadTrimResult.EachLeadStatusArray[iLoopCount - 1].SetSkewResult(eLeadStatus.LEAD_SKEW_ENABLE);
                    //
                    //            //불량이 발생해도 계속 진행 하도록 설정 _Result = false -> true;
                    //            //_Result = false;
                    //            _Result = true;
                    //        }
                    //    }
                    //
                    //    //완전히 벗어나면 Skew 불가능 에러
                    //    else
                    //    {
                    //        LeadTrimResult.IsLeadBentGood[iLoopCount - 1] = false;
                    //        LeadTrimResult.IsGood = false;
                    //
                    //        LeadTrimResult.EachLeadStatusArray[iLoopCount - 1].SetSkewResult(eLeadStatus.LEAD_SKEW_DISABLE);
                    //
                    //        //불량이 발생해도 계속 진행 하도록 설정 _Result = false -> true;
                    //        //_Result = false;
                    //        _Result = true;
                    //    }
                    //}
                    #endregion

                    #region Length 구하기
                    CogLine _CogLeadLine = new CogLine();
                    _CogLeadLine.SetFromStartXYEndXY(LeadTrimResult.LeadPitchTopX[iLoopCount], LeadTrimResult.LeadPitchTopY[iLoopCount], LeadTrimResult.LeadPitchBottomX[iLoopCount], LeadTrimResult.LeadPitchBottomY[iLoopCount]);

                    CogIntersectLineLineTool _CogIntersect = new CogIntersectLineLineTool();
                    _CogIntersect.InputImage = _SrcImage;
                    //_CogIntersect.LineA = LeadTrimResult.LeadBodyBaseLine;
                    _CogIntersect.LineA = LeadBodyBaseLine;
                    _CogIntersect.LineB = _CogLeadLine;
                    //CogSerializer.SaveObjectToFile(_CogIntersect, @"D:\_CogIntersect.vpp");
                    _CogIntersect.Run();
                    

                    LeadTrimResult.LeadPitchBottomX[iLoopCount] = _CogIntersect.X;
                    LeadTrimResult.LeadPitchBottomY[iLoopCount] = _CogIntersect.Y;

                    double _Angle;
                    double _Length = CogMath.DistancePointPoint(LeadTrimResult.LeadPitchTopX[iLoopCount], LeadTrimResult.LeadPitchTopY[iLoopCount], LeadTrimResult.LeadPitchBottomX[iLoopCount], LeadTrimResult.LeadPitchBottomY[iLoopCount], out _Angle);
                    LeadTrimResult.LeadLength[iLoopCount] = _Length * _CogLeadTrimAlgo.ResolutionY;
                    LeadTrimResult.EachLeadStatusArray[iLoopCount].Length = LeadTrimResult.LeadLength[iLoopCount].ToString();

                    //Length 비교
                    if (LeadTrimResult.LeadLength[iLoopCount] > (_CogLeadTrimAlgo.LeadLengthArray[iLoopCount] - _CogLeadTrimAlgo.LeadLengthSpec) && 
                        LeadTrimResult.LeadLength[iLoopCount] < (_CogLeadTrimAlgo.LeadLengthArray[iLoopCount] + _CogLeadTrimAlgo.LeadLengthSpec))
                    {
                        LeadTrimResult.IsLeadLengthGood[iLoopCount] = true;
                        LeadTrimResult.EachLeadStatusArray[iLoopCount].SetSkewResult(eLeadStatus.GOOD);
                    }

                    else
                    {
                        LeadTrimResult.IsLeadLengthGood[iLoopCount] = false;
                        LeadTrimResult.IsGood = false;

                        //LeadTrimResult.SkewResult[iLoopCount].Status = eTrimSkewStatus.GOOD;
                        //LeadTrimResult.SkewResult[iLoopCount].NgType = eTrimSkewNgType.LEAD_LENGTH;
                        //LeadTrimResult.SkewResult[iLoopCount].SetSkewResult(eTrimSkewStatus.GOOD, eTrimSkewNgType.LEAD_LENGTH);
                        LeadTrimResult.EachLeadStatusArray[iLoopCount].SetSkewResult(eLeadStatus.LEAD_LENGTH);

                        //불량이 발생해도 계속 진행 하도록 설정 _Result = false -> true;
                        //_Result = false;
                        _Result = true;
                    }
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

            if (false == _InInspProcess) _Result = true;

            return _Result;
        }

        public bool ShoulderInspection(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo, bool _InInspProcess = false)
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

                CogRectangle _LeadInspRegion = new CogRectangle();
                //_LeadInspRegion.SetCenterWidthHeight(_InspRegion.CenterX + LeadTrimResult.LeadBodyOffsetX, _InspRegion.CenterY + LeadTrimResult.LeadBodyOffsetY, _InspRegion.Width, _InspRegion.Height);
                _LeadInspRegion.SetCenterWidthHeight(_InspRegion.CenterX + LeadBodyOffsetX, _InspRegion.CenterY + LeadBodyOffsetY, _InspRegion.Width, _InspRegion.Height);
                _InspRegion = _LeadInspRegion;
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
                    _LeadArea[iLoopCount].SideXLength   = _ShoulderResult.PrincipalHeight[iLoopCount] + 10;
                    //_LeadArea[iLoopCount].SideXLength = _ShoulderResult.PrincipalHeight[iLoopCount];
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

                    if (_LeadCaliper.Results != null && _LeadCaliper.Results.Count > 0)
                    {
                        //Edge 추출
                        _ShoulderResult.LeadEdgeCenter[iLoopCount]  = _LeadCaliper.Results[0].PositionX;
                        _ShoulderResult.LeadEdgeWidth[iLoopCount]   = _LeadCaliper.Results[0].Width;
                        _ShoulderResult.LeadEdgeLeft[iLoopCount]    = _LeadCaliper.Results[0].Edge0.PositionX;
                        _ShoulderResult.LeadEdgeRight[iLoopCount]   = _LeadCaliper.Results[0].Edge1.PositionX;

                        //Lead 왼쪽, 오른쪽, Lead 영역 추출
                        GetLeadSectionArea(_LeadCaliper.Results, _LeadArea[iLoopCount], _LeadRotation[iLoopCount], 
                                           ref _ShoulderResult.LeadLeftArea[iLoopCount], ref _ShoulderResult.LeadRightArea[iLoopCount], ref _ShoulderResult.LeadCenterArea[iLoopCount]);

                        LeadBurrCheck(_SrcImage, _ShoulderResult.LeadLeftArea[iLoopCount], _CogLeadTrimAlgo, iLoopCount);
                        LeadBurrCheck(_SrcImage, _ShoulderResult.LeadRightArea[iLoopCount], _CogLeadTrimAlgo, iLoopCount);
                        LeadNickCheck(_SrcImage, _ShoulderResult.LeadCenterArea[iLoopCount], _CogLeadTrimAlgo, iLoopCount);

                        //if (LeadTrimResult.NgType != eNgType.GOOD)
                        //{
                        //    LeadTrimResult.IsGood = false;
                        //
                        //    //불량이 발생해도 계속 진행 하도록 설정 _Result = false -> true;
                        //    //_Result = false;
                        //    _Result = true;
                        //}

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

                    else if (_LeadCaliper.Results.Count <= 0)
                    {
                        CogRectangle _ShoulderArea = new CogRectangle();
                        _ShoulderArea.SetCenterWidthHeight(_LeadArea[iLoopCount].CenterX, _LeadArea[iLoopCount].CenterY, _LeadArea[iLoopCount].SideXLength, _LeadArea[iLoopCount].SideYLength);
                        LeadTrimResult.ShoulderBurrDefectList.Add(_ShoulderArea);
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

            if (false == _InInspProcess) _Result = true;

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
            //Burr or nick이 발생할경우 오히려 rotation 값에 오차가 발생함
            
            //Lead의 왼쪽 영역 추출 : Lead Shoulder Burr 검사
            _Left = new CogRectangleAffine();
            _CX = _LeadCaliperResult[0].Edge0.PositionX - 10;
            _CY = _LeadArea.CenterY;
            _W = 20;
            //_H = _LeadArea.SideYLength + 5;
            _H = _LeadArea.SideYLength;
            //_Left.SetCenterLengthsRotationSkew(_CX, _CY, _W, _H, _Rotation, 0);
            _Left.SetCenterLengthsRotationSkew(_CX, _CY, _W, _H, 0, 0);

            //Lead의 오른쪽 영역 추출 : Lead Shoulder Burr 검사
            _Right = new CogRectangleAffine();
            _CX = _LeadCaliperResult[0].Edge1.PositionX + 10;
            _CY = _LeadArea.CenterY;
            _W = 20;
            //_H = _LeadArea.SideYLength + 5;
            _H = _LeadArea.SideYLength;
            //_Right.SetCenterLengthsRotationSkew(_CX, _CY, _W, _H, _Rotation, 0);
            _Right.SetCenterLengthsRotationSkew(_CX, _CY, _W, _H, 0, 0);

            //Lead 영역 추출 : Lead Shoulder nick 검사
            _Center = new CogRectangleAffine();
            _CX = _LeadCaliperResult[0].PositionX;
            _CY = _LeadArea.CenterY;
            _W = _LeadCaliperResult[0].Width;
            //_H = _LeadArea.SideYLength + 5;
            _H = _LeadArea.SideYLength;
            //_Center.SetCenterLengthsRotationSkew(_CX, _CY, _W, _H, _Rotation, 0);
            _Center.SetCenterLengthsRotationSkew(_CX, _CY, _W, _H, 0, 0);
            #endregion
        }

        private void LeadBurrCheck(CogImage8Grey _SrcImage, CogRectangleAffine _BurrArea, CogLeadTrimAlgo _CogLeadTrimAlgo, int _LeadIndex)
        {
            SetPolarity(false);
            SetConnectivityMinimum(10);
            SetHardFixedThreshold(_CogLeadTrimAlgo.ShoulderBurrThreshold);
            SetMorphology(CogBlobMorphologyConstants.ErodeHorizontal);
            SetMorphology(CogBlobMorphologyConstants.DilateHorizontal);

            CogBlobResults _BlobResults = BlobProc.Execute(_SrcImage, _BurrArea);
            CogLeadTrimShoulderResult _LeadBurrResult = GetShoulderResult(_BlobResults);

            if (0 == _LeadBurrResult.BlobCount) return;

            bool _IsCurrentGood = false;
            double _MaxDefectSize = 0;
            CogRectangle _DefectArea = new CogRectangle();
            for (int iLoopCount = 0; iLoopCount < _LeadBurrResult.BlobCount; ++iLoopCount)
            {
                double _RealWidth = _LeadBurrResult.Width[iLoopCount] * _CogLeadTrimAlgo.ResolutionX;
                double _RealHeight = _LeadBurrResult.Height[iLoopCount] * _CogLeadTrimAlgo.ResolutionY;

                _IsCurrentGood = true;
                if (_RealWidth > _CogLeadTrimAlgo.ShoulderBurrSpec && _RealHeight > _CogLeadTrimAlgo.ShoulderBurrSpec)
                {
                    _DefectArea = new CogRectangle();
                    _DefectArea.SetCenterWidthHeight(_LeadBurrResult.BlobCenterX[iLoopCount], _LeadBurrResult.BlobCenterY[iLoopCount], _LeadBurrResult.Width[iLoopCount], _LeadBurrResult.Height[iLoopCount]);

                    LeadTrimResult.ShoulderBurrDefectList.Add(_DefectArea);

                    //LeadTrimResult.SkewResult[iLoopCount].Status = eTrimSkewStatus.GOOD;
                    //LeadTrimResult.SkewResult[iLoopCount].NgType = eTrimSkewNgType.SHLD_BURR;
                    //LeadTrimResult.SkewResult[iLoopCount].SetSkewResult(eTrimSkewStatus.GOOD, eTrimSkewNgType.SHLD_BURR);
                    LeadTrimResult.IsGood = false;
                    LeadTrimResult.EachLeadStatusArray[_LeadIndex].SetSkewResult(eLeadStatus.SHLD_BURR);

                    _IsCurrentGood = false;

                    //LJH 2019.07.26 : 전제 데이터를 다 남기기 위해 if 문 밖으로CC
                    //if (_RealWidth > _RealHeight)   LeadTrimResult.EachLeadStatusArray[_LeadIndex].ShoulderBurr = _RealWidth.ToString();
                    //else                            LeadTrimResult.EachLeadStatusArray[_LeadIndex].ShoulderBurr = _RealHeight.ToString();
                }

                //LJH 2019.07.26 : 전제 데이터를 다 남기기 위해 if 문 밖으로 이동
                if ((_RealWidth > _RealHeight) && (_RealWidth > _MaxDefectSize))
                {
                    if (false == _IsCurrentGood)
                    {
                        _MaxDefectSize = _RealWidth;
                        LeadTrimResult.EachLeadStatusArray[_LeadIndex].ShoulderBurr = _MaxDefectSize.ToString();
                    }

                    else
                    {
                        _MaxDefectSize = _RealHeight;
                        LeadTrimResult.EachLeadStatusArray[_LeadIndex].ShoulderBurr = _MaxDefectSize.ToString();
                    }
                }

                else if ((_RealHeight >= _RealWidth) && (_RealHeight > _MaxDefectSize))
                {
                    if (false == _IsCurrentGood)
                    {
                        _MaxDefectSize = _RealHeight;
                        LeadTrimResult.EachLeadStatusArray[_LeadIndex].ShoulderBurr = _MaxDefectSize.ToString();
                    }

                    else
                    {
                        _MaxDefectSize = _RealWidth;
                        LeadTrimResult.EachLeadStatusArray[_LeadIndex].ShoulderBurr = _MaxDefectSize.ToString();
                    }
                }
            }
        }

        private void LeadNickCheck(CogImage8Grey _SrcImage, CogRectangleAffine _NickArea, CogLeadTrimAlgo _CogLeadTrimAlgo, int _LeadIndex)
        {
            SetPolarity(true);
            SetConnectivityMinimum(10);
            SetHardFixedThreshold(_CogLeadTrimAlgo.ShoulderNickThreshold);
            SetMorphology(CogBlobMorphologyConstants.ErodeHorizontal);
            SetMorphology(CogBlobMorphologyConstants.DilateHorizontal);

            CogBlobResults _BlobResults = BlobProc.Execute(_SrcImage, _NickArea);
            CogLeadTrimShoulderResult _LeadNickResult = GetShoulderResult(_BlobResults);

            if (0 == _LeadNickResult.BlobCount) return;

            bool _IsCurrentGood = false;
            double _MaxDefectSize = 0;
            CogRectangle _DefectArea = new CogRectangle();
            for (int iLoopCount = 0; iLoopCount < _LeadNickResult.BlobCount; ++iLoopCount)
            {
                double _RealWidth = _LeadNickResult.Width[iLoopCount] * _CogLeadTrimAlgo.ResolutionX;
                double _RealHeight = _LeadNickResult.Height[iLoopCount] * _CogLeadTrimAlgo.ResolutionY;

                _IsCurrentGood = true;
                if (_RealWidth > _CogLeadTrimAlgo.ShoulderNickSpec && _RealHeight > _CogLeadTrimAlgo.ShoulderNickSpec)
                {
                    _DefectArea = new CogRectangle();
                    _DefectArea.SetCenterWidthHeight(_LeadNickResult.BlobCenterX[iLoopCount], _LeadNickResult.BlobCenterY[iLoopCount], _LeadNickResult.Width[iLoopCount], _LeadNickResult.Height[iLoopCount]);

                    //LeadTrimResult.ShoulderBurrDefectList.Add(_DefectArea);
                    LeadTrimResult.ShoulderNickDefectList.Add(_DefectArea);

                    //LeadTrimResult.SkewResult[iLoopCount].Status = eTrimSkewStatus.GOOD;
                    //LeadTrimResult.SkewResult[iLoopCount].NgType = eTrimSkewNgType.SHLD_NICK;
                    //LeadTrimResult.SkewResult[iLoopCount].SetSkewResult(eTrimSkewStatus.GOOD, eTrimSkewNgType.SHLD_NICK);
                    LeadTrimResult.IsGood = false;
                    LeadTrimResult.EachLeadStatusArray[_LeadIndex].SetSkewResult(eLeadStatus.SHLD_NICK);

                    _IsCurrentGood = false;

                    //LJH 2019.07.26 : 전제 데이터를 다 남기기 위해 if 문 밖으로 이동
                    //if (_RealWidth > _RealHeight)   LeadTrimResult.EachLeadStatusArray[_LeadIndex].ShoulderNick = _RealWidth.ToString();
                    //else                            LeadTrimResult.EachLeadStatusArray[_LeadIndex].ShoulderNick = _RealHeight.ToString();
                }

                //LJH 2019.07.26 : 전제 데이터를 다 남기기 위해 if 문 밖으로 이동
                if ((_RealWidth > _RealHeight) && (_RealWidth > _MaxDefectSize))
                {
                    if (false == _IsCurrentGood)
                    {
                        _MaxDefectSize = _RealWidth;
                        LeadTrimResult.EachLeadStatusArray[_LeadIndex].ShoulderNick = _MaxDefectSize.ToString();
                    }

                    else
                    {
                        _MaxDefectSize = _RealHeight;
                        LeadTrimResult.EachLeadStatusArray[_LeadIndex].ShoulderNick = _MaxDefectSize.ToString();
                    }
                }

                else if ((_RealHeight >= _RealWidth) && (_RealHeight > _MaxDefectSize))
                {
                    if (false == _IsCurrentGood)
                    {
                        _MaxDefectSize = _RealHeight;
                        LeadTrimResult.EachLeadStatusArray[_LeadIndex].ShoulderNick = _MaxDefectSize.ToString();
                    }

                    else
                    {
                        _MaxDefectSize = _RealWidth;
                        LeadTrimResult.EachLeadStatusArray[_LeadIndex].ShoulderNick = _MaxDefectSize.ToString();
                    }
                }
            }
        }

        public bool LeadTipInspection(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo, bool _InInspProcess = false)
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
                SetHardFixedThreshold(_CogLeadTrimAlgo.LeadTipThreshold);
                SetPolarity(Convert.ToBoolean(_CogLeadTrimAlgo.ShoulderForeground));

                CogRectangle _TipInspRegion = new CogRectangle();
                _TipInspRegion.SetCenterWidthHeight(_InspRegion.CenterX + LeadBodyOffsetX, _InspRegion.CenterY + LeadBodyOffsetY, _InspRegion.Width, _InspRegion.Height);
                _InspRegion = _TipInspRegion;
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
                _LeadCaliper.RunParams.ContrastThreshold = 5;
                _LeadCaliper.RunParams.FilterHalfSizeInPixels = 4;
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

                        LeadTipBurrCheck(_SrcImage, _LeadTipResult.LeadTipLeftArea[iLoopCount], _CogLeadTrimAlgo, iLoopCount);
                        LeadTipBurrCheck(_SrcImage, _LeadTipResult.LeadTipRightArea[iLoopCount], _CogLeadTrimAlgo, iLoopCount);

                        //if (LeadTrimResult.NgType != eNgType.GOOD)
                        //{
                        //    LeadTrimResult.IsGood = false;
                        //
                        //    //불량이 발생해도 계속 진행 하도록 설정 _Result = false -> true;
                        //    //_Result = false;
                        //    _Result = true;
                        //}

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

            if (false == _InInspProcess) _Result = true;
            return _Result;
        }

        private void LeadTipBurrCheck(CogImage8Grey _SrcImage, CogRectangleAffine _BurrArea, CogLeadTrimAlgo _CogLeadTrimAlgo, int _LeadIndex)
        {
            SetPolarity(false);
            SetConnectivityMinimum(10);
            SetHardFixedThreshold(_CogLeadTrimAlgo.LeadTipBurrThreshold);
            SetMorphology(CogBlobMorphologyConstants.ErodeHorizontal);
            SetMorphology(CogBlobMorphologyConstants.DilateHorizontal);

            CogBlobResults _BlobResults = BlobProc.Execute(_SrcImage, _BurrArea);
            CogLeadTrimLeadTipResult _LeadTipBurrResult = GetLeadTipResult(_BlobResults);

            if (0 == _LeadTipBurrResult.BlobCount) return;

            bool _IsCurrentGood = false;
            double _MaxDefectSize = 0;
            CogRectangle _DefectArea = new CogRectangle();
            for (int iLoopCount = 0; iLoopCount < _LeadTipBurrResult.BlobCount; ++iLoopCount)
            {
                double _RealWidth = _LeadTipBurrResult.Width[iLoopCount] * _CogLeadTrimAlgo.ResolutionX;
                double _RealHeight = _LeadTipBurrResult.Height[iLoopCount] * _CogLeadTrimAlgo.ResolutionY;

                _IsCurrentGood = true;
                if (_RealWidth > _CogLeadTrimAlgo.ShoulderNickSpec && _RealHeight > _CogLeadTrimAlgo.ShoulderNickSpec)
                {
                    _DefectArea = new CogRectangle();
                    _DefectArea.SetCenterWidthHeight(_LeadTipBurrResult.BlobCenterX[iLoopCount], _LeadTipBurrResult.BlobCenterY[iLoopCount], _LeadTipBurrResult.Width[iLoopCount], _LeadTipBurrResult.Height[iLoopCount]);

                    LeadTrimResult.LeadTipBurrDefectList.Add(_DefectArea);

                    LeadTrimResult.IsGood = false;
                    LeadTrimResult.EachLeadStatusArray[iLoopCount].SetSkewResult(eLeadStatus.TIP_BURR);

                    _IsCurrentGood = false;

                    //LJH 2019.07.26 : 전제 데이터를 다 남기기 위해 if 문 밖으로 이동
                    //if (_RealWidth > _RealHeight)   LeadTrimResult.EachLeadStatusArray[_LeadIndex].TipBurr = _RealWidth.ToString();
                    //else                            LeadTrimResult.EachLeadStatusArray[_LeadIndex].TipBurr = _RealHeight.ToString();
                }

                //LJH 2019.07.26 : 전제 데이터를 다 남기기 위해 if 문 밖으로 이동
                if ((_RealWidth > _RealHeight) && (_RealWidth > _MaxDefectSize))
                {
                    if (false == _IsCurrentGood)
                    {
                        _MaxDefectSize = _RealWidth;
                        LeadTrimResult.EachLeadStatusArray[_LeadIndex].TipBurr = _MaxDefectSize.ToString();
                    }

                    else
                    {
                        _MaxDefectSize = _RealHeight;
                        LeadTrimResult.EachLeadStatusArray[_LeadIndex].TipBurr = _MaxDefectSize.ToString();
                    }
                }

                else if ((_RealHeight >= _RealWidth) && (_RealHeight > _MaxDefectSize))
                {
                    if (false == _IsCurrentGood)
                    {
                        _MaxDefectSize = _RealHeight;
                        LeadTrimResult.EachLeadStatusArray[_LeadIndex].TipBurr = _MaxDefectSize.ToString();
                    }

                    else
                    {
                        _MaxDefectSize = _RealWidth;
                        LeadTrimResult.EachLeadStatusArray[_LeadIndex].TipBurr = _MaxDefectSize.ToString();
                    }
                }
            }
        }

        public bool GateReminingInspection(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadTrimAlgo _CogLeadTrimAlgo, bool _InInspProcess = false)
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

                CogRectangle _LeadInspRegion = new CogRectangle();
                _LeadInspRegion.SetCenterWidthHeight(_InspRegion.CenterX + LeadTrimResult.LeadBodyOffsetX, _InspRegion.CenterY + LeadTrimResult.LeadBodyOffsetY, _InspRegion.Width, _InspRegion.Height);
                _InspRegion = _LeadInspRegion;
                BlobResults = BlobProc.Execute(_SrcImage, _InspRegion);
                GetResult(true);

                bool _IsCurrentGood = true;
                double _MaxDefectSize = 0;
                CogLeadTrimGateRemainingResult _GateRemainingResult = GetGateRemainingResult(BlobResults);
                LeadTrimResult.GateRemainingNgList.Clear();
                for (int iLoopCount = 0; iLoopCount < _GateRemainingResult.BlobCount; ++iLoopCount)
                {
                    double _RealWidth = _GateRemainingResult.Width[iLoopCount] * _CogLeadTrimAlgo.ResolutionX;
                    double _RealHeight = _GateRemainingResult.Height[iLoopCount] * _CogLeadTrimAlgo.ResolutionY;

                    double _RealWidthSpec = _CogLeadTrimAlgo.GateRemainingSpec;
                    double _RealHeightSpec = _CogLeadTrimAlgo.GateRemainingSpec;

                    _IsCurrentGood = true;
                    if (_RealWidth > _RealWidthSpec && _RealHeight > _RealHeightSpec)
                    {
                        CogRectangle _DefectArea = new CogRectangle();
                        _DefectArea.SetXYWidthHeight(_GateRemainingResult.BlobMinX[iLoopCount], _GateRemainingResult.BlobMinY[iLoopCount], _GateRemainingResult.Width[iLoopCount], _GateRemainingResult.Height[iLoopCount]);

                        LeadTrimResult.NgType = eNgType.GATE_ERR;
                        LeadTrimResult.IsGood = false;
                        LeadTrimResult.GateRemainingNgList.Add(_DefectArea);

                        //LJH 2019.07.26 : 전제 데이터를 다 남기기 위해 if 문 밖으로 이동
                        //if (_ResultRealWidth > _ResultRealHeight) LeadTrimResult.GateRemainingStatus = _ResultRealWidth.ToString();
                        //else                                      LeadTrimResult.GateRemainingStatus = _ResultRealHeight.ToString();
                        _IsCurrentGood = false;

                        _Result = true;
                    }

                    //LJH 2019.07.26 : 전제 데이터를 다 남기기 위해 if 문 밖으로 이동
                    if ((_RealWidth > _RealHeight) && (_RealWidth > _MaxDefectSize))
                    {
                        if (false == _IsCurrentGood)
                        {
                            _MaxDefectSize = _RealWidth;
                            LeadTrimResult.GateRemainingStatus = _MaxDefectSize.ToString();
                        }

                        else
                        {
                            _MaxDefectSize = _RealHeight;
                            LeadTrimResult.GateRemainingStatus = _MaxDefectSize.ToString();
                        }
                    }

                    else if ((_RealHeight >= _RealWidth) && (_RealHeight > _MaxDefectSize))
                    {
                        if (false == _IsCurrentGood)
                        {
                            _MaxDefectSize = _RealHeight;
                            LeadTrimResult.GateRemainingStatus = _MaxDefectSize.ToString();
                        }

                        else
                        {
                            _MaxDefectSize = _RealWidth;
                            LeadTrimResult.GateRemainingStatus = _MaxDefectSize.ToString();
                        }
                    }
                    //if (_RealWidth > _RealHeight)   LeadTrimResult.GateRemainingStatus = _RealWidth.ToString();
                    //else                            LeadTrimResult.GateRemainingStatus = _RealHeight.ToString();
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
