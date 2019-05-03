using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cognex.VisionPro;

namespace ParameterManager
{
    #region AreaResultParameterList
    public class AreaResultParameter
    {
        public double OffsetX;
        public double OffsetY;
        public double OffsetT;

        public AreaResultParameter()
        {
            OffsetX = 0;
            OffsetY = 0;
            OffsetT = 0;
        }
    }

    public class AreaResultParameterList : List<AreaResultParameter>
    {

    }
    #endregion AreaResultParameterList

    #region AlgoResultParameterList
    public class AlgoResultParameter
    {
        public Object ResultParam;
        public eAlgoType ResultAlgoType;
        public double OffsetX;
        public double OffsetY;
        public double OffsetT;
        public int NgAreaNumber;

        public double TeachOriginX;
        public double TeachOriginY;

        public AlgoResultParameter()
        {
            OffsetX = 0;
            OffsetY = 0;
            ResultAlgoType = eAlgoType.C_NONE;
            ResultParam = null;

            TeachOriginX = 0;
            TeachOriginY = 0;
        }

        public AlgoResultParameter(eAlgoType _AlgoType, Object _ResultParam)
        {
            ResultParam = _ResultParam;
            ResultAlgoType = _AlgoType;

            OffsetX = 0;
            OffsetY = 0;
            TeachOriginX = 0;
            TeachOriginY = 0;
        }
    }

    public class AlgoResultParameterList : List<AlgoResultParameter>
    {

    }
    #endregion AlgoResultParameterList

    #region Inspection Result Parameter
    public class Result
    {
        public bool     IsGood;
        public eNgType  NgType;
    }

    public class CogPatternResult : Result
    {
        public int FindCount;

        public double[] Score;
        public double[] Scale;
        public double[] Angle;
        public double[] CenterX;
        public double[] CenterY;
        public double[] OriginPointX;
        public double[] OriginPointY;
        public double[] Width;
        public double[] Height;
        public bool[] IsGoods;

        public CogPatternResult()
        {
            IsGood = true;
            NgType = eNgType.GOOD;

            FindCount = 0;
        }
    }

    public class CogMultiPatternResult : Result
    {
        public int FindCount;

        public double[] Score;
        public double[] Scale;
        public double[] Angle;
        public double[] CenterX;
        public double[] CenterY;
        public double[] OriginPointX;
        public double[] OriginPointY;
        public double[] Width;
        public double[] Height;
        public double TwoPointAngle;

        public CogMultiPatternResult()
        {
            IsGood = true;
            NgType = eNgType.GOOD;

            FindCount = 0;

            TwoPointAngle = 0.0;
        }
    }

    public class CogAutoPatternResult : Result
    {
        public double Score;
        public double Scale;
        public double Angle;
        public double CenterX;
        public double CenterY;
        public double OriginPointX;
        public double OriginPointY;
        public double Width;
        public double Height;

        public CogAutoPatternResult()
        {
            IsGood = true;
            NgType = eNgType.GOOD;
        }
    }

    public class CogBlobReferenceResult : Result
    {
        public int      BlobCount;
        public double[] BlobMessCenterX;
        public double[] BlobMessCenterY;
        
        public double[] BlobCenterX;
        public double[] BlobCenterY;
        public double[] BlobMinX;
        public double[] BlobMinY;
        public double[] BlobMaxX;
        public double[] BlobMaxY;
        public double[] Width;
        public double[] Height;
        public double[] BlobRatio;
        public double[] Angle;
        public double[] BlobXMinYMax;
        public double[] BlobXMaxYMin;
        public double[] BlobArea;
        public double[] OriginX;
        public double[] OriginY;
        public bool[] IsGoods;
        public CogCompositeShape[] ResultGraphic;
        public double HistogramAvg;
        public bool DummyStatus;
    }

    public class CogNeedleFindResult : Result
    {
        public double CenterX;
        public double CenterY;
        public double OriginX;
        public double OriginY;
        public double Radius;

        public double CenterXReal;
        public double CenterYReal;
        public double OriginXReal;
        public double OriginYReal;
        public double RadiusReal;

        public int PointFoundCount;

        public double[] PointPosXInfo;
        public double[] PointPosYInfo;
        public bool[] PointStatusInfo;
    }

    public class CogLeadResult : Result
    {
        public int BlobCount;
        public double[] BlobArea;
        public double[] BlobCenterX;
        public double[] BlobCenterY;
        public double[] BlobMinX;
        public double[] BlobMinY;
        public double[] BlobMaxX;
        public double[] BlobMaxY;
        public double[] Width;
        public double[] Height;

        public double[] BlobMessCenterX;
        public double[] BlobMessCenterY;
        public double[] PrincipalWidth;
        public double[] PrincipalHeight;
        public double[] Angle;
        public double[] Degree;

        public CogCompositeShape[] ResultGraphic;

        public double OriginX;
        public double OriginY;

        //Lead 검사에 필요한 Parameter
        public int LeadCount;
        public bool IsLeadCountGood;
        public double LeadPitchAvg;
        public double LeadAngleAvg;
        public bool[] IsLeadBentGood;
        public double[] LeadPitchTopX;
        public double[] LeadPitchTopY;
        public double[] LeadPitchBottomX;
        public double[] LeadPitchBottomY;

        public double[] LeadLength;
        public double[] LeadLengthStartX;
        public double[] LeadLengthStartY;

        public CogLeadResult()
        {
            IsGood = true;
            IsLeadCountGood = true;
            NgType = eNgType.GOOD;
        }
    }

    public class CogBarCodeIDResult : Result
    {
        public int IDCount;
        public string[] IDResult;
        public double[] IDCenterX;
        public double[] IDCenterY;
        public double[] IDAngle;
        public CogPolygon[] IDPolygon;
    }

    public class CogLineFindResult : Result
    {
        public double StartX;
        public double StartY;
        public double EndX;
        public double EndY;
        public double Length;
        public double Rotation;
        public double LineRotation;
        public int PointCount;
        public bool[] PointStatus;
    }

    public class CogLeadTrimResult : Result
    {
        public string LastErrorMessage;

        //Lead Search Parameter
        public int  BlobCount;
        public double[] BlobArea;
        public double[] BlobCenterX;
        public double[] BlobCenterY;
        public double[] BlobMinX;
        public double[] BlobMinY;
        public double[] BlobMaxX;
        public double[] BlobMaxY;
        public double[] Width;
        public double[] Height;

        public double[] BlobMessCenterX;
        public double[] BlobMessCenterY;
        public double[] PrincipalWidth;
        public double[] PrincipalHeight;
        public double[] Angle;
        public double[] Degree;

        public CogCompositeShape[] ResultGraphic;

        public double OriginX;
        public double OriginY;

        //Lead 검사에 필요한 Parameter
        public int    LeadCount;

        public double LeadBodyOriginX;
        public double LeadBodyOriginY;

        public double LeadBodyOffsetX;
        public double LeadBodyOffsetY;

        public PointD LeadBodyLeftTop;
        public PointD LeadBodyRightTop;
        public PointD LeadBodyLeftBottom;
        public PointD LeadBodyRightBottom;
        public CogLine LeadBodyBaseLine;

        public List<CogRectangle> ChipOutNgList;

        public List<CogRectangle> LeadMeasureList;

        public double[] LeadAngle;
        public double[] LeadPitchTopX;
        public double[] LeadPitchTopY;
        public double[] LeadPitchBottomX;
        public double[] LeadPitchBottomY;
        public double[] LeadCenterX;
        public double[] LeadCenterY;
        public double[] LeadLength;
        public double[] LeadPitchLength;
        public double[] LeadWidth;
        public double[] LeadLengthStartX;
        public double[] LeadLengthStartY;
        public bool[] IsLeadBentGood;


        public List<CogRectangle> ShoulderBurrDefectList;

        public CogLeadTrimResult()
        {
            LeadBodyOriginX = 0;
            LeadBodyOriginY = 0;

            LeadBodyOffsetX = 0;
            LeadBodyOffsetY = 0;

            ChipOutNgList = new List<CogRectangle>();

            LeadBodyBaseLine = new CogLine();

            ShoulderBurrDefectList = new List<CogRectangle>();
        }
    }

    public class CogLeadTrimChipoutResult
    {

    }

    public class CogLeadTrimLeadResult
    {

    }

    public class CogLeadTrimShoulderResult : CogLeadBlobResult
    {
        //public int      BlobCount;
        //public double[] BlobArea;
        //public double[] BlobCenterX;
        //public double[] BlobCenterY;
        //public double[] BlobMinX;
        //public double[] BlobMinY;
        //public double[] BlobMaxX;
        //public double[] BlobMaxY;
        //public double[] Width;
        //public double[] Height;

        //public double[] BlobMessCenterX;
        //public double[] BlobMessCenterY;
        //public double[] PrincipalWidth;
        //public double[] PrincipalHeight;
        //public double[] Angle;
        //public double[] Degree;

        public double[] LeadEdgeLeft;
        public double[] LeadEdgeRight;
        public double[] LeadEdgeCenter;
        public double[] LeadEdgeWidth;

        public CogRectangleAffine[] LeadLeftArea;
        public CogRectangleAffine[] LeadRightArea;
        public CogRectangleAffine[] LeadCenterArea;
    }

    public class CogLeadTrimLeadTipResult : CogLeadBlobResult
    {
        public double[] LeadEdgeLeft;
        public double[] LeadEdgeRight;
        public double[] LeadEdgeCenter;
        public double[] LeadEdgeWidth;

        public CogRectangleAffine[] LeadTipLeftArea;
        public CogRectangleAffine[] LeadTipRightArea;
    }

    public class CogLeadBlobResult
    {
        public int      BlobCount;
        public double[] BlobArea;
        public double[] BlobCenterX;
        public double[] BlobCenterY;
        public double[] BlobMinX;
        public double[] BlobMinY;
        public double[] BlobMaxX;
        public double[] BlobMaxY;
        public double[] Width;
        public double[] Height;

        public double[] BlobMessCenterX;
        public double[] BlobMessCenterY;
        public double[] PrincipalWidth;
        public double[] PrincipalHeight;
        public double[] Angle;
        public double[] Degree;
    }
    #endregion Inspection Result Parameter

    #region Last Send Result Parameter
    public class SendResultParameter
    { 
        public eProjectItem ProjectItem;
        public eInspMode InspMode;
        public int ID;
        public bool IsGood;
        public eNgType NgType;

        public object SendResult;
    }

    public class SendNoneResult
    {
        public string ReadCode;
        public double MatchingScore;
    }

    public class SendIDResult
    {
        public string ReadCode;
    }

    public class SendNeedleAlignResult
    {
        public double AlignX;
        public double AlignY;
    }

    public class SendSurfaceResult
    {
        public double TwoPointAngle;
    }

    public class SendLeadResult
    {
        public int LeadCount;
        public bool IsLeadCountGood;

        public double BodyReferenceX;
        public double BodyReferenceY;
        public bool[] IsLeadBendGood;
        public double[] LeadAngle;
        public double[] LeadLength;
        public double[] LeadWidth;
        public double[] LeadPitch;
        public double[] LeadPitchTopX;
        public double[] LeadPitchTopY;

        public double[] LeadLengthReal;
        public double[] LeadWidthReal;
    }

    public class SendCardImageSaveResult
    {
        public CogImage8Grey SaveImage;
    }

    public class SendCardExistResult
    {
        public bool[] IsGoods;
        public CogImage8Grey SaveImage;
    }

    public class SendCardIDResult
    {
        public string ReadCode;
    }
    #endregion Last Send Result Parameter
}
