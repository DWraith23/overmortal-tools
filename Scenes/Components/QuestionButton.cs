using Godot;
using System;

[GlobalClass, Tool]
public partial class QuestionButton : Button
{
    [Export] public string Title { get; set; }
    [Export(PropertyHint.MultilineText)] public string PopupText { get; set; }

    public override void _Ready()
    {
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        var popup = new Popup()
        {
            Size = new Vector2I(250, 500),
            InitialPosition = Window.WindowInitialPosition.CenterPrimaryScreen,
            Borderless = false,
            Title = Title,
            WrapControls = false,
        };
        var label = new Label()
        {
            Text = PopupText,
            CustomMinimumSize = new Vector2I(250, 500),
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
        };
        popup.AddChild(label);
        AddChild(popup);
        popup.Popup();
    }
}
