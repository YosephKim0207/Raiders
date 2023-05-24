using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000215 RID: 533
[InspectorDropdownName("Kaliber/OneHead1")]
public class KaliberOneHead1 : Script
{
	// Token: 0x06000800 RID: 2048 RVA: 0x00026F24 File Offset: 0x00025124
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 60; i++)
		{
			AkSoundEngine.PostEvent("Play_WPN_earthwormgun_shot_01", base.BulletBank.gameObject);
			Vector2 offset;
			float angle;
			this.GetOffset(out offset, out angle, true);
			base.Fire(new Offset(offset, 0f, string.Empty, DirectionType.Absolute), new Direction(angle + (float)UnityEngine.Random.Range(-20, 20), DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), null);
			this.GetOffset(out offset, out angle, false);
			base.Fire(new Offset(offset, 0f, string.Empty, DirectionType.Absolute), new Direction(angle + (float)UnityEngine.Random.Range(-20, 20), DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), null);
			yield return base.Wait(3);
		}
		yield break;
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x00026F40 File Offset: 0x00025140
	private void GetOffset(out Vector2 offset, out float angle, bool left)
	{
		int num = base.Tick % 40;
		this.GetFrameOffset(num / 5, out offset, left);
		if (left)
		{
			if (num <= 25)
			{
				angle = Mathf.Lerp(70f, 290f, (float)num / 25f);
			}
			else
			{
				angle = Mathf.Lerp(290f, 70f, (float)(num - 25) / 15f);
			}
		}
		else if (num <= 25)
		{
			angle = Mathf.Lerp(-110f, 110f, (float)num / 25f);
		}
		else
		{
			angle = Mathf.Lerp(110f, -110f, (float)(num - 25) / 15f);
		}
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x00026FF4 File Offset: 0x000251F4
	private void GetFrameOffset(int frame, out Vector2 offset, bool left)
	{
		IntVector2 intVector = IntVector2.Zero;
		if (left)
		{
			switch (frame)
			{
			case 0:
				intVector = new IntVector2(13, 30);
				break;
			case 1:
				intVector = new IntVector2(12, 28);
				break;
			case 2:
				intVector = new IntVector2(13, 20);
				break;
			case 3:
				intVector = new IntVector2(12, 12);
				break;
			case 4:
				intVector = new IntVector2(11, 4);
				break;
			case 5:
				intVector = new IntVector2(11, 3);
				break;
			case 6:
				intVector = new IntVector2(9, 4);
				break;
			case 7:
				intVector = new IntVector2(11, 12);
				break;
			}
		}
		else
		{
			switch (frame)
			{
			case 0:
				intVector = new IntVector2(59, 3);
				break;
			case 1:
				intVector = new IntVector2(61, 4);
				break;
			case 2:
				intVector = new IntVector2(62, 11);
				break;
			case 3:
				intVector = new IntVector2(61, 20);
				break;
			case 4:
				intVector = new IntVector2(60, 28);
				break;
			case 5:
				intVector = new IntVector2(58, 31);
				break;
			case 6:
				intVector = new IntVector2(60, 28);
				break;
			case 7:
				intVector = new IntVector2(61, 21);
				break;
			}
		}
		intVector -= new IntVector2(36, 13);
		offset = PhysicsEngine.PixelToUnit(intVector);
	}

	// Token: 0x0400080F RID: 2063
	private const int NumBullets = 60;
}
