using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Player : CharacterBody2D {
	#region Variables
		[Export] private Sprite2D playerSprite;
		[Export] private Marker2D shootPoint;


		[ExportGroup("Finite State Machine")]
			[Export] private FiniteStateMachine fsm;
		[ExportSubgroup("States")]
			[Export] private PlayerIdle idleState;
			[Export] private PlayerMoveFloor moveOnFloorState;


		#region Inputs
			public float directionInput {get; protected set;}
			public bool isJumpingInput {get; protected set;}
			public bool isShootingInput {get; protected set;}

			public float directionLerp = 0;
			private float lerpSpeed = 2F;
		#endregion

		public bool canChangeState = true;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {

		}

		public override void _Process(double delta) {

		}

		public override void _PhysicsProcess(double delta) {
			ReadInput();
			ChangeStates();
			ApplyMovementLerp(delta);
		}
    #endregion

    #region My Methods
		#region Physics
			public void ApplyGravity() => Velocity = new Vector2(Velocity.X, Velocity.Y + GameResources.gravity);
			public void ApplyHorizontalVelocity(float direction) => Velocity = new Vector2(direction * GameResources.playerSpeed, Velocity.Y);
			public float ApplyMovementLerp(double delta) => directionLerp = Mathf.MoveToward(directionLerp, directionInput, (float)delta * lerpSpeed);
		#endregion

		#region Spirte
			public void FlipSpriteToRight(bool isRight) => playerSprite.FlipH = isRight;
		#endregion

		#region Input
			private void ReadInput() {
				directionInput = Input.GetAxis(GameResources.leftKey, GameResources.rightKey);
				isJumpingInput = Input.IsActionJustPressed(GameResources.jumpKey);
				isShootingInput = Input.IsActionPressed(GameResources.shootKey);
			}
		#endregion

		#region States
			private void ChangeStates() {
				if(CanMoveOnFloor()) moveOnFloorState.EmitSignal(State.SignalName.Transition, moveOnFloorState, moveOnFloorState.Name);
				else if(CanIdle()) idleState.EmitSignal(State.SignalName.Transition, idleState, idleState.Name);
			}

			private bool CanMoveOnFloor() => directionInput != 0 || directionLerp != 0;
			private bool CanIdle() => directionInput == 0 && !isJumpingInput;
		#endregion

		#region Shoot
			private void Shoot() {
				
			}
		#endregion
	#endregion

	#region Events
	#endregion
}
