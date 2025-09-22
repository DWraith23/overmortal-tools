using Godot;
using OvermortalTools.V2.Resources;
using System;
using System.Linq;

namespace OvermortalTools.V2.Scenes;

public partial class PathDataSelection : VBoxContainer
{
    private OptionButton PathOptions => GetNode<OptionButton>("Path");
    private OptionButton RealmOptions => GetNode<OptionButton>("Realm");
    private OptionButton MinorRealmOptions => GetNode<OptionButton>("Minor Realm");
    private SpinBox RealmProgress => GetNode<SpinBox>("Realm Progress");
    private LineEdit TotalXp => GetNode<LineEdit>("Total XP");

    private PathData _data = new();
    public PathData Data
    {
        get => _data;
        set
        {
            if (_data == value) return;
            _data = value;
            Update();
        }
    }

    private void Update()
    {
        if (Data == null)
        {
            PathOptions.Select(0);
            RealmOptions.Select(0);
            MinorRealmOptions.Select(0);
            RealmProgress.Value = 0f;

            PathOptions.Disabled = true;
            RealmOptions.Disabled = true;
            MinorRealmOptions.Disabled = true;
            RealmProgress.Editable = false;
            return;
        }

        PathOptions.Disabled = false;
        RealmOptions.Disabled = false;
        MinorRealmOptions.Disabled = false;
        RealmProgress.Editable = true;

        PathOptions.Select(Data.GetSelectedPathIndex());
        RealmOptions.Select(Data.GetCurrentRealmIndex());
        MinorRealmOptions.Select(Data.GetCurrentMinorRealmIndex());
        RealmProgress.SetValueNoSignal(Data.CurrentRealmProgress * 100f);

        // for (int i = 1; i < Data.GetSelectedPathIndex(); i++)
        // {
        //     PathOptions.SetItemDisabled(i, true);
        // }

        // for (int i = 1; i < Data.GetCurrentRealmIndex(); i++)
        // {
        //     RealmOptions.SetItemDisabled(i, true);
        // }

        // for (int i = 1; i < Data.GetCurrentMinorRealmIndex(); i++)
        // {
        //     MinorRealmOptions.SetItemDisabled(i, true);

        // }

        TotalXp.Text = Data.GetTotalXp().ToString("N0");
    }

    public override void _Ready()
    {
        ConnectSignals();
        SetUi();
    }

    private void ConnectSignals()
    {
        PathOptions.ItemSelected += index =>
        {
            if (Data == null) return;
            var path = Enum.GetValues<PathData.Path>()
                .Where(p => p != PathData.Path.None)
                .ElementAt((int)index - 1);
            Data.SelectedPath = path;
            Update();
        };

        RealmOptions.ItemSelected += index =>
        {
            if (Data == null) return;
            var realm = Enum.GetValues<PathData.Realm>()
                .Where(r => r != PathData.Realm.None)
                .ElementAt((int)index - 1);
            Data.CurrentRealm = realm;
            Update();
        };

        MinorRealmOptions.ItemSelected += index =>
        {
            if (Data == null) return;
            var minorRealm = Enum.GetValues<PathData.MinorRealm>()
                .Where(r => r != PathData.MinorRealm.None)
                .ElementAt((int)index - 1);
            Data.CurrentMinorRealm = minorRealm;
            Update();
        };

        RealmProgress.ValueChanged += value =>
        {
            if (Data == null) return;
            Data.CurrentRealmProgress = (float)value / 100f;
            Update();
        };
    }

    private void SetUi()
    {
        var paths = Enum.GetValues<PathData.Path>().Where(p => p != PathData.Path.None);
        var realms = Enum.GetValues<PathData.Realm>().Where(r => r != PathData.Realm.None);
        var minorRealms = Enum.GetValues<PathData.MinorRealm>().Where(mr => mr != PathData.MinorRealm.None);

        foreach (var path in paths)
        {
            PathOptions.AddItem(PathData.PathNames[path]);
        }
        foreach (var realm in realms)
        {
            RealmOptions.AddItem(PathData.RealmNames[realm]);
        }
        foreach (var minorRealm in minorRealms)
        {
            MinorRealmOptions.AddItem(PathData.MinorRealmNames[minorRealm]);
        }
    }
}
