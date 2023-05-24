using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000060 RID: 96
[InspectorDropdownName("Bosses/BossFinalBullet/AgunimLightning1")]
public class BossFinalBulletAgunimLightning1 : Script
{
	// Token: 0x06000175 RID: 373 RVA: 0x00007C68 File Offset: 0x00005E68
	protected override IEnumerator Top()
	{
		float num = BraveMathCollege.QuantizeFloat(base.AimDirection, 45f);
		base.Fire(new Offset("lightning left shoot point"), new BossFinalBulletAgunimLightning1.LightningBullet(num, -1f, 30, -4, null));
		base.Fire(new Offset("lightning left shoot point"), new BossFinalBulletAgunimLightning1.LightningBullet(num, -1f, 30, 4, null));
		if (BraveUtility.RandomBool())
		{
			base.Fire(new Offset("lightning left shoot point"), new BossFinalBulletAgunimLightning1.LightningBullet(num, -1f, 30, 4, null));
		}
		else
		{
			base.Fire(new Offset("lightning right shoot point"), new BossFinalBulletAgunimLightning1.LightningBullet(num, 1f, 30, 4, null));
		}
		base.Fire(new Offset("lightning right shoot point"), new BossFinalBulletAgunimLightning1.LightningBullet(num, 1f, 30, 4, null));
		base.Fire(new Offset("lightning right shoot point"), new BossFinalBulletAgunimLightning1.LightningBullet(num, 1f, 30, -4, null));
		return null;
	}

	// Token: 0x0400018A RID: 394
	public const float Dist = 0.8f;

	// Token: 0x0400018B RID: 395
	public const int MaxBulletDepth = 30;

	// Token: 0x0400018C RID: 396
	public const float RandomOffset = 0.3f;

	// Token: 0x0400018D RID: 397
	public const float TurnChance = 0.2f;

	// Token: 0x0400018E RID: 398
	public const float TurnAngle = 30f;

	// Token: 0x02000061 RID: 97
	private class LightningBullet : Bullet
	{
		// Token: 0x06000176 RID: 374 RVA: 0x00007D84 File Offset: 0x00005F84
		public LightningBullet(float direction, float sign, int maxRemainingBullets, int timeSinceLastTurn, Vector2? truePosition = null)
			: base(null, false, false, false)
		{
			this.m_direction = direction;
			this.m_sign = sign;
			this.m_maxRemainingBullets = maxRemainingBullets;
			this.m_timeSinceLastTurn = timeSinceLastTurn;
			this.m_truePosition = truePosition;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007DB8 File Offset: 0x00005FB8
		protected override IEnumerator Top()
		{
			yield return base.Wait(2);
			Vector2? truePosition = this.m_truePosition;
			if (truePosition == null)
			{
				this.m_truePosition = new Vector2?(base.Position);
			}
			if (this.m_maxRemainingBullets > 0)
			{
				if (this.m_timeSinceLastTurn > 0 && this.m_timeSinceLastTurn != 2 && this.m_timeSinceLastTurn != 3 && UnityEngine.Random.value < 0.2f)
				{
					this.m_sign *= -1f;
					this.m_timeSinceLastTurn = 0;
				}
				float num = this.m_direction + this.m_sign * 30f;
				Vector2 vector = this.m_truePosition.Value + BraveMathCollege.DegreesToVector(num, 0.8f);
				Vector2 vector2 = vector + BraveMathCollege.DegreesToVector(num + 90f, UnityEngine.Random.Range(-0.3f, 0.3f));
				if (!base.IsPointInTile(vector2))
				{
					BossFinalBulletAgunimLightning1.LightningBullet lightningBullet = new BossFinalBulletAgunimLightning1.LightningBullet(this.m_direction, this.m_sign, this.m_maxRemainingBullets - 1, this.m_timeSinceLastTurn + 1, new Vector2?(vector));
					base.Fire(Offset.OverridePosition(vector2), lightningBullet);
					if (lightningBullet.Projectile && lightningBullet.Projectile.specRigidbody && PhysicsEngine.Instance.OverlapCast(lightningBullet.Projectile.specRigidbody, null, true, false, null, null, false, null, null, new SpeculativeRigidbody[0]))
					{
						lightningBullet.Projectile.DieInAir(false, true, true, false);
					}
				}
			}
			yield return base.Wait(30);
			base.Vanish(true);
			yield break;
		}

		// Token: 0x0400018F RID: 399
		private float m_direction;

		// Token: 0x04000190 RID: 400
		private float m_sign;

		// Token: 0x04000191 RID: 401
		private int m_maxRemainingBullets;

		// Token: 0x04000192 RID: 402
		private int m_timeSinceLastTurn;

		// Token: 0x04000193 RID: 403
		private Vector2? m_truePosition;
	}
}
