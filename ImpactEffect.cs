using Godot;
using System;

public partial class ImpactEffect : AnimatedSprite2D
{
	public override void _Ready()
	{
		AnimationFinished += QueueFree;
	}
}
