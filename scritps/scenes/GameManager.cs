using Godot;
using System;

public partial class GameManager : Node2D {
	#region Variables
		[Export] private Node2D spawnPointBox;

		private Global global;
		private Player playerPrefab;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			global = GetNode<Global>(GameResources.GlobalPath);

			int index = 0;

			foreach(PlayerData player in Global.players) {
				Player currentPlayer = global.playerPrefab.Instantiate<Player>();
				currentPlayer.Name = player.id.ToString();
				currentPlayer.SetPlayer(player.name);
				AddChild(currentPlayer);
				foreach(Node2D spawnPoint in spawnPointBox.GetChildren()) {
					if(spawnPoint.Name == index.ToString())
						currentPlayer.GlobalPosition = spawnPoint.GlobalPosition;
				}
				index++;
			}
		}

		public override void _Process(double delta) {

		}
	#endregion

	#region My Methods
	#endregion

	#region Events
	#endregion
}
