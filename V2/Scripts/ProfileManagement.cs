using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using OvermortalTools.V2.Resources;

namespace OvermortalTools.V2.Scripts;

public static class ProfileManagement
{
    public static bool FileAlterationInProgress { get; set; } = false;

    public static void SaveProfile(ProfileData data, int slot)
    {
        SaveWindows(data, slot);
    }

    private static void SaveAndroid()
    {

    }

    private static void SaveWindows(ProfileData data, int slot)
    {
        if (FileAlterationInProgress) return;

        GD.Print("----------------------------");
        GD.Print("Saving state...");
        if (!DirAccess.DirExistsAbsolute("user://saves")) DirAccess.MakeDirAbsolute("user://saves");
        var path = $"user://saves/savestate{slot}.tres";
        var state = data;
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

    public static ProfileData LoadProfile(int slot)
    {
        return LoadWindows(slot);
    }

    private static void LoadAndroid()
    {

    }

    private static ProfileData LoadWindows(int slot)
    {
        var path = $"user://saves/savestate{slot}.tres";
        // if (!File.Exists(path)) return null; // Check if the profile save path exists

        FileAlterationInProgress = true;

        GD.Print("----------------------------");
        GD.Print("Loading state...");
        var loaded = ResourceLoader.Load(path, "", 0);
        if (loaded is ProfileData data)
        {
            GD.Print("| Loaded state successfully.");
            GD.Print("----------------------------");

            FileAlterationInProgress = false;

            return data;
        }
        else
        {
            GD.Print("| ERROR: Failed to load state.");
        }
        GD.Print("----------------------------");

        FileAlterationInProgress = false;
        return null;
    }


}
