using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x020002CD RID: 717
[InspectorDropdownName("Bosses/ResourcefulRat/Daggers1")]
public class ResourcefulRatDaggers1 : Script
{
	// Token: 0x06000B06 RID: 2822 RVA: 0x00035228 File Offset: 0x00033428
	protected override IEnumerator Top()
	{
		yield return base.Wait(18);
		float[] angles = new float[17];
		CellArea area = base.BulletBank.aiActor.ParentRoom.area;
		int totalAttackTicks = 56;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 17; j++)
			{
				float angle = base.AimDirection;
				float timeUntilFire = (float)(totalAttackTicks - j) / 60f;
				if (j != 16)
				{
					int num = j / 2;
					bool flag = j % 2 == 1;
					Vector2 vector = IntVector2.CardinalsAndOrdinals[num].ToVector2();
					float num2 = ((!flag) ? 7f : 8.15f);
					Vector2 vector2 = this.BulletManager.PlayerPosition();
					Vector2 vector3 = vector.normalized * num2;
					vector2 += vector3 * timeUntilFire;
					Vector2 predictedPosition = BraveMathCollege.GetPredictedPosition(vector2, this.BulletManager.PlayerVelocity(), base.Position, 60f);
					angle = (predictedPosition - base.Position).ToAngle();
				}
				for (int k = 0; k < j; k++)
				{
					if (!float.IsNaN(angles[k]) && BraveMathCollege.AbsAngleBetween(angles[k], angle) < 3f)
					{
						angle = float.NaN;
					}
				}
				angles[j] = angle;
				if (!float.IsNaN(angles[j]))
				{
					ResourcefulRatController component = base.BulletBank.GetComponent<ResourcefulRatController>();
					float num3 = 20f;
					Vector2 zero = Vector2.zero;
					if (BraveMathCollege.LineSegmentRectangleIntersection(base.Position, base.Position + BraveMathCollege.DegreesToVector(angle, 60f), area.UnitBottomLeft, area.UnitTopRight - new Vector2(0f, 6f), ref zero))
					{
						num3 = (zero - base.Position).magnitude;
					}
					GameObject gameObject = SpawnManager.SpawnVFX(component.ReticleQuad, false);
					tk2dSlicedSprite component2 = gameObject.GetComponent<tk2dSlicedSprite>();
					component2.transform.position = new Vector3(base.Position.x, base.Position.y, base.Position.y) + BraveMathCollege.DegreesToVector(angle, 2f);
					component2.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
					component2.dimensions = new Vector2((num3 - 3f) * 16f, 5f);
					component2.UpdateZDepth();
					this.m_reticles.Add(gameObject);
				}
				if (j < 16)
				{
					yield return base.Wait(1);
				}
			}
			yield return base.Wait(15);
			this.CleanupReticles();
			yield return base.Wait(25);
			for (int l = 0; l < 17; l++)
			{
				if (!float.IsNaN(angles[l]))
				{
					base.Fire(new Offset(new Vector2(0.5f, 0f), angles[l], string.Empty, DirectionType.Absolute), new Direction(angles[l], DirectionType.Absolute, -1f), new Speed(60f, SpeedType.Absolute), new Bullet("dagger", true, false, false));
				}
			}
			yield return base.Wait(12);
			for (int m = 0; m < 17; m++)
			{
				if (!float.IsNaN(angles[m]))
				{
					base.Fire(new Offset(new Vector2(0.5f, 0f), angles[m], string.Empty, DirectionType.Absolute), new Direction(angles[m], DirectionType.Absolute, -1f), new Speed(30f, SpeedType.Absolute), new Bullet("dagger", true, false, false));
				}
			}
			yield return base.Wait(24);
		}
		yield break;
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x00035244 File Offset: 0x00033444
	public override void OnForceEnded()
	{
		this.CleanupReticles();
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0003524C File Offset: 0x0003344C
	public Vector2 GetPredictedTargetPosition(float leadAmount, float speed, float fireDelay)
	{
		Vector2 vector = this.BulletManager.PlayerPosition();
		Vector2 vector2 = this.BulletManager.PlayerVelocity();
		vector += vector2 * fireDelay;
		return BraveMathCollege.GetPredictedPosition(vector, this.BulletManager.PlayerVelocity(), base.Position, speed);
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x00035298 File Offset: 0x00033498
	private void CleanupReticles()
	{
		for (int i = 0; i < this.m_reticles.Count; i++)
		{
			SpawnManager.Despawn(this.m_reticles[i]);
		}
		this.m_reticles.Clear();
	}

	// Token: 0x04000B9D RID: 2973
	private const int NumWaves = 3;

	// Token: 0x04000B9E RID: 2974
	private const int NumDaggersPerWave = 17;

	// Token: 0x04000B9F RID: 2975
	private const int AimDelay = 1;

	// Token: 0x04000BA0 RID: 2976
	private const int VanishDelay = 15;

	// Token: 0x04000BA1 RID: 2977
	private const int FireDelay = 25;

	// Token: 0x04000BA2 RID: 2978
	private const float DaggerSpeed = 60f;

	// Token: 0x04000BA3 RID: 2979
	private List<GameObject> m_reticles = new List<GameObject>();
}
