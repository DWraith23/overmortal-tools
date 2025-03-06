using System.Collections.Generic;
using Godot;

namespace OvermortalTools.Resources.Planner;

public partial class VaseData : Resource
{
    /// <summary>
    /// Correlates the number of stars (int) to how much energy is recharges every 15 minutes (float)
    /// </summary>
    private static Dictionary<int, float> RechargeValues => new()
    {
        { 0, 1f },
        { 1, 1.3f },
        { 2, 1.6f },
        { 3, 2f },
        { 4, 2.4f },
        { 5, 3f }
    };


    private bool _hasVase = false;
    private int _stars = 0;

    [Export]
    public bool HasVase
    {
        get => _hasVase;
        set
        {
            _hasVase = value;
            if (!value) Stars = 0;
            EmitChanged();
        }
    }

    [Export]
    public int Stars
    {
        get => _stars;
        set
        {
            _stars = value;
            EmitChanged();
        }
    }

    /// <summary>
    /// Returns what the XP provided by a generated Mythic Pill is increased by, based on Star count.
    /// </summary>
    public float XpMultiplier => GetXpMultiplier();

    private float GetXpMultiplier()
    {
        if (Stars == 0) return 1f;  // No bonus
        if (Stars < 3) return 1.1f; // 10% bonus at 1*
        return 1.2f;    // 20% bonus at 3*
    }

    /// <summary>
    /// How many whole and fractional Mythic Pills can be created by the artifact each day.  Includes the 100 energy fateum recharge.
    /// </summary>
    public float DailyMythicPills => GetDailyMythicPills();

    private float GetDailyMythicPills()
    {
        if (!HasVase) return 0f;

        var energy = 100f + (96f * RechargeValues[Stars]);  // Base daily energy with recharge
        var basic = energy / 100f;  // Base cost (assuming not using purple or gold)
        return Stars == 5 ? basic * 1.15f : basic;  // At 5*, 15% chance of duplicating pills.
    }
}
