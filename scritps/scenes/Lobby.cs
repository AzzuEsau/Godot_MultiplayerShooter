using Godot;
using System;

public partial class Lobby : Control {
	#region Variables
		[Export] private Button playButton;
		[Export] private Button exitButton;

		private Global global;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			global = GetNode<Global>(GameResources.GlobalPath);

			playButton.Pressed += PlayButton_Pressed;
			exitButton.Pressed += ExitButton_Pressed;
		}

		public override void _Process(double delta) {

		}
	#endregion

	#region My Methods
	#endregion

	#region Events
		private void PlayButton_Pressed() {
			
		}

		private void ExitButton_Pressed() {
			GetTree().ChangeSceneToPacked(global.mainMenuScene);
		}
	#endregion
}
