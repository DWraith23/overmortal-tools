using Godot;
using OvermortalTools.trash;
using System;
using System.Threading.Tasks;

namespace DumbChatBox;

public partial class DumbChatBox : PanelContainer
{
    [Export] private VBoxContainer MessageContainer { get; set; }
    [Export] private TextEdit InputField { get; set; }

    public ChatLog Log { get; set; } = new();

    public override void _Ready()
    {
        InputField.GrabFocus(); // Grab focus for quick typing. :)
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventKey key) return;    // Only care about keyboard input
        if (key.Keycode == Key.Enter && !key.Pressed)   // Only care about releasing the "enter" key
        {
            SendMessage();  // Send the message (same as pressing the button)
        }
    }

    private bool _botMessaging = false;
    private bool BotMessaging
    {
        get => _botMessaging;
        set
        {
            _botMessaging = value;
            InputField.Editable = !value;
        }
    }

    private void OnSendButtonPressed() => SendMessage();
    private async void SendMessage()
    {
        if (InputField.Text == "") return;
        // Create a ChatMessage node and put it in the container.
        var message = ChatMessage.GenerateMessage(InputField.Text, true);
        MessageContainer.AddChild(message);
        // Log the user's message
        var log = LogMessage.Create("User", InputField.Text);
        Log.AddMessage(log);
        // Clear the input for the next message
        InputField.Clear();
        // Give the AI some time to think
        var timer = GetTree().CreateTimer(0.5);
        await timer.ToSignal(timer, Timer.SignalName.Timeout);
        // Swap to AI message
        await BotMessage();
    }

    private async Task BotMessage()
    {
        BotMessaging = true;    // Prevent user input
        // AI thinks for a random amount of time
        var time = new Random().Next(0, 3) + new Random().NextDouble();
        GD.Print($"AI 'thinking' for {time} seconds...");
        var timer = GetTree().CreateTimer(time);
        // Create a ChatMessage node and put it in the container.
        var message = ChatMessage.GenerateMessage("...", false);
        MessageContainer.AddChild(message);
        // After the AI finishes thinking, he's pretty single-minded.
        await timer.ToSignal(timer, Timer.SignalName.Timeout);
        var text = "Go fuck yourself.";
        message.Text = text;

        // Log the bot's message
        var log = LogMessage.Create("Bot", text);
        Log.AddMessage(log);

        BotMessaging = false;   // Allow user input

        InputField.GrabFocus(); // Return user focus to the input field
    }
}
