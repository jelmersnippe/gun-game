using Godot;
using System;

public partial class Inventory : Node
{
	[Signal]
	public delegate void WeaponEquippedEventHandler(Weapon equippedWeapon);
	
	[Export] public PackedScene DefaultWeapon;
	public Weapon ActiveWeapon { get; private set; }

	public override void _Ready()
	{
		Equip(DefaultWeapon);
	}

	public void Equip(PackedScene weapon)
	{
		ActiveWeapon?.QueueFree();

		ActiveWeapon = weapon.Instantiate<Weapon>();
		EmitSignal(SignalName.WeaponEquipped, weapon);
	}
}
