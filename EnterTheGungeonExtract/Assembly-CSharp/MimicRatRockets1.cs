using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000290 RID: 656
public class MimicRatRockets1 : Script
{
	// Token: 0x06000A0A RID: 2570 RVA: 0x00030890 File Offset: 0x0002EA90
	protected override IEnumerator Top()
	{
		Vector2 leftGun = base.BulletBank.GetTransform("left gun").transform.position.XY();
		this.FireRocket(leftGun, -5, -5);
		this.FireRocket(leftGun, -5, 5);
		this.FireRocket(leftGun, 5, -5);
		this.FireRocket(leftGun, 5, 5);
		yield return base.Wait(42);
		Vector2 rightGun = base.BulletBank.GetTransform("right gun").transform.position.XY();
		this.FireRocket(rightGun, -5, -5);
		this.FireRocket(rightGun, -5, 5);
		this.FireRocket(rightGun, 5, -5);
		this.FireRocket(rightGun, 5, 5);
		yield break;
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x000308AC File Offset: 0x0002EAAC
	private void FireRocket(Vector2 start, int xOffset, int yOffset)
	{
		for (int i = 0; i < 3; i++)
		{
			if (i != 1 || !BraveUtility.RandomBool())
			{
				Vector2 vector = ((!BraveUtility.RandomBool()) ? base.GetPredictedTargetPosition(1f, 14f) : this.BulletManager.PlayerPosition());
				vector += BraveMathCollege.DegreesToVector(base.RandomAngle(), UnityEngine.Random.Range(0f, 2.5f));
				float num = (float)(i - 1) * UnityEngine.Random.Range(25f, 90f);
				base.Fire(Offset.OverridePosition(start + new Vector2((float)xOffset / 16f, (float)yOffset / 16f)), new Direction(0f, DirectionType.Aim, -1f), new Speed(UnityEngine.Random.Range(10f, 11f), SpeedType.Absolute), new MimicRatRockets1.ArcBullet(vector, num));
			}
		}
	}

	// Token: 0x02000291 RID: 657
	public class ArcBullet : Bullet
	{
		// Token: 0x06000A0C RID: 2572 RVA: 0x00030994 File Offset: 0x0002EB94
		public ArcBullet(Vector2 target, float offsetAngle)
			: base(null, false, false, false)
		{
			this.m_target = target;
			this.m_offsetAngle = offsetAngle;
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x000309B0 File Offset: 0x0002EBB0
		protected override IEnumerator Top()
		{
			this.Direction += this.m_offsetAngle;
			float turnDelta = Mathf.Abs(this.m_offsetAngle * 2f) / ((this.m_target - base.Position).magnitude / this.Speed);
			for (int i = 0; i < 120; i++)
			{
				float targetDirection = (this.m_target - base.Position).ToAngle();
				if (BraveMathCollege.AbsAngleBetween(this.Direction, targetDirection) > 145f)
				{
					break;
				}
				base.ChangeDirection(new Direction(targetDirection, DirectionType.Absolute, turnDelta / 75f), 1);
				yield return base.Wait(1);
			}
			yield break;
		}

		// Token: 0x04000A54 RID: 2644
		private Vector2 m_target;

		// Token: 0x04000A55 RID: 2645
		private float m_offsetAngle;
	}
}
