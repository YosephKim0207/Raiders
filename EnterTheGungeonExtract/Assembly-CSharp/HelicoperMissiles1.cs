using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001CD RID: 461
[InspectorDropdownName("Bosses/Helicopter/Missiles1")]
public class HelicoperMissiles1 : Script
{
	// Token: 0x060006E4 RID: 1764 RVA: 0x000219A4 File Offset: 0x0001FBA4
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 12; i++)
		{
			float t = UnityEngine.Random.value;
			float speed = Mathf.Lerp(8f, 11f, t);
			Vector2 target = ((!BraveUtility.RandomBool()) ? base.GetPredictedTargetPositionExact(1f, speed) : this.BulletManager.PlayerPosition());
			base.Fire(new Offset(this.s_targets[i % 4]), new Speed(speed, SpeedType.Absolute), new HelicoperMissiles1.ArcBullet(target, t));
			base.PostWwiseEvent("Play_BOSS_RatMech_Missile_01", null);
			yield return base.Wait(10);
		}
		yield break;
	}

	// Token: 0x040006C4 RID: 1732
	public string[] s_targets = new string[] { "shoot point 1", "shoot point 2", "shoot point 3", "shoot point 4" };

	// Token: 0x020001CE RID: 462
	public class ArcBullet : Bullet
	{
		// Token: 0x060006E5 RID: 1765 RVA: 0x000219C0 File Offset: 0x0001FBC0
		public ArcBullet(Vector2 target, float t)
			: base("missile", false, false, false)
		{
			this.m_target = target;
			this.m_t = t;
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x000219E0 File Offset: 0x0001FBE0
		public override void Initialize()
		{
			tk2dSpriteAnimator spriteAnimator = this.Projectile.spriteAnimator;
			spriteAnimator.Play();
			spriteAnimator.SetFrame(spriteAnimator.CurrentClip.frames.Length - 1);
			base.Initialize();
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00021A1C File Offset: 0x0001FC1C
		protected override IEnumerator Top()
		{
			Vector2 toTarget = this.m_target - base.Position;
			float trueDirection = toTarget.ToAngle();
			Vector2 truePosition = base.Position;
			Vector2 lastPosition = base.Position;
			float travelTime = toTarget.magnitude / this.Speed * 60f - 1f;
			float magnitude = BraveUtility.RandomSign() * (1f - this.m_t) * 8f;
			Vector2 offset = magnitude * toTarget.Rotate(90f).normalized;
			base.ManualControl = true;
			this.Direction = trueDirection;
			int i = 0;
			while ((float)i < travelTime)
			{
				float angleRad = trueDirection * 0.017453292f;
				this.Velocity.x = Mathf.Cos(angleRad) * this.Speed;
				this.Velocity.y = Mathf.Sin(angleRad) * this.Speed;
				truePosition += this.Velocity / 60f;
				lastPosition = base.Position;
				base.Position = truePosition + offset * Mathf.Sin((float)base.Tick / travelTime * 3.1415927f);
				this.Direction = (base.Position - lastPosition).ToAngle();
				yield return base.Wait(1);
				i++;
			}
			Vector2 v = (base.Position - lastPosition) * 60f;
			this.Speed = v.magnitude;
			this.Direction = v.ToAngle();
			base.ManualControl = false;
			yield break;
		}

		// Token: 0x040006C5 RID: 1733
		private Vector2 m_target;

		// Token: 0x040006C6 RID: 1734
		private float m_t;
	}
}
