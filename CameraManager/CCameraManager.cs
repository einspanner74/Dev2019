using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParameterManager; 

namespace CameraManager
{
    public class CCameraManager
    {
        public delegate void ImageGrabHandler(byte[] GrabImage);
        public event ImageGrabHandler ImageGrabEvent;

        public delegate void ImageGrabIntPtrHandler(IntPtr GrabImage);
        public event ImageGrabIntPtrHandler ImageGrabIntPtrEvent;

        private CEuresysManager objEuresysManager;
        private CEuresysIOTAManager objEuresysIOTAManager;
        private CBaslerManager objBaslerManager;

        private string CameraType;
        bool CamLiveFlag = false;

        public CCameraManager()
        {

        }

        public bool Initialize(int _ID, string _CamType, string _CamInfo)
        {
            bool _Result = true;

            CameraType = _CamType;

            if (CameraType == eCameraType.Euresys.ToString())
            {
                if (_ID == 0)
                {
                    objEuresysManager = new CEuresysManager(_CamInfo);
                    objEuresysManager.EuresysGrabEvent += new CEuresysManager.EuresysGrabHandler(ImageGrabEvent);
                }
            }

            else if(CameraType == eCameraType.EuresysIOTA.ToString())
            {
                if (_ID == 0)
                {
                    objEuresysIOTAManager = new CEuresysIOTAManager();
                    objEuresysIOTAManager.EuresysGrabEvent += new CEuresysIOTAManager.EuresysGrabHandler(ImageGrabEvent);
                }
            }

            else if (CameraType == eCameraType.BaslerGE.ToString())
            {
                objBaslerManager = new CBaslerManager();
                if (true == objBaslerManager.Initialize(_ID, _CamInfo))
                    objBaslerManager.BaslerGrabEvent += new CBaslerManager.BaslerGrabHandler(ImageGrabEvent);
                else
                    _Result = false;
            }

            return _Result;
        }

        public void DeInitialilze()
        {
            if (CameraType == eCameraType.Euresys.ToString())
            {
                objEuresysManager.EuresysGrabEvent -= new CEuresysManager.EuresysGrabHandler(ImageGrabEvent);
                objEuresysManager.DeInitialize();
            }

            else if (CameraType == eCameraType.EuresysIOTA.ToString())
            {
                objEuresysIOTAManager.EuresysGrabEvent -= new CEuresysIOTAManager.EuresysGrabHandler(ImageGrabEvent);
                objEuresysIOTAManager.DeInitialize();
            }

            else if (CameraType == eCameraType.BaslerGE.ToString())
            {
                objBaslerManager.BaslerGrabEvent -= new CBaslerManager.BaslerGrabHandler(ImageGrabEvent);
                objBaslerManager.DeInitialize();
            }
        }

        public void CamLive(bool _IsLive = false)
        {
            CamLiveFlag = !CamLiveFlag;
            if (CameraType == eCameraType.Euresys.ToString())           objEuresysManager.SetActive(_IsLive);
            else if (CameraType == eCameraType.EuresysIOTA.ToString())  objEuresysIOTAManager.SetActive(_IsLive);
            else if (CameraType == eCameraType.BaslerGE.ToString())     objBaslerManager.Continuous(_IsLive);
        }

        public void CameraGrab()
        {
            if (CameraType == eCameraType.BaslerGE.ToString()) objBaslerManager.OneShot();
        }
    }
}
