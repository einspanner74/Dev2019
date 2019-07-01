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

        private RectangleD       ChipOutArea = new RectangleD();
        private RectangleD       LeadMeasureArea = new RectangleD();
        private RectangleD       ShoulderInspArea = new RectangleD();
        private RectangleD       LeadTipInspArea = new RectangleD();
        private RectangleD       GateRemainingArea = new RectangleD();

        private double[]         LeadLengthArray;
        private double[]         LeadPitchArray;
        private double[]         LeadLengthArrayNew;
        private double[]         LeadPitchArrayNew;

        private double ResolutionX = 0.005;
        private double ResolutionY = 0.005;
        private double BenchMarkOffsetX = 0;
        private double BenchMarkOffsetY = 0;

        private double BodyCenterOriginX = 0;
        private double BodyCenterOriginY = 0;
        private double BodyCenterOffsetX = 0;
        private double BodyCenterOffsetY = 0;

        public delegate void ResetDisplayHandler();
        public event ResetDisplayHandler ResetDisplayEvent;

        public delegate CogRectangle GetRegionHandler();
        public event GetRegionHandler GetRegionEvent;

        public delegate void DrawRegionHandler(CogRectangle _Region, bool _IsStatic);
        public event DrawRegionHandler DrawRegionEvent;

        public delegate void DrawMaskingRegionHandler(List<RectangleD> _MaskingArea);
        public event DrawMaskingRegionHandler DrawMaskingRegionEven;

        public delegate void ApplyLeadTrimValueHandler(CogLeadTrimAlgo.eAlgoMode _Mode, CogRectangle _Region, CogLeadTrimAlgo _Algo, ref CogLeadTrimResult _Result);
        public event ApplyLeadTrimValueHandler ApplyLeadTrimValueEvent;

        #region Initialize & Deinitialize
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
            CogLeadTrimAlgoRcp.IsUseLeadBody = chUseLeadBody.Checked;
            CogLeadTrimAlgoRcp.BodyArea = new RectangleD();
            CogLeadTrimAlgoRcp.BodyArea.SetCenterWidthHeight(BodyArea.CenterX, BodyArea.CenterY, BodyArea.Width, BodyArea.Height);
            CogLeadTrimAlgoRcp.BodyCenterOrigin.X = BodyCenterOriginX;
            CogLeadTrimAlgoRcp.BodyCenterOrigin.Y = BodyCenterOriginY;
            CogLeadTrimAlgoRcp.BodyCenterOffset.X = BodyCenterOffsetX;
            CogLeadTrimAlgoRcp.BodyCenterOffset.Y = BodyCenterOffsetY;

            CogLeadTrimAlgoRcp.BodyMaskingAreaList.Clear();
            for (int iLoopCount = 0; iLoopCount < BodyMaskingAreaList.Count; ++iLoopCount)
            {
                RectangleD _MaskingArea = new RectangleD();
                _MaskingArea.SetCenterWidthHeight(BodyMaskingAreaList[iLoopCount].CenterX, BodyMaskingAreaList[iLoopCount].CenterY, BodyMaskingAreaList[iLoopCount].Width, BodyMaskingAreaList[iLoopCount].Height);
                CogLeadTrimAlgoRcp.BodyMaskingAreaList.Add(_MaskingArea);
            }


            CogLeadTrimAlgoRcp.IsUseMoldChipOut = chUseMoldChipOut.Checked;
            CogLeadTrimAlgoRcp.ChipOutArea.CenterX = ChipOutArea.CenterX;
            CogLeadTrimAlgoRcp.ChipOutArea.CenterY = ChipOutArea.CenterY;
            CogLeadTrimAlgoRcp.ChipOutArea.Width = ChipOutArea.Width;
            CogLeadTrimAlgoRcp.ChipOutArea.Height = ChipOutArea.Height;
            CogLeadTrimAlgoRcp.ChipOutForeground = 1;
            CogLeadTrimAlgoRcp.ChipOutThreshold = Convert.ToInt32(graLabelChipOutThresholdValue.Text);
            CogLeadTrimAlgoRcp.ChipOutSpec = Convert.ToDouble(numUpDownChipOutSpec.Value);
            CogLeadTrimAlgoRcp.ChipOutBlobAreaMin = Convert.ToDouble(textBoxChipOutBlobAreaMin.Text);
            CogLeadTrimAlgoRcp.ChipOutBlobAreaMax = Convert.ToDouble(textBoxChipOutBlobAreaMax.Text);
            CogLeadTrimAlgoRcp.ChipOutWidthMin = Convert.ToDouble(textBoxChipOutWidthSizeMin.Text);
            CogLeadTrimAlgoRcp.ChipOutWidthMax = Convert.ToDouble(textBoxChipOutWidthSizeMax.Text);
            CogLeadTrimAlgoRcp.ChipOutHeightMin = Convert.ToDouble(textBoxChipOutHeightSizeMin.Text);
            CogLeadTrimAlgoRcp.ChipOutHeightMax = Convert.ToDouble(textBoxChipOutHeightSizeMax.Text);


            CogLeadTrimAlgoRcp.LeadMeasurementArea.CenterX = LeadMeasureArea.CenterX;
            CogLeadTrimAlgoRcp.LeadMeasurementArea.CenterY = LeadMeasureArea.CenterY;
            CogLeadTrimAlgoRcp.LeadMeasurementArea.Width = LeadMeasureArea.Width;
            CogLeadTrimAlgoRcp.LeadMeasurementArea.Height = LeadMeasureArea.Height;


            CogLeadTrimAlgoRcp.LeadCount = Convert.ToInt32(numUpDownLeadCount.Value);
            CogLeadTrimAlgoRcp.LeadLengthSpec = Convert.ToDouble(numUpDownLeadLengthSpec.Value);
            CogLeadTrimAlgoRcp.LeadSkewSpec = Convert.ToDouble(numUpDownLeadSkewSpec.Value);
            CogLeadTrimAlgoRcp.LeadPitchSpec = Convert.ToDouble(numUpDownLeadPitchSpec.Value);

            //CogLeadTrimAlgoRcp.LeadLengthArray = new double[CogLeadTrimAlgoRcp.LeadCount];
            //CogLeadTrimAlgoRcp.LeadPitchArray = new double[CogLeadTrimAlgoRcp.LeadCount - 1];
            //CogLeadTrimAlgoRcp.LeadWidthArray = new double[CogLeadTrimAlgoRcp.LeadCount];
            //for (int iLoopCount = 0; iLoopCount < QuickGridViewLeadSetting.Rows.Count; ++iLoopCount)
            //{
            //    CogLeadTrimAlgoRcp.LeadLengthArray[iLoopCount] = Convert.ToDouble(QuickGridViewLeadSetting[1, iLoopCount].Value);
            //    //CogLeadTrimAlgoRcp.LeadWidthArray[iLoopCount] = Convert.ToDouble(QuickGridViewLeadSetting[3, iLoopCount].Value);
            //    if (iLoopCount > 0) CogLeadTrimAlgoRcp.LeadPitchArray[iLoopCount - 1] = Convert.ToDouble(QuickGridViewLeadSetting[2, iLoopCount - 1].Value);
            //}


            CogLeadTrimAlgoRcp.ShoulderInspArea.CenterX = ShoulderInspArea.CenterX;
            CogLeadTrimAlgoRcp.ShoulderInspArea.CenterY = ShoulderInspArea.CenterY;
            CogLeadTrimAlgoRcp.ShoulderInspArea.Width = ShoulderInspArea.Width;
            CogLeadTrimAlgoRcp.ShoulderInspArea.Height = ShoulderInspArea.Height;
            CogLeadTrimAlgoRcp.ShoulderBurrThreshold = Convert.ToInt32(graLabelBurrThresholdValue.Text);
            CogLeadTrimAlgoRcp.ShoulderNickThreshold = Convert.ToInt32(graLabelNickThresholdValue.Text);
            CogLeadTrimAlgoRcp.ShoulderBurrSpec = Convert.ToDouble(numUpDownShoulderBurrSpec.Value);
            CogLeadTrimAlgoRcp.ShoulderNickSpec = Convert.ToDouble(numUpDownShoulderNickSpec.Value);
            CogLeadTrimAlgoRcp.LeadEdgeWidth = Convert.ToInt32(numUpDownShoulderEdgeWidth.Value);


            CogLeadTrimAlgoRcp.LeadTipInspArea.CenterX = LeadTipInspArea.CenterX;
            CogLeadTrimAlgoRcp.LeadTipInspArea.CenterY = LeadTipInspArea.CenterY;
            CogLeadTrimAlgoRcp.LeadTipInspArea.Width = LeadTipInspArea.Width;
            CogLeadTrimAlgoRcp.LeadTipInspArea.Height = LeadTipInspArea.Height;
            CogLeadTrimAlgoRcp.LeadTipEdgeWidth = Convert.ToInt32(numUpDownLeadTipEdgeWidth.Value);
            CogLeadTrimAlgoRcp.LeadTipBurrThreshold = Convert.ToInt32(graLabelLeadTipBurrThresholdValue.Text);
            CogLeadTrimAlgoRcp.LeadTipBurrSpec = Convert.ToDouble(numUpDownLeadTipBurrSpec.Text);


            CogLeadTrimAlgoRcp.GateRemainingArea.CenterX = GateRemainingArea.CenterX;
            CogLeadTrimAlgoRcp.GateRemainingArea.CenterY = GateRemainingArea.CenterY;
            CogLeadTrimAlgoRcp.GateRemainingArea.Width = GateRemainingArea.Width;
            CogLeadTrimAlgoRcp.GateRemainingArea.Height = GateRemainingArea.Height;
            CogLeadTrimAlgoRcp.GateRemainingThreshold = Convert.ToInt32(gradientLabelGateRemainingThresholdValue.Text);
            CogLeadTrimAlgoRcp.GateRemainingSpec = Convert.ToDouble(numUpDownGateRemainingSpec.Value);
        }

        public void SetAlgoRecipe(Object _Algorithm, double _BenchMarkOffsetX, double _BenchMarkOffsetY, double _ResolutionX, double _ResolutionY)
        {
            if (null == _Algorithm) return;

            CogLeadTrimAlgoRcp = _Algorithm as CogLeadTrimAlgo;

            ResolutionX = _ResolutionX;
            ResolutionY = _ResolutionY;
            BenchMarkOffsetX = _BenchMarkOffsetX;
            BenchMarkOffsetY = _BenchMarkOffsetY;
            chUseLeadBody.Checked = CogLeadTrimAlgoRcp.IsUseLeadBody;

            //Lead Body Search Area Copy
            BodyArea.SetCenterWidthHeight(CogLeadTrimAlgoRcp.BodyArea.CenterX, CogLeadTrimAlgoRcp.BodyArea.CenterY, CogLeadTrimAlgoRcp.BodyArea.Width, CogLeadTrimAlgoRcp.BodyArea.Height);

            //Lead Body Masking Area Copy
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


            //Chip Out Area Copy
            ChipOutArea.SetCenterWidthHeight(CogLeadTrimAlgoRcp.ChipOutArea.CenterX, CogLeadTrimAlgoRcp.ChipOutArea.CenterY, CogLeadTrimAlgoRcp.ChipOutArea.Width, CogLeadTrimAlgoRcp.ChipOutArea.Height);
            graLabelChipOutThresholdValue.Text = CogLeadTrimAlgoRcp.ChipOutThreshold.ToString();
            hScrollBarChipOutThreshold.Value = CogLeadTrimAlgoRcp.ChipOutThreshold;
            numUpDownChipOutSpec.Value = Convert.ToDecimal(CogLeadTrimAlgoRcp.ChipOutSpec);
            textBoxChipOutBlobAreaMin.Text = CogLeadTrimAlgoRcp.ChipOutBlobAreaMin.ToString();
            textBoxChipOutBlobAreaMax.Text = CogLeadTrimAlgoRcp.ChipOutBlobAreaMax.ToString();
            textBoxChipOutWidthSizeMin.Text = CogLeadTrimAlgoRcp.ChipOutWidthMin.ToString();
            textBoxChipOutWidthSizeMax.Text = CogLeadTrimAlgoRcp.ChipOutWidthMax.ToString();
            textBoxChipOutHeightSizeMin.Text = CogLeadTrimAlgoRcp.ChipOutHeightMin.ToString();
            textBoxChipOutHeightSizeMax.Text = CogLeadTrimAlgoRcp.ChipOutHeightMax.ToString();


            //Lead Length / Bent Area Copy
            LeadMeasureArea.SetCenterWidthHeight(CogLeadTrimAlgoRcp.LeadMeasurementArea.CenterX, CogLeadTrimAlgoRcp.LeadMeasurementArea.CenterY, CogLeadTrimAlgoRcp.LeadMeasurementArea.Width, CogLeadTrimAlgoRcp.LeadMeasurementArea.Height);
            numUpDownLeadCount.Value = Convert.ToDecimal(CogLeadTrimAlgoRcp.LeadCount);
            numUpDownLeadLengthSpec.Value = Convert.ToDecimal(CogLeadTrimAlgoRcp.LeadLengthSpec);
            numUpDownLeadSkewSpec.Value = Convert.ToDecimal(CogLeadTrimAlgoRcp.LeadSkewSpec);
            numUpDownLeadPitchSpec.Value = Convert.ToDecimal(CogLeadTrimAlgoRcp.LeadPitchSpec);

            InitializeQuickGridView(CogLeadTrimAlgoRcp.LeadCount);
            for (int iLoopCount = 0; iLoopCount < CogLeadTrimAlgoRcp.LeadLengthArray.Length; ++iLoopCount)
            {
                QuickGridViewLeadSetting[1, iLoopCount].Value = CogLeadTrimAlgoRcp.LeadLengthArray[iLoopCount].ToString("F4");
                //QuickGridViewLeadSetting[3, iLoopCount].Value = CogLeadTrimAlgoRcp.LeadWidthArray[iLoopCount].ToString("F4");
                if (iLoopCount > 0) QuickGridViewLeadSetting[2, iLoopCount - 1].Value = CogLeadTrimAlgoRcp.LeadPitchArray[iLoopCount - 1].ToString("F4");

                if (iLoopCount % 2 == 0)
                {
                    QuickGridViewLeadSetting[1, iLoopCount].Style.BackColor = Color.DarkCyan;
                    QuickGridViewLeadSetting[2, iLoopCount].Style.BackColor = Color.DarkCyan;
                    //QuickGridViewLeadSetting[3, iLoopCount].Style.BackColor = Color.DarkCyan;
                }
                else
                {
                    QuickGridViewLeadSetting[1, iLoopCount].Style.BackColor = Color.CadetBlue;
                    QuickGridViewLeadSetting[2, iLoopCount].Style.BackColor = Color.CadetBlue;
                    //QuickGridViewLeadSetting[3, iLoopCount].Style.BackColor = Color.CadetBlue;
                }
            }

            LeadLengthArray = new double[CogLeadTrimAlgoRcp.LeadLengthArray.Length];
            LeadLengthArrayNew = new double[CogLeadTrimAlgoRcp.LeadLengthArray.Length];
            for (int iLoopCount = 0; iLoopCount < CogLeadTrimAlgoRcp.LeadLengthArray.Length; ++iLoopCount)
                LeadLengthArray[iLoopCount] = CogLeadTrimAlgoRcp.LeadLengthArray[iLoopCount];

            LeadPitchArray = new double[CogLeadTrimAlgoRcp.LeadPitchArray.Length];
            LeadPitchArrayNew = new double[CogLeadTrimAlgoRcp.LeadPitchArray.Length];
            for (int iLoopCount = 0; iLoopCount < CogLeadTrimAlgoRcp.LeadPitchArray.Length; ++iLoopCount)
                LeadPitchArray[iLoopCount] = CogLeadTrimAlgoRcp.LeadPitchArray[iLoopCount];


            //Shoulder Burr / Nick Inspection 
            ShoulderInspArea.SetCenterWidthHeight(CogLeadTrimAlgoRcp.ShoulderInspArea.CenterX, CogLeadTrimAlgoRcp.ShoulderInspArea.CenterY, CogLeadTrimAlgoRcp.ShoulderInspArea.Width, CogLeadTrimAlgoRcp.ShoulderInspArea.Height);
            graLabelBurrThresholdValue.Text = CogLeadTrimAlgoRcp.ShoulderBurrThreshold.ToString();
            graLabelNickThresholdValue.Text = CogLeadTrimAlgoRcp.ShoulderNickThreshold.ToString();
            hScrollShoulderBurrThreshold.Value = CogLeadTrimAlgoRcp.ShoulderBurrThreshold;
            hScrollShoulderNickThreshold.Value = CogLeadTrimAlgoRcp.ShoulderNickThreshold;
            numUpDownShoulderEdgeWidth.Value = Convert.ToDecimal(CogLeadTrimAlgoRcp.LeadEdgeWidth);
            numUpDownShoulderEdgeWidth.Value = Convert.ToDecimal(CogLeadTrimAlgoRcp.ShoulderBurrSpec);
            numUpDownShoulderNickSpec.Value = Convert.ToDecimal(CogLeadTrimAlgoRcp.ShoulderNickSpec);


            //Lead Tip Burr Inspection
            LeadTipInspArea.SetCenterWidthHeight(CogLeadTrimAlgoRcp.LeadTipInspArea.CenterX, CogLeadTrimAlgoRcp.LeadTipInspArea.CenterY, CogLeadTrimAlgoRcp.LeadTipInspArea.Width, CogLeadTrimAlgoRcp.LeadTipInspArea.Height);
            graLabelLeadTipBurrThresholdValue.Text = CogLeadTrimAlgoRcp.LeadTipBurrThreshold.ToString();
            hScrollLeadTipBurrThreshold.Value = CogLeadTrimAlgoRcp.LeadTipBurrThreshold;
            numUpDownLeadTipEdgeWidth.Value = Convert.ToInt32(CogLeadTrimAlgoRcp.LeadTipEdgeWidth);


            //Gate Remaining Inspection
            GateRemainingArea.SetCenterWidthHeight(CogLeadTrimAlgoRcp.GateRemainingArea.CenterX, CogLeadTrimAlgoRcp.GateRemainingArea.CenterY, CogLeadTrimAlgoRcp.GateRemainingArea.Width, CogLeadTrimAlgoRcp.GateRemainingArea.Height);
            gradientLabelGateRemainingThresholdValue.Text = CogLeadTrimAlgoRcp.GateRemainingThreshold.ToString();
            hScrollGateRemainingThreshold.Value = CogLeadTrimAlgoRcp.GateRemainingThreshold;
            numUpDownGateRemainingSpec.Value = Convert.ToDecimal(CogLeadTrimAlgoRcp.GateRemainingSpec);
        }
        #endregion

        #region Control Default Event
        private void tabControlLeadBody_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region TabControl Buttion initialize
            TabControl _TabControl = sender as TabControl;
            switch (_TabControl.SelectedTab.Text)
            {
                case " Lead Body":
                    btnLeadBodyAreaShow.Enabled = true;
                    btnLeadBodyAreaSet.Enabled = false;
                    btnMaskingShow.Enabled = true;
                    btnMaskingAdd.Enabled = false;
                    btnMaskingSet.Enabled = false;
                    btnMaskingUndo.Enabled = false;
                    btnMaskingRedo.Enabled = false;
                    btnMaskingClear.Enabled = false;
                    break;

                case " Mold ChipOut":
                    btnChipOutAreaShow.Enabled = true;
                    btnChipOutAreaSet.Enabled = false;
                    break;

                case " Bent/Length ":
                    btnLeadLengthAreaShow.Enabled = true;
                    btnLeadLengthAreaSet.Enabled = false;
                    break;

                case "Shoulder Burr/Nick":
                    btnShoulderAreaShow.Enabled = true;
                    btnShoulderAreaSet.Enabled = false;
                    break;

                case "Tip Burr":
                    btnLeadTipAreaShow.Enabled = true;
                    btnLeadTipAreaSet.Enabled = false;
                    break;

                case "Gate Remaining":
                    btnGateRemainingAreaShow.Enabled = true;
                    btnGateRemainingAreaSet.Enabled = false;
                    break;
            }
            #endregion

            var _ResetDisplayEvent = ResetDisplayEvent;
            _ResetDisplayEvent?.Invoke();
        }
        #endregion

        #region Lead Body Button Event
        private void btnLeadBodyAreaShow_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnLeadBodyAreaShow.Enabled = true;
            btnLeadBodyAreaSet.Enabled = true;

            btnMaskingShow.Enabled = true;
            btnMaskingAdd.Enabled = false;
            btnMaskingSet.Enabled = false;
            btnMaskingUndo.Enabled = false;
            btnMaskingRedo.Enabled = false;
            btnMaskingClear.Enabled = false;
            #endregion

            CogRectangle _Region = new CogRectangle();
            _Region.SetCenterWidthHeight(BodyArea.CenterX, BodyArea.CenterY, BodyArea.Width, BodyArea.Height);

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, false);
        }

        private void btnLeadBodyAreaSet_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnLeadBodyAreaShow.Enabled = true;
            btnLeadBodyAreaSet.Enabled = false;
            btnMaskingShow.Enabled = true;
            btnMaskingAdd.Enabled = false;
            btnMaskingSet.Enabled = false;
            btnMaskingUndo.Enabled = false;
            btnMaskingRedo.Enabled = false;
            btnMaskingClear.Enabled = false;
            #endregion

            var _GetRegionEvent = GetRegionEvent;
            CogRectangle _Region = GetRegionEvent?.Invoke();

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, true);

            BodyArea = new RectangleD();
            BodyArea.SetCenterWidthHeight(_Region.CenterX, _Region.CenterY, _Region.Width, _Region.Height);
        }

        private void btnMaskingShow_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnLeadBodyAreaShow.Enabled = true;
            btnLeadBodyAreaSet.Enabled = false;
            btnMaskingShow.Enabled = true;
            btnMaskingAdd.Enabled = true;
            btnMaskingSet.Enabled = true;
            btnMaskingUndo.Enabled = true;
            btnMaskingRedo.Enabled = true;
            btnMaskingClear.Enabled = true;
            #endregion

            var _DrawMaskingRegionEven = DrawMaskingRegionEven;
            _DrawMaskingRegionEven?.Invoke(BodyMaskingAreaList);
        }

        private void btnMaskingAdd_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnLeadBodyAreaShow.Enabled = true;
            btnLeadBodyAreaSet.Enabled = false;

            btnMaskingShow.Enabled = true;
            btnMaskingAdd.Enabled = true;
            btnMaskingSet.Enabled = true;
            btnMaskingUndo.Enabled = true;
            btnMaskingRedo.Enabled = true;
            btnMaskingClear.Enabled = true;
            #endregion

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
            

            CogRectangle _InspRegion = new CogRectangle();
            _InspRegion.SetCenterWidthHeight(BodyArea.CenterX, BodyArea.CenterY, BodyArea.Width, BodyArea.Height);

            CogLeadTrimResult _CogLeadTrimResult = new CogLeadTrimResult();
            CogLeadTrimAlgo _LeadTrimAlgoDest = new CogLeadTrimAlgo(ResolutionX, ResolutionY);
            _LeadTrimAlgoDest.BodyCenterOrigin.X = CogLeadTrimAlgoRcp.BodyCenterOrigin.X;
            _LeadTrimAlgoDest.BodyCenterOrigin.Y = CogLeadTrimAlgoRcp.BodyCenterOrigin.Y;

            _LeadTrimAlgoDest.BodyMaskingAreaList = new List<RectangleD>();
            //for (int iLoopCount = 0; iLoopCount < CogLeadTrimAlgoRcp.BodyMaskingAreaList.Count; ++iLoopCount)
            //{
            //    RectangleD _Area = new RectangleD();
            //    _Area.SetCenterWidthHeight(CogLeadTrimAlgoRcp.BodyMaskingAreaList[iLoopCount].CenterX, CogLeadTrimAlgoRcp.BodyMaskingAreaList[iLoopCount].CenterY, CogLeadTrimAlgoRcp.BodyMaskingAreaList[iLoopCount].Width, CogLeadTrimAlgoRcp.BodyMaskingAreaList[iLoopCount].Height);
            //    _LeadTrimAlgoDest.BodyMaskingAreaList.Add(_Area);
            //}
            for (int iLoopCount = 0; iLoopCount < CogLeadTrimAlgoRcp.BodyMaskingAreaList.Count; ++iLoopCount)
            {
                RectangleD _Area = new RectangleD();
                _Area.SetCenterWidthHeight(BodyMaskingAreaList[iLoopCount].CenterX, BodyMaskingAreaList[iLoopCount].CenterY, BodyMaskingAreaList[iLoopCount].Width, BodyMaskingAreaList[iLoopCount].Height);
                _LeadTrimAlgoDest.BodyMaskingAreaList.Add(_Area);
            }

            var _ApplyLeadTrimValueEvent = ApplyLeadTrimValueEvent;
            _ApplyLeadTrimValueEvent?.Invoke(CogLeadTrimAlgo.eAlgoMode.BODY_CHECK, _InspRegion, _LeadTrimAlgoDest, ref _CogLeadTrimResult);
            //_ApplyLeadTrimValueEvent?.Invoke(CogLeadTrimAlgo.eAlgoMode.BODY_CHECK, _InspRegion, CogLeadTrimAlgoRcp, ref _CogLeadTrimResult);

            BodyCenterOriginX = _CogLeadTrimResult.LeadBodyOriginX;
            BodyCenterOriginY = _CogLeadTrimResult.LeadBodyOriginY;
            BodyCenterOffsetX = _CogLeadTrimResult.LeadBodyOffsetX;
            BodyCenterOffsetY = _CogLeadTrimResult.LeadBodyOffsetY;
        }
        #endregion

        #region Chip Out Button Event
        private void hScrollBarChipOutThreshold_Scroll(object sender, ScrollEventArgs e)
        {
            graLabelChipOutThresholdValue.Text = hScrollBarChipOutThreshold.Value.ToString();
        }

        private void btnChipOutAreaShow_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnChipOutAreaShow.Enabled = true;
            btnChipOutAreaSet.Enabled = true;
            #endregion

            CogRectangle _Region = new CogRectangle();
            _Region.SetCenterWidthHeight(ChipOutArea.CenterX, ChipOutArea.CenterY, ChipOutArea.Width, ChipOutArea.Height);

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, false);
        }

        private void btnChipOutAreaSet_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnChipOutAreaShow.Enabled = true;
            btnChipOutAreaSet.Enabled = false;
            #endregion

            var _GetRegionEvent = GetRegionEvent;
            CogRectangle _Region = GetRegionEvent?.Invoke();

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, true);

            ChipOutArea = new RectangleD();
            ChipOutArea.SetCenterWidthHeight(_Region.CenterX, _Region.CenterY, _Region.Width, _Region.Height);
        }

        private void btnChipOutAreaCheck_Click(object sender, EventArgs e)
        {
            CogRectangle _InspRegion = new CogRectangle();
            _InspRegion.SetCenterWidthHeight(ChipOutArea.CenterX, ChipOutArea.CenterY, ChipOutArea.Width, ChipOutArea.Height);

            CogLeadTrimResult _CogLeadTrimResult = new CogLeadTrimResult();
            CogLeadTrimAlgo _LeadTrimAlgoDest = new CogLeadTrimAlgo(ResolutionX, ResolutionY);
            _LeadTrimAlgoDest.ChipOutThreshold = Convert.ToInt32(graLabelChipOutThresholdValue.Text);
            _LeadTrimAlgoDest.ChipOutBlobAreaMin = Convert.ToDouble(textBoxChipOutBlobAreaMin.Text);
            _LeadTrimAlgoDest.ChipOutBlobAreaMax = Convert.ToDouble(textBoxChipOutBlobAreaMax.Text);
            _LeadTrimAlgoDest.ChipOutWidthMin = Convert.ToDouble(textBoxChipOutWidthSizeMin.Text);
            _LeadTrimAlgoDest.ChipOutWidthMax = Convert.ToDouble(textBoxChipOutWidthSizeMax.Text);
            _LeadTrimAlgoDest.ChipOutHeightMin = Convert.ToDouble(textBoxChipOutHeightSizeMin.Text);
            _LeadTrimAlgoDest.ChipOutHeightMax = Convert.ToDouble(textBoxChipOutHeightSizeMax.Text);
            _LeadTrimAlgoDest.ChipOutSpec = Convert.ToDouble(numUpDownChipOutSpec.Value);

            var _ApplyLeadTrimValueEvent = ApplyLeadTrimValueEvent;
            _ApplyLeadTrimValueEvent?.Invoke(CogLeadTrimAlgo.eAlgoMode.CHIPOUT_CHECK, _InspRegion, _LeadTrimAlgoDest, ref _CogLeadTrimResult);
            //_ApplyLeadTrimValueEvent?.Invoke(CogLeadTrimAlgo.eAlgoMode.CHIPOUT_CHECK, _InspRegion, CogLeadTrimAlgoRcp, ref _CogLeadTrimResult);
        }
        #endregion

        #region Lead Bent Length Button Event
        private void btnLeadLengthAreaShow_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnLeadLengthAreaShow.Enabled = true;
            btnLeadLengthAreaSet.Enabled = true;
            #endregion

            CogRectangle _Region = new CogRectangle();
            _Region.SetCenterWidthHeight(LeadMeasureArea.CenterX, LeadMeasureArea.CenterY, LeadMeasureArea.Width, LeadMeasureArea.Height);

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, false);
        }

        private void btnLeadLengthAreaSet_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnLeadLengthAreaShow.Enabled = true;
            btnLeadLengthAreaSet.Enabled = false;
            #endregion

            var _GetRegionEvent = GetRegionEvent;
            CogRectangle _Region = GetRegionEvent?.Invoke();

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, true);

            LeadMeasureArea = new RectangleD();
            LeadMeasureArea.SetCenterWidthHeight(_Region.CenterX, _Region.CenterY, _Region.Width, _Region.Height);
        }

        private void btnLeadLengthAreaCheck_Click(object sender, EventArgs e)
        {
            CogRectangle _InspRegion = new CogRectangle();
            _InspRegion.SetCenterWidthHeight(LeadMeasureArea.CenterX, LeadMeasureArea.CenterY, LeadMeasureArea.Width, LeadMeasureArea.Height);

            CogLeadTrimResult _CogLeadTrimResult = new CogLeadTrimResult();

            var _ApplyLeadTrimValueEvent = ApplyLeadTrimValueEvent;
            _ApplyLeadTrimValueEvent?.Invoke(CogLeadTrimAlgo.eAlgoMode.LEAD_MEASURE, _InspRegion, CogLeadTrimAlgoRcp, ref _CogLeadTrimResult);

            //CogLeadTrimAlgoRcp.LeadCount = _CogLeadTrimResult.LeadCount;
            numUpDownLeadCount.Value = Convert.ToDecimal(_CogLeadTrimResult.LeadCount);


            SetLeadMeasurementValue(_CogLeadTrimResult);
            SetGridViewLeadMeasurementValue(LeadLengthArrayNew, LeadPitchArrayNew);
            //SetGridViewLeadMeasurementValue(_CogLeadTrimResult);
            
        }

        private void InitializeQuickGridView(int _RowCount)
        {
            QuickGridViewLeadSetting.Rows.Clear();
            for (int iLoopCount = 0; iLoopCount < _RowCount; ++iLoopCount)
            {
                DataGridViewRow _GridRow = new DataGridViewRow();
                DataGridViewCell[] _GridCell = new DataGridViewCell[3];
                _GridCell[0] = gridLeadNum.CellTemplate.Clone() as DataGridViewCell;
                _GridCell[1] = gridLeadLength.CellTemplate.Clone() as DataGridViewCell;
                _GridCell[2] = gridLeadPitch.CellTemplate.Clone() as DataGridViewCell;
                //_GridCell[3] = gridLeadWidth.CellTemplate.Clone() as DataGridViewCell;

                _GridCell[0].Value = (iLoopCount + 1);
                _GridCell[0].Style.BackColor = Color.DarkGreen;
                _GridCell[0].Style.ForeColor = Color.White;

                _GridRow.Height = 22;
                _GridRow.Cells.AddRange(_GridCell);
                QuickGridViewLeadSetting.Rows.Add(_GridRow);
            }
            QuickGridViewLeadSetting.ClearSelection();
        }

        private void SetGridViewLeadMeasurementValue(CogLeadTrimResult _Result)
        {
            if (_Result == null || _Result.LeadLength == null) return;
            InitializeQuickGridView(_Result.LeadLength.Length);

            for (int iLoopCount = 0; iLoopCount < _Result.LeadLength.Length; ++iLoopCount)
            {
                QuickGridViewLeadSetting[1, iLoopCount].Value = _Result.LeadLength[iLoopCount].ToString("F4");
                //QuickGridViewLeadSetting[3, iLoopCount].Value = _Result.LeadWidth[iLoopCount].ToString("F4");
                //if (iLoopCount > 0) QuickGridViewLeadSetting[2, iLoopCount].Value = _Result.LeadPitchLength[iLoopCount - 1].ToString("F4");
                if (iLoopCount < _Result.LeadLength.Length - 1) QuickGridViewLeadSetting[2, iLoopCount].Value = _Result.LeadPitchLength[iLoopCount].ToString("F4");

                if (iLoopCount % 2 == 0)
                {
                    QuickGridViewLeadSetting[1, iLoopCount].Style.BackColor = Color.DarkCyan;
                    QuickGridViewLeadSetting[2, iLoopCount].Style.BackColor = Color.DarkCyan;
                    //QuickGridViewLeadSetting[3, iLoopCount].Style.BackColor = Color.DarkCyan;
                }
                else
                {
                    QuickGridViewLeadSetting[1, iLoopCount].Style.BackColor = Color.CadetBlue;
                    QuickGridViewLeadSetting[2, iLoopCount].Style.BackColor = Color.CadetBlue;
                    //QuickGridViewLeadSetting[3, iLoopCount].Style.BackColor = Color.CadetBlue;
                }
            }
        }

        private void SetGridViewLeadMeasurementValue(double[] _LengthArray, double[] _PitchArray)
        {
            for  (int iLoopCount = 0; iLoopCount < _LengthArray.Length; ++iLoopCount)
            {
                QuickGridViewLeadSetting[1, iLoopCount].Value = _LengthArray[iLoopCount].ToString("F4");
                if (iLoopCount < _LengthArray.Length - 1) QuickGridViewLeadSetting[2, iLoopCount].Value = _PitchArray[iLoopCount].ToString("F4");

                if (iLoopCount % 2 == 0)
                {
                    QuickGridViewLeadSetting[1, iLoopCount].Style.BackColor = Color.DarkCyan;
                    QuickGridViewLeadSetting[2, iLoopCount].Style.BackColor = Color.DarkCyan;
                }
                else
                {
                    QuickGridViewLeadSetting[1, iLoopCount].Style.BackColor = Color.CadetBlue;
                    QuickGridViewLeadSetting[2, iLoopCount].Style.BackColor = Color.CadetBlue;
                }
            }
        }

        private void SetLeadMeasurementValue(CogLeadTrimResult _Result)
        {
            LeadLengthArrayNew = new double[_Result.LeadLength.Length];
            for (int iLoopCount = 0; iLoopCount < _Result.LeadLength.Length; ++iLoopCount)
                LeadLengthArrayNew[iLoopCount] = _Result.LeadLength[iLoopCount];

            LeadPitchArrayNew = new double[_Result.LeadPitchLength.Length];
            for (int iLoopCount = 0; iLoopCount < _Result.LeadPitchLength.Length; ++iLoopCount)
                LeadPitchArrayNew[iLoopCount] = _Result.LeadPitchLength[iLoopCount];
        }
        #endregion

        #region Shoulder Nick / Burr Button Event
        private void btnShoulderAreaShow_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnShoulderAreaShow.Enabled = true;
            btnShoulderAreaSet.Enabled = true;
            #endregion

            CogRectangle _Region = new CogRectangle();
            _Region.SetCenterWidthHeight(ShoulderInspArea.CenterX, ShoulderInspArea.CenterY, ShoulderInspArea.Width, ShoulderInspArea.Height);

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, false);
        }

        private void btnShoulderAreaSet_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnShoulderAreaShow.Enabled = true;
            btnShoulderAreaSet.Enabled = false;
            #endregion

            var _GetRegionEvent = GetRegionEvent;
            CogRectangle _Region = GetRegionEvent?.Invoke();

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, true);

            ShoulderInspArea = new RectangleD();
            ShoulderInspArea.SetCenterWidthHeight(_Region.CenterX, _Region.CenterY, _Region.Width, _Region.Height);
        }

        private void btnShoulderAreaCheck_Click(object sender, EventArgs e)
        {
            CogRectangle _InspRegion = new CogRectangle();
            _InspRegion.SetCenterWidthHeight(ShoulderInspArea.CenterX, ShoulderInspArea.CenterY, ShoulderInspArea.Width, ShoulderInspArea.Height);

            CogLeadTrimResult _CogLeadTrimResult = new CogLeadTrimResult();
            CogLeadTrimAlgo _ShoulderAlgoDest = new CogLeadTrimAlgo(ResolutionX, ResolutionY);
            _ShoulderAlgoDest.LeadCount = CogLeadTrimAlgoRcp.LeadCount;
            _ShoulderAlgoDest.ShoulderForeground = CogLeadTrimAlgoRcp.ShoulderForeground;
            //_ShoulderAlgoDest.ShoulderThreshold = CogLeadTrimAlgoRcp.ShoulderThreshold;
            _ShoulderAlgoDest.ShoulderThreshold = Convert.ToInt32(graLabelBurrThresholdValue.Text);
            _ShoulderAlgoDest.LeadEdgeWidth = Convert.ToInt32(numUpDownShoulderEdgeWidth.Value);
            _ShoulderAlgoDest.ShoulderBurrThreshold = Convert.ToInt32(graLabelBurrThresholdValue.Text);
            _ShoulderAlgoDest.ShoulderNickThreshold = Convert.ToInt32(graLabelNickThresholdValue.Text);
            _ShoulderAlgoDest.ShoulderBurrSpec = Convert.ToDouble(numUpDownShoulderBurrSpec.Value);
            _ShoulderAlgoDest.ShoulderNickSpec = Convert.ToDouble(numUpDownShoulderNickSpec.Value);

            var _ApplyLeadTrimValueEvent = ApplyLeadTrimValueEvent;
            _ApplyLeadTrimValueEvent?.Invoke(CogLeadTrimAlgo.eAlgoMode.SHOULDER_CHECK, _InspRegion, _ShoulderAlgoDest, ref _CogLeadTrimResult);
        }

        private void hScrollShoulderBurrThreshold_Scroll(object sender, ScrollEventArgs e)
        {
            graLabelBurrThresholdValue.Text = hScrollShoulderBurrThreshold.Value.ToString();
        }

        private void hScrollShoulderNickThreshold_Scroll(object sender, ScrollEventArgs e)
        {
            graLabelNickThresholdValue.Text = hScrollShoulderNickThreshold.Value.ToString();
        }
        #endregion

        #region Lead Tip Burr Button Event
        private void btnLeadTipAreaShow_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnLeadTipAreaShow.Enabled = true;
            btnLeadTipAreaSet.Enabled = true;
            #endregion 

            CogRectangle _Region = new CogRectangle();
            _Region.SetCenterWidthHeight(LeadTipInspArea.CenterX, LeadTipInspArea.CenterY, LeadTipInspArea.Width, LeadTipInspArea.Height);

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, false);
        }

        private void btnLeadTipAreaSet_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnLeadTipAreaShow.Enabled = true;
            btnLeadTipAreaSet.Enabled = false;
            #endregion 

            var _GetRegionEvent = GetRegionEvent;
            CogRectangle _Region = GetRegionEvent?.Invoke();

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, true);

            LeadTipInspArea = new RectangleD();
            LeadTipInspArea.SetCenterWidthHeight(_Region.CenterX, _Region.CenterY, _Region.Width, _Region.Height);
        }

        private void btnLeadTipAreaCheck_Click(object sender, EventArgs e)
        {
            CogRectangle _InspRegion = new CogRectangle();
            _InspRegion.SetCenterWidthHeight(LeadTipInspArea.CenterX, LeadTipInspArea.CenterY, LeadTipInspArea.Width, LeadTipInspArea.Height);
            
            CogLeadTrimResult _CogLeadTrimResult = new CogLeadTrimResult();
            CogLeadTrimAlgo _LeadTipAlgoDest = new CogLeadTrimAlgo(ResolutionX, ResolutionY);
            _LeadTipAlgoDest.LeadCount = CogLeadTrimAlgoRcp.LeadCount;
            _LeadTipAlgoDest.LeadTipEdgeWidth = Convert.ToInt32(numUpDownLeadTipEdgeWidth.Text);
            _LeadTipAlgoDest.LeadTipForeground = CogLeadTrimAlgoRcp.LeadTipForeground;
            //_LeadTipAlgoDest.LeadTipThreshold = CogLeadTrimAlgoRcp.LeadTipThreshold;
            _LeadTipAlgoDest.LeadTipThreshold = Convert.ToInt32(graLabelLeadTipBurrThresholdValue.Text);
            _LeadTipAlgoDest.LeadTipBurrThreshold = Convert.ToInt32(graLabelLeadTipBurrThresholdValue.Text);
            _LeadTipAlgoDest.LeadTipBurrSpec = Convert.ToDouble(numUpDownLeadTipBurrSpec.Value);
            
            var _ApplyLeadTrimValueEvent = ApplyLeadTrimValueEvent;
            _ApplyLeadTrimValueEvent?.Invoke(CogLeadTrimAlgo.eAlgoMode.LEADTIP_CHECK, _InspRegion, _LeadTipAlgoDest, ref _CogLeadTrimResult);
        }
        private void hScrollLeadTipBurrThreshold_Scroll(object sender, ScrollEventArgs e)
        {
            graLabelLeadTipBurrThresholdValue.Text = hScrollLeadTipBurrThreshold.Value.ToString();
        }
        #endregion

        #region Gate Remaining Button event
        private void btnGateRemainingAreaShow_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnGateRemainingAreaShow.Enabled = true;
            btnGateRemainingAreaSet.Enabled = true;
            #endregion

            CogRectangle _Region = new CogRectangle();
            _Region.SetCenterWidthHeight(GateRemainingArea.CenterX, GateRemainingArea.CenterY, GateRemainingArea.Width, GateRemainingArea.Height);

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, false);
        }

        private void btnGateRemainingAreaSet_Click(object sender, EventArgs e)
        {
            #region Button Status Set
            btnGateRemainingAreaShow.Enabled = true;
            btnGateRemainingAreaSet.Enabled = false;
            #endregion

            var _GetRegionEvent = GetRegionEvent;
            CogRectangle _Region = GetRegionEvent?.Invoke();

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, true);

            GateRemainingArea = new RectangleD();
            GateRemainingArea.SetCenterWidthHeight(_Region.CenterX, _Region.CenterY, _Region.Width, _Region.Height);
        }

        private void btnGateRemainingAreaCheck_Click(object sender, EventArgs e)
        {
            CogRectangle _InspRegion = new CogRectangle();
            _InspRegion.SetCenterWidthHeight(GateRemainingArea.CenterX, GateRemainingArea.CenterY, GateRemainingArea.Width, GateRemainingArea.Height);

            CogLeadTrimResult _CogLeadTrimResult = new CogLeadTrimResult();
            CogLeadTrimAlgo _LeadGateAlgoDest = new CogLeadTrimAlgo(ResolutionX, ResolutionY);
            _LeadGateAlgoDest.GateRemainingThreshold = Convert.ToInt32(gradientLabelGateRemainingThresholdValue.Text);
            _LeadGateAlgoDest.GateRemainingSpec = Convert.ToDouble(numUpDownGateRemainingSpec.Value);
            _LeadGateAlgoDest.GateRemainingForeground = CogLeadTrimAlgoRcp.GateRemainingForeground;

            var _ApplyLeadTrimValueEvent = ApplyLeadTrimValueEvent;
            _ApplyLeadTrimValueEvent?.Invoke(CogLeadTrimAlgo.eAlgoMode.GATE_REMAIN, _InspRegion, _LeadGateAlgoDest, ref _CogLeadTrimResult);
        }

        private void hScrollGateRemainingThreshold_Scroll(object sender, ScrollEventArgs e)
        {
            gradientLabelGateRemainingThresholdValue.Text = hScrollGateRemainingThreshold.Value.ToString();
        }
        #endregion

        private void btnNowLeadValueShow_Click(object sender, EventArgs e)
        {
            SetGridViewLeadMeasurementValue(LeadLengthArray, LeadPitchArray);
        }

        private void btnNewLeadValueShow_Click(object sender, EventArgs e)
        {
            SetGridViewLeadMeasurementValue(LeadLengthArrayNew, LeadPitchArrayNew);
        }

        private void btnNewLeadValueSave_Click(object sender, EventArgs e)
        {
            DialogResult dlgResult = MessageBox.Show(new Form { TopMost = true }, "현재 Lead Length/Bent 사이즈를 저장 하시겠습니까? ", "Exit Program", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
            if (DialogResult.Yes != dlgResult) { return; }

            CogLeadTrimAlgoRcp.LeadLengthArray = new double[CogLeadTrimAlgoRcp.LeadCount];
            CogLeadTrimAlgoRcp.LeadPitchArray = new double[CogLeadTrimAlgoRcp.LeadCount - 1];
            for (int iLoopCount = 0; iLoopCount < QuickGridViewLeadSetting.Rows.Count; ++iLoopCount)
            {
                CogLeadTrimAlgoRcp.LeadLengthArray[iLoopCount] = Convert.ToDouble(QuickGridViewLeadSetting[1, iLoopCount].Value);
                if (iLoopCount > 0)
                    CogLeadTrimAlgoRcp.LeadPitchArray[iLoopCount - 1] = Convert.ToDouble(QuickGridViewLeadSetting[2, iLoopCount - 1].Value);
            }
        }

        private void numUpDownLeadSkewSpec_ValueChanged(object sender, EventArgs e)
        {
            double _LeadSkewSpec = Convert.ToDouble(numUpDownLeadSkewSpec.Value);
            double _LeadPitchSpec = Convert.ToDouble(numUpDownLeadPitchSpec.Value);

            if (_LeadSkewSpec >= _LeadPitchSpec)
            {
                numUpDownLeadSkewSpec.Value = Convert.ToDecimal(_LeadPitchSpec - 0.001);
                return;
            }
        }
    }
}