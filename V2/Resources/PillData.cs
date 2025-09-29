using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.Scripts;

namespace OvermortalTools.V2.Resources;

public partial class PillData : Resource
{
    #region Static Data
    private static Dictionary<Quality, float> QualityMultipliers => new()
    {
        { Quality.Common, 1f },
        { Quality.Uncommon, 2f },
        { Quality.Rare, 3.2f },
        { Quality.Epic, 6f },
        { Quality.Legendary, 12f },
        { Quality.Mythic, 24f },
    };

    private static Dictionary<PathData.Realm, int> BaseRealmValues => new()
    {
        { PathData.Realm.None, 0 },
        { PathData.Realm.Novice, 0 },
        { PathData.Realm.Connection, 125 },
        { PathData.Realm.Foundation, 625 },
        { PathData.Realm.Virtuoso, 1900 },
        { PathData.Realm.NascentSoul, 5000 },
        { PathData.Realm.Incarnation, 8000 },
        { PathData.Realm.Voidbreak, 12000 },
        { PathData.Realm.Wholeness, 20500 },
        { PathData.Realm.Perfection,  31000 },
        { PathData.Realm.Nirvana, 57000 },
        { PathData.Realm.Celestial, 128375 },
        { PathData.Realm.Eternal, 304375 },
    };

    #endregion


    #region Properties and Exports

    private int _dailyRare = 0;
    private int _dailyEpic = 0;
    private int _dailyLegendary = 0;
    private float _dailyMythic = 0;

    private float _techniquesBonus = 0f;
    private float _epicCurioBonus = 0f;
    private float _immortalFriendsBonus = 0f;

    [Export]
    public int DailyRare
    {
        get => _dailyRare;
        set
        {
            if (_dailyRare == value) return;
            _dailyRare = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public int DailyEpic
    {
        get => _dailyEpic;
        set
        {
            if (_dailyEpic == value) return;
            _dailyEpic = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public int DailyLegendary
    {
        get => _dailyLegendary;
        set
        {
            if (_dailyLegendary == value) return;
            _dailyLegendary = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float DailyMythic
    {
        get => _dailyMythic;
        set
        {
            if (_dailyMythic == value) return;
            _dailyMythic = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float TechniquesBonus
    {
        get => _techniquesBonus;
        set
        {
            if (_techniquesBonus == value) return;
            _techniquesBonus = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float EpicCurioBonus
    {
        get => _epicCurioBonus;
        set
        {
            if (_epicCurioBonus == value) return;
            _epicCurioBonus = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float ImmortalFriendsBonus
    {
        get => _immortalFriendsBonus;
        set
        {
            if (_immortalFriendsBonus == value) return;
            _immortalFriendsBonus = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    #endregion

    #region Calculated Properties
    public float TotalBonus => (TechniquesBonus + EpicCurioBonus + ImmortalFriendsBonus) / 100.0f;

    public int TotalDailyPills => DailyRare + DailyEpic + DailyLegendary;


    #endregion


    #region Methods

    public static long GetRareValue(PathData.Realm realm) => (long)(BaseRealmValues[realm] * QualityMultipliers[Quality.Rare]);
    public static long GetEpicValue(PathData.Realm realm) => (long)(BaseRealmValues[realm] * QualityMultipliers[Quality.Epic]);
    public static long GetLegendaryValue(PathData.Realm realm) => (long)(BaseRealmValues[realm] * QualityMultipliers[Quality.Legendary]);
    public static long GetMythicValue(PathData.Realm realm) => (long)(BaseRealmValues[realm] * QualityMultipliers[Quality.Mythic]);


    public long GetTotalRareValue(PathData.Realm realm, float otherBonuses = 0) =>
        (long)Math.Floor(GetRareValue(realm) * DailyRare * (1 + TotalBonus + otherBonuses));
    public long GetTotalEpicValue(PathData.Realm realm, float otherBonuses = 0) =>
        (long)Math.Floor(GetEpicValue(realm) * DailyEpic * (1 + TotalBonus + otherBonuses));
    public long GetTotalLegendaryValue(PathData.Realm realm, float otherBonuses = 0) =>
        (long)Math.Floor(GetLegendaryValue(realm) * DailyLegendary * (1 + TotalBonus + otherBonuses));
    public long GetTotalMythicValue(PathData.Realm realm, float otherBonuses = 0) =>
        (long)Math.Floor(GetMythicValue(realm) * DailyMythic * (1 + TotalBonus + otherBonuses));

    #endregion

    public bool NeedsAttention =>
        TotalDailyPills == 0 || TotalBonus == 0;

}
