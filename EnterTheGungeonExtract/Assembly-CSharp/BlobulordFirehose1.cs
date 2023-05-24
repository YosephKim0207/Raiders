using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200003C RID: 60
[InspectorDropdownName("Bosses/Blobulord/Firehose1")]
public class BlobulordFirehose1 : Script
{
	// Token: 0x060000E0 RID: 224 RVA: 0x000059A0 File Offset: 0x00003BA0
	protected override IEnumerator Top()
	{
		float aim = base.AimDirection;
		for (int i = 0; i < 210; i++)
		{
			float newAim = base.AimDirection;
			aim = Mathf.MoveTowardsAngle(aim, newAim, 1f);
			float t = Mathf.PingPong((float)i / 60f, 1f);
			Bullet bullet;
			if ((t < 0.1f || t > 0.9f) && UnityEngine.Random.value < 0.2f)
			{
				bullet = new BlobulordFirehose1.FirehoseBullet((float)((t >= 0.1f) ? 1 : (-1)));
			}
			else
			{
				bullet = new Bullet("firehose", false, false, false);
			}
			base.Fire(new Offset(UnityEngine.Random.insideUnitCircle * 0.5f, 0f, string.Empty, DirectionType.Absolute), new Direction(aim + Mathf.SmoothStep(-35f, 35f, t), DirectionType.Absolute, -1f), new Speed(14f, SpeedType.Absolute), bullet);
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x040000E6 RID: 230
	private const float SpawnVariance = 0.5f;

	// Token: 0x040000E7 RID: 231
	private const float WobbleRange = 35f;

	// Token: 0x040000E8 RID: 232
	private const float BreakAwayChance = 0.2f;

	// Token: 0x0200003D RID: 61
	public class FirehoseBullet : Bullet
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x000059BC File Offset: 0x00003BBC
		public FirehoseBullet(float direction)
			: base("firehose", false, false, false)
		{
			this.m_direction = direction;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000059D4 File Offset: 0x00003BD4
		protected override IEnumerator Top()
		{
			yield return base.Wait(UnityEngine.Random.Range(5, 30));
			this.Direction += this.m_direction * UnityEngine.Random.Range(10f, 25f);
			yield break;
		}

		// Token: 0x040000E9 RID: 233
		private float m_direction;
	}
}
