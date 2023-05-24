using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020000BF RID: 191
[InspectorDropdownName("Bosses/BossStatues/KaliSlam1")]
public class BossStatuesKaliSlam1 : Script
{
	// Token: 0x060002EA RID: 746 RVA: 0x0000F5E8 File Offset: 0x0000D7E8
	protected override IEnumerator Top()
	{
		Vector2 fixedPosition = base.Position;
		for (int i = 0; i < 36; i++)
		{
			base.Fire(Offset.OverridePosition(fixedPosition), new Direction(-60f + (float)(i * 10), DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new BossStatuesKaliSlam1.SpiralBullet1());
		}
		yield return base.Wait(5);
		for (int j = 0; j < 36; j++)
		{
			base.Fire(Offset.OverridePosition(fixedPosition), new Direction(-66f + (float)(j * 10), DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new BossStatuesKaliSlam1.SpiralBullet2());
		}
		for (int k = 0; k < 8; k++)
		{
			base.Fire(Offset.OverridePosition(fixedPosition), new Direction((float)(k * 45), DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new Bullet("egg", false, false, false));
		}
		yield return base.Wait(5);
		for (int l = 0; l < 36; l++)
		{
			base.Fire(Offset.OverridePosition(fixedPosition), new Direction(-72f + (float)(l * 10), DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new BossStatuesKaliSlam1.SpiralBullet3());
		}
		yield break;
	}

	// Token: 0x020000C0 RID: 192
	public class SpiralBullet1 : Bullet
	{
		// Token: 0x060002EB RID: 747 RVA: 0x0000F604 File Offset: 0x0000D804
		public SpiralBullet1()
			: base("spiralbullet1", false, false, false)
		{
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000F614 File Offset: 0x0000D814
		protected override IEnumerator Top()
		{
			base.ChangeDirection(new Direction(0.8f, DirectionType.Sequence, -1f), 1);
			base.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 180);
			yield return base.Wait(600);
			base.Vanish(false);
			yield break;
		}
	}

	// Token: 0x020000C2 RID: 194
	public class SpiralBullet2 : Bullet
	{
		// Token: 0x060002F3 RID: 755 RVA: 0x0000F70C File Offset: 0x0000D90C
		public SpiralBullet2()
			: base("spiralbullet2", false, false, false)
		{
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000F71C File Offset: 0x0000D91C
		protected override IEnumerator Top()
		{
			base.ChangeDirection(new Direction(-0.8f, DirectionType.Sequence, -1f), 1);
			base.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 180);
			yield return base.Wait(600);
			base.Vanish(false);
			yield break;
		}
	}

	// Token: 0x020000C4 RID: 196
	public class SpiralBullet3 : Bullet
	{
		// Token: 0x060002FB RID: 763 RVA: 0x0000F814 File Offset: 0x0000DA14
		public SpiralBullet3()
			: base("spiralbullet3", false, false, false)
		{
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000F824 File Offset: 0x0000DA24
		protected override IEnumerator Top()
		{
			base.ChangeDirection(new Direction(0.8f, DirectionType.Sequence, -1f), 1);
			base.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 180);
			yield return base.Wait(600);
			base.Vanish(false);
			yield break;
		}
	}
}
