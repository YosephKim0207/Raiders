using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000177 RID: 375
[InspectorDropdownName("Bosses/DraGun/KnifeDaggers1")]
public class DraGunKnifeDaggers1 : Script
{
	// Token: 0x0600059D RID: 1437 RVA: 0x0001ACD0 File Offset: 0x00018ED0
	protected override IEnumerator Top()
	{
		float[] angles = new float[7];
		CellArea area = base.BulletBank.aiActor.ParentRoom.area;
		Vector2 roomLowerLeft = area.UnitBottomLeft + new Vector2(0f, 19f);
		Vector2 roomUpperRight = roomLowerLeft + new Vector2(36f, 14f);
		DraGunKnifeController knifeController = base.BulletBank.GetComponent<DraGunKnifeController>();
		for (int i = 0; i < 1; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				float num = base.AimDirection;
				float num2 = 0.7f;
				if (j != 6)
				{
					int num3 = j / 2;
					bool flag = j % 2 == 1;
					Vector2 vector = IntVector2.CardinalsAndOrdinals[num3].ToVector2();
					float num4 = ((!flag) ? 7f : 8.15f);
					Vector2 vector2 = this.BulletManager.PlayerPosition();
					Vector2 vector3 = vector.normalized * num4;
					vector2 += vector3 * num2;
					Vector2 predictedPosition = BraveMathCollege.GetPredictedPosition(vector2, this.BulletManager.PlayerVelocity(), base.Position, 60f);
					num = (predictedPosition - base.Position).ToAngle();
				}
				for (int k = 0; k < j; k++)
				{
					if (!float.IsNaN(angles[k]) && BraveMathCollege.AbsAngleBetween(angles[k], num) < 3f)
					{
						num = float.NaN;
					}
				}
				angles[j] = num;
				if (!float.IsNaN(angles[j]))
				{
					float num5 = 20f;
					Vector2 zero = Vector2.zero;
					if (BraveMathCollege.LineSegmentRectangleIntersection(base.Position, base.Position + BraveMathCollege.DegreesToVector(num, 60f), roomLowerLeft, roomUpperRight, ref zero))
					{
						num5 = (zero - base.Position).magnitude;
					}
					GameObject gameObject = SpawnManager.SpawnVFX(knifeController.ReticleQuad, false);
					LineReticleController component = gameObject.GetComponent<LineReticleController>();
					component.Init(new Vector3(base.Position.x, base.Position.y, base.Position.y) + BraveMathCollege.DegreesToVector(num, 2f), Quaternion.Euler(0f, 0f, num), num5 - 3f);
					this.m_reticles.Add(component);
				}
			}
			yield return base.Wait(37);
			this.CleanupReticles();
			yield return base.Wait(5);
			for (int l = 0; l < 7; l++)
			{
				if (!float.IsNaN(angles[l]))
				{
					base.Fire(new Offset(new Vector2(0.5f, 0f), angles[l], string.Empty, DirectionType.Absolute), new Direction(angles[l], DirectionType.Absolute, -1f), new Speed(60f, SpeedType.Absolute), new Bullet("dagger", true, false, false));
				}
			}
		}
		yield break;
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x0001ACEC File Offset: 0x00018EEC
	public override void OnForceEnded()
	{
		this.CleanupReticles();
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0001ACF4 File Offset: 0x00018EF4
	public Vector2 GetPredictedTargetPosition(float leadAmount, float speed, float fireDelay)
	{
		Vector2 vector = this.BulletManager.PlayerPosition();
		Vector2 vector2 = this.BulletManager.PlayerVelocity();
		vector += vector2 * fireDelay;
		return BraveMathCollege.GetPredictedPosition(vector, this.BulletManager.PlayerVelocity(), base.Position, speed);
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0001AD40 File Offset: 0x00018F40
	private void CleanupReticles()
	{
		for (int i = 0; i < this.m_reticles.Count; i++)
		{
			this.m_reticles[i].Cleanup();
		}
		this.m_reticles.Clear();
	}

	// Token: 0x04000564 RID: 1380
	private const int NumWaves = 1;

	// Token: 0x04000565 RID: 1381
	private const int NumDaggersPerWave = 7;

	// Token: 0x04000566 RID: 1382
	private const int AttackDelay = 42;

	// Token: 0x04000567 RID: 1383
	private const float DaggerSpeed = 60f;

	// Token: 0x04000568 RID: 1384
	private List<LineReticleController> m_reticles = new List<LineReticleController>();
}
