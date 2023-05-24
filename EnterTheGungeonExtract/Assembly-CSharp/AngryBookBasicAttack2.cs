using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000008 RID: 8
[InspectorDropdownName("AngryBook/BasicAttack2")]
public class AngryBookBasicAttack2 : Script
{
	// Token: 0x0600001C RID: 28 RVA: 0x000025E0 File Offset: 0x000007E0
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		int count = 0;
		float xOffset = 1.9f / (float)(this.LineBullets - 1);
		float yOffset = 2.5f / (float)(this.LineBullets - 1);
		for (int i = 0; i < this.LineBullets; i++)
		{
			Offset offset = new Offset(-0.95f, -1.25f + yOffset * (float)i, 0f, string.Empty, DirectionType.Absolute);
			int num;
			count = (num = count) + 1;
			base.Fire(offset, new AngryBookBasicAttack2.DefaultBullet(num));
			yield return base.Wait(1);
		}
		for (int j = 0; j < this.LineBullets; j++)
		{
			Offset offset2 = new Offset(-0.95f + xOffset * (float)j, 1.25f - yOffset * (float)j, 0f, string.Empty, DirectionType.Absolute);
			int num;
			count = (num = count) + 1;
			base.Fire(offset2, new AngryBookBasicAttack2.DefaultBullet(num));
			yield return base.Wait(1);
		}
		for (int k = 0; k < this.LineBullets; k++)
		{
			Offset offset3 = new Offset(0.95f, -1.25f + yOffset * (float)k, 0f, string.Empty, DirectionType.Absolute);
			int num;
			count = (num = count) + 1;
			base.Fire(offset3, new AngryBookBasicAttack2.DefaultBullet(num));
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x0400001B RID: 27
	public int LineBullets = 10;

	// Token: 0x0400001C RID: 28
	public const float Height = 2.5f;

	// Token: 0x0400001D RID: 29
	public const float Width = 1.9f;

	// Token: 0x02000009 RID: 9
	public class DefaultBullet : Bullet
	{
		// Token: 0x0600001D RID: 29 RVA: 0x000025FC File Offset: 0x000007FC
		public DefaultBullet(int spawnTime)
			: base(null, false, false, false)
		{
			this.spawnTime = spawnTime;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002610 File Offset: 0x00000810
		protected override IEnumerator Top()
		{
			yield return base.Wait(45 - this.spawnTime);
			base.ChangeDirection(new Direction(0f, DirectionType.Aim, -1f), 1);
			base.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 1);
			yield break;
		}

		// Token: 0x0400001E RID: 30
		public int spawnTime;
	}
}
