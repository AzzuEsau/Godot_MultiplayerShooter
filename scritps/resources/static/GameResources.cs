using Godot;
using System;

public static class GameResources {
	#region Variables
		// [Export] private int vairbaleInEditor;
		#region Resources Path
			public static string bulletPath = "";
		#endregion

		#region Autloads Path
			public static string GlobalPath = "/root/Global";
		#endregion

		#region Physics
			public static float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
			public static float playerSpeed = 150F;
			public static float jumpSpeed = 250F;

		#endregion

		#region Player Animations
			public static string playerIdleAnimation = "idle";
			public static string playerWalkAnimation = "walk";
			public static string playerPunchAnimation = "punch";
			public static string playerRunAnimation = "run";
			public static string playerJumpAnimation = "jump";
			public static string playerFallAnimation = "fall";
		#endregion

		#region Inputs Names
			public static string leftKey = "left";
			public static string rightKey = "right";
			public static string upKey = "up";
			public static string downKey = "down";
			public static string shootKey = "shoot";
			public static string jumpKey = "jump";
			public static string reloadKey = "reload";
			public static string runKey = "run";
			public static string dropWeaponKey = "drop";
		#endregion
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion
}
