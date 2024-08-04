using Godot;

[GlobalClass]
public partial class RocketVelocityStrategy : VelocityStrategy {
	[Export] public float TimeToMaxSpeed = 1f;

	public override void Apply(Projectile projectile) {
		Tween? tween = projectile.GetTree().CreateTween();

		float targetSpeed = projectile.Speed;
		projectile.Speed = 0;
		tween.TweenProperty(projectile, "Speed", targetSpeed, TimeToMaxSpeed);
		tween.SetTrans(Tween.TransitionType.Quint);
		tween.SetEase(Tween.EaseType.InOut);

		tween.Play();
	}
}
