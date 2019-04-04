using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        #region Initialize & DeInitialize
        public MainProcessCardManager()
        {

        }

        public override void Initialize(string _CommonFolderPath)
        {
            EthernetServerWnd = new EthernetWindow();
            EthernetServerWnd.Initialize(_CommonFolderPath);
            EthernetServerWnd.ReceiveStringEvent += new EthernetWindow.ReceiveStringHandler(EthernetRecvString);
        }

        public override void DeInitialize()
        {

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
        #endregion Ethernet Window Function
    }
}
