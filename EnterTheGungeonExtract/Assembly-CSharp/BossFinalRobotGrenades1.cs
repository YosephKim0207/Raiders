using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000096 RID: 150
[InspectorDropdownName("Bosses/BossFinalRobot/Grenades1")]
public class BossFinalRobotGrenades1 : Script
{
	// Token: 0x0600024F RID: 591 RVA: 0x0000B8D0 File Offset: 0x00009AD0
	protected override IEnumerator Top()
	{
		float airTime = base.BulletBank.GetBullet("grenade").BulletObject.GetComponent<ArcProjectile>().GetTimeInFlight();
		Vector2 vector = this.BulletManager.PlayerPosition();
		Bullet bullet2 = new Bullet("grenade", false, false, false);
		float num = (vector - base.Position).ToAngle();
		base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), bullet2);
		(bullet2.Projectile as ArcProjectile).AdjustSpeedToHit(vector);
		bullet2.Projectile.ImmuneToSustainedBlanks = true;
		for (int i = 0; i < 4; i++)
		{
			yield return base.Wait(6);
			Vector2 targetVelocity = this.BulletManager.PlayerVelocity();
			float startAngle;
			float dist;
			if (targetVelocity != Vector2.zero && targetVelocity.magnitude > 0.5f)
			{
				startAngle = targetVelocity.ToAngle();
				dist = targetVelocity.magnitude * airTime;
			}
			else
			{
				startAngle = base.RandomAngle();
				dist = 5f * airTime;
			}
			float angle = base.SubdivideCircle(startAngle, 4, i, 1f, false);
			Vector2 targetPoint = this.BulletManager.PlayerPosition() + BraveMathCollege.DegreesToVector(angle, dist);
			float direction = (targetPoint - base.Position).ToAngle();
			if (i > 0)
			{
				direction += UnityEngine.Random.Range(-12.5f, 12.5f);
			}
			Bullet bullet = new Bullet("grenade", false, false, false);
			base.Fire(new Direction(direction, DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), bullet);
			(bullet.Projectile as ArcProjectile).AdjustSpeedToHit(targetPoint);
			bullet.Projectile.ImmuneToSustainedBlanks = true;
		}
		yield break;
	}

	// Token: 0x04000272 RID: 626
	private const int NumBullets = 4;
}
