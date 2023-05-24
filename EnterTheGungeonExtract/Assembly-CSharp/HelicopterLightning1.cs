using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001CA RID: 458
[InspectorDropdownName("Bosses/Helicopter/Lightning1")]
public class HelicopterLightning1 : Script
{
	// Token: 0x060006DA RID: 1754 RVA: 0x000214EC File Offset: 0x0001F6EC
	protected override IEnumerator Top()
	{
		float num = BraveMathCollege.QuantizeFloat(base.AimDirection, 45f);
		base.PostWwiseEvent("Play_BOSS_agunim_ribbons_01", null);
		base.Fire(new Offset(-0.5f, 0.5f, 0f, string.Empty, DirectionType.Absolute), new HelicopterLightning1.LightningBullet(num, -1f, 40, -4, null));
		base.Fire(new Offset(-0.5f, 0.5f, 0f, string.Empty, DirectionType.Absolute), new HelicopterLightning1.LightningBullet(num, -1f, 40, 4, null));
		base.Fire(new Offset(-0.5f, 0.5f, 0f, string.Empty, DirectionType.Absolute), new HelicopterLightning1.LightningBullet(num, -1f, 40, 4, null));
		base.Fire(new Offset(0.5f, 0.5f, 0f, string.Empty, DirectionType.Absolute), new HelicopterLightning1.LightningBullet(num, 1f, 40, 4, null));
		base.Fire(new Offset(0.5f, 0.5f, 0f, string.Empty, DirectionType.Absolute), new HelicopterLightning1.LightningBullet(num, 1f, 40, 4, null));
		base.Fire(new Offset(0.5f, 0.5f, 0f, string.Empty, DirectionType.Absolute), new HelicopterLightning1.LightningBullet(num, 1f, 40, -4, null));
		return null;
	}

	// Token: 0x040006B6 RID: 1718
	public const float Dist = 0.8f;

	// Token: 0x040006B7 RID: 1719
	public const int MaxBulletDepth = 40;

	// Token: 0x040006B8 RID: 1720
	public const float RandomOffset = 0.3f;

	// Token: 0x040006B9 RID: 1721
	public const float TurnChance = 0.2f;

	// Token: 0x040006BA RID: 1722
	public const float TurnAngle = 30f;

	// Token: 0x020001CB RID: 459
	private class LightningBullet : Bullet
	{
		// Token: 0x060006DB RID: 1755 RVA: 0x00021664 File Offset: 0x0001F864
		public LightningBullet(float direction, float sign, int maxRemainingBullets, int timeSinceLastTurn, Vector2? truePosition = null)
			: base(null, false, false, false)
		{
			this.m_direction = direction;
			this.m_sign = sign;
			this.m_maxRemainingBullets = maxRemainingBullets;
			this.m_timeSinceLastTurn = timeSinceLastTurn;
			this.m_truePosition = truePosition;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00021698 File Offset: 0x0001F898
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
					HelicopterLightning1.LightningBullet lightningBullet = new HelicopterLightning1.LightningBullet(this.m_direction, this.m_sign, this.m_maxRemainingBullets - 1, this.m_timeSinceLastTurn + 1, new Vector2?(vector));
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

		// Token: 0x040006BB RID: 1723
		private float m_direction;

		// Token: 0x040006BC RID: 1724
		private float m_sign;

		// Token: 0x040006BD RID: 1725
		private int m_maxRemainingBullets;

		// Token: 0x040006BE RID: 1726
		private int m_timeSinceLastTurn;

		// Token: 0x040006BF RID: 1727
		private Vector2? m_truePosition;
	}
}
