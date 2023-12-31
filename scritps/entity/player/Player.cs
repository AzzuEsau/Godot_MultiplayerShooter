using Godot;
using System;

public partial class Player : CharacterBody2D {
	#region Variables
		[Export] private Sprite2D playerSprite;
		[Export] private Marker2D gunRotation;
		[Export] private Sprite2D gunSprite;
		[Export] private Label playerNameLabel;
		[Export] private MultiplayerSynchronizer multiplayerSynchronizer;
		[Export] private AnimationPlayer animationPlayer;

		#region FNS
			[ExportGroup("Finite State Machine")]
				[Export] private FiniteStateMachine fsm;
			[ExportSubgroup("States")]
				[Export] private PlayerIdle idleState;
				[Export] private PlayerMoveFloor moveOnFloorState;
				[Export] private PlayerPunch punchState;
				[Export] private PlayerJump jumpState;
				[Export] private PlayerAir airState;
		#endregion

		#region Inputs
			public bool isShootingInput {get; protected set;}
			public bool isRunningInput {get; protected set;}

			private float mouseRotation;

			#region Direction Effects
				public float directionInput {get; protected set;}
				public float directionLerp = 0;
				public float lerpSpeed {get; protected set;} = 2F;
			#endregion

			#region Jump Effects
				public bool isJumpingInput {get; protected set;}
				public bool isKeepingJumpingInput = false;

				// When the jump button is pressed before the player touch the floor
				public float jumpBufferTimer = 0;
				public const float maxJumpBufferTimer = .3F;
				// When the jump button is pressed in the air
				public float coyoteTime = 0;
				public const float maxCoyoteTime = .25F;
			#endregion
		#endregion

		#region Multiplayer
			public Vector2 syncPos = new Vector2();
			public float syncRotation = 0;
		#endregion

		public bool canChangeState = true;
		public bool canChangeAniamtion = true;

		private bool isOwner;
    #endregion

    #region Signals
    // [Signal] public delegate void ExampleSignalEventHandler();
    #endregion

    #region Godot Methdos
		public override void _EnterTree() {
			multiplayerSynchronizer.SetMultiplayerAuthority(int.Parse(Name));
		}

    	public override void _Ready() {
			isOwner = multiplayerSynchronizer.IsMultiplayerAuthority();
			if(!isOwner) fsm.Stop();
		}

		public override void _Process(double delta) {

		}

		public override void _PhysicsProcess(double delta) {
			if(!isOwner) return;

			ReadInput();
			ChangeStates();
			ApplyMovementLerp(delta);

			RotateGun();

			PlayJumpDetails(delta);

			syncPos = GlobalPosition;
			syncRotation = gunRotation.RotationDegrees;

		}

		public void SetPlayer(string name) {
			playerNameLabel.Text = name;
		}
    #endregion

    #region My Methods
		#region Physics
			public void ApplyGravity(float multiplier = 1F) => Velocity = new Vector2(Velocity.X, Velocity.Y + (GameResources.gravity * multiplier));
			public void ApplyHorizontalVelocity(float direction, float multiplier = 1F) => Velocity = new Vector2(direction * GameResources.playerSpeed * multiplier, Velocity.Y);
			private float ApplyMovementLerp(double delta) => directionLerp = Mathf.MoveToward(directionLerp, directionInput, (float)delta * lerpSpeed);
		#endregion

		#region Jump Details
			private void PlayJumpDetails(double delta) {
				PlayJumpBufferTimer(delta);
				PlayCoyoteTime(delta);
			}

			private void PlayJumpBufferTimer(double delta) {
				if(IsOnFloor()) return; 

				if(isJumpingInput) jumpBufferTimer = maxJumpBufferTimer;
				else if(jumpBufferTimer > 0) jumpBufferTimer -= (float)delta;
			}

			private void PlayCoyoteTime(double delta) {
				if(IsOnFloor()) coyoteTime = maxCoyoteTime;
				else if(coyoteTime > 0) coyoteTime -= (float)delta;
			}

			private bool CanJumpInFloor() => IsOnFloor() && (jumpBufferTimer > 0 || isJumpingInput);

			private bool CanJumpInAirBuffer() => coyoteTime > 0 && isJumpingInput && Velocity.Y >= 0;
		#endregion

		#region Spirte
			public void FlipSpriteToRight(bool isRight) => playerSprite.FlipH = isRight;
		#endregion

		#region Input
			private void ReadInput() {
				directionInput = Input.GetAxis(GameResources.leftKey, GameResources.rightKey);
				isJumpingInput = Input.IsActionJustPressed(GameResources.jumpKey);
				isShootingInput = Input.IsActionPressed(GameResources.shootKey);
				isRunningInput = Input.IsActionPressed(GameResources.runKey);
				
				if(isKeepingJumpingInput) isKeepingJumpingInput = !Input.IsActionJustReleased(GameResources.jumpKey);
			}

			private bool isDirectionActive() => directionInput != 0 || directionLerp != 0;
		#endregion

		#region States
			private void ChangeStates() {
				if(!canChangeState) return;

				// First Check if the player can Jump
				if (CanJump()) jumpState.EmitSignal(State.SignalName.Transition, jumpState, jumpState.Name);
				// Then Check if the playen can be in air This condition is requiered by the coyote time
				else if(CanAir()) airState.EmitSignal(State.SignalName.Transition, airState, airState.Name);

				else if(CanPunch()) punchState.EmitSignal(State.SignalName.Transition, punchState, punchState.Name);
				else if(CanMoveOnFloor()) moveOnFloorState.EmitSignal(State.SignalName.Transition, moveOnFloorState, moveOnFloorState.Name);
				else if(CanIdle()) idleState.EmitSignal(State.SignalName.Transition, idleState, idleState.Name);
			}

			private bool CanJump() => CanJumpInAirBuffer() || CanJumpInFloor();
			private bool CanAir() => !IsOnFloor();
			private bool CanPunch() => isShootingInput;
			private bool CanMoveOnFloor() => isDirectionActive() && IsOnFloor();
			private bool CanIdle() => directionInput == 0 && !isJumpingInput && IsOnFloor();
		#endregion

		#region Shoot
			private void RotateGun() {
				gunRotation.LookAt(GetGlobalMousePosition());

				mouseRotation = gunRotation.RotationDegrees % 360;
				while(mouseRotation < 0) mouseRotation += 360;
				
				gunRotation.Rotation = Mathf.DegToRad(mouseRotation);
				gunSprite.FlipV = mouseRotation > 90 && mouseRotation < 270;
			}

			public float GetMouseDegrees() => mouseRotation;

			private void Shoot() {
				
			}
		#endregion
	
		#region Animation
			public void PlayAnimation(string animation) {
				// animationPlayer.Play(animation);
				Rpc(nameof(PlayAnimationOnline), animation);
			} 

			[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
			public void PlayAnimationOnline(string animation) {
				animationPlayer.Play(animation);
			}
		#endregion
	#endregion

	#region Events
	#endregion
}
