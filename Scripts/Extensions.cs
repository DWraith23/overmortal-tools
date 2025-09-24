using System;
using System.Linq;
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

    public static long RoundDown(this float f) => (long)Math.Floor(f);
    public static long RoundUp(this float f) => (long)Math.Ceiling(f);

    public static long RoundDown(this double d) => (long)Math.Floor(d);
    public static long RoundUp(this double d) => (long)Math.Ceiling(d);
}
