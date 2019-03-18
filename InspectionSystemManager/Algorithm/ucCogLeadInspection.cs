using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ParameterManager;
using LogMessageManager;

namespace InspectionSystemManager
{
    public partial class ucCogLeadInspection : UserControl
    {
        private CogLeadAlgo CogLeadAlgoRcp = new CogLeadAlgo();
        private Panel[] LeadPanelArray;

        private int LeadCount = 26;

        private double ResolutionX = 0.005;
        private double ResolutionY = 0.005;

        private double LeadPitchAverage = 0;
        private double LeadAngleAverage = 0;

        private double BenchMarkOffsetX = 0;
        private double BenchMarkOffsetY = 0;

        private bool AlgoInitFlag = false;

        public delegate void ApplyLeadInspValueHandler(CogLeadAlgo _CogLeadAlgo, ref CogLeadResult _CogLeadResult, bool _IsDisplay = true);
        public event ApplyLeadInspValueHandler ApplyLeadInspValueEvent;

        #region Initialize & DeInitialize
        public ucCogLeadInspection()
        {
            InitializeComponent();
            InitializeControl();
        }

        public void Initialize()
        {

        }

        private void InitializeControl()
        {
            LeadPanelArray = new Panel[26] { panelLead1,  panelLead2,  panelLead3,  panelLead4,  panelLead5,  panelLead6,  panelLead7,  panelLead8,  panelLead9,  panelLead10, panelLead11, panelLead12, panelLead13,
                                             panelLead14, panelLead15, panelLead16, panelLead17, panelLead18, panelLead19, panelLead20, panelLead21, panelLead22, panelLead23, panelLead24, panelLead25, panelLead26 };

            for (int iLoopCount = 0; iLoopCount < LeadCount; ++iLoopCount)
            {
                LeadPanelArray[iLoopCount].BackColor = Color.White;
                LeadPanelArray[iLoopCount].Tag = iLoopCount;
            }

        }

        public void DeInitialize()
        {

        }
        #endregion Initialize & DeInitialize

        #region Control Event
        private void btnSetting_Click(object sender, EventArgs e)
        {
            ApplySettingValue();
        }

        private void rbForeground_MouseUp(object sender, MouseEventArgs e)
        {
            RadioButton _RadioForeColor = (RadioButton)sender;
            int _ForeColor = Convert.ToInt32(_RadioForeColor.Tag);
            SetForegroundComboBox(_ForeColor);
            graLabelForeground.Text = _ForeColor.ToString();
            ApplySettingValue();
        }

        private void hScrollBarThreshold_ValueChanged(object sender, EventArgs e)
        {
            graLabelThresholdValue.Text = hScrollBarThreshold.Value.ToString();
            ApplySettingValue();
        }

        private void panelLead_MouseUp(object sender, MouseEventArgs e)
        {
            DialogResult dlgResult = MessageBox.Show(new Form { TopMost = true }, "Lead 검사 여부를 변경하시겠습니까 ? ", "Lead inspection status change", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
            if (DialogResult.Yes != dlgResult) return;

            short _Tag = Convert.ToInt16(((Panel)sender).Tag);
            ChangeLeadPanelStatus(_Tag);
        }

        private void btnGetLeadInfo_Click(object sender, EventArgs e)
        {
            ApplySettingValue(false);
            textBoxLeadCount.Text = LeadCount.ToString();
            textBoxLeadPitch.Text = (LeadPitchAverage * ResolutionX).ToString("F3");

            if (LeadAngleAverage > 0)   textBoxLeadBentAngle.Text = (90 - (LeadAngleAverage * 180 / Math.PI)).ToString("F3");
            else                        textBoxLeadBentAngle.Text = (-(90 + (LeadAngleAverage * 180 / Math.PI))).ToString("F3");

        }

        private void textBoxBlobAreaMin_TextChanged(object sender, EventArgs e)
        {
            string _TextBoxValue = TextboxValueCheckNumbers(textBoxBlobAreaMin.Text);
            if (!textBoxBlobAreaMin.Text.Equals(_TextBoxValue))
            {
                textBoxBlobAreaMin.Text = _TextBoxValue;
                textBoxBlobAreaMin.Select(textBoxBlobAreaMin.Text.Length, 0);
            }
        }

        private void textBoxBlobAreaMax_TextChanged(object sender, EventArgs e)
        {
            string _TextBoxValue = TextboxValueCheckNumbers(textBoxBlobAreaMax.Text);
            if (!textBoxBlobAreaMax.Text.Equals(_TextBoxValue))
            {
                textBoxBlobAreaMax.Text = _TextBoxValue;
                textBoxBlobAreaMax.Select(textBoxBlobAreaMax.Text.Length, 0);
            }
        }

        private void textBoxWidthSizeMin_TextChanged(object sender, EventArgs e)
        {
            string _TextBoxValue = TextboxValueCheckNumbers(textBoxWidthSizeMin.Text, true);
            if (!textBoxWidthSizeMin.Text.Equals(_TextBoxValue))
            {
                textBoxWidthSizeMin.Text = _TextBoxValue;
                textBoxWidthSizeMin.Select(textBoxWidthSizeMin.Text.Length, 0);
            }
        }

        private void textBoxWidthSizeMax_TextChanged(object sender, EventArgs e)
        {
            string _TextBoxValue = TextboxValueCheckNumbers(textBoxWidthSizeMax.Text, true);
            if (!textBoxWidthSizeMax.Text.Equals(_TextBoxValue))
            {
                textBoxWidthSizeMax.Text = _TextBoxValue;
                textBoxWidthSizeMax.Select(textBoxWidthSizeMax.Text.Length, 0);
            }
        }

        private void textBoxHeightSizeMin_TextChanged(object sender, EventArgs e)
        {
            string _TextBoxValue = TextboxValueCheckNumbers(textBoxHeightSizeMin.Text, true);
            if (!textBoxHeightSizeMin.Text.Equals(_TextBoxValue))
            {
                textBoxHeightSizeMin.Text = _TextBoxValue;
                textBoxHeightSizeMin.Select(textBoxHeightSizeMin.Text.Length, 0);
            }
        }

        private void textBoxHeightSizeMax_TextChanged(object sender, EventArgs e)
        {
            string _TextBoxValue = TextboxValueCheckNumbers(textBoxHeightSizeMax.Text, true);
            if (!textBoxHeightSizeMax.Text.Equals(_TextBoxValue))
            {
                textBoxHeightSizeMax.Text = _TextBoxValue;
                textBoxHeightSizeMax.Select(textBoxHeightSizeMax.Text.Length, 0);
            }
        }

        private void textBoxLeadCount_TextChanged(object sender, EventArgs e)
        {
            string _TextBoxValue = TextboxValueCheckNumbers(textBoxLeadCount.Text);
            if (!textBoxLeadCount.Text.Equals(_TextBoxValue))
            {
                textBoxLeadCount.Text = _TextBoxValue;
                textBoxLeadCount.Select(textBoxLeadCount.Text.Length, 0);
            }
        }

        private void textBoxLeadPitch_TextChanged(object sender, EventArgs e)
        {
            string _TextBoxValue = TextboxValueCheckNumbers(textBoxLeadPitch.Text, true);
            if (!textBoxLeadPitch.Text.Equals(_TextBoxValue))
            {
                textBoxLeadPitch.Text = _TextBoxValue;
                textBoxLeadPitch.Select(textBoxLeadPitch.Text.Length, 0);
            }
        }

        private void textBoxLeadPitchMin_TextChanged(object sender, EventArgs e)
        {
            string _TextBoxValue = TextboxValueCheckNumbers(textBoxLeadPitchMin.Text, true);
            if (!textBoxLeadPitchMin.Text.Equals(_TextBoxValue))
            {
                textBoxLeadPitchMin.Text = _TextBoxValue;
                textBoxLeadPitchMin.Select(textBoxLeadPitchMin.Text.Length, 0);
            }
        }

        private void textBoxLeadPitchMax_TextChanged(object sender, EventArgs e)
        {
            string _TextBoxValue = TextboxValueCheckNumbers(textBoxLeadPitchMax.Text, true);
            if (!textBoxLeadPitchMax.Text.Equals(_TextBoxValue))
            {
                textBoxLeadPitchMax.Text = _TextBoxValue;
                textBoxLeadPitchMax.Select(textBoxLeadPitchMax.Text.Length, 0);
            }
        }

        private void textBoxLeadBentAngle_TextChanged(object sender, EventArgs e)
        {
            string _TextBoxValue = TextboxValueCheckNumbers(textBoxLeadBentAngle.Text, true);
            if (!textBoxLeadBentAngle.Text.Equals(_TextBoxValue))
            {
                textBoxLeadBentAngle.Text = _TextBoxValue;
                textBoxLeadBentAngle.Select(textBoxLeadBentAngle.Text.Length, 0);
            }
        }

        private void textBoxLeadBentAngleMin_TextChanged(object sender, EventArgs e)
        {
            string _TextBoxValue = TextboxValueCheckNumbers(textBoxLeadBentAngleMin.Text, true);
            if (!textBoxLeadBentAngleMin.Text.Equals(_TextBoxValue))
            {
                textBoxLeadBentAngleMin.Text = _TextBoxValue;
                textBoxLeadBentAngleMin.Select(textBoxLeadBentAngleMin.Text.Length, 0);
            }
        }

        private void textBoxLeadBentAngleMax_TextChanged(object sender, EventArgs e)
        {
            string _TextBoxValue = TextboxValueCheckNumbers(textBoxLeadBentAngleMax.Text, true);
            if (!textBoxLeadBentAngleMax.Text.Equals(_TextBoxValue))
            {
                textBoxLeadBentAngleMax.Text = _TextBoxValue;
                textBoxLeadBentAngleMax.Select(textBoxLeadBentAngleMax.Text.Length, 0);
            }
        }
        private string TextboxValueCheckNumbers(string _TextBoxValue, bool _UseDecimalPoint = false)
        {
            string _ResultValue = String.Empty;
            foreach (char _c in _TextBoxValue)
            {

                if (true == _UseDecimalPoint && (char.IsNumber(_c) || _c.Equals('.'))) _ResultValue = String.Format("{0}{1}", _ResultValue, _c);
                else if (false == _UseDecimalPoint && char.IsNumber(_c)) _ResultValue = String.Format("{0}{1}", _ResultValue, _c);
            }

            return _ResultValue;
        }
        #endregion Control Event

        public void SetAlgoRecipe(Object _Algorithm, double _BenchMarkOffsetX, double _BenchMarkOffsetY, double _ResolutionX, double _ResolutionY)
        {
            if (_Algorithm != null)
            {
                AlgoInitFlag = false;
                
                CogLeadAlgoRcp = _Algorithm as CogLeadAlgo;

                ResolutionX = _ResolutionX;
                ResolutionY = _ResolutionY;
                BenchMarkOffsetX = _BenchMarkOffsetX;
                BenchMarkOffsetY = _BenchMarkOffsetY;

                LeadCount = CogLeadAlgoRcp.LeadCount;

                textBoxLeadCount.Text = LeadCount.ToString();
                graLabelForeground.Text = CogLeadAlgoRcp.ForeGround.ToString();
                graLabelThresholdValue.Text = CogLeadAlgoRcp.ThresholdMin.ToString();
                hScrollBarThreshold.Value = CogLeadAlgoRcp.ThresholdMin;
                textBoxBlobAreaMin.Text = CogLeadAlgoRcp.BlobAreaMin.ToString();
                textBoxBlobAreaMax.Text = CogLeadAlgoRcp.BlobAreaMax.ToString();
                textBoxWidthSizeMin.Text = CogLeadAlgoRcp.WidthMin.ToString();
                textBoxWidthSizeMax.Text = CogLeadAlgoRcp.WidthMax.ToString();
                textBoxHeightSizeMin.Text = CogLeadAlgoRcp.HeightMin.ToString();
                textBoxHeightSizeMax.Text = CogLeadAlgoRcp.HeightMax.ToString();

                ckLeadBent.Checked = CogLeadAlgoRcp.IsLeadBentInspection;
                textBoxLeadBentAngle.Text = CogLeadAlgoRcp.LeadBent.ToString();
                textBoxLeadBentAngleMin.Text = CogLeadAlgoRcp.LeadBentMin.ToString();
                textBoxLeadBentAngleMax.Text = CogLeadAlgoRcp.LeadBentMax.ToString();

                ckLeadPitch.Checked = CogLeadAlgoRcp.IsLeadPitchInspection;
                textBoxLeadPitch.Text = CogLeadAlgoRcp.LeadPitch.ToString();
                textBoxLeadPitchMin.Text = CogLeadAlgoRcp.LeadPitchMin.ToString();
                textBoxLeadPitchMax.Text = CogLeadAlgoRcp.LeadPitchMax.ToString();

                SetForegroundComboBox(CogLeadAlgoRcp.ForeGround);
                SetLeadPanelAllStatus();

                AlgoInitFlag = true;
            }

            else
            {
                //LOG
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "Teaching LeadInspection SetAlgoRecipe", CLogManager.LOG_LEVEL.MID);
            }
        }

        public void SaveAlgoRecipe()
        {
            CogLeadAlgoRcp.ForeGround = Convert.ToInt32(graLabelForeground.Text);
            CogLeadAlgoRcp.ThresholdMin = Convert.ToInt32(graLabelThresholdValue.Text);
            CogLeadAlgoRcp.BlobAreaMin = Convert.ToDouble(textBoxBlobAreaMin.Text);
            CogLeadAlgoRcp.BlobAreaMax = Convert.ToDouble(textBoxBlobAreaMax.Text);
            CogLeadAlgoRcp.WidthMin = Convert.ToDouble(textBoxWidthSizeMin.Text);
            CogLeadAlgoRcp.WidthMax = Convert.ToDouble(textBoxWidthSizeMax.Text);
            CogLeadAlgoRcp.HeightMin = Convert.ToDouble(textBoxHeightSizeMin.Text);
            CogLeadAlgoRcp.HeightMax = Convert.ToDouble(textBoxHeightSizeMax.Text);

            CogLeadAlgoRcp.IsLeadBentInspection = ckLeadBent.Checked;
            CogLeadAlgoRcp.LeadBent = Convert.ToDouble(textBoxLeadBentAngle.Text);
            CogLeadAlgoRcp.LeadBentMin = Convert.ToDouble(textBoxLeadBentAngleMin.Text);
            CogLeadAlgoRcp.LeadBentMax= Convert.ToDouble(textBoxLeadBentAngleMax.Text);

            CogLeadAlgoRcp.LeadCount = Convert.ToInt32(textBoxLeadCount.Text);

            CogLeadAlgoRcp.IsLeadPitchInspection = ckLeadPitch.Checked;
            CogLeadAlgoRcp.LeadPitch = Convert.ToDouble(textBoxLeadPitch.Text);
            CogLeadAlgoRcp.LeadPitchMin = Convert.ToDouble(textBoxLeadPitchMin.Text);
            CogLeadAlgoRcp.LeadPitchMax = Convert.ToDouble(textBoxLeadPitchMax.Text);

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "Teaching LeadInspection SaveAlgoRecipe", CLogManager.LOG_LEVEL.MID);
        }

        private void SetForegroundComboBox(int _RangeType)
        {
            rbForegroundWhite.Checked = false;
            rbForegroundBlack.Checked = false;

            switch ((eForeColor)_RangeType)
            {
                case eForeColor.BLACK: rbForegroundBlack.Checked = true; break;
                case eForeColor.WHITE: rbForegroundWhite.Checked = true; break;
            }
        }

        private void SetLeadPanelAllStatus()
        {
            for (int iLoopCount = 0; iLoopCount < CogLeadAlgoRcp.LeadCount; ++iLoopCount)   SetLeadPanelStatus(iLoopCount);
        }

        private void SetLeadPanelStatus(int _Index)
        {
            if (CogLeadAlgoRcp.LeadUsable.Substring(_Index, 1) == "0")
                LeadPanelArray[_Index].BackColor = Color.FromArgb(83, 83, 83);
            else
                LeadPanelArray[_Index].BackColor = Color.White;
        }

        private void ChangeLeadPanelStatus(int _Index)
        {
            char[] _CharArray = CogLeadAlgoRcp.LeadUsable.ToCharArray();

            if (_CharArray[_Index] == '0')  _CharArray[_Index] = '1';
            else                            _CharArray[_Index] = '0';

            CogLeadAlgoRcp.LeadUsable = new string(_CharArray);
            SetLeadPanelStatus(_Index);
        }

        private void ApplySettingValue(bool _IsDisplay = true)
        {
            if (!AlgoInitFlag) return;

            CogLeadResult _CogLeadResult = new CogLeadResult();
            CogLeadAlgo _CogLeadAlgoRcp = new CogLeadAlgo();
            _CogLeadAlgoRcp.ThresholdMin = Convert.ToInt32(graLabelThresholdValue.Text);
            _CogLeadAlgoRcp.BlobAreaMin = Convert.ToDouble(textBoxBlobAreaMin.Text);
            _CogLeadAlgoRcp.BlobAreaMax = Convert.ToDouble(textBoxBlobAreaMax.Text);
            _CogLeadAlgoRcp.WidthMin = Convert.ToDouble(textBoxWidthSizeMin.Text);
            _CogLeadAlgoRcp.WidthMax = Convert.ToDouble(textBoxWidthSizeMax.Text);
            _CogLeadAlgoRcp.HeightMin = Convert.ToDouble(textBoxHeightSizeMin.Text);
            _CogLeadAlgoRcp.HeightMax = Convert.ToDouble(textBoxHeightSizeMax.Text);
            _CogLeadAlgoRcp.ForeGround = Convert.ToInt32(graLabelForeground.Text);
            
            var _ApplyLeadInspValueEvent = ApplyLeadInspValueEvent;
            if (_ApplyLeadInspValueEvent != null)
                _ApplyLeadInspValueEvent(_CogLeadAlgoRcp, ref _CogLeadResult, _IsDisplay);

            LeadPitchAverage = _CogLeadResult.LeadPitchAvg;
            LeadAngleAverage = _CogLeadResult.LeadAngleAvg;
            LeadCount = _CogLeadResult.LeadCount;
        }
    }
}
