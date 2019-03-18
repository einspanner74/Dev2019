using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CustomControl;
using ParameterManager;
using LogMessageManager;

namespace KPVisionInspectionFramework
{
    public partial class ucMainResultCardManager : UserControl
    {
        private bool AutoModeFlag = false;

        private string[] HistoryParam;
        private string[] LastRecipeName;
        private string LastResult;

        public delegate void ScreenshotHandler(string ScreenshotImagePath);
        public event ScreenshotHandler ScreenshotEvent;

        #region Initialize & DeInitialize
        public ucMainResultCardManager(string[] _LastRecipeName)
        {
            LastRecipeName = new string[_LastRecipeName.Count()];
            SetLastRecipeName(_LastRecipeName);

            InitializeComponent();
            InitializeControl();
            this.Location = new Point(1, 1);
        }

        private void InitializeControl()
        {

        }

        public void DeInitialize()
        {

        }
        #endregion Initialize & DeInitialize

        public void SetAutoMode(bool _AutoModeFlag)
        {

        }

        public void SetLastRecipeName(string[] _LastRecipeName)
        {
            for (int iLoopCount = 0; iLoopCount < _LastRecipeName.Count(); iLoopCount++)
            {
                LastRecipeName[iLoopCount] = _LastRecipeName[iLoopCount];
            }
        }

        //LDH, 2018.10.01, Result clear
        public void ClearResult()
        {

        }

        public void SetImageSaveResultData(SendResultParameter _ResultParam)
        {

        }

        public void SetQrCodResultData(SendResultParameter _ResultParam)
        {

        }

        public void SetExistResultData(SendResultParameter _ResultParam)
        {

        }
    }
}
