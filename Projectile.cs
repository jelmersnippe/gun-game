using Godot;
using System;

public partial class Projectile : Node2D {
	[Export] public PackedScene Shell;
	[Export] public float Speed = 200f;
	
	public override void _Process(double delta)
	{
		Position += Transform.X * Speed * (float)delta;
	}
}
