using Godot;
using System;

public partial class Pickup : Area2D
{
	private readonly ShaderMaterial _outlineMaterialResource = ResourceLoader.Load<ShaderMaterial>("res://outline_material.tres");
	[Export] public PackedScene? WeaponScene;
	[Export] public Label Text = null!;
	
	public Weapon? Weapon { get; private set; }
	public ShaderMaterial? OutlineShader { get; private set; }
	
	public override void _Ready()
	{
		Text.Hide();
		if (WeaponScene == null)
		{
			return;
		}
		
		Weapon = WeaponScene.Instantiate<Weapon>();
		SpawnPickupVisual();
	}

	public void SetWeapon(Weapon weapon)
	{
		Weapon?.QueueFree();
		Weapon = weapon;
		
		SpawnPickupVisual();
	}

	public void SpawnPickupVisual()
	{
		if (Weapon == null)
		{
			return;
		}
		
		var outlineMaterial = _outlineMaterialResource.Duplicate() as ShaderMaterial;
		OutlineShader = outlineMaterial!;
		OutlineShader.SetShaderParameter("width", 0);

		Weapon.Sprite.Position = Vector2.Zero;
		Weapon.Sprite.FlipV = false;
		Weapon.Sprite.Material = outlineMaterial;
		ShowInteractable(false);
		
		AddChild(Weapon);
	}

	public void ShowInteractable(bool interactable)
	{
		if (interactable)
		{
			OutlineShader?.SetShaderParameter("width", 1);
			Text.Show();
		}
		else
		{
			OutlineShader?.SetShaderParameter("width", 0);
			Text.Hide();
		}
	}
}
