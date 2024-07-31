using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public VelocityComponent VelocityComponent;
	[Export] public InputComponent InputComponent;
	[Export] public AnimatedSprite2D Sprite;
	[Export] public Inventory Inventory;
	[Export] public Node2D Hand;

	public override void _Ready()
	{
		InputComponent.MoveInput += movement => VelocityComponent.Velocity = movement;
		InputComponent.AttackInput += () => Inventory.ActiveWeapon?.Fire(Hand.GlobalPosition.DirectionTo(GetGlobalMousePosition()));
		InputComponent.AttackReleased += () => Inventory.ActiveWeapon?.StopFiring();
		
		if (Inventory.ActiveWeapon != null) {
			Hand.CallDeferred("add_child", Inventory.ActiveWeapon);
		}
		Inventory.WeaponEquipped += weapon => Hand.CallDeferred("add_child", weapon);
	}

	public override void _Process(double delta)
	{
		if (VelocityComponent.Velocity != Vector2.Zero) {
			Sprite.Play("Move");
		} else {
			Sprite.Play("Idle");
		}
		var directionToMouse = Hand.GlobalPosition.DirectionTo(GetGlobalMousePosition());
		Hand.Rotation = directionToMouse.Angle();
		Sprite.FlipH = directionToMouse.X < 0;
	}
}
