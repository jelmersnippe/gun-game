using Godot;

public partial class Camera : Camera2D {
	[Export] public float MaxOffset = 100f;
	[Export] public float Smoothing = 5f;
	[Export] public Node2D? Target;

	public override void _Process(double delta) {
		if (Target == null) {
			return;
		}

		Vector2 screenSize = GetViewportRect().Size;
		Vector2 mousePosition = GetGlobalMousePosition();

		Vector2 direction = Target.GlobalPosition.DirectionTo(mousePosition);
		float distanceToMouse = Target.GlobalPosition.DistanceTo(mousePosition);

		float maxDistanceToEdge = (screenSize / 2).Length();

		Vector2 offset = direction * Mathf.Lerp(0, MaxOffset, distanceToMouse / maxDistanceToEdge);

		Vector2 desiredPosition = Target.GlobalPosition + offset;

		GlobalPosition = new Vector2(
			Mathf.Lerp(GlobalPosition.X, desiredPosition.X, Smoothing * (float)delta),
			Mathf.Lerp(GlobalPosition.Y, desiredPosition.Y, Smoothing * (float)delta)
		);
	}
}