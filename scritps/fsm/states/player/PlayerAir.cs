using Godot;
using System;

public partial class PlayerAir : State {
	#region Variables
		[Export] private Player player;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {
			Helperuitilies.ValidateCheckNullValue(this, nameof(player), player);
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
			// if(player.Velocity.Y < 0) animationPlayer.Play(GameResources.playerJumpAnimation);
			// else animationPlayer.Play(GameResources.playerFallAnimation);
			if(player.Velocity.Y < 0) player.PlayAnimation(GameResources.playerJumpAnimation);
			else player.PlayAnimation(GameResources.playerFallAnimation);

			if(player.directionInput != 0) player.FlipSpriteToRight(player.directionInput > 0);
		}

		private void MovePlayer() {
			// Move the player vertical
			if(player.Velocity.Y < 0 && player.isKeepingJumpingInput) player.ApplyGravity(.65f);
			else {
				player.ApplyGravity(); 
				player.isKeepingJumpingInput = false;
			}

			// Move the player horizontal
			if(player.directionInput == 0) player.ApplyHorizontalVelocity(player.directionLerp);
			else player.ApplyHorizontalVelocity(player.directionInput);


			player.MoveAndSlide();
		}
    #endregion

    #region Events
    #endregion
}
