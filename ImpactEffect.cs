using Godot;

public partial class ImpactEffect : Node2D {
	[Export] public AnimatedSprite2D Sprite = null!;

	public override void _Ready() {
		Sprite.AnimationFinished += QueueFree;
	}
}
