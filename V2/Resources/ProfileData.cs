using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.Resources.Planner;
using OvermortalTools.Scripts;

namespace OvermortalTools.V2.Resources;

public partial class ProfileData : Resource
{


    #region Properties and Exports
    // Paths
    private PathData _path1 = new();
    private PathData _path2 = new();
    private PathData _path3 = new();
    private PathData _path4 = new();

    [Export]
    public PathData Path1
    {
        get => _path1;
        set
        {
            if (_path1 == value) return;
            _path1 = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                _path1.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    [Export]
    public PathData Path2
    {
        get => _path2;
        set
        {
            if (_path2 == value) return;
            _path2 = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                _path2.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    [Export]
    public PathData Path3
    {
        get => _path3;
        set
        {
            if (_path3 == value) return;
            _path3 = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                _path3.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    [Export]
    public PathData Path4
    {
        get => _path4;
        set
        {
            if (_path4 == value) return;
            _path4 = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                _path4.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    // Passive Cultivation
    private PassiveData _passiveCultivation = new();
    [Export]
    public PassiveData PassiveCultivation
    {
        get => _passiveCultivation;
        set
        {
            if (_passiveCultivation == value) return;
            _passiveCultivation = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                _passiveCultivation.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    // Pills
    private PillData _pillData = new();
    [Export]
    public PillData PillData
    {
        get => _pillData;
        set
        {
            if (_pillData == value) return;
            _pillData = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                _pillData.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    // Respira
    private RespiraData _respiraData = new();
    [Export]
    public RespiraData RespiraData
    {
        get => _respiraData;
        set
        {
            if (_respiraData == value) return;
            _respiraData = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                _respiraData.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    // Star Marks
    private StarMarksData _starMarks = new();
    [Export]
    public StarMarksData StarMarks
    {
        get => _starMarks;
        set
        {
            if (_starMarks == value) return;
            _starMarks = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                _starMarks.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    // Creation Artifacts
    private CreationArtifactData _creationArtifacts = new();
    [Export]
    public CreationArtifactData CreationArtifacts
    {
        get => _creationArtifacts;
        set
        {
            if (_creationArtifacts == value) return;
            _creationArtifacts = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                _creationArtifacts.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    // Target Realms
    [Export] public PathData.Realm TargetMajorRealm { get; set; } = PathData.Realm.Novice;
    [Export] public PathData.MinorRealm TargetMinorRealm { get; set; } = PathData.MinorRealm.Early;

    #endregion


    #region Calculated Properties

    public (PathData.Realm, PathData.MinorRealm) HighestRealm => (GetHighestRealmInformation().Item1, GetHighestRealmInformation().Item2);

    public long DailyPassiveExp => PassiveCultivation.GetDailyCosmoapsisExp(HighestRealm) + PassiveCultivation.GetDailyAuraGemExp(HighestRealm);

    public long DailyPillExp =>
            PillData.GetTotalRareValue(HighestRealm.Item1, StarMarks.RarePills * 0.1f) +
            PillData.GetTotalEpicValue(HighestRealm.Item1, StarMarks.EpicPills * 0.1f) +
            PillData.GetTotalLegendaryValue(HighestRealm.Item1, StarMarks.LegendaryPills * 0.1f) +
            PillData.GetTotalMythicValue(HighestRealm.Item1, StarMarks.RespiraExp * 0.1f);

    public long DailyRespiraExp => RespiraData.GetDailyRespiraValue(HighestRealm.Item1) * (long)Math.Floor(1 + StarMarks.RespiraExp * 0.1f);


    public long TotalDailyExp => DailyPassiveExp + DailyPillExp + DailyRespiraExp;

    #endregion


    #region Methods

    public (PathData.Realm, PathData.MinorRealm, float) GetHighestRealmInformation()
    {
        var paths = new List<PathData> { Path1, Path2, Path3, Path4 };
        var highest = paths
            .Where(p => p != null)
            .Select(p => (p.CurrentRealm, p.CurrentMinorRealm, p.CurrentRealmProgress))
            .OrderByDescending(t => t.CurrentRealm)
            .ThenByDescending(t => t.CurrentMinorRealm)
            .ThenByDescending(t => t.CurrentRealmProgress)
            .FirstOrDefault();
        return highest;
    }

    public IOrderedEnumerable<PathData> GetPathsInOrder()
    {
        var paths = new List<PathData> { Path1, Path2, Path3, Path4 };
        var ordered = paths
            .Where(p => p != null)
            .OrderByDescending(p => p.CurrentRealm)
            .ThenByDescending(p => p.CurrentMinorRealm)
            .ThenByDescending(p => p.CurrentRealmProgress);
        return ordered;
    }


    #endregion

    public ProfileData()
    {
        Path1.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        Path2.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        Path3.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        Path4.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        PassiveCultivation.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        PillData.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        RespiraData.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        StarMarks.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        CreationArtifacts.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
    }

}
