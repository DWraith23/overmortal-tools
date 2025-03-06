using Godot;
using OvermortalTools.Resources;
using System;

namespace OvermortalTools.Scenes;

public partial class Main : Control
{
    // public ToolSelection Tools { get; set; }
    public override void _Ready()
    {
        // Tools = ToolSelection.GenerateInstance();
        // AddChild(Tools);
        // LoadSaveState();
    }

    // private void LoadSaveState()
    // {
    //     var path = OS.GetExecutablePath().GetBaseDir() + "/savestate.res";
    //     if (!ResourceLoader.Exists(path)) return;
    //     var loaded = ResourceLoader.Load(path, "", 0);
    //     if (loaded is SaveState state)
    //     {
    //         SaveState.LoadSaveState(Tools, state);
    //     }
    // }
}
