using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200009B RID: 155
[InspectorDropdownName("Bosses/BossFinalRobot/Uzi1")]
public class BossFinalRobotUzi1 : Script
{
	// Token: 0x06000261 RID: 609 RVA: 0x0000BF6C File Offset: 0x0000A16C
	protected override IEnumerator Top()
	{
		bool offhand = false;
		int i = 0;
		while ((float)i < 70f)
		{
			float angle;
			if (UnityEngine.Random.value < this.NarrowAngleChance)
			{
				angle = base.GetAimDirection("left hand shoot point") + UnityEngine.Random.Range(-this.NarrowAngle, this.NarrowAngle);
			}
			else
			{
				angle = UnityEngine.Random.Range(65f, 295f);
			}
			base.Fire(new Offset("left hand shoot point"), new Direction(angle, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
			if (UnityEngine.Random.value < this.NarrowAngleChance)
			{
				angle = base.GetAimDirection("right hand shoot point") + UnityEngine.Random.Range(-this.NarrowAngle, this.NarrowAngle);
			}
			else
			{
				angle = UnityEngine.Random.Range(-115f, 115f);
			}
			base.Fire(new Offset("right hand shoot point"), new Direction(angle, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
			if (i % 3 == 0)
			{
				string text = ((!offhand) ? "left" : "right") + " hand shoot point";
				angle = base.GetAimDirection(text);
				base.Fire(new Offset(text), new Direction(angle, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
				offhand = !offhand;
			}
			yield return base.Wait(4);
			i++;
		}
		yield break;
	}

	// Token: 0x0400028F RID: 655
	private const float NumBullets = 70f;

	// Token: 0x04000290 RID: 656
	private float NarrowAngle = 60f;

	// Token: 0x04000291 RID: 657
	private float NarrowAngleChance = 0.5f;
}
