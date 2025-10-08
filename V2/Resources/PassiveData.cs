using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.Scripts;

namespace OvermortalTools.V2.Resources;

public partial class PassiveData : Resource
{

    #region Static Data

    public static readonly Dictionary<(PathData.Realm, PathData.MinorRealm), float> AbsorptionRates = new()
    {
        { (PathData.Realm.Incarnation, PathData.MinorRealm.Early), .317f },
        { (PathData.Realm.Incarnation, PathData.MinorRealm.Middle), .3580f },
        { (PathData.Realm.Incarnation, PathData.MinorRealm.Late), .4f },

        { (PathData.Realm.Voidbreak, PathData.MinorRealm.Early), .5f },
        { (PathData.Realm.Voidbreak, PathData.MinorRealm.Middle), .65f },
        { (PathData.Realm.Voidbreak, PathData.MinorRealm.Late), .8f },

        { (PathData.Realm.Wholeness, PathData.MinorRealm.Early), .85f },
        { (PathData.Realm.Wholeness, PathData.MinorRealm.Middle), .9f },
        { (PathData.Realm.Wholeness, PathData.MinorRealm.Late), .9f },

        { (PathData.Realm.Perfection, PathData.MinorRealm.Early), 1.3f },
        { (PathData.Realm.Perfection, PathData.MinorRealm.Middle), 1.6f },
        { (PathData.Realm.Perfection, PathData.MinorRealm.Late), 1.8f },

        { (PathData.Realm.Nirvana, PathData.MinorRealm.Early), 2f },
        { (PathData.Realm.Nirvana, PathData.MinorRealm.Middle), 2.2f },
        { (PathData.Realm.Nirvana, PathData.MinorRealm.Late), 2.4f },

        { (PathData.Realm.Celestial, PathData.MinorRealm.Early), 2.6f },
        { (PathData.Realm.Celestial, PathData.MinorRealm.Middle), 2.8f },
        { (PathData.Realm.Celestial, PathData.MinorRealm.Late), 3f },

        { (PathData.Realm.Eternal, PathData.MinorRealm.Early), 4f },
        { (PathData.Realm.Eternal, PathData.MinorRealm.Middle), 4.2f },
        { (PathData.Realm.Eternal, PathData.MinorRealm.Late), 4.4f },
    };

    private static Dictionary<int, float> AuraGemValues => new()
    {
        { -1, 0f },
        { 0, 0f },
        { 1, 0.1f },
        { 2, 0.13f },
        { 3, 0.16f },
        { 4, 0.2f },
        { 5, 0.24f },
        { 6, 0.28f },
    };


    #endregion

    #region Properties and Exports
    // Cosmoapsis
    private float _abodeAura = 0.0f;
    private float _viryaPercent = 0.0f;
    private float _strivePercent = 0.0f;

    [Export]
    public float AbodeAura
    {
        get => _abodeAura;
        set
        {
            if (_abodeAura == value) return;
            _abodeAura = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float ViryaPercent
    {
        get => _viryaPercent;
        set
        {
            if (_viryaPercent == value) return;
            _viryaPercent = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float StrivePercent
    {
        get => _strivePercent;
        set
        {
            if (_strivePercent == value) return;
            _strivePercent = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    // Aura Gem
    private int _auraGemIndex = 0;
    private float _auraseepSpeedBonus = 0.0f;
    private float _auraseepBonus = 0.0f;

    [Export]
    public int AuraGemIndex
    {
        get => _auraGemIndex;
        set
        {
            if (_auraGemIndex == value) return;
            _auraGemIndex = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float AuraseepSpeedBonus
    {
        get => _auraseepSpeedBonus;
        set
        {
            if (_auraseepSpeedBonus == value) return;
            _auraseepSpeedBonus = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public float AuraseepBonus
    {
        get => _auraseepBonus;
        set
        {
            if (_auraseepBonus == value) return;
            _auraseepBonus = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    #endregion


    #region Calculated Properties
    private float Multiplier => ViryaPercent + StrivePercent;


    #endregion

    public float GetCosmoapsisPerCycle((PathData.Realm realm, PathData.MinorRealm minorRealm) realm)
    {
        var baseRate = AbodeAura;
        var absorbtion = AbsorptionRates.TryGetValue(realm, out var rate) ? rate : 0.2f;
        var bonus = absorbtion + Multiplier;
        return baseRate * bonus;
    }

    public long GetDailyCosmoapsisExp((PathData.Realm realm, PathData.MinorRealm minorRealm) realm)
    {
        var perCycle = GetCosmoapsisPerCycle(realm);
        var perSecond = perCycle / 8f;
        return (long)Math.Floor(perSecond * 60f * 60f * 24f);
    }

    public long GetDailyAuraGemExp((PathData.Realm realm, PathData.MinorRealm minorRealm) realm)
    {
        var daily = GetDailyCosmoapsisExp(realm);
        var auraGemMultiplier = AuraGemValues[AuraGemIndex];
        var auraGemValue = (long)Math.Floor(daily * (auraGemMultiplier + AuraseepSpeedBonus / 100f));
        var auraseepBonusValue = (long)Math.Floor(auraGemValue * AuraseepBonus / 100f);
        return auraGemValue + auraseepBonusValue;
    }


    public bool NeedsAttention =>
        AbodeAura == 0f;


}
