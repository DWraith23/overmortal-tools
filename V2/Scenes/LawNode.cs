using Godot;
using OvermortalTools.Scripts;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class LawNode : HBoxContainer
{
    [Export] public string LawName { get; set; }

    private Label Label => GetNode<Label>("Label");
    private SpinBox Level => GetNode<SpinBox>("Level");
    private SpinBox Bonus => GetNode<SpinBox>("Bonus");

    private LawData _data;
    public LawData Data
    {
        get => _data;
        set
        {
            if (_data == value) return;
            _data = value;
            Update();
            if (value != null)
            {
                _data.Changed += Update;
            }
        }
    }

    private void Update()
    {
        if (Data == null) return;

        Level.SetValueNoSignal(Data.Level);
        Bonus.SetValueNoSignal(Data.Bonus);
    }

    public override void _Ready()
    {
        ConnectSignals();
        Label.Text = LawName;
    }

    private void ConnectSignals()
    {
        Level.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.Level = (int)value;
        };

        Bonus.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.Bonus = (float)value;
        };
    }
}
