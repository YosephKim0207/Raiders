using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000212 RID: 530
[InspectorDropdownName("Bosses/Infinilich/Sweep1")]
public class InfinilichSweep1 : Script
{
	// Token: 0x060007F1 RID: 2033 RVA: 0x00026B94 File Offset: 0x00024D94
	protected override IEnumerator Top()
	{
		base.StartTask(this.VerticalAttacks());
		int frames = 45;
		for (int i = 0; i < 6; i++)
		{
			float startAngle = (float)((i % 2 != 0) ? 60 : 120);
			float endAngle = (float)((i % 2 != 0) ? (-60) : 240);
			for (int j = 0; j < frames; j++)
			{
				float angle = Mathf.Lerp(startAngle, endAngle, (float)j / ((float)frames - 1f));
				for (int k = 0; k < 1; k++)
				{
					base.Fire(new Offset(new Vector2(UnityEngine.Random.Range(0.5f, 1.5f), 0f), angle, string.Empty, DirectionType.Absolute), new Direction(angle + (float)UnityEngine.Random.Range(-5, 5), DirectionType.Absolute, -1f), new Speed((float)UnityEngine.Random.Range(9, 15), SpeedType.Absolute), new SpeedChangingBullet(12f, 30, -1));
				}
				yield return base.Wait(1);
			}
		}
		this.m_isFinished = true;
		yield break;
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x00026BB0 File Offset: 0x00024DB0
	private IEnumerator VerticalAttacks()
	{
		while (!this.m_isFinished)
		{
			float angle = (float)UnityEngine.Random.Range(60, 120);
			base.Fire(new Offset(new Vector2(1.5f, 0f), angle, string.Empty, DirectionType.Absolute), new Direction(angle, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
			angle = (float)UnityEngine.Random.Range(-60, -120);
			base.Fire(new Offset(new Vector2(0.75f, 0f), angle, string.Empty, DirectionType.Absolute), new Direction(angle, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x040007FF RID: 2047
	private bool m_isFinished;
}
