using Godot;
using System;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Linq;
using Godot.Collections;

public partial class MainMenu : Control {
	#region Variables
		[ExportGroup("UI")]
			[Export] private CanvasLayer canvasLayer;
			[Export] private Button hostButton;
			[Export] private Button joinButton;
			[Export] private Button playButton;
			[Export] private LineEdit roomCodeTextEdit;
			[Export] private LineEdit userNameTextExit;
			[Export] private VBoxContainer playersIdContainer;

		[ExportGroup("Multiplayer")]
			[Export] private int port = 6969;
			[Export] private string address = "127.0.0.1";
			[Export] private ENetMultiplayerPeer peer;


		private Global global;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			global = GetNode<Global>(GameResources.GlobalPath);

			// roomCodeTextEdit.TextChanged += RoomCodeTextEdit_TextChanged;
			userNameTextExit.TextChanged += UserNameTextExit_TextChanged;

			hostButton.Pressed += HostButton_Pressed;
			joinButton.Pressed += JoinButton_Pressed;
			playButton.Pressed += PlayButton_Pressed;

			// RoomCodeTextEdit_TextChanged("");
			UserNameTextExit_TextChanged("");


			Multiplayer.PeerConnected += Multiplayer_PeerConnected;
			Multiplayer.PeerDisconnected += Multiplayer_PeerDisconnected;
			Multiplayer.ConnectedToServer += Multiplayer_ConnectedToServer;
			Multiplayer.ConnectionFailed += Multiplayer_ConnectionFailed;
		}

		public override void _Process(double delta) {

		}
	#endregion

	#region My Methods
		private void CreateENetMultiplayerPeer() {
			ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
			// Create a server setting
			var error = peer.CreateServer(port, 2);
			if(error != Error.Ok) {
				GD.Print("error can not host! " + error.ToString());
				return;
			}
			// Compress the packages to be fastter
			peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
			Multiplayer.MultiplayerPeer = peer;
		}

		private void ConnectENetMultiplayerPeer() {
			peer = new ENetMultiplayerPeer();
			peer.CreateClient(address, port);

			peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
			Multiplayer.MultiplayerPeer = peer;
			GD.Print("Joining Game");
		}

		private void AddLabel(string name) {
			foreach(Label playerId in playersIdContainer.GetChildren())
				if(playerId.Text == name) 
					return;

			Label label = new Label();
			playersIdContainer.AddChild(label);
			label.Text = name;
		}

		private void RemoveLabel(string name) {
			foreach(Label playerId in playersIdContainer.GetChildren())
				if(playerId.Text == name) 
					playerId.QueueFree();
		}

		[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
		private void InstantiatePlayScene() {
			Node gameScene = global.gameScene.Instantiate();
			GetTree().Root.AddChild(gameScene);
			this.Hide();
			canvasLayer.Hide();
		}

		[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
		private void SendPlayerInfromation(string name, long id) {
			PlayerData playerData = new PlayerData(name, id);
			if(!Global.players.Contains(playerData)) {
				Global.players.Add(playerData);
			}

			foreach(PlayerData temp in Global.players) {
				AddLabel(temp.name);
			}

			// If im the server then send all the information that i got
			if(Multiplayer.IsServer())
				foreach(PlayerData player in Global.players)
					Rpc(nameof(SendPlayerInfromation), player.name, player.id);
		}
	#endregion

	#region Events
		#region Inputs
			private void RoomCodeTextEdit_TextChanged(string text) {
				if(roomCodeTextEdit.Text.Length > 5) {
					roomCodeTextEdit.Text = roomCodeTextEdit.Text.Substring(0, 5);
				}

				if(roomCodeTextEdit.Text.Length != 5) joinButton.Disabled = true;
				else joinButton.Disabled = false;

				UserNameTextExit_TextChanged("");
			}

			private void UserNameTextExit_TextChanged(string text) {
				if(userNameTextExit.Text.Length == 0) {
					joinButton.Disabled = true;
					hostButton.Disabled = true;
				}
				else {
					hostButton.Disabled = false;
					joinButton.Disabled = false;
					// if(roomCodeTextEdit.Text.Length == 5) joinButton.Disabled = false;
				}
			}

			private void HostButton_Pressed() {
				CreateENetMultiplayerPeer();
				SendPlayerInfromation(userNameTextExit.Text, 1);
				// GetTree().ChangeSceneToPacked(global.lobbyScene);
			}

			private void JoinButton_Pressed() {
				ConnectENetMultiplayerPeer();
				// GetTree().ChangeSceneToPacked(global.lobbyScene);
			}

			private void PlayButton_Pressed() {
				Rpc(nameof(InstantiatePlayScene));
			}
		#endregion

		#region Multiplayer
			// Run when a player connects run in all pears
			private void Multiplayer_PeerConnected(long id) {
				GD.Print("Runs in all pears, player connected :) " + id);
			}

			// Run when a player disconnect run in all pears
			private void Multiplayer_PeerDisconnected(long id) {
				GD.Print("Runs in all pears, player disconected :( " + id);
				PlayerData playerDisconected = Global.players.Where(x => x.id == id).First<PlayerData>();
				Global.players.Remove(playerDisconected);

				Array<Node> players = GetTree().GetNodesInGroup("Player");

				GD.Print(players);

				foreach(Node player in players)
					if(player.Name == id.ToString())
						player.QueueFree();
			}


			// Connection was succes and only runs in the client
			private void Multiplayer_ConnectedToServer() {
				GD.Print("Runs in client, connected to server");
				// I just want to send info to an specific Id
				RpcId(1, nameof(SendPlayerInfromation), userNameTextExit.Text, Multiplayer.GetUniqueId());
			}

			// It just run in the client
			private void Multiplayer_ConnectionFailed() {
				GD.Print("Runs in client, connection fail");
			}
		#endregion
	#endregion
}
