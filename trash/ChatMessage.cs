using Godot;
using System;

namespace DumbChatBox;

public partial class ChatMessage : PanelContainer
{
    private static string UserColor => "9ffffb";
    private static string BotColor => "c8ffa1";

    [Export] private Label TextLabel { get; set; }

    private string _text = "";
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            TextLabel.Text = value;
            CheckLabelSize();
        }
    }

    private void CheckLabelSize()
    {
        if (TextLabel.Size.X <= 450) return;

        TextLabel.CustomMinimumSize = new(450, 0);
        TextLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
    }

    private void SetBackgroundColor(bool user)
    {
        var box = GetThemeStylebox("panel").Duplicate(true) as StyleBoxFlat;
        box.BgColor = user ? Color.FromHtml(UserColor) : Color.FromHtml(BotColor);
        AddThemeStyleboxOverride("panel", box);
    }

    public static ChatMessage GenerateMessage(string text, bool user)
    {
        var instance = GD.Load<PackedScene>("res://trash/chat_message.tscn").Instantiate<ChatMessage>();
        instance.Text = text;
        if (!user)
        {
            instance.TextLabel.HorizontalAlignment = HorizontalAlignment.Right;
            instance.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
        }
        else
        {
            instance.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        }

        instance.SetBackgroundColor(user);

        return instance;
    }
}
