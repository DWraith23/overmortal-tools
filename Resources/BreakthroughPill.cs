using Godot;
using OvermortalTools.Scripts.Enums;

namespace OvermortalTools.Resources;

[GlobalClass, Tool]
public partial class BreakthroughPill : Consumable
{
    public override string DisplayName { get; set; } = "Breakthrough Pill";

    private int _rank = 1;
    [Export]
    public int Rank
    {
        get => _rank;
        set
        {
            _rank = value;
        }
    }

    private Quality.Classification _quality = Scripts.Enums.Quality.Classification.Common;
    [Export]
    public Quality.Classification Quality
    {
        get => _quality;
        set
        {
            _quality = value;
        }
    }
}
