using System;
using Godot;

namespace OvermortalTools.Scripts;

public static class Extensions
{
    public static void DeleteLastCharacter(this LineEdit node)
    {
        var text = node.Text;
        if (text.Length == 0) return;
        var newText = text.Substr(0, text.Length - 1);
        node.SetText(newText);
    }

    public static int RoundDown(this float f) => (int)Math.Floor(f);
    public static int RoundUp(this float f) => (int)Math.Ceiling(f);
}
