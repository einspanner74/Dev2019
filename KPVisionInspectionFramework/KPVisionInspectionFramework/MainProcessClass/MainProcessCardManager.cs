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
        EthernetWindow EthernetServerWnd;

        //public delegate void EthernetRecvStringHandler(string[] _EthernetRecvMessage);
        //public event EthernetRecvStringHandler EthernetRecvStringEvent;

        private Queue<string[]> RecvDataQueue = new Queue<string[]>();

        private Thread ThreadGetReceiveData;
        private bool IsThreadGetReceiveDataExit = false;

        #region Initialize & DeInitialize
        public MainProcessCardManager()
        {

        }

        public override void Initialize(string _CommonFolderPath)
        {
            EthernetServerWnd = new EthernetWindow();
            EthernetServerWnd.Initialize(_CommonFolderPath);
            EthernetServerWnd.ReceiveStringEvent += new EthernetWindow.ReceiveStringHandler(GetEthernetRecvData);

            //LDH, 2019.04.04, Receive Data Queue 관리용 Thread 추가
            ThreadGetReceiveData = new Thread(ThreadGetReceiveDataFunction);
            IsThreadGetReceiveDataExit = false;
            ThreadGetReceiveData.Start();
        }

        public override void DeInitialize()
        {
            EthernetServerWnd.ReceiveStringEvent -= new EthernetWindow.ReceiveStringHandler(GetEthernetRecvData);
            EthernetServerWnd.DeInitialize();

            if (ThreadGetReceiveData != null) { IsThreadGetReceiveDataExit = true; Thread.Sleep(200); ThreadGetReceiveData.Abort(); ThreadGetReceiveData = null; }
        }
        #endregion Initialize & DeInitialize    

        #region Ethernet Window Function
        public override void ShowEthernetWindow()
        {
            EthernetServerWnd.Show();
        }

        public override bool GetEhernetWindowShown()
        {
            return EthernetServerWnd.IsShowWindow;
        }

        public override void SetEthernetWindowTopMost(bool _IsTopMost)
        {
            EthernetServerWnd.TopMost = _IsTopMost;
        }

        //LDH, 2019.04.04, Receive Data Queue에 담는 함수
        private bool GetEthernetRecvData(string[] _RecvData)
        {
            try
            {
                RecvDataQueue.Enqueue(_RecvData);
            }
            catch
            {
                return false;
            }

            return true;
        }
        #endregion Ethernet Window Function
        
        //LDH, 2019.04.04, Receive Data Queue 처리 Thread
        private void ThreadGetReceiveDataFunction()
        {
            try
            {
                while (false == IsThreadGetReceiveDataExit)
                {
                    if (RecvDataQueue.Count != 0) EthernetRecvString(RecvDataQueue.Dequeue());
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
