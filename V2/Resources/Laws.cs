using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.Scripts;

namespace OvermortalTools.V2.Resources;

public partial class Laws : Resource
{
    private int _averageBlitzHours = 120;
    private Quality _fruitQuality = Quality.Legendary;

    private bool _hasShears = false;
    private int _shearsStars = 0;

    private LawData _metal;
    private LawData _wood;
    private LawData _water;
    private LawData _fire;
    private LawData _earth;

    public LawData[] AllLaws => [Metal, Wood, Water, Fire, Earth];

    [Export]
    public LawData Metal
    {
        get => _metal;
        set
        {
            if (_metal == value) return;
            _metal = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                value.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    [Export]
    public LawData Wood
    {
        get => _wood;
        set
        {
            if (_wood == value) return;
            _wood = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                value.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    [Export]
    public LawData Water
    {
        get => _water;
        set
        {
            if (_water == value) return;
            _water = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                value.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    [Export]
    public LawData Fire
    {
        get => _fire;
        set
        {
            if (_fire == value) return;
            _fire = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                value.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    [Export]
    public LawData Earth
    {
        get => _earth;
        set
        {
            if (_earth == value) return;
            _earth = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (value != null)
            {
                value.Changed += () => Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            }
        }
    }

    [Export]
    public int AverageBlitzHours
    {
        get => _averageBlitzHours;
        set
        {
            if (_averageBlitzHours == value) return;
            _averageBlitzHours = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public Quality FruitQuality
    {
        get => _fruitQuality;
        set
        {
            if (_fruitQuality == value) return;
            _fruitQuality = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    [Export]
    public bool HasShears
    {
        get => _hasShears;
        set
        {
            if (_hasShears == value) return;
            _hasShears = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
            if (!value)
            {
                ShearsStars = 0;
            }
        }
    }

    [Export]
    public int ShearsStars
    {
        get => _shearsStars;
        set
        {
            if (_shearsStars == value) return;
            _shearsStars = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }

    public Laws()
    {
        Metal = new();
        Wood = new();
        Water = new();
        Fire = new();
        Earth = new();
    }

    public long TotalXpPerHour => Math.Max(AllLaws.Select(l => l.PointsPerHour).Sum(), 1);
    public long TotalLawLevel => AllLaws.Select(l => l.Level).Sum();

    public float ShearsHours => (ShearsRechargeValues[ShearsStars] * 96f + 100) / 100f * 14;
    
    private static Dictionary<int, float> ShearsRechargeValues => new()
    {
        { 0, 1f },
        { 1, 1.3f },
        { 2, 1.6f },
        { 3, 2f },
        { 4, 2.4f },
        { 5, 3f }
    };

}
