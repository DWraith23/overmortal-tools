using Godot;
using OvermortalTools.Scripts;

namespace OvermortalTools.Resources.Planner;

public partial class StarMarksData : Resource
{
    private float _rarePills = 0f;
    private float _epicPills = 0f;
    private float _legendaryPills = 0f;
    private float _respiraExp = 0f;

    [Export]
    public float RarePills
    {
        get => _rarePills;
        set
        {
            if (_rarePills == value) return;
            _rarePills = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float EpicPills
    {
        get => _epicPills;
        set
        {
            if (_epicPills == value) return;
            _epicPills = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float LegendaryPills
    {
        get => _legendaryPills;
        set
        {
            if (_legendaryPills == value) return;
            _legendaryPills = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float RespiraExp
    {
        get => _respiraExp;
        set
        {
            if (_respiraExp == value) return;
            _respiraExp = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }
}
