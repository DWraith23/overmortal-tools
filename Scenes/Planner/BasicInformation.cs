using Godot;
using System;

namespace OvermortalTools.Scenes.Planner;

public partial class BasicInformation : VBoxContainer
{
    [Signal] public delegate void ValuesChangedEventHandler();

    [Export] public StageCalculator StageCalculator { get; set; }
    [Export] public PassiveCultivation PassiveCultivation { get; set; }
    [Export] public VaseData VaseData { get; set; }
    [Export] public MirrorData MirrorData { get; set; }

    private void OnValuesChanged() => EmitSignal(SignalName.ValuesChanged);
}
