using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParameterManager
{
    public class AlignProjectParameter
    {
        public double OriginX;
        public double OriginY;
        public double OriginRadius;

        public AlignProjectParameter()
        {
            OriginX = 0;
            OriginY = 0;
        }
    }

    public class LeadProjectParameter
    {
        public int LeadCount;

        public LeadProjectParameter()
        {
            LeadCount = 0;
        }
    }
}
