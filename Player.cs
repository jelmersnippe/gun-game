using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public VelocityComponent VelocityComponent;
	[Export] public InputComponent InputComponent;
	[Export] public AnimatedSprite2D Sprite;
	[Export] public Inventory Inventory;
	[Export] public Node2D Hand;
	[Export] public Area2D PickupRadius;

	[Export] public Vector2 CarryingOffset = new(24, -2);
	
	[Export] public float Speed = 200f;

	private Pickup? AvailablePickup;

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
		InputComponent.InteractInput += Interact;
		
		if (Inventory.ActiveWeapon != null) {
			EquipWeapon(Inventory.ActiveWeapon);
		}
		Inventory.WeaponEquipped += EquipWeapon;

		PickupRadius.AreaEntered += EnterPickupArea;
		PickupRadius.AreaExited += ExitPickupArea;
	}

	private void EquipWeapon(Weapon weapon)
	{
		Hand.CallDeferred("add_child", weapon);
		weapon.Sprite.Position = CarryingOffset;
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

	private void EnterPickupArea(Area2D area)
	{
		if (area is not Pickup pickup)
		{
			return;
		}
		
		AvailablePickup?.OutlineShader?.SetShaderParameter("width", 0);

		AvailablePickup = pickup;
		AvailablePickup.ShowInteractable(true);
	}
	
	private void ExitPickupArea(Area2D area)
	{
		if (area is not Pickup pickup)
		{
			return;
		}

		if (pickup == AvailablePickup)
		{
			AvailablePickup.ShowInteractable(false);
			AvailablePickup = null;
		}
	}

	private void Interact()
	{
		if (AvailablePickup?.Weapon == null)
		{
			return;
		}

		Inventory.Equip(AvailablePickup.Weapon);
		AvailablePickup.QueueFree();
	}
}
