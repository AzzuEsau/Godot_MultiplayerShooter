using Godot;
using System;

public partial class PlayerPunch : State {
	#region Variables
		[Export] private Player player;
		[Export] private AnimationPlayer animationPlayer;

		public float slownesVelocity;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Validaitons
		public override void _Ready() {
			Helperuitilies.ValidateCheckNullValue(this, nameof(player), player);
			Helperuitilies.ValidateCheckNullValue(this, nameof(animationPlayer), animationPlayer);
		}
	#endregion

	#region Godot Methdos

		public override void Enter() {
			player.canChangeState = false;
			slownesVelocity = player.directionLerp;

			AnimatePlayer();
		}

		public override void Exit() {
			
		}

		public override void Update(double delta) {
			
		}

		public override void PhysicsUpdate(double delta) {
			CheckIsOnFloor();
			ReduceVelocity(delta);
			MovePlayer();
		}
    #endregion

    #region My Methods
		private async void AnimatePlayer() {
			float mouseDegress = player.GetMouseDegrees();
			bool doFlipSprite = mouseDegress > 90 && mouseDegress < 270;
			player.FlipSpriteToRight(!doFlipSprite);

			player.PlayAnimation(GameResources.playerPunchAnimation);
			await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);

			player.canChangeState = true;
		}

		private void MovePlayer() {
			player.ApplyGravity();
			player.ApplyHorizontalVelocity(slownesVelocity);
			player.MoveAndSlide();
		}

		private void ReduceVelocity(double delta) {
			slownesVelocity = Mathf.MoveToward(slownesVelocity, 0, (float)delta * player.lerpSpeed);
		}

		private void CheckIsOnFloor() {
			if(!player.IsOnFloor()) player.canChangeState = true;
		}
    #endregion

    #region Events
    #endregion
}
