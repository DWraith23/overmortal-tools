using Godot;
using System;

namespace OvermortalTools.V2.Resources;

public partial class SettingsMenu : Window
{
    public static SettingsMenu CreateInstance() =>
        GD.Load<PackedScene>("res://V2/Scenes/settings_menu.tscn").Instantiate<SettingsMenu>();

    private PanelContainer Panel => GetNode<PanelContainer>("Panel");

    private ColorPickerButton FavoriteColor => GetNode<ColorPickerButton>("Panel/Contents/Settings 1/Favorite Color/ColorPickerButton");

    private Button SaveButton => GetNode<Button>("Panel/Contents/Buttons/Save");
    private Button CancelButton => GetNode<Button>("Panel/Contents/Buttons/Cancel");

    public SettingsData Data { get; set; }

    public override void _Ready()
    {
        ConnectSignals();
        FavoriteColor.Color = Data.FavoriteColor;
        Update();
    }

    private void Update()
    {
        ChangePanelColor(FavoriteColor.Color);
    }

    private void ConnectSignals()
    {
        FavoriteColor.ColorChanged += color =>
        {
            Update();
        };

        SaveButton.Pressed += () =>
        {
            Data.FavoriteColor = FavoriteColor.Color;
            QueueFree();
        };

        CancelButton.Pressed += () =>
        {
            QueueFree();
        };
    }

    private void ChangePanelColor(Color color)
    {
        var box = Panel.GetThemeStylebox("panel").Duplicate(true) as StyleBoxFlat;
        box.BgColor = color;
        Panel.AddThemeStyleboxOverride("panel", box);
    }
}
