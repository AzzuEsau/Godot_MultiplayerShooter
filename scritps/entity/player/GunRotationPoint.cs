using Godot;
using System;

public partial class GunRotationPoint : Marker2D {
	#region Variables
		[Export] private Sprite2D gunSprite;

		private float orientation;
	#endregion

	#region Signals
		// [Signal] public delegate void ExampleSignalEventHandler();
	#endregion

	#region Godot Methdos
		public override void _Ready() {

		}

		public override void _Process(double delta) {
			FlipGun();
		}

		public override void _PhysicsProcess(double delta) {
			this.LookAt(GetGlobalMousePosition());
		}
    #endregion

    #region My Methods
		private void FlipGun() {
			orientation = RotationDegrees % 360;
			while(orientation < 0) orientation += 360;
			Rotation = Mathf.DegToRad(orientation);
			gunSprite.FlipV = orientation > 90 && orientation < 270;
		}

		public float GetMouseDegrees() => orientation;
    #endregion

    #region Events
    #endregion
}
