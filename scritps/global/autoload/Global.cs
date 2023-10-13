using Godot;
using System;
using System.Collections.Generic;

public partial class Global : Node {
	#region Variables
		// public PackedScene bullet = (PackedScene)ResourceLoader.Load(GameResources.bulletPath);
		[Export] public PackedScene mainMenuScene;
		[Export] public PackedScene lobbyScene;
		[Export] public PackedScene gameScene;

		[ExportGroup("Prefabs")]
			[Export] public PackedScene playerPrefab;

		public ENetMultiplayerPeer multiplayerPeer;

		public static List<PlayerData> players = new List<PlayerData>();
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {

		}

		public override void _Process(double delta) {

		}
	#endregion

	#region My Methods
	#endregion

	#region Events
	#endregion
}
