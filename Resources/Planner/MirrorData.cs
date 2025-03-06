using System.Collections.Generic;
using Godot;

namespace OvermortalTools.Resources.Planner;

public partial class MirrorData : Resource
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


    private bool _hasMirror = false;
    private int _stars = 0;

    [Export]
    public bool HasMirror
    {
        get => _hasMirror;
        set
        {
            _hasMirror = value;
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
    /// How many whole and fractional Mythic Pills can be created by the artifact each day.  Includes the 100 energy fateum recharge.
    /// </summary>
    public float DailyMythicPills => GetDailyMythicPills();

    private float GetDailyMythicPills()
    {
        if (!HasMirror) return 0f;

        var energy = 100f + (96f * RechargeValues[Stars]);
        var cost = 200 *
            (Stars == 0 ? 1f
                : Stars < 3 ? 0.95f // 1* mirror has 5% cost reduction
                : 0.9f); // 3* mirror has 10% cost reduction
        return energy / cost * (Stars == 5 ? 1.15f : 1f); // 5* mirror has 15% chance of double duplication
    }
}
