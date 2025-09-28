using Godot;
using OvermortalTools.Scripts;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class ProfileSwapper : VBoxContainer
{
    [Export] public LineEdit ProfileName { get; set; }

    [Signal] public delegate void SwapButtonPressedEventHandler(int profile);
    [Signal] public delegate void ProfileNameChangedEventHandler(string name);

    private Button ProfileButton1 => GetNode<Button>("Buttons/Profile 1");
    private Button ProfileButton2 => GetNode<Button>("Buttons/Profile 2");
    private Button ProfileButton3 => GetNode<Button>("Buttons/Profile 3");

    public override void _Ready()
    {
        ProfileButton1.Pressed += () => OnButtonPressed(0);
        ProfileButton2.Pressed += () => OnButtonPressed(1);
        ProfileButton3.Pressed += () => OnButtonPressed(2);
        ProfileName.TextChanged += OnProfileNameChanged;
    }



    private void OnButtonPressed(int profile) => Tools.EmitLoggedSignal(this, SignalName.SwapButtonPressed, profile);
    private void OnProfileNameChanged(string text) => Tools.EmitLoggedSignal(this, SignalName.ProfileNameChanged, text);
}
