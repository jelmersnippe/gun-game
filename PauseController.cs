using Godot;

public partial class PauseController : CanvasLayer {
	private bool _buttonHeld;
	private bool _paused;

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

	private void Toggle() {
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
}
