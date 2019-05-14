using System;
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

            for (int iLoopCount = 0; iLoopCount < AlgoResultParamList.Count; ++iLoopCount)
            {
                if (eAlgoType.C_ELLIPSE == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogEllipseResult;
                    SendEllipseResult _SendResult = new SendEllipseResult();

                    _SendResParam.IsGood = _AlgoResultParam.IsGood;

                    if (_SendResParam.NgType == eNgType.GOOD)
                        _SendResParam.NgType = (_AlgoResultParam.IsGood == true) ? eNgType.GOOD : eNgType.MEASURE;

                    _SendResult.RadiusX = _AlgoResultParam.RadiusX;
                    _SendResult.RadiusX = _AlgoResultParam.RadiusY;
                    _SendResParam.SendResult = _SendResult;
                }

                else if (eAlgoType.C_ID == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogBarCodeIDResult;
                    SendCardIDResult _SendResult = new SendCardIDResult();

                    for (int jLoopCount = 0; jLoopCount < _AlgoResultParam.IDResult.Length; jLoopCount++)
                    {
                        _SendResParam.IsGood &= _AlgoResultParam.IsGood;
                        _SendResult.ReadCode = (_AlgoResultParam.IsGood == true) ? _AlgoResultParam.IDResult[jLoopCount] : "";
                        if (_SendResParam.NgType == eNgType.GOOD)
                            _SendResParam.NgType = (_AlgoResultParam.IsGood == true) ? eNgType.GOOD : eNgType.ID;
                    }

                    _SendResParam.SendResult = _SendResult;
                }

                else if (eAlgoType.C_LINE_FIND == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogLineFindResult;

                    _SendResParam.IsGood = _AlgoResultParam.IsGood;
                    if (_SendResParam.NgType == eNgType.GOOD)
                        _SendResParam.NgType = (_AlgoResultParam.IsGood == true) ? eNgType.GOOD : eNgType.EMPTY;
                }

                else if (eAlgoType.C_PATTERN == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogPatternResult;
                    SendNoneResult _SendResult = new SendNoneResult();
                    _SendResParam.IsGood &= _AlgoResultParam.IsGood;
                    if (_SendResParam.NgType == eNgType.GOOD)
                        _SendResParam.NgType = (_AlgoResultParam.IsGood == true) ? eNgType.GOOD : eNgType.REF_NG;

                    _SendResult.MatchingScore = _AlgoResultParam.Score[0];

                    _SendResParam.SendResult = _SendResult;
                }
            }

            return _SendResParam;
        }
    }
}
