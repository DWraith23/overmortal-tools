using Godot;
using System;

public partial class ProfileSwapper : VBoxContainer
{
    [Export] public LineEdit ProfileName { get; set; }

    [Signal] public delegate void SwapButtonPressedEventHandler(int profile);
    [Signal] public delegate void ProfileNameChangedEventHandler();

    private void OnButtonPressed(int profile) => EmitSignal(SignalName.SwapButtonPressed, profile);
    private void OnProfileNameChanged(string text) => EmitSignal(SignalName.ProfileNameChanged);
}
