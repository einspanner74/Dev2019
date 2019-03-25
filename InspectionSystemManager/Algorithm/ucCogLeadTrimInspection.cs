using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ParameterManager;
using LogMessageManager;
using KPDisplay;

using Cognex.VisionPro;

namespace InspectionSystemManager
{
    public partial class ucCogLeadTrimInspection : UserControl
    {
        private CogLeadTrimAlgo CogLeadTrimAlgoRcp = new CogLeadTrimAlgo();

        private RectangleD       BodyArea = new RectangleD();
        private List<RectangleD> BodyMaskingAreaList = new List<RectangleD>();
        private List<RectangleD> BodyMaskingAreaListBack = new List<RectangleD>();
        private RectangleD       BodyMaskingLastArea = new RectangleD();

        private double ResolutionX = 0.005;
        private double ResolutionY = 0.005;
        private double BenchMarkOffsetX = 0;
        private double BenchMarkOffsetY = 0;

        public delegate CogRectangle GetRegionHandler();
        public event GetRegionHandler GetRegionEvent;

        public delegate void DrawRegionHandler(CogRectangle _Region, bool _IsStatic);
        public event DrawRegionHandler DrawRegionEvent;

        public delegate void DrawMaskingRegionHandler(List<RectangleD> _MaskingArea);
        public event DrawMaskingRegionHandler DrawMaskingRegionEven;

        public ucCogLeadTrimInspection()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {

        }

        public void SaveAlgoRecipe()
        {
            CogLeadTrimAlgoRcp.BodyArea = new RectangleD();
            CogLeadTrimAlgoRcp.BodyArea.SetCenterWidthHeight(BodyArea.CenterX, BodyArea.CenterY, BodyArea.Width, BodyArea.Height);

            CogLeadTrimAlgoRcp.BodyMaskingAreaList.Clear();
            for (int iLoopCount = 0; iLoopCount < BodyMaskingAreaList.Count; ++iLoopCount)
            {
                RectangleD _MaskingArea = new RectangleD();
                _MaskingArea.SetCenterWidthHeight(BodyMaskingAreaList[iLoopCount].CenterX, BodyMaskingAreaList[iLoopCount].CenterY, BodyMaskingAreaList[iLoopCount].Width, BodyMaskingAreaList[iLoopCount].Height);
                CogLeadTrimAlgoRcp.BodyMaskingAreaList.Add(_MaskingArea);
            }
        }

        public void SetAlgoRecipe(Object _Algorithm, double _BenchMarkOffsetX, double _BenchMarkOffsetY, double _ResolutionX, double _ResolutionY)
        {
            if (null == _Algorithm) return;

            CogLeadTrimAlgoRcp = _Algorithm as CogLeadTrimAlgo;

            ResolutionX = _ResolutionX;
            ResolutionY = _ResolutionY;
            BenchMarkOffsetX = _BenchMarkOffsetX;
            BenchMarkOffsetY = _BenchMarkOffsetY;

            //Lead Body Search Area Copy
            BodyArea.SetCenterWidthHeight(CogLeadTrimAlgoRcp.BodyArea.CenterX, CogLeadTrimAlgoRcp.BodyArea.CenterY, CogLeadTrimAlgoRcp.BodyArea.Width, CogLeadTrimAlgoRcp.BodyArea.Height);

            //Masking Area Copy
            BodyMaskingAreaList.Clear();
            BodyMaskingAreaListBack.Clear();
            for (int iLoopCount = 0; iLoopCount < CogLeadTrimAlgoRcp.BodyMaskingAreaList.Count; ++iLoopCount)
            {
                RectangleD _MaskingArea = new RectangleD();
                RectangleD _MaskingAreaBack = new RectangleD();
                _MaskingArea.SetCenterWidthHeight(CogLeadTrimAlgoRcp.BodyMaskingAreaList[iLoopCount].CenterX, CogLeadTrimAlgoRcp.BodyMaskingAreaList[iLoopCount].CenterY,
                                                  CogLeadTrimAlgoRcp.BodyMaskingAreaList[iLoopCount].Width, CogLeadTrimAlgoRcp.BodyMaskingAreaList[iLoopCount].Height);
                _MaskingAreaBack.SetCenterWidthHeight(CogLeadTrimAlgoRcp.BodyMaskingAreaList[iLoopCount].CenterX, CogLeadTrimAlgoRcp.BodyMaskingAreaList[iLoopCount].CenterY,
                                                      CogLeadTrimAlgoRcp.BodyMaskingAreaList[iLoopCount].Width, CogLeadTrimAlgoRcp.BodyMaskingAreaList[iLoopCount].Height);
                BodyMaskingAreaList.Add(_MaskingArea);
                BodyMaskingAreaListBack.Add(_MaskingAreaBack);
            }
        }

        private void btnLeadBodyAreaShow_Click(object sender, EventArgs e)
        {
            CogRectangle _Region = new CogRectangle();
            _Region.SetCenterWidthHeight(BodyArea.CenterX, BodyArea.CenterY, BodyArea.Width, BodyArea.Height);

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, false);
        }

        private void btnLeadBodyAreaSet_Click(object sender, EventArgs e)
        {
            var _GetRegionEvent = GetRegionEvent;
            CogRectangle _Region = GetRegionEvent?.Invoke();

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, true);

            BodyArea = new RectangleD();
            BodyArea.SetCenterWidthHeight(_Region.CenterX, _Region.CenterY, _Region.Width, _Region.Height);
        }

        private void btnMaskingShow_Click(object sender, EventArgs e)
        {
            var _DrawMaskingRegionEven = DrawMaskingRegionEven;
            _DrawMaskingRegionEven?.Invoke(BodyMaskingAreaList);
        }

        private void btnMaskingAdd_Click(object sender, EventArgs e)
        {
            CogRectangle _Region = new CogRectangle();
            _Region.SetCenterWidthHeight(500, 500, 500, 500);

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, false);
        }

        private void btnMaskingSet_Click(object sender, EventArgs e)
        {
            var _GetRegionEvent = GetRegionEvent;
            CogRectangle _Region = GetRegionEvent?.Invoke();

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, true);

            RectangleD _MaskingArea = new RectangleD();
            _MaskingArea.SetCenterWidthHeight(_Region.CenterX, _Region.CenterY, _Region.Width, _Region.Height);
            BodyMaskingAreaList.Add(_MaskingArea);

            var _DrawMaskingRegionEven = DrawMaskingRegionEven;
            _DrawMaskingRegionEven?.Invoke(BodyMaskingAreaList);
        }

        private void btnMaskingUndo_Click(object sender, EventArgs e)
        {
            if (BodyMaskingAreaList.Count == 0) return;
            BodyMaskingLastArea = new RectangleD();
            BodyMaskingLastArea.CenterX = BodyMaskingAreaList[BodyMaskingAreaList.Count - 1].CenterX;
            BodyMaskingLastArea.CenterY = BodyMaskingAreaList[BodyMaskingAreaList.Count - 1].CenterY;
            BodyMaskingLastArea.Width = BodyMaskingAreaList[BodyMaskingAreaList.Count - 1].Width;
            BodyMaskingLastArea.Height = BodyMaskingAreaList[BodyMaskingAreaList.Count - 1].Height;

            BodyMaskingAreaList.RemoveAt(BodyMaskingAreaList.Count - 1);

            var _DrawMaskingRegionEven = DrawMaskingRegionEven;
            _DrawMaskingRegionEven?.Invoke(BodyMaskingAreaList);
        }

        private void btnMaskingRedo_Click(object sender, EventArgs e)
        {
            if (BodyMaskingLastArea == null) return;

            RectangleD _MaskingArea = new RectangleD();
            _MaskingArea.SetCenterWidthHeight(BodyMaskingLastArea.CenterX, BodyMaskingLastArea.CenterY, BodyMaskingLastArea.Width, BodyMaskingLastArea.Height);
            BodyMaskingAreaList.Add(_MaskingArea);
            BodyMaskingLastArea = null;

            var _DrawMaskingRegionEven = DrawMaskingRegionEven;
            _DrawMaskingRegionEven?.Invoke(BodyMaskingAreaList);

            
        }

        private void btnMaskingClear_Click(object sender, EventArgs e)
        {
            BodyMaskingAreaList.Clear();
            var _DrawMaskingRegionEven = DrawMaskingRegionEven;
            _DrawMaskingRegionEven?.Invoke(BodyMaskingAreaList);
        }

        private void btnBodyAreaCheck_Click(object sender, EventArgs e)
        {

        }
    }
}
