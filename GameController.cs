using Godot;

public partial class GameController : Node {
	[Export] public GunRegistry GunRegistry = null!;

	public override void _Ready() {
		GunRegistry.Empty += () => { GameOverMenu.Instance.Activate("GUN REGISTRY COMPLETED!"); };
	}
}
