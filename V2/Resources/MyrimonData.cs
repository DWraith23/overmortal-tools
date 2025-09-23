using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.Scripts;

namespace OvermortalTools.V2.Resources;

public partial class MyrimonData : Resource
{

    #region Static Data

    public static Dictionary<PathData.Realm, int> BaseFruitValues => new()
    {
        { PathData.Realm.Virtuoso, 65000 },
        { PathData.Realm.NascentSoul, 65000 },
        { PathData.Realm.Incarnation, 65000 },
        { PathData.Realm.Voidbreak, 96000 },
        { PathData.Realm.Wholeness, 130000 },
        { PathData.Realm.Perfection, 240000 },
        { PathData.Realm.Nirvana, 420000 },
        { PathData.Realm.Celestial, 800000 },
        { PathData.Realm.Eternal, 1810000 },
    };

    public static Dictionary<Quality, float> QualityMultipliers => new()
    {
        { Quality.Common, 1f },
        { Quality.Uncommon, 1.3f },
        { Quality.Rare, 1.6f },
        { Quality.Epic, 2f },
        { Quality.Legendary, 2.4f },
        { Quality.Mythic, 3f },
    };

    public static Dictionary<Quality, int> TechDrops => new()
    {
        { Quality.Common, 150 },
        { Quality.Uncommon, 200 },
        { Quality.Rare, 250 },
        { Quality.Epic, 300 },
        { Quality.Legendary, 350 },
        { Quality.Mythic, 400 },
    };

    #endregion


    #region Properties and Exports

    private int _expLevel = 0;
    private int _qualityLevel = 0;
    private int _gushLevel = 0;
    private int _techLevel = 0;

    private Quality _extractorRank = 0;

    private int _currentFruitQuantity = 0;

    private bool _buysFruitPacks = false;
    private bool _buysTokenPacks = false;
    private bool _matchesServerRealm = true;

    [Export]
    public int ExpLevel
    {
        get => _expLevel;
        set
        {
            if (_expLevel == value) return;
            _expLevel = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public int QualityLevel
    {
        get => _qualityLevel;
        set
        {
            if (_qualityLevel == value) return;
            _qualityLevel = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public int GushLevel
    {
        get => _gushLevel;
        set
        {
            if (_gushLevel == value) return;
            _gushLevel = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public int TechLevel
    {
        get => _techLevel;
        set
        {
            if (_techLevel == value) return;
            _techLevel = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public Quality ExtractorRank
    {
        get => _extractorRank;
        set
        {
            if (_extractorRank == value) return;
            _extractorRank = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public int CurrentFruitQuantity
    {
        get => _currentFruitQuantity;
        set
        {
            if (_currentFruitQuantity == value) return;
            _currentFruitQuantity = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public bool BuysFruitPacks
    {
        get => _buysFruitPacks;
        set
        {
            if (_buysFruitPacks == value) return;
            _buysFruitPacks = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public bool BuysTokenPacks
    {
        get => _buysTokenPacks;
        set
        {
            if (_buysTokenPacks == value) return;
            _buysTokenPacks = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public bool MatchesServerRealm
    {
        get => _matchesServerRealm;
        set
        {
            if (_matchesServerRealm == value) return;
            _matchesServerRealm = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }


    #endregion


    #region Calculated Properties

    public float GushMultiplier => 1.5f + 0.04f * GushLevel;
    public float GushChance => (float)Math.Floor(GushLevel / 5.1f) * 0.05f + 0.1f;
    public float TechChance => TechLevel == 0 ? 0f : 0.19f + TechLevel * 0.01f;
    public int TechPts => (int)Math.Floor(TechLevel / 5.1f) * 50 + 150;


    #endregion

    #region Methods

    public Dictionary<Quality, float> GetQualityChances()
    {
        var result = new Dictionary<Quality, float>()
        {
            { Quality.Common, 0f },
            { Quality.Uncommon, 0f },
            { Quality.Rare, 0f },
            { Quality.Epic, 0f },
            { Quality.Legendary, 0f },
            { Quality.Mythic, 0f },
        };

        var bonus = (float)Math.Floor(QualityLevel / 5.1f) * 20
                    + (int)ExtractorRank * 30
                    + Math.Min(QualityLevel, 10) * 5
                    + Math.Min(Math.Max(QualityLevel - 10, 0), 20) * 10;

        var qualities = Enum.GetValues<Quality>()
            .Where(q => q != Quality.Common)
            .Reverse();
        var baseValue = -400;

        foreach (var quality in qualities)
        {
            var chance = Math.Min(baseValue + bonus, 100) - result.Values.Sum() * 100f;
            chance = Math.Max(chance, 0);
            result[quality] = chance / 100f;
            baseValue += 100;
        }

        result[Quality.Common] = Math.Max(100 - bonus, 0) / 100f;

        return result;
    }

    public Dictionary<Quality, int> GetMinimumQuantities(int fruit)
    {
        var result = new Dictionary<Quality, int>()
        {
            { Quality.Common, 0 },
            { Quality.Uncommon, 0 },
            { Quality.Rare, 0 },
            { Quality.Epic, 0 },
            { Quality.Legendary, 0 },
            { Quality.Mythic, 0 },
        };

        foreach (var kvp in GetQualityChances())
        {
            if (kvp.Value == 0) continue;
            result[kvp.Key] = fruit;
            break;
        }

        return result;
    }

    public Dictionary<Quality, int> GetMaximumQuantities(int fruit)
    {
        var result = new Dictionary<Quality, int>()
        {
            { Quality.Common, 0 },
            { Quality.Uncommon, 0 },
            { Quality.Rare, 0 },
            { Quality.Epic, 0 },
            { Quality.Legendary, 0 },
            { Quality.Mythic, 0 },
        };

        foreach (var kvp in GetQualityChances().Reverse())
        {
            if (kvp.Value == 0) continue;
            result[kvp.Key] = fruit;
            break;
        }

        return result;
    }

    public Dictionary<Quality, float> GetAverageQuantities(int fruit)
    {
        var result = new Dictionary<Quality, float>()
        {
            { Quality.Common, 0 },
            { Quality.Uncommon, 0 },
            { Quality.Rare, 0 },
            { Quality.Epic, 0 },
            { Quality.Legendary, 0 },
            { Quality.Mythic, 0 },
        };

        if (fruit <= 3)
        {
            var max = GetQualityChances().MaxBy(kvp => kvp.Key).Key;
            result[max] = fruit;
            return result;
        }

        foreach (var kvp in GetQualityChances())
        {
            GD.Print($"DEBUG: kvp.Key:{kvp.Key}|kvp.Value:{kvp.Value}");
            if (kvp.Value == 0) continue;
            if (kvp.Value == 1)
            {
                result[kvp.Key] = fruit;
                break;
            }
            result[kvp.Key] = (float)Math.Round(fruit * kvp.Value);
        }

        return result;
    }

    public long GetFruitValue(PathData.Realm realm, Quality quality)
    {
        if (!BaseFruitValues.TryGetValue(realm, out var value)) return 0;

        value = (int)Math.Floor(
            value
            * QualityMultipliers[quality]
            * (1 + ExpLevel * 0.02f)
            * (MatchesServerRealm ? 1.5f : 1)
            );

        return value;
    }

    public long GetMinimumValue(PathData.Realm realm, int fruit)
    {
        var gushes = (int)Math.Floor(fruit / 6f);
        var nonGushes = fruit - gushes;

        long result = 0;

        foreach (var kvp in GetMinimumQuantities(fruit))
        {
            if (kvp.Value == 0) continue;
            var value = GetFruitValue(realm, kvp.Key);
            result += gushes * value;
            result += nonGushes * value;
        }

        return result;
    }

    public long GetMaximumValue(PathData.Realm realm, int fruit)
    {
        var gushes = (int)Math.Floor(fruit / 6f);
        var nonGushes = fruit - gushes;

        long result = 0;

        foreach (var kvp in GetMaximumQuantities(fruit))
        {
            if (kvp.Value == 0) continue;
            var value = GetFruitValue(realm, kvp.Key);
            result += gushes * value;
            result += nonGushes * value;
        }

        return result;
    }


    public long GetAverageValue(PathData.Realm realm) => GetAverageValue(realm, CurrentFruitQuantity);
    public long GetAverageValue(PathData.Realm realm, int fruit)
    {
        long result = 0;
        var gushQty = (float)Math.Floor(fruit - Math.Floor(fruit / 6f)) * GushChance;

        foreach (var kvp in GetAverageQuantities(fruit))
        {
            if (kvp.Value == 0) continue;

            var gushes = kvp.Value / fruit * gushQty;
            var qty = kvp.Value - gushes;
            var value = GetFruitValue(realm, kvp.Key);
            result += (long)Math.Floor(qty * value) + (long)Math.Floor(gushes * value * GushMultiplier);
        }

        return result;
    }

    public int GetAverageTechPts() => GetAverageTechPts(CurrentFruitQuantity);
    public int GetAverageTechPts(int fruit) =>
        (int)Math.Floor(TechPts * TechChance * fruit);

    #endregion

}
