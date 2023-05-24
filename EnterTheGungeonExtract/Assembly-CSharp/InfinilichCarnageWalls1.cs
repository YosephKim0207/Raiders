using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x020001F9 RID: 505
[InspectorDropdownName("Bosses/Infinilich/CarnageWalls1")]
public class InfinilichCarnageWalls1 : Script
{
	// Token: 0x06000785 RID: 1925 RVA: 0x000242F4 File Offset: 0x000224F4
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		base.Fire(new Offset("limb 1"), new Direction(45f, DirectionType.Absolute, -1f), new Speed(36f, SpeedType.Absolute), new InfinilichCarnageWalls1.TipBullet(this));
		base.Fire(new Offset("limb 2"), new Direction(-45f, DirectionType.Absolute, -1f), new Speed(36f, SpeedType.Absolute), new InfinilichCarnageWalls1.TipBullet(this));
		base.Fire(new Offset("limb 3"), new Direction(-135f, DirectionType.Absolute, -1f), new Speed(36f, SpeedType.Absolute), new InfinilichCarnageWalls1.TipBullet(this));
		base.Fire(new Offset("limb 4"), new Direction(135f, DirectionType.Absolute, -1f), new Speed(36f, SpeedType.Absolute), new InfinilichCarnageWalls1.TipBullet(this));
		yield return base.Wait(60);
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				Vector2 vector;
				Vector2 vector2;
				this.RandomWallPoints(out vector, out vector2);
				base.Fire(Offset.OverridePosition(vector), new Direction((vector2 - vector).ToAngle(), DirectionType.Absolute, -1f), new Speed(24f, SpeedType.Absolute), new InfinilichCarnageWalls1.TipBullet(this));
			}
			yield return base.Wait(180);
		}
		this.Done = true;
		yield return base.Wait(60);
		yield break;
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x00024310 File Offset: 0x00022510
	private void RandomWallPoints(out Vector2 startPoint, out Vector2 endPoint)
	{
		CellArea area = base.BulletBank.aiActor.ParentRoom.area;
		Vector2 vector = area.basePosition.ToVector2() + new Vector2(0.75f, 2f);
		Vector2 vector2 = (area.basePosition + area.dimensions).ToVector2() - new Vector2(0.75f, 0.5f);
		InfinilichCarnageWalls1.s_wallPoints.Clear();
		InfinilichCarnageWalls1.s_wallPoints.Add(new Vector2(UnityEngine.Random.Range(vector.x + 5f, vector2.x - 5f), vector2.y));
		InfinilichCarnageWalls1.s_wallPoints.Add(new Vector2(UnityEngine.Random.Range(vector.x + 5f, vector2.x - 5f), vector.y));
		InfinilichCarnageWalls1.s_wallPoints.Add(new Vector2(vector.x, UnityEngine.Random.Range(vector.y + 5f, vector2.y - 5f)));
		InfinilichCarnageWalls1.s_wallPoints.Add(new Vector2(vector2.x, UnityEngine.Random.Range(vector.y + 5f, vector2.y - 5f)));
		Vector2 vector3 = BraveUtility.RandomElement<Vector2>(InfinilichCarnageWalls1.s_wallPoints);
		InfinilichCarnageWalls1.s_wallPoints.Remove(vector3);
		startPoint = vector3;
		float num = 0f;
		int num2 = 0;
		for (int i = 0; i < InfinilichCarnageWalls1.s_wallPoints.Count; i++)
		{
			float num3 = Vector2Extensions.SqrDistance(InfinilichCarnageWalls1.s_wallPoints[i], startPoint);
			if (i == 0 || num3 < num)
			{
				num = num3;
				num2 = i;
			}
		}
		InfinilichCarnageWalls1.s_wallPoints.RemoveAt(num2);
		endPoint = BraveUtility.RandomElement<Vector2>(InfinilichCarnageWalls1.s_wallPoints);
	}

	// Token: 0x04000763 RID: 1891
	public bool Done;

	// Token: 0x04000764 RID: 1892
	private const float c_wallBuffer = 5f;

	// Token: 0x04000765 RID: 1893
	private static List<Vector2> s_wallPoints = new List<Vector2>(4);

	// Token: 0x020001FA RID: 506
	public class TipBullet : Bullet
	{
		// Token: 0x06000788 RID: 1928 RVA: 0x00024508 File Offset: 0x00022708
		public TipBullet(InfinilichCarnageWalls1 parentScript)
			: base("carnageTip", false, false, false)
		{
			this.m_parentScript = parentScript;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00024520 File Offset: 0x00022720
		protected override IEnumerator Top()
		{
			int i = 0;
			for (;;)
			{
				base.Fire(new Direction(0f, DirectionType.Relative, -1f), new Speed(0f, SpeedType.Absolute), new InfinilichCarnageWalls1.ChainBullet(this.m_parentScript, this, i, this.Speed));
				yield return base.Wait(2);
				i++;
			}
			yield break;
		}

		// Token: 0x04000766 RID: 1894
		private InfinilichCarnageWalls1 m_parentScript;
	}

	// Token: 0x020001FC RID: 508
	public class ChainBullet : Bullet
	{
		// Token: 0x06000790 RID: 1936 RVA: 0x00024638 File Offset: 0x00022838
		public ChainBullet(InfinilichCarnageWalls1 parentScript, InfinilichCarnageWalls1.TipBullet tip, int spawnDelay, float tipSpeed)
			: base(null, false, false, false)
		{
			this.m_parentScript = parentScript;
			this.m_tip = tip;
			this.m_spawnDelay = spawnDelay;
			this.m_tipSpeed = tipSpeed;
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00024664 File Offset: 0x00022864
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 truePosition = base.Position;
			float wigglePeriod = 0.333f * this.m_tipSpeed;
			float currentOffset = 0f;
			int i = 0;
			while (!this.m_tip.Destroyed)
			{
				float magnitude = 0.75f;
				magnitude = Mathf.Min(magnitude, Mathf.Lerp(0f, 0.75f, (float)i / 20f));
				magnitude = Mathf.Min(magnitude, Mathf.Lerp(0f, 0.75f, (float)this.m_spawnDelay / 10f));
				currentOffset = Mathf.SmoothStep(-magnitude, magnitude, Mathf.PingPong(0.5f + (float)i / 60f * wigglePeriod, 1f));
				base.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, currentOffset);
				yield return base.Wait(1);
				i++;
			}
			float lastOffset = currentOffset;
			for (i = 0; i < 3; i++)
			{
				currentOffset = Mathf.Lerp(lastOffset, 0f, (float)i / 2f);
				base.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, currentOffset);
				yield return base.Wait(1);
			}
			int holdTime = UnityEngine.Random.Range(0, 240);
			int timer = 0;
			while (!this.m_parentScript.Done || timer < holdTime)
			{
				if (this.m_parentScript.Done)
				{
					timer++;
				}
				if (UnityEngine.Random.value < 0.0001f)
				{
					base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0400076C RID: 1900
		private const float WiggleMagnitude = 0.75f;

		// Token: 0x0400076D RID: 1901
		private const float WigglePeriodMultiplier = 0.333f;

		// Token: 0x0400076E RID: 1902
		private InfinilichCarnageWalls1 m_parentScript;

		// Token: 0x0400076F RID: 1903
		private InfinilichCarnageWalls1.TipBullet m_tip;

		// Token: 0x04000770 RID: 1904
		private int m_spawnDelay;

		// Token: 0x04000771 RID: 1905
		private float m_tipSpeed;
	}
}
