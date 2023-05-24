using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000072 RID: 114
[InspectorDropdownName("Bosses/BossFinalBullet/GunonRing2")]
public class BossFinalBulletGunonRing2 : Script
{
	// Token: 0x060001BE RID: 446 RVA: 0x00008F4C File Offset: 0x0000714C
	protected override IEnumerator Top()
	{
		base.Fire(new BossFinalBulletGunonRing2.BatBullet());
		yield return base.Wait(30);
		yield break;
	}

	// Token: 0x040001D6 RID: 470
	private const float ExpandSpeed = 3f;

	// Token: 0x040001D7 RID: 471
	private const float ExpandAcceleration = -0.3f;

	// Token: 0x040001D8 RID: 472
	private const float RotationalSpeed = 13f;

	// Token: 0x02000073 RID: 115
	public class BatBullet : Bullet
	{
		// Token: 0x060001BF RID: 447 RVA: 0x00008F68 File Offset: 0x00007168
		public BatBullet()
			: base("bat", false, false, false)
		{
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008F78 File Offset: 0x00007178
		protected override IEnumerator Top()
		{
			this.Projectile.specRigidbody.CollideWithTileMap = false;
			Vector2 center = base.Position;
			float radius = 0.5f;
			float expandSpeed = 3f;
			while (base.Tick < 360)
			{
				expandSpeed += -0.0050000004f;
				radius += expandSpeed / 60f;
				this.m_angle -= Mathf.Min(360f, 13f / (radius * 3.1415927f) * 360f) / 60f;
				base.Position = center + BraveMathCollege.DegreesToVector(this.m_angle, radius);
				if (base.Tick >= 10 && base.Tick % 12 == 0 && !base.IsPointInTile(base.Position))
				{
					base.Fire(new BossFinalBulletGunonRing2.FireBullet());
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040001D9 RID: 473
		private float m_angle;

		// Token: 0x040001DA RID: 474
		private int m_index;

		// Token: 0x040001DB RID: 475
		private BossFinalBulletGunonRing1 m_parentScript;
	}

	// Token: 0x02000075 RID: 117
	public class FireBullet : Bullet
	{
		// Token: 0x060001C7 RID: 455 RVA: 0x0000916C File Offset: 0x0000736C
		public FireBullet()
			: base("fire", false, false, false)
		{
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000917C File Offset: 0x0000737C
		protected override IEnumerator Top()
		{
			yield return base.Wait(90);
			base.Vanish(false);
			yield break;
		}
	}
}
