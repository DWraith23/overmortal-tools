using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.Scripts;

namespace OvermortalTools.V2.Resources;

public partial class RespiraData : Resource
{
    #region Static Data

    private static Dictionary<PathData.Realm, int> RealmValues => new()
    {
        { PathData.Realm.None, 0 },
        { PathData.Realm.Novice, 6 },
        { PathData.Realm.Connection, 20 },
        { PathData.Realm.Foundation, 110 },
        { PathData.Realm.Virtuoso, 650 },
        { PathData.Realm.NascentSoul, 3200 },
        { PathData.Realm.Incarnation, 5300 },
        { PathData.Realm.Voidbreak, 7800 },
        { PathData.Realm.Wholeness, 10500 },
        { PathData.Realm.Perfection, 13500 },
        { PathData.Realm.Nirvana, 25000 },
        { PathData.Realm.Celestial, 37500 },
    };

    private static List<(float, int)> MultiplierOdds =>
    [
        (0.55f, 1),
        (0.30f, 2),
        (0.1475f, 5),
        (0.0025f, 10),
    ];


    #endregion

    #region Properties and Exports

    private int _respiraAttemptsFromTechniques = 0;
    private float _respiraBonusFromTechniques = 0.0f;
    private int _respiraAttempsFromFriends = 0;
    private float _respiraBonusFromFriends = 0.0f;
    private int _respiraAttemptsFromCurios = 0;
    private float _respiraBonusFromCurios = 0.0f;

    [Export]
    public int RespiraAttemptsFromTechniques
    {
        get => _respiraAttemptsFromTechniques;
        set
        {
            if (_respiraAttemptsFromTechniques == value) return;
            _respiraAttemptsFromTechniques = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float RespiraBonusFromTechniques
    {
        get => _respiraBonusFromTechniques;
        set
        {
            if (_respiraBonusFromTechniques == value) return;
            _respiraBonusFromTechniques = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public int RespiraAttemptsFromFriends
    {
        get => _respiraAttempsFromFriends;
        set
        {
            if (_respiraAttempsFromFriends == value) return;
            _respiraAttempsFromFriends = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float RespiraBonusFromFriends
    {
        get => _respiraBonusFromFriends;
        set
        {
            if (_respiraBonusFromFriends == value) return;
            _respiraBonusFromFriends = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public int RespiraAttemptsFromCurios
    {
        get => _respiraAttemptsFromCurios;
        set
        {
            if (_respiraAttemptsFromCurios == value) return;
            _respiraAttemptsFromCurios = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float RespiraBonusFromCurios
    {
        get => _respiraBonusFromCurios;
        set
        {
            if (_respiraBonusFromCurios == value) return;
            _respiraBonusFromCurios = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }


    #endregion

    #region Calculated Properties

    public int TotalRespiraAttempts => 10 + RespiraAttemptsFromCurios + RespiraAttemptsFromFriends + RespiraAttemptsFromTechniques;

    public float TotalRespiraBonus => 1 + (RespiraBonusFromCurios + RespiraBonusFromFriends + RespiraBonusFromTechniques) / 100f;

    #endregion

    #region Methods

    public long GetAverageRespiraValue(PathData.Realm realm)
    {
        var value = RealmValues[realm];
        var withOdds = value * MultiplierOdds.Sum(mo => mo.Item1 * mo.Item2);
        return (long)Math.Floor(withOdds);
    }

    public long GetTotalAverageRespiraValue(PathData.Realm realm)
    {
        var value = GetAverageRespiraValue(realm);
        return (long)Math.Floor(value * TotalRespiraBonus);
    }

    public long GetDailyRespiraValue(PathData.Realm realm) => GetAverageRespiraValue(realm) * TotalRespiraAttempts;

    #endregion


    public bool NeedsAttention =>
        TotalRespiraAttempts == 10 || TotalRespiraBonus == 1;

}
