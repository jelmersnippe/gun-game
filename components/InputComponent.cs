using Godot;
using System;

public partial class InputComponent : Node
{
	[Signal]
	public delegate void MoveInputEventHandler(Vector2 movement);
	
	[Signal]
	public delegate void AttackInputEventHandler();
	
	[Signal]
	public delegate void AttackReleasedEventHandler();
	
	public override void _PhysicsProcess(double delta)
	{
		var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		EmitSignal(SignalName.MoveInput, direction.Normalized());

		if (Input.IsActionPressed("attack"))
		{
			EmitSignal(SignalName.AttackInput);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionReleased("attack"))
		{
			EmitSignal(SignalName.AttackReleased);
		}
	}
}