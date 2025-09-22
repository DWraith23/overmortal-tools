using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.Scripts;

namespace OvermortalTools.V2.Resources;

public partial class CreationArtifactData : Resource
{
    private static Dictionary<int, float> RechargeValues => new()
    {
        { 0, 1f },
        { 1, 1.3f },
        { 2, 1.6f },
        { 3, 2f },
        { 4, 2.4f },
        { 5, 3f }
    };

    private bool _hasVase = false;
    private bool _hasMirror = false;

    private int _vaseStars = 0;
    private int _mirrorStars = 0;

    private bool _hasVaseTransmog = false;

    [Export]
    public bool HasVase
    {
        get => _hasVase;
        set
        {
            if (_hasVase == value) return;
            _hasVase = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);

            if (!value)
            {
                HasVaseTransmog = false;
                VaseStars = 0;
            }
        }
    }

    [Export]
    public bool HasMirror
    {
        get => _hasMirror;
        set
        {
            if (_hasMirror == value) return;
            _hasMirror = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);

            if (!value)
            {
                MirrorStars = 0;
            }
        }
    }

    [Export]
    public int VaseStars
    {
        get => _vaseStars;
        set
        {
            if (_vaseStars == value) return;
            _vaseStars = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public int MirrorStars
    {
        get => _mirrorStars;
        set
        {
            if (_mirrorStars == value) return;
            _mirrorStars = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public bool HasVaseTransmog
    {
        get => _hasVaseTransmog;
        set
        {
            if (_hasVaseTransmog == value) return;
            _hasVaseTransmog = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    private float GetVaseMythicPills()
    {
        if (!HasVase) return 0f;

        var energy = 100f + (96f * RechargeValues[VaseStars]);  // Base daily energy with recharge
        var basic = energy / 100f;  // Base cost (assuming not using purple or gold)
        return VaseStars == 5 ? basic * 1.15f : basic;  // At 5*, 15% chance of duplicating pills.
    }

    private float GetMirrorMythicPills()
    {
        if (!HasMirror) return 0f;

        var energy = 100f + (96f * RechargeValues[MirrorStars]);
        var efficiencyMultiplier =
            MirrorStars == 0 ? 1f
                : MirrorStars < 3 ? 0.95f // 1* mirror has 5% cost reduction
                : 0.9f; // 3* mirror has 10% cost reduction
        var duplicationMultiplier = MirrorStars == 5 ? 1.15f : 1f; // 5* mirror has 15% chance of double duplication
        var cost = 200 * efficiencyMultiplier;
        return energy / cost * duplicationMultiplier;
    }

    private float GetVaseXpMultiplier()
    {
        var bonus = HasVaseTransmog ? 0.08f : 0.0f;
        if (VaseStars == 0) return 1f + bonus;  // No bonus
        if (VaseStars < 3) return 1.1f + bonus; // 10% bonus at 1*
        return 1.2f + bonus;    // 20% bonus at 3*
    }

    public float DailyMythicPills => GetVaseMythicPills() + GetMirrorMythicPills();
    public float XpMultiplier => GetVaseXpMultiplier();
}
