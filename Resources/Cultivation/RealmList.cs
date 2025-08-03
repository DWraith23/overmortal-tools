using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.Scripts;
using static OvermortalTools.Resources.Cultivation.Realm;

namespace OvermortalTools.Resources.Cultivation;

public static class RealmList
{
    public static List<Realm> Realms { get; set; } = [];

    public static void LoadRealms()
    {
        Realms =
        [
            new(MajorRealm.Novice, false, 2, 0, 0),
            new(MajorRealm.Connection, false, 16864, 0, 0),
            new(MajorRealm.Foundation, true, 28000, 57804, 201000),
            new(MajorRealm.Virtuoso, true, 219100, 465000, 1569300),
            new(MajorRealm.NascentSoul, true, 1818600, 3865034, 11134522),
            new(MajorRealm.Incarnation, true, 12158860, 25853565, 61792200),
            new(MajorRealm.Voidbreak, true, 68014736, 142093920, 307715252),
            new(MajorRealm.Wholeness, true, 186813569, 263951236, 304450560),
            new(MajorRealm.Perfection, true, 469855611, 690774727, 915460544),
            new(MajorRealm.Nirvana, true, 1159191947, 1499362923, 1916841693),
            new(MajorRealm.Celestial, true, 2923599212, 3856472370, 5215416662),
            new(MajorRealm.Eternal, true, 8427284639, 9849448914, 12813035198),
        ];
    }

    public static int RealmIndex(MajorRealm realm) => Realms.IndexOf(GetRealm(realm));

    public static Realm GetRealm(MajorRealm realm) => Realms.FirstOrDefault(r => r.Name == realm);
    public static long GetTotalRealmXp(MajorRealm realm) => GetRealm(realm).GetFullRealmXp();
    public static long GetMinorRealmXp(MajorRealm major, MinorRealm minor) => GetRealm(major).GetMinorRealmXp(minor);
    public static long GetXpToTarget(RealmInfo current, RealmInfo target)
    {
        if (current.Major == target.Major)
        {
            if (current.Minor == target.Minor)
            {
                return GetXpNeededToCompleteCurrent(current);
            }
            else
            {

                return GetXpNeededToCompleteTarget(current, target.Minor);
            }
        }
        else
        {
            return GetXpNeededToCompleteTarget(current, target);
        }
    }
    public static long SumCompletedXp(RealmInfo current)
    {
        var realm = GetRealm(current.Major);
        var completed = Realms[..RealmIndex(current.Major)];
        var partial = realm.GetXpCompleted(current.Minor, current.PercentComplete);
        var sum = completed.Select(r => r.GetFullRealmXp()).Sum() + partial;

        return sum;
    }


    private static long GetXpNeededToCompleteCurrent(RealmInfo current)
    {
        var total = GetMinorRealmXp(current.Major, current.Minor);
        return (total - (total * current.PercentComplete)).RoundDown();
    }

    private static long GetXpNeededToCompleteTarget(RealmInfo current, MinorRealm target)
    {
        var cXp = (GetMinorRealmXp(current.Major, current.Minor) * current.PercentComplete).RoundDown();
        MinorRealm[] realms = (current.Minor == MinorRealm.Early && target == MinorRealm.Late)
            ? [MinorRealm.Early, MinorRealm.Middle, MinorRealm.Late]
            : [current.Minor, target];
        return GetRealm(current.Major).SumMinorXp(realms) - cXp;
    }

    private static List<Realm> GetRealmsBetween(MajorRealm from, MajorRealm to)
    {
        if (from == to) return [];

        var start = GetRealm(from);
        var end = GetRealm(to);

        var startIndex = Realms.IndexOf(start);
        var endIndex = Realms.IndexOf(end);

        startIndex += 1;
        var length = endIndex - startIndex - 1;

        if (startIndex > endIndex || length < 1) return [];

        return Realms.Slice(startIndex, length);
    }

    private static long GetXpNeededToCompleteTarget(RealmInfo current, RealmInfo target)
    {
        var cRealm = GetRealm(current.Major);
        var cXp = cRealm.GetXpCompleted(current.Minor, current.PercentComplete);
        var betweenRealms = GetRealmsBetween(current.Major, target.Major);
        var tRealm = GetRealm(target.Major);

        var xpNeeded = tRealm.SumMinorXp(Realm.MinorRealmsTo[target.Minor]);
        foreach (var realm in betweenRealms)
        {
            xpNeeded += realm.GetFullRealmXp();
        }
        return xpNeeded - cXp;
    }

}
