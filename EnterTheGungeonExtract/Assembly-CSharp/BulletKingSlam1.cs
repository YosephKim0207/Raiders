using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020000FE RID: 254
[InspectorDropdownName("Bosses/BulletKing/Slam1")]
public class BulletKingSlam1 : Script
{
	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060003CA RID: 970 RVA: 0x00012B84 File Offset: 0x00010D84
	protected bool IsHard
	{
		get
		{
			return this is BulletKingSlamHard1;
		}
	}

	// Token: 0x060003CB RID: 971 RVA: 0x00012B90 File Offset: 0x00010D90
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		float startAngle = base.RandomAngle();
		float delta = 10f;
		for (int i = 0; i < 36; i++)
		{
			float num = startAngle + (float)i * delta;
			base.Fire(new Offset(1f, 0f, num, string.Empty, DirectionType.Absolute), new Direction(num, DirectionType.Absolute, -1f), new Speed((float)((!this.IsHard) ? 5 : 8), SpeedType.Absolute), new BulletKingSlam1.SpinningBullet(base.Position, num, this.IsHard));
		}
		if (this.IsHard)
		{
			for (int j = 0; j < 12; j++)
			{
				float num2 = base.RandomAngle();
				base.Fire(new Offset(1f, 0f, num2, string.Empty, DirectionType.Absolute), new Direction(num2, DirectionType.Absolute, -1f), new Speed(UnityEngine.Random.Range(3f, 5f), SpeedType.Absolute), new BulletKingSlam1.SpinningBullet(base.Position, num2, this.IsHard));
			}
		}
		yield return base.Wait(90);
		yield break;
	}

	// Token: 0x040003BB RID: 955
	private const int NumBullets = 36;

	// Token: 0x040003BC RID: 956
	private const int NumHardBullets = 12;

	// Token: 0x040003BD RID: 957
	private const float RadiusAcceleration = 8f;

	// Token: 0x040003BE RID: 958
	private const float SpinAccelration = 10f;

	// Token: 0x040003BF RID: 959
	public static float SpinningBulletSpinSpeed = 180f;

	// Token: 0x040003C0 RID: 960
	private const int Time = 180;

	// Token: 0x020000FF RID: 255
	public class SpinningBullet : Bullet
	{
		// Token: 0x060003CD RID: 973 RVA: 0x00012BB8 File Offset: 0x00010DB8
		public SpinningBullet(Vector2 centerPoint, float startAngle, bool isHard)
		{
			string text = "slam";
			base..ctor(text, false, false, isHard);
			this.centerPoint = centerPoint;
			this.startAngle = startAngle;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00012BE8 File Offset: 0x00010DE8
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			float radius = Vector2.Distance(this.centerPoint, base.Position);
			float speed = this.Speed;
			float spinAngle = this.startAngle;
			float spinSpeed = 0f;
			for (int i = 0; i < 180; i++)
			{
				speed += 0.13333334f;
				radius += speed / 60f;
				spinSpeed += 0.16666667f;
				spinAngle += spinSpeed / 60f;
				base.Position = this.centerPoint + BraveMathCollege.DegreesToVector(spinAngle, radius);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040003C1 RID: 961
		private Vector2 centerPoint;

		// Token: 0x040003C2 RID: 962
		private float startAngle;
	}
}
