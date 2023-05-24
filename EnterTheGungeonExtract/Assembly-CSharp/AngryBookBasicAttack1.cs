using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000004 RID: 4
[InspectorDropdownName("AngryBook/BasicAttack1")]
public class AngryBookBasicAttack1 : Script
{
	// Token: 0x0600000C RID: 12 RVA: 0x000021D8 File Offset: 0x000003D8
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		int count = 0;
		float degDelta = 360f / (float)this.CircleBullets;
		for (int i = 0; i < this.CircleBullets; i++)
		{
			Offset offset = new Offset(0f, 1.3f, (float)i * degDelta, string.Empty, DirectionType.Absolute);
			Direction direction = new Direction(90f + (float)i * degDelta, DirectionType.Absolute, -1f);
			int num;
			count = (num = count) + 1;
			base.Fire(offset, direction, new AngryBookBasicAttack1.DefaultBullet(num));
			yield return base.Wait(1);
		}
		for (int j = 0; j < this.LineBullets / 2; j++)
		{
			Offset offset2 = new Offset(0f, 1.6f - 3.2f / (float)(this.LineBullets - 1) * (float)j, 0f, string.Empty, DirectionType.Absolute);
			Direction direction2 = new Direction(90f, DirectionType.Absolute, -1f);
			int num;
			count = (num = count) + 1;
			base.Fire(offset2, direction2, new AngryBookBasicAttack1.DefaultBullet(num));
			yield return base.Wait(1);
		}
		for (int k = 0; k < this.LineBullets / 2; k++)
		{
			Offset offset3 = new Offset(0f, 1.6f - 3.2f / (float)(this.LineBullets - 1) * (float)(k + this.LineBullets / 2), 0f, string.Empty, DirectionType.Absolute);
			Direction direction3 = new Direction(-90f, DirectionType.Absolute, -1f);
			int num;
			count = (num = count) + 1;
			base.Fire(offset3, direction3, new AngryBookBasicAttack1.DefaultBullet(num));
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x04000009 RID: 9
	public int CircleBullets = 20;

	// Token: 0x0400000A RID: 10
	public int LineBullets = 12;

	// Token: 0x0400000B RID: 11
	public const float CircleRadius = 1.3f;

	// Token: 0x0400000C RID: 12
	public const float LineHalfDist = 1.6f;

	// Token: 0x02000005 RID: 5
	public class DefaultBullet : Bullet
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000021F4 File Offset: 0x000003F4
		public DefaultBullet(int spawnTime)
			: base(null, false, false, false)
		{
			this.spawnTime = spawnTime;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002208 File Offset: 0x00000408
		protected override IEnumerator Top()
		{
			yield return base.Wait(45 - this.spawnTime);
			base.ChangeSpeed(new Speed(8f, SpeedType.Absolute), 1);
			yield break;
		}

		// Token: 0x0400000D RID: 13
		public int spawnTime;
	}
}
