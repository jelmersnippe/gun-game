using Godot;

public partial class MainMenu : CanvasLayer {
	private void _on_play_pressed() {
		GetTree().ChangeSceneToFile("res://main.tscn");
	}

	private void _on_armory_pressed() {
		GetTree().ChangeSceneToFile("res://armory.tscn");
	}


	private void _on_exit_pressed() {
		GetTree().Quit();
	}
}
