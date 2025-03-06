using Godot;
using System;

namespace OvermortalTools.Scenes.Planner;

public partial class BasicInformation : VBoxContainer
{
    [Signal] public delegate void ValuesChangedEventHandler();

    [Export] public StageCalculator StageCalculator { get; set; }
    [Export] public PassiveCultivation PassiveCultivation { get; set; }
    [Export] public VaseStatus VaseData { get; set; }
    [Export] public MirrorStatus MirrorData { get; set; }

    private void OnValuesChanged() => EmitSignal(SignalName.ValuesChanged);
}
