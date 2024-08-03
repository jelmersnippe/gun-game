using Godot;
using Godot.Collections;

public partial class GunRegistry : Node {
	private int _currentWeaponIndex;
	[Export] public Array<PackedScene> WeaponScenes = new();

	public Weapon? GetNext() {
		if (_currentWeaponIndex >= WeaponScenes.Count) {
			return null;
		}

		var weapon = WeaponScenes[_currentWeaponIndex].Instantiate<Weapon>();
		_currentWeaponIndex++;

		return weapon;
	}

	public Weapon? GetRandom() {
		if (WeaponScenes.Count == 0) {
			return null;
		}

		var rng = new RandomNumberGenerator();
		int weaponIndex = rng.RandiRange(0, WeaponScenes.Count - 1);
		var weapon = WeaponScenes[weaponIndex].Instantiate<Weapon>();

		return weapon;
	}
}
