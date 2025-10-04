using Godot;
using OvermortalTools.Scripts;

namespace OvermortalTools.V2.Resources;

public partial class SettingsData : Resource
{
    private Color _favoriteColor = Colors.Purple;

    [Export]
    public Color FavoriteColor
    {
        get => _favoriteColor;
        set
        {
            if (_favoriteColor == value) return;
            _favoriteColor = value;
            Tools.EmitLoggedSignal(this, Resource.SignalName.Changed);
        }
    }
}
