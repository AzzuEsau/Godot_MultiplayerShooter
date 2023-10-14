using Godot;
using System;

public partial class PlayerJump : State {
    #region Variables
		[Export] private Player player;
    #endregion

    #region Signals
    	// [Signal] public delegate void ExampleSignalEventHandler();
    #endregion

    #region Godot Validations
		public override void _Ready() {
			Helperuitilies.ValidateCheckNullValue(this, nameof(player), player);
		}
    #endregion

	#region States Methdos
		public override void Enter() {
			player.jumpBufferTimer = 0;
			player.coyoteTime = 0;
		}

		public override void Exit() {
			player.isKeepingJumpingInput = true;
		}

		public override void Update(double delta) {
			AnimatePlayer();
		}

		public override void PhysicsUpdate(double delta) {
			MovePlayer();
		}
    #endregion

    #region My Methods
		private void AnimatePlayer() {
			player.PlayAnimation(GameResources.playerJumpAnimation);
		}

		private void MovePlayer() {
			player.Velocity = new Vector2(player.Velocity.X, -GameResources.jumpSpeed);
			player.MoveAndSlide();
		}
    #endregion

    #region Events
    #endregion
}
