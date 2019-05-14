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
    public partial class ucMainResultNavien : UserControl
    {
        #region Count & Yield Variable
        private uint TotalCount
        {
            set { SegmentValueInvoke(SevenSegTotal1, value.ToString()); }
            get { return Convert.ToUInt32(SevenSegTotal1.Value); }
        }

        private uint GoodCount
        {
            set { SegmentValueInvoke(SevenSegGood1, value.ToString()); }
            get { return Convert.ToUInt32(SevenSegGood1.Value); }
        }

        private uint NgCount
        {
            set { SegmentValueInvoke(SevenSegNg1, value.ToString()); }
            get { return Convert.ToUInt32(SevenSegNg1.Value); }
        }

        private double Yield
        {
            set { SegmentValueInvoke(SevenSegYield1, value.ToString()); }
            get { return Convert.ToDouble(SevenSegYield1.Value); }
        }

        private bool AutoModeFlag = false;

        #endregion Count & Yield Variable

        private string[] LastRecipeName;

        public delegate void ScreenshotHandler(string ScreenshotImagePath, Size ScreenshotSize);
        public event ScreenshotHandler ScreenshotEvent;

        public ucMainResultNavien(string[] _LastRecipeName)
        {
            LastRecipeName = new string[_LastRecipeName.Count()];
            SetLastRecipeName(LastRecipeName);
            InitializeComponent();
        }

        public void ClearResult()
        {
            TotalCount = 0;
            GoodCount = 0;
            NgCount = 0;
            Yield = 0.0;

            SevenSegTotal1.Value = TotalCount.ToString();
            SevenSegYield1.Value = Yield.ToString();
            SevenSegGood1.Value = GoodCount.ToString();
            SevenSegNg1.Value = NgCount.ToString();
        }

        public void SetAutoMode(bool _AutoModeFlag)
        {
            AutoModeFlag = _AutoModeFlag;
        }

        public void SetLastRecipeName(string[] _LastRecipeName)
        {
            for (int iLoopCount = 0; iLoopCount < _LastRecipeName.Count(); iLoopCount++)
            {
                LastRecipeName[iLoopCount] = _LastRecipeName[iLoopCount];
            }
        }

        private void SegmentValueInvoke(DmitryBrant.CustomControls.SevenSegmentArray _Control, string _Value)
        {
            if (_Control.InvokeRequired)
            {
                _Control.Invoke(new MethodInvoker(delegate () { _Control.Value = _Value; _Control.Refresh(); }));
            }
            else
            {
                _Control.Value = _Value; _Control.Refresh();
            }
        }

        public void SetResult(SendResultParameter _ResultParam)
        {
            //lock
            if (_ResultParam.ID == 0) SetLeftResultData(_ResultParam);
            else                      SetRightResultData(_ResultParam);
        }

        public void SetLeftResultData(SendResultParameter _ResultParam)
        {
            bool LastResultFlag = true;

            LastResultFlag = _ResultParam.IsGood;

            switch (LastResultFlag)
            {
                case true: ControlInvoke.GradientLabelText(gradientLabelResult1, "OK", Color.Lime); break;
                case false: ControlInvoke.GradientLabelText(gradientLabelResult1, "NG", Color.Red); break;
            }

            if (AutoModeFlag)
            {
                //TotalCount[1]++;
                //if (LastResultFlag) GoodCount[1]++;
                //else NgCount[1]++;

                //Yield[1] = (double)GoodCount[1] / (double)TotalCount[1] * 100;

                //SegmentValueInvoke(SevenSegTotalCountArr[1], TotalCount[1].ToString());
                //SegmentValueInvoke(SevenSegGoodCountArr[1], GoodCount[1].ToString());
                //SegmentValueInvoke(SevenSegNgCountArr[1], NgCount[1].ToString());
                //SegmentValueInvoke(SevenSegYieldArr[1], Yield[1].ToString("F2"));
            }
        }

        public void SetRightResultData(SendResultParameter _ResultParam)
        {
            bool LastResultFlag = true;

            LastResultFlag = _ResultParam.IsGood;

            switch (LastResultFlag)
            {
                case true: ControlInvoke.GradientLabelText(gradientLabelResult2, "OK", Color.Lime); break;
                case false: ControlInvoke.GradientLabelText(gradientLabelResult2, "NG", Color.Red); break;
            }

            if (AutoModeFlag)
            {
                //TotalCount[1]++;
                //if (LastResultFlag) GoodCount[1]++;
                //else NgCount[1]++;

                //Yield[1] = (double)GoodCount[1] / (double)TotalCount[1] * 100;

                //SegmentValueInvoke(SevenSegTotalCountArr[1], TotalCount[1].ToString());
                //SegmentValueInvoke(SevenSegGoodCountArr[1], GoodCount[1].ToString());
                //SegmentValueInvoke(SevenSegNgCountArr[1], NgCount[1].ToString());
                //SegmentValueInvoke(SevenSegYieldArr[1], Yield[1].ToString("F2"));
            }
        }
    }
}
