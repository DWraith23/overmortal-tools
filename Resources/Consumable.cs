using Godot;

namespace OvermortalTools.Resources;

[GlobalClass, Tool]
public partial class Consumable : Resource
{
    public virtual string DisplayName { get; set; } = "";
}
