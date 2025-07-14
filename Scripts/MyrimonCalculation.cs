using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace OvermortalTools.Scripts;

public static class MyrimonCalculation
{
    public enum Quality
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic
    }

    public enum Realm
    {
        Mortal,
        Voidbreak,
        Wholeness,
        Perfection,
    }

    public static Dictionary<Realm, int> BaseFruitValues => new()
    {
        { Realm.Mortal, 65000 },
        { Realm.Voidbreak, 96000 },
        { Realm.Wholeness, 130000 },
        { Realm.Perfection, 240000 }
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

    private static Dictionary<Quality, int> QualityThresholds => new()
    {
        { Quality.Common, 0 },
        { Quality.Uncommon, 6 },
        { Quality.Rare, 11 },
        { Quality.Epic, 16 },
        { Quality.Legendary, 21 },
        { Quality.Mythic, 26 },
    };

    public static Dictionary<Quality, float> QualityChances(Quality extractorQuality, int qualityLevel)
        => CalculateQualityChances(extractorQuality, qualityLevel);

    public static float GushMultiplier(int gushLevel) => 1.5f + 0.04f * gushLevel;
    public static float GushChance(int gushLevel) => (float) Math.Floor(gushLevel / 5.1f) * 0.05f + 0.1f;
    public static float TechChance(Quality extractorQuality, int techLevel)
        => (int) extractorQuality > 3 ? 0.19f + techLevel * 0.01f : 0f;
    public static int TechPoints(int techLevel) => (int) Math.Floor(techLevel / 5.1f) * 50 + 150;

    private static int GetGuaranteedGushes(int fruit) => (int) Math.Floor(fruit / 6f);

    private static Dictionary<Quality, float> CalculateQualityChances(Quality extractorQuality, int qualityLevel)
    {
        var result = new Dictionary<Quality, float>()
        {
            { Quality.Common, 1f },
            { Quality.Uncommon, 0f },
            { Quality.Rare, 0f },
            { Quality.Epic, 0f },
            { Quality.Legendary, 0f },
            { Quality.Mythic, 0f },
        };

        var qualityBonus = (float) Math.Floor(qualityLevel / 5.1d) * 20f;
        qualityBonus += (int) extractorQuality * 30f;
        for (int i = 0; i < qualityLevel; i++)
        {
            if (i < 10) qualityBonus += 5;
            else qualityBonus += 10;
        }

        result[Quality.Mythic] = (float) Math.Max(Math.Min(-400+qualityBonus, 100), 0);
        result[Quality.Legendary] = (float) Math.Max(Math.Min(-300+qualityBonus, 100) - result[Quality.Mythic], 0);
        result[Quality.Epic] = (float) Math.Max
        (
            Math.Min(-200+qualityBonus, 100)
            - result[Quality.Mythic]
            - result[Quality.Legendary],
            0
        );
        result[Quality.Rare] = (float) Math.Max
        (
          Math.Min(-100 + qualityBonus, 100)
          - result[Quality.Mythic]
          - result[Quality.Legendary]
          - result[Quality.Epic],
          0  
        );
        result[Quality.Uncommon] = (float) Math.Max
        (
          Math.Min(qualityBonus, 100)
          - result[Quality.Mythic]
          - result[Quality.Legendary]
          - result[Quality.Epic]
          - result[Quality.Rare],
          0  
        );
        result[Quality.Common] = (float) Math.Max(100 - qualityBonus, 0);

        return result;
    }
    
    private static Dictionary<Quality, int> GetMinimumQuantities(Quality extractorQuality, int qualityLevel, int quantity)
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

        foreach (var kvp in CalculateQualityChances(extractorQuality, qualityLevel))
        {
            if (kvp.Value == 0f) continue;
            result[kvp.Key] = quantity;
            break;        
        }

        return result;
    }

    private static Dictionary<Quality, int> GetAverageQuantities(Quality extractorQuality, int qualityLevel, int quantity)
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

        foreach (var kvp in CalculateQualityChances(extractorQuality, qualityLevel))
        {
            if (kvp.Value == 0f) continue;
            if (kvp.Value == 50)
            {
                if (quantity == 1)
                {
                    result[kvp.Key] = 1;
                    break;
                }
                if (quantity % 2 != 0 && result.Values.Sum() == 0)
                {
                    result[kvp.Key] = quantity / 2 + 1;
                    continue;
                }
                result[kvp.Key] = quantity / 2;
                continue;
            }
            result[kvp.Key] = (int) Math.Round(quantity * kvp.Value / 100);
        }

        return result;
    }

    private static Dictionary<Quality, int> GetMaximumQuantities(Quality extractorQuality, int qualityLevel, int quantity)
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

        foreach (var kvp in CalculateQualityChances(extractorQuality, qualityLevel).Reverse())
        {
            if (kvp.Value == 0f) continue;
            result[kvp.Key] = quantity;
            break;
        }

        return result;
    }

    private static int GetFruitValue(Quality quality, Realm realm, int expLevel, bool matchesServerLevel)
    {
        var baseValue = BaseFruitValues[realm];
        var value = (baseValue * QualityMultipliers[quality]);
        value *= 1 + expLevel * 0.02f;
        if (matchesServerLevel) value *= 1.5f;
        return (int) Math.Floor(value);
    }

    public static int GetAverageTechPts(Quality extractorQuality, int techLevel, int quantity)
    {
        var averagePts = TechPoints(techLevel) * TechChance(extractorQuality, techLevel);
        return (int) Math.Floor(averagePts * quantity);
    }

    public static int GetMinimumXP(
        Quality extractorQuality,
        int qualityLevel,
        int quantity,
        Realm realm,
        int expLevel,
        bool matchesServerLevel,
        int gushLevel
        )
    {
        int value = 0;
        
        foreach (var kvp in GetMinimumQuantities(extractorQuality, qualityLevel, quantity))
        {
            if (kvp.Value == 0) continue;
            var gushes = GetGuaranteedGushes(kvp.Value);
            var fruit = kvp.Value - gushes;
            value += fruit * GetFruitValue(kvp.Key, realm, expLevel, matchesServerLevel);
            value += (int) Math.Floor(gushes * GetFruitValue(kvp.Key, realm, expLevel, matchesServerLevel) * GushMultiplier(gushLevel));
        }

        return value;
    }

    public static int GetAverageXP(
        Quality extractorQuality,
        int qualityLevel,
        int quantity,
        Realm realm,
        int expLevel,
        bool matchesServerLevel,
        int gushLevel
        )
    {
        int value = 0;
        float totalGushes = GetGuaranteedGushes(quantity);
        totalGushes += (float) Math.Floor((quantity - totalGushes) * GushChance(gushLevel));

        foreach (var kvp in GetAverageQuantities(extractorQuality, qualityLevel, quantity))
        {
            if (kvp.Value == 0) continue;

            var gushes = (float) kvp.Value / quantity * totalGushes;
            var fruit = kvp.Value - gushes;
            var gushValue = (int) Math.Floor(gushes * GushMultiplier(gushLevel) * GetFruitValue(kvp.Key, realm, expLevel, matchesServerLevel));
            var fruitValue = (int) Math.Floor(fruit * GetFruitValue(kvp.Key, realm, expLevel, matchesServerLevel));
            value += fruitValue + gushValue;

        }

        return value;
    }

    public static int GetMaximumXP(
        Quality extractorQuality,
        int qualityLevel,
        int quantity,
        Realm realm,
        int expLevel,
        bool matchesServerLevel,
        int gushLevel
        )
    {
        int value = 0;

        foreach (var kvp in GetMaximumQuantities(extractorQuality, qualityLevel, quantity))
        {
            if (kvp.Value == 0) continue;
            var gushes = quantity;
            var fruit = 0;
            value += fruit * GetFruitValue(kvp.Key, realm, expLevel, matchesServerLevel);
            value += (int) Math.Floor(gushes * GetFruitValue(kvp.Key, realm, expLevel, matchesServerLevel) * GushMultiplier(gushLevel));
        }

        return value;
    }

}
