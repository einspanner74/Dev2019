using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using LogMessageManager;
using ParameterManager;
using DIOControlManager;
using SerialManager;
using EthernetServerManager;

namespace KPVisionInspectionFramework
{
    class MainProcessCardManager : MainProcessBase
    {
        EthernetWindow[] EthernetServerWnd;

        EthernetRecvInfo[] RecvInfo;

        private Thread[] ThreadGetReceiveData;
        private bool[] IsThreadGetReceiveDataTrigger;
        private bool[] IsThreadGetReceiveDataExit;

        private delegate void ThreadGetReceiveDataFunction();
        private ThreadGetReceiveDataFunction[] ThreadGetReceiveDataFunctionArray;

        #region Initialize & DeInitialize
        public MainProcessCardManager()
        {

        }

        public override void Initialize(string _CommonFolderPath, bool _IsIOBoardUsable, bool _IsEthernetUsable)
        {
            RecvInfo = new EthernetRecvInfo[4];

            EthernetServerWnd = new EthernetWindow[4];

            ThreadGetReceiveData = new Thread[4];
            IsThreadGetReceiveDataTrigger = new bool[4];
            IsThreadGetReceiveDataExit = new bool[4];

            ThreadGetReceiveDataFunctionArray = new ThreadGetReceiveDataFunction[4] { ThreadGetReceiveDataFunction1, ThreadGetReceiveDataFunction2, ThreadGetReceiveDataFunction3, ThreadGetReceiveDataFunction4 };

            for (int iLoopCount = 0; iLoopCount < 4; iLoopCount++)
            {
                RecvInfo[iLoopCount] = new EthernetRecvInfo();
                RecvInfo[iLoopCount].PortNumber = Convert.ToInt16(5000 + iLoopCount);

                EthernetServerWnd[iLoopCount] = new EthernetWindow();
                EthernetServerWnd[iLoopCount].Initialize(_CommonFolderPath, (short)iLoopCount);
                EthernetServerWnd[iLoopCount].ReceiveStringEvent += new EthernetWindow.ReceiveStringHandler(GetEthernetRecvData);

                //LDH, 2019.04.04, Receive Data Queue 관리용 Thread 추가
                //switch(iLoopCount)
                //{
                //    case 0: ThreadGetReceiveData[iLoopCount] = new Thread(ThreadGetReceiveDataFunction1); break;
                //    case 1: ThreadGetReceiveData[iLoopCount] = new Thread(ThreadGetReceiveDataFunction2); break;
                //    case 2: ThreadGetReceiveData[iLoopCount] = new Thread(ThreadGetReceiveDataFunction3); break;
                //    case 3: ThreadGetReceiveData[iLoopCount] = new Thread(ThreadGetReceiveDataFunction4); break;
                //}
                //LJH, 2019.05.02 Receive Data Queue 관리용 Thread 추가 부분 수정
                ThreadGetReceiveData[iLoopCount] = new Thread(ThreadGetReceiveDataFunctionArray[iLoopCount].Invoke);

                IsThreadGetReceiveDataTrigger[iLoopCount] = false;
                IsThreadGetReceiveDataExit[iLoopCount] = false;
                ThreadGetReceiveData[iLoopCount].Start();
            }
        }

        public override void DeInitialize()
        {
            for (int iLoopCount = 0; iLoopCount < 4; iLoopCount++)
            {
                EthernetServerWnd[iLoopCount].ReceiveStringEvent -= new EthernetWindow.ReceiveStringHandler(GetEthernetRecvData);
                EthernetServerWnd[iLoopCount].DeInitialize();

                if (ThreadGetReceiveData[iLoopCount] != null) { IsThreadGetReceiveDataExit[iLoopCount] = true; Thread.Sleep(200); ThreadGetReceiveData[iLoopCount].Abort(); ThreadGetReceiveData[iLoopCount] = null; }
            }
        }
        #endregion Initialize & DeInitialize    

        #region Ethernet Window Function
        public override void ShowEthernetWindow()
        {
            for (int iLoopCount = 0; iLoopCount < 4; iLoopCount++)
            {
                EthernetServerWnd[iLoopCount].Show();
            }
        }

        public override bool GetEhernetWindowShown()
        {
            return EthernetServerWnd[0].IsShowWindow;
        }

        public override void SetEthernetWindowTopMost(bool _IsTopMost)
        {
            for (int iLoopCount = 0; iLoopCount < 4; iLoopCount++)
            {
                EthernetServerWnd[iLoopCount].TopMost = _IsTopMost;
            }
        }

        //LDH, 2019.04.26, 일반 Data 전송
        public override void SendSerialData(eMainProcCmd _SendCmd, string _PortNumber = "")
        {
            int PortNum = Convert.ToInt32(_PortNumber) - 5000;

            if (eMainProcCmd.ACK_COMPLETE == _SendCmd)
            {
                EthernetServerWnd[PortNum].SendResultData(">2000", false);
            } 
        }

        //LDH, 2019.04.26, 검사결과 Result 전송
        public override bool SendResultData(SendResultParameter _ResultParam)
        {
            bool _Result = true;

            bool _ResultFlag = _ResultParam.IsGood;

            if (_ResultFlag) EthernetServerWnd[_ResultParam.ID].SendResultData(">Pass", false);
            else             EthernetServerWnd[_ResultParam.ID].SendResultData(">Fail", false);

            return _Result;
        }

        //LDH, 2019.04.04, Receive Data Queue에 담는 함수
        private bool GetEthernetRecvData(string[] _RecvData, int _PortNumber)
        {
            int EthernetNum = _PortNumber - 5000;

            try
            {
                RecvInfo[EthernetNum].RecvData = null;
                RecvInfo[EthernetNum].SetRecvInfo(_PortNumber, _RecvData);
                IsThreadGetReceiveDataTrigger[EthernetNum] = true;
            }
            catch
            {
                return false;
            }

            return true;
        }
        #endregion Ethernet Window Function
        
        //LDH, 2019.04.04, Receive Data Queue 처리 Thread
        private void ThreadGetReceiveDataFunction1()
        {
            try
            {
                while (false == IsThreadGetReceiveDataExit[0])
                {
                    if (IsThreadGetReceiveDataTrigger[0])
                    {
                        IsThreadGetReceiveDataTrigger[0] = false;
                        OnMainProcessCommand(eMainProcCmd.RECV_DATA, RecvInfo[0]);

                        switch(RecvInfo[0].RecvData[0])
                        {
                            case "00": /*Send*/ break;
                            case "GO": OnMainProcessCommand(eMainProcCmd.TRG, 0); break;
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (System.Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "ThreadGetReceiveDataFunction Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
            }
        }

        private void ThreadGetReceiveDataFunction2()
        {
            try
            {
                while (false == IsThreadGetReceiveDataExit[1])
                {
                    if (IsThreadGetReceiveDataTrigger[1])
                    {
                        IsThreadGetReceiveDataTrigger[1] = false;
                        OnMainProcessCommand(eMainProcCmd.RECV_DATA, RecvInfo[1]);
                        switch (RecvInfo[1].RecvData[0])
                        {
                            case "00": /*Send*/ break;
                            default: OnMainProcessCommand(eMainProcCmd.TRG, 1); break;
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (System.Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "ThreadGetReceiveDataFunction Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
            }
        }

        private void ThreadGetReceiveDataFunction3()
        {
            try
            {
                while (false == IsThreadGetReceiveDataExit[2])
                {
                    if (IsThreadGetReceiveDataTrigger[2])
                    {
                        IsThreadGetReceiveDataTrigger[2] = false;
                        OnMainProcessCommand(eMainProcCmd.RECV_DATA, RecvInfo[2]);
                        switch (RecvInfo[2].RecvData[0])
                        {
                            case "00": /*Send*/ break;
                            default: OnMainProcessCommand(eMainProcCmd.TRG, 2); break;
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (System.Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "ThreadGetReceiveDataFunction Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
            }
        }

        private void ThreadGetReceiveDataFunction4()
        {
            try
            {
                while (false == IsThreadGetReceiveDataExit[3])
                {
                    if (IsThreadGetReceiveDataTrigger[3])
                    {
                        IsThreadGetReceiveDataTrigger[3] = false;
                        OnMainProcessCommand(eMainProcCmd.RECV_DATA, RecvInfo[3]);
                        switch (RecvInfo[3].RecvData[0])
                        {
                            case "00": /*Send*/ break;
                            default: OnMainProcessCommand(eMainProcCmd.TRG, 3); break;
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (System.Exception ex)
            {
                CLogManager.AddSystemLog(CLogManager.LOG_TYPE.ERR, "ThreadGetReceiveDataFunction Exception : " + ex.ToString(), CLogManager.LOG_LEVEL.LOW);
            }
        }
    }
}
