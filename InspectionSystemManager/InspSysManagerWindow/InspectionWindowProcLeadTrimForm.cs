using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private SendResultParameter GetLeadTrimInspectionResultAnalysis()
        {
            SendResultParameter _SendResParam = new SendResultParameter();
            _SendResParam.ID = ID;
            _SendResParam.NgType = eNgType.GOOD;
            _SendResParam.IsGood = true;
            _SendResParam.ProjectItem = ProjectItem;

            SendLeadTrimResult _SendResult = new SendLeadTrimResult();
            for (int iLoopCount = 0; iLoopCount < AlgoResultParamList.Count; ++iLoopCount)
            {
                if (eAlgoType.C_LINE_FIND == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogLineFindResult;

                    //_SendResult.BodyReferenceX = (_AlgoResultParam.StartX + _AlgoResultParam.EndX) / 2;
                    //_SendResult.BodyReferenceY = (_AlgoResultParam.StartY + _AlgoResultParam.EndY) / 2;
                }

                else if (eAlgoType.C_LEAD_TRIM == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogLeadTrimResult;

                    _SendResult.LeadCount = _AlgoResultParam.LeadCount;
                    _SendResult.LeadLengthReal = _AlgoResultParam.LeadLength;
                    _SendResult.IsLeadLengthGood = _AlgoResultParam.IsLeadLengthGood;

                    _SendResult.LeadPitchReal = _AlgoResultParam.LeadPitchLength;
                    _SendResult.IsLeadPitchGood = _AlgoResultParam.IsLeadBentGood;

                    _SendResParam.SendResult = _SendResult;
                    _SendResParam.NgType = _AlgoResultParam.NgType;
                    _SendResParam.IsGood = _AlgoResultParam.IsGood;
                }
            }

            return _SendResParam;
        }

        private SendResultParameter GetLeadFormAlignResultAnalysis()
        {
            SendResultParameter _SendResParam = new SendResultParameter();
            _SendResParam.ID = ID;
            _SendResParam.NgType = eNgType.GOOD;
            _SendResParam.IsGood = true;
            _SendResParam.ProjectItem = ProjectItem;

            SendLeadResult _SendResult = new SendLeadResult();
            for (int iLoopCount = 0; iLoopCount < AlgoResultParamList.Count; ++iLoopCount)
            {

            }

            return _SendResParam;
        }
    }
}
