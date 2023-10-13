using Godot;
using System;

public partial class Player : CharacterBody2D {
	#region Variables
		[Export] private Sprite2D playerSprite;
		[Export] private Marker2D shootPoint;


		[ExportGroup("Finite State Machine")]
			[Export] private FiniteStateMachine fsm;
		[ExportSubgroup("States")]
			[Export] private PlayerIdle idleState;
			[Export] private PlayerMoveFloor moveOnFloorState;
			[Export] private PlayerPunch punchState;
			[Export] private PlayerJump jumpState;
			[Export] private PlayerAir airState;



		#region Inputs
			public float directionInput {get; protected set;}
			public bool isJumpingInput {get; protected set;}
			public bool isShootingInput {get; protected set;}
			public bool isRunningInput {get; protected set;}

			public float directionLerp = 0;
			public float lerpSpeed {get; protected set;} = 2F;
		#endregion

		public bool canChangeState = true;
		public bool canChangeAniamtion = true;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {

		}

		public override void _Process(double delta) {
			GD.Print(Velocity);
		}

		public override void _PhysicsProcess(double delta) {
			ReadInput();
			ChangeStates();
			ApplyMovementLerp(delta);
		}
    #endregion

    #region My Methods
		#region Physics
			public void ApplyGravity(float multiplier = 1F) => Velocity = new Vector2(Velocity.X, Velocity.Y + (GameResources.gravity * multiplier));
			public void ApplyHorizontalVelocity(float direction, float multiplier = 1F) => Velocity = new Vector2(direction * GameResources.playerSpeed * multiplier, Velocity.Y);
			private float ApplyMovementLerp(double delta) => directionLerp = Mathf.MoveToward(directionLerp, directionInput, (float)delta * lerpSpeed);
		#endregion

		#region Spirte
			public void FlipSpriteToRight(bool isRight) => playerSprite.FlipH = isRight;
		#endregion

		#region Input
			private void ReadInput() {
				directionInput = Input.GetAxis(GameResources.leftKey, GameResources.rightKey);
				isJumpingInput = Input.IsActionPressed(GameResources.jumpKey);
				isShootingInput = Input.IsActionPressed(GameResources.shootKey);
				isRunningInput = Input.IsActionPressed(GameResources.runKey);
			}
		#endregion

		#region States
			private void ChangeStates() {
				if(!canChangeState) return;

				if(CanAir()) airState.EmitSignal(State.SignalName.Transition, airState, airState.Name);
				else if (CanJump()) jumpState.EmitSignal(State.SignalName.Transition, jumpState, jumpState.Name);
				else if(CanPunch()) punchState.EmitSignal(State.SignalName.Transition, punchState, punchState.Name);
				else if(CanMoveOnFloor()) moveOnFloorState.EmitSignal(State.SignalName.Transition, moveOnFloorState, moveOnFloorState.Name);
				else if(CanIdle()) idleState.EmitSignal(State.SignalName.Transition, idleState, idleState.Name);
			}

			private bool CanAir() => !IsOnFloor();
			private bool CanJump() => IsOnFloor() && isJumpingInput;
			private bool CanPunch() => isShootingInput;
			private bool CanMoveOnFloor() => (directionInput != 0 || directionLerp != 0) && IsOnFloor();
			private bool CanIdle() => directionInput == 0 && !isJumpingInput && IsOnFloor();
		#endregion

		#region Shoot
			private void Shoot() {
				
			}
		#endregion
	#endregion

	#region Events
	#endregion
}
