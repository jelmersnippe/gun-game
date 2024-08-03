using Godot;
using Godot.Collections;

public partial class GunRegistry : Node {
	[Signal]
	public delegate void EmptyEventHandler();

	private int _currentWeaponIndex;
	private int _prevWeaponIndex = -1;

	[Export] public bool RandomMode;
	[Export] public Array<PackedScene> WeaponScenes = new();

	public Weapon? GetNext() {
		if (RandomMode) {
			return GetRandom();
		}

		if (_currentWeaponIndex >= WeaponScenes.Count) {
			EmitSignal(SignalName.Empty);
			return null;
		}

		var weapon = WeaponScenes[_currentWeaponIndex].Instantiate<Weapon>();
		_prevWeaponIndex = _currentWeaponIndex;
		_currentWeaponIndex++;

		return weapon;
	}

	private Weapon? GetRandom() {
		if (WeaponScenes.Count == 0) {
			return null;
		}

		var rng = new RandomNumberGenerator();
		int weaponIndex = rng.RandiRange(0, WeaponScenes.Count - 1);
		if (weaponIndex == _prevWeaponIndex) {
			return GetRandom();
		}

		var weapon = WeaponScenes[weaponIndex].Instantiate<Weapon>();
		_prevWeaponIndex = weaponIndex;

		return weapon;
	}
}
