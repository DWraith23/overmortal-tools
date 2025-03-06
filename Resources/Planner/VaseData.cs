using Godot;

namespace OvermortalTools.Resources.Planner;

public partial class VaseData : Resource
{
    private bool _hasVase = false;
    private int _stars = 0;

    [Export]
    public bool HasVase
    {
        get => _hasVase;
        set
        {
            _hasVase = value;
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
}
