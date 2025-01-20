using System;
using System.Collections.Generic;
using Godot;
using OvermortalTools.Scripts.Enums;

namespace OvermortalTools.Resources;

[GlobalClass, Tool]
public partial class Pill : Consumable
{
    public enum Quality
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic
    }

    public enum Realm
    {
        Connection,
        Foundation,
        Virtuoso,
        NascentSoul,
        Incarnation,
        Voidbreak,
        Wholeness
    }

    private static Dictionary<Quality, float> QualityMultipliers => new()
    {
        { Quality.Common, 1f },
        { Quality.Uncommon, 2f },
        { Quality.Rare, 3.2f },
        { Quality.Epic, 6f },
        { Quality.Legendary, 12f },
        { Quality.Mythic, 24f },
    };

    private static Dictionary<Realm, int> BaseRealmValues => new()
    {
        { Realm.Connection, 125 },
        { Realm.Foundation, 625 },
        { Realm.Virtuoso, 1900 },
        { Realm.NascentSoul, 5000 },
        { Realm.Incarnation, 8000 },
        { Realm.Voidbreak, 12000 },
        { Realm.Wholeness, 20500 },
    };

    [Export] public Realm CultivationRealm { get; set; } = Realm.Connection;
    [Export] public Quality PillQuality { get; set; } = Quality.Common;

    public string PillName => $"{PillQuality} {CultivationRealm} Pill";
    public int PillValue => (int) Math.Floor(BaseRealmValues[CultivationRealm] * QualityMultipliers[PillQuality]);
    public int GetValue(float multiplier) => (int) Math.Floor(PillValue * multiplier);

    public static Pill[] GeneratePills()
    {
        List<Pill> result = [];

        foreach (var realm in Enum.GetValues<Realm>())
        {
            foreach(var quality in Enum.GetValues<Quality>())
            {
                result.Add(
                    new Pill()
                    {
                        CultivationRealm = realm,
                        PillQuality = quality
                    }
                );
            }
        }

        return [.. result];
    }

}
