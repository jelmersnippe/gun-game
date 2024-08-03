using System.Dynamic;
using Godot;

public abstract partial class Objective : RefCounted {
	[Signal]
	public delegate void CompletedEventHandler(Objective objective);

	private int _currentProgression;

	protected Objective(int requiredProgression) {
		RequiredProgression = requiredProgression;
	}

	private int RequiredProgression { get; }
	public string DisplayText => ObjectiveText(RequiredProgression) + $" ({_currentProgression}/{RequiredProgression})";
	protected abstract string ObjectiveText(int requiredProgression);

	protected abstract bool ValidateProgression(CombatEvent @event);
	protected abstract bool ValidateFailure(CombatEvent @event);

	public bool HandleEvent(CombatEvent @event) {
		return TryProgress(@event) || TryFailure(@event);
	}

	private bool TryProgress(CombatEvent @event) {
		if (_currentProgression >= RequiredProgression || !ValidateProgression(@event)) {
			return false;
		}

		_currentProgression++;

		if (_currentProgression >= RequiredProgression) {
			Complete();
		}

		return true;
	}

	private void Complete() {
		EmitSignal(SignalName.Completed, this);
	}

	private bool TryFailure(CombatEvent @event) {
		if (!ValidateFailure(@event)) {
			return false;
		}

		_currentProgression = 0;
		return true;
	}
}
