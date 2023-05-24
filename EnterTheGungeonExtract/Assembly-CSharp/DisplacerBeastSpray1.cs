using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000154 RID: 340
[InspectorDropdownName("DisplacerBeastSpray1")]
public class DisplacerBeastSpray1 : Script
{
	// Token: 0x0600051A RID: 1306 RVA: 0x0001885C File Offset: 0x00016A5C
	protected override IEnumerator Top()
	{
		BulletLimbController[] limbs = base.BulletBank.aiAnimator.GetComponentsInChildren<BulletLimbController>();
		for (int j = 0; j < limbs.Length; j++)
		{
			limbs[j].DoingTell = true;
		}
		yield return base.Wait(54);
		base.BulletBank.aiAnimator.LockFacingDirection = true;
		yield return base.Wait(6);
		for (int k = 0; k < limbs.Length; k++)
		{
			limbs[k].DoingTell = false;
		}
		string[] transformNames = this.GetTransformNames();
		for (int i = 0; i < 20; i++)
		{
			string transformName = transformNames[i % 2];
			base.Fire(new Offset(transformName), new Direction(base.GetAimDirection(transformName) + UnityEngine.Random.RandomRange(-27f, 27f), DirectionType.Absolute, -1f), new Speed(14f, SpeedType.Absolute), new DisplacerBeastSpray1.DisplacerBullet());
			yield return base.Wait(3);
		}
		base.BulletBank.aiAnimator.LockFacingDirection = false;
		yield break;
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00018878 File Offset: 0x00016A78
	private string[] GetTransformNames()
	{
		Transform transform = base.BulletBank.transform.Find("bullet limbs").Find("back tip 1");
		if (transform && transform.gameObject.activeSelf)
		{
			return new string[] { "bullet tip 1", "back tip 1" };
		}
		return new string[] { "bullet tip 1", "bullet tip 2" };
	}

	// Token: 0x040004EE RID: 1262
	private const int NumBullets = 20;

	// Token: 0x040004EF RID: 1263
	private const float BulletSpread = 27f;

	// Token: 0x02000155 RID: 341
	public class DisplacerBullet : Bullet
	{
		// Token: 0x0600051C RID: 1308 RVA: 0x000188F0 File Offset: 0x00016AF0
		public DisplacerBullet()
			: base(null, false, false, false)
		{
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000188FC File Offset: 0x00016AFC
		protected override IEnumerator Top()
		{
			if (this.Projectile)
			{
				this.Projectile.IgnoreTileCollisionsFor(0.25f);
			}
			return null;
		}
	}
}
