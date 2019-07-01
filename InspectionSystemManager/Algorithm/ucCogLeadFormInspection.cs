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

using Cognex.VisionPro;

namespace InspectionSystemManager
{
    public partial class ucCogLeadFormInspection : UserControl
    {
        private CogLeadFormAlgo CogLeadFormAlgoRcp = new CogLeadFormAlgo();

        private RectangleD      LeadFormOriginArea = new RectangleD();
        private RectangleD      LeadFormAlignArea = new RectangleD();

        private double ResolutionX = 0.005;
        private double ResolutionY = 0.005;

        private double[] LeadPositionArrayX;
        private double[] LeadPositionArrayY;

        private double[] LeadPositionArrayXNew;
        private double[] LeadPositionArrayYNew;

        public delegate CogRectangle GetRegionHandler();
        public event GetRegionHandler GetRegionEvent;

        public delegate void DrawRegionHandler(CogRectangle _Region, bool _IsStatic);
        public event DrawRegionHandler DrawRegionEvent;

        public delegate void ApplyLeadFormValueHandler(CogLeadFormAlgo.eAlgoMode _Mode, CogRectangle _Region, CogLeadFormAlgo _Algo, ref CogLeadFormResult _Result);
        public event ApplyLeadFormValueHandler ApplyLeadFormValueEvent;

        #region Initialize & Deinitialize
        public ucCogLeadFormInspection()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {

        }

        public void SaveAlgoRecipe()
        {
            CogLeadFormAlgoRcp.IsUseOrigin = chUseLeadFormOrigin.Checked;
            CogLeadFormAlgoRcp.OriginArea = new RectangleD();
            CogLeadFormAlgoRcp.OriginArea.SetCenterWidthHeight(LeadFormOriginArea.CenterX, LeadFormOriginArea.CenterY, LeadFormOriginArea.Width, LeadFormOriginArea.Height);


            CogLeadFormAlgoRcp.IsUseAlign = chUseLeadFormAlign.Checked;
            CogLeadFormAlgoRcp.AlignArea = new RectangleD();
            CogLeadFormAlgoRcp.AlignArea.SetCenterWidthHeight(LeadFormAlignArea.CenterX, LeadFormAlignArea.CenterY, LeadFormAlignArea.Width, LeadFormAlignArea.Height);
            CogLeadFormAlgoRcp.AlignThreshold = Convert.ToInt32(graLabelLeadFormAlignThresholdValue.Text);
            CogLeadFormAlgoRcp.AlignSkewSpec = Convert.ToDouble(numUpDownAlignSkewSpec.Value);
            CogLeadFormAlgoRcp.AlignPitchSpec = Convert.ToDouble(numUpDownLeadFormAlignPitchSpec.Value);

            CogLeadFormAlgoRcp.LeadCount = Convert.ToInt32(numUpDownLeadCount.Value);
            //CogLeadFormAlgoRcp.AlignPositionArray = new PointD[CogLeadFormAlgoRcp.LeadCount];
            //for (int iLoopCount = 0; iLoopCount < QuickGridViewLeadFormAlignPitch.Rows.Count; ++iLoopCount)
            //{
            //    CogLeadFormAlgoRcp.AlignPositionArray[iLoopCount] = new PointD();
            //    CogLeadFormAlgoRcp.AlignPositionArray[iLoopCount].X = Convert.ToDouble(QuickGridViewLeadFormAlignPitch[1, iLoopCount].Value);
            //    CogLeadFormAlgoRcp.AlignPositionArray[iLoopCount].Y = Convert.ToDouble(QuickGridViewLeadFormAlignPitch[2, iLoopCount].Value);
            //}
        }

        public void SetAlgoRecipe(Object _Algorithm, double _BenchMarkOffsetX, double _BenchMarkOffsetY, double _ResolutionX, double _ResolutionY)
        {
            if (null == _Algorithm) return;

            CogLeadFormAlgoRcp = _Algorithm as CogLeadFormAlgo;

            ResolutionX = _ResolutionX;
            ResolutionY = _ResolutionY;

            chUseLeadFormOrigin.Checked = CogLeadFormAlgoRcp.IsUseOrigin;
            LeadFormOriginArea.SetCenterWidthHeight(CogLeadFormAlgoRcp.OriginArea.CenterX, CogLeadFormAlgoRcp.OriginArea.CenterY, CogLeadFormAlgoRcp.OriginArea.Width, CogLeadFormAlgoRcp.OriginArea.Height);

            chUseLeadFormAlign.Checked = CogLeadFormAlgoRcp.IsUseAlign;
            LeadFormAlignArea.SetCenterWidthHeight(CogLeadFormAlgoRcp.AlignArea.CenterX, CogLeadFormAlgoRcp.AlignArea.CenterY, CogLeadFormAlgoRcp.AlignArea.Width, CogLeadFormAlgoRcp.AlignArea.Height);

            hScrollBarLeadFormAlignThreshold.Value = CogLeadFormAlgoRcp.AlignThreshold;
            graLabelLeadFormAlignThresholdValue.Text = CogLeadFormAlgoRcp.AlignThreshold.ToString();
            numUpDownAlignSkewSpec.Value = Convert.ToDecimal(CogLeadFormAlgoRcp.AlignSkewSpec);
            numUpDownLeadFormAlignPitchSpec.Value = Convert.ToDecimal(CogLeadFormAlgoRcp.AlignPitchSpec);

            InitializeQuickGridView(CogLeadFormAlgoRcp.LeadCount);
            numUpDownLeadCount.Value = Convert.ToDecimal(CogLeadFormAlgoRcp.LeadCount);
            for (int iLoopCount = 0; iLoopCount < CogLeadFormAlgoRcp.LeadCount; ++iLoopCount)
            {
                double _RealCenterX = CogLeadFormAlgoRcp.AlignPositionArray[iLoopCount].X;
                double _RealCenterY = CogLeadFormAlgoRcp.AlignPositionArray[iLoopCount].Y;

                QuickGridViewLeadFormAlignPitch[1, iLoopCount].Value = _RealCenterX.ToString("F4");
                QuickGridViewLeadFormAlignPitch[2, iLoopCount].Value = _RealCenterY.ToString("F4");

                if (iLoopCount % 2 == 0)
                {
                    QuickGridViewLeadFormAlignPitch[1, iLoopCount].Style.BackColor = Color.DarkCyan;
                    QuickGridViewLeadFormAlignPitch[2, iLoopCount].Style.BackColor = Color.DarkCyan;
                }
                else
                {
                    QuickGridViewLeadFormAlignPitch[1, iLoopCount].Style.BackColor = Color.CadetBlue;
                    QuickGridViewLeadFormAlignPitch[2, iLoopCount].Style.BackColor = Color.CadetBlue;
                }   
            }

            LeadPositionArrayX = new double[CogLeadFormAlgoRcp.LeadCount];
            LeadPositionArrayY = new double[CogLeadFormAlgoRcp.LeadCount];
            LeadPositionArrayXNew = new double[CogLeadFormAlgoRcp.LeadCount];
            LeadPositionArrayYNew = new double[CogLeadFormAlgoRcp.LeadCount];
            for (int iLoopCount =0; iLoopCount < CogLeadFormAlgoRcp.LeadCount; ++iLoopCount)
            {
                LeadPositionArrayX[iLoopCount] = CogLeadFormAlgoRcp.AlignPositionArray[iLoopCount].X;
                LeadPositionArrayY[iLoopCount] = CogLeadFormAlgoRcp.AlignPositionArray[iLoopCount].Y;
            }
        }
        #endregion

        #region Lead Form Origin Button Event
        private void btnLeadFormOriginAreaShow_Click(object sender, EventArgs e)
        {
            CogRectangle _Region = new CogRectangle();
            _Region.SetCenterWidthHeight(LeadFormOriginArea.CenterX, LeadFormOriginArea.CenterY, LeadFormOriginArea.Width, LeadFormOriginArea.Height);

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, false);
        }

        private void btnLeadFormOriginAreaSet_Click(object sender, EventArgs e)
        {
            var _GetRegionEvent = GetRegionEvent;
            CogRectangle _Region = GetRegionEvent?.Invoke();

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, true);

            LeadFormOriginArea = new RectangleD();
            LeadFormOriginArea.SetCenterWidthHeight(_Region.CenterX, _Region.CenterY, _Region.Width, _Region.Height);
        }

        private void btnLeadFormOriginAreaCheck_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void btnLeadFormAlignAreaShow_Click(object sender, EventArgs e)
        {
            CogRectangle _Region = new CogRectangle();
            _Region.SetCenterWidthHeight(LeadFormAlignArea.CenterX, LeadFormAlignArea.CenterY, LeadFormAlignArea.Width, LeadFormAlignArea.Height);

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, false);
        }

        private void btnLeadFormAlignAreaSet_Click(object sender, EventArgs e)
        {
            var _GetRegionEvent = GetRegionEvent;
            CogRectangle _Region = GetRegionEvent?.Invoke();

            var _DrawRegionEvent = DrawRegionEvent;
            _DrawRegionEvent?.Invoke(_Region, true);

            LeadFormAlignArea = new RectangleD();
            LeadFormAlignArea.SetCenterWidthHeight(_Region.CenterX, _Region.CenterY, _Region.Width, _Region.Height);
        }

        private void btnLeadFormAlignAreaCheck_Click(object sender, EventArgs e)
        {
            CogRectangle _InspRegion = new CogRectangle();
            _InspRegion.SetCenterWidthHeight(LeadFormAlignArea.CenterX, LeadFormAlignArea.CenterY, LeadFormAlignArea.Width, LeadFormAlignArea.Height);

            CogLeadFormResult _CogLeadFormResult = new CogLeadFormResult();
            CogLeadFormAlgo _CogLeadFormAlgo = new CogLeadFormAlgo(ResolutionX, ResolutionY);
            _CogLeadFormAlgo.AlignThreshold = Convert.ToInt32(graLabelLeadFormAlignThresholdValue.Text);
            _CogLeadFormAlgo.AlignSkewSpec = Convert.ToDouble(numUpDownAlignSkewSpec.Value);
            _CogLeadFormAlgo.AlignPitchSpec = Convert.ToDouble(numUpDownLeadFormAlignPitchSpec.Value);

            var _ApplyLeadFormValueEvent = ApplyLeadFormValueEvent;
            _ApplyLeadFormValueEvent?.Invoke(CogLeadFormAlgo.eAlgoMode.LEAD_ALIGN, _InspRegion, _CogLeadFormAlgo, ref _CogLeadFormResult);

            numUpDownLeadCount.Value = Convert.ToDecimal(_CogLeadFormResult.LeadCount);        

            SetLeadAlignmentValue(_CogLeadFormResult);
            SetGridViewLeadAlignmentValue(LeadPositionArrayXNew, LeadPositionArrayYNew);
            //SetGridViewLeadAlignmentValue(_CogLeadFormResult);
        }

        private void InitializeQuickGridView(int _RowCount)
        {
            QuickGridViewLeadFormAlignPitch.Rows.Clear();
            for (int iLoopCount = 0; iLoopCount < _RowCount; ++iLoopCount)
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
                QuickGridViewLeadFormAlignPitch.Rows.Add(_GridRow);
            }
            QuickGridViewLeadFormAlignPitch.ClearSelection();
        }

        private void SetGridViewLeadAlignmentValue(CogLeadFormResult _Result)
        {
            if (_Result == null || _Result.AlignResultDataList == null) return;
            InitializeQuickGridView(_Result.AlignResultDataList.Count);

            for (int iLoopCount = 0; iLoopCount < _Result.AlignResultDataList.Count; ++iLoopCount)
            {
                double _RealCenterX = _Result.AlignResultDataList[iLoopCount].CenterX * ResolutionX;
                double _RealCenterY = _Result.AlignResultDataList[iLoopCount].CenterY * ResolutionY;

                QuickGridViewLeadFormAlignPitch[1, iLoopCount].Value = _RealCenterX.ToString("F4");
                QuickGridViewLeadFormAlignPitch[2, iLoopCount].Value = _RealCenterY.ToString("F4");

                if (iLoopCount % 2 == 0)
                {
                    QuickGridViewLeadFormAlignPitch[1, iLoopCount].Style.BackColor = Color.DarkCyan;
                    QuickGridViewLeadFormAlignPitch[2, iLoopCount].Style.BackColor = Color.DarkCyan;
                }
                else
                {
                    QuickGridViewLeadFormAlignPitch[1, iLoopCount].Style.BackColor = Color.CadetBlue;
                    QuickGridViewLeadFormAlignPitch[2, iLoopCount].Style.BackColor = Color.CadetBlue;
                }
            }
        }

        private void SetGridViewLeadAlignmentValue(double[] _LeadPositionX, double[] _LeadPositionY)
        {
            for (int iLoopCount = 0; iLoopCount < _LeadPositionX.Length; ++iLoopCount)
            {
                QuickGridViewLeadFormAlignPitch[1, iLoopCount].Value = _LeadPositionX[iLoopCount].ToString("F4");
                QuickGridViewLeadFormAlignPitch[2, iLoopCount].Value = _LeadPositionY[iLoopCount].ToString("F4");

                if (iLoopCount % 2 == 0)
                {
                    QuickGridViewLeadFormAlignPitch[1, iLoopCount].Style.BackColor = Color.DarkCyan;
                    QuickGridViewLeadFormAlignPitch[2, iLoopCount].Style.BackColor = Color.DarkCyan;
                }
                else
                {
                    QuickGridViewLeadFormAlignPitch[1, iLoopCount].Style.BackColor = Color.CadetBlue;
                    QuickGridViewLeadFormAlignPitch[2, iLoopCount].Style.BackColor = Color.CadetBlue;
                }
            }
        }

        private void SetLeadAlignmentValue(CogLeadFormResult _Result)
        {
            for (int iLoopCount = 0; iLoopCount < _Result.AlignResultDataList.Count; ++iLoopCount)
            {
                LeadPositionArrayXNew[iLoopCount] = _Result.AlignResultDataList[iLoopCount].CenterX * ResolutionX;
                LeadPositionArrayYNew[iLoopCount] = _Result.AlignResultDataList[iLoopCount].CenterY * ResolutionY;
            }
        }

        private void hScrollBarLeadFormAlignThreshold_Scroll(object sender, ScrollEventArgs e)
        {
            graLabelLeadFormAlignThresholdValue.Text = hScrollBarLeadFormAlignThreshold.Value.ToString();
        }

        private void btnNowLeadPositionValueShow_Click(object sender, EventArgs e)
        {
            SetGridViewLeadAlignmentValue(LeadPositionArrayX, LeadPositionArrayY);
        }

        private void btnNewLeadPositionValueShow_Click(object sender, EventArgs e)
        {
            SetGridViewLeadAlignmentValue(LeadPositionArrayXNew, LeadPositionArrayYNew);
        }

        private void btnLeadPositionValueSave_Click(object sender, EventArgs e)
        {
            DialogResult dlgResult = MessageBox.Show(new Form { TopMost = true }, "현재 Lead Position을 저장 하시겠습니까? ", "Exit Program", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
            if (DialogResult.Yes != dlgResult) { return; }

            CogLeadFormAlgoRcp.AlignPositionArray = new PointD[CogLeadFormAlgoRcp.LeadCount];
            for (int iLoopCount = 0; iLoopCount < QuickGridViewLeadFormAlignPitch.Rows.Count; ++iLoopCount)
            {
                CogLeadFormAlgoRcp.AlignPositionArray[iLoopCount] = new PointD();
                CogLeadFormAlgoRcp.AlignPositionArray[iLoopCount].X = Convert.ToDouble(QuickGridViewLeadFormAlignPitch[1, iLoopCount].Value);
                CogLeadFormAlgoRcp.AlignPositionArray[iLoopCount].Y = Convert.ToDouble(QuickGridViewLeadFormAlignPitch[2, iLoopCount].Value);
            }
        }

        private void numUpDownAlignSkewSpec_ValueChanged(object sender, EventArgs e)
        {
            double _AlignSkewSpec = Convert.ToDouble(numUpDownAlignSkewSpec.Value);
            double _AlignPitchSpec = Convert.ToDouble(numUpDownLeadFormAlignPitchSpec.Value);

            if (_AlignSkewSpec >= _AlignPitchSpec)
            {
                numUpDownAlignSkewSpec.Value = Convert.ToDecimal(_AlignPitchSpec - 0.001);
                return;
            }
        }
    }
}
