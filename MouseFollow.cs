using Godot;

public partial class MouseFollow : Sprite2D {
	public override void _Process(double delta) {
		GlobalPosition = GetGlobalMousePosition();
	}
}