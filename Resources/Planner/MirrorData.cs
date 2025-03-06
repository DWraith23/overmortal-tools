using Godot;

namespace OvermortalTools.Resources.Planner;

public partial class MirrorData : Resource
{
    private bool _hasMirror = false;
    private int _mirrorStars = 0;

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
    public int MirrorStars
    {
        get => _mirrorStars;
        set
        {
            _mirrorStars = value;
            EmitChanged();
        }
    }
}
