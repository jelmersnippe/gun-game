using Godot;

[GlobalClass]
public partial class PelletVelocityStrategy : VelocityStrategy {
	public override void Apply(Projectile projectile) {
		Tween? tween = projectile.GetTree().CreateTween();

		tween.TweenProperty(projectile, "Speed", 0, projectile.LifeTime);
		tween.SetTrans(Tween.TransitionType.Expo);
		tween.SetEase(Tween.EaseType.Out);

		tween.Play();
	}
}
