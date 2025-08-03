using Godot;
using OvermortalTools.Resources.Cultivation;

public partial class RealmsOptions : OptionButton
{
    public override void _Ready()
    {
        Realm.NamesList.ForEach(realm => AddItem(realm));
    }
}
