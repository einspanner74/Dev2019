﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ParameterManager;

namespace InspectionSystemManager
{
    public partial class InspectionWindow : Form
    {
        private SendResultParameter GetResultAnalysis()
        {
            SendResultParameter _SendResParam = new SendResultParameter();
            _SendResParam.ID = ID;
            _SendResParam.NgType = eNgType.GOOD;
            _SendResParam.IsGood = true;
            _SendResParam.ProjectItem = ProjectItem;

            //ProjectItem Measure를 사용할경우 결과값 전달용
            _SendResParam.SendResultList = new object[AlgoResultParamList.Count];
            _SendResParam.AlgoTypeList = new eAlgoType[AlgoResultParamList.Count];

            for (int iLoopCount = 0; iLoopCount < AlgoResultParamList.Count; ++iLoopCount)
            {
                if (eAlgoType.C_ELLIPSE == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogEllipseResult;
                    SendMeasureResult _SendResult = new SendMeasureResult();
                    _SendResult.CaliperPointX = new double[_AlgoResultParam.PointFoundCount];
                    _SendResult.CaliperPointY = new double[_AlgoResultParam.PointFoundCount];

                    for (int jLoopCount = 0; jLoopCount < _AlgoResultParam.PointFoundCount; jLoopCount++)
                    {
                        _SendResult.CaliperPointX[jLoopCount] = _AlgoResultParam.PointPosXInfo[jLoopCount];
                        _SendResult.CaliperPointY[jLoopCount] = _AlgoResultParam.PointPosYInfo[jLoopCount];
                    }

                    _SendResParam.AlgoTypeList[iLoopCount] = eAlgoType.C_ELLIPSE;
                    _SendResParam.IsGood &= _AlgoResultParam.IsGood;

                    if (_SendResParam.NgType == eNgType.GOOD)
                        _SendResParam.NgType = (_AlgoResultParam.IsGood == true) ? eNgType.GOOD : eNgType.MEASURE;

                    _SendResult.NGAreaNum = AlgoResultParamList[iLoopCount].NgAreaNumber;
                    _SendResult.IsGoodAlgo = _AlgoResultParam.IsGood;
                    _SendResult.RadiusX = _AlgoResultParam.RadiusX * ResolutionX;
                    _SendResult.RadiusY = _AlgoResultParam.RadiusY * ResolutionY;
                    _SendResult.DiameterMinAlgo = _AlgoResultParam.DiameterMinAlgo;
                    _SendResult.DiameterMaxAlgo = _AlgoResultParam.DiameterMaxAlgo;
                    _SendResParam.SendResultList[iLoopCount] = _SendResult;
                }

                else if (eAlgoType.C_BLOB_REFER == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogBlobReferenceResult;
                    SendMeasureResult _SendResult = new SendMeasureResult();

                    _SendResParam.AlgoTypeList[iLoopCount] = eAlgoType.C_BLOB_REFER;
                    _SendResParam.IsGood &= _AlgoResultParam.IsGood;

                    if (_SendResParam.NgType == eNgType.GOOD)
                        _SendResParam.NgType = (_AlgoResultParam.IsGood == true) ? eNgType.GOOD : eNgType.MEASURE;

                    _SendResult.NGAreaNum = AlgoResultParamList[iLoopCount].NgAreaNumber;
                    _SendResult.IsGoodAlgo = _AlgoResultParam.IsGood;

                    if (_AlgoResultParam.BlobMaxX != null) _SendResult.MeasureData = (_AlgoResultParam.BlobMaxX[0] - _AlgoResultParam.BlobMinX[0]) * ResolutionX;
                    else _SendResult.MeasureData = 0;

                    _SendResParam.SendResultList[iLoopCount] = _SendResult;
                }

                else if (eAlgoType.C_ID == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogBarCodeIDResult;
                    SendCardIDResult _SendResult = new SendCardIDResult();

                    _SendResParam.AlgoTypeList[iLoopCount] = eAlgoType.C_ID;

                    for (int jLoopCount = 0; jLoopCount < _AlgoResultParam.IDResult.Length; jLoopCount++)
                    {
                        _SendResParam.IsGood &= _AlgoResultParam.IsGood;
                        _SendResult.ReadCode = (_AlgoResultParam.IsGood == true) ? _AlgoResultParam.IDResult[jLoopCount] : "";
                        if (_SendResParam.NgType == eNgType.GOOD)
                            _SendResParam.NgType = (_AlgoResultParam.IsGood == true) ? eNgType.GOOD : eNgType.ID;
                    }

                    _SendResParam.SendResultList[iLoopCount] = _SendResult;
                }

                else if (eAlgoType.C_LINE_FIND == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogLineFindResult;

                    _SendResParam.AlgoTypeList[iLoopCount] = eAlgoType.C_LINE_FIND;
                    _SendResParam.IsGood &= _AlgoResultParam.IsGood;

                    if (_SendResParam.NgType == eNgType.GOOD)
                        _SendResParam.NgType = (_AlgoResultParam.IsGood == true) ? eNgType.GOOD : eNgType.EMPTY;
                }

                else if (eAlgoType.C_PATTERN == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogPatternResult;
                    SendNoneResult _SendResult = new SendNoneResult();

                    _SendResParam.AlgoTypeList[iLoopCount] = eAlgoType.C_PATTERN;
                    _SendResParam.IsGood &= _AlgoResultParam.IsGood;

                    if (_SendResParam.NgType == eNgType.GOOD)
                        _SendResParam.NgType = (_AlgoResultParam.IsGood == true) ? eNgType.GOOD : eNgType.REF_NG;

                    _SendResult.MatchingScore = _AlgoResultParam.Score[0];

                    _SendResParam.SendResultList[iLoopCount] = _SendResult;
                }
            }

            return _SendResParam;
        }
    }
}
