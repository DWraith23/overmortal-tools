using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using OvermortalTools.Scripts;

namespace OvermortalTools.Resources.Cultivation;

public partial class Realm : Resource
{
    public enum MajorRealm
    {
        Novice,
        Connection,
        Foundation,
        Virtuoso,
        NascentSoul,
        Incarnation,
        Voidbreak,
        Wholeness,
        Perfection,
        Nirvana,
        Celestial,
        Eternal,
    }

    public static readonly Dictionary<MajorRealm, string> Names = new()
    {
            { MajorRealm.Novice, "Novice" },
            { MajorRealm.Connection, "Connection" },
            { MajorRealm.Foundation, "Foundation" },
            { MajorRealm.Virtuoso, "Virtuoso" },
            { MajorRealm.NascentSoul, "Nascent Soul" },
            { MajorRealm.Incarnation, "Incarnation" },
            { MajorRealm.Voidbreak, "Voidbreak" },
            { MajorRealm.Wholeness, "Wholeness" },
            { MajorRealm.Perfection, "Perfection" },
            { MajorRealm.Nirvana, "Nirvana" },
            { MajorRealm.Celestial, "Celestial" },
            { MajorRealm.Eternal, "Eternal" },
    };

    public static List<string> NamesList =>
    [
            "Novice",
            "Connection",
            "Foundation",
            "Virtuoso",
            "Nascent Soul",
            "Incarnation",
            "Voidbreak",
            "Wholeness",
            "Perfection",
            "Nirvana",
            "Celestial",
            "Eternal",
    ];
    public enum MinorRealm
    {
        Early,
        Middle,
        Late
    }

    public Dictionary<MinorRealm, long> MinorRealmXp => new()
    {
        { MinorRealm.Early, EarlyTotalXp },
        { MinorRealm.Middle, MiddleTotalXp },
        { MinorRealm.Late, LateTotalXp }
    };

    public static Dictionary<MinorRealm, MinorRealm[]> MinorRealmsTo => new()
    {
        { MinorRealm.Early, [MinorRealm.Early] },
        { MinorRealm.Middle, [MinorRealm.Early, MinorRealm.Middle] },
        { MinorRealm.Late, [MinorRealm.Early, MinorRealm.Middle, MinorRealm.Late] }
    };

    public static Dictionary<MinorRealm, MinorRealm[]> MinorRealmsFrom => new()
    {
        { MinorRealm.Early, [MinorRealm.Early, MinorRealm.Middle, MinorRealm.Late] },
        { MinorRealm.Middle, [MinorRealm.Middle, MinorRealm.Late] },
        { MinorRealm.Late, [MinorRealm.Late] }
    };

    public struct RealmInfo(MajorRealm major, MinorRealm minor, float percentComplete = 1f)
    {
        public MajorRealm Major = major;
        public MinorRealm Minor = minor;
        public float PercentComplete = percentComplete;

        public override string ToString() =>
            $"{Major} {Minor} {PercentComplete:P1}";
    }

    private MajorRealm _name;
    private bool _usesMinorRealms = true;
    private long _earlyTotalXp = 0;
    private long _middleTotalXp = 0;
    private long _lateTotalXp = 0;

    [Export]
    public MajorRealm Name
    {
        get => _name;
        set
        {
            if (_name == value) return;
            _name = value;
            EmitChanged();
        }
    }


    [Export]
    public bool UsesMinorRealms
    {
        get => _usesMinorRealms;
        set
        {
            if (_usesMinorRealms == value) return;
            _usesMinorRealms = value;
            EmitChanged();
        }
    }

    [Export]
    public long EarlyTotalXp
    {
        get => _earlyTotalXp;
        set
        {
            if (_earlyTotalXp == value) return;
            _earlyTotalXp = value;
            EmitChanged();
        }
    }

    [Export]
    public long MiddleTotalXp
    {
        get => _middleTotalXp;
        set
        {
            if (_middleTotalXp == value) return;
            _middleTotalXp = value;
            EmitChanged();
        }
    }

    [Export]
    public long LateTotalXp
    {
        get => _lateTotalXp;
        set
        {
            if (_lateTotalXp == value) return;
            _lateTotalXp = value;
            EmitChanged();
        }
    }

    public Realm(
        MajorRealm name,
        bool usesMinorRealms,
        long earlyTotalXp,
        long middleTotalXp,
        long lateTotalXp
    )
    {
        Name = name;
        UsesMinorRealms = usesMinorRealms;
        EarlyTotalXp = earlyTotalXp;
        MiddleTotalXp = middleTotalXp;
        LateTotalXp = lateTotalXp;
    }


    public long GetRemainingXp(float percentComplete, MinorRealm realm) =>
        (long)Math.Floor(MinorRealmXp[realm] - (MinorRealmXp[realm] * percentComplete));

    public long GetMinorRealmXp(MinorRealm realm) => MinorRealmXp[realm];

    public long GetFullRealmXp() =>
        EarlyTotalXp + MiddleTotalXp + LateTotalXp;

    public long GetXpCompleted(MinorRealm realm, float percentComplete)
    {
        if (realm == MinorRealm.Early) return (GetMinorRealmXp(realm) * percentComplete).RoundDown();
        else if (realm == MinorRealm.Middle)
        {
            var early = GetMinorRealmXp(MinorRealm.Early);
            return early + (GetMinorRealmXp(realm) * percentComplete).RoundDown();
        }
        else
        {
            var early = GetMinorRealmXp(MinorRealm.Early);
            var middle = GetMinorRealmXp(MinorRealm.Middle);
            return early + middle + (GetMinorRealmXp(realm) * percentComplete).RoundDown();
        }
    }

    public long SumMinorXp(MinorRealm[] realms) =>
        realms.Select(r => MinorRealmXp[r]).Sum();

    

}
