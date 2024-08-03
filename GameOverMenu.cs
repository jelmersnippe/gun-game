using Godot;

public partial class GameOverMenu : CanvasLayer {
	private static GameOverMenu? _instance;
	[Export] public Label Title = null!;
	public static GameOverMenu Instance => _instance!;

	public override void _EnterTree() {
		if (_instance != null) {
			QueueFree();
			return;
		}

		_instance = this;
	}

	public void Activate(string text) {
		GetTree().Paused = true;

		Title.Text = text;
		Show();
	}

	public override void _Ready() {
		Hide();
	}

	private void _on_restart_pressed() {
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
		Hide();
	}


	private void _on_main_menu_pressed() {
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://main_menu.tscn");
		Hide();
	}


	private void _on_exit_pressed() {
		GetTree().Paused = false;
		GetTree().Quit();
	}
}
