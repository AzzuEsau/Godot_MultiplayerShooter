using Godot;
using System;

public partial class PlayerJump : State {
    #region Variables
		[Export] private Player player;
		[Export] private AnimationPlayer animationPlayer;
    #endregion

    #region Signals
    	// [Signal] public delegate void ExampleSignalEventHandler();
    #endregion

    #region Godot Validations
		public override void _Ready() {
			Helperuitilies.ValidateCheckNullValue(this, nameof(player), player);
			Helperuitilies.ValidateCheckNullValue(this, nameof(animationPlayer), animationPlayer);
		}
    #endregion

	#region States Methdos
		public override void Enter() {
			
		}

		public override void Exit() {
			
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
			animationPlayer.Play(GameResources.playerJumpAnimation);
		}

		private void MovePlayer() {
			player.Velocity = new Vector2(player.Velocity.X, player.Velocity.Y - GameResources.jumpSpeed);
			player.MoveAndSlide();
		}
    #endregion

    #region Events
    #endregion
}
