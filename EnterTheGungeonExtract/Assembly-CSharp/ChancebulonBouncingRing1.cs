using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000132 RID: 306
[InspectorDropdownName("Chancebulon/BouncingRing1")]
public class ChancebulonBouncingRing1 : Script
{
	// Token: 0x06000486 RID: 1158 RVA: 0x000159F0 File Offset: 0x00013BF0
	protected override IEnumerator Top()
	{
		float num = base.GetAimDirection((float)((UnityEngine.Random.value >= 0.4f) ? 0 : 1), 8f) + UnityEngine.Random.Range(-10f, 10f);
		for (int i = 0; i < 18; i++)
		{
			float num2 = (float)i * 20f;
			Vector2 vector = BraveMathCollege.DegreesToVector(num2, 1.8f);
			base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new ChancebulonBouncingRing1.BouncingRingBullet("bouncingRing", vector));
		}
		base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new ChancebulonBouncingRing1.BouncingRingBullet("bouncingRing", new Vector2(-0.7f, 0.7f)));
		base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new ChancebulonBouncingRing1.BouncingRingBullet("bouncingMouth", new Vector2(0f, 0.4f)));
		base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new ChancebulonBouncingRing1.BouncingRingBullet("bouncingRing", new Vector2(0.7f, 0.7f)));
		return null;
	}

	// Token: 0x04000465 RID: 1125
	private const int NumBullets = 18;

	// Token: 0x02000133 RID: 307
	public class BouncingRingBullet : Bullet
	{
		// Token: 0x06000487 RID: 1159 RVA: 0x00015B24 File Offset: 0x00013D24
		public BouncingRingBullet(string name, Vector2 desiredOffset)
			: base(name, false, false, false)
		{
			this.m_desiredOffset = desiredOffset;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00015B38 File Offset: 0x00013D38
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

		// Token: 0x04000466 RID: 1126
		private Vector2 m_desiredOffset;
	}
}
