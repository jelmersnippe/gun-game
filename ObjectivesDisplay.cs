using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class ObjectivesDisplay : Label {
    [Export] public ObjectiveController ObjectiveController;

    public override void _Ready() {
        ObjectiveController.ObjectivesUpdated += ObjectiveControllerOnObjectivesUpdated;
        ObjectiveController.ObjectiveCompleted += ObjectiveControllerOnObjectiveCompleted;
    }

    private void ObjectiveControllerOnObjectiveCompleted(Objective objective) {
        GD.Print("Objective completed");
        Text = "Objectives:\n Completed!";
    }

    private void ObjectiveControllerOnObjectivesUpdated(IReadOnlyList<Objective> objectives) {
        Text = "Objectives:\n" + string.Join("\n", objectives.Select(x => x.DisplayText));
    }
}