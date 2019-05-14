using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Cognex.VisionPro;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.Dimensioning;

using ParameterManager;
using LogMessageManager;

namespace InspectionSystemManager
{
    public partial class TeachingWindow
    {
        #region InitializeEvent & DeInitializeEvent
        private void InitializeEvent()
        {
            ucCogPatternWnd.DrawReferRegionEvent += new ucCogPattern.DrawReferRegionHandler(DrawReferRegionFunction);
            ucCogPatternWnd.ReferenceActionEvent += new ucCogPattern.ReferenceActionHandler(ReferenceActionFunction);
            ucCogPatternWnd.ApplyPatternMatchingValueEvent += new ucCogPattern.ApplyPatternMatchingValueHandler(ApplyPatternMatchingValueFunction);
            ucCogBlobReferWnd.ApplyBlobReferValueEvent += new ucCogBlobReference.ApplyBlobReferValueHandler(ApplyBlobReferenceValueFunction);
            ucCogBlobReferWnd.GetHistogramValueEvent += new ucCogBlobReference.GetHistogramValueHandler(GetHistogramValueFunction);
            ucCogNeedleFindWnd.ApplyNeedleCircleFindValueEvent += new ucCogNeedleCircleFind.ApplyNeedleCircleFindValueHandler(ApplyNeedleCircleFindValueFunction);
            ucCogNeedleFindWnd.DrawNeedleCircleFindCaliperEvent += new ucCogNeedleCircleFind.DrawNeedleCircleFindCaliperHandler(DrawNeedleCircleFindCaliperFunction);
            ucCogEllipseFindWnd.ApplyEllipseValueEvent += new ucCogEllipseFind.ApplyEllipseValueHandler(ApplyEllipseValueFunction);
            ucCogEllipseFindWnd.DrawEllipseCaliperEvent += new ucCogEllipseFind.DrawEllipseCaliperHandler(DrawEllipseCaliperFunction);
            ucCogLeadInspWnd.ApplyLeadInspValueEvent += new ucCogLeadInspection.ApplyLeadInspValueHandler(ApplyLeadInspValueFunction);
            ucCogIDInspWnd.ApplyBarCodeIDInspValueEvent += new ucCogID.ApplyBarCodeIDInspValueHandler(ApplyBarCodeIDInspValueFunction);
            ucCogLineFindWnd.ApplyLineFindEvent += new ucCogLineFind.ApplyLineFindHandler(ApplyLineFindValueFunction);
            ucCogLineFindWnd.DrawLineFindCaliperEvent += new ucCogLineFind.DrawLineFindCaliperHandler(DrawLineFindCaliperFunction);
            ucCogLineFindWnd.CheckCaliperStatusEvent += new ucCogLineFind.CheckCaliperStatusHandler(CheckCaliperStatusFunction);
            kpTeachDisplay.CogDisplayMouseUpEvent += new KPDisplay.KPCogDisplayControl.CogDisplayMouseUpHandler(TeachDisplayMouseUpEvent);
            ucCogMultiPatternWnd.DrawReferRegionEvent += new ucCogMultiPattern.DrawReferRegionHandler(DrawReferRegionFunction);
            ucCogMultiPatternWnd.ReferenceActionEvent += new ucCogMultiPattern.ReferenceActionHandler(ReferenceActionFunction);
            ucCogMultiPatternWnd.ApplyMultiPatternValueEvent += new ucCogMultiPattern.ApplyMultiPatternValueHandler(ApplyMultiPatternValueFunction);
            ucCogAutoPatternWnd.DrawReferRegionEvent += new ucCogAutoPattern.DrawReferRegionHandler(DrawReferRegionFunction);
            ucCogAutoPatternWnd.ReferenceActionEvent += new ucCogAutoPattern.ReferenceActionHandler(ReferenceActionFunction);
            ucCogAutoPatternWnd.ApplyAutoPatternFindValueEvent += new ucCogAutoPattern.ApplyAutoPatternFindValueHandler(ApplyAutoPatternFindValueFunction);

            ucCogLeadTrimInspWnd.GetRegionEvent += new ucCogLeadTrimInspection.GetRegionHandler(GetRegionFunction);
            ucCogLeadTrimInspWnd.DrawRegionEvent += new ucCogLeadTrimInspection.DrawRegionHandler(DrawRegionFunction);
            ucCogLeadTrimInspWnd.DrawMaskingRegionEven += new ucCogLeadTrimInspection.DrawMaskingRegionHandler(DrawMaskingRegionFunction);
            ucCogLeadTrimInspWnd.ApplyLeadTrimValueEvent += new ucCogLeadTrimInspection.ApplyLeadTrimValueHandler(ApplyLeadTrimValueFunction);
        }

        private void DeInitializeEvent()
        {
            ucCogPatternWnd.DrawReferRegionEvent -= new ucCogPattern.DrawReferRegionHandler(DrawReferRegionFunction);
            ucCogPatternWnd.ReferenceActionEvent -= new ucCogPattern.ReferenceActionHandler(ReferenceActionFunction);
            ucCogPatternWnd.ApplyPatternMatchingValueEvent -= new ucCogPattern.ApplyPatternMatchingValueHandler(ApplyPatternMatchingValueFunction);
            ucCogBlobReferWnd.ApplyBlobReferValueEvent -= new ucCogBlobReference.ApplyBlobReferValueHandler(ApplyBlobReferenceValueFunction);
            ucCogBlobReferWnd.GetHistogramValueEvent -= new ucCogBlobReference.GetHistogramValueHandler(GetHistogramValueFunction);
            ucCogNeedleFindWnd.ApplyNeedleCircleFindValueEvent -= new ucCogNeedleCircleFind.ApplyNeedleCircleFindValueHandler(ApplyNeedleCircleFindValueFunction);
            ucCogNeedleFindWnd.DrawNeedleCircleFindCaliperEvent -= new ucCogNeedleCircleFind.DrawNeedleCircleFindCaliperHandler(DrawNeedleCircleFindCaliperFunction);
            ucCogEllipseFindWnd.ApplyEllipseValueEvent -= new ucCogEllipseFind.ApplyEllipseValueHandler(ApplyEllipseValueFunction);
            ucCogEllipseFindWnd.DrawEllipseCaliperEvent -= new ucCogEllipseFind.DrawEllipseCaliperHandler(DrawEllipseCaliperFunction);
            ucCogLeadInspWnd.ApplyLeadInspValueEvent -= new ucCogLeadInspection.ApplyLeadInspValueHandler(ApplyLeadInspValueFunction);
            ucCogIDInspWnd.ApplyBarCodeIDInspValueEvent -= new ucCogID.ApplyBarCodeIDInspValueHandler(ApplyBarCodeIDInspValueFunction);
            ucCogLineFindWnd.ApplyLineFindEvent -= new ucCogLineFind.ApplyLineFindHandler(ApplyLineFindValueFunction);
            ucCogLineFindWnd.DrawLineFindCaliperEvent -= new ucCogLineFind.DrawLineFindCaliperHandler(DrawLineFindCaliperFunction);
            ucCogLineFindWnd.CheckCaliperStatusEvent -= new ucCogLineFind.CheckCaliperStatusHandler(CheckCaliperStatusFunction);
            kpTeachDisplay.CogDisplayMouseUpEvent -= new KPDisplay.KPCogDisplayControl.CogDisplayMouseUpHandler(TeachDisplayMouseUpEvent);
            ucCogMultiPatternWnd.DrawReferRegionEvent -= new ucCogMultiPattern.DrawReferRegionHandler(DrawReferRegionFunction);
            ucCogMultiPatternWnd.ReferenceActionEvent -= new ucCogMultiPattern.ReferenceActionHandler(ReferenceActionFunction);
            ucCogMultiPatternWnd.ApplyMultiPatternValueEvent -= new ucCogMultiPattern.ApplyMultiPatternValueHandler(ApplyMultiPatternValueFunction);
            ucCogAutoPatternWnd.DrawReferRegionEvent -= new ucCogAutoPattern.DrawReferRegionHandler(DrawReferRegionFunction);
            ucCogAutoPatternWnd.ReferenceActionEvent -= new ucCogAutoPattern.ReferenceActionHandler(ReferenceActionFunction);
            ucCogAutoPatternWnd.ApplyAutoPatternFindValueEvent -= new ucCogAutoPattern.ApplyAutoPatternFindValueHandler(ApplyAutoPatternFindValueFunction);

            ucCogLeadTrimInspWnd.GetRegionEvent -= new ucCogLeadTrimInspection.GetRegionHandler(GetRegionFunction);
            ucCogLeadTrimInspWnd.DrawRegionEvent -= new ucCogLeadTrimInspection.DrawRegionHandler(DrawRegionFunction);
            ucCogLeadTrimInspWnd.DrawMaskingRegionEven -= new ucCogLeadTrimInspection.DrawMaskingRegionHandler(DrawMaskingRegionFunction);
            ucCogLeadTrimInspWnd.ApplyLeadTrimValueEvent -= new ucCogLeadTrimInspection.ApplyLeadTrimValueHandler(ApplyLeadTrimValueFunction);
        }
        #endregion InitializeEvent & DeInitializeEvent

        #region KPCogDisplay Control Event : KPCogDisplayControl -> TeachingWindow
        private void TeachDisplayMouseUpEvent(object _CaliperTool)
        {
            if (CurrentAlgoType != eAlgoType.C_NEEDLE_FIND && CurrentAlgoType != eAlgoType.C_LINE_FIND && CurrentAlgoType != eAlgoType.C_ELLIPSE) return;
            if (CurrentTeachStep != eTeachStep.ALGO_SET)    return;

            if (CurrentAlgoType == eAlgoType.C_NEEDLE_FIND)
            {
                CogFindCircleTool _CircleCaliperTool = _CaliperTool as CogFindCircleTool;

                double _CenterX = 0, _CenterY = 0, _Radius = 0, _AngleStart = 0, _AngleSpan = 0;
                _CircleCaliperTool.RunParams.ExpectedCircularArc.GetCenterRadiusAngleStartAngleSpan(out _CenterX, out _CenterY, out _Radius, out _AngleStart, out _AngleSpan);

                int _CaliperNumber = 0;
                double _CaliperSearchLength = 0, _CaliperProjectionLength = 0;
                eSearchDirection _CaliperSearchDir = eSearchDirection.IN_WARD;
                ePolarity _CaliperPolarity = ePolarity.DARK_TO_LIGHT;
                _CaliperNumber = _CircleCaliperTool.RunParams.NumCalipers;
                _CaliperSearchLength = _CircleCaliperTool.RunParams.CaliperSearchLength;
                _CaliperProjectionLength = _CircleCaliperTool.RunParams.CaliperProjectionLength;
                _CaliperSearchDir = (eSearchDirection)_CircleCaliperTool.RunParams.CaliperSearchDirection;
                _CaliperPolarity = (ePolarity)_CircleCaliperTool.RunParams.CaliperRunParams.Edge0Polarity;

                ucCogNeedleFindWnd.SetCaliper(_CaliperNumber, _CaliperSearchLength, _CaliperProjectionLength, _CaliperSearchDir, _CaliperPolarity);
                ucCogNeedleFindWnd.SetCircularArc(_CenterX, _CenterY, _Radius, _AngleStart, _AngleSpan);
            }

            else if (CurrentAlgoType == eAlgoType.C_ELLIPSE)
            {
                CogFindEllipseTool _EllipseCaliperTool = _CaliperTool as CogFindEllipseTool;

                double _CenterX = 0, _CenterY = 0, _RadiusX = 0, _RadiusY = 0, _Rotation = 0, _AngleStart = 0, _AngleSpan = 0;
                _EllipseCaliperTool.RunParams.ExpectedEllipticalArc.GetCenterRadiusXYRotationAngleStartAngleSpan(out _CenterX, out _CenterY, out _RadiusX, out _RadiusY, out _Rotation, out _AngleStart, out _AngleSpan);

                int _CaliperNumber = 0;
                double _CaliperSearchLength = 0, _CaliperProjectionLength = 0;
                eSearchDirection _CaliperSearchDir = eSearchDirection.IN_WARD;
                ePolarity _CaliperPolarity = ePolarity.DARK_TO_LIGHT;
                _CaliperNumber = _EllipseCaliperTool.RunParams.NumCalipers;
                _CaliperSearchLength = _EllipseCaliperTool.RunParams.CaliperSearchLength;
                _CaliperProjectionLength = _EllipseCaliperTool.RunParams.CaliperProjectionLength;
                _CaliperSearchDir = (eSearchDirection)_EllipseCaliperTool.RunParams.CaliperSearchDirection;
                _CaliperPolarity = (ePolarity)_EllipseCaliperTool.RunParams.CaliperRunParams.Edge0Polarity;

                ucCogEllipseFindWnd.SetCaliper(_CaliperNumber, _CaliperSearchLength, _CaliperProjectionLength, _CaliperSearchDir, _CaliperPolarity);
                ucCogEllipseFindWnd.SetEllipticalArc(_CenterX, _CenterY, _RadiusX, _RadiusY, _AngleSpan);
            }

            else if (CurrentAlgoType == eAlgoType.C_LINE_FIND)
            {
                CogFindLineTool _FindLineTool = _CaliperTool as CogFindLineTool;

                double _StartX = 0, _StartY = 0, _EndX = 0, _EndY = 0;
                _FindLineTool.RunParams.ExpectedLineSegment.GetStartEnd(out _StartX, out _StartY, out _EndX, out _EndY);

                int _CaliperNumber = 0;
                double _CaliperSearchLength = 0, _CaliperProjectionLength = 0, _CaliperDirection;
                _CaliperNumber = _FindLineTool.RunParams.NumCalipers;
                _CaliperSearchLength = _FindLineTool.RunParams.CaliperSearchLength;
                _CaliperProjectionLength = _FindLineTool.RunParams.CaliperProjectionLength;
                _CaliperDirection = _FindLineTool.RunParams.CaliperSearchDirection;

                ucCogLineFindWnd.SetCaliper(_CaliperNumber, _CaliperSearchLength, _CaliperProjectionLength, _CaliperDirection);
                ucCogLineFindWnd.SetCaliperLine(_StartX, _StartY, _EndX, _EndY);
            }
        }
        #endregion KPCogDisplay Control Event : KPCogDisplayControl -> TeachingWindow

        #region Pattern Matching Window Event : ucCogPatternWindow -> TeachingWindow
        private void DrawReferRegionFunction(CogRectangle _ReferRegion, double _OriginX, double _OriginY, CogColorConstants _Color)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }
            AlgorithmAreaDisplayRefresh();

            CogPointMarker _PointMarker = new CogPointMarker();
            _PointMarker.SetCenterRotationSize(_OriginX, _OriginY, 0, 1);

            kpTeachDisplay.DrawInterActiveShape(_ReferRegion, "ReferRegion", _Color);
            kpTeachDisplay.DrawInterActiveShape(_PointMarker, "ReferOriginPoint", _Color, 100);
        }

        private void ReferenceActionFunction(eReferAction _ReferAction, int _Index = 0, bool _MultiFlag = false)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }

            if (_ReferAction == eReferAction.ADD)
            {
                CogPointMarker _PointMark = new CogPointMarker();
                int _Pixel;
                double _PointCenterX, _PointCenterY, _Rotate;
                _PointMark = kpTeachDisplay.GetInterActivePoint();
                _PointMark.GetCenterRotationSize(out _PointCenterX, out _PointCenterY, out _Rotate, out _Pixel);

                CogRectangle _ReferRegion = new CogRectangle();
                CogRectangle _Boundary = new CogRectangle();
                _Boundary.SetXYWidthHeight(AlgoRegionRectangle.X, AlgoRegionRectangle.Y, AlgoRegionRectangle.Width, AlgoRegionRectangle.Height);
                if (false == GetCorrectionRectangle(kpTeachDisplay, _Boundary, ref _ReferRegion)) { MessageBox.Show("The rectangle is outside the inspection area."); return; }

                double _OriginPointOffsetX = _ReferRegion.CenterX - _PointCenterX;
                double _OriginPointOffsetY = _ReferRegion.CenterY - _PointCenterY;

                DrawReferRegionFunction(_ReferRegion, _PointMark.X, _PointMark.Y, CogColorConstants.Cyan);

                //Pattern 추출
                ReferenceInformation _PatternInfo = new ReferenceInformation();
                _PatternInfo.StaticStartX = _ReferRegion.X;
                _PatternInfo.StaticStartY = _ReferRegion.Y;
                _PatternInfo.CenterX = _ReferRegion.CenterX;
                _PatternInfo.CenterY = _ReferRegion.CenterY;
                _PatternInfo.Width = _ReferRegion.Width;
                _PatternInfo.Height = _ReferRegion.Height;
                _PatternInfo.OriginPointOffsetX = _OriginPointOffsetX;
                _PatternInfo.OriginPointOffsetY = _OriginPointOffsetY;
                bool PatternResult = InspPatternProcess.GetPatternReference(InspectionImage, _ReferRegion, _PointCenterX, _PointCenterY, ref _PatternInfo.Reference);

                if (PatternResult)
                {
                    if (!_MultiFlag) ((CogPatternAlgo)InspParam.InspAreaParam[InspAreaSelected].InspAlgoParam[InspAlgoSelected].Algorithm).ReferenceInfoList.Add(_PatternInfo);
                    else ((CogMultiPatternAlgo)InspParam.InspAreaParam[InspAreaSelected].InspAlgoParam[InspAlgoSelected].Algorithm).ReferenceInfoList.Add(_PatternInfo);
                }
                else MessageBox.Show("패턴을 등록할 수 없습니다.");
            }

            else if (_ReferAction == eReferAction.MODIFY)
            {
                CogPointMarker _PointMark = new CogPointMarker();
                int _Pixel;
                double _PointCenterX, _PointCenterY, _Rotate;
                _PointMark = kpTeachDisplay.GetInterActivePoint();
                _PointMark.GetCenterRotationSize(out _PointCenterX, out _PointCenterY, out _Rotate, out _Pixel);

                CogRectangle _ReferRegion = new CogRectangle();
                CogRectangle _Boundary = new CogRectangle();
                _Boundary.SetXYWidthHeight(AlgoRegionRectangle.X, AlgoRegionRectangle.Y, AlgoRegionRectangle.Width, AlgoRegionRectangle.Height);
                if (false == GetCorrectionRectangle(kpTeachDisplay, _Boundary, ref _ReferRegion)) { MessageBox.Show("The rectangle is outside the inspection area."); return; }

                double _OriginPointOffsetX = _ReferRegion.CenterX - _PointCenterX;
                double _OriginPointOffsetY = _ReferRegion.CenterY - _PointCenterY;

                DrawReferRegionFunction(_ReferRegion, _PointMark.X, _PointMark.Y, CogColorConstants.Cyan);

                //Pattern 추출
                ReferenceInformation _PatternInfo = new ReferenceInformation();
                _PatternInfo.StaticStartX = _ReferRegion.X;
                _PatternInfo.StaticStartY = _ReferRegion.Y;
                _PatternInfo.CenterX = _ReferRegion.CenterX;
                _PatternInfo.CenterY = _ReferRegion.CenterY;
                _PatternInfo.Width = _ReferRegion.Width;
                _PatternInfo.Height = _ReferRegion.Height;
                _PatternInfo.OriginPointOffsetX = _OriginPointOffsetX;
                _PatternInfo.OriginPointOffsetY = _OriginPointOffsetY;
                bool PatternResult = InspPatternProcess.GetPatternReference(InspectionImage, _ReferRegion, _PointCenterX, _PointCenterY, ref _PatternInfo.Reference);

                if (PatternResult)
                {
                    if (!_MultiFlag) ((CogPatternAlgo)InspParam.InspAreaParam[InspAreaSelected].InspAlgoParam[InspAlgoSelected].Algorithm).ReferenceInfoList[_Index] = _PatternInfo;
                    else ((CogMultiPatternAlgo)InspParam.InspAreaParam[InspAreaSelected].InspAlgoParam[InspAlgoSelected].Algorithm).ReferenceInfoList[_Index] = _PatternInfo;
                }
                else MessageBox.Show("패턴을 등록할 수 없습니다.");
            }

            else if (_ReferAction == eReferAction.DEL)
            {
                kpTeachDisplay.ClearDisplay("ReferRegion");
                kpTeachDisplay.ClearDisplay("ReferOriginPoint");

                ((CogPatternAlgo)InspParam.InspAreaParam[InspAreaSelected].InspAlgoParam[InspAlgoSelected].Algorithm).ReferenceInfoList.RemoveAt(_Index);
            }

            GC.Collect();
        }

        private void ApplyPatternMatchingValueFunction(CogPatternAlgo _CogPatternAlgo, ref CogPatternResult _CogPatternResult)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }
            AlgorithmAreaDisplayRefresh();

            bool _Result = InspPatternProcess.Run(InspectionImage, AlgoRegionRectangle, _CogPatternAlgo, ref _CogPatternResult);

            for (int iLoopCount = 0; iLoopCount < _CogPatternResult.FindCount; ++iLoopCount)
            {
                CogRectangle _PatternRect = new CogRectangle();
                _PatternRect.SetCenterWidthHeight(_CogPatternResult.CenterX[iLoopCount], _CogPatternResult.CenterY[iLoopCount], _CogPatternResult.Width[iLoopCount], _CogPatternResult.Height[iLoopCount]);
                kpTeachDisplay.DrawStaticShape(_PatternRect, "PatternRect" + (iLoopCount + 1), CogColorConstants.Green);

                CogPointMarker _Point = new CogPointMarker();
                _Point.SetCenterRotationSize(_CogPatternResult.OriginPointX[iLoopCount], _CogPatternResult.OriginPointY[iLoopCount], 0, 2);
                kpTeachDisplay.DrawStaticShape(_Point, "PatternOrigin" + (iLoopCount + 1), CogColorConstants.Green, 12);

                string _MatchingName = string.Format($"Rate = {_CogPatternResult.Score[iLoopCount]:F2}, X = {_CogPatternResult.OriginPointX[iLoopCount]:F2}, Y = {_CogPatternResult.OriginPointY[iLoopCount]:F2}");
                kpTeachDisplay.DrawText(_MatchingName, _CogPatternResult.OriginPointX[iLoopCount], _CogPatternResult.OriginPointY[iLoopCount] + 30, CogColorConstants.Green, 10, CogGraphicLabelAlignmentConstants.BaselineCenter);
            }
        }

        private void ApplyMultiPatternValueFunction(CogMultiPatternAlgo _CogMultiPatternAlgo, ref CogMultiPatternResult _CogMultiPatternResult)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }
            AlgorithmAreaDisplayRefresh();

            bool _Result = InspMultiPatternProcess.Run(InspectionImage, AlgoRegionRectangle, _CogMultiPatternAlgo, ref _CogMultiPatternResult);

            for (int iLoopCount = 0; iLoopCount < _CogMultiPatternResult.FindCount; ++iLoopCount)
            {
                CogRectangle _PatternRect = new CogRectangle();
                _PatternRect.SetCenterWidthHeight(_CogMultiPatternResult.CenterX[iLoopCount], _CogMultiPatternResult.CenterY[iLoopCount], _CogMultiPatternResult.Width[iLoopCount], _CogMultiPatternResult.Height[iLoopCount]);
                kpTeachDisplay.DrawStaticShape(_PatternRect, "PatternRect" + (iLoopCount + 1), CogColorConstants.Green);

                CogPointMarker _Point = new CogPointMarker();
                _Point.SetCenterRotationSize(_CogMultiPatternResult.OriginPointX[iLoopCount], _CogMultiPatternResult.OriginPointY[iLoopCount], 0, 2);
                kpTeachDisplay.DrawStaticShape(_Point, "PatternOrigin" + (iLoopCount + 1), CogColorConstants.Green, 12);

                string _MatchingName = string.Format($"Rate = {_CogMultiPatternResult.Score[iLoopCount]:F2}, X = {_CogMultiPatternResult.OriginPointX[iLoopCount]:F2}, Y = {_CogMultiPatternResult.OriginPointY[iLoopCount]:F2}");
                kpTeachDisplay.DrawText(_MatchingName, _CogMultiPatternResult.OriginPointX[iLoopCount], _CogMultiPatternResult.OriginPointY[iLoopCount] + 30, CogColorConstants.Green, 10, CogGraphicLabelAlignmentConstants.BaselineCenter);
            }
        }
        #endregion Pattern Matching Window Event : ucCogPatternWindow -> TeachingWindow

        #region Auto Pattern Find Window Event : ucCogAutoPatternWindow -> TeachingWindow
        //private void DrawAutoPatternRegionFunction(CogRectangle _ReferRegion, double _OriginX, double _OriginY, CogColorConstants _Color)
        //{
        //    if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }
        //    AlgorithmAreaDisplayRefresh();

        //    CogPointMarker _PointMarker = new CogPointMarker();
        //    _PointMarker.SetCenterRotationSize(_OriginX, _OriginY, 0, 1);

        //    kpTeachDisplay.DrawInterActiveShape(_ReferRegion, "ReferRegion", _Color);
        //    kpTeachDisplay.DrawInterActiveShape(_PointMarker, "ReferOriginPoint", _Color, 14);
        //}

        private void AutoPatternActionFunction(eReferAction _ReferAction, int _Index = 0)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }

            //if (_ReferAction == eReferAction.ADD)
            //{
            //    CogPointMarker _PointMark = new CogPointMarker();
            //    int _Pixel;
            //    double _PointCenterX, _PointCenterY, _Rotate;
            //    _PointMark = kpTeachDisplay.GetInterActivePoint();
            //    _PointMark.GetCenterRotationSize(out _PointCenterX, out _PointCenterY, out _Rotate, out _Pixel);

            //    CogRectangle _ReferRegion = new CogRectangle();
            //    CogRectangle _Boundary = new CogRectangle();
            //    _Boundary.SetXYWidthHeight(AlgoRegionRectangle.X, AlgoRegionRectangle.Y, AlgoRegionRectangle.Width, AlgoRegionRectangle.Height);
            //    if (false == GetCorrectionRectangle(kpTeachDisplay, _Boundary, ref _ReferRegion)) { MessageBox.Show("The rectangle is outside the inspection area."); return; }

            //    double _OriginPointOffsetX = _ReferRegion.CenterX - _PointCenterX;
            //    double _OriginPointOffsetY = _ReferRegion.CenterY - _PointCenterY;

            //    DrawReferRegionFunction(_ReferRegion, _PointMark.X, _PointMark.Y, CogColorConstants.Cyan);

            //    //Pattern 추출
            //    ReferenceInformation _PatternInfo = new ReferenceInformation();
            //    _PatternInfo.StaticStartX = _ReferRegion.X;
            //    _PatternInfo.StaticStartY = _ReferRegion.Y;
            //    _PatternInfo.CenterX = _ReferRegion.CenterX;
            //    _PatternInfo.CenterY = _ReferRegion.CenterY;
            //    _PatternInfo.Width = _ReferRegion.Width;
            //    _PatternInfo.Height = _ReferRegion.Height;
            //    _PatternInfo.OriginPointOffsetX = _OriginPointOffsetX;
            //    _PatternInfo.OriginPointOffsetY = _OriginPointOffsetY;
            //    _PatternInfo.Reference = InspPatternProcess.GetPatternReference(InspectionImage, _ReferRegion, _PointCenterX, _PointCenterY);
            //    ((CogPatternAlgo)InspParam.InspAreaParam[InspAreaSelected].InspAlgoParam[InspAlgoSelected].Algorithm).ReferenceInfoList.Add(_PatternInfo);
            //    //((CogMultiPatternAlgo)InspParam.InspAreaParam[InspAreaSelected].InspAlgoParam[InspAlgoSelected].Algorithm).ReferenceInfoList.Add(_PatternInfo);
            //}

            else if (_ReferAction == eReferAction.MODIFY)
            {
                CogPointMarker _PointMark = new CogPointMarker();
                int _Pixel;
                double _PointCenterX, _PointCenterY, _Rotate;
                _PointMark = kpTeachDisplay.GetInterActivePoint();
                _PointMark.GetCenterRotationSize(out _PointCenterX, out _PointCenterY, out _Rotate, out _Pixel);

                CogRectangle _ReferRegion = new CogRectangle();
                CogRectangle _Boundary = new CogRectangle();
                _Boundary.SetXYWidthHeight(AlgoRegionRectangle.X, AlgoRegionRectangle.Y, AlgoRegionRectangle.Width, AlgoRegionRectangle.Height);
                if (false == GetCorrectionRectangle(kpTeachDisplay, _Boundary, ref _ReferRegion)) { MessageBox.Show("The rectangle is outside the inspection area."); return; }

                double _OriginPointOffsetX = _ReferRegion.CenterX - _PointCenterX;
                double _OriginPointOffsetY = _ReferRegion.CenterY - _PointCenterY;

                DrawReferRegionFunction(_ReferRegion, _PointMark.X, _PointMark.Y, CogColorConstants.Cyan);

                //Pattern 추출
                ReferenceInformation _PatternInfo = new ReferenceInformation();
                _PatternInfo.StaticStartX = _ReferRegion.X;
                _PatternInfo.StaticStartY = _ReferRegion.Y;
                _PatternInfo.CenterX = _ReferRegion.CenterX;
                _PatternInfo.CenterY = _ReferRegion.CenterY;
                _PatternInfo.Width = _ReferRegion.Width;
                _PatternInfo.Height = _ReferRegion.Height;
                _PatternInfo.OriginPointOffsetX = _OriginPointOffsetX;
                _PatternInfo.OriginPointOffsetY = _OriginPointOffsetY;
                bool PatternResult = InspPatternProcess.GetPatternReference(InspectionImage, _ReferRegion, _PointCenterX, _PointCenterY, ref _PatternInfo.Reference);


                if (PatternResult)
                {
                    ((CogPatternAlgo)InspParam.InspAreaParam[InspAreaSelected].InspAlgoParam[InspAlgoSelected].Algorithm).ReferenceInfoList[_Index] = _PatternInfo;
                }
            }

            //else if (_ReferAction == eReferAction.DEL)
            //{
            //    kpTeachDisplay.ClearDisplay("ReferRegion");
            //    kpTeachDisplay.ClearDisplay("ReferOriginPoint");

            //    ((CogPatternAlgo)InspParam.InspAreaParam[InspAreaSelected].InspAlgoParam[InspAlgoSelected].Algorithm).ReferenceInfoList.RemoveAt(_Index);
            //}

            GC.Collect();
        }

        private void ApplyAutoPatternFindValueFunction(CogAutoPatternAlgo _CogAutoPatternAlgo, ref CogAutoPatternResult _CogAutoPatternResult)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }
            AlgorithmAreaDisplayRefresh();

            bool _Result = InspAutoPatternProcess.Run(InspectionImage, AlgoRegionRectangle, _CogAutoPatternAlgo, ref _CogAutoPatternResult);

            if (_CogAutoPatternAlgo.ReferenceInfoList.Count != 0)
            {
                CogRectangle _PatternRect = new CogRectangle();
                _PatternRect.SetCenterWidthHeight(_CogAutoPatternResult.CenterX, _CogAutoPatternResult.CenterY, _CogAutoPatternResult.Width, _CogAutoPatternResult.Height);
                kpTeachDisplay.DrawStaticShape(_PatternRect, "AutoPatternRect" + 1, CogColorConstants.Green);

                CogPointMarker _Point = new CogPointMarker();
                _Point.SetCenterRotationSize(_CogAutoPatternResult.OriginPointX, _CogAutoPatternResult.OriginPointY, 0, 2);
                kpTeachDisplay.DrawStaticShape(_Point, "AutoPatternOrigin" + 1, CogColorConstants.Green, 12);

                string _MatchingName = string.Format($"Rate = {_CogAutoPatternResult.Score:F2}, X = {_CogAutoPatternResult.OriginPointX:F2}, Y = {_CogAutoPatternResult.OriginPointY:F2}");
                kpTeachDisplay.DrawText(_MatchingName, _CogAutoPatternResult.OriginPointX, _CogAutoPatternResult.OriginPointY + 30, CogColorConstants.Green, 10, CogGraphicLabelAlignmentConstants.BaselineCenter);
            }
        }
        #endregion Pattern Matching Window Event : ucCogPatternWindow -> TeachingWindow

        #region Blob Reference Window Event : ucCogBlobReferenceWindow -> TeachingWindow
        private void ApplyBlobReferenceValueFunction(CogBlobReferenceAlgo _CogBlobReferAlgo, ref CogBlobReferenceResult _CogBlobReferResult)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }
            AlgorithmAreaDisplayRefresh();

            CogRectangleAffine _AlgoRegionAffine = new CogRectangleAffine();
            _AlgoRegionAffine.SetCenterLengthsRotationSkew(AlgoRegionRectangle.CenterX, AlgoRegionRectangle.CenterY, AlgoRegionRectangle.Width, AlgoRegionRectangle.Height, 0, 0);
            bool _Result = InspBlobReferProcess.Run(InspectionImage, _AlgoRegionAffine, _CogBlobReferAlgo, ref _CogBlobReferResult);

            for (int iLoopCount = 0; iLoopCount < _CogBlobReferResult.BlobCount; ++iLoopCount)
            {
                double _ResultRealWidth = _CogBlobReferResult.Width[iLoopCount] * ResolutionX;
                double _ResultRealHeight = _CogBlobReferResult.Height[iLoopCount] * ResolutionY;

                double _RealWidth       = _CogBlobReferAlgo.Width;
                double _RealWidthPos    = _CogBlobReferAlgo.WidthPos;
                double _RealWidthNeg    = _CogBlobReferAlgo.WidthNeg;
                double _RealHeight      = _CogBlobReferAlgo.Height;
                double _RealHeightPos   = _CogBlobReferAlgo.HeightPos;
                double _RealHeightNeg   = _CogBlobReferAlgo.HeightNeg;

                if ((_ResultRealWidth > _RealWidth - Math.Abs(_RealWidthNeg)) && (_ResultRealWidth < _RealWidth + _RealWidthPos) && (_ResultRealHeight > _RealHeight - Math.Abs(_RealHeightNeg)) && (_ResultRealHeight < _RealHeight + _RealHeightPos))
                {
                    CogRectangle _BlobRect = new CogRectangle();
                    _BlobRect.SetCenterWidthHeight(_CogBlobReferResult.BlobCenterX[iLoopCount], _CogBlobReferResult.BlobCenterY[iLoopCount], _CogBlobReferResult.Width[iLoopCount], _CogBlobReferResult.Height[iLoopCount]);
                    kpTeachDisplay.DrawStaticShape(_BlobRect, "BlobRect" + (iLoopCount + 1), CogColorConstants.Green);
                    kpTeachDisplay.DrawBlobResult(_CogBlobReferResult.ResultGraphic[iLoopCount], "BlobRectGra" + (iLoopCount + 1));

                    CogPointMarker _Point = new CogPointMarker();
                    _Point.X = _CogBlobReferResult.OriginX[iLoopCount];
                    _Point.Y = _CogBlobReferResult.OriginY[iLoopCount];
                    kpTeachDisplay.DrawStaticShape(_Point, "BlobSearchPoint", CogColorConstants.Green, 5);

                    string _RectSizeName = string.Format("W : {0:F2}mm, H : {1:F2}mm, Area : {2}", _ResultRealWidth, _ResultRealHeight, _CogBlobReferResult.BlobArea[iLoopCount]);
                    kpTeachDisplay.DrawText(_RectSizeName, _CogBlobReferResult.BlobCenterX[iLoopCount] + _CogBlobReferResult.Width[iLoopCount] / 2 + 100,
                                                           _CogBlobReferResult.BlobCenterY[iLoopCount] + _CogBlobReferResult.Height[iLoopCount] / 2 + 100, CogColorConstants.Green, 10, CogGraphicLabelAlignmentConstants.BaselineCenter);

                }
            }
        }

        private double GetHistogramValueFunction()
        {
            double _HistoStandardDeviatioValue = 0;

            _HistoStandardDeviatioValue  = InspBlobReferProcess.GetHistogramStandardDeviatioValue(InspectionImage, AlgoRegionRectangle);

            return _HistoStandardDeviatioValue;
        }
        #endregion Blob Reference Window Event : ucCogBlobReferenceWindow -> TeachingWindow

        #region Needle Circle Find Window Event : ucCogNeedleCircleFind -> TeachingWindow
        private void ApplyNeedleCircleFindValueFunction(CogNeedleFindAlgo _CogNeedleFindAlgo, ref CogNeedleFindResult _CogNeedleFindResult)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }
            AlgorithmAreaDisplayRefresh();

            bool _Result = InspNeedleCircleFindProcess.Run(InspectionImage, _CogNeedleFindAlgo, ref _CogNeedleFindResult);

            _CogNeedleFindResult.CenterXReal = (_CogNeedleFindResult.CenterX - (InspectionImage.Width / 2)) * ResolutionX;
            _CogNeedleFindResult.CenterYReal = (_CogNeedleFindResult.CenterY - (InspectionImage.Height / 2)) * ResolutionY;
            _CogNeedleFindResult.OriginXReal = (_CogNeedleFindResult.OriginX - (InspectionImage.Width / 2)) * ResolutionX;
            _CogNeedleFindResult.OriginYReal = (_CogNeedleFindResult.OriginY - (InspectionImage.Height / 2)) * ResolutionY;
            _CogNeedleFindResult.RadiusReal = _CogNeedleFindResult.Radius * ResolutionX;

            CogCircle _CogCircle = new CogCircle();
            if (_CogNeedleFindResult.Radius <= 0) return;
            _CogCircle.SetCenterRadius(_CogNeedleFindResult.CenterX, _CogNeedleFindResult.CenterY, _CogNeedleFindResult.Radius);
            CogPointMarker _CogCenterPoint = new CogPointMarker();
            _CogCenterPoint.SetCenterRotationSize(_CogNeedleFindResult.CenterX, _CogNeedleFindResult.CenterY, 0, 2);
            kpTeachDisplay.DrawStaticShape(_CogCircle, "Circle", CogColorConstants.Green, 3);
            kpTeachDisplay.DrawStaticShape(_CogCenterPoint, "CirclePoint", CogColorConstants.Green);

            string _CenterName = string.Format("X = {0:F2}mm, Y = {1:F2}mm, R = {2:F2}mm", _CogNeedleFindResult.CenterXReal, _CogNeedleFindResult.CenterYReal, _CogNeedleFindResult.RadiusReal);
            kpTeachDisplay.DrawText(_CenterName, _CogNeedleFindResult.CenterX , _CogNeedleFindResult.CenterY + 150, CogColorConstants.Green, 10, CogGraphicLabelAlignmentConstants.BaselineCenter);
        }

        private void DrawNeedleCircleFindCaliperFunction(CogNeedleFindAlgo _CogNeedleFindAlgo)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }
            AlgorithmAreaDisplayRefresh();

            CogFindCircle _CogFindCircle = new CogFindCircle();
            _CogFindCircle.NumCalipers = _CogNeedleFindAlgo.CaliperNumber;
            _CogFindCircle.CaliperSearchLength = _CogNeedleFindAlgo.CaliperSearchLength;
            _CogFindCircle.CaliperProjectionLength = _CogNeedleFindAlgo.CaliperProjectionLength;
            _CogFindCircle.NumToIgnore = _CogNeedleFindAlgo.CaliperIgnoreNumber;
            _CogFindCircle.CaliperSearchDirection = (CogFindCircleSearchDirectionConstants)_CogNeedleFindAlgo.CaliperSearchDirection;
            _CogFindCircle.CaliperRunParams.Edge0Polarity = (CogCaliperPolarityConstants)_CogNeedleFindAlgo.CaliperPolarity;

            _CogFindCircle.ExpectedCircularArc.CenterX = _CogNeedleFindAlgo.ArcCenterX;
            _CogFindCircle.ExpectedCircularArc.CenterY = _CogNeedleFindAlgo.ArcCenterY;
            _CogFindCircle.ExpectedCircularArc.Radius = _CogNeedleFindAlgo.ArcRadius;
            _CogFindCircle.ExpectedCircularArc.AngleStart = _CogNeedleFindAlgo.ArcAngleStart;
            _CogFindCircle.ExpectedCircularArc.AngleSpan = _CogNeedleFindAlgo.ArcAngleSpan;

            kpTeachDisplay.DrawFindCircleCaliper(_CogFindCircle);
        }
        #endregion Needle Circle Find Window Event : ucCogNeedleCircleFind -> TeachingWindow

        #region Ellipse Find Window Event : ucCogEllipseFind -> TeachingWindow
        private void ApplyEllipseValueFunction(CogEllipseAlgo _CogEllipseAlgo, ref CogEllipseResult _CogEllipseResult)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }
            AlgorithmAreaDisplayRefresh();

            bool _Result = InspEllipseFindProcess.Run(InspectionImage, AlgoRegionRectangle, _CogEllipseAlgo, ref _CogEllipseResult);

            _CogEllipseResult.CenterXReal = (_CogEllipseResult.CenterX - (InspectionImage.Width / 2)) * ResolutionX;
            _CogEllipseResult.CenterYReal = (_CogEllipseResult.CenterY - (InspectionImage.Height / 2)) * ResolutionY;
            _CogEllipseResult.OriginXReal = (_CogEllipseResult.OriginX - (InspectionImage.Width / 2)) * ResolutionX;
            _CogEllipseResult.OriginYReal = (_CogEllipseResult.OriginY - (InspectionImage.Height / 2)) * ResolutionY;
            _CogEllipseResult.RadiusXReal = _CogEllipseResult.RadiusX * ResolutionX;
            _CogEllipseResult.RadiusYReal = _CogEllipseResult.RadiusY * ResolutionY;

            CogEllipse _CogEllipse = new CogEllipse();
            if (_CogEllipseResult.RadiusX <= 0 || _CogEllipseResult.RadiusY <= 0) return;
            _CogEllipse.SetCenterXYRadiusXYRotation(_CogEllipseResult.CenterX, _CogEllipseResult.CenterY, _CogEllipseResult.RadiusX, _CogEllipseResult.RadiusY, 0);
            CogPointMarker _CogCenterPoint = new CogPointMarker();
            _CogCenterPoint.SetCenterRotationSize(_CogEllipseResult.CenterX, _CogEllipseResult.CenterY, 0, 2);
            kpTeachDisplay.DrawStaticShape(_CogEllipse, "Ellipse", CogColorConstants.Green, 3);
            kpTeachDisplay.DrawStaticShape(_CogCenterPoint, "EllipsePoint", CogColorConstants.Green);

            string _CenterName = string.Format("X = {0:F2}mm, Y = {1:F2}mm, XR = {2:F2}mm, YR = {3:F2}mm", _CogEllipseResult.CenterXReal, _CogEllipseResult.CenterYReal, _CogEllipseResult.RadiusXReal, _CogEllipseResult.RadiusYReal);
            kpTeachDisplay.DrawText(_CenterName, _CogEllipseResult.CenterX, _CogEllipseResult.CenterY + 150, CogColorConstants.Green, 10, CogGraphicLabelAlignmentConstants.BaselineCenter);
        }

        private void DrawEllipseCaliperFunction(CogEllipseAlgo _CogEllipseAlgo)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }
            AlgorithmAreaDisplayRefresh();

            CogFindEllipse _CogEllipse = new CogFindEllipse();
            _CogEllipse.NumCalipers = _CogEllipseAlgo.CaliperNumber;
            _CogEllipse.CaliperSearchLength = _CogEllipseAlgo.CaliperSearchLength;
            _CogEllipse.CaliperProjectionLength = _CogEllipseAlgo.CaliperProjectionLength;
            _CogEllipse.NumToIgnore = _CogEllipseAlgo.CaliperIgnoreNumber;
            _CogEllipse.CaliperSearchDirection = (CogFindEllipseSearchDirectionConstants)_CogEllipseAlgo.CaliperSearchDirection;
            _CogEllipse.CaliperRunParams.Edge0Polarity = (CogCaliperPolarityConstants)_CogEllipseAlgo.CaliperPolarity;

            _CogEllipse.ExpectedEllipticalArc.CenterX = _CogEllipseAlgo.ArcCenterX;
            _CogEllipse.ExpectedEllipticalArc.CenterY = _CogEllipseAlgo.ArcCenterY;
            _CogEllipse.ExpectedEllipticalArc.RadiusX = _CogEllipseAlgo.ArcRadiusX;
            _CogEllipse.ExpectedEllipticalArc.RadiusY = _CogEllipseAlgo.ArcRadiusY;
            _CogEllipse.ExpectedEllipticalArc.AngleSpan = _CogEllipseAlgo.ArcAngleSpan;

            kpTeachDisplay.DrawFindEllipseCaliper(_CogEllipse);
        }
        #endregion Ellipse Find Window Event : ucCogEllipseFind -> TeachingWindow

        #region Lead Inspection Window Event : ucCogLeadInspection -> TeachingWindow
        private void ApplyLeadInspValueFunction(CogLeadAlgo _CogLeadAlgo, ref CogLeadResult _CogLeadResult, bool _IsDisplay = true)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }
            if (_IsDisplay) AlgorithmAreaDisplayRefresh();

            bool _Result = InspLeadProcess.Run(InspectionImage, AlgoRegionRectangle, _CogLeadAlgo, ref _CogLeadResult);

            #region Result Display
            if (_IsDisplay)
            {
                for (int iLoopCount = 0; iLoopCount < _CogLeadResult.BlobCount; ++iLoopCount)
                {
                    //Blob Boundary
                    CogRectangleAffine _BlobRectAffine = new CogRectangleAffine();
                    _BlobRectAffine.SetCenterLengthsRotationSkew(_CogLeadResult.BlobCenterX[iLoopCount], _CogLeadResult.BlobCenterY[iLoopCount], _CogLeadResult.PrincipalWidth[iLoopCount], _CogLeadResult.PrincipalHeight[iLoopCount], _CogLeadResult.Angle[iLoopCount], 0);
                    kpTeachDisplay.DrawStaticShape(_BlobRectAffine, "BlobRectAffine" + (iLoopCount + 1), CogColorConstants.Green);
                    kpTeachDisplay.DrawBlobResult(_CogLeadResult.ResultGraphic[iLoopCount], "BlobRectGra" + (iLoopCount + 1));

                    CogLineSegment _CenterLine = new CogLineSegment();
                    kpTeachDisplay.DrawStaticLine(_CogLeadResult.BlobCenterX[iLoopCount], _CogLeadResult.BlobCenterY[iLoopCount], _CogLeadResult.PrincipalWidth[iLoopCount] / 2 + 20, _CogLeadResult.Angle[iLoopCount], 2, "CenterLine+_" + (iLoopCount + 1), CogColorConstants.Cyan);
                    kpTeachDisplay.DrawStaticLine(_CogLeadResult.BlobCenterX[iLoopCount], _CogLeadResult.BlobCenterY[iLoopCount], _CogLeadResult.PrincipalWidth[iLoopCount] / 2 + 20, (Math.PI) + _CogLeadResult.Angle[iLoopCount], 2, "CenterLine-_" + (iLoopCount + 1), CogColorConstants.Cyan);

                    CogPointMarker _PitchPoint = new CogPointMarker();
                    _PitchPoint.SetCenterRotationSize(_CogLeadResult.LeadPitchTopX[iLoopCount], _CogLeadResult.LeadPitchTopY[iLoopCount], 0, 1);
                    kpTeachDisplay.DrawStaticShape(_PitchPoint, "PointStart" + (iLoopCount + 1), CogColorConstants.Yellow, 2);
                    _PitchPoint.SetCenterRotationSize(_CogLeadResult.LeadPitchBottomX[iLoopCount], _CogLeadResult.LeadPitchBottomY[iLoopCount], 0, 1);
                    kpTeachDisplay.DrawStaticShape(_PitchPoint, "PointEnd" + (iLoopCount + 1), CogColorConstants.Orange, 2);
                }
            }
            #endregion Result Display

            #region Pitch Average 측정
            try
            {
                double[] _LeadPitchX = new double[_CogLeadResult.BlobCount];
                Array.Copy(_CogLeadResult.LeadPitchTopX, _LeadPitchX, _CogLeadResult.BlobCount);
                Array.Sort(_LeadPitchX);

                double[] _LeadPitches = new double[_CogLeadResult.BlobCount - 1];
                for (int iLoopCount = 0; iLoopCount < _CogLeadResult.BlobCount - 1; ++iLoopCount)
                    _LeadPitches[iLoopCount] = _LeadPitchX[iLoopCount + 1] - _LeadPitchX[iLoopCount];

                Array.Sort(_LeadPitches);
                double _Gab = _LeadPitches[4];
                int _Index = 1;
                double _PitchSum = _LeadPitches[4], _PitchArray = 0;
                for (int iLoopCount = 5; iLoopCount < _CogLeadResult.BlobCount - 1; ++iLoopCount)
                {
                    if (_Gab + 20 < _LeadPitches[iLoopCount]) break;
                    _PitchSum += _LeadPitches[iLoopCount];
                    _Index++;
                }

                _PitchArray = _PitchSum / _Index;
                _CogLeadResult.LeadPitchAvg = _PitchArray;
            }

            catch
            {
                _CogLeadResult.LeadPitchAvg = 0;
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.ERR, "ApplyLeadInspValueFunction Exception", CLogManager.LOG_LEVEL.LOW);
            }
            #endregion Pitch Average 측정

            #region Lead Angle Average 측정
            try
            {
                double[] _LeadAngles = new double[_CogLeadResult.BlobCount];
                Array.Copy(_CogLeadResult.Angle, _LeadAngles, _CogLeadResult.BlobCount);
                Array.Sort(_LeadAngles);

                int _Index = 0;
                double _AngleSum = 0, _AngleAvg = 0;
                for (int iLoopCount = 5; iLoopCount < _CogLeadResult.BlobCount - 5; ++iLoopCount)
                {
                    _AngleSum += _CogLeadResult.Angle[iLoopCount];
                    _Index++;
                }
                _AngleAvg = _AngleSum / _Index;
                _CogLeadResult.LeadAngleAvg = _AngleAvg;
            }

            catch
            {
                _CogLeadResult.LeadAngleAvg = 0;
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.ERR, "ApplyLeadInspValueFunction Exception", CLogManager.LOG_LEVEL.LOW);
            }
            #endregion Lead Angle Average 측정
        }
        #endregion Lead Inspection Window Event : ucCogLeadInspection -> TeachingWindow

        #region ID Reading Window Event : ucCogID ->TeachingWindow
        private void ApplyBarCodeIDInspValueFunction(CogBarCodeIDAlgo _CogBarCodeIDAlgo, ref CogBarCodeIDResult _CogBarCodeIDResult)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }
            AlgorithmAreaDisplayRefresh();

            bool _Result = InspIDProcess.Run(InspectionImage, AlgoRegionRectangle, _CogBarCodeIDAlgo, ref _CogBarCodeIDResult);

            CogPolygon _Polygon = new CogPolygon();

            if (_CogBarCodeIDResult != null)
            {
                for (int iLoopCount = 0; iLoopCount < _CogBarCodeIDResult.IDCount; iLoopCount++)
                {
                    _Polygon.SetVertices(_CogBarCodeIDResult.IDPolygon[iLoopCount].GetVertices());
                    kpTeachDisplay.DrawStaticShape(_Polygon, "BarCodeID" + iLoopCount + "_Polygon", CogColorConstants.Green);

                    string _ResultIDName = string.Format("ID = {0}", _CogBarCodeIDResult.IDResult);
                    kpTeachDisplay.DrawText(_ResultIDName, _CogBarCodeIDResult.IDCenterX[iLoopCount], _CogBarCodeIDResult.IDCenterY[iLoopCount] + 30, CogColorConstants.Green, 8, CogGraphicLabelAlignmentConstants.BaselineCenter);
                }
            }
        }
        #endregion ID Reading Window Event : ucCogID ->TeachingWindow

        #region Line Find Window Event : ucCogLineFind -> TeachingWindow
        private void ApplyLineFindValueFunction(CogLineFindAlgo _CogLineFindAlgo, ref CogLineFindResult _CogLineFindResult)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }

            #region Caliper Area Check
            Rectangle _Bound = new Rectangle((int)AlgoRegionRectangle.X, (int)AlgoRegionRectangle.Y, (int)AlgoRegionRectangle.Width, (int)AlgoRegionRectangle.Height);
            if (false == _Bound.Contains((int)_CogLineFindAlgo.CaliperLineStartX, (int)_CogLineFindAlgo.CaliperLineStartY))  { MessageBox.Show("The caliper is outisde the inspection area."); return;}
            else if (false == _Bound.Contains((int)_CogLineFindAlgo.CaliperLineEndX, (int)_CogLineFindAlgo.CaliperLineEndY)) { MessageBox.Show("The caliper is outisde the inspection area."); return; }
            #endregion

            AlgorithmAreaDisplayRefresh();

            #region 글자 제거용 Width Morphology 사용 : 주석 처리(문제 될 시 사용 검토
            //CogIPOneImageTool _CogOneImageTool = new CogIPOneImageTool();
            //CogIPOneImageGreyMorphologyNxM _Morphology = new CogIPOneImageGreyMorphologyNxM();
            //_Morphology.Operation = CogIPOneImageMorphologyOperationConstants.Dilate;
            //_Morphology.KernelHeight = 1;
            //_Morphology.KernelWidth = 9;
            //
            //ICogIPOneImageOperatorParams _Operators = _Morphology;
            //_CogOneImageTool.Operators.Add(_Operators);
            //_CogOneImageTool.InputImage = InspectionImage;
            //_CogOneImageTool.Run();
            //
            //CogImage8Grey _MorphImage = (CogImage8Grey)_CogOneImageTool.OutputImage;
            #endregion

            CogImage8Grey _DestImage = new CogImage8Grey();
            bool _Result = InspLineFindProcess.Run(InspectionImage, ref _DestImage, AlgoRegionRectangle, _CogLineFindAlgo, ref _CogLineFindResult);

            CogLineSegment _CogLine = new CogLineSegment();
            if (_CogLineFindResult.StartX != 0 && _CogLineFindResult.StartY != 0 && _CogLineFindResult.Length != 0)
            {
                _CogLine.SetStartLengthRotation(_CogLineFindResult.StartX, _CogLineFindResult.StartY, _CogLineFindResult.Length, _CogLineFindResult.Rotation);
                kpTeachDisplay.DrawStaticLine(_CogLine, "LineFind", CogColorConstants.Green);
            }
        }

        private void DrawLineFindCaliperFunction(CogLineFindAlgo _CogLineFindAlgo)
        {
            if (eTeachStep.ALGO_SET != CurrentTeachStep) { MessageBox.Show("Not select \"Algorithm Set\" button"); return; }
            AlgorithmAreaDisplayRefresh();

            CogFindLine _CogFindLine = new CogFindLine();
            _CogFindLine.NumCalipers = _CogLineFindAlgo.CaliperNumber;
            _CogFindLine.CaliperSearchLength = _CogLineFindAlgo.CaliperSearchLength;
            _CogFindLine.CaliperProjectionLength = _CogLineFindAlgo.CaliperProjectionLength;
            _CogFindLine.CaliperSearchDirection = (_CogLineFindAlgo.CaliperSearchDirection == 90) ? 1.5708 : -1.5708;
            _CogFindLine.NumToIgnore = _CogLineFindAlgo.IgnoreNumber;
            _CogFindLine.CaliperRunParams.ContrastThreshold = _CogLineFindAlgo.ContrastThreshold;
            _CogFindLine.CaliperRunParams.FilterHalfSizeInPixels = _CogLineFindAlgo.FilterHalfSizePixels;
            _CogFindLine.ExpectedLineSegment.SetStartEnd(_CogLineFindAlgo.CaliperLineStartX, _CogLineFindAlgo.CaliperLineStartY, _CogLineFindAlgo.CaliperLineEndX, _CogLineFindAlgo.CaliperLineEndY);

            kpTeachDisplay.DrawFindLineCaliper(_CogFindLine);
        }

        private bool CheckCaliperStatusFunction(CogLineFindAlgo _CogLineFindAlgo)
        {
            bool _Result = true;

            #region Caliper Area Check
            Rectangle _Bound = new Rectangle((int)AlgoRegionRectangle.X, (int)AlgoRegionRectangle.Y, (int)AlgoRegionRectangle.Width, (int)AlgoRegionRectangle.Height);
            if (false == _Bound.Contains((int)_CogLineFindAlgo.CaliperLineStartX, (int)_CogLineFindAlgo.CaliperLineStartY))  { MessageBox.Show("The caliper is outisde the inspection area."); return false; }
            else if (false == _Bound.Contains((int)_CogLineFindAlgo.CaliperLineEndX, (int)_CogLineFindAlgo.CaliperLineEndY)) { MessageBox.Show("The caliper is outisde the inspection area."); return false; }
            #endregion

            return _Result;

        }
        #endregion Line Find Window Event : ucCogLineFind -> TeachingWindow

        #region Lead Trim Inspection Window Event : ucCogLeadTrimInspection -> TeachingWindow
        private CogRectangle GetRegionFunction()
        {
            return kpTeachDisplay.GetInterActiveRectangle();
        }

        private void DrawRegionFunction(CogRectangle _Region, bool _IsStatic)
        {
            string[] _ExceptGroupName = new string[3] { "InspRegion", "AlgoRegion", "Message" };
            kpTeachDisplay.ClearDisplay(_ExceptGroupName);

            if (_IsStatic)
                kpTeachDisplay.DrawStaticShape(_Region, "Region", CogColorConstants.Magenta, 2, CogGraphicLineStyleConstants.Dash);
            else
                kpTeachDisplay.DrawInterActiveShape(_Region, "Region", CogColorConstants.Magenta);
        }

        private void DrawMaskingRegionFunction(List<RectangleD> _MaskingArea)
        {
            kpTeachDisplay.ClearDisplay(false, true);

            string[] _ExceptGroupName = new string[3] { "InspRegion", "AlgoRegion", "Message" };
            kpTeachDisplay.ClearDisplay(_ExceptGroupName);

            for (int iLoopCount = 0; iLoopCount < _MaskingArea.Count; ++iLoopCount)
            {
                CogRectangle _Area = new CogRectangle();
                _Area.SetCenterWidthHeight(_MaskingArea[iLoopCount].CenterX, _MaskingArea[iLoopCount].CenterY,
                                           _MaskingArea[iLoopCount].Width, _MaskingArea[iLoopCount].Height);

                kpTeachDisplay.DrawStaticShape(_Area, "MaskingArea" + iLoopCount, CogColorConstants.Yellow, 2);
            }
        }

        private void ApplyLeadTrimValueFunction(CogLeadTrimAlgo.eAlgoMode _Mode, CogRectangle _InspRegion, CogLeadTrimAlgo _InspAlgo, ref CogLeadTrimResult _InspResult)
        {
            string[] _ExceptGroupName = new string[3] { "InspRegion", "AlgoRegion", "Message" };
            kpTeachDisplay.ClearDisplay(_ExceptGroupName);

            switch (_Mode)
            {
                case CogLeadTrimAlgo.eAlgoMode.BODY_CHECK:      LeadBodyCheck(_InspRegion, _InspAlgo, ref _InspResult);         break;
                case CogLeadTrimAlgo.eAlgoMode.CHIPOUT_CHECK:   ChipOutInspection(_InspRegion, _InspAlgo, ref _InspResult);     break;
                case CogLeadTrimAlgo.eAlgoMode.LEAD_MEASURE:    LeadMeasurement(_InspRegion, _InspAlgo, ref _InspResult);       break;
                case CogLeadTrimAlgo.eAlgoMode.SHOULDER_CHECK:  ShoulderInspection(_InspRegion, _InspAlgo, ref _InspResult);    break;
                case CogLeadTrimAlgo.eAlgoMode.LEADTIP_CHECK:   LeadTipInspection(_InspRegion, _InspAlgo, ref _InspResult);     break;
            }
        }

        private void LeadBodyCheck(CogRectangle _InspRegion, CogLeadTrimAlgo _InspAlgo, ref CogLeadTrimResult _InspResult)
        {
            bool _Result = InspLeadTrimProcess.LeadBodySearch(InspectionImage, _InspRegion, _InspAlgo);

            if (true == _Result)
            {
                CogLeadTrimResult _LeadTrimResult = new CogLeadTrimResult();
                _LeadTrimResult = InspLeadTrimProcess.GetLeadTrimResult();

                _InspResult.LeadBodyOriginX = _LeadTrimResult.LeadBodyOriginX;
                _InspResult.LeadBodyOriginY = _LeadTrimResult.LeadBodyOriginY;
                _InspResult.LeadBodyOffsetX = _LeadTrimResult.LeadBodyOffsetX;
                _InspResult.LeadBodyOffsetY = _LeadTrimResult.LeadBodyOffsetY;

                #region Draw Lead Body check Display
                CogPointMarker _LT = new CogPointMarker();
                CogPointMarker _RT = new CogPointMarker();
                CogPointMarker _LB = new CogPointMarker();
                CogPointMarker _RB = new CogPointMarker();

                _LT.SetCenterRotationSize(_LeadTrimResult.LeadBodyLeftTop.X, _LeadTrimResult.LeadBodyLeftTop.Y, 0, 2);
                _RT.SetCenterRotationSize(_LeadTrimResult.LeadBodyRightTop.X, _LeadTrimResult.LeadBodyRightTop.Y, 0, 2);
                _LB.SetCenterRotationSize(_LeadTrimResult.LeadBodyLeftBottom.X, _LeadTrimResult.LeadBodyLeftBottom.Y, 0, 2);
                _RB.SetCenterRotationSize(_LeadTrimResult.LeadBodyRightBottom.X, _LeadTrimResult.LeadBodyRightBottom.Y, 0, 2);

                kpTeachDisplay.DrawStaticShape(_LT, "LT", CogColorConstants.Green, 50, true);
                kpTeachDisplay.DrawStaticShape(_RT, "RT", CogColorConstants.Green, 50, true);
                kpTeachDisplay.DrawStaticShape(_LB, "LB", CogColorConstants.Green, 50, true);
                kpTeachDisplay.DrawStaticShape(_RB, "RB", CogColorConstants.Green, 50, true);

                CogRectangle _BodyRect = new CogRectangle();
                _BodyRect.SetXYWidthHeight(_LT.X, _LT.Y, _RT.X - _LT.X, _RB.Y - _RT.Y);
                kpTeachDisplay.DrawStaticShape(_BodyRect, "BodyRect", CogColorConstants.Green);
                #endregion
            }
        }

        private void ChipOutInspection(CogRectangle _InspRegion, CogLeadTrimAlgo _InspAlgo, ref CogLeadTrimResult _InspResult)
        {
            bool _Result = InspLeadTrimProcess.MoldChipOutInspection(InspectionImage, _InspRegion, _InspAlgo);

            if (true == _Result)
            {
                CogLeadTrimResult _LeadTrimResult = new CogLeadTrimResult();
                _LeadTrimResult = InspLeadTrimProcess.GetLeadTrimResult();

                #region Draw Chip Out Inspection Display
                for (int iLoopCount = 0; iLoopCount < _LeadTrimResult.ChipOutNgList.Count; ++iLoopCount)
                {
                    CogRectangle _NgRegion = new CogRectangle(_LeadTrimResult.ChipOutNgList[iLoopCount]);
                    _InspResult.ChipOutNgList.Add(_NgRegion);

                    kpTeachDisplay.DrawStaticShape(_NgRegion, "ChipOutDefect" + (iLoopCount + 1), CogColorConstants.Red);

                    CogPointMarker _Point = new CogPointMarker();
                    _Point.X = _NgRegion.CenterX;
                    _Point.Y = _NgRegion.CenterY;
                    kpTeachDisplay.DrawStaticShape(_Point, "BlobSearchPoint", CogColorConstants.Red, 5);

                    string _RectSizeName = string.Format("W : {0:F2}mm, H : {1:F2}mm", _NgRegion.Width, _NgRegion.Height);
                    kpTeachDisplay.DrawText("ChipOutDefectMessage",_RectSizeName, _NgRegion.CenterX + _NgRegion.Width / 2 + 100,
                                            _NgRegion.CenterY + _NgRegion.Height / 2 + 100, CogColorConstants.Red, 8, CogGraphicLabelAlignmentConstants.BaselineCenter);
                }
                #endregion 
            }
        }

        private void LeadMeasurement(CogRectangle _InspRegion, CogLeadTrimAlgo _InspAlgo, ref CogLeadTrimResult _InspResult)
        {
            bool _Result = InspLeadTrimProcess.LeadMeasurement(InspectionImage, _InspRegion, _InspAlgo);

            if (true == _Result)
            {
                CogLeadTrimResult _LeadTrimResult = new CogLeadTrimResult();
                _LeadTrimResult = InspLeadTrimProcess.GetLeadTrimResult();

                _InspResult.LeadCount = _LeadTrimResult.LeadCount;
                _InspResult.LeadLength = _LeadTrimResult.LeadLength;
                _InspResult.LeadPitchLength = _LeadTrimResult.LeadPitchLength;
                _InspResult.LeadWidth = _LeadTrimResult.LeadWidth;

                #region Draw Lead Measurement Display
                for (int iLoopCount = 0; iLoopCount < _LeadTrimResult.LeadCount; ++iLoopCount)
                {
                    //Blob Boundary
                    //CogRectangleAffine _BlobRectAffine = new CogRectangleAffine();
                    //_BlobRectAffine.SetCenterLengthsRotationSkew(_LeadTrimResult.LeadCenterX[iLoopCount], _LeadTrimResult.LeadCenterY[iLoopCount], _LeadTrimResult.LeadWidth[iLoopCount], _LeadTrimResult.LeadLength[iLoopCount], _LeadTrimResult.Angle[iLoopCount], 0);
                    //kpTeachDisplay.DrawStaticShape(_BlobRectAffine, "BlobRectAffine" + (iLoopCount + 1), CogColorConstants.Green);
                    
                    //kpTeachDisplay.DrawBlobResult(_LeadTrimResult.ResultGraphic[iLoopCount], "BlobRectGra" + (iLoopCount + 1));

                    CogPointMarker _PitchPoint = new CogPointMarker();
                    _PitchPoint.SetCenterRotationSize(_LeadTrimResult.LeadPitchTopX[iLoopCount], _LeadTrimResult.LeadPitchTopY[iLoopCount], 0, 1);
                    kpTeachDisplay.DrawStaticShape(_PitchPoint, "PointStart" + (iLoopCount + 1), CogColorConstants.Yellow, 10);
                    _PitchPoint.SetCenterRotationSize(_LeadTrimResult.LeadPitchBottomX[iLoopCount], _LeadTrimResult.LeadPitchBottomY[iLoopCount], 0, 1);
                    kpTeachDisplay.DrawStaticShape(_PitchPoint, "PointEnd" + (iLoopCount + 1), CogColorConstants.Orange, 10);

                    CogLineSegment _LeadLine = new CogLineSegment();
                    _LeadLine.SetStartEnd(_LeadTrimResult.LeadPitchTopX[iLoopCount], _LeadTrimResult.LeadPitchTopY[iLoopCount], _LeadTrimResult.LeadPitchBottomX[iLoopCount], _LeadTrimResult.LeadPitchBottomY[iLoopCount]);
                    kpTeachDisplay.DrawStaticLine(_LeadLine, "CenterLine+_" + (iLoopCount + 1), CogColorConstants.Cyan);
                }

                #endregion
            }
        }

        private void ShoulderInspection(CogRectangle _InspRegion, CogLeadTrimAlgo _InspAlgo, ref CogLeadTrimResult _InspResult)
        {
            bool _Result = InspLeadTrimProcess.ShoulderInspection(InspectionImage, _InspRegion, _InspAlgo);

            if (true == _Result)
            {
                CogLeadTrimResult _LeadTrimResult = new CogLeadTrimResult();
                _LeadTrimResult = InspLeadTrimProcess.GetLeadTrimResult();

                for (int iLoopCount = 0; iLoopCount < _LeadTrimResult.ShoulderBurrDefectList.Count; ++iLoopCount)
                {
                    CogRectangle _NgRegion = new CogRectangle(_LeadTrimResult.ShoulderBurrDefectList[iLoopCount]);
                    kpTeachDisplay.DrawStaticShape(_NgRegion, "ShoulderBurr" + (iLoopCount + 1), CogColorConstants.Red);

                    //string _RectSizeName = string.Format("W : {0:F2}mm, H : {1:F2}mm", _NgRegion.Width, _NgRegion.Height);
                    //kpTeachDisplay.DrawText("ShoulderBurr", _RectSizeName, _NgRegion.CenterX + _NgRegion.Width / 2 + 100,
                    //                        _NgRegion.CenterY + _NgRegion.Height / 2 + 100, CogColorConstants.Red, 8, CogGraphicLabelAlignmentConstants.BaselineCenter);
                }
            }
        }

        private void LeadTipInspection(CogRectangle _InspRegion, CogLeadTrimAlgo _InspAlgo, ref CogLeadTrimResult _InspResult)
        {
            bool _Result = InspLeadTrimProcess.LeadTipInspection(InspectionImage, _InspRegion, _InspAlgo);

            if (true == _Result)
            {
                CogLeadTrimResult _LeadTrimResult = new CogLeadTrimResult();
                _LeadTrimResult = InspLeadTrimProcess.GetLeadTrimResult();

                for (int iLoopCount = 0; iLoopCount < _LeadTrimResult.LeadTipBurrDefectList.Count; ++iLoopCount)
                {
                    CogRectangle _NgRegion = new CogRectangle(_LeadTrimResult.LeadTipBurrDefectList[iLoopCount]);
                    kpTeachDisplay.DrawStaticShape(_NgRegion, "LeadTipBurr" + (iLoopCount + 1), CogColorConstants.Red);
                }
            }
        }
        #endregion
    }
}
