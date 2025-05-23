using Godot;
using OvermortalTools.Scripts.Enums;

namespace OvermortalTools.Resources.Planner;

public partial class StageCalculatorData : Resource
{
    // UI Components
    private int _currentMajorRealmIndex = -1;
    private int _currentMinorRealmIndex = -1;
    private int _currentStageIndex = -1;
    private float _currentPercent = 0.0f;
    private int _targetMajorRealmIndex = -1;
    private int _targetMinorRealmIndex = -1;
    private int _targetStageIndex = -1;

    // Sub-Data
    private string _currentMajorRealm = "Novice";
    private string _currentMinorRealm = "---";
    private string _currentStage = "---";
    private string _targetMajorRealm = "Novice";
    private string _targetMinorRealm = "---";
    private string _targetStage = "---";

    [ExportGroup("UI Components")]
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

    [ExportGroup("Sub-Data")]
    [Export]
    public string CurrentMajorRealm
    {
        get => _currentMajorRealm;
        set
        {
            _currentMajorRealm = value;
        }
    }

    [Export]
    public string CurrentMinorRealm
    {
        get => _currentMinorRealm;
        set
        {
            _currentMinorRealm = value;
        }
    }

    [Export]
    public string CurrentStage
    {
        get => _currentStage;
        set
        {
            _currentStage = value;
        }
    }

    [Export]
    public string TargetMajorRealm
    {
        get => _targetMajorRealm;
        set
        {
            _targetMajorRealm = value;
        }
    }

    [Export]
    public string TargetMinorRealm
    {
        get => _targetMinorRealm;
        set
        {
            _targetMinorRealm = value;
        }
    }

    [Export]
    public string TargetStage
    {
        get => _targetStage;
        set
        {
            _targetStage = value;
        }
    }


    /// <summary>
    /// How much total XP has been accumulated on this Path, including all previous stages.
    /// </summary>
    public long CurrentXp =>
        CultivationStage.GetStage(
                CurrentMajorRealm,
                CurrentMinorRealm,
                CurrentStage
            ) == null
                ? 0
                : CultivationStage.GetStage(
                    CurrentMajorRealm,
                    CurrentMinorRealm,
                    CurrentStage
                ).GetTotalXp(CurrentPercent);

    /// <summary>
    /// Total XP required to complete the target stage.
    /// </summary>
    public long TargetXp =>
        CultivationStage.GetStage(
                TargetMajorRealm,
                TargetMinorRealm,
                TargetStage
            ) == null
                ? 0
                : CultivationStage.GetXpToComplete(CultivationStage.GetStage(
                    TargetMajorRealm,
                    TargetMinorRealm,
                    TargetStage
                ));

    /// <summary>
    /// The difference between TargetXp and CurrentXp.
    /// </summary>
    public long RemainingXpValue => TargetXp - CurrentXp;

    public Godot.Collections.Array<string> GetAllMajorRealms()
    {
        var result = new Godot.Collections.Array<string>();

        for (int i = CurrentMajorRealmIndex; i < Realm.NamesList.Count; i++)
        {
            result.Add(Realm.NamesList[i]);
            if (Realm.NamesList[i] == TargetMajorRealm)
            {
                break;  // Stop adding realms once we reach the target realm
            }
        }

        return result;
    }

    public Godot.Collections.Dictionary<string, long> GetAllMajorRealmsXp()
    {
        var result = new Godot.Collections.Dictionary<string, long>();

        foreach (var realm in GetAllMajorRealms())
        {
            result[realm] = CultivationStage.MajorRealmXp.TryGetValue(realm, out var xp) ? xp : 0;
            if (realm == CurrentMajorRealm)
            {
                result[realm] = CultivationStage.GetTotalXpToCompletion(realm) - CurrentXp;
            }
        }

        return result;
    }

}

