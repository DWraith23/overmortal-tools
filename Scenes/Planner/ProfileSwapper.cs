using Godot;
using System;

public partial class ProfileSwapper : HBoxContainer
{
    [Signal] public delegate void SwapButtonPressedEventHandler(int profile);

    private void OnButtonPressed(int profile) => EmitSignal(SignalName.SwapButtonPressed, profile);
}
