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
                    _SendResParam.IsGood = _AlgoResultParam.IsGood;
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

                    //결과 분석
                    _SendResult.EachLeadStatusArray = _AlgoResultParam.EachLeadStatusArray;

                    _SendResParam.SendResult = _SendResult;
                    _SendResParam.NgType = _AlgoResultParam.NgType;
                    _SendResParam.IsGood = _AlgoResultParam.IsGood;
                    _SendResParam.SearchArea = _AlgoResultParam.SearchArea;
                }
            }

            _SendResult.SaveImage = OriginImage;

            return _SendResParam;
        }

        private SendResultParameter GetLeadFormAlignResultAnalysis()
        {
            SendResultParameter _SendResParam = new SendResultParameter();
            _SendResParam.ID = ID;
            _SendResParam.NgType = eNgType.GOOD;
            _SendResParam.IsGood = true;
            _SendResParam.ProjectItem = ProjectItem;

            SendLeadFormResult _SendResult = new SendLeadFormResult();
            for (int iLoopCount = 0; iLoopCount < AlgoResultParamList.Count; ++iLoopCount)
            {
                if (eAlgoType.C_LINE_FIND == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogLineFindResult;
                    
                    //_SendResult.BodyReferenceX = (_AlgoResultParam.StartX + _AlgoResultParam.EndX) / 2;
                    //_SendResult.BodyReferenceY = (_AlgoResultParam.StartY + _AlgoResultParam.EndY) / 2;
                }

                else if (eAlgoType.C_LEAD_FORM == AlgoResultParamList[iLoopCount].ResultAlgoType)
                {
                    var _AlignResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogLeadFormResult;
                    _SendResult.LeadCount = _AlignResultParam.LeadCount;
                    _SendResult.LeadOffset = new PointD[_SendResult.LeadCount];
                    _SendResult.IsLeadOffsetGood = new bool[_SendResult.LeadCount];
                    if (_AlignResultParam.NgType != eNgType.LEAD_CNT)
                    {
                        for (int jLoopCount = 0; jLoopCount < _AlignResultParam.LeadCount; ++jLoopCount)
                        {
                            _SendResult.LeadOffset[jLoopCount] = new PointD();
                            _SendResult.LeadOffset[jLoopCount].X = _AlignResultParam.AlignOffsetDataList[jLoopCount].X;
                            _SendResult.LeadOffset[jLoopCount].Y = _AlignResultParam.AlignOffsetDataList[jLoopCount].Y;
                            _SendResult.IsLeadOffsetGood[jLoopCount] = _AlignResultParam.AlignResultDataList[jLoopCount].IsGood;
                        }
                    }

                    //결과 분석
                    _SendResult.EachLeadStatusArray = _AlignResultParam.EachLeadStatusArray;

                    _SendResParam.SendResult = _SendResult;
                    _SendResParam.NgType = _AlignResultParam.NgType;
                    _SendResParam.IsGood = _AlignResultParam.IsGood;
                    _SendResParam.SearchArea = _AlignResultParam.SearchArea;
                }
            }
            _SendResult.SaveImage = OriginImage;

            return _SendResParam;
        }
    }
}
