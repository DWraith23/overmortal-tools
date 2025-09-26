using Godot;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class LawsCalculator : VBoxContainer
{

    private LawCalculatorInput Input => GetNode<LawCalculatorInput>("Laws Tabs/Input");
    private LawCalculatorOutput Output => GetNode<LawCalculatorOutput>("Laws Tabs/Output");

    private ProfileData _data = new();
    public ProfileData Data
    {
        get => _data;
        set
        {
            if (_data == value) return;
            _data = value;
            Update();
            if (value != null)
            {
                _data.Laws.Changed += Update;
            }
        }
    }

    private void Update()
    {
        Input.Profile = Data;
        Output.Profile = Data;
    }

    public override void _Ready()
    {
        _data.Laws.Changed += Update;
    }




}
