using System.Collections.Generic;
using Godot;

namespace OvermortalTools.Scripts.Enums;

public static class Quality
{
    public enum Classification
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic
    }

    public static readonly Dictionary<Classification, Color> Colors = new()
    {
        { Classification.Common, Godot.Colors.Gray },
        { Classification.Uncommon, Godot.Colors.Green },
        { Classification.Rare, Godot.Colors.LightBlue },
        { Classification.Epic, Godot.Colors.Purple },
        { Classification.Legendary, Godot.Colors.Orange },
        { Classification.Mythic, Godot.Colors.Red }
    };

    public static readonly Dictionary<Classification, int> Indexes = new()
    {
        { Classification.Common, 0 },
        { Classification.Uncommon, 1 },
        { Classification.Rare, 2 },
        { Classification.Epic, 3 },
        { Classification.Legendary, 4 },
        { Classification.Mythic, 5 }
    };

    public static readonly Dictionary<Classification, string> Names = new()
    {
        { Classification.Common, "Common" },
        { Classification.Uncommon, "Uncommon" },
        { Classification.Rare, "Rare" },
        { Classification.Epic, "Epic" },
        { Classification.Legendary, "Legendary" },
        { Classification.Mythic, "Mythic" }
    };

    public static List<string> NamesList =>
    [
        "Common",
        "Uncommon",
        "Rare",
        "Epic",
        "Legendary",
        "Mythic"
    ];
}
