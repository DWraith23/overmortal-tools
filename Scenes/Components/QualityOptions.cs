using Godot;
using OvermortalTools.Scripts.Enums;
using System;

public partial class QualityOptions : OptionButton
{
    public override void _Ready()
    {
        Quality.NamesList.ForEach(quality => AddItem(quality));
    }

}
