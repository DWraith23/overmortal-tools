using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.Scripts.Enums;

namespace OvermortalTools.Resources;

public partial class Elixir : Resource
{
    public static Dictionary<Realm.Classification, int> MonspiritiaElixirsCount => new()
    {
        { Realm.Classification.Foundation, 12 },
        { Realm.Classification.Virtuoso, 27 },
        { Realm.Classification.NascentSoul, 78 },
        { Realm.Classification.Incarnation, 176 },
    };

    private static Dictionary<Realm.Classification, int> MonspiritiaElixirsValue => new()
    {
        { Realm.Classification.Foundation, 2600 },
        { Realm.Classification.Virtuoso, 12400 },
        { Realm.Classification.NascentSoul, 44000 },
        { Realm.Classification.Incarnation, 57000 },
    };

    private static Dictionary<Realm.Classification, List<(int, float)>> MonspiritiaElixirsMultipliers => new()
    {
        { Realm.Classification.Foundation, [ (5, 1.5f), (10, 1f) ] },
        { Realm.Classification.Virtuoso, [ (5, 1.5f), (10, 1f), (15, 0.8f) ] },
        { Realm.Classification.NascentSoul, [ (5, 1.5f), (10, 1.2f), (20, 1f), (40, 0.8f), (80, 0.7f) ] },
        { Realm.Classification.Incarnation, [ (10, 1.5f), (20, 1.2f), (40, 1f), (80, 0.8f), (160, 0.7f) ] },
    };

    private static Dictionary<Realm.Classification, int> MainPathElixirsValue => new()
    {
        { Realm.Classification.Novice, 0 },
        { Realm.Classification.Connection, 350 },
        { Realm.Classification.Foundation, 2800 },
        { Realm.Classification.Virtuoso, 8800 },
        { Realm.Classification.NascentSoul, 24000 },
        { Realm.Classification.Incarnation, 38000 },
        { Realm.Classification.Voidbreak, 60000 },
        { Realm.Classification.Wholeness, 84000 },
        { Realm.Classification.Perfection, 128000 },
        { Realm.Classification.Nirvana, 180000 },
        // { Realm.Classification.Celestial, 368000 },
    };

    private static Dictionary<Realm.Classification, List<(int, float)>> MainPathElixirsMultipliers => new()
    {
        {  Realm.Classification.Novice, [ ] },
        {  Realm.Classification.Connection, [ (5, 1.5f), (15, 1f), ] },
        {  Realm.Classification.Foundation, [ (5, 1.5f), (10, 1f), (20, 0.8f), ] },
        {  Realm.Classification.Virtuoso, [ (5, 1.5f), (10, 1f), (15, .8f), (30, .6f), ] },
        {  Realm.Classification.NascentSoul, [ (5, 1.5f), (10, 1.2f), (20, 1f), (50, .7f), ] },
        {  Realm.Classification.Incarnation, [ (20, 1.5f), (40, 1.2f), (80, 1f), (160, .7f), ] },
        {  Realm.Classification.Voidbreak, [ (20, 1.5f), (40, 1.2f), (80, 1f), (120, .8f), (160, .6f), (240, .4f), (320, 0.3f), ] },
        {  Realm.Classification.Wholeness, [ (20, 1.5f), (40, 1.2f), (80, 1f), (120, .8f), (160, .6f), (240, .4f), (320, 0.3f), ] },
        {  Realm.Classification.Perfection, [ (20, 1.5f), (40, 1.2f), (80, 1f), (120, .8f), (160, .6f), (240, .4f), (320, 0.3f), ] },
        {  Realm.Classification.Nirvana, [ (20, 1.5f), (40, 1.2f), (80, 1f), (120, .8f), (160, .6f), (240, .4f), (320, 0.3f), ] },

    };

    public static long GetMonspiritiaElixirValue(Realm.Classification realm, int used)
    {
        if (!MonspiritiaElixirsValue.TryGetValue(realm, out int value)) return 0;   // If the realm is not found, return 0
        float multiplier = GetElixirMultiplier(MonspiritiaElixirsMultipliers[realm], used);
        return (int)Math.Floor(value * multiplier);
    }

    public static long GetMainPathElixirValue(Realm.Classification realm, int used)
    {
        if (!MainPathElixirsValue.TryGetValue(realm, out int value)) return 0;  // If the realm is not found, return 0
        float multiplier = GetElixirMultiplier(MainPathElixirsMultipliers[realm], used);
        return (int)Math.Floor(value * multiplier);
    }

    public static long GetMonspiritiaTotalValue(Realm.Classification realm, int used, int planned)
    {
        if (!MonspiritiaElixirsCount.ContainsKey(realm)) return 0;  // If the realm is not found, return 0

        long value = 0;
        for (int i = 0; i < planned; i++)
        {
            value += GetMonspiritiaElixirValue(realm, used + i);
        }
        return value;
    }

    public static long GetMainPathTotalValue(Realm.Classification realm, int used, int planned)
    {
        if (!MainPathElixirsValue.ContainsKey(realm)) return 0;  // If the realm is not found, return 0

        long value = 0;
        for (int i = 0; i < planned; i++)
        {
            value += GetMainPathElixirValue(realm, used + i);
        }
        return value;
    }

    private static float GetElixirMultiplier (List<(int, float)> multipliers, long used)
    {
        var multiplier = 0f;
        var added = 0;
        foreach (var (threshold, value) in multipliers)
        {
            added += threshold;
            if (used <= added)
            {
                return value;
            }
        }
        return multiplier;
    }
}
