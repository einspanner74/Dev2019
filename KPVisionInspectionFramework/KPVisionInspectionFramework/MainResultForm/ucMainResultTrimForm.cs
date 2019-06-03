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
    public partial class ucMainResultTrimForm : UserControl
    {
        #region Count & Yield Variable
        private uint TotalCount
        {
            set { SegmentValueInvoke(SevenSegTrimTotal, value.ToString()); }
            get { return Convert.ToUInt32(SevenSegTrimTotal.Value); }
        }

        private uint GoodCount
        {
            set { SegmentValueInvoke(SevenSegTrimGood, value.ToString()); }
            get { return Convert.ToUInt32(SevenSegTrimGood.Value); }
        }

        private uint NgCount
        {
            set { SegmentValueInvoke(SevenSegTrimNg, value.ToString()); }
            get { return Convert.ToUInt32(SevenSegTrimNg.Value); }
        }

        private double Yield
        {
            set { SegmentValueInvoke(SevenSegTrimYield, value.ToString()); }
            get { return Convert.ToDouble(SevenSegTrimYield.Value); }
        }
        #endregion Count & Yield Variable

        #region Count & Yield Registry Variable
        private RegistryKey RegTotalCount;
        private RegistryKey RegGoodCount;
        private RegistryKey RegNgCount;
        private RegistryKey RegYield;

        private string RegTotalCountPath = String.Format(@"KPVision\ResultCount\TotalCount");
        private string RegGoodCountPath = String.Format(@"KPVision\ResultCount\GoodCount");
        private string RegNgCountPath = String.Format(@"KPVision\ResultCount\NgCount");
        private string RegYieldPath = String.Format(@"KPVision\ResultCount\Yield");
        #endregion Count & Yield Registry Variable

        private string[] HistoryParam;
        private string[] LastRecipeName;
        private string LastResult;

        public delegate void ScreenshotHandler(string ScreenshotImagePath);
        public event ScreenshotHandler ScreenshotEvent;

        #region Initialize & DeInitialize
        public ucMainResultTrimForm(string[] _LastRecipeName)
        {
            LastRecipeName = new string[_LastRecipeName.Count()];
            SetLastRecipeName(_LastRecipeName);

            InitializeComponent();
            InitializeControl();
            this.Location = new Point(1, 1);

            RegTotalCount = Registry.CurrentUser.CreateSubKey(RegTotalCountPath);
            RegGoodCount = Registry.CurrentUser.CreateSubKey(RegGoodCountPath);
            RegNgCount = Registry.CurrentUser.CreateSubKey(RegNgCountPath);
            RegYield = Registry.CurrentUser.CreateSubKey(RegYieldPath);
            LoadResultCount();
        }

        private void InitializeControl()
        {
            for (int iLoopCount = 0; iLoopCount < 20; ++iLoopCount)
            {
                DataGridViewRow _GridRow = new DataGridViewRow();
                DataGridViewCell[] _GridCell = new DataGridViewCell[3];
                _GridCell[0] = gridLeadNum.CellTemplate.Clone() as DataGridViewCell;
                _GridCell[1] = gridLeadLength.CellTemplate.Clone() as DataGridViewCell;
                _GridCell[2] = gridLeadPitch.CellTemplate.Clone() as DataGridViewCell;
            
                _GridCell[0].Value = (iLoopCount + 1);
                _GridCell[0].Style.BackColor = Color.DarkGreen;
                _GridCell[0].Style.ForeColor = Color.White;
            
                _GridRow.Height = 22;
                _GridRow.Cells.AddRange(_GridCell);
                QuickGridViewLeadTrimResult.Rows.Add(_GridRow);
            }
            QuickGridViewLeadTrimResult.ClearSelection();

            for (int iLoopCount = 0; iLoopCount < 20; ++iLoopCount)
            {
                DataGridViewRow _GridRow = new DataGridViewRow();
                DataGridViewCell[] _GridCell = new DataGridViewCell[3];
                _GridCell[0] = gridLeadNum.CellTemplate.Clone() as DataGridViewCell;
                _GridCell[1] = gridLeadPositionX.CellTemplate.Clone() as DataGridViewCell;
                _GridCell[2] = gridLeadPositionY.CellTemplate.Clone() as DataGridViewCell;
            
                _GridCell[0].Value = (iLoopCount + 1);
                _GridCell[0].Style.BackColor = Color.DarkGreen;
                _GridCell[0].Style.ForeColor = Color.White;
            
                _GridRow.Height = 22;
                _GridRow.Cells.AddRange(_GridCell);
                QuickGridViewLeadFormResult.Rows.Add(_GridRow);
            }
            QuickGridViewLeadFormResult.ClearSelection();

            //LDH, 2018.08.14, Hitory Parameter용 배열 초기화
            HistoryParam = new string[4];
            for (int iLoopCount = 0; iLoopCount < HistoryParam.Count(); iLoopCount++)
            {
                HistoryParam[iLoopCount] = "-";
            }
        }

        public void DeInitialize()
        {

        }

        private void LoadResultCount()
        {
            TotalCount = Convert.ToUInt32(RegTotalCount.GetValue("Value"));
            GoodCount = Convert.ToUInt32(RegGoodCount.GetValue("Value"));
            NgCount = Convert.ToUInt32(RegNgCount.GetValue("Value"));
            Yield = Convert.ToDouble(RegYield.GetValue("Value"));

            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, "Load Result Count");
            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, String.Format("TotalCount : {0}, GoodCount : {1}, NgCount : {2}, Yield : {3:F3}", TotalCount, GoodCount, NgCount, Yield));
        }

        private void SaveResultCount()
        {
            RegTotalCount.SetValue("Value", TotalCount, RegistryValueKind.String);
            RegGoodCount.SetValue("Value", GoodCount, RegistryValueKind.String);
            RegNgCount.SetValue("Value", NgCount, RegistryValueKind.String);
            RegYield.SetValue("Value", Yield, RegistryValueKind.String);

            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, "Save Result Count");
            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, String.Format("TotalCount : {0}, GoodCount : {1}, NgCount : {2}, Yield : {3:F3}", TotalCount, GoodCount, NgCount, Yield));
        }
        #endregion Initialize & DeInitialize

        #region Control Default Event
        private void panelMain_Paint(object sender, PaintEventArgs e)
        {
            //ControlPaint.DrawBorder(e.Graphics, this.panelMain.ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }
        #endregion

        #region Label Invoke
        /// <summary>
        /// Label Update
        /// </summary>
        /// <param name="_Control">Label Conrol</param>
        /// <param name="_Msg">Label Text</param>
        /// <param name="_BackColor">Label BackColor</param>
        /// <param name="_FontColor">Label ForeColor</param>
        private void LabelUpdateInvoke(Label _Control, string _Msg, Color _BackColor, Color _FontColor)
        {
            if (_Control.InvokeRequired)
            {
                _Control.Invoke(new MethodInvoker(delegate ()
                {
                    _Control.Text = _Msg;
                    _Control.BackColor = _BackColor;
                    _Control.ForeColor = _FontColor;
                }
                ));
            }

            else
            {
                _Control.Text = _Msg;
                _Control.BackColor = _BackColor;
                _Control.ForeColor = _FontColor;
            }
        }

        /// <summary>
        /// Label Update
        /// </summary>
        /// <param name="_Control">Label Conrol</param>
        /// <param name="_Msg">Label Text</param>
        /// <param name="_BackColor">Label BackColor</param>
        /// <param name="_FontColor">Label ForeColor</param>
        private void LabelUpdateInvoke(Label _Control, string _Msg)
        {
            if (_Control.InvokeRequired)
            {
                _Control.Invoke(new MethodInvoker(delegate ()
                {
                    _Control.Text = _Msg;
                }
                ));
            }

            else
            {
                _Control.Text = _Msg;
            }
        }
        #endregion Label Invoke

        #region Segment Invoke
        private void SegmentValueInvoke(DmitryBrant.CustomControls.SevenSegmentArray _Control, string _Value)
        {
            if (_Control.InvokeRequired)
            {
                _Control.Invoke(new MethodInvoker(delegate () { _Control.Value = _Value; }));
            }
            else
            {
                _Control.Value = _Value;
            }
        }
        #endregion

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

        public void ClearResult()
        {

        }

        public void SetResult(SendResultParameter _ResultParam)
        {
            switch (_ResultParam.ProjectItem)
            {
                case eProjectItem.LEAD_TRIM_INSP:   SetTrimResultData(_ResultParam); break;
                case eProjectItem.LEAD_FORM_ALIGN:  SetFormResultData(_ResultParam); break;
            }
        }

        private void SetTrimResultData(SendResultParameter _ResultParam)
        {
            ClearLeadTrimResultControl();

            var _Result = _ResultParam.SendResult as SendLeadTrimResult;

            #region 결과창 업데이트
            //결과창 업데이트 
            if (_ResultParam.IsGood)
            {
                TotalCount++;
                GoodCount++;
                Yield = (double)GoodCount / (double)TotalCount * 100;
                SegmentValueInvoke(SevenSegTrimTotal, TotalCount.ToString());
                SegmentValueInvoke(SevenSegTrimGood, GoodCount.ToString());
                SegmentValueInvoke(SevenSegTrimYield, Yield.ToString("F2"));

                ControlInvoke.GradientLabelText(gradientLabelTrimResult, "GOOD", Color.Lime);
            }

            else
            {
                TotalCount++;
                NgCount++;
                Yield = (double)GoodCount / (double)TotalCount * 100;
                SegmentValueInvoke(SevenSegTrimTotal, TotalCount.ToString());
                SegmentValueInvoke(SevenSegTrimNg, NgCount.ToString());
                SegmentValueInvoke(SevenSegTrimYield, Yield.ToString("F2"));

                switch (_ResultParam.NgType)
                {
                    case eNgType.EMPTY:         ControlInvoke.GradientLabelText(gradientLabelTrimResult, "EMPTY", Color.Red); break;
                    case eNgType.CHIP_OUT:      ControlInvoke.GradientLabelText(gradientLabelTrimResult, "CHIP OUT", Color.Red); break;
                    case eNgType.LEAD_LENGTH:   ControlInvoke.GradientLabelText(gradientLabelTrimResult, "LENGTH ERR", Color.Red); break;
                    case eNgType.SHD_BURR:      ControlInvoke.GradientLabelText(gradientLabelTrimResult, "SHLD BURR", Color.Red); break;
                    case eNgType.SHD_NICK:      ControlInvoke.GradientLabelText(gradientLabelTrimResult, "SHLD NICK", Color.Red); break;
                    case eNgType.TIP_BURR:      ControlInvoke.GradientLabelText(gradientLabelTrimResult, "TIP BURR", Color.Red); break;
                    case eNgType.GATE_ERR:      ControlInvoke.GradientLabelText(gradientLabelTrimResult, "GATE BURR", Color.Red); break;
                }
            }
            #endregion

            #region Lead 결과 Gridview 업데이트
            if (_Result != null)
            {
                for (int iLoopCount = 0; iLoopCount < _Result.LeadCount; ++iLoopCount)
                {
                    QuickGridViewLeadTrimResult[1, iLoopCount].Value = _Result.LeadLengthReal[iLoopCount].ToString("F3") + " mm";
                    
                    if (_Result.IsLeadLengthGood[iLoopCount] && iLoopCount % 2 == 0)        QuickGridViewLeadTrimResult[1, iLoopCount].Style.BackColor = Color.PowderBlue;
                    else if (_Result.IsLeadLengthGood[iLoopCount] && iLoopCount % 2 == 1)   QuickGridViewLeadTrimResult[1, iLoopCount].Style.BackColor = Color.White;
                    else                                                                    QuickGridViewLeadTrimResult[1, iLoopCount].Style.BackColor = Color.Red;
                    
                    if (iLoopCount < _Result.LeadCount - 1)
                    {
                        QuickGridViewLeadTrimResult[2, iLoopCount].Value = _Result.LeadPitchReal[iLoopCount].ToString("F3") + " mm";

                        if (_Result.IsLeadPitchGood[iLoopCount] && iLoopCount % 2 == 0)         QuickGridViewLeadTrimResult[2, iLoopCount].Style.BackColor = Color.PowderBlue;
                        else if (_Result.IsLeadPitchGood[iLoopCount] && iLoopCount % 2 == 1)    QuickGridViewLeadTrimResult[2, iLoopCount].Style.BackColor = Color.White;
                        else                                                                    QuickGridViewLeadTrimResult[2, iLoopCount].Style.BackColor = Color.Red;
                    }

                    else
                        QuickGridViewLeadTrimResult[2, iLoopCount].Value = 0;
                }
            }

            #endregion

            SaveResultCount();
        }

        private void ClearLeadTrimResultControl()
        {
            for (int iLoopCount = 0; iLoopCount < 20; ++iLoopCount)
            {
                QuickGridViewLeadTrimResult[1, iLoopCount].Value = "0 mm ";
                QuickGridViewLeadTrimResult[2, iLoopCount].Value = "0 mm ";
            }
        }

        private void SetFormResultData(SendResultParameter _ResultParam)
        {
            ClearLeadFormResultControl();
        }

        private void ClearLeadFormResultControl()
        {
            for (int iLoopCount = 0; iLoopCount < 20; ++iLoopCount)
            {
                QuickGridViewLeadFormResult[1, iLoopCount].Value = "0";
                QuickGridViewLeadFormResult[2, iLoopCount].Value = "0";
            }
            
        }

        private void InspectionHistory(int _ID, string _Result)
        {

        }
    }
}
