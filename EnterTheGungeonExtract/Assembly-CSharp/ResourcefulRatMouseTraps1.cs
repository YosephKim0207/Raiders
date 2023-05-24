using System;
using System.Collections;
using Brave.BulletScript;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x020002CF RID: 719
[InspectorDropdownName("Bosses/ResourcefulRat/MouseTraps1")]
public class ResourcefulRatMouseTraps1 : Script
{
	// Token: 0x06000B11 RID: 2833 RVA: 0x00035908 File Offset: 0x00033B08
	protected override IEnumerator Top()
	{
		yield return base.Wait(56);
		if (ResourcefulRatMouseTraps1.s_xValues == null || ResourcefulRatMouseTraps1.s_yValues == null)
		{
			ResourcefulRatMouseTraps1.s_xValues = new int[10];
			ResourcefulRatMouseTraps1.s_yValues = new int[10];
			for (int j = 0; j < 10; j++)
			{
				ResourcefulRatMouseTraps1.s_xValues[j] = j;
				ResourcefulRatMouseTraps1.s_yValues[j] = j;
			}
		}
		CellArea area = base.BulletBank.aiActor.ParentRoom.area;
		Vector2 roomLowerLeft = area.UnitBottomLeft;
		Vector2 dimensions = area.dimensions.ToVector2() - new Vector2(0f, 6f);
		Vector2 delta = new Vector2(dimensions.x / 10f, dimensions.y / 10f);
		Vector2 safeZoneLowerLeft = area.UnitBottomLeft + new Vector2(15f, 9f);
		Vector2 safeZoneUpperRight = area.UnitBottomLeft + new Vector2(21f, 15f);
		BraveUtility.RandomizeArray<int>(ResourcefulRatMouseTraps1.s_xValues, 0, -1);
		BraveUtility.RandomizeArray<int>(ResourcefulRatMouseTraps1.s_yValues, 0, -1);
		for (int i = 0; i < 10; i++)
		{
			int baseX = ResourcefulRatMouseTraps1.s_xValues[i];
			int baseY = ResourcefulRatMouseTraps1.s_yValues[i];
			Vector2 goalPos = roomLowerLeft + new Vector2(((float)baseX + UnityEngine.Random.value) * delta.x, ((float)baseY + UnityEngine.Random.value) * delta.y);
			if (goalPos.IsWithin(safeZoneLowerLeft, safeZoneUpperRight))
			{
				if (BraveUtility.RandomBool())
				{
					baseX += (int)(BraveUtility.RandomSign() * 3f);
				}
				else
				{
					baseY += (int)(BraveUtility.RandomSign() * 3f);
				}
				goalPos = roomLowerLeft + new Vector2(((float)baseX + UnityEngine.Random.value) * delta.x, ((float)baseY + UnityEngine.Random.value) * delta.y);
			}
			this.Fire(goalPos);
			yield return base.Wait(2);
		}
		yield break;
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x00035924 File Offset: 0x00033B24
	private void Fire(Vector2 goal)
	{
		float num = (goal - base.Position).ToAngle();
		GameObject[] mouseTraps = base.BulletBank.GetComponent<ResourcefulRatController>().MouseTraps;
		base.Fire(new Direction(num, DirectionType.Absolute, -1f), new ResourcefulRatMouseTraps1.MouseTrapBullet(goal, BraveUtility.RandomElement<GameObject>(mouseTraps)));
	}

	// Token: 0x04000BAF RID: 2991
	private const int FlightTime = 60;

	// Token: 0x04000BB0 RID: 2992
	private const int NumTraps = 10;

	// Token: 0x04000BB1 RID: 2993
	private static int[] s_xValues;

	// Token: 0x04000BB2 RID: 2994
	private static int[] s_yValues;

	// Token: 0x020002D0 RID: 720
	private class MouseTrapBullet : Bullet
	{
		// Token: 0x06000B13 RID: 2835 RVA: 0x00035974 File Offset: 0x00033B74
		public MouseTrapBullet(Vector2 goalPos, GameObject mouseTrapPrefab)
			: base("mousetrap", true, false, false)
		{
			this.m_goalPos = goalPos;
			this.m_mouseTrapPrefab = mouseTrapPrefab;
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00035994 File Offset: 0x00033B94
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			this.Direction = (this.m_goalPos - base.Position).ToAngle();
			this.Speed = Vector2.Distance(this.m_goalPos, base.Position) / 1f;
			Vector2 truePosition = base.Position;
			for (int i = 0; i < 60; i++)
			{
				truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
				base.Position = truePosition + new Vector2(0f, Mathf.Sin((float)i / 60f * 3.1415927f) * 3.5f);
				yield return base.Wait(1);
			}
			GameObject go = UnityEngine.Object.Instantiate<GameObject>(this.m_mouseTrapPrefab, this.Projectile.specRigidbody.UnitCenter + new Vector2(-0.8f, -1.2f), Quaternion.identity);
			go.GetComponent<SpeculativeRigidbody>().Initialize();
			base.Vanish(true);
			yield break;
		}

		// Token: 0x04000BB3 RID: 2995
		private Vector2 m_goalPos;

		// Token: 0x04000BB4 RID: 2996
		private GameObject m_mouseTrapPrefab;
	}
}
