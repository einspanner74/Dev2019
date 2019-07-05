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
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;

using CustomControl;
using ParameterManager;
using LogMessageManager;
using HistoryManager;

namespace KPVisionInspectionFramework
{
    public partial class ucMainResultTrimForm : UserControl
    {
        #region Count & Yield Variable
        private uint TrimTotalCount
        {
            set { SegmentValueInvoke(SevenSegTrimTotal, value.ToString()); }
            get { return Convert.ToUInt32(SevenSegTrimTotal.Value); }
        }

        private uint TrimGoodCount
        {
            set { SegmentValueInvoke(SevenSegTrimGood, value.ToString()); }
            get { return Convert.ToUInt32(SevenSegTrimGood.Value); }
        }

        private uint TrimNgCount
        {
            set { SegmentValueInvoke(SevenSegTrimNg, value.ToString()); }
            get { return Convert.ToUInt32(SevenSegTrimNg.Value); }
        }

        private double TrimYield
        {
            set { SegmentValueInvoke(SevenSegTrimYield, value.ToString()); }
            get { return Convert.ToDouble(SevenSegTrimYield.Value); }
        }

        private uint FormTotalCount
        {
            set { SegmentValueInvoke(SevenSegFormTotal, value.ToString()); }
            get { return Convert.ToUInt32(SevenSegFormTotal.Value); }
        }

        private uint FormGoodCount
        {
            set { SegmentValueInvoke(SevenSegFormGood, value.ToString()); }
            get { return Convert.ToUInt32(SevenSegFormGood.Value); }
        }

        private uint FormNgCount
        {
            set { SegmentValueInvoke(SevenSegFormNg, value.ToString()); }
            get { return Convert.ToUInt32(SevenSegFormNg.Value); }
        }

        private double FormYield
        {
            set { SegmentValueInvoke(SevenSegFormYield, value.ToString()); }
            get { return Convert.ToDouble(SevenSegFormYield.Value); }
        }
        #endregion Count & Yield Variable

        #region Count & Yield Registry Variable
        private RegistryKey RegTrimTotalCount;
        private RegistryKey RegTrimGoodCount;
        private RegistryKey RegTrimNgCount;
        private RegistryKey RegTrimYield;

        private string RegTrimTotalCountPath = String.Format(@"KPVision\ResultCount\TrimTotalCount");
        private string RegTrimGoodCountPath = String.Format(@"KPVision\ResultCount\TrimGoodCount");
        private string RegTrimNgCountPath = String.Format(@"KPVision\ResultCount\TrimNgCount");
        private string RegTrimYieldPath = String.Format(@"KPVision\ResultCount\TrimYield");

        private RegistryKey RegFormTotalCount;
        private RegistryKey RegFormGoodCount;
        private RegistryKey RegFormNgCount;
        private RegistryKey RegFormYield;

        private string RegFormTotalCountPath = String.Format(@"KPVision\ResultCount\FormTotalCount");
        private string RegFormGoodCountPath = String.Format(@"KPVision\ResultCount\FormGoodCount");
        private string RegFormNgCountPath = String.Format(@"KPVision\ResultCount\FormNgCount");
        private string RegFormYieldPath = String.Format(@"KPVision\ResultCount\FormYield");
        #endregion Count & Yield Registry Variable

        private string[] HistoryParam;
        private string[] LastRecipeName;
        private string[] ImageFolderPath;
        private string[] LastResult;
        private string ModelName;
        private string SerialNum;
        private string InspectionTime;

        private QuickDataGridView[] QuickGridViewResult;

        public delegate void ScreenshotHandler(string ScreenshotImagePath);
        public event ScreenshotHandler ScreenshotEvent;

        #region Initialize & DeInitialize
        public ucMainResultTrimForm(string[] _LastRecipeName)
        {
            LastRecipeName = new string[_LastRecipeName.Count()];
            ImageFolderPath = new string[1];
            SetLastRecipeName(_LastRecipeName);

            InitializeComponent();
            InitializeControl();
            this.Location = new Point(1, 1);

            RegTrimTotalCount = Registry.CurrentUser.CreateSubKey(RegTrimTotalCountPath);
            RegTrimGoodCount = Registry.CurrentUser.CreateSubKey(RegTrimGoodCountPath);
            RegTrimNgCount = Registry.CurrentUser.CreateSubKey(RegTrimNgCountPath);
            RegTrimYield = Registry.CurrentUser.CreateSubKey(RegTrimYieldPath);

            RegFormTotalCount = Registry.CurrentUser.CreateSubKey(RegFormTotalCountPath);
            RegFormGoodCount = Registry.CurrentUser.CreateSubKey(RegFormGoodCountPath);
            RegFormNgCount = Registry.CurrentUser.CreateSubKey(RegFormNgCountPath);
            RegFormYield = Registry.CurrentUser.CreateSubKey(RegFormYieldPath);

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

            QuickGridViewResult = new QuickDataGridView[2] { QuickGridViewLeadTrimResult, QuickGridViewLeadFormResult };

            //LDH, 2018.08.14, Hitory Parameter용 배열 초기화
            HistoryParam = new string[6];
            for (int iLoopCount = 0; iLoopCount < HistoryParam.Count(); iLoopCount++)
            {
                HistoryParam[iLoopCount] = "-";
            }

            LastResult = new string[2];
            for (int iLoopCount = 0; iLoopCount < LastResult.Count(); iLoopCount++)
            {
                LastResult[iLoopCount] = "OK";
            }

            ModelName = "ModelName";
            SerialNum = "0123456";
        }

        public void DeInitialize()
        {

        }

        private void LoadResultCount()
        {
            TrimTotalCount = Convert.ToUInt32(RegTrimTotalCount.GetValue("Value"));
            TrimGoodCount = Convert.ToUInt32(RegTrimGoodCount.GetValue("Value"));
            TrimNgCount = Convert.ToUInt32(RegTrimNgCount.GetValue("Value"));
            TrimYield = Convert.ToDouble(RegTrimYield.GetValue("Value"));

            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, "Load Trim Result Count");
            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, String.Format("TrimTotalCount : {0}, TrimGoodCount : {1}, TrimNgCount : {2}, TrimYield : {3:F3}", TrimTotalCount, TrimGoodCount, TrimNgCount, TrimYield));


            FormTotalCount = Convert.ToUInt32(RegFormTotalCount.GetValue("Value"));
            FormGoodCount = Convert.ToUInt32(RegFormGoodCount.GetValue("Value"));
            FormNgCount = Convert.ToUInt32(RegFormNgCount.GetValue("Value"));
            FormYield = Convert.ToDouble(RegFormYield.GetValue("Value"));

            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, "Load Form Result Count");
            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, String.Format("FormTotalCount : {0}, FormGoodCount : {1}, FormNgCount : {2}, FormYield : {3:F3}", FormTotalCount, FormGoodCount, FormNgCount, FormYield));
        }

        private void SaveResultCount()
        {
            RegTrimTotalCount.SetValue("Value", TrimTotalCount, RegistryValueKind.String);
            RegTrimGoodCount.SetValue("Value", TrimGoodCount, RegistryValueKind.String);
            RegTrimNgCount.SetValue("Value", TrimNgCount, RegistryValueKind.String);
            RegTrimYield.SetValue("Value", TrimYield, RegistryValueKind.String);

            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, "Save Trim Result Count");
            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, String.Format("TrimTotalCount : {0}, TrimGoodCount : {1}, TrimNgCount : {2}, TrimYield : {3:F3}", TrimTotalCount, TrimGoodCount, TrimNgCount, TrimYield));


            RegFormTotalCount.SetValue("Value", FormTotalCount, RegistryValueKind.String);
            RegFormGoodCount.SetValue("Value", FormGoodCount, RegistryValueKind.String);
            RegFormNgCount.SetValue("Value", FormNgCount, RegistryValueKind.String);
            RegFormYield.SetValue("Value", FormYield, RegistryValueKind.String);

            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, "Save Form Result Count");
            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, String.Format("FormTotalCount : {0}, FormGoodCount : {1}, FormNgCount : {2}, FormYield : {3:F3}", FormTotalCount, FormGoodCount, FormNgCount, FormYield));
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

        public void SetLOTNum(string[] _LOTNum)
        {
            SerialNum = _LOTNum[1];
            ModelName = _LOTNum[2];
        }

        public void ClearResult()
        {
            LastResult[0] = "OK";
            LastResult[1] = "OK";
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
                TrimTotalCount++;
                TrimGoodCount++;
                if (TrimTotalCount == 0 || TrimGoodCount == 0) { TrimTotalCount = 0; TrimGoodCount = 0; TrimNgCount = 0; }

                TrimYield = (double)TrimGoodCount / (double)TrimTotalCount * 100;
                SegmentValueInvoke(SevenSegTrimTotal, TrimTotalCount.ToString());
                SegmentValueInvoke(SevenSegTrimGood, TrimGoodCount.ToString());
                SegmentValueInvoke(SevenSegTrimYield, TrimYield.ToString("F2"));

                ControlInvoke.GradientLabelText(gradientLabelTrimResult, "GOOD", Color.Lime);

                LastResult[0] = "OK";
            }

            else
            {
                TrimTotalCount++;
                TrimNgCount++;
                if (TrimTotalCount == 0 || TrimNgCount == 0) { TrimTotalCount = 0; TrimGoodCount = 0; TrimNgCount = 0; }


                TrimYield = (double)TrimGoodCount / (double)TrimTotalCount * 100;
                SegmentValueInvoke(SevenSegTrimTotal, TrimTotalCount.ToString());
                SegmentValueInvoke(SevenSegTrimNg, TrimNgCount.ToString());
                SegmentValueInvoke(SevenSegTrimYield, TrimYield.ToString("F2"));

                //switch (_ResultParam.NgType)
                //{
                //    case eNgType.EMPTY:         ControlInvoke.GradientLabelText(gradientLabelTrimResult, "EMPTY", Color.Red); break;
                //    case eNgType.CHIP_OUT:      ControlInvoke.GradientLabelText(gradientLabelTrimResult, "CHIP OUT", Color.Red); break;
                //    case eNgType.LEAD_CNT:      ControlInvoke.GradientLabelText(gradientLabelTrimResult, "LEAD_CNT", Color.Red); break;
                //    case eNgType.LEAD_LENGTH:   ControlInvoke.GradientLabelText(gradientLabelTrimResult, "LENGTH ERR", Color.Red); break;
                //    case eNgType.LEAD_BENT:     ControlInvoke.GradientLabelText(gradientLabelTrimResult, "LEAD_BENT", Color.Red); break;
                //    case eNgType.SHD_BURR:      ControlInvoke.GradientLabelText(gradientLabelTrimResult, "SHLD BURR", Color.Red); break;
                //    case eNgType.SHD_NICK:      ControlInvoke.GradientLabelText(gradientLabelTrimResult, "SHLD NICK", Color.Red); break;
                //    case eNgType.TIP_BURR:      ControlInvoke.GradientLabelText(gradientLabelTrimResult, "TIP BURR", Color.Red); break;
                //    case eNgType.GATE_ERR:      ControlInvoke.GradientLabelText(gradientLabelTrimResult, "GATE BURR", Color.Red); break;
                //}

                ControlInvoke.GradientLabelText(gradientLabelTrimResult, "NG", Color.Red);

                LastResult[0] = "NG";
            }
            #endregion

            #region Lead 결과 Gridview 업데이트
            if (_Result != null)
            {
                for (int iLoopCount = 0; iLoopCount < _Result.LeadCount; ++iLoopCount)
                {
                    //Lead Length 결과 업데이트
                    QuickGridViewLeadTrimResult[1, iLoopCount].Value = _Result.LeadLengthReal[iLoopCount].ToString("F3");// + " mm";
                    
                    if (_Result.IsLeadLengthGood[iLoopCount] && iLoopCount % 2 == 0)        QuickGridViewLeadTrimResult[1, iLoopCount].Style.BackColor = Color.PowderBlue;
                    else if (_Result.IsLeadLengthGood[iLoopCount] && iLoopCount % 2 == 1)   QuickGridViewLeadTrimResult[1, iLoopCount].Style.BackColor = Color.White;
                    else                                                                    QuickGridViewLeadTrimResult[1, iLoopCount].Style.BackColor = Color.Red;
                    
                    //Lead Pitch 결과 업데이트
                    if (iLoopCount < _Result.LeadCount - 1)
                    {
                        QuickGridViewLeadTrimResult[2, iLoopCount].Value = _Result.LeadPitchReal[iLoopCount].ToString("F3");// + " mm";

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

            SaveResult(0, _Result.ImageAutoSave, _Result.SaveImage, ref _Result.ResultImagePath);
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

            var _Result = _ResultParam.SendResult as SendLeadFormResult;

            #region 결과창 업데이트
            if (_ResultParam.IsGood)
            {
                FormTotalCount++;
                FormGoodCount++;
                if (FormTotalCount == 0 || FormGoodCount == 0) { FormTotalCount = 0; FormGoodCount = 0; FormNgCount = 0; }

                FormYield = (double)FormGoodCount / (double)FormTotalCount * 100;
                SegmentValueInvoke(SevenSegFormTotal, FormTotalCount.ToString());
                SegmentValueInvoke(SevenSegFormGood, FormGoodCount.ToString());
                SegmentValueInvoke(SevenSegFormYield, FormYield.ToString("F2"));

                ControlInvoke.GradientLabelText(gradientLabelFormResult, "GOOD", Color.Lime);

                LastResult[1] = "OK";
            }

            else
            {
                FormTotalCount++;
                FormNgCount++;
                if (FormTotalCount == 0 || FormNgCount == 0) { FormTotalCount = 0; FormGoodCount = 0; FormNgCount = 0; }

                FormYield = (double)FormGoodCount / (double)FormTotalCount * 100;
                SegmentValueInvoke(SevenSegFormTotal, FormTotalCount.ToString());
                SegmentValueInvoke(SevenSegFormNg, FormNgCount.ToString());
                SegmentValueInvoke(SevenSegFormYield, FormYield.ToString("F2"));


                //switch (_ResultParam.NgType)
                //{
                //    case eNgType.EMPTY:         ControlInvoke.GradientLabelText(gradientLabelFormResult, "EMPTY", Color.Red); break;
                //    case eNgType.LEAD_CNT:      ControlInvoke.GradientLabelText(gradientLabelFormResult, "LEAD_CNT", Color.Red); break;
                //    case eNgType.LEAD_BENT_H:   ControlInvoke.GradientLabelText(gradientLabelFormResult, "LEAD_BENT_H", Color.Red); break;
                //    case eNgType.LEAD_BENT_V:   ControlInvoke.GradientLabelText(gradientLabelFormResult, "LEAD_BENT_V", Color.Red); break;
                //}

                ControlInvoke.GradientLabelText(gradientLabelFormResult, "NG", Color.Red);

                LastResult[1] = "NG";
            }
            #endregion

            if (_Result != null)
            {
                for (int iLoopCount = 0; iLoopCount < _Result.LeadOffset.Length; ++iLoopCount)
                {
                    QuickGridViewLeadFormResult[1, iLoopCount].Value = _Result.LeadOffset[iLoopCount].X.ToString("F3");// + " mm";
                    QuickGridViewLeadFormResult[2, iLoopCount].Value = _Result.LeadOffset[iLoopCount].Y.ToString("F3");// + " mm";

                    if (_Result.IsLeadOffsetGood[iLoopCount] && iLoopCount % 2 == 0)
                    {
                        QuickGridViewLeadFormResult[1, iLoopCount].Style.BackColor = Color.PowderBlue;
                        QuickGridViewLeadFormResult[2, iLoopCount].Style.BackColor = Color.PowderBlue;
                    }

                    else if (_Result.IsLeadOffsetGood[iLoopCount] && iLoopCount % 2 == 1)
                    {
                        QuickGridViewLeadFormResult[1, iLoopCount].Style.BackColor = Color.White;
                        QuickGridViewLeadFormResult[2, iLoopCount].Style.BackColor = Color.White;
                    }

                    else
                    {
                        QuickGridViewLeadFormResult[1, iLoopCount].Style.BackColor = Color.Red;
                        QuickGridViewLeadFormResult[2, iLoopCount].Style.BackColor = Color.Red;
                    }
                }
            }

            SaveResultCount();
                        
            SaveResult(1, _Result.ImageAutoSave, _Result.SaveImage, ref _Result.ResultImagePath);
        }

        private void ClearLeadFormResultControl()
        {
            for (int iLoopCount = 0; iLoopCount < 20; ++iLoopCount)
            {
                QuickGridViewLeadFormResult[1, iLoopCount].Value = "0";
                QuickGridViewLeadFormResult[2, iLoopCount].Value = "0";
            }            
        }

        public void SetDataFolderPath(string[] _DataFolderPath)
        {
            for (int iLoopCount = 0; iLoopCount < 1; iLoopCount++) ImageFolderPath[iLoopCount] = _DataFolderPath[iLoopCount];
        }

        public void SaveResult(int _InspectionNum, eSaveMode ImageAutoSave, CogImage8Grey _SaveImage, ref string _ResultImagePath)
        {
            string SaveFileName = "";

            string CameraType = "";
            if (_InspectionNum == 0) CameraType = "TOP";
            else                     CameraType = "SIDE";

            DateTime dateTime = DateTime.Now;
            string SaveImageFilePath = ImageFolderPath[0];

            if (SaveImageFilePath != "")
            {
                #region Set File Path
                if (false == Directory.Exists(SaveImageFilePath)) Directory.CreateDirectory(SaveImageFilePath);
                SaveImageFilePath = String.Format("{0}\\{1:D4}\\{2:D2}\\{3:D2}", SaveImageFilePath, dateTime.Year, dateTime.Month, dateTime.Day);
                if (false == Directory.Exists(SaveImageFilePath)) Directory.CreateDirectory(SaveImageFilePath);
                SaveImageFilePath = String.Format("{0}\\{1}\\{2}", SaveImageFilePath, ModelName, LastResult[_InspectionNum]);
                if (false == Directory.Exists(SaveImageFilePath)) Directory.CreateDirectory(SaveImageFilePath);

                //연월일시분초밀리
                SaveFileName = String.Format("{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}{6:D3}", dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
                InspectionTime = SaveFileName;

                //_1차종_2시리얼번호_3카메라(TOP/SIDE)_4검사결과(OK/NG)
                SaveFileName = String.Format("{0}_{1}_{2}_{3}_{4}", SaveFileName, ModelName, SerialNum, CameraType, LastResult[_InspectionNum]);

                //Display Image 저장용 Path
                _ResultImagePath = SaveImageFilePath + "\\" + SaveFileName + "_Mark.bmp";
                #endregion Set File Path

                #region Image Save
                //LDH, 2019.06.18, Image Save
                try
                {
                    ICogImage _CogSaveImage = _SaveImage;
                    CogImageFile _CogImageFile = new CogImageFile();

                    if (_CogSaveImage == null)
                    {
                        CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.ERR, "SaveResult _CogSaveImage null!!", CLogManager.LOG_LEVEL.LOW);
                    }
                    else
                    {
                        _CogImageFile.Open(SaveImageFilePath + "\\" + SaveFileName + ".bmp", CogImageFileModeConstants.Write);

                        if (eSaveMode.ONLY_NG == ImageAutoSave)
                        {
                            if (LastResult[_InspectionNum] == "NG")
                            {                               
                                _CogImageFile.Append(_CogSaveImage);
                            }
                        }
                        else { _CogImageFile.Append(_CogSaveImage); }

                        _CogImageFile.Close();
                    }
                }
                catch
                {
                    CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.ERR, "SaveResult Image Save Exception!!", CLogManager.LOG_LEVEL.LOW);
                }
                #endregion

                #region Data Save
                //LDH, 2019.06.18, Data Save
                CSVManagerSaveAll SaveCSVControl = new CSVManagerSaveAll();

                string SaveCSVFilePath = String.Format("{0}\\Result", SaveImageFilePath);
                if (false == Directory.Exists(SaveCSVFilePath)) Directory.CreateDirectory(SaveCSVFilePath);

                SaveCSVFilePath = String.Format("{0}\\{1}.csv", SaveCSVFilePath, SaveFileName);
                SaveCSVControl.SaveGridViewAll(QuickGridViewResult[_InspectionNum], SaveCSVFilePath);

                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, string.Format("Save {0} CSV File", _InspectionNum), CLogManager.LOG_LEVEL.LOW);
                #endregion Data Save

                if(LastResult[_InspectionNum] == "NG") InspectionHistory(CameraType, LastResult[_InspectionNum], _ResultImagePath);
            }
            else
            {
                MessageBox.Show("Data를 저장할 수 없습니다. \n저장경로를 설정하세요.");
            }
        }

        //LDH, 2019.06.26, History 추가용 함수
        private void InspectionHistory(string _CamType, string _LastResult, string _ImageFilePath)
        {
            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, String.Format("InspectionHistory Start"), CLogManager.LOG_LEVEL.LOW);

            //LDH, 2019.06.26, DB에 해당하는 history 내역을 string 배열로 전달
            HistoryParam[0] = InspectionTime;
            HistoryParam[1] = _CamType;
            HistoryParam[2] = SerialNum;
            HistoryParam[3] = ModelName;
            HistoryParam[4] = _LastResult;
            HistoryParam[5] = _ImageFilePath;

            CHistoryManager.AddHistory(HistoryParam);
            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, String.Format("InspectionHistory End"), CLogManager.LOG_LEVEL.LOW);
        }

        private void gradientLabelTrimTotalCount_DoubleClick(object sender, EventArgs e)
        {
            DialogResult _DlgResult = MessageBox.Show(new Form { TopMost = true }, "Clear Result Count ?", "Clear Count", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
            if (_DlgResult != DialogResult.Yes) return;

            TrimTotalCount = 0;
            TrimGoodCount = 0;
            TrimNgCount = 0;
            TrimYield = 0;

            SaveResultCount();
        }

        private void gradientLabelTrimGoodCount_DoubleClick(object sender, EventArgs e)
        {
            DialogResult _DlgResult = MessageBox.Show(new Form { TopMost = true }, "Clear Result Count ?", "Clear Count", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
            if (_DlgResult != DialogResult.Yes) return;

            TrimTotalCount -= TrimGoodCount;
            TrimGoodCount = 0;

            if (TrimTotalCount != 0)
                TrimYield = (double)TrimGoodCount / (double)TrimTotalCount * 100;
            else
                TrimYield = 0;

            SaveResultCount();
        }

        private void gradientLabelTrimNgCount_DoubleClick(object sender, EventArgs e)
        {
            DialogResult _DlgResult = MessageBox.Show(new Form { TopMost = true }, "Clear Result Count ?", "Clear Count", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
            if (_DlgResult != DialogResult.Yes) return;

            TrimTotalCount -= TrimNgCount;
            TrimNgCount = 0;

            if (TrimTotalCount != 0)
                TrimYield = (double)TrimGoodCount / (double)TrimTotalCount * 100;
            else
                TrimYield = 0;

            SaveResultCount();
        }

        private void gradientLabelFormTotalCount_DoubleClick(object sender, EventArgs e)
        {
            DialogResult _DlgResult = MessageBox.Show(new Form { TopMost = true }, "Clear Result Count ?", "Clear Count", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
            if (_DlgResult != DialogResult.Yes) return;

            FormTotalCount = 0;
            FormGoodCount = 0;
            FormNgCount = 0;
            FormYield = 0;

            SaveResultCount();
        }

        private void gradientLabelFormGoodCount_DoubleClick(object sender, EventArgs e)
        {
            DialogResult _DlgResult = MessageBox.Show(new Form { TopMost = true }, "Clear Result Count ?", "Clear Count", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
            if (_DlgResult != DialogResult.Yes) return;

            FormTotalCount -= FormGoodCount;
            FormGoodCount = 0;

            if (FormTotalCount != 0)
                FormYield = (double)FormGoodCount / (double)FormTotalCount * 100;
            else
                FormYield = 0;

            SaveResultCount();
        }

        private void gradientLabelFormNgCount_DoubleClick(object sender, EventArgs e)
        {
            DialogResult _DlgResult = MessageBox.Show(new Form { TopMost = true }, "Clear Result Count ?", "Clear Count", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
            if (_DlgResult != DialogResult.Yes) return;

            FormTotalCount -= FormNgCount;
            FormNgCount = 0;

            if (FormTotalCount != 0)
                FormYield = (double)FormGoodCount / (double)FormTotalCount * 100;
            else
                FormYield = 0;

            SaveResultCount();
        }
    }
}
