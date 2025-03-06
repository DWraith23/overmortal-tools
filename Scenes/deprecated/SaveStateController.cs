using Godot;
using OvermortalTools.Resources;
using System;

public partial class SaveStateController : PanelContainer
{}
//     public ToolSelection Tools { get; set; }

//     [Export] private Label SuccessLabel { get; set; }
//     private void SaveButtonPressed()
//     {
//         var path = OS.GetExecutablePath().GetBaseDir() + "/savestate.res";
//         var state = SaveState.GenerateSaveState(Tools);
//         var result = ResourceSaver.Save(state, path);
//         if (result != Error.Ok)
//         {
//             SuccessLabel.Text = "Failed to save.";
//         }
//         else
//         {
//             SuccessLabel.Text = "Game saved.";
//         }
//     }

//     private void LoadButtonPressed()
//     {
//         var path = OS.GetExecutablePath().GetBaseDir() + "/savestate.res";
//         var loaded = ResourceLoader.Load(path, "", 0);
//         if (loaded is SaveState state)
//         {
//             SaveState.LoadSaveState(Tools, state);
//             SuccessLabel.Text = "Game loaded.";
//         }
//         else
//         {
//             SuccessLabel.Text = "Failed to load.";
//         }
//     }
// }

// public static async void SaveGame(string name, bool autosave = false)
//     {
//         var path = OS.GetExecutablePath().GetBaseDir() + "/Saves/" + name + ".res";
//         _ = System.IO.Directory.CreateDirectory(OS.GetExecutablePath().GetBaseDir() + "/Saves");

//         // Prompt the player to confirm if overwriting an existing save file.
//         if (ResourceLoader.Exists(path) && !autosave)
//         {
//             var question = $"{name} exists. Overwrite existing save file?";
//             var confirmed = await ConfirmationCreator(CurrentGame.CurrentScene, string.Empty, question);
//             if (!confirmed) return;
//         }

//         var result = ResourceSaver.Save(CurrentGame, path);
//         if (result != Error.Ok)
//         {
//             GD.Print("Error saving game: " + result);
//         }
//         else if (!autosave)
//         {
//             await PopupCreator(CurrentGame.CurrentScene, "Success", "Game saved as " + name + ".");
//         }
//     }

    // public static async void LoadGame(string name)
    // {
    //     var path = OS.GetExecutablePath().GetBaseDir() + "/Saves/" + name + ".res";
        
    //     if (!ResourceLoader.Exists(path))
    //     {
    //         await PopupCreator(GameRoot, "Error", "Save file not found.");
    //         return;
    //     }
    //     CurrentGame = null;
    //     var loaded = ResourceLoader.Load(path, "", 0);
    //     if (loaded is GameResource game)
    //     {
    //         GD.Print("Game loaded.");
    //         CurrentGame = game;
    //         CurrentGame.GameName = name;
    //     }
    //     else
    //     {
    //         GD.Print("Error loading game.");
    //         await PopupCreator(GameRoot, "Error", "Error loading game.");
    //         return;
    //     }
    // }
