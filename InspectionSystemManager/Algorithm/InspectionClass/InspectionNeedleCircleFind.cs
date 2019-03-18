using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cognex.VisionPro;
using Cognex.VisionPro.Caliper;

using ParameterManager;
using LogMessageManager;

namespace InspectionSystemManager
{
    class InspectionNeedleCircleFind
    {
        private CogFindCircleTool       FindCircleProc;
        private CogFindCircleResults    FindCircleResults;

        private double CircleCenterOffsetX;
        private double CircleCenterOffsetY;

        #region Initialize & Deinitialize
        public InspectionNeedleCircleFind()
        {
            FindCircleProc = new CogFindCircleTool();
            FindCircleResults = new CogFindCircleResults();
        }

        public void Initialize()
        {

        }

        public void DeInitialize()
        {
            FindCircleProc.Dispose();
        }
        #endregion Initialize & Deinitialize

        //public void SetOffsetValue(double _OffsetX, double _OffsetY)
        //{
        //    CircleCenterOffsetX = _OffsetX;
        //    CircleCenterOffsetY = _OffsetY;
        //}

        public bool Run(CogImage8Grey _SrcImage, CogNeedleFindAlgo _CogNeedleFindAlgo, ref CogNeedleFindResult _CogNeedleFindResult, double _OffsetX = 0, double _OffsetY = 0, int _NgNumber = 0)
        {
            bool _Result = true;

            SetCaliperDirection(_CogNeedleFindAlgo.CaliperSearchDirection, _CogNeedleFindAlgo.CaliperPolarity);
            SetCaliper(_CogNeedleFindAlgo.CaliperNumber, _CogNeedleFindAlgo.CaliperSearchLength, _CogNeedleFindAlgo.CaliperProjectionLength, _CogNeedleFindAlgo.CaliperIgnoreNumber);
            SetCircularArc(_CogNeedleFindAlgo.ArcCenterX - _OffsetX, _CogNeedleFindAlgo.ArcCenterY - _OffsetY, _CogNeedleFindAlgo.ArcRadius, _CogNeedleFindAlgo.ArcAngleStart, _CogNeedleFindAlgo.ArcAngleSpan);

            if (true == Inspection(_SrcImage)) GetResult();

            if (FindCircleResults != null && FindCircleResults.Count > 0)   _CogNeedleFindResult.IsGood = true;
            else                                                            _CogNeedleFindResult.IsGood = false;

            if (!_CogNeedleFindResult.IsGood)
            {
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, " - Needle Find Fail!!", CLogManager.LOG_LEVEL.MID);
                _CogNeedleFindResult.CenterX = _CogNeedleFindAlgo.ArcCenterX;
                _CogNeedleFindResult.CenterY = _CogNeedleFindAlgo.ArcCenterY;
                _CogNeedleFindResult.Radius = _CogNeedleFindAlgo.ArcRadius;
                _CogNeedleFindResult.OriginX = 0;
                _CogNeedleFindResult.OriginY = 0;
            }

            else
            {
                if (FindCircleResults.GetCircle() != null)
                {
                    CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, " - Needle Find Complete", CLogManager.LOG_LEVEL.MID);

                    _CogNeedleFindResult.PointFoundCount = FindCircleResults.NumPointsFound;
                    _CogNeedleFindResult.CenterX = FindCircleResults.GetCircle().CenterX;
                    _CogNeedleFindResult.CenterY = FindCircleResults.GetCircle().CenterY;
                    _CogNeedleFindResult.Radius = FindCircleResults.GetCircle().Radius;
                    _CogNeedleFindResult.OriginX = FindCircleResults.GetCircle().CenterX;
                    _CogNeedleFindResult.OriginY = FindCircleResults.GetCircle().CenterY;

                    _CogNeedleFindResult.PointPosXInfo = new double[FindCircleResults.Count];
                    _CogNeedleFindResult.PointPosYInfo = new double[FindCircleResults.Count];
                    _CogNeedleFindResult.PointStatusInfo = new bool[FindCircleResults.Count];
                    for (int iLoopCount = 0; iLoopCount < FindCircleResults.Count; ++iLoopCount)
                    {
                        if (true == FindCircleResults[iLoopCount].Found)
                        {
                            _CogNeedleFindResult.PointPosXInfo[iLoopCount] = FindCircleResults[iLoopCount].X;
                            _CogNeedleFindResult.PointPosYInfo[iLoopCount] = FindCircleResults[iLoopCount].Y;
                        }
                        _CogNeedleFindResult.PointStatusInfo[iLoopCount] = FindCircleResults[iLoopCount].Used;
                    }

                    CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, String.Format(" - Center X : {0}, Y : {1}", _CogNeedleFindResult.CenterX.ToString("F2"), _CogNeedleFindResult.CenterY.ToString("F2")), CLogManager.LOG_LEVEL.MID);
                    CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, String.Format(" - Radius : {0}", _CogNeedleFindResult.Radius.ToString("F2")), CLogManager.LOG_LEVEL.MID);
                }

                else
                {
                    CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, " - Needle Find Fail!!", CLogManager.LOG_LEVEL.MID);

                    _CogNeedleFindResult.CenterX = 0;
                    _CogNeedleFindResult.CenterY = 0;
                    _CogNeedleFindResult.Radius = 0;
                    _CogNeedleFindResult.OriginX = 0;
                    _CogNeedleFindResult.OriginY = 0;
                    _CogNeedleFindResult.IsGood = false;
                }
            }

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, " - Result : " + _CogNeedleFindResult.IsGood.ToString(), CLogManager.LOG_LEVEL.MID);

            return _Result;
        }

        private bool Inspection(CogImage8Grey _SrcImage)
        {
            bool _Result = true;

            try
            {
                FindCircleProc.InputImage = _SrcImage;
                FindCircleProc.Run();
            }

            catch(System.Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "InspectionNeedleCircleFind - Inspection Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
                _Result = false;
            }

            return _Result;
        }

        private void SetCaliper(int _CaliperNumber, double _SearchLength, double _ProjectionLength, int _CaliperIgnoreNumber)
        {
            FindCircleProc.RunParams.NumCalipers = _CaliperNumber;
            FindCircleProc.RunParams.NumToIgnore = _CaliperIgnoreNumber;
            FindCircleProc.RunParams.CaliperSearchLength = _SearchLength;
            FindCircleProc.RunParams.CaliperProjectionLength = _ProjectionLength;
        }

        private void SetCaliperDirection(int _eSearchDir, int _eEdgePolarity = (int)CogCaliperPolarityConstants.DarkToLight)
        {
            FindCircleProc.RunParams.CaliperSearchDirection = (CogFindCircleSearchDirectionConstants)_eSearchDir;
            FindCircleProc.RunParams.CaliperRunParams.Edge0Polarity = (CogCaliperPolarityConstants)_eEdgePolarity;
        }

        private void SetCircularArc(double _CenterX, double _CenterY, double _Radius, double _AngleStart, double _AngleSpan)
        {
            FindCircleProc.RunParams.ExpectedCircularArc.CenterX = _CenterX;
            FindCircleProc.RunParams.ExpectedCircularArc.CenterY = _CenterY;
            FindCircleProc.RunParams.ExpectedCircularArc.Radius = _Radius;
            FindCircleProc.RunParams.ExpectedCircularArc.AngleStart = _AngleStart;
            FindCircleProc.RunParams.ExpectedCircularArc.AngleSpan = _AngleSpan;
        }

        private void GetResult()
        {
            FindCircleResults = FindCircleProc.Results;
        }
    }
}
