using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using InspectionSystemManager;
using LogMessageManager;
using ParameterManager;
using DIOControlManager;
using EthernetServerManager;

namespace KPVisionInspectionFramework
{
    class MainProcessTrimForm : MainProcessBase
    {
        public const string CR = "cr";

        public DIOControlWindow DIOWnd;
        public EthernetWindow EthernetServWnd;

        private AckStruct[] AckStructs;

        private bool UseDIOCommFlag = true;

        private Thread ThreadAckSignalCheck;
        private bool IsThreadAckSignalCheckExit;
        private bool AckSignal = false;

        private short WaitingPeriod = 50;
        private short WaitingLimitTime = 5000;

        string LOTNum = "";
        string CarModel = "";

        #region Initialize & DeInitialize
        public MainProcessTrimForm()
        {

        }

        public override void Initialize(string _CommonFolderPath, bool _IsIOBoardUsable, bool _IsEthernetUsable)
        {
            UseDIOCommFlag = _IsIOBoardUsable;

            if (UseDIOCommFlag)
            {
                DIOWnd = new DIOControlWindow((int)eProjectType.TRIM_FORM, _CommonFolderPath);
                DIOWnd.InputChangedEvent += new DIOControlWindow.InputChangedHandler(InputChangeEventFunction);
                DIOWnd.Initialize();

                EthernetServWnd = new EthernetWindow();
                EthernetServWnd.Initialize(_CommonFolderPath, 0);
                EthernetServWnd.ReceiveStringEvent += new EthernetWindow.ReceiveStringHandler(ReceiveStringEventFunction);

                int _CompleteCmdBit = DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_COMPLETE);
                int _ReadyCmdBit = DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_READY);
                int _ResultCmdBit = DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_RESULT);
                int _LiveCmdBit = DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_LIVE);

                if (_CompleteCmdBit >= 0) DIOWnd.SetOutputSignal((short)_CompleteCmdBit, false);
                if (_ReadyCmdBit >= 0) DIOWnd.SetOutputSignal((short)_ReadyCmdBit, false);
                if (_ResultCmdBit >= 0) DIOWnd.SetOutputSignal((short)_ResultCmdBit, false);
                if (_LiveCmdBit >= 0) DIOWnd.SetOutputSignal((short)_LiveCmdBit, true);

                AckStructs = new AckStruct[3];
                for (int iLoopCount = 0; iLoopCount < 3; ++iLoopCount) AckStructs[iLoopCount] = new AckStruct(); ;

                ThreadAckSignalCheck = new Thread(ThreadAckSignalCheckFunc);
                ThreadAckSignalCheck.IsBackground = true;
                IsThreadAckSignalCheckExit = false;
            }
        }

        public override void DeInitialize()
        {
            if (UseDIOCommFlag)
            {
                int _CompleteCmdBit = DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_COMPLETE);
                int _ReadyCmdBit = DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_READY);
                int _ResultCmdBit = DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_RESULT);
                int _LiveCmdBit = DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_LIVE);

                if (_CompleteCmdBit >= 0) DIOWnd.SetOutputSignal((short)_CompleteCmdBit, false);
                if (_ReadyCmdBit >= 0) DIOWnd.SetOutputSignal((short)_ReadyCmdBit, false);
                if (_ResultCmdBit >= 0) DIOWnd.SetOutputSignal((short)_ResultCmdBit, false);
                if (_LiveCmdBit >= 0) DIOWnd.SetOutputSignal((short)_LiveCmdBit, false);

                DIOWnd.InputChangedEvent -= new DIOControlWindow.InputChangedHandler(InputChangeEventFunction);
                DIOWnd.DeInitialize();
            }

            EthernetServWnd.ReceiveStringEvent -= new EthernetWindow.ReceiveStringHandler(ReceiveStringEventFunction);
            EthernetServWnd.DeInitialize();

            if (ThreadAckSignalCheck != null) { IsThreadAckSignalCheckExit = true; Thread.Sleep(200); ThreadAckSignalCheck.Abort(); ThreadAckSignalCheck = null; }

        }
        #endregion Initialize & DeInitialize

        #region DIO Window Function
        public override void ShowDIOWindow()
        {
            if(DIOWnd != null) DIOWnd.ShowDIOWindow();
        }

        public override bool GetDIOWindowShown()
        {
            return DIOWnd.IsShowWindow;
        }

        public override void SetDIOWindowTopMost(bool _IsTopMost)
        {
            DIOWnd.TopMost = _IsTopMost;
        }

        public override void SetDIOOutputSignal(short _BitNumber, bool _Signal)
        {
            DIOWnd.SetOutputSignal(_BitNumber, _Signal);
        }
        #endregion DIO Window Function

        #region Ethernet Window Function
        public override void ShowEthernetWindow()
        {
            EthernetServWnd.Show();
        }

        public override bool GetEhernetWindowShown()
        {
            return EthernetServWnd.IsShowWindow;
        }

        public override void SetEthernetWindowTopMost(bool _IsTopMost)
        {
            for (int iLoopCount = 0; iLoopCount < 4; iLoopCount++)
            {
                EthernetServWnd.TopMost = _IsTopMost;
            }
        }
        #endregion Ethernet Window Function

        public override bool AutoMode(bool _Flag)
        {
            bool _Result = true;

            int _AutoCmdBit = DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_AUTO);
            DIOWnd.SetOutputSignal((short)_AutoCmdBit, _Flag);

            int _CompleteBit = DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_COMPLETE);
            DIOWnd.SetOutputSignal((short)_CompleteBit, false);

            return _Result;
        }

        //LDH, 내부 Test 용. 실제로는 Camera Trigger로 동작
        public override bool TriggerOn(int _ID)
        {
            bool _Result = false;

            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, String.Format("Main : Trigger{0} On Event", _ID + 1));
            OnMainProcessCommand(eMainProcCmd.TRG, _ID);

            return _Result;
        }

        public override bool Reset(int _ID)
        {
            bool _Result = false;
            int _CompleteCmdBit, _ReadyCmdBit;

            if (0 == _ID)
            {
                _CompleteCmdBit = (short)DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_COMPLETE);
                _ReadyCmdBit = (short)DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_READY);
            }
            else
            {
                _CompleteCmdBit = (short)DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_COMPLETE_2);
                _ReadyCmdBit = (short)DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_READY_2);
            }

            if (_CompleteCmdBit >= 0) DIOWnd.SetOutputSignal((short)_CompleteCmdBit, false);
            if (_ReadyCmdBit >= 0) DIOWnd.SetOutputSignal((short)_ReadyCmdBit, true,1000);

            return _Result;
        }

        public override bool SendResultData(SendResultParameter _ResultParam)
        {
            bool _Result = true;

            string _VisionString, _ResultString;
            _VisionString = String.Format("V{0}", _ResultParam.ID + 1);

            if (_ResultParam.IsGood) _ResultString = "OK";
            else _ResultString = "NG";

            string LeadStatus = "";
            string MoldStatus = "";

            if (_ResultParam.ProjectItem == eProjectItem.LEAD_TRIM_INSP)
            {
                var _SendResult = _ResultParam.SendResult as SendLeadTrimResult;

                //LDH, 2019.06.21, NG Type 우선순위에 따라 순차적으로 지정
                if (_SendResult != null)
                {
                    for (int iLoopCount = 0; iLoopCount < _SendResult.EachLeadStatusArray.Count(); iLoopCount++)
                    {
                        List<eLeadStatus> _ListTemp = _SendResult.EachLeadStatusArray[iLoopCount].NgTypeList;

                        if      (_ListTemp.Contains(eLeadStatus.SHLD_NICK))         { LeadStatus = LeadStatus + ((int)eLeadStatus.SHLD_NICK).ToString(); continue; }
                        else if (_ListTemp.Contains(eLeadStatus.SHLD_BURR))         { LeadStatus = LeadStatus + ((int)eLeadStatus.SHLD_BURR).ToString(); continue; }
                        else if (_ListTemp.Contains(eLeadStatus.TIP_BURR))          { LeadStatus = LeadStatus + ((int)eLeadStatus.TIP_BURR).ToString(); continue; }
                        else if (_ListTemp.Contains(eLeadStatus.LEAD_LENGTH))       { LeadStatus = LeadStatus + ((int)eLeadStatus.LEAD_LENGTH).ToString(); continue; }
                        else if (_ListTemp.Contains(eLeadStatus.LEAD_SKEW_DISABLE)) { LeadStatus = LeadStatus + ((int)eLeadStatus.LEAD_SKEW_DISABLE).ToString(); continue; }
                        else if (_ListTemp.Contains(eLeadStatus.LEAD_SKEW_ENABLE))  { LeadStatus = LeadStatus + ((int)eLeadStatus.LEAD_SKEW_ENABLE).ToString(); continue; }
                        else                                                        { LeadStatus = LeadStatus + ((int)eLeadStatus.GOOD).ToString(); continue; }                    
                    }

                    switch(_ResultParam.NgType)
                    {
                        case eNgType.GOOD:     MoldStatus = "0"; break;
                        case eNgType.CHIP_OUT: MoldStatus = "1"; break;
                        case eNgType.GATE_ERR: MoldStatus = "2"; break;
                        case eNgType.LEAD_CNT: MoldStatus = "3"; break;
                        case eNgType.EMPTY:    MoldStatus = "4"; break;
                    }
                }
                else
                {
                    LeadStatus = "00000000000000000000";
                    _ResultString = "NG";
                }                                
            }

            else if (_ResultParam.ProjectItem == eProjectItem.LEAD_FORM_ALIGN)
            {               
                var _SendResult = _ResultParam.SendResult as SendLeadFormResult;

                if (_SendResult != null)
                {
                    for (int iLoopCount = 0; iLoopCount < _SendResult.EachLeadStatusArray.Count(); iLoopCount++)
                    {
                        List<eLeadStatus> _ListTemp = _SendResult.EachLeadStatusArray[iLoopCount].NgTypeList;

                        if      (_ListTemp.Contains(eLeadStatus.LEAD_SKEW_DISABLE)) { LeadStatus = LeadStatus + ((int)eLeadStatus.LEAD_SKEW_DISABLE).ToString(); continue; }
                        else if (_ListTemp.Contains(eLeadStatus.LEAD_SKEW_ENABLE))  { LeadStatus = LeadStatus + ((int)eLeadStatus.LEAD_SKEW_ENABLE).ToString(); continue; }
                        else                                                        { LeadStatus = LeadStatus + ((int)eLeadStatus.GOOD).ToString(); continue; }
                    }

                    switch (_ResultParam.NgType)
                    {
                        case eNgType.GOOD:     MoldStatus = "0"; break;
                        case eNgType.LEAD_CNT: MoldStatus = "3"; break;
                    }
                }
                else
                {
                    LeadStatus = "00000000000000000000";
                    _ResultString = "NG";
                }
            }

            string _ResultDataString = string.Format("{0},{1},{2},{3},{4}", _VisionString, _ResultString, LeadStatus, MoldStatus, LOTNum);
            EthernetServWnd.SendResultData(_ResultDataString, true);
            AckStructs[_ResultParam.ID].Initialize();

            return _Result;
        }

        #region Communication Event Function
        private void InputChangeEventFunction(short _BitNum, bool _Signal)
        {
            switch (_BitNum)
            {
                case DIO_DEF.IN_RESET:      Reset(0);       break;
                case DIO_DEF.IN_TRG:        TriggerOn(0);   break;
                case DIO_DEF.IN_REQUEST:    DataRequest(0); break;
                case DIO_DEF.IN_RESET_2:    Reset(1);       break;
                case DIO_DEF.IN_TRG_2:      TriggerOn(1);   break;
                case DIO_DEF.IN_REQUEST_2:  DataRequest(1); break;
            }
        }

        /// <summary>
        /// Ethernet => MainProcess Event
        /// </summary>
        /// <param name="_RecvMessage"></param>
        private bool ReceiveStringEventFunction(string[] _RecvMessage, int _PortNum)
        {
            if (_RecvMessage[0] == "V1")
            {
                AckStructs[0].AckComplete = true;                
                LOTChange(_RecvMessage);
                TriggerOn(0);
            }
            else if (_RecvMessage[0] == "V2")
            {
                AckStructs[1].AckComplete = true;
                LOTChange(_RecvMessage);
                TriggerOn(1);
            }

            return true;
        }

        private string[] ChangeResult(double _ResultX, double _ResultY)
        {
            string[] LastResult = new string[2];
            int[] ResultTemp = new int[2];

            ResultTemp[0] = Convert.ToInt32(_ResultX * 100);
            ResultTemp[1] = Convert.ToInt32(_ResultY * 100);

            for (int iLoopCount = 0; iLoopCount < 2; iLoopCount++)
            {
                if (Math.Abs(ResultTemp[iLoopCount]) < 10000)
                {
                    if (ResultTemp[iLoopCount] < 0) { ResultTemp[iLoopCount] = -ResultTemp[iLoopCount]; LastResult[iLoopCount] = "0"; }
                    else { LastResult[iLoopCount] = "1"; }

                    for (int multipleCnt = 3; multipleCnt >= 0; multipleCnt--)
                    {
                        LastResult[iLoopCount] = LastResult[iLoopCount] + Math.Truncate(ResultTemp[iLoopCount] / Math.Pow(10, multipleCnt));
                        ResultTemp[iLoopCount] = Convert.ToInt32(Math.Truncate(ResultTemp[iLoopCount] % Math.Pow(10, multipleCnt)));
                    }
                }
                else LastResult[iLoopCount] = "00000";
            }

            return LastResult;
        }

        public override bool DataRequest(int _ID)
        {
            bool _Result = true;

            OnMainProcessCommand(eMainProcCmd.REQUEST, _ID);

            return _Result;
        }

        public override bool InspectionComplete(int _ID, bool _Flag)
        {
            bool _Result = true;
            int _CompleteCmdBit = 0;

            if (_ID == 0)       _CompleteCmdBit = DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_COMPLETE);
            else if (_ID == 1)  _CompleteCmdBit = DIOWnd.DioBaseCmd.OutputBitIndexCheck((int)DIO_DEF.OUT_COMPLETE_2);
            DIOWnd.SetOutputSignal((short)_CompleteCmdBit, _Flag);

            return _Result;
        }

        private void LOTChange(string[] _RecvMessage)
        {
            LOTNum = _RecvMessage[1];
            CarModel = _RecvMessage[2];
            OnMainProcessCommand(eMainProcCmd.RECV_DATA, _RecvMessage);
        }
        #endregion Communication Event Function

        private void ThreadAckSignalCheckFunc()
        {
            try
            {
                while (false == IsThreadAckSignalCheckExit)
                {
                    Thread.Sleep(WaitingPeriod);

                    for (int iLoopCount = 0; iLoopCount < AckStructs.Length; ++iLoopCount)
                    {
                        if (true == AckStructs[iLoopCount].AckRequest) AckStructs[iLoopCount].WaitTime += WaitingPeriod;
                        if (true == AckStructs[iLoopCount].AckComplete) AckStructs[iLoopCount].SetStatus(ACK_STATUS.COMPLETE, false);
                        if (AckStructs[iLoopCount].WaitTime >= WaitingLimitTime) AckStructs[iLoopCount].SetStatus(ACK_STATUS.TIME_OVER, false);
                    }
                }
            }

            catch
            {

            }
        }
    }

    public class AckStruct
    {
        public ACK_STATUS AckStatus = ACK_STATUS.WAIT;
        public bool AckRequest = false;
        public bool AckComplete = false;
        public short WaitTime = 0;

        public void Initialize()
        {
            WaitTime = 0;
            AckRequest = true;
            AckComplete = false;
            AckStatus = ACK_STATUS.WAIT;
        }

        public void SetStatus(ACK_STATUS _AckStatus, bool _AckRequest)
        {
            AckStatus = _AckStatus;
            AckRequest = _AckRequest;
        }
    }

    public enum ACK_STATUS { TIME_OVER, COMPLETE, FAIL, WAIT }
}
