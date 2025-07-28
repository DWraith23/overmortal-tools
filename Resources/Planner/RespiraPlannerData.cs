using System;
using System.Collections.Generic;
using Godot;
using OvermortalTools.Scripts.Enums;

namespace OvermortalTools.Resources.Planner;

public partial class RespiraPlannerData : Resource
{
    private static Dictionary<Realm.Classification, int> RealmValues => new()
    {
        { Realm.Classification.Novice, 6 },
        { Realm.Classification.Connection, 20 },
        { Realm.Classification.Foundation, 110 },
        { Realm.Classification.Virtuoso, 650 },
        { Realm.Classification.NascentSoul, 3200 },
        { Realm.Classification.Incarnation, 5300 },
        { Realm.Classification.Voidbreak, 7800 },
        { Realm.Classification.Wholeness, 10500 },
        { Realm.Classification.Perfection, 13500 },
        { Realm.Classification.Nirvana, 25000 },
        { Realm.Classification.Celestial, 37500 },
    };

    private static List<(float, int)> MultiplierOdds =>
    [
        (0.55f, 1),
        (0.30f, 2),
        (0.1475f, 5),
        (0.0025f, 10),
    ];

    private int _realmIndex = 0;
    private int _respiraAttemptsFromTechniques = 0;
    private float _respiraBonusFromTechniques = 0.0f;
    private int _respiraAttempsFromFriends = 0;
    private float _respiraBonusFromFriends = 0.0f;
    private int _respiraAttemptsFromCurios = 0;
    private float _respiraBonusFromCurios = 0.0f;

    [Export] public int RealmIndex
    {
        get => _realmIndex;
        set
        {
            _realmIndex = value;
            EmitChanged();
        }
    }

    [Export] public int RespiraAttemptsFromTechniques
    {
        get => _respiraAttemptsFromTechniques;
        set
        {
            _respiraAttemptsFromTechniques = value;
            EmitChanged();
        }
    }

    [Export] public float RespiraBonusFromTechniques
    {
        get => _respiraBonusFromTechniques;
        set
        {
            _respiraBonusFromTechniques = value;
            EmitChanged();
        }
    }

    [Export] public int RespiraAttemptsFromFriends
    {
        get => _respiraAttempsFromFriends;
        set
        {
            _respiraAttempsFromFriends = value;
            EmitChanged();
        }
    }

    [Export] public float RespiraBonusFromFriends
    {
        get => _respiraBonusFromFriends;
        set
        {
            _respiraBonusFromFriends = value;
            EmitChanged();
        }
    }

    [Export] public int RespiraAttemptsFromCurios
    {
        get => _respiraAttemptsFromCurios;
        set
        {
            _respiraAttemptsFromCurios = value;
            EmitChanged();
        }
    }

    [Export] public float RespiraBonusFromCurios
    {
        get => _respiraBonusFromCurios;
        set
        {
            _respiraBonusFromCurios = value;
            EmitChanged();
        }
    }

    public int TotalRespiraAttempts => 10 + _respiraAttemptsFromTechniques + _respiraAttempsFromFriends + _respiraAttemptsFromCurios;
    public float TotalRespiraBonus => 1f + (_respiraBonusFromTechniques + _respiraBonusFromFriends + _respiraBonusFromCurios) / 100f;

    public long RespiraValue => (int)Math.Floor(RealmValues[(Realm.Classification)_realmIndex] * TotalRespiraBonus);
    public long DailyRespiraValue => GetDailyRespiraValue();
    private long GetDailyRespiraValue()
    {
        var result = 0;
        foreach (var odds in MultiplierOdds)
        {
            result += (int)Math.Floor(RespiraValue * odds.Item1 * odds.Item2 * TotalRespiraAttempts);
        }
        return result;
    }

}
