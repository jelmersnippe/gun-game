using Godot;

public partial class Player : CharacterBody2D {
	private Pickup? _availablePickup;
	private int _enemiesKilled;

	[Export] public Vector2 CarryingOffset = new(24, -2);

	[Export] public bool DropWeapons;

	[Export] public GunRegistry? GunRegistry;
	[Export] public Node2D Hand;
	[Export] public HealthComponent HealthComponent;
	[Export] public HurtboxComponent HurtboxComponent;
	[Export] public InputComponent InputComponent;
	[Export] public Inventory Inventory;

	[Export] public int KillsPerWeapon = 20;
	[Export] public Area2D PickupRadius;

	[Export] public float Speed = 200f;
	[Export] public AnimatedSprite2D Sprite;
	[Export] public VelocityComponent VelocityComponent;

	public override void _Ready() {
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

		Inventory.CreatePickupForWeapons = DropWeapons;
		if (Inventory.ActiveWeapon != null) {
			EquipWeapon(Inventory.ActiveWeapon);
		}

		Inventory.WeaponEquipped += EquipWeapon;

		PickupRadius.AreaEntered += EnterPickupArea;
		PickupRadius.AreaExited += ExitPickupArea;

		Weapon? startingWeapon = GunRegistry?.GetNext();
		Inventory.Equip(startingWeapon);

		HurtboxComponent.Hit += (component, direction) => HealthComponent.TakeDamage(component.ContactDamage);
		HealthComponent.Died += Die;

		CombatEventHandler.CombatEventTriggered += CombatEventHandlerOnCombatEventTriggered;
	}

	private void CombatEventHandlerOnCombatEventTriggered(CombatEvent obj) {
		if (obj.Type != CombatEventType.EnemyKilled) {
			return;
		}

		_enemiesKilled++;

		if (_enemiesKilled % KillsPerWeapon == 0) {
			Inventory.Equip(GunRegistry?.GetNext());
		}
	}

	private void Die() {
		// TODO: Spawn effects
		ShowGameOver("GAME OVER");
	}

	private void ShowGameOver(string title) {
		GameOverMenu.Instance.Activate(title);
		QueueFree();
	}

	private void EquipWeapon(Weapon weapon) {
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

	public override void _Process(double delta) {
		Vector2 directionToMouse = Hand.GlobalPosition.DirectionTo(GetGlobalMousePosition());
		Hand.Rotation = directionToMouse.Angle();
		Sprite.FlipH = directionToMouse.X < 0;

		Inventory.ActiveWeapon?.RotateToMouse();
	}

	private void EnterPickupArea(Area2D area) {
		if (area is not Pickup pickup) {
			return;
		}

		_availablePickup?.ShowInteractable(false);

		_availablePickup = pickup;
		_availablePickup.ShowInteractable(true);
	}

	private void ExitPickupArea(Area2D area) {
		if (area is not Pickup pickup) {
			return;
		}

		if (pickup == _availablePickup) {
			_availablePickup.ShowInteractable(false);
			_availablePickup = null;
		}
	}

	private void Interact() {
		if (_availablePickup?.Weapon == null) {
			return;
		}

		Inventory.Equip(_availablePickup.Weapon);
		_availablePickup.QueueFree();
	}
}
