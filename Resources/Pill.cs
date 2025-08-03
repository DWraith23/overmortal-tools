using System;
using System.Collections.Generic;
using Godot;
using OvermortalTools.Resources.Cultivation;
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

    private static Dictionary<Realm.MajorRealm, int> BaseRealmValues => new()
    {
        { Realm.MajorRealm.Novice, 0 },
        { Realm.MajorRealm.Connection, 125 },
        { Realm.MajorRealm.Foundation, 625 },
        { Realm.MajorRealm.Virtuoso, 1900 },
        { Realm.MajorRealm.NascentSoul, 5000 },
        { Realm.MajorRealm.Incarnation, 8000 },
        { Realm.MajorRealm.Voidbreak, 12000 },
        { Realm.MajorRealm.Wholeness, 20500 },
        { Realm.MajorRealm.Perfection,  31000 },
        { Realm.MajorRealm.Nirvana, 57000 },
        { Realm.MajorRealm.Celestial, 128375 },
        { Realm.MajorRealm.Eternal, 304375 },
    };

    [Export] public Realm.MajorRealm CultivationRealm { get; set; } = Realm.MajorRealm.Connection;
    [Export] public Quality.Classification PillQuality { get; set; } = Quality.Classification.Common;

    public string PillName => $"{PillQuality} {CultivationRealm} Pill";
    public long PillValue => (int) Math.Floor(BaseRealmValues[CultivationRealm] * QualityMultipliers[PillQuality]);
    public long GetValue(float multiplier) => (int) Math.Floor(PillValue * multiplier);

    public static Pill[] GeneratePills()
    {
        List<Pill> result = [];

        foreach (var realm in Enum.GetValues<Realm.MajorRealm>())
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
