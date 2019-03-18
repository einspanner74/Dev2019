using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

using CustomControl;
using ParameterManager;
using LogMessageManager;
using HistoryManager;

namespace KPVisionInspectionFramework
{
    public partial class ucMainResultSorter : UserControl
    {
        #region Initialize & DeInitialize
        public ucMainResultSorter(string[] _LastRecipeName)
        {
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

        public void SetSurfaceResultData(SendResultParameter _ResultParam)
        {

        }

        public void ClearResult()
        {

        }
    }
}
