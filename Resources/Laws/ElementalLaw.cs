using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using OvermortalTools.Scripts;

namespace OvermortalTools.Resources.Laws;

[GlobalClass]
public partial class ElementalLaw : Resource
{
    public override string ToString() =>
        $"{Name}: Level {Level} - Bonus {Bonus + 1:P0} | To {NextThreshold}: {XpTowards}/{GetNextXpNeeded()}";
    private const float BASE_VALUE = 480.4f;

    private static Dictionary<int, long> ThresholdCosts => new()
    {
        { 50, 324000 },
        { 150, 16876000 },
        { 250, 112800000 },
        { 350, 450000000 },
        { 450, 1470000000 },
        { 550, 7950000000 },
        { 650, 27000000000 },
        { 750, 64000000000 },
        { 850, 162000000000 },
        { 950, 404000000000 },
        { 1050, 1403000000000 },
        { 1150, 3880000000000 },
        { 1250, 8250000000000 },
        { 1350, 19100000000000 },
        { 1450, 43800000000000 },
        { 1550, 130900000000000 },
        { 1650, 329000000000000 },
        { 1750, 673000000000000 },
        { 1850, 1510000000000000 },
        { 1950, 3340000000000000 },
        { 2000, 490000000000000 },
    };

    private string _name = string.Empty;
    private int _level = 1;
    private float _bonus = 0.0f;


    [Export]
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value) return;
            _name = value;
            EmitChanged();
        }
    }

    [Export]
    public int Level
    {
        get => _level;
        set
        {
            if (_level == value) return;
            _level = value;
            EmitChanged();
        }
    }

    [Export]
    public float Bonus
    {
        get => _bonus;
        set
        {
            if (_bonus == value) return;
            _bonus = value;
            EmitChanged();
        }
    }

    private long LeftoverXp { get; set; } = 0;

    public int Multiplier => GetMultiplier();
    public long PointsPerHour => GetPointsPerHour();
    public long NextThresholdXp => GetNextXpNeeded();
    public long XpRemaining => GetXpRequired();
    public long XpTowards => GetXpTowards();
    public long NextLevelXp => GetNextLevelXp();

    private int ThresholdLevel => GetThresholdLevel();
    private int GetThresholdLevel()
    {
        var thresholds = ThresholdCosts.Keys.ToList();
        if (Level == 2000) return thresholds.Count - 1;
        var result = thresholds.Where(t => t > Level).FirstOrDefault();
        // if (result == 0) return -1; // No threshold found
        var index = thresholds.IndexOf(result);
        return index;
    }

    public int NextThreshold => GetNextThreshold();
        

    private int GetNextThreshold()
    {
        if (Level >= 2000) return -1;
        if (Level >= 1950) return 2000;
        return ThresholdCosts.Keys.ToList().Where(t => t > Level).FirstOrDefault();
    }

    private int GetMultiplier() => (int)Math.Pow(2, ThresholdLevel);

    private long GetPointsPerHour()
    {
        long result = (long)Math.Floor(BASE_VALUE * Level * GetMultiplier() * (1f + Bonus));
        return result;
    }

    private long GetNextXpNeeded()
    {
        if (Level >= 2000) return 0;
        if (Level >= 1950) return 490000000000000;
        return ThresholdCosts.Values.ToList()[ThresholdLevel];
    }


    private long GetXpTowards()
    {
        if (Level == 0 || Level >= 2000) return 0;
        long total = GetNextXpNeeded();
        float fraction;
        if (Level < 50)
        {
            fraction = Level / 50f;
            return (long)Math.Floor(total * fraction);
        }
        else if (Level >= 1950)
        {
            var distance = 50 - (2000 - Level);
            fraction = distance / 50f;
            return (long)Math.Floor(total * fraction);
        }

        var away = 100 - (NextThreshold - Level);
        fraction = away / 100f;
        return (long)Math.Floor(total * fraction);
    }

    private long GetXpRequired() => GetNextXpNeeded() - GetXpTowards();

    private long GetNextLevelXp()
    {
        if (Level >= 2000) return 0;
        if (Level >= 1950)
        {
            return (490000000000000 / 50f).RoundDown();
        }
        long total = GetNextXpNeeded();
        var perLevel = total / 100f;
        if (NextThreshold - Level > 50)
        {
            return (long)Math.Floor(perLevel * 0.6f);
        }
        return (long)Math.Floor(perLevel * 1.4f);
    }

    public void AddXp(long xp)
    {
        var remaining = xp + LeftoverXp;
        while (remaining > NextLevelXp && Level < 2000)
        {
            remaining -= NextLevelXp;
            Level++;
        }
        LeftoverXp = remaining;
    }

}
