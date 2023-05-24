using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class CultistKaliberTentacle1 : Script
{
	// Token: 0x060004D1 RID: 1233 RVA: 0x000175A0 File Offset: 0x000157A0
	protected override IEnumerator Top()
	{
		this.m_targetPositions = new Vector2?[4];
		float aimDirection = base.AimDirection;
		for (int i = 0; i < 4; i++)
		{
			float num = base.SubdivideArc(aimDirection - 65f, 130f, 4, i, false) + UnityEngine.Random.Range(-6f, 6f);
			for (int j = 0; j < 8; j++)
			{
				base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new CultistKaliberTentacle1.TentacleBullet(j * 6, this, i, (float)(((float)i >= 2f) ? (-1) : 1)));
			}
		}
		return null;
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00017648 File Offset: 0x00015848
	public Vector2 GetTargetPosition(int index, Bullet bullet)
	{
		Vector2? vector = this.m_targetPositions[index];
		if (vector == null)
		{
			this.m_targetPositions[index] = new Vector2?(bullet.GetPredictedTargetPosition((float)(((double)UnityEngine.Random.value >= 0.5) ? 1 : 0), 10f));
		}
		return this.m_targetPositions[index].Value;
	}

	// Token: 0x040004A4 RID: 1188
	private const int NumTentacles = 4;

	// Token: 0x040004A5 RID: 1189
	private const int NumBullets = 8;

	// Token: 0x040004A6 RID: 1190
	private const int BulletSpeed = 10;

	// Token: 0x040004A7 RID: 1191
	private const float TentacleMagnitude = 0.75f;

	// Token: 0x040004A8 RID: 1192
	private const float TentaclePeriod = 3f;

	// Token: 0x040004A9 RID: 1193
	private Vector2?[] m_targetPositions;

	// Token: 0x02000145 RID: 325
	public class TentacleBullet : Bullet
	{
		// Token: 0x060004D3 RID: 1235 RVA: 0x000176C4 File Offset: 0x000158C4
		public TentacleBullet(int delay, CultistKaliberTentacle1 parentScript, int index, float offsetScalar)
			: base(null, false, false, false)
		{
			this.m_delay = delay;
			this.m_parentScript = parentScript;
			this.m_index = index;
			this.m_offsetScalar = offsetScalar;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000176F0 File Offset: 0x000158F0
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			yield return base.Wait(this.m_delay);
			Vector2 truePosition = base.Position;
			for (int i = 0; i < 360; i++)
			{
				float offsetMagnitude = this.m_offsetScalar * Mathf.SmoothStep(-0.75f, 0.75f, Mathf.PingPong(0.5f + (float)i / 60f * 3f, 1f));
				offsetMagnitude *= Mathf.Lerp(1f, 0.25f, (float)(i - 20) / 40f);
				if (i >= 20 && i <= 60)
				{
					if (i == 20)
					{
						this.m_target = this.m_parentScript.GetTargetPosition(this.m_index, this);
					}
					float num = (this.m_target - truePosition).ToAngle();
					float num2 = BraveMathCollege.ClampAngle180(num - this.Direction);
					this.Direction += Mathf.Clamp(num2, -6f, 6f);
				}
				truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
				base.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, offsetMagnitude);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040004AA RID: 1194
		private int m_delay;

		// Token: 0x040004AB RID: 1195
		private CultistKaliberTentacle1 m_parentScript;

		// Token: 0x040004AC RID: 1196
		private int m_index;

		// Token: 0x040004AD RID: 1197
		private float m_offsetScalar;

		// Token: 0x040004AE RID: 1198
		private Vector2 m_target;
	}
}
