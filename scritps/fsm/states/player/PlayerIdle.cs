using Godot;
using System;

public partial class PlayerIdle : State {
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
			player.PlayAnimation(GameResources.playerIdleAnimation);
		}

		private void MovePlayer() {
			player.ApplyGravity();
			player.MoveAndSlide();
		}
    #endregion

    #region Events
    #endregion
}
