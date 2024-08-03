using Godot;

public partial class Dummy : Node2D {
	[Export] public Area2D Hitbox;
	[Export] public HitflashComponent HitflashComponent;
	[Export] public float HurtTime = 0.2f;
	[Export] public AnimatedSprite2D Sprite;

	public override void _Ready() {
		Hitbox.AreaEntered += HitboxOnAreaEntered;
	}

	private void HitboxOnAreaEntered(Area2D area) {
		if (area is not Projectile) {
			return;
		}

		Sprite.Play("Hurt");
		HitflashComponent.Flash();

		SceneTreeTimer? hurtTimer = GetTree().CreateTimer(HurtTime);
		hurtTimer.Timeout += () => Sprite.Play("Idle");

		// TODO: Move to projectile code
		area.QueueFree();
	}
}