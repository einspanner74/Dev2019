using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionSystemManager;
using LogMessageManager;
using ParameterManager;
using DIOControlManager;
using EthernetServerManager;

namespace KPVisionInspectionFramework
{
    public class MainProcessSorter : MainProcessBase
    {
        private DIOControlWindow DIOWnd;
        private EthernetWindow EthernetServWnd;

        public MainProcessSorter()
        {

        }

        public override void Initialize(string CommonFolderPath)
        {
            
        }

        public override void DeInitialize()
        {
            
        }
    }
}
