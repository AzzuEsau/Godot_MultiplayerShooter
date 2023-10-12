using Godot;
using System;

public partial class PlayerIdle : State
{
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

    #region State Methods
    	public override void Enter() {
			
		}

		public override void Exit() {
			
		}

		public override void PhysicsUpdate(double delta) {
			
		}

		public override void Update(double delta) {
			AnimatePlayer();
		}
    #endregion

    #region My Methods
		private void AnimatePlayer() {
			animationPlayer.Play(GameResources.playerIdleAnimation);
		}
    #endregion

    #region Events
    #endregion
}
