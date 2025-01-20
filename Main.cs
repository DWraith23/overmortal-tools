using Godot;
using OvermortalTools.Resources;
using System;

public partial class Main : Control
{
    public ToolSelection Tools { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Tools = ToolSelection.GenerateInstance();
        AddChild(Tools);
        LoadSaveState();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void LoadSaveState()
    {
        var path = OS.GetExecutablePath().GetBaseDir() + "/savestate.res";
        if (!ResourceLoader.Exists(path)) return;
        var loaded = ResourceLoader.Load(path, "", 0);
        if (loaded is SaveState state)
        {
            SaveState.LoadSaveState(Tools, state);
        }
    }
}
