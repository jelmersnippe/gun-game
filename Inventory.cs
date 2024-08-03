using Godot;

public partial class Inventory : Node {
	[Signal]
	public delegate void WeaponEquippedEventHandler(Weapon equippedWeapon);

	private readonly PackedScene _pickupScene = ResourceLoader.Load<PackedScene>("res://pickup.tscn");

	[Export] public bool CreatePickupForWeapons;
	[Export] public PackedScene? DefaultWeapon;
	public Weapon? ActiveWeapon { get; private set; }

	public override void _Ready() {
		if (DefaultWeapon == null) {
			return;
		}

		var weaponInstance = DefaultWeapon.Instantiate<Weapon>();
		Equip(weaponInstance);
	}

	public void Equip(Weapon? weapon) {
		if (ActiveWeapon != null) {
			if (CreatePickupForWeapons) {
				Node? activeWeaponParent = ActiveWeapon.GetParent();
				activeWeaponParent?.RemoveChild(ActiveWeapon);

				var droppedWeaponPickup = _pickupScene.Instantiate<Pickup>();
				droppedWeaponPickup.GlobalPosition = (GetParent() as Node2D)!.GlobalPosition;
				droppedWeaponPickup.SetWeapon(ActiveWeapon);
				ActiveWeapon.StopFiring();

				GetTree().CurrentScene.CallDeferred("add_child", droppedWeaponPickup);
			}
			else {
				ActiveWeapon.QueueFree();
			}
		}

		Node? weaponParent = weapon?.GetParent();
		weaponParent?.RemoveChild(weapon);

		ActiveWeapon = weapon;
		if (ActiveWeapon != null) {
			EmitSignal(SignalName.WeaponEquipped, ActiveWeapon);
		}
	}
}
