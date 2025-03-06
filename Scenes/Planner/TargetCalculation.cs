using System;
using Godot;

namespace OvermortalTools.Scenes.Planner;

public partial class TargetCalculation : VBoxContainer
{
    [Export] private LineEdit WithMyrimonDays { get; set; }
    [Export] private LineEdit WithoutMyrimonDays { get; set; }

    private int _currentXp = 0;
    private int _targetXp = 2;
    private int _dailyXp = 1;
    private int _myrimonAverageXp = 0;

    public int CurrentXp
    {
        get => _currentXp;
        set
        {
            _currentXp = value;
            Update();
        }
    }

    public int TargetXp
    {
        get => _targetXp;
        set
        {
            _targetXp = value;
            Update();
        }
    }

    public int DailyXp
    {
        get => _dailyXp;
        set
        {
            _dailyXp = value;
            Update();
        }
    }

    public int MyrimonAverageXp
    {
        get => _myrimonAverageXp;
        set
        {
            _myrimonAverageXp = value;
            Update();
        }
    }

    private int RemainingXp => TargetXp - CurrentXp;
    private int RemainingXpAfterMyrm => RemainingXp - MyrimonAverageXp;
    private int DaysNoMyrm => (int)Math.Ceiling(RemainingXp / DailyXp + 0d);
    private int DaysWithMyrm => (int)Math.Ceiling(RemainingXpAfterMyrm / DailyXp + 0d);

    private void Update()
    {
        WithMyrimonDays.Text = DaysWithMyrm.ToString("N0");
        WithoutMyrimonDays.Text = DaysNoMyrm.ToString("N0");
    }
}
