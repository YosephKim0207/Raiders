using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000078 RID: 120
[InspectorDropdownName("Bosses/BossFinalConvict/Grenades1")]
public abstract class BossFinalConvictGrenades1 : Script
{
	// Token: 0x060001D5 RID: 469 RVA: 0x000092DC File Offset: 0x000074DC
	public BossFinalConvictGrenades1(float minAngle, float maxAngle)
	{
		this.m_minAngle = minAngle;
		this.m_maxAngle = maxAngle;
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x000092F4 File Offset: 0x000074F4
	protected override IEnumerator Top()
	{
		int num = 2;
		this.FireGrenade(num);
		for (int i = 1; i <= 2; i++)
		{
			this.FireGrenade(num + i);
			this.FireGrenade(num - i);
		}
		return null;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00009330 File Offset: 0x00007530
	private void FireGrenade(int i)
	{
		float num = Mathf.Lerp(this.m_minAngle, this.m_maxAngle, (float)i / 4f);
		Bullet bullet = new Bullet("grenade", false, false, false);
		base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), bullet);
		ArcProjectile arcProjectile = bullet.Projectile as ArcProjectile;
		float? playerDist = this.m_playerDist;
		if (playerDist == null)
		{
			float timeInFlight = arcProjectile.GetTimeInFlight();
			Vector2 vector = this.BulletManager.PlayerPosition() + this.BulletManager.PlayerVelocity() * timeInFlight;
			this.m_playerDist = new float?(Vector2.Distance(base.Position, vector));
		}
		arcProjectile.AdjustSpeedToHit(base.Position + BraveMathCollege.DegreesToVector(num, this.m_playerDist.Value));
	}

	// Token: 0x040001EB RID: 491
	private const int NumBullets = 5;

	// Token: 0x040001EC RID: 492
	private float? m_playerDist;

	// Token: 0x040001ED RID: 493
	private readonly float m_minAngle;

	// Token: 0x040001EE RID: 494
	private readonly float m_maxAngle;
}
