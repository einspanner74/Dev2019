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
        public bool         IsGood;
        public eNgType      NgType;
        public RectangleD   SearchArea;
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

    public class CogEllipseResult : Result
    {
        public double CenterX;
        public double CenterY;
        public double OriginX;
        public double OriginY;
        public double RadiusX;
        public double RadiusY;

        public double CenterXReal;
        public double CenterYReal;
        public double OriginXReal;
        public double OriginYReal;
        public double RadiusXReal;
        public double RadiusYReal;

        public double Rotation;

        public double DiameterMinAlgo;
        public double DiameterMaxAlgo;

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
        public double[] LeadWidth;
        public double[] LeadLength;
        public bool[]   IsLeadLengthGood;
        public double[] LeadPitchLength;
        public double[] LeadLengthStartX;
        public double[] LeadLengthStartY;
        public bool[]   IsLeadBentGood;

        //LengthResult 추가
        public bool[] LeadLengthResult;


        public List<CogRectangle> ShoulderBurrDefectList;
        public List<CogRectangle> ShoulderNickDefectList;
        public List<CogRectangle> LeadTipBurrDefectList;
        public List<CogRectangle> GateRemainingNgList;

        public EachLeadStatus[]   EachLeadStatusArray;

        public CogLeadTrimResult()
        {
            IsGood = true;
            NgType = eNgType.GOOD;
            SearchArea = new RectangleD();

            LeadBodyOriginX = 0;
            LeadBodyOriginY = 0;

            LeadBodyOffsetX = 0;
            LeadBodyOffsetY = 0;

            ChipOutNgList = new List<CogRectangle>();

            LeadBodyBaseLine = new CogLine();

            ShoulderBurrDefectList = new List<CogRectangle>();
            ShoulderNickDefectList = new List<CogRectangle>();
            LeadTipBurrDefectList = new List<CogRectangle>();

            GateRemainingNgList = new List<CogRectangle>();
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
        public CogRectangleAffine[] LeadTipCenterArea;
    }

    public class CogLeadTrimGateRemainingResult : CogLeadBlobResult
    {

    }

    public class CogLeadFormOriginResult : CogLeadBlobResult
    {

    }

    public class CogLeadFormAlignResult : CogLeadBlobResult
    {

    }

    public class CogLeadFormResult : Result
    {
        public string LastErrorMessge;

        //Lead Form Search Parameter
        public int LeadCount;

        public List<LeadFormAlignResultData> AlignResultDataList;
        public List<PointD>                  AlignOffsetDataList;

        public EachLeadStatus[]              EachLeadStatusArray;

        public CogLeadFormResult()
        {
            IsGood = true;
            NgType = eNgType.GOOD;
            SearchArea = new RectangleD();

            AlignResultDataList = new List<LeadFormAlignResultData>();
            AlignOffsetDataList = new List<PointD>();
        }
    }

    public class LeadFormAlignResultData
    {
        public double Area;
        public double CenterX;
        public double CenterY;
        public double Width;
        public double Height;
        public double Angle;
        public bool IsGood;
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
        public RectangleD SearchArea;
        public eNgType NgType;

        public object SendResult;

        //LDH, 2019.05.15, ProjectItem Measure용
        public object[] SendResultList;
        public eAlgoType[] AlgoTypeList;
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

        public CogImage8Grey SaveImage;
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

    public class SendMeasureResult
    {
        public int NGAreaNum;
        public bool IsGoodAlgo;

        public double[] CaliperPointX;
        public double[] CaliperPointY;

        //Ellipse
        public double RadiusX;
        public double RadiusY;
        public double DiameterMinAlgo;
        public double DiameterMaxAlgo;

        public double MeasureData;
    }

    public class SendLeadTrimResult
    {
        public int      LeadCount;

        public double[] LeadLength;
        public double[] LeadLengthReal;
        public bool[]   IsLeadLengthGood;

        public double[] LeadPitch;
        public double[] LeadPitchReal;
        public bool[]   IsLeadPitchGood;

        public EachLeadStatus[] EachLeadStatusArray;

        public CogImage8Grey SaveImage;
    }

    public class SendLeadFormResult
    {
        public int LeadCount;
        public PointD[] LeadOffset;
        public bool[] IsLeadOffsetGood;

        public EachLeadStatus[] EachLeadStatusArray;

        public CogImage8Grey SaveImage;
    }

    public class EachLeadStatus
    {
        public eLeadStatus NgType;

        public List<eLeadStatus> NgTypeList;

        public EachLeadStatus()
        {
            NgType = eLeadStatus.GOOD;
            NgTypeList = new List<eLeadStatus>();
        }

        public void ResetResult()
        {
            NgType = eLeadStatus.GOOD;
            NgTypeList.Clear();
        }

        public void SetSkewResult(eLeadStatus _NgType)
        {
            if (_NgType != eLeadStatus.GOOD) NgType = _NgType;
            NgTypeList.Add(_NgType);
        }
    }
    #endregion Last Send Result Parameter
}
