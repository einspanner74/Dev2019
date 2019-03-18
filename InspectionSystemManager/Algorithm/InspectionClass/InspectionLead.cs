using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cognex.VisionPro;
using Cognex.VisionPro.Blob;

using ParameterManager;
using LogMessageManager;

namespace InspectionSystemManager
{
    class InspectionLead
    {
        private CogBlob         BlobProc;
        private CogBlobResults  BlobResults;
        private CogBlobResult   BlobResult;
        private CogLeadResult   InspResults;

        #region Initialize & Deinitialize
        public InspectionLead()
        {
            BlobProc = new CogBlob();
            BlobResults = new CogBlobResults();
            BlobResult = new CogBlobResult();
            InspResults = new CogLeadResult();

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
            BlobResults.Dispose();
            BlobResult.Dispose();
            BlobProc.Dispose();
        }
        #endregion Initialize & Deinitialize

        public bool Run(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogLeadAlgo _CogLeadAlgo, ref CogLeadResult _CogLeadResult, double _ReferX = -1, double _ReferY = -1)
        {
            bool _Result = true;

            SetHardFixedThreshold(_CogLeadAlgo.ThresholdMin);
            SetConnectivityMinimum((int)_CogLeadAlgo.BlobAreaMin);
            SetPolarity(Convert.ToBoolean(_CogLeadAlgo.ForeGround));

            if (true == Inspection(_SrcImage, _InspRegion)) GetResult(true);

            if (GetResults().BlobCount > 0)
            {
                _CogLeadResult = GetResults();
                _CogLeadResult.LeadCount = _CogLeadResult.BlobCount;

                #region Lead Pitch Point Get
                for (int iLoopCount = 0; iLoopCount < _CogLeadResult.BlobCount; ++iLoopCount)
                {
                    #region Lead Bent Check
                    double _Angle = _CogLeadResult.Angle[iLoopCount] * 180 / Math.PI;
                    if (_Angle > 0) _Angle = 90 - (_CogLeadResult.Angle[iLoopCount] * 180 / Math.PI);
                    else            _Angle = -(90 + (_CogLeadResult.Angle[iLoopCount] * 180 / Math.PI));

                    _CogLeadResult.IsLeadBentGood[iLoopCount] = true;
                    if (_CogLeadAlgo.IsLeadBentInspection)
                    {
                        if ((_Angle > _CogLeadAlgo.LeadBentMax) || (_Angle < -_CogLeadAlgo.LeadBentMin))
                        {
                            _CogLeadResult.IsLeadBentGood[iLoopCount] = false;
                            _CogLeadResult.IsGood &= _CogLeadResult.IsLeadBentGood[iLoopCount];
                        }
                    }
                    #endregion Lead Bent Check

                    #region Pitch Point 구하기
                    if (_CogLeadResult.Angle[iLoopCount] > 0)
                    {
                        CogLineSegment _CenterLine = new CogLineSegment();
                        _CenterLine.SetStartLengthRotation(_CogLeadResult.BlobCenterX[iLoopCount], _CogLeadResult.BlobCenterY[iLoopCount], _CogLeadResult.PrincipalWidth[iLoopCount] / 2, (Math.PI) + _CogLeadResult.Angle[iLoopCount]);
                        _CogLeadResult.LeadPitchTopX[iLoopCount] = _CenterLine.EndX;
                        _CogLeadResult.LeadPitchTopY[iLoopCount] = _CenterLine.EndY;

                        _CenterLine.SetStartLengthRotation(_CogLeadResult.BlobCenterX[iLoopCount], _CogLeadResult.BlobCenterY[iLoopCount], _CogLeadResult.PrincipalWidth[iLoopCount] / 2, _CogLeadResult.Angle[iLoopCount]);
                        _CogLeadResult.LeadPitchBottomX[iLoopCount] = _CenterLine.EndX;
                        _CogLeadResult.LeadPitchBottomY[iLoopCount] = _CenterLine.EndY;
                    }

                    else
                    {
                        CogLineSegment _CenterLine = new CogLineSegment();
                        _CenterLine.SetStartLengthRotation(_CogLeadResult.BlobCenterX[iLoopCount], _CogLeadResult.BlobCenterY[iLoopCount], _CogLeadResult.PrincipalWidth[iLoopCount] / 2, _CogLeadResult.Angle[iLoopCount]);
                        _CogLeadResult.LeadPitchTopX[iLoopCount] = _CenterLine.EndX;
                        _CogLeadResult.LeadPitchTopY[iLoopCount] = _CenterLine.EndY;

                        _CenterLine.SetStartLengthRotation(_CogLeadResult.BlobCenterX[iLoopCount], _CogLeadResult.BlobCenterY[iLoopCount], _CogLeadResult.PrincipalWidth[iLoopCount] / 2, (Math.PI) + _CogLeadResult.Angle[iLoopCount]);
                        _CogLeadResult.LeadPitchBottomX[iLoopCount] = _CenterLine.EndX;
                        _CogLeadResult.LeadPitchBottomY[iLoopCount] = _CenterLine.EndY;
                    }
                    #endregion Pitch Point 구하기

                    #region Length 구하기
                    if (_ReferY != -1 && _ReferY != -1)
                    {
                        _CogLeadResult.LeadLength[iLoopCount] = Math.Abs(_ReferY - _CogLeadResult.LeadPitchTopY[iLoopCount]);
                        _CogLeadResult.LeadLengthStartX[iLoopCount] = _CogLeadResult.LeadPitchTopX[iLoopCount];
                        _CogLeadResult.LeadLengthStartY[iLoopCount] = _ReferY;
                    }
                    #endregion Length 구하기
                }
                #endregion Lead Pitch Point Get

                #region Lead Length Get
                #endregion Lead Length Get

                _CogLeadResult.IsGood &= true;
            }

            else
            {
                _CogLeadResult.IsGood = false;
            }

            return _Result;
        }

        private bool Inspection(CogImage8Grey _SrcImage, CogRectangle _InspArea)
        {
            bool _Result = true;

            try
            {
                BlobResults = BlobProc.Execute(_SrcImage, _InspArea);
            }
            catch(System.Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "InspectionLead - Inspection Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);

                _Result = false;
            }

            return _Result;
        }

        private CogLeadResult GetResults()
        {
            return InspResults;
        }

        private CogCompositeShape[] GetGraphicResult()
        {
            CogCompositeShape[] _GraphicResult = new CogCompositeShape[BlobResults.GetBlobs().Count];
            CogBlobResult _BlobResult = new CogBlobResult();

            for (int iLoopCount = 0; iLoopCount < BlobResults.GetBlobs().Count; ++iLoopCount)
            {
                BlobResult = BlobResults.GetBlobByID(iLoopCount);
                _GraphicResult[iLoopCount] = BlobResult.CreateResultGraphics(CogBlobResultGraphicConstants.Boundary);
            }

            return _GraphicResult;
        }

        private bool GetResult(bool _IsGraphicResult = false)
        {
            bool _Result = true;

            if (null == BlobResult || BlobResults.GetBlobs().Count < 0) return false;
            InspResults.IsGood = true;
            InspResults.BlobCount = BlobResults.GetBlobs().Count;
            InspResults.BlobArea = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobCenterX = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobCenterY = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobMinX = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobMinY = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobMaxX = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobMaxY = new double[BlobResults.GetBlobs().Count];
            InspResults.Width = new double[BlobResults.GetBlobs().Count];
            InspResults.Height = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobMessCenterX = new double[BlobResults.GetBlobs().Count];
            InspResults.BlobMessCenterY = new double[BlobResults.GetBlobs().Count];
            InspResults.PrincipalWidth = new double[BlobResults.GetBlobs().Count];
            InspResults.PrincipalHeight = new double[BlobResults.GetBlobs().Count];
            InspResults.Angle = new double[BlobResults.GetBlobs().Count];
            InspResults.Degree = new double[BlobResults.GetBlobs().Count];

            InspResults.IsLeadBentGood = new bool[BlobResults.GetBlobs().Count];
            InspResults.LeadPitchTopX = new double[BlobResults.GetBlobs().Count];
            InspResults.LeadPitchTopY = new double[BlobResults.GetBlobs().Count];
            InspResults.LeadPitchBottomX = new double[BlobResults.GetBlobs().Count];
            InspResults.LeadPitchBottomY = new double[BlobResults.GetBlobs().Count];

            InspResults.LeadLength = new double[BlobResults.GetBlobs().Count];
            InspResults.LeadLengthStartX = new double[BlobResults.GetBlobs().Count];
            InspResults.LeadLengthStartY = new double[BlobResults.GetBlobs().Count];

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
                InspResults.PrincipalWidth[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPrincipalAxisWidth, iLoopCount);
                InspResults.PrincipalHeight[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.BoundingBoxPrincipalAxisHeight, iLoopCount);
                InspResults.Angle[iLoopCount] = BlobResults.GetBlobMeasure(CogBlobMeasureConstants.Angle, iLoopCount);
                InspResults.Degree[iLoopCount] = InspResults.Angle[iLoopCount] * 180 / Math.PI;
                if (_IsGraphicResult) InspResults.ResultGraphic[iLoopCount] = BlobResult.CreateResultGraphics(CogBlobResultGraphicConstants.Boundary);
            }

            return _Result;
        }

        private void SetPolarity(bool _IsMode)
        {
            if (_IsMode) BlobProc.SegmentationParams.Polarity = CogBlobSegmentationPolarityConstants.LightBlobs;
            else BlobProc.SegmentationParams.Polarity = CogBlobSegmentationPolarityConstants.DarkBlobs;
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

        private void SetMorphology(eMorphologyMode _OperationMode)
        {
            switch (_OperationMode)
            {
                case eMorphologyMode.OPEN: BlobProc.MorphologyOperations.Add(CogBlobMorphologyConstants.OpenSquare); break;
                case eMorphologyMode.CLOSE: BlobProc.MorphologyOperations.Add(CogBlobMorphologyConstants.CloseSquare); break;
                case eMorphologyMode.DILATE: BlobProc.MorphologyOperations.Add(CogBlobMorphologyConstants.DilateHorizontal); break;
                case eMorphologyMode.ERODE: BlobProc.MorphologyOperations.Add(CogBlobMorphologyConstants.ErodeHorizontal); break;
            }
        }

        private void SetMorphologyHorizon(int _Time)
        {
            for (int iLoopCount = 0; iLoopCount < _Time; ++iLoopCount)
                BlobProc.MorphologyOperations.Add(CogBlobMorphologyConstants.ErodeHorizontal);
            for (int iLoopCount = 0; iLoopCount < _Time; ++iLoopCount)
                BlobProc.MorphologyOperations.Add(CogBlobMorphologyConstants.DilateHorizontal);
        }

        private void SetMorphologyVertical(int _Time)
        {
            for (int iLoopCount = 0; iLoopCount < _Time; ++iLoopCount)
                BlobProc.MorphologyOperations.Add(CogBlobMorphologyConstants.ErodeVertical);
            for (int iLoopCount = 0; iLoopCount < _Time; ++iLoopCount)
                BlobProc.MorphologyOperations.Add(CogBlobMorphologyConstants.DilateVertical);
        }

        private void SetMorphologySquare(int _Time)
        {
            for (int iLoopCount = 0; iLoopCount < _Time; ++iLoopCount)
                BlobProc.MorphologyOperations.Add(CogBlobMorphologyConstants.ErodeSquare);
            for (int iLoopCount = 0; iLoopCount < _Time; ++iLoopCount)
                BlobProc.MorphologyOperations.Add(CogBlobMorphologyConstants.DilateSquare);
        }
    }
}
