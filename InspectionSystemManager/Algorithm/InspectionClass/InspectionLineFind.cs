using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.ImageProcessing;

using ParameterManager;
using LogMessageManager;

namespace InspectionSystemManager
{
    class InspectionLineFind
    {
        private CogFindLineTool     FindLineProc;
        private CogFindLineResults  FindLineResults;

        #region Initialize & Deinitialize
        public InspectionLineFind()
        {
            FindLineProc = new CogFindLineTool();
            FindLineResults = new CogFindLineResults();

            FindLineProc.RunParams.DecrementNumToIgnore = true;
        }

        public void Initialize()
        {

        }

        public void DeInitialize()
        {
            FindLineProc.Dispose();
        }
        #endregion Initialize & Deinitialize

        public bool Run(CogImage8Grey _SrcImage, ref CogImage8Grey _DestImage, CogRectangle _InspRegion, CogLineFindAlgo _CogLineFindAlgo, ref CogLineFindResult _CogLineFindResult, int _NgNumber = 0)
        {
            bool _Result = true;

            SetCaliperDirection(_CogLineFindAlgo.CaliperSearchDirection);
            SetCaliper(_CogLineFindAlgo.CaliperNumber, _CogLineFindAlgo.CaliperSearchLength, _CogLineFindAlgo.CaliperProjectionLength, _CogLineFindAlgo.IgnoreNumber);
            SetCaliperLine(_CogLineFindAlgo.CaliperLineStartX, _CogLineFindAlgo.CaliperLineStartY, _CogLineFindAlgo.CaliperLineEndX, _CogLineFindAlgo.CaliperLineEndY);

            if (true == Inspection(_SrcImage)) GetResult();
            if (FindLineResults != null && (_CogLineFindAlgo.CaliperNumber - _CogLineFindAlgo.IgnoreNumber) < (FindLineResults.NumPointsFound + 5))
            {
                try
                {
                    _CogLineFindResult.StartX = FindLineResults.GetLineSegment().StartX;
                    _CogLineFindResult.StartY = FindLineResults.GetLineSegment().StartY;
                    _CogLineFindResult.EndX = FindLineResults.GetLineSegment().EndX;
                    _CogLineFindResult.EndY = FindLineResults.GetLineSegment().EndY;
                    _CogLineFindResult.Length = FindLineResults.GetLineSegment().Length;
                    _CogLineFindResult.Rotation = FindLineResults.GetLineSegment().Rotation;
                    _CogLineFindResult.PointCount = FindLineResults.Count;

                    if (_CogLineFindAlgo.UseAlignment)
                    {
                        CogAffineTransformTool _CogTransForm = new CogAffineTransformTool();
                        CogRectangleAffine _AffineRegion = new CogRectangleAffine();
                        _AffineRegion.SetCenterLengthsRotationSkew(_InspRegion.CenterX, _InspRegion.CenterY, _InspRegion.Width, _InspRegion.Height, _CogLineFindResult.Rotation, 0);
                        _CogTransForm.InputImage = _SrcImage;
                        _CogTransForm.Region = _AffineRegion;
                        _CogTransForm.Run();

                        CogCopyRegionTool _CopyRegion = new CogCopyRegionTool();
                        _CopyRegion.InputImage = _CogTransForm.OutputImage;
                        _CopyRegion.DestinationImage = _SrcImage;
                        _CopyRegion.RunParams.ImageAlignmentEnabled = true;
                        _CopyRegion.Region = null;
                        _CopyRegion.Run();

                        _DestImage = (CogImage8Grey)_CopyRegion.OutputImage;

                        if (true == Inspection(_DestImage)) GetResult();
                        if (FindLineResults != null)
                        {
                            _CogLineFindResult.StartX = FindLineResults.GetLineSegment().StartX;
                            _CogLineFindResult.StartY = FindLineResults.GetLineSegment().StartY;
                            _CogLineFindResult.EndX = FindLineResults.GetLineSegment().EndX;
                            _CogLineFindResult.EndY = FindLineResults.GetLineSegment().EndY;
                            _CogLineFindResult.Length = FindLineResults.GetLineSegment().Length;
                            _CogLineFindResult.Rotation = FindLineResults.GetLineSegment().Rotation;
                            _CogLineFindResult.PointCount = FindLineResults.Count;

                            _CogLineFindResult.IsGood = true;
                        }

                        else
                        {
                            _CogLineFindResult.IsGood = false;
                        }

                        GC.Collect();
                    }

                    else
                    {
                        _CogLineFindResult.Rotation = FindLineResults.GetLineSegment().Rotation;
                        double _Rotation = 0;
                        _Rotation = _CogLineFindResult.Rotation * 180 / Math.PI;
                        _CogLineFindResult.IsGood = true;
                    }
                }

                catch
                {
                    _CogLineFindResult.IsGood = false;
                }
            }

            else
            {
                _CogLineFindResult.IsGood = false;
            }

            return _Result;
        }

        private bool Inspection(CogImage8Grey _SrcImage)
        {
            bool _Result = true;

            try
            {
                FindLineProc.InputImage = _SrcImage;
                FindLineProc.Run();
            }

            catch(System.Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "InspectionLineFind - Inspection Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
            }

            return _Result;
        }

        private void SetCaliperDirection(int _eSearchDir)
        {
            FindLineProc.RunParams.CaliperSearchDirection = _eSearchDir;
        }

        private void SetCaliper(int _CaliperNumber, double _SearchLength, double _ProjectionLength, int _CaliperIgnoreNumber)
        {
            FindLineProc.RunParams.NumCalipers = _CaliperNumber;
            FindLineProc.RunParams.CaliperSearchLength = _SearchLength;
            FindLineProc.RunParams.CaliperProjectionLength = _ProjectionLength;
            FindLineProc.RunParams.NumToIgnore = _CaliperIgnoreNumber;
            //FindLineProc.RunParams.CaliperSearchDirection = _SearchDir;
        }

        private void SetCaliperLine(double _StartX, double _StartY, double _EndX, double _EndY)
        {
            FindLineProc.RunParams.ExpectedLineSegment.SetStartEnd(_StartX, _StartY, _EndX, _EndY);
        }

        private void GetResult()
        {
            FindLineResults = FindLineProc.Results;
        }
    }
}
