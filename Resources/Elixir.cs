using System;
using System.Collections.Generic;
using Godot;
using OvermortalTools.Resources.Cultivation;

namespace OvermortalTools.Resources;

public partial class Elixir : Resource
{
    public static Dictionary<Realm.MajorRealm, int> MonspiritiaElixirsCount => new()
    {
        { Realm.MajorRealm.Foundation, 12 },
        { Realm.MajorRealm.Virtuoso, 27 },
        { Realm.MajorRealm.NascentSoul, 78 },
        { Realm.MajorRealm.Incarnation, 176 },
    };

    private static Dictionary<Realm.MajorRealm, int> MonspiritiaElixirsValue => new()
    {
        { Realm.MajorRealm.Foundation, 2600 },
        { Realm.MajorRealm.Virtuoso, 12400 },
        { Realm.MajorRealm.NascentSoul, 44000 },
        { Realm.MajorRealm.Incarnation, 57000 },
    };

    private static Dictionary<Realm.MajorRealm, List<(int, float)>> MonspiritiaElixirsMultipliers => new()
    {
        { Realm.MajorRealm.Foundation, [ (5, 1.5f), (10, 1f) ] },
        { Realm.MajorRealm.Virtuoso, [ (5, 1.5f), (10, 1f), (15, 0.8f) ] },
        { Realm.MajorRealm.NascentSoul, [ (5, 1.5f), (10, 1.2f), (20, 1f), (40, 0.8f), (80, 0.7f) ] },
        { Realm.MajorRealm.Incarnation, [ (10, 1.5f), (20, 1.2f), (40, 1f), (80, 0.8f), (160, 0.7f) ] },
    };

    private static Dictionary<Realm.MajorRealm, int> MainPathElixirsValue => new()
    {
        { Realm.MajorRealm.Novice, 0 },
        { Realm.MajorRealm.Connection, 350 },
        { Realm.MajorRealm.Foundation, 2800 },
        { Realm.MajorRealm.Virtuoso, 8800 },
        { Realm.MajorRealm.NascentSoul, 24000 },
        { Realm.MajorRealm.Incarnation, 38000 },
        { Realm.MajorRealm.Voidbreak, 60000 },
        { Realm.MajorRealm.Wholeness, 84000 },
        { Realm.MajorRealm.Perfection, 128000 },
        { Realm.MajorRealm.Nirvana, 180000 },
        // { Realm.MajorRealm.Celestial, 368000 },
    };

    private static Dictionary<Realm.MajorRealm, List<(int, float)>> MainPathElixirsMultipliers => new()
    {
        {  Realm.MajorRealm.Novice, [ ] },
        {  Realm.MajorRealm.Connection, [ (5, 1.5f), (15, 1f), ] },
        {  Realm.MajorRealm.Foundation, [ (5, 1.5f), (10, 1f), (20, 0.8f), ] },
        {  Realm.MajorRealm.Virtuoso, [ (5, 1.5f), (10, 1f), (15, .8f), (30, .6f), ] },
        {  Realm.MajorRealm.NascentSoul, [ (5, 1.5f), (10, 1.2f), (20, 1f), (50, .7f), ] },
        {  Realm.MajorRealm.Incarnation, [ (20, 1.5f), (40, 1.2f), (80, 1f), (160, .7f), ] },
        {  Realm.MajorRealm.Voidbreak, [ (20, 1.5f), (40, 1.2f), (80, 1f), (120, .8f), (160, .6f), (240, .4f), (320, 0.3f), ] },
        {  Realm.MajorRealm.Wholeness, [ (20, 1.5f), (40, 1.2f), (80, 1f), (120, .8f), (160, .6f), (240, .4f), (320, 0.3f), ] },
        {  Realm.MajorRealm.Perfection, [ (20, 1.5f), (40, 1.2f), (80, 1f), (120, .8f), (160, .6f), (240, .4f), (320, 0.3f), ] },
        {  Realm.MajorRealm.Nirvana, [ (20, 1.5f), (40, 1.2f), (80, 1f), (120, .8f), (160, .6f), (240, .4f), (320, 0.3f), ] },

    };

    public static long GetMonspiritiaElixirValue(Realm.MajorRealm realm, int used)
    {
        if (!MonspiritiaElixirsValue.TryGetValue(realm, out int value)) return 0;   // If the realm is not found, return 0
        float multiplier = GetElixirMultiplier(MonspiritiaElixirsMultipliers[realm], used);
        return (int)Math.Floor(value * multiplier);
    }

    public static long GetMainPathElixirValue(Realm.MajorRealm realm, int used)
    {
        if (!MainPathElixirsValue.TryGetValue(realm, out int value)) return 0;  // If the realm is not found, return 0
        float multiplier = GetElixirMultiplier(MainPathElixirsMultipliers[realm], used);
        return (int)Math.Floor(value * multiplier);
    }

    public static long GetMonspiritiaTotalValue(Realm.MajorRealm realm, int used, int planned)
    {
        if (!MonspiritiaElixirsCount.ContainsKey(realm)) return 0;  // If the realm is not found, return 0

        long value = 0;
        for (int i = 0; i < planned; i++)
        {
            value += GetMonspiritiaElixirValue(realm, used + i);
        }
        return value;
    }

    public static long GetMainPathTotalValue(Realm.MajorRealm realm, int used, int planned)
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
