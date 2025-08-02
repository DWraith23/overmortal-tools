using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace OvermortalTools.Resources.Laws;

public partial class LawsData : Resource
{
    public enum LawFruitQuality
    {
        Green,
        Blue,
        Purple,
        Gold
    }

    private ElementalLaw _metal = new() { Name = "Metal" };
    private ElementalLaw _wood = new() { Name = "Wood" };
    private ElementalLaw _water = new() { Name = "Water" };
    private ElementalLaw _fire = new() { Name = "Fire" };
    private ElementalLaw _earth = new() { Name = "Earth" };

    private int _averageBlitzHours = 120;
    private LawFruitQuality _fruitQuality = LawFruitQuality.Gold;

    private bool _hasShears = false;
    private int _shearsStars = 0;




    [Export]
    public ElementalLaw Metal
    {
        get => _metal;
        set
        {
            if (_metal == value) return;
            _metal = value;
        }
    }

    [Export]
    public ElementalLaw Wood
    {
        get => _wood;
        set
        {
            if (_wood == value) return;
            _wood = value;
        }
    }

    [Export]
    public ElementalLaw Water
    {
        get => _water;
        set
        {
            if (_water == value) return;
            _water = value;
        }
    }

    [Export]
    public ElementalLaw Fire
    {
        get => _fire;
        set
        {
            if (_fire == value) return;
            _fire = value;
        }
    }

    [Export]
    public ElementalLaw Earth
    {
        get => _earth;
        set
        {
            if (_earth == value) return;
            _earth = value;
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
            EmitChanged();
        }
    }

    [Export]
    public LawFruitQuality FruitQuality
    {
        get => _fruitQuality;
        set
        {
            if (_fruitQuality == value) return;
            _fruitQuality = value;
            EmitChanged();
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
            EmitChanged();
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
            EmitChanged();
        }
    }

    public LawsData()
    {
        Metal.Changed += EmitChanged;
        Wood.Changed += EmitChanged;
        Water.Changed += EmitChanged;
        Fire.Changed += EmitChanged;
        Earth.Changed += EmitChanged;
    }

}
