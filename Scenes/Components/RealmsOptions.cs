using Godot;
using OvermortalTools.Scripts.Enums;

public partial class RealmsOptions : OptionButton
{
    public override void _Ready()
    {
        Realm.NamesList.ForEach(realm => AddItem(realm));
    }
}
