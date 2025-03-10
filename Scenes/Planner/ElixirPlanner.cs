using Godot;
using OvermortalTools.Resources.Planner;
using System;

namespace OvermortalTools.Scenes.Planner;

public partial class ElixirPlanner : VBoxContainer
{
    [Signal] public delegate void ValuesChangedEventHandler();

    #region Exports
    [ExportGroup("Nodes")]
    [Export] private VBoxContainer MonspiritiaContainer { get; set; }
    [ExportSubgroup("Inputs")]
    [Export] private OptionButton RealmSelect { get; set; }
    [Export] private SpinBox UsedElixirs { get; set; }
    [Export] private SpinBox DailyElixirs { get; set; }
    [Export] private SpinBox UsedMonspiritia { get; set; }
    [Export] private SpinBox PlannedMonspiritia { get; set; }
    [ExportSubgroup("Outputs")]
    [Export] private LineEdit MaxMonspiritia { get; set; }
    [Export] private LineEdit MonspiritiaValue { get; set; }
    [Export] private LineEdit UsedValue { get; set; }
    [Export] private LineEdit CurrentValue { get; set; }
    [Export] private LineEdit DailyValue { get; set; }


    #endregion

    private ElixirPlannerData _data = new();
    public ElixirPlannerData Data
    {
        get => _data;
        set
        {
            _data = value;
            _data.Changed += Update;
            Update();
        }
    }

    public override void _Ready()
    {
        Data.Changed += Update;
    }

    #region Events
    private void OnRealmOptionSelected(int index) => Data.RealmIndex = index;
    private void OnUsedElixirsChanged(double value) => Data.UsedElixirs = (int)value;
    private void OnDailyElixirsChanged(double value) => Data.DailyElixirs = (int)value;
    private void OnUsedMonspiritiaChanged(double value) => Data.UsedMonspiritia = (int)value;
    private void OnPlannedMonspiritiaChanged(double value) => Data.PlannedMonspiritia = (int)value;

    #endregion

    private void Update()
    {
        GD.Print($"{DateTime.Now} : DEBUG: Updating ElixirPlanner.");

        ValidateInputs();
        CheckMonspiritia();
        UpdateOutputs();

        EmitSignal(SignalName.ValuesChanged);
    }

    private void ValidateInputs()
    {
        RealmSelect.Select(Data.RealmIndex);
        UsedElixirs.Value = Data.UsedElixirs;
        DailyElixirs.Value = Data.DailyElixirs;
        UsedMonspiritia.Value = Data.UsedMonspiritia;
        PlannedMonspiritia.Value = Data.PlannedMonspiritia;
    }

    private void UpdateOutputs()
    {
        MaxMonspiritia.Text = Data.MaxMonspiritia.ToString("N0");
        MonspiritiaValue.Text = Data.PlannedMonspiritiaValue.ToString("N0");
        UsedValue.Text = Data.UsedValue.ToString("N0");
        GD.Print("Getting Current Elixir Value");
        CurrentValue.Text = Data.CurrentValue.ToString("N0");
        DailyValue.Text = Data.DailyValue.ToString("N0");
    }

    private void CheckMonspiritia()
    {
        MonspiritiaContainer.Visible = Data.MonspiritiaAvailable;
        PlannedMonspiritia.MaxValue = Data.MonspiritiaRemaining;
        var value = Data.MonspiritiaAvailable
            ? Math.Clamp(Data.PlannedMonspiritia, 0, Data.MonspiritiaRemaining)
            : 0;
        if (Data.PlannedMonspiritia != value) Data.PlannedMonspiritia = value;
    }
}
