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

        private bool AutoModeFlag = false;

        #endregion Count & Yield Variable

        private string[] LastRecipeName;

        string[] LeftHeaderName;
        string[] RightHeaderName;

        string ProductCode = "";

        string[] CSVData = new string[14];
        string[] CSVHeader;

        bool[] LeftResultUseFlag;
        bool[] RightResultUseFlag;

        bool[] LastIsGoodFlag = new bool[2];

        GradientLabel[] GradientLabelResultLeft;
        GradientLabel[] GradientLabelResultRight;

        public delegate void ScreenshotHandler(string ScreenshotImagePath, Size ScreenshotSize);
        public event ScreenshotHandler ScreenshotEvent;

        public delegate void DIOResultHandler(bool _LastResult);
        public event DIOResultHandler DIOResultEvent;

        public ucMainResultNavien(string[] _LastRecipeName)
        {
            InitializeComponent();

            LastRecipeName = new string[_LastRecipeName.Count()];
            SetLastRecipeName(LastRecipeName);

            LeftHeaderName = new string[] { "Num", "Min", "Max", "Length" };
            RightHeaderName = new string[] { "Num", "Min", "Max", "Length" };

            CSVHeader = new string[14] { "일시", "Barcode", "오링Min", "오링Max", "오리피스1-1", "오리피스1-2", "볼트1", "볼트2", "오리피스2", "패킹2", "패킹1-가Min", "패킹1-가Max", "패킹1-나Min", "패킹1-나Max" };

            GradientLabelResultRight = new GradientLabel[] { gradientLabelResultRight1, gradientLabelResultRight2, gradientLabelResultRight3 };
            GradientLabelResultLeft = new GradientLabel[] { gradientLabelResultLeft1, gradientLabelResultLeft2 };

            SetGridViewHeader(dataGridViewLeft, LeftHeaderName.Count(), LeftHeaderName);
            SetGridViewHeader(dataGridViewRight, RightHeaderName.Count(), RightHeaderName);

            SetGridViewRowInit(dataGridViewLeft, 3);
            SetGridViewRowInit(dataGridViewRight, 6);

            dataGridViewLeft.ClearSelection();
            dataGridViewRight.ClearSelection();

            LastIsGoodFlag[0] = true;
            LastIsGoodFlag[1] = true;

            LeftResultUseFlag = new bool[GradientLabelResultLeft.Count()];
            RightResultUseFlag = new bool[GradientLabelResultRight.Count()];
            for (int iLoopCount = 0; iLoopCount < LeftResultUseFlag.Count(); iLoopCount++) { LeftResultUseFlag[iLoopCount] = true; }
            for (int iLoopCount = 0; iLoopCount < RightResultUseFlag.Count(); iLoopCount++) { RightResultUseFlag[iLoopCount] = true; }
        }

        private void SetGridViewHeader(DataGridView _GridView, int _HeaderCount, string[] _HeaderName)
        {
            _GridView.ColumnCount = _HeaderCount;

            for (int iLoopCount = 0; iLoopCount < _HeaderCount; iLoopCount++)
            {
                _GridView.Columns[iLoopCount].Name = _HeaderName[iLoopCount];

                if (iLoopCount == 0) _GridView.Columns[iLoopCount].Width = 50;
                else _GridView.Columns[iLoopCount].Width = 100;
            }

            _GridView.Refresh();
        }

        private void SetGridViewRowInit(DataGridView _GridView, int _RowCount)
        {
            for (int iLoopCount = 0; iLoopCount < _RowCount; iLoopCount++)
            {
                _GridView.Rows.Add();
                _GridView.Rows[iLoopCount].Cells[0].Value = iLoopCount + 1;
            }

            _GridView.Refresh();
        }

        public void ClearResult(int _GridViewNum = 2)
        {
            switch(_GridViewNum)
            {
                case 0: ClearLeftResult(); break;
                case 1: ClearRightResult(); break;
                case 2: ClearLeftResult(); ClearRightResult(); break;
            }
        }

        private void ClearLeftResult()
        {
            LastIsGoodFlag[0] = true;

            for (int iLoopCount = 0; iLoopCount < dataGridViewLeft.RowCount; iLoopCount++)
            {
                for (int jLoopCount = 1; jLoopCount < dataGridViewLeft.ColumnCount; jLoopCount++)
                {
                    dataGridViewLeft.Rows[iLoopCount].Cells[jLoopCount].Value = "-";
                }

                ControlInvoke.GridViewRowsColor(dataGridViewLeft, iLoopCount, Color.FromKnownColor(KnownColor.Window), Color.Black);
            }
        }

        private void ClearRightResult()
        {
            LastIsGoodFlag[1] = true;

            for (int iLoopCount = 0; iLoopCount < dataGridViewRight.RowCount; iLoopCount++)
            {
                for (int jLoopCount = 1; jLoopCount < dataGridViewRight.ColumnCount; jLoopCount++)
                {
                    dataGridViewRight.Rows[iLoopCount].Cells[jLoopCount].Value = "-";
                }

                ControlInvoke.GridViewRowsColor(dataGridViewRight, iLoopCount, Color.FromKnownColor(KnownColor.Window), Color.Black);
            }
        }

        private void dataGridViewLeft_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewLeft.ClearSelection();
        }

        private void dataGridViewRight_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewRight.ClearSelection();
        }

        private void textBoxBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            ProductCode = textBoxBarcode.Text;

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                panelLeft.Focus();
            }
        }

        private void gradientLabelLeftResult_DoubleClick(object sender, EventArgs e)
        {
            int _Tag = Convert.ToInt32(((GradientLabel)sender).Tag);

            LeftResultUseFlag[_Tag] = !LeftResultUseFlag[_Tag];

            if (LeftResultUseFlag[_Tag])
            {
                ControlInvoke.GradientLabelColor(GradientLabelResultLeft[_Tag], Color.White, Color.White);
                ControlInvoke.GradientLabelText(GradientLabelResultLeft[_Tag], "-", Color.Black);
            }
            else
            {
                ControlInvoke.GradientLabelColor(GradientLabelResultLeft[_Tag], Color.White, Color.DarkGray);
                ControlInvoke.GradientLabelText(GradientLabelResultLeft[_Tag], "-", Color.Black);
            }
        }

        private void gradientLabelRightResult_DoubleClick(object sender, EventArgs e)
        {
            int _Tag = Convert.ToInt32(((GradientLabel)sender).Tag);

            RightResultUseFlag[_Tag] = !RightResultUseFlag[_Tag];

            if (RightResultUseFlag[_Tag])
            {
                ControlInvoke.GradientLabelColor(GradientLabelResultRight[_Tag], Color.White, Color.White);
                ControlInvoke.GradientLabelText(GradientLabelResultRight[_Tag], "-", Color.Black);
            }
            else
            {
                ControlInvoke.GradientLabelColor(GradientLabelResultRight[_Tag], Color.White, Color.DarkGray);
                ControlInvoke.GradientLabelText(GradientLabelResultRight[_Tag], "-", Color.Black);
            }
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

        public void SetResult(SendResultParameter _ResultParam)
        {
            //lock
            if (_ResultParam.ID == 0) SetLeftResultData(_ResultParam);
            else SetRightResultData(_ResultParam);
        }

        private void WriteCSVFile()
        {
            CSVManagerStringArr SaveCSVControl = new CSVManagerStringArr();

            DateTime dateTime = DateTime.Now;
            string SaveImageFilePath = @"D:\VisionInspectionData\NavienInspection\CSVData";
            if (false == Directory.Exists(SaveImageFilePath)) Directory.CreateDirectory(SaveImageFilePath);
            SaveImageFilePath = String.Format("{0}\\{1:D4}\\{2:D2}", SaveImageFilePath, dateTime.Year, dateTime.Month);
            if (false == Directory.Exists(SaveImageFilePath)) Directory.CreateDirectory(SaveImageFilePath);
            SaveImageFilePath = String.Format("{0}\\VisionData_{1:D4}{2:D2}{3:D2}.csv", SaveImageFilePath, dateTime.Year, dateTime.Month, dateTime.Day);

            CSVData[0] = String.Format("{0:D2}{1:D2}{2:D2}{3:D3}", dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
            CSVData[1] = ProductCode;

            SaveCSVControl.SaveStringArrAll(CSVHeader, CSVData, SaveImageFilePath);
        }

        private void CalculateDiameter(double[] _FoundPointX, double[] _FoundPointY, ref List<double> _DiameterResultList)
        {
            int _Cnt = _FoundPointX.Count() / 2;
            
            for (int iLoopCount = 0; iLoopCount < _Cnt; iLoopCount++)
            {
                if (_FoundPointX[iLoopCount] != 0 && _FoundPointX[iLoopCount + _Cnt] != 0)
                {
                    double _DiameterTemp = 0.0;
                    double _distanceX = _FoundPointX[iLoopCount + _Cnt] - _FoundPointX[iLoopCount];
                    double _distanceY = _FoundPointY[iLoopCount + _Cnt] - _FoundPointY[iLoopCount];

                    _DiameterTemp = Math.Sqrt(Math.Pow(_distanceX, 2) + Math.Pow(_distanceY, 2));

                    _DiameterResultList.Add(_DiameterTemp);
                }
            }

            _DiameterResultList.Sort();
        }

        public void SetLeftResultData(SendResultParameter _ResultParam)
        {
            bool[] LastResultFlag = new bool[GradientLabelResultLeft.Count()];
            for (int ResultCnt = 0; ResultCnt < GradientLabelResultLeft.Count(); ResultCnt++)
            {
                LastResultFlag[ResultCnt] = true;
            }

            ClearLeftResult();

            for (int iLoopCount = 0; iLoopCount < _ResultParam.SendResultList.Count(); iLoopCount++)
            {
                var _ResultData = _ResultParam.SendResultList[iLoopCount] as SendMeasureResult;

                if (LeftResultUseFlag[_ResultData.NGAreaNum - 1])
                {
                    if (_ResultParam.AlgoTypeList[iLoopCount] == eAlgoType.C_ELLIPSE)
                    {
                        List<double> _DiameterResultList = new List<double>();
                        double _DiameterMin = 0.0;
                        double _DiameterMax = 0.0;
                        double _DiameterEvg = 0.0;

                        CalculateDiameter(_ResultData.CaliperPointX, _ResultData.CaliperPointY, ref _DiameterResultList);

                        if (_DiameterResultList.Count != 0)
                        {
                            _DiameterMin = _DiameterResultList[1] * 0.0145;
                            _DiameterMax = _DiameterResultList[_DiameterResultList.Count - 1] * 0.0145;

                            if (_DiameterMin < _ResultData.DiameterMinAlgo) { _DiameterEvg = _DiameterMin; _ResultData.IsGoodAlgo = false; }
                            else if (_DiameterMax > _ResultData.DiameterMaxAlgo) { _DiameterEvg = _DiameterMax; _ResultData.IsGoodAlgo = false; }
                            else
                            {
                                double _SumDiameter = 0.0;

                                for (int ListLoopCnt = 1; ListLoopCnt < _DiameterResultList.Count - 1; ListLoopCnt++)
                                {
                                    _SumDiameter = _SumDiameter + _DiameterResultList[ListLoopCnt];
                                }

                                _DiameterEvg = _SumDiameter / (_DiameterResultList.Count - 2) * 0.0145;
                            }
                        }

                        if (iLoopCount == 0)
                        {
                            ControlInvoke.GridViewCellText(dataGridViewLeft, iLoopCount, 1, string.Format("{0:F3}", _DiameterMin));
                            ControlInvoke.GridViewCellText(dataGridViewLeft, iLoopCount, 2, string.Format("{0:F3}", _DiameterMax));
                        }
                        else
                        {
                            ControlInvoke.GridViewCellText(dataGridViewLeft, iLoopCount, 3, string.Format("{0:F3}", _DiameterEvg));
                        }
                    }

                    LastResultFlag[_ResultData.NGAreaNum - 1] &= _ResultData.IsGoodAlgo;

                    if (!_ResultData.IsGoodAlgo) ControlInvoke.GridViewRowsColor(dataGridViewLeft, iLoopCount, Color.Maroon, Color.White);
                    else ControlInvoke.GridViewRowsColor(dataGridViewLeft, iLoopCount, Color.FromKnownColor(KnownColor.Window), Color.Black);
                }
            }

            for (int jLoopCount = 0; jLoopCount < LastResultFlag.Count(); jLoopCount++)
            {
                if (LeftResultUseFlag[jLoopCount])
                {
                    switch (LastResultFlag[jLoopCount])
                    {
                        case true:
                            {
                                ControlInvoke.GradientLabelText(GradientLabelResultLeft[jLoopCount], "OK", Color.White);
                                ControlInvoke.GradientLabelColor(GradientLabelResultLeft[jLoopCount], Color.DarkGreen, Color.FromArgb(0, 44, 0));
                            }
                            break;
                        case false:
                            {
                                ControlInvoke.GradientLabelText(GradientLabelResultLeft[jLoopCount], "NG", Color.White);
                                ControlInvoke.GradientLabelColor(GradientLabelResultLeft[jLoopCount], Color.Maroon, Color.FromArgb(49, 0, 0));
                            }
                            break;
                    }

                    LastIsGoodFlag[0] &= LastResultFlag[jLoopCount];
                }
            }

            //CSV 배열에 결과값 담기
            CSVData[2] = dataGridViewLeft[1, 0].Value.ToString();
            CSVData[3] = dataGridViewLeft[2, 0].Value.ToString();
            CSVData[4] = dataGridViewLeft[3, 1].Value.ToString();
            CSVData[5] = dataGridViewLeft[3, 2].Value.ToString();
            
            dataGridViewLeft.ClearSelection();
        }

        public void SetRightResultData(SendResultParameter _ResultParam)
        {
            bool[] LastResultFlag = new bool[GradientLabelResultRight.Count()];
            for (int ResultCnt = 0; ResultCnt < GradientLabelResultRight.Count(); ResultCnt++)
            {
                LastResultFlag[ResultCnt] = true;
            }

            ClearRightResult();

            for (int iLoopCount = 0; iLoopCount < _ResultParam.SendResultList.Count(); iLoopCount++)
            {
                var _ResultData = _ResultParam.SendResultList[iLoopCount] as SendMeasureResult;

                if (RightResultUseFlag[_ResultData.NGAreaNum - 1])
                {
                    if (_ResultParam.AlgoTypeList[iLoopCount] == eAlgoType.C_ELLIPSE)
                    {
                        List<double> _DiameterResultList = new List<double>();
                        double _DiameterMin = 0.0;
                        double _DiameterMax = 0.0;
                        double _DiameterEvg = 0.0;

                        CalculateDiameter(_ResultData.CaliperPointX, _ResultData.CaliperPointY, ref _DiameterResultList);

                        if (_DiameterResultList.Count != 0)
                        {
                            _DiameterMin = _DiameterResultList[1] * 0.0145;
                            _DiameterMax = _DiameterResultList[_DiameterResultList.Count - 1] * 0.0145;

                            if (_DiameterMin < _ResultData.DiameterMinAlgo) { _DiameterEvg = _DiameterMin; _ResultData.IsGoodAlgo = false; }
                            else if (_DiameterMax > _ResultData.DiameterMaxAlgo) { _DiameterEvg = _DiameterMax; _ResultData.IsGoodAlgo = false; }
                            else
                            {
                                double _SumDiameter = 0.0;

                                for (int ListLoopCnt = 1; ListLoopCnt < _DiameterResultList.Count - 1; ListLoopCnt++)
                                {
                                    _SumDiameter = _SumDiameter + _DiameterResultList[ListLoopCnt];
                                }

                                _DiameterEvg = _SumDiameter / (_DiameterResultList.Count - 2) * 0.0145;
                            }
                        }

                        if (iLoopCount < 4)
                        {
                            ControlInvoke.GridViewCellText(dataGridViewRight, iLoopCount, 3, string.Format("{0:F3}", _DiameterEvg));
                        }
                        else
                        {
                            ControlInvoke.GridViewCellText(dataGridViewRight, iLoopCount, 1, string.Format("{0:F3}", _DiameterMin));
                            ControlInvoke.GridViewCellText(dataGridViewRight, iLoopCount, 2, string.Format("{0:F3}", _DiameterMax));
                        }

                    }

                    else if (_ResultParam.AlgoTypeList[iLoopCount] == eAlgoType.C_BLOB_REFER)
                    {
                        ControlInvoke.GridViewCellText(dataGridViewRight, iLoopCount, 3, string.Format("{0:F3}", _ResultData.MeasureData));
                    }

                    LastResultFlag[_ResultData.NGAreaNum - 1] &= _ResultData.IsGoodAlgo;


                    if (!_ResultData.IsGoodAlgo) ControlInvoke.GridViewRowsColor(dataGridViewRight, iLoopCount, Color.Maroon, Color.White);
                    else ControlInvoke.GridViewRowsColor(dataGridViewRight, iLoopCount, Color.FromKnownColor(KnownColor.Window), Color.Black);
                }
            }

            for (int jLoopCount = 0; jLoopCount < LastResultFlag.Count(); jLoopCount++)
            {
                if (RightResultUseFlag[jLoopCount])
                {
                    switch (LastResultFlag[jLoopCount])
                    {
                        case true:
                            {
                                ControlInvoke.GradientLabelText(GradientLabelResultRight[jLoopCount], "OK", Color.White);
                                ControlInvoke.GradientLabelColor(GradientLabelResultRight[jLoopCount], Color.DarkGreen, Color.FromArgb(0, 44, 0));
                            }
                            break;
                        case false:
                            {
                                ControlInvoke.GradientLabelText(GradientLabelResultRight[jLoopCount], "NG", Color.White);
                                ControlInvoke.GradientLabelColor(GradientLabelResultRight[jLoopCount], Color.Maroon, Color.FromArgb(49, 0, 0));
                            }
                            break;
                    }

                    LastIsGoodFlag[1] &= LastResultFlag[jLoopCount];
                }
            }

            //CSV 배열에 결과값 담기
            CSVData[6] = dataGridViewRight[3, 0].Value.ToString();
            CSVData[7] = dataGridViewRight[3, 1].Value.ToString();
            CSVData[8] = dataGridViewRight[3, 2].Value.ToString();
            CSVData[9] = dataGridViewRight[3, 3].Value.ToString();
            CSVData[10] = dataGridViewRight[1, 4].Value.ToString();
            CSVData[11] = dataGridViewRight[2, 4].Value.ToString();
            CSVData[12] = dataGridViewRight[1, 5].Value.ToString();
            CSVData[13] = dataGridViewRight[2, 5].Value.ToString();

            if (CParameterManager.SystemMode == eSysMode.AUTO_MODE)
            {
                WriteCSVFile();

                if (LastIsGoodFlag[0] && LastIsGoodFlag[1]) DIOResultEvent(true);
                else                                        DIOResultEvent(false);
                CParameterManager.SystemMode = eSysMode.MANUAL_MODE;
            }

            dataGridViewRight.ClearSelection();
        }
    }
}
