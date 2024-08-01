using Godot;
using System;

public partial class Dummy : Node2D
{
	[Export] public float HurtTime = 0.2f;
	[Export] public Area2D Hitbox;
	[Export] public AnimatedSprite2D Sprite;
	[Export] public HitflashComponent HitflashComponent;
	
	public override void _Ready()
	{
		Hitbox.AreaEntered += HitboxOnAreaEntered;
	}

	private void HitboxOnAreaEntered(Area2D area)
	{
		if (area is not Projectile)
		{
			return;
		} 
		
		Sprite.Play("Hurt");
		HitflashComponent.Flash();

		var hurtTimer = GetTree().CreateTimer(HurtTime);
		hurtTimer.Timeout += () => Sprite.Play("Idle");
		
		// TODO: Move to projectile code
		area.QueueFree();
	}
}
