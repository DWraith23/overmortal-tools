using Godot;

namespace OvermortalTools.Resources.Planner;

public partial class StageCalculatorData : Resource
{
    private int _currentMajorRealmIndex = 0;
    private int _currentMinorRealmIndex = 0;
    private int _currentStageIndex = 0;
    private float _currentPercent = 0.0f;
    private int _targetMajorRealmIndex = 0;
    private int _targetMinorRealmIndex = 0;
    private int _targetStageIndex = 0;

    [Export]
    public int CurrentMajorRealmIndex
    {
        get => _currentMajorRealmIndex;
        set
        {
            _currentMajorRealmIndex = value;
            EmitChanged();
        }
    }

    [Export]
    public int CurrentMinorRealmIndex
    {
        get => _currentMinorRealmIndex;
        set
        {
            _currentMinorRealmIndex = value;
            EmitChanged();
        }
    }

    [Export]
    public int CurrentStageIndex
    {
        get => _currentStageIndex;
        set
        {
            _currentStageIndex = value;
            EmitChanged();
        }
    }

    [Export]
    public float CurrentPercent
    {
        get => _currentPercent;
        set
        {
            _currentPercent = value;
            EmitChanged();
        }
    }

    [Export]
    public int TargetMajorRealmIndex
    {
        get => _targetMajorRealmIndex;
        set
        {
            _targetMajorRealmIndex = value;
            EmitChanged();
        }
    }

    [Export]
    public int TargetMinorRealmIndex
    {
        get => _targetMinorRealmIndex;
        set
        {
            _targetMinorRealmIndex = value;
            EmitChanged();
        }
    }

    [Export]
    public int TargetStageIndex
    {
        get => _targetStageIndex;
        set
        {
            _targetStageIndex = value;
            EmitChanged();
        }
    }

}

