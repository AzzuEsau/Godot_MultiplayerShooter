using Godot;
using System;

public partial class PlayerMoveFloor : State {
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

		}

		public override void Exit() {
			
		}

		public override void Update(double delta) {
			AnimatePlayer();
		}

		public override void PhysicsUpdate(double delta) {
			MovePlayer();
			FlipSprite();
    	}
    #endregion

    #region My Methods
		private void MovePlayer() {
			player.ApplyGravity();
			if(player.isRunningInput) player.ApplyHorizontalVelocity(player.directionLerp, 1.5f);
			else player.ApplyHorizontalVelocity(player.directionLerp);			
			player.MoveAndSlide();
		}

		private void FlipSprite() {
			if(player.directionInput != 0) player.FlipSpriteToRight(player.directionInput > 0);
		}

		private void AnimatePlayer() {
			if(player.directionInput != 0) 
				if(player.isRunningInput)
					player.PlayAnimation(GameResources.playerRunAnimation);
				else if(player.directionLerp != 0)
					player.PlayAnimation(GameResources.playerWalkAnimation);
		}
    #endregion

    #region Events
    #endregion
}
