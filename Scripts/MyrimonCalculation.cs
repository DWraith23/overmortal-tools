using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace OvermortalTools.Scripts;

public partial class MyrimonCalculation : Resource
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

    public static Dictionary<string, int> BaseFruitValues => new()
    {
        { "Mortal", 65000 },
        { "Voidbreak", 96000 },
        { "Wholeness", 130000 },
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

    public Dictionary<Quality, float> QualityChances => CalculateQualityChances();

    [Export] public Quality ExtractorQuality { get; set; } = Quality.Common;
    [Export] public string FruitType { get; set; } = "Mortal";

    [Export] public int XPLevel { get; set; } = 0;
    [Export] public int QualityLevel { get; set; } = 0;
    [Export] public int GushLevel { get; set; } = 0;
    [Export] public int TechLevel { get; set; } = 0;

    [Export] public bool MatchesServerLevel { get; set; } = true;
    [Export] public int FruitQuantity { get; set; } = 0;
    public float GushMultiplier => 1.5f + 0.04f * GushLevel;
    public float GushChance => (float) Math.Floor(GushLevel / 5.1f) * 0.05f + 0.1f;
    public float TechChance => (int) ExtractorQuality > 3 ? 0.19f + TechLevel * 0.01f : 0f;
    public int TechPoints =>
        (int) Math.Floor(TechLevel / 5.1f) * 50 + 150;

    private int GetGuaranteedGushes(int fruit) => (int) Math.Floor(fruit / 6f);

    private Dictionary<Quality, float> CalculateQualityChances()
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

        var qualityBonus = (float) Math.Floor(QualityLevel / 5.1d) * 20f;
        qualityBonus += (int) ExtractorQuality * 30f;
        for (int i = 0; i < QualityLevel; i++)
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
    
    private Dictionary<Quality, int> GetMinimumQuantities()
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

        foreach (var kvp in CalculateQualityChances())
        {
            if (kvp.Value == 0f) continue;
            result[kvp.Key] = FruitQuantity;
            break;        
        }

        return result;
    }

    private Dictionary<Quality, int> GetAverageQuantities()
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

        foreach (var kvp in CalculateQualityChances())
        {
            if (kvp.Value == 0f) continue;
            if (kvp.Value == 50)
            {
                if (FruitQuantity == 1)
                {
                    result[kvp.Key] = 1;
                    break;
                }
                if (FruitQuantity % 2 != 0 && result.Values.Sum() == 0)
                {
                    result[kvp.Key] = FruitQuantity / 2 + 1;
                    continue;
                }
                result[kvp.Key] = FruitQuantity / 2;
                continue;
            }
            result[kvp.Key] = (int) Math.Round(FruitQuantity * kvp.Value / 100);
        }

        return result;
    }

    private Dictionary<Quality, int> GetMaximumQuantities()
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

        foreach (var kvp in CalculateQualityChances().Reverse())
        {
            if (kvp.Value == 0f) continue;
            result[kvp.Key] = FruitQuantity;
            break;
        }

        return result;
    }

    public int GetFruitValue(Quality quality)
    {
        var baseValue = BaseFruitValues[FruitType];
        var value = (baseValue * QualityMultipliers[quality]);
        value *= 1 + XPLevel * 0.02f;
        if (MatchesServerLevel) value *= 1.5f;
        return (int) Math.Floor(value);
    }

    public int GetMinimumXP()
    {
        int value = 0;
        
        foreach (var kvp in GetMinimumQuantities())
        {
            if (kvp.Value == 0) continue;
            var gushes = GetGuaranteedGushes(kvp.Value);
            var fruit = kvp.Value - gushes;
            value += fruit * GetFruitValue(kvp.Key);
            value += (int) Math.Floor(gushes * GetFruitValue(kvp.Key) * GushMultiplier);
        }

        return value;
    }

    public int GetAverageXP()
    {
        int value = 0;
        float totalGushes = GetGuaranteedGushes(FruitQuantity);
        totalGushes += (float) Math.Floor((FruitQuantity - totalGushes) * GushChance);


        foreach (var kvp in GetAverageQuantities())
        {
            if (kvp.Value == 0) continue;

            var gushes = (float) kvp.Value / FruitQuantity * totalGushes;
            var fruit = kvp.Value - gushes;
            var gushValue = (int) Math.Floor(gushes * GushMultiplier * GetFruitValue(kvp.Key));
            var fruitValue = (int) Math.Floor(fruit * GetFruitValue(kvp.Key));
            value += fruitValue + gushValue;

        }

        return value;
    }

    public int GetMaximumXP()
    {
        int value = 0;

        foreach (var kvp in GetMaximumQuantities())
        {
            if (kvp.Value == 0) continue;
            var gushes = FruitQuantity;
            var fruit = 0;
            value += fruit * GetFruitValue(kvp.Key);
            value += (int) Math.Floor(gushes * GetFruitValue(kvp.Key) * GushMultiplier);
        }

        return value;
    }

}
