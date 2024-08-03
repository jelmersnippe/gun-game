using Godot;

public partial class PauseController : CanvasLayer {
	private static PauseController? _instance;

	private bool _buttonHeld;
	private bool _paused;
	public static PauseController Instance => _instance!;

	public override void _EnterTree() {
		if (_instance != null) {
			QueueFree();
			return;
		}

		_instance = this;
	}

	public override void _Ready() {
		ProcessMode = ProcessModeEnum.Always;
		Hide();
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("pause") && !_buttonHeld) {
			Toggle();
			_buttonHeld = true;
		}

		if (@event.IsActionReleased("pause")) {
			_buttonHeld = false;
		}
	}

	public void Toggle() {
		_paused = !_paused;

		GetTree().Paused = _paused;

		if (_paused) {
			Show();
		}
		else {
			Hide();
		}
	}

	private void _on_button_pressed() {
		Toggle();
	}

	private void _on_main_menu_pressed() {
		GetTree().ChangeSceneToFile("res://main_menu.tscn");
		Toggle();
	}

	private void _on_exit_pressed() {
		GetTree().Quit();
	}

	private void _on_restart_pressed() {
		GetTree().ReloadCurrentScene();
		Toggle();
	}
}
