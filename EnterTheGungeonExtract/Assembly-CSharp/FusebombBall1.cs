using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200019C RID: 412
[InspectorDropdownName("Bosses/Fusebomb/Ball1")]
public class FusebombBall1 : Script
{
	// Token: 0x0600062C RID: 1580 RVA: 0x0001DAE4 File Offset: 0x0001BCE4
	protected override IEnumerator Top()
	{
		float num = UnityEngine.Random.Range(-30f, 30f);
		base.Fire(new Direction(0f, DirectionType.Absolute, -1f), new Speed(3f, SpeedType.Absolute), new FusebombBall1.RollyBall());
		return null;
	}

	// Token: 0x0200019D RID: 413
	private class RollyBall : Bullet
	{
		// Token: 0x0600062D RID: 1581 RVA: 0x0001DB28 File Offset: 0x0001BD28
		public RollyBall()
			: base("ball", false, false, false)
		{
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001DB38 File Offset: 0x0001BD38
		protected override IEnumerator Top()
		{
			float num = (float)(-(float)UnityEngine.Random.Range(20, 55));
			base.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 60);
			base.ChangeDirection(new Direction(num, DirectionType.Absolute, -1f), 60);
			return null;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0001DB7C File Offset: 0x0001BD7C
		public override void OnForceRemoved()
		{
			this.Speed = 12f;
			if (this.Projectile && this.Projectile.specRigidbody && this.Projectile.specRigidbody.Velocity != Vector2.zero)
			{
				this.Projectile.specRigidbody.Velocity = this.Projectile.specRigidbody.Velocity.normalized * 12f;
			}
		}

		// Token: 0x040005FD RID: 1533
		private const float TargetSpeed = 12f;
	}
}
