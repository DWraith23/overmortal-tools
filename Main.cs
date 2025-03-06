using Godot;
using OvermortalTools.Resources;
using OvermortalTools.Scenes.Planner;
using System;
using System.IO;

namespace OvermortalTools.Scenes;

public partial class Main : Control
{
    [Export] private CultivationPlanner Planner { get; set; }

    private bool IsLoading { get; set; } = false;

    public override void _Ready()
    {
        LoadData();
    }

    private void OnSaveRequested() => SaveData();

    public void SaveData()
    {
        if (!IsNodeReady() || IsLoading) return;
        GD.Print("----------------------------");
        GD.Print("Saving state...");
        var path = OS.GetExecutablePath().GetBaseDir() + "/savestate.tres";
        var state = SaveState.GenerateSaveState(Planner);
        var result = ResourceSaver.Save(state, path);
        if (result != Error.Ok)
        {
            GD.Print("| ERROR: Failed to save state.");
        }
        else
        {
            GD.Print("| Saved state successfully.");
        }
        GD.Print("----------------------------");
    }

    private void LoadData()
    {
        var path = OS.GetExecutablePath().GetBaseDir() + "/savestate.tres";
        if (File.Exists(path) == false) return;

        IsLoading = true;

        GD.Print("----------------------------");
        GD.Print("Loading state...");
        var loaded = ResourceLoader.Load(path, "", 0);
        if (loaded is SaveState state)
        {
            SaveState.LoadSaveState(Planner, state);
            GD.Print("| Loaded state successfully.");
        }
        else
        {
            GD.Print("| ERROR: Failed to load state.");
        }
        GD.Print("----------------------------");

        IsLoading = false;
    }
}
