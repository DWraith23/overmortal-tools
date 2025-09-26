using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.Scripts;

namespace OvermortalTools.V2.Resources;

public partial class LawData : Resource
{

    #region Static Data
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

    private static Dictionary<int, float> ShearsRecharge => new()
    {
        { 0, 1f },
        { 1, 1.3f },
        { 2, 1.6f },
        { 3, 2f },
        { 4, 2.4f },
        { 5, 3f }
    };
    #endregion

    #region Properties and Exports

    private int _level = 0;
    private float _bonus = 0.0f;

    [Export]
    public int Level
    {
        get => _level;
        set
        {
            if (_level == value) return;
            _level = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
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
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }
    private float BonusPercent => Bonus / 100f;

    private long TempLeftoverXp { get; set; } = 0;

    #endregion

    #region Calculated Properties

    public int NextMultiplierThreshold => ThresholdCosts.Keys.Where(t => t > Level || t == 2000).Min();
    public int CurrentMultiplierThreshold => ThresholdCosts.Keys.ToList().IndexOf(NextMultiplierThreshold);
    public int Multiplier => (int)Math.Pow(2, CurrentMultiplierThreshold);
    public long PointsPerHour => (long)Math.Floor(BASE_VALUE * Level * Multiplier * (1f + BonusPercent));

    public long AccumulatedXP => ThresholdCosts.Where(kvp => kvp.Key < Level).Select(kvp => kvp.Value).Sum();
    public long XPNeededForNextThreshold => ThresholdCosts[NextMultiplierThreshold];
    public long XPGainedTowardsNextThreshold => (NextMultiplierThreshold - Level) / 100 * XPNeededForNextThreshold;
    public long XPRemainingForNextThreshold => XPNeededForNextThreshold - XPGainedTowardsNextThreshold;
    public long XPNeededForNextLevel => XPNeededForNextThreshold / (NextMultiplierThreshold == 50 ? 50 : 100);

    #endregion

    public void AddXp(long xp)
    {
        var remaining = xp + TempLeftoverXp;
        while (remaining > XPNeededForNextLevel && Level < 2000)
        {
            remaining -= XPNeededForNextLevel;
            Level++;
        }
        TempLeftoverXp = remaining;
    }

}
