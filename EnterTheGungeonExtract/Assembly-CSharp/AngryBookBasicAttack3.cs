using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200000C RID: 12
[InspectorDropdownName("AngryBook/BasicAttack3")]
public class AngryBookBasicAttack3 : Script
{
	// Token: 0x0600002C RID: 44 RVA: 0x000029DC File Offset: 0x00000BDC
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		int count = 0;
		for (int i = 0; i < this.LineBullets; i++)
		{
			Offset offset = new Offset(-0.75f, base.SubdivideRange(-1f, 1f, this.LineBullets, i, false), 0f, string.Empty, DirectionType.Absolute);
			int num;
			count = (num = count) + 1;
			base.Fire(offset, new AngryBookBasicAttack3.DefaultBullet(num));
			yield return base.Wait(1);
		}
		for (int j = 0; j < this.LineBullets; j++)
		{
			Offset offset2 = new Offset(base.SubdivideRange(-0.75f, 0.25f, this.EdgeBullets, j, false), 1f, 0f, string.Empty, DirectionType.Absolute);
			int num;
			count = (num = count) + 1;
			base.Fire(offset2, new AngryBookBasicAttack3.DefaultBullet(num));
			yield return base.Wait(1);
		}
		for (int k = 0; k < this.CircleBullets; k++)
		{
			Offset offset3 = new Offset(new Vector2(0.25f, 0.5f) + new Vector2(0.5f, 0f).Rotate(base.SubdivideArc(90f, -180f, this.CircleBullets, k, false)), 0f, string.Empty, DirectionType.Absolute);
			int num;
			count = (num = count) + 1;
			base.Fire(offset3, new AngryBookBasicAttack3.DefaultBullet(num));
			yield return base.Wait(1);
		}
		for (int l = 0; l < this.LineBullets; l++)
		{
			Offset offset4 = new Offset(base.SubdivideRange(0.25f, -0.75f, this.EdgeBullets, l, false), 0f, 0f, string.Empty, DirectionType.Absolute);
			int num;
			count = (num = count) + 1;
			base.Fire(offset4, new AngryBookBasicAttack3.DefaultBullet(num));
			yield return base.Wait(1);
		}
		for (int m = 0; m < this.StemBullets; m++)
		{
			Offset offset5 = new Offset(base.SubdivideRange(0f, 0.75f, this.StemBullets, m, false), base.SubdivideRange(0f, -1f, this.StemBullets, m, false), 0f, string.Empty, DirectionType.Absolute);
			int num;
			count = (num = count) + 1;
			base.Fire(offset5, new AngryBookBasicAttack3.DefaultBullet(num));
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x0400002D RID: 45
	public int LineBullets = 6;

	// Token: 0x0400002E RID: 46
	public int EdgeBullets = 4;

	// Token: 0x0400002F RID: 47
	public int CircleBullets = 6;

	// Token: 0x04000030 RID: 48
	public int StemBullets = 6;

	// Token: 0x04000031 RID: 49
	public const float Height = 2f;

	// Token: 0x04000032 RID: 50
	public const float Width = 1.5f;

	// Token: 0x04000033 RID: 51
	public const float CircleRadius = 0.5f;

	// Token: 0x0200000D RID: 13
	public class DefaultBullet : Bullet
	{
		// Token: 0x0600002D RID: 45 RVA: 0x000029F8 File Offset: 0x00000BF8
		public DefaultBullet(int spawnTime)
			: base(null, false, false, false)
		{
			this.spawnTime = spawnTime;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002A0C File Offset: 0x00000C0C
		protected override IEnumerator Top()
		{
			yield return base.Wait(45 + this.spawnTime);
			base.ChangeDirection(new Direction(Mathf.Sin((float)this.spawnTime / 10f * 3.1415927f) * 10f, DirectionType.Aim, -1f), 1);
			base.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 1);
			yield break;
		}

		// Token: 0x04000034 RID: 52
		public int spawnTime;
	}
}
