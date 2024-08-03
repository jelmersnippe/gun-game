using Godot;

public partial class MovementComponent : Node {
	[Signal]
	public delegate void CollidedEventHandler(KinematicCollision2D collision);

	[Export] public CharacterBody2D Body;
	[Export] public VelocityComponent VelocityComponent;

	public override void _Process(double delta) {
		Move((float)delta);
	}

	private void Move(float delta) {
		Body.Velocity = VelocityComponent.Velocity;
		KinematicCollision2D? collision = Body.MoveAndCollide(Body.Velocity * delta, true);

		Body.MoveAndSlide();

		if (collision != null) {
			EmitSignal(SignalName.Collided, collision);
		}
	}
}
