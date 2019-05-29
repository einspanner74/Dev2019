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

        public delegate CogRectangle GetRegionHandler();
        public event GetRegionHandler GetRegionEvent;

        public delegate void DrawRegionHandler(CogRectangle _Region, bool _IsStatic);
        public event DrawRegionHandler DrawRegionEvent;

        public delegate void ApplyLeadFormValueHandler(CogLeadFormAlgo.eAlgoMode _Mode, CogLeadFormAlgo _Algo, ref CogLeadFormResult _Result);

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
            CogLeadFormAlgoRcp.AlignPitchSpec = Convert.ToDouble(textBoxLeadFormAlignPitchSpec.Text);
        }

        public void SetAlgoRecipe(Object _Algorithm, double _ResolutionX, double _ResolutionY)
        {
            if (null == _Algorithm) return;

            CogLeadFormAlgoRcp = _Algorithm as CogLeadFormAlgo;

            ResolutionX = _ResolutionX;
            ResolutionY = _ResolutionY;

            chUseLeadFormOrigin.Checked = CogLeadFormAlgoRcp.IsUseOrigin;
            LeadFormOriginArea.SetCenterWidthHeight(CogLeadFormAlgoRcp.OriginArea.CenterX, CogLeadFormAlgoRcp.OriginArea.CenterY, CogLeadFormAlgoRcp.OriginArea.Width, CogLeadFormAlgoRcp.OriginArea.Height);

            chUseLeadFormAlign.Checked = CogLeadFormAlgoRcp.IsUseAlign;
            LeadFormAlignArea.SetCenterWidthHeight(CogLeadFormAlgoRcp.AlignArea.CenterX, CogLeadFormAlgoRcp.OriginArea.CenterY, CogLeadFormAlgoRcp.OriginArea.Width, CogLeadFormAlgoRcp.OriginArea.Height);

            hScrollBarLeadFormAlignThreshold.Value = CogLeadFormAlgoRcp.AlignThreshold;
            graLabelLeadFormAlignThresholdValue.Text = CogLeadFormAlgoRcp.AlignThreshold.ToString();
            textBoxLeadFormAlignPitchSpec.Text = CogLeadFormAlgoRcp.AlignPitchSpec.ToString();
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
        }
    }
}
