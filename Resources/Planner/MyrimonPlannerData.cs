using Godot;

namespace OvermortalTools.Resources.Planner;

public partial class MyrimonPlannerData : Resource
{
    private int _expLevel = 0;
    private int _qualityLevel = 0;
    private int _gushLevel = 0;
    private int _highRankLevel = 0;
    private int _fruitTypeIndex = 0;
    private int _extractorQualityIndex = 0;
    private int _fruitQuantity = 0;
    private bool _isRealmMatch = false;


    [Export]
    public int ExpLevel
    {
        get => _expLevel;
        set
        {
            _expLevel = value;
            EmitChanged();
        }
    }

    [Export]
    public int QualityLevel
    {
        get => _qualityLevel;
        set
        {
            _qualityLevel = value;
            EmitChanged();
        }
    }

    [Export]
    public int GushLevel
    {
        get => _gushLevel;
        set
        {
            _gushLevel = value;
            EmitChanged();
        }
    }

    [Export]
    public int HighRankLevel
    {
        get => _highRankLevel;
        set
        {
            _highRankLevel = value;
            EmitChanged();
        }
    }

    [Export]
    public int FruitTypeIndex
    {
        get => _fruitTypeIndex;
        set
        {
            _fruitTypeIndex = value;
            EmitChanged();
        }
    }

    [Export]
    public int ExtractorQualityIndex
    {
        get => _extractorQualityIndex;
        set
        {
            _extractorQualityIndex = value;
            EmitChanged();
        }
    }

    [Export]
    public int FruitQuantity
    {
        get => _fruitQuantity;
        set
        {
            _fruitQuantity = value;
            EmitChanged();
        }
    }

    [Export]
    public bool IsRealmMatch
    {
        get => _isRealmMatch;
        set
        {
            _isRealmMatch = value;
            EmitChanged();
        }
    }
}
