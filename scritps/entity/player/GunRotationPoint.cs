using Godot;
using System;

public partial class GunRotationPoint : Marker2D {
	#region Variables
		// [Export] private int vairbaleInEditor;
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
			this.LookAt(GetGlobalMousePosition());
		}
    #endregion

    #region My Methods
    #endregion

    #region Events
    #endregion
}
