using System;
using Godot;

namespace OvermortalTools.Scenes.Planner;

public partial class TargetCalculation : VBoxContainer
{
    [ExportGroup("Nodes")]
    [ExportSubgroup("Input")]
    [Export] private CheckBox PassiveCheck { get; set; }
    [Export] private CheckBox RespiraCheck { get; set; }
    [Export] private CheckBox PillCheck { get; set; }
    [ExportSubgroup("Output")]
    [Export] private LineEdit PassiveXpNode { get; set; }
    [Export] private LineEdit RespiraXpNode { get; set; }
    [Export] private LineEdit PillXpNode { get; set; }
    [Export] private LineEdit DailyXpNode { get; set; }
    [Export] private LineEdit WithMyrimonDays { get; set; }
    [Export] private LineEdit WithoutMyrimonDays { get; set; }

    private int _currentXp = 0;
    private int _targetXp = 2;
    private int _passiveXp = 0;
    private int _respiraXp = 60;
    private int _pillXp = 0;
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

    public int PassiveXp
    {
        get => _passiveXp;
        set
        {
            _passiveXp = value;
            Update();
        }
    }

    public int RespiraXp
    {
        get => _respiraXp;
        set
        {
            _respiraXp = value;
            Update();
        }
    }

    public int PillXp
    {
        get => _pillXp;
        set
        {
            _pillXp = value;
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

    private bool AddPassive => PassiveCheck.ButtonPressed;
    private bool AddRespira => RespiraCheck.ButtonPressed;
    private bool AddPill => PillCheck.ButtonPressed;

    private int DailyXp => GetDailyXp();
    private int GetDailyXp()
    {
        var xp = 0;
        if (AddPassive) xp += PassiveXp;
        if (AddRespira) xp += RespiraXp;
        if (AddPill) xp += PillXp;
        return xp;
    }

    private int RemainingXp => TargetXp - CurrentXp;
    private int RemainingXpAfterMyrm => RemainingXp - MyrimonAverageXp;
    private int DaysNoMyrm => (int)Math.Ceiling(RemainingXp / DailyXp + 0d);
    private int DaysWithMyrm => (int)Math.Ceiling(RemainingXpAfterMyrm / DailyXp + 0d);

    private void OnCheckBoxChecked(bool buttonPressed) => Update();

    private void Update()
    {
        PassiveXpNode.Text = PassiveXp.ToString("N0");
        RespiraXpNode.Text = RespiraXp.ToString("N0");
        PillXpNode.Text = PillXp.ToString("N0");
        DailyXpNode.Text = DailyXp.ToString("N0");
        WithMyrimonDays.Text = DailyXp == 0 ? "Infinity" : DaysWithMyrm.ToString("N0");
        WithoutMyrimonDays.Text = DailyXp == 0 ? "Infinity" : DaysNoMyrm.ToString("N0");
    }
}
