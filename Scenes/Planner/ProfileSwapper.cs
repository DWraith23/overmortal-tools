using Godot;
using OvermortalTools.Scripts;
using System;

public partial class ProfileSwapper : VBoxContainer
{
    [Export] public LineEdit ProfileName { get; set; }

    [Signal] public delegate void SwapButtonPressedEventHandler(int profile);
    [Signal] public delegate void ProfileNameChangedEventHandler(string name);

    private void OnButtonPressed(int profile) => Tools.EmitLoggedSignal(this, SignalName.SwapButtonPressed, profile);
    private void OnProfileNameChanged(string text) => Tools.EmitLoggedSignal(this, SignalName.ProfileNameChanged, text);
}
