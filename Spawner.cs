using Godot;

public partial class Spawner : Node2D {
	private readonly PackedScene _enemyScene = ResourceLoader.Load<PackedScene>("res://enemy.tscn");
	private readonly PackedScene _spawnVisualScene = ResourceLoader.Load<PackedScene>("res://spawn_visual.tscn");
	[Export] public float SpawnDelay = 1f;
	[Export] public float SpawnInterval = 2.5f;
	[Export] public Node2D? Target;

	[Export] public TileMap TileMap = null!;

	public override void _Ready() {
		StartSpawnInterval();
	}

	private void StartSpawnInterval() {
		SceneTreeTimer? timer = GetTree().CreateTimer(SpawnInterval);
		timer.Timeout += () => {
			Spawn();
			StartSpawnInterval();
		};
	}

	private void Spawn() {
		var cells = TileMap.GetUsedCells(0);

		var rng = new RandomNumberGenerator();
		int cellIndex = rng.RandiRange(0, cells.Count - 1);

		Vector2 spawnPosition = ToGlobal(TileMap.MapToLocal(cells[cellIndex]));

		var spawnVisual = _spawnVisualScene.Instantiate<Node2D>();
		spawnVisual.GlobalPosition = spawnPosition;
		AddChild(spawnVisual);

		SceneTreeTimer? spawnTimer = GetTree().CreateTimer(SpawnDelay);
		spawnTimer.Timeout += () => {
			var enemy = _enemyScene.Instantiate<Enemy>();
			enemy.GlobalPosition = spawnPosition;

			enemy.Target = Target;

			AddChild(enemy);
			spawnVisual.QueueFree();
		};
	}
}
