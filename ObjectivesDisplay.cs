using Godot;
using System;

public partial class ObjectivesDisplay : Label
{
	[Export] public ObjectiveController ObjectiveController;
	
	public override void _Ready()
	{
		ObjectiveController.ObjectiveUpdated += ObjectiveControllerOnObjectiveUpdated;
		ObjectiveController.ObjectiveCompleted += ObjectiveControllerOnObjectiveCompleted;
	}

	private void ObjectiveControllerOnObjectiveCompleted(Objective objective)
	{
		GD.Print("Updating objective display toc omplete");
		Text = "Objectives:\n Completed!";
	}

	private void ObjectiveControllerOnObjectiveUpdated(Objective objective)
	{
		GD.Print("Updating objective display");
		Text = "Objectives:\n " + objective.DisplayText;
	}
}
