using Godot;

public partial class MainMenu : CanvasLayer {
	private void _on_play_pressed() {
		GetTree().ChangeSceneToFile("res://classic_mode.tscn");
	}

	private void _on_armory_pressed() {
		GetTree().ChangeSceneToFile("res://armory.tscn");
	}

	private void _on_exit_pressed() {
		GetTree().Quit();
	}
	
	private void _on_play_endless_pressed()
	{
		GetTree().ChangeSceneToFile("res://endless_mode.tscn");
	}
}
