using Godot;
using System;

namespace OvermortalTools.Scenes.Planner;

public partial class BasicInformation : VBoxContainer
{
    [Signal] public delegate void ValuesChangedEventHandler();
    [Signal] public delegate void ArtifactsUpdatedEventHandler(float mythicPills);

    [Export] public StageCalculator StageCalculator { get; set; }
    [Export] public PassiveCultivation PassiveCultivation { get; set; }
    [Export] public VaseStatus VaseStatus { get; set; }
    [Export] public MirrorStatus MirrorStatus { get; set; }

    private void OnValuesChanged() => EmitSignal(SignalName.ValuesChanged);
    private void OnArtifactsUpdated() => EmitSignal(SignalName.ArtifactsUpdated, VaseStatus.Data.DailyMythicPills + MirrorStatus.Data.DailyMythicPills);
}
