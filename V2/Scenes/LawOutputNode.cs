using Godot;
using OvermortalTools.Scripts;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class LawOutputNode : HBoxContainer
{
    [Export] private string LawName { get; set; }

    private Label Label => GetNode<Label>("Label");
    private LineEdit Hourly => GetNode<LineEdit>("Hourly");
    private LineEdit Hours => GetNode<LineEdit>("Hours");

    private Laws _laws;
    public Laws Laws
    {
        get => _laws;
        set
        {
            if (_laws == value) return;
            _laws = value;
            Update();
            if (value != null)
            {
                _laws.Changed += Update;
            }
        }
    }

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

    public override void _Ready()
    {
        Label.Text = LawName;
    }


    private void Update()
    {
        if (Data == null || Laws == null) return;

        Hourly.Text = Data.PointsPerHour.FormatLargeNumber();
        Hours.Text = Math.Ceiling(Data.XPRemainingForNextThreshold / (decimal)Laws.TotalXpPerHour).ToString("N0");
    }

}
