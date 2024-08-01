using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public VelocityComponent VelocityComponent;
	[Export] public InputComponent InputComponent;
	[Export] public AnimatedSprite2D Sprite;
	[Export] public Inventory Inventory;
	[Export] public Node2D Hand;
	
	[Export] public float Speed = 200f;

	public override void _Ready()
	{
		InputComponent.MoveInput += movement => {
			if (movement != Vector2.Zero) {
				VelocityComponent.Velocity += movement * Speed;
				Sprite.Play("Move");
			}
			else {
				Sprite.Play("Idle");
			}
		};
		InputComponent.AttackInput += InputComponentOnAttackInput; 
		InputComponent.AttackReleased += () => Inventory.ActiveWeapon?.StopFiring();
		
		if (Inventory.ActiveWeapon != null) {
			Hand.CallDeferred("add_child", Inventory.ActiveWeapon);
		}
		Inventory.WeaponEquipped += weapon => Hand.CallDeferred("add_child", weapon);
	}

	private void InputComponentOnAttackInput() {
		Vector2? kickback = Inventory.ActiveWeapon?.Fire(Hand.GlobalPosition.DirectionTo(GetGlobalMousePosition()));

		if (kickback.HasValue) {
			VelocityComponent.Velocity += kickback.Value;
		}
	}

	public override void _Process(double delta)
	{
		var directionToMouse = Hand.GlobalPosition.DirectionTo(GetGlobalMousePosition());
		Hand.Rotation = directionToMouse.Angle();
		Sprite.FlipH = directionToMouse.X < 0;
	}
}
