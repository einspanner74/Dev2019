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
    public partial class ucCogNeedleCircleFind : UserControl
    {
        private CogNeedleFindAlgo CogNeedleFindAlgoRcp = new CogNeedleFindAlgo();

        private double OriginX = 0;
        private double OriginY = 0;
        private double ResolutionX = 0.005;
        private double ResolutionY = 0.005;
        private double BenchMarkOffsetX = 0;
        private double BenchMarkOffsetY = 0;

        private bool AlgoInitFlag = false;

        public delegate void ApplyNeedleCircleFindValueHandler(CogNeedleFindAlgo _CogNeedleFindAlgo, ref CogNeedleFindResult _CogNeedleFindResult);
        public event ApplyNeedleCircleFindValueHandler ApplyNeedleCircleFindValueEvent;

        public delegate void DrawNeedleCircleFindCaliperHandler(CogNeedleFindAlgo _CogNeedleFindAlgo);
        public event DrawNeedleCircleFindCaliperHandler DrawNeedleCircleFindCaliperEvent;

        #region Initialize & DeInitialize
        public ucCogNeedleCircleFind()
        {
            InitializeComponent();
        }

        public void Initialize()
        {

        }

        private void InitializeControl()
        {

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

        private void btnDrawCaliper_Click(object sender, EventArgs e)
        {
            DrawCircleFindCaliper();
        }

        private void rbSearchDirection_MouseUp(object sender, MouseEventArgs e)
        {
            RadioButton _RadioDirection = (RadioButton)sender;
            int _Direction = Convert.ToInt32(_RadioDirection.Tag);
            SetSearchDirection(_Direction);
            graLabelSearchDirection.Text = _Direction.ToString();
            //ApplySettingValue();
            DrawCircleFindCaliper();
        }

        private void rbCaliperPolarityD_MouseUp(object sender, MouseEventArgs e)
        {
            RadioButton _RadioPolarity = (RadioButton)sender;
            int _Polarity = Convert.ToInt32(_RadioPolarity.Tag);
            SetPolarity(_Polarity);
            graLabelPolarity.Text = _Polarity.ToString();
            DrawCircleFindCaliper();
        }

        private void numUpDownCaliperNumber_ValueChanged(object sender, EventArgs e)
        {
            DrawCircleFindCaliper();
        }

        private void numUpDownSearchLength_ValueChanged(object sender, EventArgs e)
        {
            DrawCircleFindCaliper();
        }

        private void numUpDownProjectionLength_ValueChanged(object sender, EventArgs e)
        {
            DrawCircleFindCaliper();
        }

        private void numUpDownArcCenterX_ValueChanged(object sender, EventArgs e)
        {
            DrawCircleFindCaliper();
        }

        private void numUpDownArcCenterY_ValueChanged(object sender, EventArgs e)
        {
            DrawCircleFindCaliper();
        }

        private void numUpDownArcRadius_ValueChanged(object sender, EventArgs e)
        {
            DrawCircleFindCaliper();
        }

        private void numUpDownAngleStart_ValueChanged(object sender, EventArgs e)
        {
            DrawCircleFindCaliper();
        }

        private void numUpDownAngleSpan_ValueChanged(object sender, EventArgs e)
        {
            DrawCircleFindCaliper();
        }
        #endregion Control Event

        public void SetAlgoRecipe(Object _Algorithm, double _BenchMarkOffsetX, double _BenchMarkOffsetY, double _ResolutionX, double _ResolutionY)
        {
            if (_Algorithm != null)
            {
                AlgoInitFlag = false;

                CogNeedleFindAlgoRcp = _Algorithm as CogNeedleFindAlgo;

                ResolutionX = _ResolutionX;
                ResolutionY = _ResolutionY;
                BenchMarkOffsetX = _BenchMarkOffsetX;
                BenchMarkOffsetY = _BenchMarkOffsetY;
                numUpDownCaliperNumber.Value = Convert.ToDecimal(CogNeedleFindAlgoRcp.CaliperNumber);
                numUpDownSearchLength.Value = Convert.ToDecimal(CogNeedleFindAlgoRcp.CaliperSearchLength);
                numUpDownProjectionLength.Value = Convert.ToDecimal(CogNeedleFindAlgoRcp.CaliperProjectionLength);
                numUpDownIgnoreNumber.Value = Convert.ToDecimal(CogNeedleFindAlgoRcp.CaliperIgnoreNumber);
                numUpDownArcCenterX.Value = Convert.ToDecimal(CogNeedleFindAlgoRcp.ArcCenterX - BenchMarkOffsetX);
                numUpDownArcCenterY.Value = Convert.ToDecimal(CogNeedleFindAlgoRcp.ArcCenterY - BenchMarkOffsetY);
                numUpDownArcRadius.Value = Convert.ToDecimal(CogNeedleFindAlgoRcp.ArcRadius);
                numUpDownAngleStart.Value = Convert.ToDecimal(CogNeedleFindAlgoRcp.ArcAngleStart);
                numUpDownAngleSpan.Value = Convert.ToDecimal(CogNeedleFindAlgoRcp.ArcAngleSpan);
                textBoxCenterX.Text = (CogNeedleFindAlgoRcp.OriginX * ResolutionX).ToString("F3");
                textBoxCenterY.Text = (CogNeedleFindAlgoRcp.OriginY * ResolutionY).ToString("F3");
                textBoxRadius.Text = CogNeedleFindAlgoRcp.OriginRadius.ToString("F3");

                graLabelSearchDirection.Text = CogNeedleFindAlgoRcp.CaliperSearchDirection.ToString();
                graLabelPolarity.Text = CogNeedleFindAlgoRcp.CaliperPolarity.ToString();
                SetSearchDirection(CogNeedleFindAlgoRcp.CaliperSearchDirection);
                SetPolarity(CogNeedleFindAlgoRcp.CaliperPolarity);

                AlgoInitFlag = true;
            }

            else
            {
                //LOG
            }
        }

        public void SaveAlgoRecipe()
        {
            CogNeedleFindAlgoRcp.CaliperNumber           = Convert.ToInt32(numUpDownCaliperNumber.Value);
            CogNeedleFindAlgoRcp.CaliperSearchLength     = Convert.ToDouble(numUpDownSearchLength.Value);
            CogNeedleFindAlgoRcp.CaliperProjectionLength = Convert.ToDouble(numUpDownProjectionLength.Value);
            CogNeedleFindAlgoRcp.CaliperSearchDirection  = Convert.ToInt32(graLabelSearchDirection.Text);
            CogNeedleFindAlgoRcp.CaliperIgnoreNumber = Convert.ToInt32(numUpDownIgnoreNumber.Value);
            CogNeedleFindAlgoRcp.CaliperPolarity = Convert.ToInt32(graLabelPolarity.Text);
            CogNeedleFindAlgoRcp.ArcCenterX     = Convert.ToDouble(numUpDownArcCenterX.Value) + BenchMarkOffsetX;
            CogNeedleFindAlgoRcp.ArcCenterY     = Convert.ToDouble(numUpDownArcCenterY.Value) + BenchMarkOffsetY;
            CogNeedleFindAlgoRcp.ArcRadius      = Convert.ToDouble(numUpDownArcRadius.Value);
            CogNeedleFindAlgoRcp.ArcAngleStart  = Convert.ToDouble(numUpDownAngleStart.Value);
            CogNeedleFindAlgoRcp.ArcAngleSpan   = Convert.ToDouble(numUpDownAngleSpan.Value);
            //CogNeedleFindAlgoRcp.OriginX        = Convert.ToDouble(textBoxCenterX.Text) / ResolutionX;
            //CogNeedleFindAlgoRcp.OriginY        = Convert.ToDouble(textBoxCenterY.Text) / ResolutionY;
            CogNeedleFindAlgoRcp.OriginX        = OriginX;
            CogNeedleFindAlgoRcp.OriginY        = OriginY;
            CogNeedleFindAlgoRcp.OriginRadius   = Convert.ToDouble(textBoxRadius.Text);

            CLogManager.AddInspectionLog(CLogManager.LOG_TYPE.INFO, "Teaching NeedleCircleFind SaveAlgoRecipe", CLogManager.LOG_LEVEL.MID);
        }

        public void SetCaliper(int _CaliperNumber, double _SearchLength, double _ProjectionLength, eSearchDirection _eSearchDir, ePolarity _ePolarity)
        {
            numUpDownCaliperNumber.Value = Convert.ToDecimal(_CaliperNumber);
            numUpDownSearchLength.Value = Convert.ToDecimal(_SearchLength);
            numUpDownProjectionLength.Value = Convert.ToDecimal(_ProjectionLength);
            SetSearchDirection(Convert.ToInt32(_eSearchDir));
            SetPolarity(Convert.ToInt32(_ePolarity));
        }

        public void SetCircularArc(double _CenterX, double _CenterY, double _Radius, double _AngleStart, double _AngleSpan)
        {
            numUpDownArcCenterX.Value = Convert.ToDecimal(_CenterX);
            numUpDownArcCenterY.Value = Convert.ToDecimal(_CenterY);
            numUpDownArcRadius.Value = Convert.ToDecimal(_Radius);
            numUpDownAngleStart.Value = Convert.ToDecimal(_AngleStart);
            numUpDownAngleSpan.Value = Convert.ToDecimal(_AngleSpan);
        }

        private void SetSearchDirection(int _Direction)
        {
            rbSearchDirectionIn.Checked = false;
            rbSearchDirectionOut.Checked = false;

            switch ((eSearchDirection)_Direction)
            {
                case eSearchDirection.IN_WARD:  rbSearchDirectionIn.Checked = true;     break;
                case eSearchDirection.OUT_WARD: rbSearchDirectionOut.Checked = true;    break;
            }
        }

        private void SetPolarity(int _Polarity)
        {
            rbCaliperPolarityDarkToLight.Checked = false;
            rbCaliperPolarityLightToDark.Checked = false;

            switch ((ePolarity)_Polarity)
            {
                case ePolarity.DARK_TO_LIGHT: rbCaliperPolarityDarkToLight.Checked = true; break;
                case ePolarity.LIGHT_TO_DARK: rbCaliperPolarityLightToDark.Checked = true; break;
            }
        }
        
        private void ApplySettingValue()
        {
            CogNeedleFindResult _CogNeedleFindResult = new CogNeedleFindResult();
            CogNeedleFindAlgo _CogNeedleFindAlgoRcp = new CogNeedleFindAlgo();
            _CogNeedleFindAlgoRcp.CaliperNumber           = Convert.ToInt32(numUpDownCaliperNumber.Value);
            _CogNeedleFindAlgoRcp.CaliperSearchLength     = Convert.ToDouble(numUpDownSearchLength.Value);
            _CogNeedleFindAlgoRcp.CaliperProjectionLength = Convert.ToDouble(numUpDownProjectionLength.Value);
            _CogNeedleFindAlgoRcp.CaliperSearchDirection  = Convert.ToInt32(graLabelSearchDirection.Text);
            _CogNeedleFindAlgoRcp.CaliperIgnoreNumber = Convert.ToInt32(numUpDownIgnoreNumber.Value);
            _CogNeedleFindAlgoRcp.CaliperPolarity = Convert.ToInt32(graLabelPolarity.Text);
            _CogNeedleFindAlgoRcp.ArcCenterX     = Convert.ToDouble(numUpDownArcCenterX.Value);
            _CogNeedleFindAlgoRcp.ArcCenterY     = Convert.ToDouble(numUpDownArcCenterY.Value);
            _CogNeedleFindAlgoRcp.ArcRadius      = Convert.ToDouble(numUpDownArcRadius.Value);
            _CogNeedleFindAlgoRcp.ArcAngleStart  = Convert.ToDouble(numUpDownAngleStart.Value);
            _CogNeedleFindAlgoRcp.ArcAngleSpan   = Convert.ToDouble(numUpDownAngleSpan.Value);

            var _ApplyNeedleCircleFindValueEvent = ApplyNeedleCircleFindValueEvent;
            if (_ApplyNeedleCircleFindValueEvent != null)
                _ApplyNeedleCircleFindValueEvent(_CogNeedleFindAlgoRcp, ref _CogNeedleFindResult);

            if (_CogNeedleFindResult.IsGood)
            {
                textBoxCenterX.Text = (_CogNeedleFindResult.CenterXReal).ToString("F3");
                textBoxCenterY.Text = (_CogNeedleFindResult.CenterYReal).ToString("F3");
                textBoxRadius.Text = (_CogNeedleFindResult.RadiusReal).ToString("F3");

                OriginX = _CogNeedleFindResult.CenterX;
                OriginY = _CogNeedleFindResult.CenterY;
            }

            else
            {
                textBoxCenterX.Text = "-";
                textBoxCenterY.Text = "-";
                textBoxRadius.Text = "-";

                _CogNeedleFindAlgoRcp.OriginX = 0;
                _CogNeedleFindAlgoRcp.OriginY = 0;
            }
        }

        private void DrawCircleFindCaliper()
        {
            if (!AlgoInitFlag) return;

            CogNeedleFindAlgo _CogNeedleFindAlgoRcp = new CogNeedleFindAlgo();
            _CogNeedleFindAlgoRcp.CaliperNumber = Convert.ToInt32(numUpDownCaliperNumber.Value);
            _CogNeedleFindAlgoRcp.CaliperSearchLength = Convert.ToDouble(numUpDownSearchLength.Value);
            _CogNeedleFindAlgoRcp.CaliperProjectionLength = Convert.ToDouble(numUpDownProjectionLength.Value);
            _CogNeedleFindAlgoRcp.CaliperSearchDirection = Convert.ToInt32(graLabelSearchDirection.Text);
            _CogNeedleFindAlgoRcp.CaliperIgnoreNumber = Convert.ToInt32(numUpDownIgnoreNumber.Value);
            _CogNeedleFindAlgoRcp.CaliperPolarity = Convert.ToInt32(graLabelPolarity.Text);
            _CogNeedleFindAlgoRcp.ArcCenterX = Convert.ToDouble(numUpDownArcCenterX.Value);
            _CogNeedleFindAlgoRcp.ArcCenterY = Convert.ToDouble(numUpDownArcCenterY.Value);
            _CogNeedleFindAlgoRcp.ArcRadius = Convert.ToDouble(numUpDownArcRadius.Value);
            _CogNeedleFindAlgoRcp.ArcAngleStart = Convert.ToDouble(numUpDownAngleStart.Value);
            _CogNeedleFindAlgoRcp.ArcAngleSpan = Convert.ToDouble(numUpDownAngleSpan.Value);

            var _DrawNeedleCircleFindCaliperEvent = DrawNeedleCircleFindCaliperEvent;
            if (_DrawNeedleCircleFindCaliperEvent != null)
                _DrawNeedleCircleFindCaliperEvent(_CogNeedleFindAlgoRcp);
        }
    }
}
