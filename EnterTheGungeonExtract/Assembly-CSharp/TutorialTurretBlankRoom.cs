using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200031B RID: 795
[InspectorDropdownName("TutorialTurret/BlankRoom")]
public class TutorialTurretBlankRoom : Script
{
	// Token: 0x06000C4A RID: 3146 RVA: 0x0003B540 File Offset: 0x00039740
	protected override IEnumerator Top()
	{
		bool doWarp = Mathf.Abs(base.Position.y % 1f) > 0.5f;
		yield return base.Wait(20);
		while (base.BulletBank.aiActor.enabled)
		{
			base.Fire(new Direction(180f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new TutorialTurretBlankRoom.WarpBullet(doWarp));
			yield return base.Wait(15);
		}
		yield break;
	}

	// Token: 0x04000CFC RID: 3324
	public int CircleBullets = 20;

	// Token: 0x04000CFD RID: 3325
	public int LineBullets = 12;

	// Token: 0x0200031C RID: 796
	public class WarpBullet : Bullet
	{
		// Token: 0x06000C4B RID: 3147 RVA: 0x0003B55C File Offset: 0x0003975C
		public WarpBullet(bool doWarp)
			: base(null, false, false, false)
		{
			this.m_doWarp = doWarp;
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x0003B570 File Offset: 0x00039770
		protected override IEnumerator Top()
		{
			if (this.m_doWarp)
			{
				base.Position += new Vector2(-0.75f, 0f);
			}
			base.Position = base.Position.WithY(BraveMathCollege.QuantizeFloat(base.Position.y, 0.0625f));
			return null;
		}

		// Token: 0x04000CFE RID: 3326
		private bool m_doWarp;
	}
}
