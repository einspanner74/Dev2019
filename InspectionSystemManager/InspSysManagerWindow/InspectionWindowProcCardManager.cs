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
        private SendResultParameter GetCardImageSaveResultAnalysis()
        {
            SendResultParameter _SendResParam = new SendResultParameter();
            _SendResParam.ID = ID;
            _SendResParam.NgType = eNgType.GOOD;
            _SendResParam.IsGood = true;
            _SendResParam.ProjectItem = ProjectItem;

            SendCardImageSaveResult _SendResult = new SendCardImageSaveResult();
            _SendResult.SaveImage = OriginImage;
            _SendResParam.SendResult = _SendResult;

            return _SendResParam;
        }

        private SendResultParameter GetCardExistResultAnalysis()
        {
            SendResultParameter _SendResParam = new SendResultParameter();
            _SendResParam.ID = ID;
            _SendResParam.NgType = eNgType.GOOD;
            _SendResParam.IsGood = false;
            _SendResParam.ProjectItem = ProjectItem;

            SendCardExistResult _SendResult = new SendCardExistResult();
            _SendResult.IsGoods = new bool[2];
            _SendResult.IsGoods[0] = true;
            _SendResult.IsGoods[1] = true;
            _SendResult.SaveImage = OriginImage;

            for (int iLoopCount = 0; iLoopCount < AlgoResultParamList.Count; ++iLoopCount)
            {
                var _AlgoResultParam = AlgoResultParamList[iLoopCount].ResultParam as CogLineFindResult;
                if (iLoopCount < 1) _SendResult.IsGoods[0] &= _AlgoResultParam.IsGood;
                else                _SendResult.IsGoods[1] &= _AlgoResultParam.IsGood;
            }

            //LDH, 2019.03.21, 카드가 1개도 없으면 NG 처리
            for(int jLoopCount = 0; jLoopCount <2; jLoopCount++) _SendResParam.IsGood |= _SendResult.IsGoods[0];

            _SendResParam.SendResult = _SendResult;

            return _SendResParam;
        }

        private SendResultParameter GetCardIDResultAnalysis()
        {
            SendResultParameter _SendResParam = new SendResultParameter();
            _SendResParam.ID = ID;
            _SendResParam.NgType = eNgType.GOOD;
            _SendResParam.IsGood = true;
            _SendResParam.ProjectItem = ProjectItem;

            for (int iLoopCount = 0; iLoopCount < AlgoResultParamList.Count; ++iLoopCount)
            {
                if (eAlgoType.C_ID == AlgoResultParamList[iLoopCount].ResultAlgoType)
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
            }

            return _SendResParam;
        }
    }
}
