using System;
using Godot;

public partial class TimeLabel : Label {
	[Export] public Spawner Spawner = null!;

	public override void _Process(double delta) {
		TimeSpan time = TimeSpan.FromSeconds(Spawner.ElapsedTime);
		Text = time.ToString(@"mm\:ss\.fff");
	}
}
