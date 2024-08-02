using Godot;
using System;

public partial class MainMenu : CanvasLayer
{
	private void _on_play_pressed()
	{
		GD.Print("Not implemented");
		// GetTree().ChangeSceneToFile();
	}


	private void _on_armory_pressed()
	{
		GetTree().ChangeSceneToFile("res://armory.tscn");
	}


	private void _on_exit_pressed()
	{
		GetTree().Quit();
	}
}
