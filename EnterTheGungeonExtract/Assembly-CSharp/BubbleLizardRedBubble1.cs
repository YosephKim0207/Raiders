using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020000CA RID: 202
[InspectorDropdownName("BubbleLizard/RedBubble1")]
public class BubbleLizardRedBubble1 : Script
{
	// Token: 0x06000315 RID: 789 RVA: 0x0000FEBC File Offset: 0x0000E0BC
	protected override IEnumerator Top()
	{
		float num = base.RandomAngle();
		float num2 = 90f;
		for (int i = 0; i < 4; i++)
		{
			base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(2f, SpeedType.Absolute), new BubbleLizardRedBubble1.BubbleBullet());
		}
		return null;
	}

	// Token: 0x04000336 RID: 822
	private const int NumBullets = 4;

	// Token: 0x04000337 RID: 823
	private const float WaftXPeriod = 3f;

	// Token: 0x04000338 RID: 824
	private const float WaftXMagnitude = 1f;

	// Token: 0x04000339 RID: 825
	private const float WaftYPeriod = 1f;

	// Token: 0x0400033A RID: 826
	private const float WaftYMagnitude = 0.25f;

	// Token: 0x0400033B RID: 827
	private const int BubbleLifeTime = 960;

	// Token: 0x0400033C RID: 828
	private const int DumbfireTime = 300;

	// Token: 0x020000CB RID: 203
	public class BubbleBullet : Bullet
	{
		// Token: 0x06000316 RID: 790 RVA: 0x0000FF10 File Offset: 0x0000E110
		public BubbleBullet()
			: base("bubble", false, false, false)
		{
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000FF20 File Offset: 0x0000E120
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 truePosition = base.Position;
			this.Projectile.spriteAnimator.Play("bubble_projectile_spawn");
			int animTime = Mathf.RoundToInt(this.Projectile.spriteAnimator.CurrentClip.BaseClipLength * 60f);
			float speed = this.Speed;
			this.Speed = 0f;
			yield return base.Wait(animTime);
			this.Speed = speed;
			for (int i = 0; i < 960; i++)
			{
				if (i > 300)
				{
					this.Direction = base.AimDirection;
				}
				base.UpdateVelocity();
				truePosition += this.Velocity / 60f;
				float t = (float)i / 60f;
				float waftXOffset = Mathf.Sin(t * 3.1415927f / 3f) * 1f;
				float waftYOffset = Mathf.Sin(t * 3.1415927f / 1f) * 0.25f;
				base.Position = truePosition + new Vector2(waftXOffset, waftYOffset);
				yield return base.Wait(1);
			}
			this.Projectile.spriteAnimator.Play("bubble_projectile_burst");
			animTime = Mathf.RoundToInt(this.Projectile.spriteAnimator.CurrentClip.BaseClipLength * 60f);
			yield return base.Wait(animTime);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000FF3C File Offset: 0x0000E13C
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			base.Fire(new Direction(base.GetAimDirection(1f, 14f), DirectionType.Absolute, -1f), new Speed(14f, SpeedType.Absolute), null);
		}
	}
}
