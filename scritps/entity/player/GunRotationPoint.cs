using Godot;
using System;

public partial class GunRotationPoint : Marker2D {
	#region Variables
		[Export] private Sprite2D gunSprite;
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
			float orientation = Mathf.RadToDeg(Rotation) % 360;
			GD.Print(orientation);
			gunSprite.FlipV = orientation > 90 && orientation < 270;
		}
    #endregion

    #region Events
    #endregion
}
