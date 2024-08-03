using Godot;

public partial class Weapon : Node2D {
    private bool _isReloading;
    private bool _waitingForTriggerRelease;

    [Export] public float AttacksPerSecond = 6;
    [Export] public float DistanceBetweenProjectiles = 10f;
    [Export] public Node2D FirePoint;
    [Export] public bool IsAutomatic;
    [Export] public PackedScene ProjectileScene;

    [Export] public int ProjectilesPerShot = 1;
    [Export] public Node2D ShellEjectionPoint;

    [Export] public AudioStream Sound;
    [Export] public float Spread = 5f;
    [Export] public Sprite2D Sprite;

    [Export] public float UserKickback = 5f;
    [Export] public float WeaponKickback = 5f;

    private float ReloadTime => 1f / AttacksPerSecond;

    public Vector2 Fire(Vector2 direction) {
        if (_isReloading || _waitingForTriggerRelease) {
            return Vector2.Zero;
        }

        if (!IsAutomatic) {
            _waitingForTriggerRelease = true;
        }

        float middleProjectileIndex = (ProjectilesPerShot - 1) / 2f;
        for (int i = 0; i < ProjectilesPerShot; i++) {
            float offset = i - middleProjectileIndex;

            var projectile = ProjectileScene.Instantiate<Projectile>();
            projectile.GlobalPosition = FirePoint.GlobalPosition;
            projectile.Rotation = direction.Angle();

            projectile.RotationDegrees += offset * DistanceBetweenProjectiles;

            // Apply spread
            var rng = new RandomNumberGenerator();
            projectile.RotationDegrees += rng.RandfRange(-Spread, Spread);

            GetTree().CurrentScene.CallDeferred("add_child", projectile);

            var shell = projectile.Shell.Instantiate<Shell>();
            shell.GlobalPosition = ShellEjectionPoint.GlobalPosition;
            shell.Rotation = direction.Angle();
            shell.Velocity = new Vector2(-direction.Normalized().X * 50, -1 * 200);

            GetTree().CurrentScene.CallDeferred("add_child", shell);
        }

        // Apply kickback
        Position = Vector2.Zero - direction * WeaponKickback;

        Reload();

        var soundPlayer = new AudioStreamPlayer2D();
        GetParent().AddChild(soundPlayer);
        soundPlayer.Stream = Sound;
        soundPlayer.Play();
        soundPlayer.Finished += () => soundPlayer.QueueFree();

        // Perform at the end so all game tree related stuff like reloading can still happen
        for (int i = 0; i < ProjectilesPerShot; i++) {
            CombatEventHandler.HandleEvent(CombatEvent.ProjectileFired);
        }

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