using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Cognex.VisionPro;
using ParameterManager;

namespace InspectionSystemManager
{
    public partial class InspectionWindow : Form
    {
        private SendResultParameter GetSurfaceInspectionResultAnalysis()
        {
            SendResultParameter _SendResParam = new SendResultParameter();
            _SendResParam.ID = ID;
            _SendResParam.NgType = eNgType.GOOD;
            _SendResParam.IsGood = true;
            _SendResParam.ProjectItem = ProjectItem;

            for (int iLoopCount = 0; iLoopCount < AlgoResultParamList.Count; ++iLoopCount)
            {
                if (eAlgoType.C_MULTI_PATTERN == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogMultiPatternResult;
                    SendSurfaceResult _SendResult = new SendSurfaceResult();

                    _SendResParam.IsGood &= _AlgoResultParam.IsGood;
                    _SendResult.TwoPointAngle = (_AlgoResultParam.IsGood == true) ? _AlgoResultParam.TwoPointAngle : 0.0;
                    if (_SendResParam.NgType == eNgType.GOOD)
                        _SendResParam.NgType = (_AlgoResultParam.IsGood == true) ? eNgType.GOOD : eNgType.M_REF;

                    _SendResParam.SendResult = _SendResult;
                }
            }

            return _SendResParam;
        }
    }
}
