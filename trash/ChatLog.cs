using DumbChatBox;
using Godot;

namespace OvermortalTools.trash;

public partial class ChatLog : Resource
{
    [Export] public Godot.Collections.Array<LogMessage> Messages { get; set; } = [];

    public void AddMessage(LogMessage message)
    {
        Messages.Add(message);
        SaveLog();
    }

    public LogMessage RetrieveMessage(int index) => Messages[index];

    public void SaveLog()
    {
        var path = OS.GetExecutablePath().GetBaseDir() + "/chat_log.tres";
        var result = ResourceSaver.Save(this, path);
        if (result != Error.Ok)
        {
            GD.Print("Failed to save.");
        }
        else
        {
            GD.Print("Successful save.");
        }
    }
}
