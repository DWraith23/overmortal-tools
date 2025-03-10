using System;
using System.Collections.Generic;
using Godot;
using OvermortalTools.Scripts.Enums;

namespace OvermortalTools.Resources;

[GlobalClass, Tool]
public partial class Pill : Resource
{
    private static Dictionary<Quality.Classification, float> QualityMultipliers => new()
    {
        { Quality.Classification.Common, 1f },
        { Quality.Classification.Uncommon, 2f },
        { Quality.Classification.Rare, 3.2f },
        { Quality.Classification.Epic, 6f },
        { Quality.Classification.Legendary, 12f },
        { Quality.Classification.Mythic, 24f },
    };

    private static Dictionary<Realm, int> BaseRealmValues => new()
    {
        { Realm.Novice, 0 },
        { Realm.Connection, 125 },
        { Realm.Foundation, 625 },
        { Realm.Virtuoso, 1900 },
        { Realm.NascentSoul, 5000 },
        { Realm.Incarnation, 8000 },
        { Realm.Voidbreak, 12000 },
        { Realm.Wholeness, 20500 },
        { Realm.Perfection,  31000 },
        { Realm.Nirvana, 57000 },
    };

    [Export] public Realm CultivationRealm { get; set; } = Realm.Connection;
    [Export] public Quality.Classification PillQuality { get; set; } = Quality.Classification.Common;

    public string PillName => $"{PillQuality} {CultivationRealm} Pill";
    public int PillValue => (int) Math.Floor(BaseRealmValues[CultivationRealm] * QualityMultipliers[PillQuality]);
    public int GetValue(float multiplier) => (int) Math.Floor(PillValue * multiplier);

    public static Pill[] GeneratePills()
    {
        List<Pill> result = [];

        foreach (var realm in Enum.GetValues<Realm>())
        {
            foreach(var quality in Enum.GetValues<Quality.Classification>())
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
