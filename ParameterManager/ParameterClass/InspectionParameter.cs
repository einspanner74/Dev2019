﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cognex.VisionPro.PMAlign;

namespace ParameterManager
{
    #region Reference class
    public class ReferenceInformation
    {
        //public CogPattern Reference;
        //public Object Reference;
        public CogPMAlignPattern Reference;
        public string ReferencePath;
        public double InterActiveStartX;
        public double InterActiveStartY;
        public double StaticStartX;
        public double StaticStartY;
        public double CenterX;
        public double CenterY;
        public double OriginPointOffsetX;
        public double OriginPointOffsetY;
        public double Width;
        public double Height;
    }

    public class References : List<ReferenceInformation>
    {

    }
    #endregion Reference class

    #region Cog Algorithm Class
    /// <summary>
    /// Pattern 매칭 알고리즘
    /// </summary>
    public class CogPatternAlgo
    {
        public References ReferenceInfoList;
        public int PatternCount;
        public double MatchingScore;
        public double MatchingAngle;
        public int MatchingCount;
        public bool IsShift;
        public double AllowedShiftX;
        public double AllowedShiftY;

        public CogPatternAlgo()
        {
            ReferenceInfoList = new References();
            ReferenceInfoList.Clear();

            MatchingScore = 75;
            MatchingAngle = 1.0;
            MatchingCount = 1;
            IsShift = false;
            AllowedShiftX = 1;
            AllowedShiftY = 1;
        }
    }

    /// <summary>
    /// Multi Pattern 매칭 알고리즘
    /// </summary>
    public class CogAutoPatternAlgo
    {
        public References ReferenceInfoList;
        public double MatchingScore;
        public int MatchingCount;
        public int PatternThreshold;

        public CogAutoPatternAlgo()
        {
            ReferenceInfoList = new References();

            MatchingScore = 75;
            MatchingCount = 1;
            PatternThreshold = 128;
        }
    }

    /// <summary>
    /// Multi Pattern 매칭 알고리즘
    /// </summary>
    public class CogMultiPatternAlgo
    {
        public References ReferenceInfoList;
        public int PatternCount;
        public double MatchingScore;
        public double MatchingAngle;
        public int MatchingCount;
        public double TwoPointAngle;

        public CogMultiPatternAlgo()
        {
            ReferenceInfoList = new References();

            MatchingScore = 75;
            MatchingAngle = 1.0;
            MatchingCount = 1;
            TwoPointAngle = 0.0;
        }
    }

    /// <summary>
    /// Blob Reference Algorithm
    /// </summary>
    public class CogBlobReferenceAlgo
    {
        //Condition Parameter
        public int ForeGround;
        public int ThresholdMin;
        public int ThresholdMax;
        public double BlobAreaMin;
        public double BlobAreaMax;

        public double Width;
        public double WidthPos;
        public double WidthNeg;
        public double Height;
        public double HeightPos;
        public double HeightNeg;

        public double OriginX;
        public double OriginY;

        public double BodyArea;
        public double BodyWidth;
        public double BodyHeight;
        public double BodyAreaPermitPercent;
        public double BodyWidthPermitPercent;
        public double BodyHeightPermitPercent;
        public bool UseBodyArea;
        public bool UseBodyWidth;
        public bool UseBodyHeight;

        public bool UseDummyValue;
        public double DummyHistoMeanValue;

        public int BenchMarkPosition;

        public double ResolutionX;
        public double ResolutionY;

        public CogBlobReferenceAlgo(double _ResolutionX = 1, double _ResolutionY = 1)
        {
            ForeGround = 1;
            ThresholdMin = 80;
            ThresholdMax = 200;
            BlobAreaMin = 1000;
            BlobAreaMax = 9000000;

            Width = 10;
            WidthPos = 0.5;
            WidthNeg = 0.5;
            Height = 5;
            HeightPos = 0.5;
            HeightNeg = 0.5;

            BodyAreaPermitPercent = 85;
            BodyWidthPermitPercent = 85;
            BodyHeightPermitPercent = 85;

            UseDummyValue = false;
            DummyHistoMeanValue = 5;

            BenchMarkPosition = 4;

            ResolutionX = _ResolutionX;
            ResolutionY = _ResolutionY;
        }
    }

    /// <summary>
    /// Blob 검사 알고리즘
    /// </summary>
    public class CogBlobAlgo
    {
        public int ForeGround;
        public int ThresholdMin;
        public int ThresholdMax;
        public double BlobAreaMin;
        public double BlobAreaMax;
        public double WidthMin;
        public double WidthMax;
        public double HeightMin;
        public double HeightMax;
        public double OriginX;
        public double OriginY;

        public int BenchMarkPosition;

        public CogBlobAlgo()
        {
            ForeGround = 0;
            ThresholdMin = 50;
            ThresholdMax = 200;
            BlobAreaMin = 5000;
            BlobAreaMax = 9000000;
            WidthMin = 5;
            WidthMax = 200;
            HeightMin = 5;
            HeightMax = 200;

            BenchMarkPosition = 0;
        }
    }

    /// <summary>
    /// Lead - Bent 검사 알고리즘
    /// </summary>
    public class CogLeadAlgo
    {
        public int LeadCount;
        public double LeadPitch;
        public string LeadUsable;

        //Condition Parameter
        public int ForeGround;
        public int ThresholdMin;
        public int ThresholdMax;
        public double BlobAreaMin;
        public double BlobAreaMax;
        public double WidthMin;
        public double WidthMax;
        public double HeightMin;
        public double HeightMax;
        public double OriginX;
        public double OriginY;
        public bool IsShowBoundary;

        //Lead Bent & Pitch 검사 Parameter
        public bool IsLeadPitchInspection;
        public double LeadPich;
        public double LeadPitchMin;
        public double LeadPitchMax;

        public bool IsLeadBentInspection;
        public double LeadBent;
        public double LeadBentMin;
        public double LeadBentMax;

        public CogLeadAlgo()
        {
            LeadCount = 26;
            LeadUsable = "11111111111111111111111111";

            ForeGround = 1;
            ThresholdMin = 80;
            ThresholdMax = 200;
            BlobAreaMin = 1000;
            BlobAreaMax = 9000000;
            WidthMin = 5;
            WidthMax = 200;
            HeightMin = 5;
            HeightMax = 200;
        }
    }

    public class CogNeedleFindAlgo
    {
        public int CaliperNumber;
        public double CaliperSearchLength;
        public double CaliperProjectionLength;
        public int CaliperSearchDirection;
        public int CaliperPolarity;
        public int CaliperIgnoreNumber;

        public double ArcCenterX;
        public double ArcCenterY;
        public double ArcRadius;
        public double ArcAngleStart;
        public double ArcAngleSpan;

        public double OriginX;
        public double OriginY;
        public double OriginRadius;

        public CogNeedleFindAlgo()
        {
            CaliperNumber = 15;
            CaliperSearchLength = 30;
            CaliperProjectionLength = 10;
            CaliperSearchDirection = 1;
            CaliperPolarity = 1;
            CaliperIgnoreNumber = 10;

            ArcCenterX = 150;
            ArcCenterY = 50;
            ArcRadius = 100;
            ArcAngleStart = 0;
            ArcAngleSpan = 360;

            OriginX = 0;
            OriginY = 0;
            OriginRadius = 0;
        }
    }

    public class CogEllipseAlgo
    {
        public int CaliperNumber;
        public double CaliperSearchLength;
        public double CaliperProjectionLength;
        public int CaliperSearchDirection;
        public int CaliperPolarity;
        public int CaliperIgnoreNumber;

        public double ArcCenterX;
        public double ArcCenterY;
        public double ArcRadiusX;
        public double ArcRadiusY;
        public double ArcAngleSpan;

        public double OriginX;
        public double OriginY;
        public double OriginRadiusX;
        public double OriginRadiusY;

        public double DiameterSize;
        public double DiameterMinus;
        public double DiameterPlus;

        public CogEllipseAlgo()
        {
            CaliperNumber = 8;
            CaliperSearchLength = 30;
            CaliperProjectionLength = 10;
            CaliperSearchDirection = 1;
            CaliperPolarity = 1;
            CaliperIgnoreNumber = 10;

            ArcCenterX = 150;
            ArcCenterY = 50;
            ArcRadiusX = 100;
            ArcRadiusY = 100;
            ArcAngleSpan = 360;

            OriginX = 0;
            OriginY = 0;
            OriginRadiusX = 0;
            OriginRadiusY = 0;

            DiameterSize = 5;
            DiameterMinus = 0.5;
            DiameterPlus = 0.5;
        }
    }

    /// <summary>
    /// Code Read 검사 알고리즘
    /// </summary>
    public class CogBarCodeIDAlgo
    {
        public string Symbology;
        public int TimeLimit;
        public int FindCount;
        public double OriginX;
        public double OriginY;

        public CogBarCodeIDAlgo()
        {
            Symbology = "DataMatrix";
            TimeLimit = 500;
            FindCount = 1;
            OriginX = 0;
            OriginY = 0;
        }
    }

    public class CogLineFindAlgo
    {
        public int CaliperNumber;
        public double CaliperSearchLength;
        public double CaliperProjectionLength;
        public int CaliperSearchDirection;
        public int IgnoreNumber;

        public double CaliperLineStartX;
        public double CaliperLineStartY;
        public double CaliperLineEndX;
        public double CaliperLineEndY;

        public int ContrastThreshold;
        public int FilterHalfSizePixels;

        public bool UseAlignment;

        public CogLineFindAlgo()
        {
            CaliperNumber = 6;
            CaliperSearchLength = 30;
            CaliperProjectionLength = 10;
            CaliperSearchDirection = 90;

            CaliperLineStartX = 50;
            CaliperLineStartY = 50;
            CaliperLineEndX = 500;
            CaliperLineEndY = 50;

            ContrastThreshold = 5;
            FilterHalfSizePixels = 2;

            UseAlignment = false;
        }
    }

    /// <summary>
    /// Lead Trim 검사 알고리즘 
    /// </summary>
    public class CogLeadTrimAlgo
    {
        public enum eAlgoMode   { BODY_CHECK = 0, CHIPOUT_CHECK, LEAD_MEASURE, SHOULDER_CHECK, LEADTIP_CHECK, GATE_REMAIN };


        public bool             IsUseLeadBody;
        public RectangleD       BodyArea;
        public CenterPoint      BodyCenterOrigin;
        public CenterPoint      BodyCenterOffset;

        public List<RectangleD> BodyMaskingAreaList;
        public double           BodyAngle;


        public bool             IsUseMoldChipOut;
        public RectangleD       ChipOutArea;
        public int              ChipOutForeground;
        public int              ChipOutThreshold;
        public double           ChipOutSpec;
        public double           ChipOutBlobAreaMin;
        public double           ChipOutBlobAreaMax;
        public double           ChipOutWidthMin;
        public double           ChipOutWidthMax;
        public double           ChipOutHeightMin;
        public double           ChipOutHeightMax;


        public bool             IsUseLeadMeasurement;
        public RectangleD       LeadMeasurementArea;
        public int              LeadCount;
        public int              LeadForeground;
        public int              LeadThreshold;
        public double           LeadSkewSpec;
        public double           LeadPitchSpec;
        public double           LeadLengthSpec;
        public double[]         LeadPitchArray;
        public double[]         LeadLengthArray;
        public double[]         LeadWidthArray;

        public bool             IsUseShoulderInspection;
        public RectangleD       ShoulderInspArea;
        public int              ShoulderForeground;
        public int              ShoulderThreshold;

        public int              LeadEdgeWidth;
        public int              ShoulderBurrThreshold;
        public int              ShoulderNickThreshold;
        public double           ShoulderBurrSpec;
        public double           ShoulderNickSpec;


        public bool             IsUseLeadTipInspection;
        public RectangleD       LeadTipInspArea;
        public int              LeadTipForeground;
        public int              LeadTipThreshold;
        public int              LeadTipEdgeWidth;
        public int              LeadTipBurrThreshold;
        public double           LeadTipBurrSpec;


        public bool             IsUseGateRemainingInspection;
        public RectangleD       GateRemainingArea;
        public int              GateRemainingForeground;
        public int              GateRemainingThreshold;
        public double           GateRemainingSpec;

        public double           ResolutionX;
        public double           ResolutionY;

        public CogLeadTrimAlgo(double _ResolutionX = 1, double _ResolutionY = 1)
        {
            IsUseLeadBody = true;
            BodyArea = new RectangleD();
            BodyMaskingAreaList = new List<RectangleD>();

            IsUseMoldChipOut = true;
            ChipOutArea = new RectangleD();
            ChipOutForeground = 1;
            ChipOutThreshold = 200;
            ChipOutSpec = 0.4;
            ChipOutBlobAreaMin = 1000;
            ChipOutBlobAreaMax = 9000000;
            ChipOutWidthMin = 5;
            ChipOutWidthMax = 2000;
            ChipOutHeightMin = 5;
            ChipOutHeightMax = 2000;

            IsUseLeadMeasurement = true;
            LeadMeasurementArea = new RectangleD();
            LeadCount = 10;
            LeadForeground = 0;
            LeadThreshold = 200;
            LeadPitchSpec = 0.5;
            LeadLengthSpec = 0.5;

            LeadLengthArray = new double[LeadCount];
            LeadPitchArray = new double[LeadCount - 1];
            LeadWidthArray = new double[LeadCount];

            IsUseShoulderInspection = true;
            ShoulderInspArea = new RectangleD();
            ShoulderForeground = 0;
            ShoulderThreshold = 180;

            LeadEdgeWidth = 36;
            ShoulderBurrThreshold = 200;
            ShoulderNickThreshold = 100;
            ShoulderBurrSpec = 0.075;
            ShoulderNickSpec = 0.075;


            IsUseLeadTipInspection = true;
            LeadTipInspArea = new RectangleD();
            LeadTipForeground = 0;
            LeadTipThreshold = 180;
            LeadTipEdgeWidth = 36;
            LeadTipBurrThreshold = 200;
            LeadTipBurrSpec = 0.075;


            IsUseGateRemainingInspection = true;
            GateRemainingArea = new RectangleD();
            GateRemainingForeground = 0;
            GateRemainingThreshold = 200;
            GateRemainingSpec = 0.2;

            ResolutionX = _ResolutionX;
            ResolutionY = _ResolutionY;
        }
    }

    public class CogLeadFormAlgo
    {
        public enum eAlgoMode   { LEAD_ORG = 0, LEAD_ALIGN };

        public int              LeadCount;

        public bool             IsUseOrigin;
        public RectangleD       OriginArea;
                                                                                                                                                 
        public bool             IsUseAlign;
        public RectangleD       AlignArea;
        public int              AlignThreshold;
        public double           AlignSkewSpec;
        public double           AlignPitchSpec;
        public PointD[]         AlignPositionArray;

        public double           ResolutionX;
        public double           ResolutionY;

        public CogLeadFormAlgo(double _ResolutionX = 1, double _ResolutionY = 1)
        {
            LeadCount = 20;

            IsUseOrigin = true;
            OriginArea = new RectangleD();

            IsUseAlign = true;
            AlignArea = new RectangleD();
            AlignThreshold = 200;
            AlignSkewSpec = 0.050;
            AlignPitchSpec = 0.100;
            AlignPositionArray = new PointD[LeadCount];

            ResolutionX = _ResolutionX;
            ResolutionY = _ResolutionY;
        }
    }
    #endregion Cog Algorithm Class

    public class InspectionAlgorithmParameter
    {
        public int AlgoType;
        public int AlgoBenchMark;
        public double AlgoRegionCenterX = 100;
        public double AlgoRegionCenterY = 100;
        public double AlgoRegionWidth = 100;
        public double AlgoRegionHeight = 100;
        public bool AlgoEnable;
        public Object Algorithm;

        public double BenchMarkOffsetX = 0;
        public double BenchMarkOffsetY = 0;

        public InspectionAlgorithmParameter()
        {

        }

        public InspectionAlgorithmParameter(eAlgoType _AlgoType, double _ResolutionX = 0, double _ResolutionY = 0)
        {
            AlgoType = (int)_AlgoType;
            AlgoBenchMark = 0;
            AlgoEnable = true;
            if (_AlgoType == eAlgoType.C_PATTERN)            Algorithm = new CogPatternAlgo();
            else if (_AlgoType == eAlgoType.C_BLOB)          Algorithm = new CogBlobAlgo();
            else if (_AlgoType == eAlgoType.C_BLOB_REFER)    Algorithm = new CogBlobReferenceAlgo(_ResolutionX, _ResolutionY);
            else if (_AlgoType == eAlgoType.C_LEAD)          Algorithm = new CogLeadAlgo();
            else if (_AlgoType == eAlgoType.C_NEEDLE_FIND)   Algorithm = new CogNeedleFindAlgo();
            else if (_AlgoType == eAlgoType.C_ELLIPSE)       Algorithm = new CogEllipseAlgo();
            else if (_AlgoType == eAlgoType.C_ID)            Algorithm = new CogBarCodeIDAlgo();
            else if (_AlgoType == eAlgoType.C_LINE_FIND)     Algorithm = new CogLineFindAlgo();
            else if (_AlgoType == eAlgoType.C_LEAD_TRIM)     Algorithm = new CogLeadTrimAlgo(_ResolutionX, _ResolutionY);
            else if (_AlgoType == eAlgoType.C_LEAD_FORM)     Algorithm = new CogLeadFormAlgo(_ResolutionX, _ResolutionY);
            else if (_AlgoType == eAlgoType.C_MULTI_PATTERN) Algorithm = new CogMultiPatternAlgo();
        }
    }

    public class InspectionAreaParameter
    {
        public List<InspectionAlgorithmParameter> InspAlgoParam;

        public int AreaBenchMark = 0;
        public double AreaRegionCenterX = 300;
        public double AreaRegionCenterY = 300;
        public double AreaRegionWidth = 200;
        public double AreaRegionHeight = 200;
        public bool Enable = true;
        public int NgAreaNumber = 1;
        public int BaseIndexNumber = -1;

        //MapData 관련 변수
        public bool IsUseMapData = false;
        public int MapDataUnitTotalCount = 1;
        public int MapDataStartNumber = 1;
        public int MapDataEndNumber = 1;

        public InspectionAreaParameter()
        {
            InspAlgoParam = new List<InspectionAlgorithmParameter>();
        }
    }

    public class InspectionParameter
    {
        public List<InspectionAreaParameter> InspAreaParam;
        //public MapDataParameter              MapDataParam;
        public double ResolutionX = 0.005;
        public double ResolutionY = 0.005;
        public double LastResultDisplayPosX = 50;
        public double LastResultDisplayPosY = 50;

        //LDH, 2019.06.11, Navien용 Result 사용 변수
        public string ResultUseFlag = "1";

        public InspectionParameter()
        {
            InspAreaParam = new List<InspectionAreaParameter>();
        }
    }
}
