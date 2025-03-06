using Godot;

namespace OvermortalTools.Resources.Planner;

public partial class PassiveCultivationData : Resource
{
    private float _cosmoapsis = 0.0f;
    private int _auraGemIndex = 0;

    [Export]
    public float Cosmoapsis
    {
        get => _cosmoapsis;
        set
        {
            _cosmoapsis = value;
            EmitChanged();
        }
    }

    [Export]
    public int AuraGemIndex
    {
        get => _auraGemIndex;
        set
        {
            _auraGemIndex = value;
            EmitChanged();
        }
    }
}
