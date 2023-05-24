using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000202 RID: 514
[InspectorDropdownName("Bosses/Infinilich/MorphGun1")]
public class InfinilichMorphGun1 : Script
{
	// Token: 0x060007A9 RID: 1961 RVA: 0x00025170 File Offset: 0x00023370
	protected override IEnumerator Top()
	{
		float num = BraveMathCollege.ClampAngle180(base.BulletBank.aiAnimator.FacingDirection);
		this.m_sign = (float)((num > 90f || num < -90f) ? (-1) : 1);
		Vector2 vector = base.Position + new Vector2(this.m_sign * 2.5f, 1f);
		float num2 = (this.BulletManager.PlayerPosition() - vector).ToAngle();
		int[][] array = ((this.m_sign >= 0f) ? InfinilichMorphGun1.RightBulletOrder : InfinilichMorphGun1.LeftBulletOrder);
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < array[i].Length; j++)
			{
				string text = "morph bullet " + array[i][j];
				base.Fire(new Offset(text), new Direction(num2, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new InfinilichMorphGun1.GunBullet(i));
			}
		}
		return null;
	}

	// Token: 0x04000792 RID: 1938
	private static int[][] LeftBulletOrder = new int[][]
	{
		new int[] { 1, 8 },
		new int[] { 2, 9 },
		new int[] { 3, 10 },
		new int[] { 4, 11, 15 },
		new int[] { 5, 12, 16, 19 },
		new int[] { 6, 13, 17, 20, 22 },
		new int[] { 7, 14, 18, 21, 23 }
	};

	// Token: 0x04000793 RID: 1939
	private static int[][] RightBulletOrder = new int[][]
	{
		new int[] { 2, 9 },
		new int[] { 1, 8 },
		new int[] { 4, 11 },
		new int[] { 3, 10, 15 },
		new int[] { 7, 14, 18, 21 },
		new int[] { 6, 13, 17, 20, 23 },
		new int[] { 5, 12, 16, 19, 22 }
	};

	// Token: 0x04000794 RID: 1940
	private float m_sign;

	// Token: 0x02000203 RID: 515
	public class GunBullet : Bullet
	{
		// Token: 0x060007AB RID: 1963 RVA: 0x000253B8 File Offset: 0x000235B8
		public GunBullet(int delay)
			: base(null, false, false, false)
		{
			this.m_delay = delay;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x000253CC File Offset: 0x000235CC
		protected override IEnumerator Top()
		{
			yield return base.Wait(this.m_delay * 8);
			this.Speed = 12f;
			this.Direction = base.GetAimDirection((float)((!BraveUtility.RandomBool()) ? 0 : 1), this.Speed);
			AkSoundEngine.PostEvent("Play_WPN_minigun_shot_01", base.BulletBank.gameObject);
			yield break;
		}

		// Token: 0x04000795 RID: 1941
		private int m_delay;
	}
}
