using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020000C7 RID: 199
[InspectorDropdownName("BubbleLizard/Bubble1")]
public class BubbleLizardBubble1 : Script
{
	// Token: 0x0600030A RID: 778 RVA: 0x0000FB3C File Offset: 0x0000DD3C
	protected override IEnumerator Top()
	{
		base.Fire(new Direction(0f, DirectionType.Absolute, -1f), new Speed(2f, SpeedType.Absolute), new BubbleLizardBubble1.BubbleBullet());
		return null;
	}

	// Token: 0x04000326 RID: 806
	private const float WaftXPeriod = 3f;

	// Token: 0x04000327 RID: 807
	private const float WaftXMagnitude = 1f;

	// Token: 0x04000328 RID: 808
	private const float WaftYPeriod = 1f;

	// Token: 0x04000329 RID: 809
	private const float WaftYMagnitude = 0.25f;

	// Token: 0x0400032A RID: 810
	private const int BubbleLifeTime = 960;

	// Token: 0x020000C8 RID: 200
	public class BubbleBullet : Bullet
	{
		// Token: 0x0600030B RID: 779 RVA: 0x0000FB68 File Offset: 0x0000DD68
		public BubbleBullet()
			: base("bubble", false, false, false)
		{
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000FB78 File Offset: 0x0000DD78
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
			this.Direction = base.AimDirection;
			for (int i = 0; i < 960; i++)
			{
				this.Direction = base.AimDirection;
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

		// Token: 0x0600030D RID: 781 RVA: 0x0000FB94 File Offset: 0x0000DD94
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
