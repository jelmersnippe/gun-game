using Godot;
using System;
using System.Linq;
using Godot.Collections;

public partial class GunRegistry : Node
{
	[Export] public Array<PackedScene> WeaponScenes = new();
	[Export] public bool RemoveOnAcquire = true;

	public Weapon? GetNext()
	{
		if (WeaponScenes.Count == 0)
		{
			return null;
		}

		var weapon = WeaponScenes.First().Instantiate<Weapon>();

		if (RemoveOnAcquire)
		{
			WeaponScenes.RemoveAt(0);
		}
		
		return weapon;
	}

	public Weapon? GetRandom()
	{
		if (WeaponScenes.Count == 0)
		{
			return null;
		}
		
		var rng = new RandomNumberGenerator();
		var weaponIndex = rng.RandiRange(0, WeaponScenes.Count - 1);
		var weapon = WeaponScenes[weaponIndex].Instantiate<Weapon>();
		
		if (RemoveOnAcquire)
		{
			WeaponScenes.RemoveAt(weaponIndex);
		}

		return weapon;
	}
}
