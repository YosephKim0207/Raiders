using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000147 RID: 327
[InspectorDropdownName("Bosses/DemonWall/BasicLines1")]
public class DemonWallBasicLines1 : Script
{
	// Token: 0x060004DC RID: 1244 RVA: 0x000179AC File Offset: 0x00015BAC
	protected override IEnumerator Top()
	{
		int group = 1;
		for (int i = 0; i < 10; i++)
		{
			group = BraveUtility.SequentialRandomRange(0, DemonWallBasicLines1.shootPoints.Length, group, null, true);
			int wallLine = 0;
			if (i % 6 == 1)
			{
				wallLine = -1;
			}
			if (i % 6 == 5)
			{
				wallLine = 1;
			}
			this.FireLine(BraveUtility.RandomElement<string>(DemonWallBasicLines1.shootPoints[group]), wallLine);
			int otherGroup = ((!BraveUtility.RandomBool()) ? 2 : 0);
			if (group != 1)
			{
				otherGroup = ((group != 0) ? 0 : 2);
			}
			float angle = (float)(-90 + ((otherGroup != 0) ? 45 : (-45)));
			this.FireCrossBullets(BraveUtility.RandomElement<string>(DemonWallBasicLines1.shootPoints[group]), angle);
			yield return base.Wait(20);
		}
		yield break;
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x000179C8 File Offset: 0x00015BC8
	private void FireLine(string transform, int wallLine)
	{
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				base.Fire(new Offset(transform), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(9f - (float)i * 1.5f, SpeedType.Absolute), new DemonWallBasicLines1.LineBullet(i == 0, (float)(j - 1)));
			}
		}
		if (wallLine != 0)
		{
			Vector2 vector = ((wallLine >= 0) ? new Vector2(23.75f, 3f) : new Vector2(0.5f, 3f));
			for (int k = 0; k < 5; k++)
			{
				base.Fire(new Offset(vector, 0f, string.Empty, DirectionType.Absolute), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(9f - (float)k * 1.5f, SpeedType.Absolute), new DemonWallBasicLines1.LineBullet(true, 0f));
			}
		}
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x00017AC0 File Offset: 0x00015CC0
	private void FireCrossBullets(string transform, float angle)
	{
		for (int i = 0; i < 2; i++)
		{
			Offset offset = new Offset(transform);
			Direction direction = new Direction(angle + (float)UnityEngine.Random.Range(-30, 30), DirectionType.Absolute, -1f);
			Speed speed = new Speed((float)(2 + 5 * i), SpeedType.Absolute);
			string text = "wave";
			float num = 7f;
			int num2 = 90;
			bool flag = i != 3;
			base.Fire(offset, direction, speed, new SpeedChangingBullet(text, num, num2, -1, flag));
		}
	}

	// Token: 0x040004B6 RID: 1206
	public static string[][] shootPoints = new string[][]
	{
		new string[] { "sad bullet", "blobulon", "dopey bullet" },
		new string[] { "left eye", "right eye", "crashed bullet" },
		new string[] { "sideways bullet", "shotgun bullet", "cultist", "angry bullet" }
	};

	// Token: 0x040004B7 RID: 1207
	public const int NumBursts = 10;

	// Token: 0x02000148 RID: 328
	public class LineBullet : Bullet
	{
		// Token: 0x060004E0 RID: 1248 RVA: 0x00017BB8 File Offset: 0x00015DB8
		public LineBullet(bool doVfx, float horizontalSign)
			: base("line", false, false, false)
		{
			base.SuppressVfx = !doVfx;
			this.m_horizontalSign = horizontalSign;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00017BDC File Offset: 0x00015DDC
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			base.ChangeSpeed(new Speed(9f, SpeedType.Absolute), 90);
			for (int i = 0; i < 600; i++)
			{
				base.UpdateVelocity();
				base.UpdatePosition();
				if (i < 90)
				{
					base.Position += new Vector2(this.m_horizontalSign * 0.011111111f, 0f);
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040004B8 RID: 1208
		private float m_horizontalSign;
	}
}
