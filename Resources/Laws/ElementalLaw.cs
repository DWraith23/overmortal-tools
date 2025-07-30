using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace OvermortalTools.Resources.Laws;

[GlobalClass]
public partial class ElementalLaw : Resource
{
    private const int BASE_VALUE = 480;

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
    private int _level = 0;
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
            GD.Print($"ThresholdLevel:{ThresholdLevel}");
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

    public int Multiplier => GetMultiplier();
    public long PointsPerHour => GetPointsPerHour();
    public long NextThresholdXp => GetNextXpNeeded();
    public long XpRemaining => GetXpRequired();
    public long XpTowards => GetXpTowards();

    private int ThresholdLevel => GetThresholdLevel();
    private int GetThresholdLevel()
    {
        var thresholds = ThresholdCosts.Keys.ToList();
        var result = thresholds.Where(t => t > Level).First();
        var index = thresholds.IndexOf(result);
        return index;
    }

    private int NextThreshold =>
        ThresholdCosts.Keys.ToList().Where(t => t > Level).First();

    private int GetMultiplier() => (int)Math.Pow(2, ThresholdLevel);

    private long GetPointsPerHour()
    {
        long result = BASE_VALUE * Level;
        result *= GetMultiplier();
        result = (long)Math.Floor(result * (1f + Bonus));
        return result;
    }

    private long GetNextXpNeeded() => ThresholdCosts.Values.ToList()[ThresholdLevel];

    private long GetXpTowards()
    {
        if (Level == 0 || Level == 2000) return 0;
        long total = GetNextXpNeeded();
        float fraction;
        if (Level < 50)
        {
            fraction = Level / 50f;
            return (long)Math.Floor(total * fraction);
        }

        var away = 100 - (NextThreshold - Level);
        fraction = away / 100f;
        return (long)Math.Floor(total * fraction);
    }

    private long GetXpRequired() => GetNextXpNeeded() - GetXpTowards();
}
