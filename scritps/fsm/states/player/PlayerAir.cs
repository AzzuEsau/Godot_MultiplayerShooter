using Godot;
using System;

public partial class PlayerAir : State {
	#region Variables
		[Export] private Player player;
		[Export] private AnimationPlayer animationPlayer;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {

		}
	#endregion

	#region State Methods
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
			if(player.Velocity.Y < 0) animationPlayer.Play(GameResources.playerJumpAnimation);
			else animationPlayer.Play(GameResources.playerFallAnimation);

			if(player.directionInput != 0) player.FlipSpriteToRight(player.directionInput > 0);
		}

		private void MovePlayer() {
			if(player.Velocity.Y < 0 && player.isJumpingInput) player.ApplyGravity(.65f);
			else player.ApplyGravity(); 

			player.ApplyHorizontalVelocity(player.directionInput);
			player.MoveAndSlide();
		}
    #endregion

    #region Events
    #endregion
}
