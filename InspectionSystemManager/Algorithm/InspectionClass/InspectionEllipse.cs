using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;

using ParameterManager;
using LogMessageManager;

namespace InspectionSystemManager
{
    class InspectionEllipse
    {
        public InspectionEllipse()
        {

        }

        public bool Run(CogImage8Grey _SrcImage, CogRectangle _InspRegion, CogEllipseAlgo _CogEllipseAlgo, ref CogEllipseResult _CogEllipseResult, int _NgNumber = 0)
        {
            bool _Result = true;
            return _Result;
        }
    }
}
