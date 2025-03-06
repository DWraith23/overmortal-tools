using Godot;

namespace OvermortalTools.Resources.Planner;

public partial class PillPlannerData : Resource
{
    private int _pillRealmIndex = 0;
    private int _rarePills = 0;
    private int _epicPills = 0;
    private int _legendaryPills = 0;
    private int _totalPillValue = 0;
    private int _bonusPillValue = 0;

    [Export]
    public int PillRealmIndex
    {
        get => _pillRealmIndex;
        set
        {
            _pillRealmIndex = value;
            EmitChanged();
        }
    }

    [Export]
    public int RarePills
    {
        get => _rarePills;
        set
        {
            _rarePills = value;
            EmitChanged();
        }
    }

    [Export]
    public int EpicPills
    {
        get => _epicPills;
        set
        {
            _epicPills = value;
            EmitChanged();
        }
    }

    [Export]
    public int LegendaryPills
    {
        get => _legendaryPills;
        set
        {
            _legendaryPills = value;
            EmitChanged();
        }
    }

    [Export]
    public int TotalPillValue
    {
        get => _totalPillValue;
        set
        {
            _totalPillValue = value;
            EmitChanged();
        }
    }

    [Export]
    public int BonusPillValue
    {
        get => _bonusPillValue;
        set
        {
            _bonusPillValue = value;
            EmitChanged();
        }
    }
}
