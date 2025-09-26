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

    public static string FormatLargeNumber(this long number)
    {
        if (number < 10000) return number.ToString("N0");
        if (number < 1000000) return $"{number / 1000f:N2}K";
        if (number < 1_000_000_000) return $"{number / 1_000_000f:N2}M";
        if (number < 1_000_000_000_000L) return $"{number / 1_000_000_000d:N2}B";
        if (number < 1_000_000_000_000_000L) return $"{number / 1_000_000_000_000d:N2}T";
        return $"{number / 1_000_000_000_000_000d:N2}Q";
    }
}
