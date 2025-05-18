using System.Collections.Generic;
using Godot;

namespace OvermortalTools.Resources.Abilities;

[GlobalClass]
public partial class AbilityData : Resource
{
    public enum CultivationStages
    {
        Connection,
        Foundation,
        Virtuoso,
        NascentSoul,
        Incarnation,
        Voidbreak,
        Wholeness,
        Perfection,
        Nirvana
    }

    /// <summary>
    /// How many books it takes to upgrade to a specific level.
    /// </summary>
    private static Dictionary<int, int> CostThresholds => new()
    {
        { 20, 4 }
    };

    private static Dictionary<CultivationStages, int> StageBookCost => new()
    {

    };

    [Export] public string Name { get; set; }
    [Export] public Texture2D Icon { get; set; }
    [Export] public int MaxLevel { get; set; }
    [Export] public CultivationStages Stage { get; set; }
    [Export] public bool SecondaryIncarnation { get; set; }

    private int _level = 0;
    public int Level { get => _level; set { _level = value; EmitChanged(); } }

    public int NextLevelCost => GetNextLevelCost();
    private int GetNextLevelCost()
    {
        return default; // TODO: Implement
    }
}
