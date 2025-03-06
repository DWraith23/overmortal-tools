using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace DumbChatBox;

public partial class LogMessage : Resource
{
    [Export] public string Sender { get; set; }
    [Export] public string Message { get; set; }
    [Export] public string Time { get; set; }

    public static LogMessage Create(string sender, string message) =>
        new()
        {
            Sender = sender,
            Message = message,
            Time = DateTime.Now.ToString("G")
        };
}
