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

using CustomControl;
using ParameterManager;
using LogMessageManager;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;

namespace KPVisionInspectionFramework
{
    public partial class ucMainResultCardManager : UserControl
    {
        private bool AutoModeFlag = false;

        private string[] HistoryParam;
        private string[] LastRecipeName;
        private string LastResult;
        private string ImageFolderPath;

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

        public void SetDataFolderPath(string _DataFolderPath)
        {
            ImageFolderPath = _DataFolderPath;
        }

        public void SaveImageJPG(CogImage8Grey _SaveImage)
        {
            DateTime dateTime = DateTime.Now;
            if (false == Directory.Exists(ImageFolderPath)) Directory.CreateDirectory(ImageFolderPath);
            ImageFolderPath = String.Format("{0}\\{1:D4}\\{2:D2}\\{3:D2}", ImageFolderPath, dateTime.Year, dateTime.Month, dateTime.Day);
            if (false == Directory.Exists(ImageFolderPath)) Directory.CreateDirectory(ImageFolderPath);

            string ImageSaveFile;
            ImageSaveFile = String.Format("{0}\\{1:D2}{2:D2}{3:D2}{4:D3}.jpg", ImageFolderPath, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);

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
                    _CogImageFile.Open(ImageSaveFile, CogImageFileModeConstants.Write);
                    _CogImageFile.Append(_CogSaveImage);
                    _CogImageFile.Close();
                }
            }
            catch
            {
                CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.ERR, "SetDisplayImageJPG(string) Exception!!", CLogManager.LOG_LEVEL.LOW);
            }
        }

        //LDH, 2019.04.02, Ethernet string 결과창에 출력
        public void SetEthernetRecvData(string[] _Value)
        {
            string _RecvString = string.Join(",", _Value);

            ControlInvoke.GradientLabelText(gradientLabelEtherRecv1, _RecvString);
        }

        //LDH, 2019.03.20, Inspection #1, JPG로 저장 결과
        public void SetImageSaveResultData(SendResultParameter _ResultParam)
        {
            var _Result = _ResultParam.SendResult as SendCardImageSaveResult;

            if (_ResultParam.IsGood)
            {
                SaveImageJPG(_Result.SaveImage);
            }
        }

        //LDH, 2019.03.20, Inspection #2, QRCode 검사 결과
        public void SetQRCodeResultData(SendResultParameter _ResultParam)
        {
            var _Result = _ResultParam.SendResult as SendCardIDResult;

            if(_ResultParam.IsGood)
            {

            }
        }

        //LDH, 2019.03.20, Inspection #3, 카드 유무 검사 결과
        public void SetExistResultData(SendResultParameter _ResultParam)
        {
            var _Result = _ResultParam.SendResult as SendCardExistResult;

            int CardCount = 2;

            bool LastResultFlag = true;

            if (_Result != null)
            {
                for(int iLoopCount = 0; iLoopCount < CardCount; iLoopCount++)
                {
                    LastResultFlag &= _Result.IsGoods[iLoopCount];
                }
            }

            switch(LastResultFlag)
            {
                case true: ControlInvoke.GradientLabelText(gradientLabelResult3, "OK", Color.Lime); break;
                case false: ControlInvoke.GradientLabelText(gradientLabelResult3, "NG", Color.Red); break;
            }

        }

        //LDH, 2019.03.20, Inspection #4, 2번째 QRCode 검사 결과 
        public void SetSecondQRCodeResultData(SendResultParameter _ResultParam)
        {

        }
    }
}
