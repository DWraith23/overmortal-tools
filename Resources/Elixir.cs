using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace OvermortalTools.Resources;

[GlobalClass, Tool]
public partial class Elixir : Consumable
{
    [Export] public override string DisplayName { get; set; }
    [Export] public int BaseExpValue { get; set; } = 350;

    [Export] private Godot.Collections.Dictionary<int, float> Ratios { get; set; }

    public int ModifiedValue(float ratio) => (int)(BaseExpValue * ratio);

    public float GetRatio(int uses)
    {
        int cumulative = 0;
        foreach (var entry in Ratios)
        {
            cumulative += entry.Key;
            if (uses <= cumulative)
            {
                return entry.Value;
            }
        }
        return 0f;
    }
}
