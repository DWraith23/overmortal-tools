using Godot;
using OvermortalTools.Resources.Cultivation;
using OvermortalTools.Scripts.Enums;

namespace OvermortalTools.Resources.Planner;

public partial class StageCalculatorData : Resource
{

    private Realm.MajorRealm _currentMajorRealm;
    private Realm.MinorRealm _currentMinorRealm;
    private float _currentPercent;

    private Realm.MajorRealm _targetMajorRealm;
    private Realm.MinorRealm _targetMinorRealm;

    [Export]
    public Realm.MajorRealm CurrentMajorRealm
    {
        get => _currentMajorRealm;
        set
        {
            if (_currentMajorRealm == value) return;
            _currentMajorRealm = value;
            EmitChanged();
        }
    }

    [Export]
    public Realm.MinorRealm CurrentMinorRealm
    {
        get => _currentMinorRealm;
        set
        {
            if (_currentMinorRealm == value) return;
            _currentMinorRealm = value;
            EmitChanged();
        }
    }

    [Export]
    public float CurrentPercent
    {
        get => _currentPercent;
        set
        {
            if (_currentPercent == value) return;
            _currentPercent = value;
            EmitChanged();
        }
    }

    [Export]
    public Realm.MajorRealm TargetMajorRealm
    {
        get => _targetMajorRealm;
        set
        {
            if (_targetMajorRealm == value) return;
            _targetMajorRealm = value;
            EmitChanged();
        }
    }

    [Export]
    public Realm.MinorRealm TargetMinorRealm
    {
        get => _targetMinorRealm;
        set
        {
            if (_targetMinorRealm == value) return;
            _targetMinorRealm = value;
            EmitChanged();
        }
    }

    public Realm.RealmInfo CurrentRealm => new(CurrentMajorRealm, CurrentMinorRealm, CurrentPercent);
    public Realm.RealmInfo TargetRealm => new(TargetMajorRealm, TargetMinorRealm);

    public long CompletedXp => RealmList.SumCompletedXp(CurrentRealm);
    public long RemainingXp => RealmList.GetXpToTarget(CurrentRealm, TargetRealm);

}

