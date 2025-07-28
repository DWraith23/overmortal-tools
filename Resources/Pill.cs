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

    private static Dictionary<Realm.Classification, int> BaseRealmValues => new()
    {
        { Realm.Classification.Novice, 0 },
        { Realm.Classification.Connection, 125 },
        { Realm.Classification.Foundation, 625 },
        { Realm.Classification.Virtuoso, 1900 },
        { Realm.Classification.NascentSoul, 5000 },
        { Realm.Classification.Incarnation, 8000 },
        { Realm.Classification.Voidbreak, 12000 },
        { Realm.Classification.Wholeness, 20500 },
        { Realm.Classification.Perfection,  31000 },
        { Realm.Classification.Nirvana, 57000 },
        { Realm.Classification.Celestial, 128375 },
        { Realm.Classification.Eternal, 304375 },
    };

    [Export] public Realm.Classification CultivationRealm { get; set; } = Realm.Classification.Connection;
    [Export] public Quality.Classification PillQuality { get; set; } = Quality.Classification.Common;

    public string PillName => $"{PillQuality} {CultivationRealm} Pill";
    public long PillValue => (int) Math.Floor(BaseRealmValues[CultivationRealm] * QualityMultipliers[PillQuality]);
    public long GetValue(float multiplier) => (int) Math.Floor(PillValue * multiplier);

    public static Pill[] GeneratePills()
    {
        List<Pill> result = [];

        foreach (var realm in Enum.GetValues<Realm.Classification>())
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
