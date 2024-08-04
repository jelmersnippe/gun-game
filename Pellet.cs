using Godot;

public partial class Pellet : Projectile {
	private Tween _tween = null!;

	public override void _Ready() {
		base._Ready();

		_tween = GetTree().CreateTween();

		_tween.TweenProperty(this, "Speed", 0, LifeTime);
		_tween.SetTrans(Tween.TransitionType.Expo);
		_tween.SetEase(Tween.EaseType.Out);

		_tween.Play();
	}
}
