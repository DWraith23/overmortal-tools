using Godot;
using OvermortalTools.V2.Resources;
using System;

namespace OvermortalTools.V2.Scenes;

public partial class SectionTabs : TabContainer
{
    private Label Paths => GetNode<Label>("Exclamations/Paths");
    private Label Daily => GetNode<Label>("Exclamations/Daily");
    private Label StarMarks => GetNode<Label>("Exclamations/Star Marks");
    private Label Myrimon => GetNode<Label>("Exclamations/Myrimon");


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
                _data.Changed += Update;
            }
        }
    }


    private void Update()
    {
        Paths.Visible = Data.PathsNeedAttention;
        Daily.Visible = Data.DailyNeedsAttention;
        StarMarks.Visible = Data.StarMarksNeedAttention;
        Myrimon.Visible = Data.MyrimonNeedsAttention;
    }

}
