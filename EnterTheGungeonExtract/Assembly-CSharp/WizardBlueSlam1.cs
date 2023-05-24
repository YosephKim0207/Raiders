using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000322 RID: 802
public class WizardBlueSlam1 : Script
{
	// Token: 0x06000C65 RID: 3173 RVA: 0x0003BB48 File Offset: 0x00039D48
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		for (int i = 0; i < 22; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				base.Fire(new Direction(16.363636f * (float)i, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new WizardBlueSlam1.ClusterBullet(this, 120f * (float)j));
			}
		}
		this.aimDirection = (this.BulletManager.PlayerPosition() - base.Position).ToAngle();
		yield return base.Wait(36);
		this.aimDirection = (this.BulletManager.PlayerPosition() - base.Position).ToAngle();
		yield break;
	}

	// Token: 0x04000D0E RID: 3342
	private const int BulletClusters = 22;

	// Token: 0x04000D0F RID: 3343
	private const int BulletsPerCluster = 3;

	// Token: 0x04000D10 RID: 3344
	private const float ClusterRadius = 0.4f;

	// Token: 0x04000D11 RID: 3345
	private const float ClusterRotationDegPerFrame = -8f;

	// Token: 0x04000D12 RID: 3346
	public float aimDirection;

	// Token: 0x02000323 RID: 803
	public class ClusterBullet : Bullet
	{
		// Token: 0x06000C66 RID: 3174 RVA: 0x0003BB64 File Offset: 0x00039D64
		public ClusterBullet(WizardBlueSlam1 parent, float clusterAngle)
			: base("Trio", false, false, false)
		{
			this.parent = parent;
			this.clusterAngle = clusterAngle;
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0003BB84 File Offset: 0x00039D84
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 centerPosition = base.Position;
			for (int i = 0; i < 36; i++)
			{
				base.UpdateVelocity();
				centerPosition += this.Velocity / 60f;
				this.clusterAngle += -8f;
				base.Position = centerPosition + BraveMathCollege.DegreesToVector(this.clusterAngle, 0.4f);
				yield return base.Wait(1);
			}
			this.Direction = this.parent.aimDirection;
			this.Speed = 8f;
			for (int j = 0; j < 300; j++)
			{
				base.UpdateVelocity();
				centerPosition += this.Velocity / 60f;
				this.clusterAngle += -8f;
				base.Position = centerPosition + BraveMathCollege.DegreesToVector(this.clusterAngle, 0.4f);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000D13 RID: 3347
		private WizardBlueSlam1 parent;

		// Token: 0x04000D14 RID: 3348
		private float clusterAngle;
	}
}
