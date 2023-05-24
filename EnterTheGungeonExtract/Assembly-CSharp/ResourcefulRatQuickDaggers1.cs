using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x020002D3 RID: 723
[InspectorDropdownName("Bosses/ResourcefulRat/QuickDaggers1")]
public class ResourcefulRatQuickDaggers1 : Script
{
	// Token: 0x06000B22 RID: 2850 RVA: 0x00035F10 File Offset: 0x00034110
	protected override IEnumerator Top()
	{
		float[] angles = new float[4];
		CellArea area = base.BulletBank.aiActor.ParentRoom.area;
		for (int i = 0; i < 1; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				float num = base.AimDirection;
				float num2 = 0.43333334f;
				if (j != 3)
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
					ResourcefulRatController component = base.BulletBank.GetComponent<ResourcefulRatController>();
					float num5 = 20f;
					Vector2 zero = Vector2.zero;
					if (BraveMathCollege.LineSegmentRectangleIntersection(base.Position, base.Position + BraveMathCollege.DegreesToVector(num, 60f), area.UnitBottomLeft, area.UnitTopRight - new Vector2(0f, 6f), ref zero))
					{
						num5 = (zero - base.Position).magnitude;
					}
					GameObject gameObject = SpawnManager.SpawnVFX(component.ReticleQuad, false);
					tk2dSlicedSprite component2 = gameObject.GetComponent<tk2dSlicedSprite>();
					component2.transform.position = new Vector3(base.Position.x, base.Position.y, base.Position.y) + BraveMathCollege.DegreesToVector(num, 2f);
					component2.transform.localRotation = Quaternion.Euler(0f, 0f, num);
					component2.dimensions = new Vector2((num5 - 3f) * 16f, 5f);
					component2.UpdateZDepth();
					this.m_reticles.Add(gameObject);
				}
			}
			yield return base.Wait(26);
			this.CleanupReticles();
			for (int l = 0; l < 4; l++)
			{
				if (!float.IsNaN(angles[l]))
				{
					base.Fire(new Offset(new Vector2(0.5f, 0f), angles[l], string.Empty, DirectionType.Absolute), new Direction(angles[l], DirectionType.Absolute, -1f), new Speed(60f, SpeedType.Absolute), new Bullet("dagger", true, false, false));
				}
			}
		}
		yield break;
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x00035F2C File Offset: 0x0003412C
	public override void OnForceEnded()
	{
		this.CleanupReticles();
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x00035F34 File Offset: 0x00034134
	public Vector2 GetPredictedTargetPosition(float leadAmount, float speed, float fireDelay)
	{
		Vector2 vector = this.BulletManager.PlayerPosition();
		Vector2 vector2 = this.BulletManager.PlayerVelocity();
		vector += vector2 * fireDelay;
		return BraveMathCollege.GetPredictedPosition(vector, this.BulletManager.PlayerVelocity(), base.Position, speed);
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x00035F80 File Offset: 0x00034180
	private void CleanupReticles()
	{
		for (int i = 0; i < this.m_reticles.Count; i++)
		{
			SpawnManager.Despawn(this.m_reticles[i]);
		}
		this.m_reticles.Clear();
	}

	// Token: 0x04000BCA RID: 3018
	private const int NumWaves = 1;

	// Token: 0x04000BCB RID: 3019
	private const int NumDaggersPerWave = 4;

	// Token: 0x04000BCC RID: 3020
	private const int AttackDelay = 26;

	// Token: 0x04000BCD RID: 3021
	private const float DaggerSpeed = 60f;

	// Token: 0x04000BCE RID: 3022
	private List<GameObject> m_reticles = new List<GameObject>();
}
