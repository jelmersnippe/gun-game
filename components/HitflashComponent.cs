using Godot;
using System;

public partial class HitflashComponent : Node
{
	private readonly ShaderMaterial _hitflashMaterialResource = ResourceLoader.Load<ShaderMaterial>("res://hit_flash_material.tres");
	
	[Export] public float FlashTime = 0.1f;
	[Export] public Color FlashColor = Colors.White;
	[Export] public CanvasItem Sprite;

	private ShaderMaterial _hitflashMaterialInstance;

	public override void _Ready()
	{
		_hitflashMaterialInstance = _hitflashMaterialResource.Duplicate() as ShaderMaterial;
		Sprite.Material = _hitflashMaterialInstance;
		
		_hitflashMaterialInstance!.SetShaderParameter("r", FlashColor.R);
		_hitflashMaterialInstance.SetShaderParameter("g", FlashColor.G);
		_hitflashMaterialInstance.SetShaderParameter("b", FlashColor.B);
	}

	public void Flash()
	{
		SetFlash(true);
		var timer = GetTree().CreateTimer(FlashTime);
		timer.Timeout += () => SetFlash(false);
	}

	private void SetFlash(bool active)
	{
		_hitflashMaterialInstance.SetShaderParameter("active", active);
	}
}