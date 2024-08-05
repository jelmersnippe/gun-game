using Godot;

public partial class Spawner : Node2D {
	private readonly PackedScene _enemyScene = ResourceLoader.Load<PackedScene>("res://enemy.tscn");
	private readonly PackedScene _spawnVisualScene = ResourceLoader.Load<PackedScene>("res://spawn_visual.tscn");

	[Export] public int BurstSize = 5;
	[Export] public float BurstSpawnIntervalDecay = 0.01f;
	[Export] public float InitialBurstSpawnInterval = 30f;

	[Export] public float InitialSpawnInterval = 2.5f;
	[Export] public float MinimumBurstSpawnInterval = 10f;
	[Export] public float SpawnDelay = 1f;
	[Export] public float SpawnIntervalDecay = 0.001f;
	[Export] public Node2D? Target;

	[Export] public TileMap TileMap = null!;

	public float ElapsedTime { get; private set; }

	private float SpawnInterval => InitialSpawnInterval * Mathf.Exp(-SpawnIntervalDecay * ElapsedTime);

	private float BurstSpawnInterval => Mathf.Max(InitialBurstSpawnInterval - BurstSpawnIntervalDecay * ElapsedTime,
		MinimumBurstSpawnInterval);

	public override void _Ready() {
		StartSpawnInterval();
		StartBurstSpawnInterval();
	}

	public override void _Process(double delta) {
		ElapsedTime += (float)delta;
	}

	private void StartBurstSpawnInterval() {
		SceneTreeTimer? timer = GetTree().CreateTimer(BurstSpawnInterval);
		timer.Timeout += () => {
			SpawnBurst();
			StartBurstSpawnInterval();
		};
	}

	private void StartSpawnInterval() {
		SceneTreeTimer? timer = GetTree().CreateTimer(SpawnInterval);
		timer.Timeout += () => {
			Spawn();
			StartSpawnInterval();
		};
	}

	private void SpawnBurst() {
		for (int i = 0; i < BurstSize; i++) {
			Spawn();
		}
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
