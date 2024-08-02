using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public ObjectiveController ObjectiveController;
	[Export] public GunRegistry GunRegistry;
	
	[Export] public VelocityComponent VelocityComponent;
	[Export] public InputComponent InputComponent;
	[Export] public AnimatedSprite2D Sprite;
	[Export] public Inventory Inventory;
	[Export] public Node2D Hand;
	[Export] public Area2D PickupRadius;

	[Export] public Vector2 CarryingOffset = new(24, -2);
	
	[Export] public float Speed = 200f;

	private Pickup? _availablePickup;

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
		
		ObjectiveController.ObjectiveCompleted += ObjectiveControllerOnObjectiveCompleted;

		var startingWeapon = GunRegistry.GetNext();
		Inventory.Equip(startingWeapon);
	}

	private void ObjectiveControllerOnObjectiveCompleted(Objective obj)
	{
		Inventory.Equip(GunRegistry.GetNext());
	}

	private void EquipWeapon(Weapon weapon)
	{
		Hand.AddChild(weapon);
		weapon.Reload();
		weapon.Sprite.Position = CarryingOffset;
	}

	private void InputComponentOnAttackInput() {
		var kickback = Inventory.ActiveWeapon?.Fire(Hand.GlobalPosition.DirectionTo(GetGlobalMousePosition()));

		if (kickback.HasValue) {
			VelocityComponent.Velocity += kickback.Value;
		}
	}

	public override void _Process(double delta)
	{
		var directionToMouse = Hand.GlobalPosition.DirectionTo(GetGlobalMousePosition());
		Hand.Rotation = directionToMouse.Angle();
		Sprite.FlipH = directionToMouse.X < 0;
		
		Inventory.ActiveWeapon?.RotateToMouse();
	}

	private void EnterPickupArea(Area2D area)
	{
		if (area is not Pickup pickup)
		{
			return;
		}
		
		_availablePickup?.ShowInteractable(false);

		_availablePickup = pickup;
		_availablePickup.ShowInteractable(true);
	}
	
	private void ExitPickupArea(Area2D area)
	{
		if (area is not Pickup pickup)
		{
			return;
		}

		if (pickup == _availablePickup)
		{
			_availablePickup.ShowInteractable(false);
			_availablePickup = null;
		}
	}

	private void Interact()
	{
		if (_availablePickup?.Weapon == null)
		{
			return;
		}

		Inventory.Equip(_availablePickup.Weapon);
		_availablePickup.QueueFree();
	}
}
