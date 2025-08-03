using Godot;
using OvermortalTools.Resources.Abilities;
using OvermortalTools.Scripts;
using System;

namespace OvermortalTools.Scenes.Abilities;

public partial class Ability : PanelContainer
{
    [Signal] public delegate void DataChangedEventHandler();

    private AbilityData _data;
    [Export]
    public AbilityData Data
    {
        get => _data;
        set
        {
            _data = value;
            Update();
            if (value != null) _data.Changed += OnDataChanged;
        }
    }

    [ExportGroup("Nodes")]
    [Export] private TextureRect Icon { get; set; }
    [Export] private Label Label { get; set; }
    [Export] private SpinBox SpinBox { get; set; }
    [ExportGroup("Requirements")]
    [Export] private Ability PrereqAbility { get; set; }
    [Export] private int PrereqLevel { get; set; }

    private bool _disabled = true;
    public bool Disabled
    {
        get => _disabled;
        set
        {
            _disabled = value;
            SpinBox.Editable = !value;
        }
    }

    public int Level => (int)SpinBox.Value;

    private void Update()
    {
        if (Data == null)
        {
            Icon.Texture = null;
            Label.Text = "";
            SpinBox.Value = 0;
            SpinBox.MaxValue = 0;
            return;
        }
        Icon.Texture = Data.Icon;
        Label.Text = Data.Name;
        SpinBox.Value = 0;
        SpinBox.MaxValue = Data.MaxLevel;
    }

    private void OnDataChanged() => Tools.EmitLoggedSignal(this, SignalName.DataChanged);
    private void OnSpinBoxChanged(double value) => Data.Level = (int)value;

    public void CheckAvailability() => Disabled = PrereqAbility.Level < PrereqLevel;
    
}
