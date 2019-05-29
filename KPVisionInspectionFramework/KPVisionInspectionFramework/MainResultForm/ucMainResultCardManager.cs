using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

using CustomControl;
using ParameterManager;
using LogMessageManager;
using HistoryManager;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using DmitryBrant.CustomControls;

namespace KPVisionInspectionFramework
{
    public partial class ucMainResultCardManager : UserControl
    {
        #region Count & Yield Variable
        private uint[] TotalCount = new uint[4];
        private uint[] GoodCount = new uint[4];
        private uint[] NgCount = new uint[4];
        private double[] Yield = new double[4];

        SevenSegmentArray[] SevenSegTotalCountArr;
        SevenSegmentArray[] SevenSegGoodCountArr;
        SevenSegmentArray[] SevenSegNgCountArr;
        SevenSegmentArray[] SevenSegYieldArr;
        #endregion Count & Yield Variable

        #region Count & Yield Registry Variable
        private RegistryKey[] RegTotalCount = new RegistryKey[4];
        private RegistryKey[] RegGoodCount = new RegistryKey[4];
        private RegistryKey[] RegNgCount = new RegistryKey[4];
        private RegistryKey[] RegYield = new RegistryKey[4];

        private string[] RegTotalCountPath = new string[4];
        private string[] RegGoodCountPath = new string[4];
        private string[] RegNgCountPath = new string[4];
        private string[] RegYieldPath = new string[4];
        #endregion Count & Yield Registry Variable

        private bool AutoModeFlag = false;

        private string[] InspectionTime;

        private string[] HistoryParam;
        private string[] LastRecipeName;
        private string[] ImageFolderPath;
        private string LastResult;

        public delegate void ScreenshotHandler(string ScreenshotImagePath, Size ScreenshotSize);
        public event ScreenshotHandler ScreenshotEvent;

        private object lockObject = new object();

        #region Initialize & DeInitialize
        public ucMainResultCardManager(string[] _LastRecipeName)
        {
            LastRecipeName = new string[_LastRecipeName.Count()];
            ImageFolderPath = new string[2];
            LastResult = "";
            SetLastRecipeName(_LastRecipeName);

            InitializeComponent();
            InitializeControl();
            InitializeLanguage();
            this.Location = new Point(1, 1);

            InspectionTime = new string[4];

            SevenSegTotalCountArr = new SevenSegmentArray[4] { SevenSegTotal1, SevenSegTotal2, SevenSegTotal3, SevenSegTotal4 };
            SevenSegGoodCountArr = new SevenSegmentArray[4] { SevenSegGood1, SevenSegGood2, SevenSegGood3, SevenSegGood4 };
            SevenSegNgCountArr = new SevenSegmentArray[4] { SevenSegNg1, SevenSegNg2, SevenSegNg3, SevenSegNg4 };
            SevenSegYieldArr = new SevenSegmentArray[4] { SevenSegYield1, SevenSegYield2, SevenSegYield3, SevenSegYield4 };

            for (int iLoopCount = 0; iLoopCount < 4; iLoopCount++)
            {
                TotalCount[iLoopCount] = 0;
                GoodCount[iLoopCount] = 0;
                NgCount[iLoopCount] = 0;
                Yield[iLoopCount] = 0;

                RegTotalCountPath[iLoopCount] =  String.Format(@"KPVision\ResultCount\TotalCount{0}",iLoopCount);
                RegGoodCountPath[iLoopCount] = String.Format(@"KPVision\ResultCount\GoodCount{0}", iLoopCount);
                RegNgCountPath[iLoopCount] = String.Format(@"KPVision\ResultCount\NgCount{0}", iLoopCount);
                RegYieldPath[iLoopCount] = String.Format(@"KPVision\ResultCount\Yield{0}", iLoopCount);

                RegTotalCount[iLoopCount] = Registry.CurrentUser.CreateSubKey(RegTotalCountPath[iLoopCount]);
                RegGoodCount[iLoopCount] = Registry.CurrentUser.CreateSubKey(RegGoodCountPath[iLoopCount]);
                RegNgCount[iLoopCount] = Registry.CurrentUser.CreateSubKey(RegNgCountPath[iLoopCount]);
                RegYield[iLoopCount] = Registry.CurrentUser.CreateSubKey(RegYieldPath[iLoopCount]);

                InspectionTime[iLoopCount] = "";
            }
        }

        private void InitializeControl()
        {

        }

        private void InitializeLanguage()
        {
            #region Control Name Setting
            labelResultTitle1.Text = LanguageResource.ResultTitle + " 1";
            labelResultTitle2.Text = LanguageResource.ResultTitle + " 2";
            labelResultTitle3.Text = LanguageResource.ResultTitle + " 3";
            labelResultTitle4.Text = LanguageResource.ResultTitle + " 4";
            gradientLabelTotalCount1.Text = LanguageResource.Total;
            gradientLabelTotalCount2.Text = LanguageResource.Total;
            gradientLabelTotalCount3.Text = LanguageResource.Total;
            gradientLabelTotalCount4.Text = LanguageResource.Total;
            gradientLabelGoodCount1.Text = LanguageResource.Good;
            gradientLabelGoodCount2.Text = LanguageResource.Good;
            gradientLabelGoodCount3.Text = LanguageResource.Good;
            gradientLabelGoodCount4.Text = LanguageResource.Good;
            gradientLabelNgCount1.Text = LanguageResource.NG;
            gradientLabelNgCount2.Text = LanguageResource.NG;
            gradientLabelNgCount3.Text = LanguageResource.NG;
            gradientLabelNgCount4.Text = LanguageResource.NG;
            gradientLabelYield1.Text = LanguageResource.Yield;
            gradientLabelYield2.Text = LanguageResource.Yield;
            gradientLabelYield3.Text = LanguageResource.Yield;
            gradientLabelYield4.Text = LanguageResource.Yield;
            gradientLabelResultComment1.Text = LanguageResource.ResultComment;
            gradientLabelResultComment2.Text = LanguageResource.ResultComment;
            gradientLabelResultComment3.Text = LanguageResource.ResultComment;
            gradientLabelResultComment4.Text = LanguageResource.ResultComment;
            #endregion
        }

        public void DeInitialize()
        {

        }

        //LDH, 2019.04.15,Count 불러올때 사용
        private void LoadResultCount()
        {
            for (int iLoopCount = 0; iLoopCount < 4; iLoopCount++)
            {
                TotalCount[iLoopCount] = Convert.ToUInt32(RegTotalCount[iLoopCount]);
                GoodCount[iLoopCount] = Convert.ToUInt32(RegGoodCount[iLoopCount]);
                NgCount[iLoopCount] = Convert.ToUInt32(RegNgCount[iLoopCount]);
                Yield[iLoopCount] = Convert.ToDouble(RegYield[iLoopCount]);
            }

            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, "Load Result Count");
            CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, String.Format("TotalCount : {0}, GoodCount : {1}, NgCount : {2}, Yield : {3:F3}", TotalCount, GoodCount, NgCount, Yield));
        }
        #endregion Initialize & DeInitialize

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

        //LDH, 2018.10.01, Result clear
        public void ClearResult()
        {
            for(int iLoopCount = 0; iLoopCount < 4; iLoopCount++)
            {
                TotalCount[iLoopCount] = 0;
                GoodCount[iLoopCount] = 0;
                NgCount[iLoopCount] = 0;
                Yield[iLoopCount] = 0.0;

                SevenSegTotalCountArr[iLoopCount].Value = TotalCount[iLoopCount].ToString();
                SevenSegGoodCountArr[iLoopCount].Value = GoodCount[iLoopCount].ToString();
                SevenSegNgCountArr[iLoopCount].Value = NgCount[iLoopCount].ToString();
                SevenSegYieldArr[iLoopCount].Value = Yield[iLoopCount].ToString("F2");
            }

            HistoryParam = new string[4];
            for (int iLoopCount = 0; iLoopCount < HistoryParam.Count(); iLoopCount++)
            {
                HistoryParam[iLoopCount] = "-";
            }
        }

        public void SetDataFolderPath(string[] _DataFolderPath)
        {
            for(int iLoopCount = 0; iLoopCount < 2; iLoopCount++) ImageFolderPath[iLoopCount] = _DataFolderPath[iLoopCount];
        }

        public void SaveImageJPG(int _InspectionNum, CogImage8Grey _SaveImage, string SaveImageFileName = "")
        {
            DateTime dateTime = DateTime.Now;
            string SaveImageFilePath = ImageFolderPath[_InspectionNum];
            if (false == Directory.Exists(SaveImageFilePath)) Directory.CreateDirectory(SaveImageFilePath);
            SaveImageFilePath = String.Format("{0}\\{1:D4}\\{2:D2}\\{3:D2}", SaveImageFilePath, dateTime.Year, dateTime.Month, dateTime.Day);
            if (false == Directory.Exists(SaveImageFilePath)) Directory.CreateDirectory(SaveImageFilePath);

            if(SaveImageFileName == "")
            {
                SaveImageFileName = String.Format("{0}\\{1:D2}{2:D2}{3:D2}{4:D3}.jpg", SaveImageFilePath, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
            }
            else
            {
                SaveImageFileName = String.Format("{0}\\{1}.jpg", SaveImageFilePath, SaveImageFileName);
            }

            try
            {
                ICogImage _CogSaveImage = _SaveImage;
                CogImageFile _CogImageFile = new CogImageFile();

                if (_CogSaveImage == null)
                {
                    //MessageBox.Show(new Form{TopMost = true}, "영상이 없습니다.");
                }
                else
                {
                    _CogImageFile.Open(SaveImageFileName, CogImageFileModeConstants.Write);
                    _CogImageFile.Append(_CogSaveImage);
                    _CogImageFile.Close();
                }
            }
            catch
            {
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.ERR, "SetDisplayImageJPG(string) Exception!!", CLogManager.LOG_LEVEL.LOW);
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

        //LDH, 2019.04.02, Ethernet string 결과창에 출력
        public void SetEthernetRecvData(object _Value)
        {
            EthernetRecvInfo _RecvInfo = _Value as EthernetRecvInfo;

            string _RecvString = string.Join(";", _RecvInfo.RecvData);

            switch (_RecvInfo.PortNumber)
            {
                case 5000: ControlInvoke.GradientLabelText(gradientLabelEtherRecv1, _RecvString); break;
                case 5001: ControlInvoke.GradientLabelText(gradientLabelEtherRecv2, _RecvString); break;
                case 5002: ControlInvoke.GradientLabelText(gradientLabelEtherRecv3, _RecvString); break;
                case 5003: ControlInvoke.GradientLabelText(gradientLabelEtherRecv4, _RecvString); break;
            }
            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.ERR, _RecvString, CLogManager.LOG_LEVEL.LOW);
        }

        public void SetResult(SendResultParameter _ResultParam)
        {
            lock (lockObject)
            {
                if      (_ResultParam.ProjectItem == eProjectItem.BC_IMG_SAVE) 	SetImageSaveResultData(_ResultParam);
                else if (_ResultParam.ProjectItem == eProjectItem.BC_ID) 		SetQRCodeResultData(_ResultParam);
                else if (_ResultParam.ProjectItem == eProjectItem.BC_ID_SECOND) SetSecondQRCodeResultData(_ResultParam);
                else if (_ResultParam.ProjectItem == eProjectItem.BC_EXIST) 	SetExistResultData(_ResultParam);
            }
        }

        //LDH, 2019.03.20, Inspection #1, JPG로 저장 결과
        public void SetImageSaveResultData(SendResultParameter _ResultParam)
        {
            var _Result = _ResultParam.SendResult as SendCardImageSaveResult;

            bool LastResultFlag = true;

            if (_ResultParam.IsGood)
            {
                try
                {
                    SaveImageJPG(0, _Result.SaveImage);
                }
                catch
                {
                    LastResultFlag = false;
                }
            }

            _ResultParam.IsGood = LastResultFlag;

            switch (LastResultFlag)
            {
                case true: ControlInvoke.GradientLabelText(gradientLabelResult1, "OK", Color.Lime); break;
                case false: ControlInvoke.GradientLabelText(gradientLabelResult1, "NG", Color.Red); break;
            }

            if (AutoModeFlag)
            {
                TotalCount[0]++;
                if (LastResultFlag) GoodCount[0]++;
                else                NgCount[0]++;

                Yield[0] = (double)GoodCount[0] / (double)TotalCount[0] * 100;

                SegmentValueInvoke(SevenSegTotalCountArr[0], TotalCount[0].ToString());
                SegmentValueInvoke(SevenSegGoodCountArr[0], GoodCount[0].ToString());
                SegmentValueInvoke(SevenSegNgCountArr[0], NgCount[0].ToString());
                SegmentValueInvoke(SevenSegYieldArr[0], Yield[0].ToString("F2"));
            }
        }

        //LDH, 2019.03.20, Inspection #2, 카드 유무 검사 결과
        public void SetExistResultData(SendResultParameter _ResultParam)
        {
            var _Result = _ResultParam.SendResult as SendCardExistResult;

            int CardCount = 2;

            bool SetResultlabelFlag = true;
            bool LastResultFlag = true;
            string LastResultString = "";

            if (_Result != null)
            {
                for (int iLoopCount = 0; iLoopCount < CardCount; iLoopCount++)
                {
                    if (_Result.IsGoods[iLoopCount]) LastResultString = LastResultString + "1";
                    else LastResultString = LastResultString + "0";
                }

                string[] RecvData = gradientLabelEtherRecv2.Text.Split(';');

                if (RecvData.Length >= 3)
                {
                    if (LastResultString != RecvData[1]) LastResultFlag = false;

                    SaveImageJPG(1, _Result.SaveImage, RecvData[2].Substring(1, RecvData[2].Length - 1));
                }
                else
                {
                    SetResultlabelFlag = false;
                    ControlInvoke.GradientLabelText(gradientLabelResult2, LastResultString, Color.Lime);
                }
            }
            else LastResultFlag = false;

            _ResultParam.IsGood = LastResultFlag;

            DateTime dateTime = DateTime.Now;
            InspectionTime[1] = String.Format("{0:D2}{1:D2}{2:D2}{3:D3}", dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);

            if (SetResultlabelFlag)
            {
                switch (LastResultFlag)
                {
                    case true: ControlInvoke.GradientLabelText(gradientLabelResult2, "OK", Color.Lime); break;
                    case false: ControlInvoke.GradientLabelText(gradientLabelResult2, "NG", Color.Red); break;
                }
            }

            if (AutoModeFlag)
            {
                TotalCount[1]++;
                if (LastResultFlag) GoodCount[1]++;
                else NgCount[1]++;

                Yield[1] = (double)GoodCount[1] / (double)TotalCount[1] * 100;

                SegmentValueInvoke(SevenSegTotalCountArr[1], TotalCount[1].ToString());
                SegmentValueInvoke(SevenSegGoodCountArr[1], GoodCount[1].ToString());
                SegmentValueInvoke(SevenSegNgCountArr[1], NgCount[1].ToString());
                SegmentValueInvoke(SevenSegYieldArr[1], Yield[1].ToString("F2"));
            }

            //this.Refresh();
            InspectionHistory("2");
        }

        //LDH, 2019.03.20, Inspection #3, QRCode 검사 결과
        public void SetQRCodeResultData(SendResultParameter _ResultParam)
        {
            var _Result = _ResultParam.SendResult as SendCardIDResult;

            bool LastResultFlag = true;

            if (_Result != null)
            {
                LastResultFlag = _ResultParam.IsGood;

                if (LastResultFlag)
                {
                    string[] RecvData = gradientLabelEtherRecv3.Text.Split(';');
                    if (_Result.ReadCode != RecvData[0]) LastResultFlag = false;
                }
            }

            _ResultParam.IsGood = LastResultFlag;

            DateTime dateTime = DateTime.Now;
            InspectionTime[2] = String.Format("{0:D2}{1:D2}{2:D2}{3:D3}", dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);

            switch (LastResultFlag)
            {
                case true: ControlInvoke.GradientLabelText(gradientLabelResult3, "OK", Color.Lime); break;
                case false: ControlInvoke.GradientLabelText(gradientLabelResult3, "NG", Color.Red); break;
            }

            if (AutoModeFlag)
            {
                TotalCount[2]++;
                if (LastResultFlag) GoodCount[2]++;
                else                NgCount[2]++;

                Yield[2] = (double)GoodCount[2] / (double)TotalCount[2] * 100;

                SegmentValueInvoke(SevenSegTotalCountArr[2], TotalCount[2].ToString());
                SegmentValueInvoke(SevenSegGoodCountArr[2], GoodCount[2].ToString());
                SegmentValueInvoke(SevenSegNgCountArr[2], NgCount[2].ToString());
                SegmentValueInvoke(SevenSegYieldArr[2], Yield[2].ToString("F2"));
            }

            //this.Refresh();
            InspectionHistory("3", _Result.ReadCode);
        }

        //LDH, 2019.03.20, Inspection #4, 2번째 QRCode 검사 결과 
        public void SetSecondQRCodeResultData(SendResultParameter _ResultParam)
        {
            var _Result = _ResultParam.SendResult as SendCardIDResult;
            
            bool LastResultFlag = true;

            if (_Result != null)
            {
                LastResultFlag = _ResultParam.IsGood;

                if (LastResultFlag)
                {
                    string[] RecvData = gradientLabelEtherRecv4.Text.Split(';');
                    if (_Result.ReadCode != RecvData[0]) LastResultFlag = false;
                }
            }

            _ResultParam.IsGood = LastResultFlag;

            DateTime dateTime = DateTime.Now;
            InspectionTime[3] = String.Format("{0:D2}{1:D2}{2:D2}{3:D3}", dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);

            switch (LastResultFlag)
            {
                case true: ControlInvoke.GradientLabelText(gradientLabelResult4, "OK", Color.Lime); break;
                case false: ControlInvoke.GradientLabelText(gradientLabelResult4, "NG", Color.Red); break;
            }

            if (AutoModeFlag)
            {
                TotalCount[3]++;
                if (LastResultFlag) GoodCount[3]++;
                else                NgCount[3]++;

                Yield[3] = (double)GoodCount[3] / (double)TotalCount[3] * 100;

                SegmentValueInvoke(SevenSegTotalCountArr[3], TotalCount[3].ToString());
                SegmentValueInvoke(SevenSegGoodCountArr[3], GoodCount[3].ToString());
                SegmentValueInvoke(SevenSegNgCountArr[3], NgCount[3].ToString());
                SegmentValueInvoke(SevenSegYieldArr[3], Yield[3].ToString("F2"));
            }

            //this.Refresh();
            InspectionHistory("4");
        }

        //LDH, 2019.04.16, History 추가용 함수
        private void InspectionHistory(string _InspectionNum, string _ReadCodeData = "")
        {
            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, String.Format("InspectionHistory Start"), CLogManager.LOG_LEVEL.LOW);

            DateTime dateTime = DateTime.Now;
            string InspScreenshotPath = @"D:\VisionInspectionData\BCCard\HistoryData\Screenshot\";
            string ImageSaveFolder = String.Format("{0}{1:D4}\\{2:D2}\\{3:D2}", InspScreenshotPath, dateTime.Year, dateTime.Month, dateTime.Day);

            if (false == Directory.Exists(ImageSaveFolder)) Directory.CreateDirectory(ImageSaveFolder);

            string ImageSaveFile;
            //ImageSaveFile = String.Format("{0}\\{1:D2}{2:D2}{3:D2}{4:D3}.bmp", ImageSaveFolder, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
            ImageSaveFile = String.Format("{0}\\{1}.jpg", ImageSaveFolder, InspectionTime[Convert.ToInt32(_InspectionNum)-1]);

            //LDH, 2018.08.13, 프로젝트별로 DB에 해당하는 history 내역을 string 배열로 전달
            HistoryParam[0] = _InspectionNum;
            HistoryParam[1] = LastResult;
            HistoryParam[2] = _ReadCodeData;
            HistoryParam[3] = ImageSaveFile;

            CHistoryManager.AddHistory(HistoryParam);
            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, String.Format("InspectionHistory End"), CLogManager.LOG_LEVEL.LOW);

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, String.Format("Screenshot Start"), CLogManager.LOG_LEVEL.LOW);
            ScreenshotEvent(ImageSaveFile, new Size(635,900));
            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, String.Format("Screenshot End"), CLogManager.LOG_LEVEL.LOW);
        }
    }
}
