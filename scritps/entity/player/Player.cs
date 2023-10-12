using Godot;
using System;

public partial class Player : CharacterBody2D {
	#region Variables
		[ExportGroup("Finite State Machine")]
			[Export] private FiniteStateMachine fsm;
		[ExportSubgroup("States")]
			[Export] private PlayerIdle idleState;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {

		}

		public override void _Process(double delta) {

		}

		public override void _PhysicsProcess(double delta) {
			ReadInput();
		}
    #endregion

    #region My Methods
    public void ApplyGravity() => Velocity = new Vector2(Velocity.X, Velocity.Y + GameResources.gravity);

		private void ReadInput() {
			float direction = Input.GetAxis(GameResources.leftKey, GameResources.rightKey);
			bool isJumping = Input.IsActionJustPressed(GameResources.jumpKey);
			bool isShooting = Input.IsActionPressed(GameResources.shootKey);
		}
	#endregion

	#region Events
	#endregion
}
