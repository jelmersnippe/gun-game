using Godot;

public partial class Weapon : Node2D {
	private bool _isReloading;
	private bool _waitingForTriggerRelease;

	[Export] public float AttacksPerSecond = 6;
	[Export] public Node2D FirePoint;

	[Export] public FiringMode FiringMode;
	[Export] public bool IsAutomatic;
	[Export] public PackedScene ProjectileScene;

	[Export] public Node2D ShellEjectionPoint;
	[Export] public ShotPattern ShotPattern;

	[Export] public AudioStream Sound;
	[Export] public Sprite2D Sprite;

	[Export] public float UserKickback = 5f;
	[Export] public float WeaponKickback = 5f;

	private float ReloadTime => 1f / AttacksPerSecond;

	public override void _Ready() {
		FiringMode.Fired += projectiles => {
			foreach (Projectile projectile in projectiles) {
				GetTree().CurrentScene.CallDeferred("add_child", projectile);
				CombatEventHandler.HandleEvent(new ProjectileFiredCombatEvent(projectile));
			}
		};
	}

	public Vector2 Fire(Vector2 direction) {
		if (_isReloading || _waitingForTriggerRelease) {
			return Vector2.Zero;
		}

		if (!IsAutomatic) {
			_waitingForTriggerRelease = true;
		}

		FiringMode.Fire(this);

		// Apply kickback
		Position = Vector2.Zero - direction * WeaponKickback;

		Reload();

		return -direction.Normalized() * UserKickback;
	}

	public override void _Process(double delta) {
		Position = Position.MoveToward(Vector2.Zero, 30 * (float)delta);
	}

	public void StopFiring() {
		_waitingForTriggerRelease = false;
	}

	public void RotateToMouse() {
		Vector2 directionToMouse = GlobalPosition.DirectionTo(GetGlobalMousePosition());
		Sprite.FlipV = directionToMouse.X < 0;
	}

	public void Reload() {
		_isReloading = true;
		SceneTreeTimer? reloadTimer = GetTree().CreateTimer(ReloadTime);
		reloadTimer.Timeout += () => _isReloading = false;
	}
}
