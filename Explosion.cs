using Godot;

public partial class Explosion : Node2D {
	[Export] public AudioStreamPlayer2D AudioStreamPlayer2D = null!;

	public override void _Ready() {
		AudioStreamPlayer2D.Finished += QueueFree;
	}
}
