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
    private int ActiveProfile { get; set; } = 0;

    public override void _Ready()
    {
        LoadData();
    }

    private void OnSaveRequested() => SaveData();
    private void OnChangeProfileRequested(int profile) => SwapProfile(profile);

    public void SaveData()
    {
        if (!IsNodeReady() || IsLoading) return;
        GD.Print("----------------------------");
        GD.Print("Saving state...");
        var path = OS.GetExecutablePath().GetBaseDir() + $"/savestate{ActiveProfile}.tres";
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
        var path = OS.GetExecutablePath().GetBaseDir() + "/savestate.tres"; // Original save path before profiles were added
        if (File.Exists(path) == false) // Check if the original save path exists
        {
            path = OS.GetExecutablePath().GetBaseDir() + $"/savestate{ActiveProfile}.tres";
            if (File.Exists(path) == false) return; // Check if the profile save path exists
        }

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

        if (path == OS.GetExecutablePath().GetBaseDir() + "/savestate.tres")
        {
            SaveData(); // Save the loaded state to the profile save file
            File.Delete(path);  // Delete the original save file
        }
    }

    private void CreateNewProfile(int slot)
    {
        var path = OS.GetExecutablePath().GetBaseDir() + $"/savestate{slot}.tres";
        if (File.Exists(path)) return;  // Check if the profile save path exists, don't create if it does

        var state = SaveState.GenerateFreshState();
        var result = ResourceSaver.Save(state, path);
        if (result != Error.Ok)
        {
            GD.Print("| ERROR: Failed to create new profile.");
        }
        else
        {
            GD.Print("| Created new profile successfully.");
        }
    }

    private void SwapProfile(int profile)
    {
        if (profile == ActiveProfile) return;
        SaveData();
        ActiveProfile = profile;
        CreateNewProfile(ActiveProfile);
        LoadData();
    }
}
