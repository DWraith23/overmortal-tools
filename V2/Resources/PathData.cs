using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.Scripts;

namespace OvermortalTools.V2.Resources;

public partial class PathData : Resource
{
    #region Enums and Static Data
    public enum Path
    {
        None,
        Corporia,
        Magicka,
        Swordia,
        Ghostia,
        Literatia,
    }

    public enum Realm
    {
        None,
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

    public enum MinorRealm
    {
        None,
        Early,
        Middle,
        Late
    }

    public static readonly Dictionary<Path, string> PathNames = new()
    {
        { Path.None, "None" },
        { Path.Corporia, "Corporia" },
        { Path.Magicka, "Magicka" },
        { Path.Swordia, "Swordia" },
        { Path.Ghostia, "Ghostia" },
        { Path.Literatia, "Literatia" },
    };

    public static readonly Dictionary<Realm, string> RealmNames = new()
    {
        { Realm.None, "None" },
        { Realm.Novice, "Novice" },
        { Realm.Connection, "Connection" },
        { Realm.Foundation, "Foundation" },
        { Realm.Virtuoso, "Virtuoso" },
        { Realm.NascentSoul, "Nascent Soul" },
        { Realm.Incarnation, "Incarnation" },
        { Realm.Voidbreak, "Voidbreak" },
        { Realm.Wholeness, "Wholeness" },
        { Realm.Perfection, "Perfection" },
        { Realm.Nirvana, "Nirvana" },
        { Realm.Celestial, "Celestial" },
        { Realm.Eternal, "Eternal" },
    };

    public static readonly Dictionary<MinorRealm, string> MinorRealmNames = new()
    {
        { MinorRealm.None, "None" },
        { MinorRealm.Early, "Early" },
        { MinorRealm.Middle, "Middle" },
        { MinorRealm.Late, "Late" }
    };

    public static readonly Dictionary<(Realm, MinorRealm), long> ExperienceReqs = new()
    {
        { (Realm.None, MinorRealm.Early), 0 },
        { (Realm.Novice, MinorRealm.Early), 2 },
        { (Realm.Connection, MinorRealm.Early),  16864 },
        { (Realm.Foundation, MinorRealm.Early), 28000 },
        { (Realm.Foundation, MinorRealm.Middle), 57804 },
        { (Realm.Foundation, MinorRealm.Late), 201000 },
        { (Realm.Virtuoso, MinorRealm.Early), 219100 },
        { (Realm.Virtuoso, MinorRealm.Middle), 465000 },
        { (Realm.Virtuoso, MinorRealm.Late), 1569300 },
        { (Realm.NascentSoul, MinorRealm.Early), 1818600 },
        { (Realm.NascentSoul, MinorRealm.Middle), 3865034 },
        { (Realm.NascentSoul, MinorRealm.Late), 11134522 },
        { (Realm.Incarnation, MinorRealm.Early), 12158860 },
        { (Realm.Incarnation, MinorRealm.Middle), 25853565 },
        { (Realm.Incarnation, MinorRealm.Late), 61792200 },
        { (Realm.Voidbreak, MinorRealm.Early), 68014736 },
        { (Realm.Voidbreak, MinorRealm.Middle), 142093920 },
        { (Realm.Voidbreak, MinorRealm.Late), 307715252 },
        { (Realm.Wholeness, MinorRealm.Early), 186813569 },
        { (Realm.Wholeness, MinorRealm.Middle), 263951236 },
        { (Realm.Wholeness, MinorRealm.Late), 304450560 },
        { (Realm.Perfection, MinorRealm.Early), 469855611 },
        { (Realm.Perfection, MinorRealm.Middle), 690774727 },
        { (Realm.Perfection, MinorRealm.Late), 915460544 },
        { (Realm.Nirvana, MinorRealm.Early), 1159191947 },
        { (Realm.Nirvana, MinorRealm.Middle), 1499362923 },
        { (Realm.Nirvana, MinorRealm.Late), 1916841693 },
        { (Realm.Celestial, MinorRealm.Early), 2923599212 },
        { (Realm.Celestial, MinorRealm.Middle), 3856472370 },
        { (Realm.Celestial, MinorRealm.Late), 5215416662 },
        { (Realm.Eternal, MinorRealm.Early), 8427284639 },
        { (Realm.Eternal, MinorRealm.Middle), 9849448914 },
        { (Realm.Eternal, MinorRealm.Late), 12813035198 },
    };

    public static long GetRealmExpReq(Realm realm, MinorRealm minorRealm) =>
        ExperienceReqs.TryGetValue((realm, minorRealm), out var req) ? req : 0;


    #endregion


    #region Exports

    private Path _selectedPath = Path.None;
    private Realm _currentRealm = Realm.None;
    private MinorRealm _currentMinorRealm = MinorRealm.Early;
    private float _currentRealmProgress = 0f;

    [Export]
    public Path SelectedPath
    {
        get => _selectedPath;
        set
        {
            if (_selectedPath == value) return;
            _selectedPath = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public Realm CurrentRealm
    {
        get => _currentRealm;
        set
        {
            if (_currentRealm == value) return;
            _currentRealm = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public MinorRealm CurrentMinorRealm
    {
        get => _currentMinorRealm;
        set
        {
            if (_currentMinorRealm == value) return;
            _currentMinorRealm = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float CurrentRealmProgress
    {
        get => _currentRealmProgress;
        set
        {
            if (_currentRealmProgress == value || value < 0f) return;
            _currentRealmProgress = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    #endregion

    public long GetTotalXp() =>
        ExperienceReqs
            .Where(kv => kv.Key.Item1 < CurrentRealm ||
                         (kv.Key.Item1 == CurrentRealm && kv.Key.Item2 < CurrentMinorRealm))
            .Sum(kv => kv.Value) +
        (long)(CurrentRealmProgress * ExperienceReqs[(CurrentRealm, CurrentMinorRealm)]);

    public long GetXpToTargetRealm(Realm targetRealm, MinorRealm targetMinorRealm)
    {
        if (targetRealm < CurrentRealm ||
            (targetRealm == CurrentRealm && targetMinorRealm <= CurrentMinorRealm))
            return 0;

        var totalXp = GetTotalXp();

        var targetXp = ExperienceReqs
            .Where(kv => kv.Key.Item1 < targetRealm ||
                         (kv.Key.Item1 == targetRealm && kv.Key.Item2 <= targetMinorRealm))
            .Sum(kv => kv.Value);

        return targetXp - totalXp;
    }

    public Realm[] GetLowerRealms() =>
        [.. Enum.GetValues<Realm>().Where(r => r < CurrentRealm)];

    public MinorRealm[] GetPassedMinorRealms() =>
        [.. Enum.GetValues<MinorRealm>().Where(mr => mr < CurrentMinorRealm)];

    public string[] GetLowerRealmsStrings() =>
        [.. GetLowerRealms().Select(r => RealmNames[r])];

    public string[] GetPassedMinorRealmsStrings() =>
        [.. GetPassedMinorRealms().Select(mr => MinorRealmNames[mr])];

    public int GetSelectedPathIndex() =>
        Array.IndexOf(Enum.GetValues<Path>(), SelectedPath);

    public int GetCurrentRealmIndex() =>
        Array.IndexOf(Enum.GetValues<Realm>(), CurrentRealm);

    public int GetCurrentMinorRealmIndex() =>
        Array.IndexOf(Enum.GetValues<MinorRealm>(), CurrentMinorRealm);


}
