using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace OvermortalTools.Scripts;

public static class Tools
{

    public static int GetRandomIndex(List<float> odds)
    {
        var sum = odds.Sum();
        var rand = new Random().NextDouble();
        var total = 0f;
        odds.Sort();
        var i = 0;

        foreach (var odd in odds)
        {
            total += odd / sum;
            if (rand < total) return i;
            i++;
        }
        return -1;
    }

    /// <summary>
    /// Returns a random Key from a dictionary of Key Value Pairs including an object and a float.
    /// The floats are normalized to make their values total 1.
    /// </summary>
    /// <typeparam name="T">The object type to be returned</typeparam>
    /// <param name="odds">A dictionary containing objects and weighted odds</param>
    /// <returns>A random object from the dictionary based on weights</returns>
    public static T GetRandomKey<T>(Dictionary<T, float> odds)
    {
        var sum = odds.Values.Sum();
        if (sum == 0f)
        {
            throw new Exception("Error, no weights assigned.");
        }

        var rand = new Random().NextDouble();
        var total = 0f;
        var sorted = odds.ToImmutableSortedDictionary();

        foreach (var kvp in sorted)
        {
            total += kvp.Value / sum;
            if (rand < total) return kvp.Key;
        }
        return default;
    }

    public static void EmitLoggedSignal(GodotObject obj, StringName signal, params Variant[] args)
    {
        string signalStr = signal;
        string argsStr = string.Join(", ", args.Select(a => a.ToString()));
        string objStr = obj.ToString();
        GD.PrintRich($"{DateTime.Now} : [color=green]{objStr}[/color] emitting [color=yellow]{signalStr}[/color] with args [color=light_blue]{argsStr}");
        obj.EmitSignal(signal, args);
    }
}
