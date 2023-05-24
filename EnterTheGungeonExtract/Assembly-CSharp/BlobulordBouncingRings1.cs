using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000038 RID: 56
[InspectorDropdownName("Bosses/Blobulord/BouncingRings1")]
public class BlobulordBouncingRings1 : Script
{
	// Token: 0x060000D0 RID: 208 RVA: 0x00005398 File Offset: 0x00003598
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 8; i++)
		{
			float aim = base.GetAimDirection((float)((UnityEngine.Random.value >= 0.4f) ? 0 : 1), 8f) + UnityEngine.Random.Range(-10f, 10f);
			for (int j = 0; j < 18; j++)
			{
				float num = (float)j * 20f;
				Vector2 vector = BraveMathCollege.DegreesToVector(num, 1.8f);
				base.Fire(new Direction(aim, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new BlobulordBouncingRings1.BouncingRingBullet("bouncingRing", vector));
			}
			base.Fire(new Direction(aim, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new BlobulordBouncingRings1.BouncingRingBullet("bouncingRing", new Vector2(-0.7f, 0.7f)));
			base.Fire(new Direction(aim, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new BlobulordBouncingRings1.BouncingRingBullet("bouncingMouth", new Vector2(0f, 0.4f)));
			base.Fire(new Direction(aim, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new BlobulordBouncingRings1.BouncingRingBullet("bouncingRing", new Vector2(0.7f, 0.7f)));
			yield return base.Wait(40);
		}
		yield break;
	}

	// Token: 0x040000CF RID: 207
	private const int NumBlobs = 8;

	// Token: 0x040000D0 RID: 208
	private const int NumBullets = 18;

	// Token: 0x02000039 RID: 57
	public class BouncingRingBullet : Bullet
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x000053B4 File Offset: 0x000035B4
		public BouncingRingBullet(string name, Vector2 desiredOffset)
			: base(name, false, false, false)
		{
			this.m_desiredOffset = desiredOffset;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000053C8 File Offset: 0x000035C8
		protected override IEnumerator Top()
		{
			Vector2 centerPoint = base.Position;
			Vector2 lowestOffset = BraveMathCollege.DegreesToVector(-90f, 1.5f);
			Vector2 currentOffset = Vector2.zero;
			float squishFactor = 1f;
			float verticalOffset = 0f;
			int unsquishIndex = 100;
			base.ManualControl = true;
			for (int i = 0; i < 300; i++)
			{
				if (i < 30)
				{
					currentOffset = Vector2.Lerp(Vector2.zero, this.m_desiredOffset, (float)i / 30f);
				}
				verticalOffset = (Mathf.Abs(Mathf.Cos((float)i / 90f * 3.1415927f)) - 1f) * 2.5f;
				if (unsquishIndex <= 10)
				{
					squishFactor = Mathf.Abs(Mathf.SmoothStep(0.6f, 1f, (float)unsquishIndex / 10f));
					unsquishIndex++;
				}
				Vector2 relativeOffset = currentOffset - lowestOffset;
				Vector2 squishedOffset = lowestOffset + relativeOffset.Scale(1f, squishFactor);
				base.UpdateVelocity();
				centerPoint += this.Velocity / 60f;
				base.Position = centerPoint + squishedOffset + new Vector2(0f, verticalOffset);
				if (i % 90 == 45)
				{
					for (int j = 1; j <= 10; j++)
					{
						squishFactor = Mathf.Abs(Mathf.SmoothStep(1f, 0.5f, (float)j / 10f));
						relativeOffset = currentOffset - lowestOffset;
						squishedOffset = lowestOffset + relativeOffset.Scale(1f, squishFactor);
						centerPoint += 0.333f * this.Velocity / 60f;
						base.Position = centerPoint + squishedOffset + new Vector2(0f, verticalOffset);
						yield return base.Wait(1);
					}
					unsquishIndex = 1;
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040000D1 RID: 209
		private Vector2 m_desiredOffset;
	}
}
