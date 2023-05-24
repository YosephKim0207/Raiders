using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001683 RID: 5763
public class TachyonProjectile : Projectile
{
	// Token: 0x06008668 RID: 34408 RVA: 0x0037A15C File Offset: 0x0037835C
	public override void Start()
	{
		base.Start();
		Vector2 unitPosition = base.specRigidbody.Position.UnitPosition;
		Vector2 vector = this.FindExpectedEndPoint();
		this.baseData.range = Vector2.Distance(vector, base.transform.position.XY());
		base.transform.position = vector.ToVector3ZisY(0f);
		base.specRigidbody.Reinitialize();
		base.Direction = (vector - unitPosition).normalized;
		base.SendInDirection(base.Direction * -1f, true, true);
		this.m_distanceElapsed = 0f;
		base.LastPosition = base.transform.position;
		this.SpawnVFX.SpawnAtPosition(vector.ToVector3ZisY(0f), 0f, null, null, null, null, false, null, null, false);
	}

	// Token: 0x06008669 RID: 34409 RVA: 0x0037A254 File Offset: 0x00378454
	public override void Update()
	{
		base.Update();
		Vector2 unitCenter = base.specRigidbody.UnitCenter;
		if (unitCenter.GetAbsoluteRoom() != this.m_room)
		{
			base.DieInAir(false, true, true, false);
		}
	}

	// Token: 0x0600866A RID: 34410 RVA: 0x0037A290 File Offset: 0x00378490
	protected Vector2 FindExpectedEndPoint()
	{
		Dungeon dungeon = GameManager.Instance.Dungeon;
		Vector2 unitCenter = base.specRigidbody.UnitCenter;
		Vector2 vector = unitCenter + base.Direction.normalized * this.baseData.range;
		this.m_room = unitCenter.GetAbsoluteRoom();
		bool flag = false;
		Vector2 vector2 = unitCenter;
		IntVector2 intVector = vector2.ToIntVector2(VectorConversions.Floor);
		if (dungeon.data.CheckInBoundsAndValid(intVector))
		{
			flag = dungeon.data[intVector].isExitCell;
		}
		float num = vector.x - unitCenter.x;
		float num2 = vector.y - unitCenter.y;
		float num3 = Mathf.Sign(vector.x - unitCenter.x);
		float num4 = Mathf.Sign(vector.y - unitCenter.y);
		bool flag2 = num3 > 0f;
		bool flag3 = num4 > 0f;
		int num5 = 0;
		while (Vector2.Distance(vector2, vector) > 0.1f && num5 < 10000)
		{
			num5++;
			float num6 = Mathf.Abs((((!flag2) ? Mathf.Floor(vector2.x) : Mathf.Ceil(vector2.x)) - vector2.x) / num);
			float num7 = Mathf.Abs((((!flag3) ? Mathf.Floor(vector2.y) : Mathf.Ceil(vector2.y)) - vector2.y) / num2);
			int num8 = Mathf.FloorToInt(vector2.x);
			int num9 = Mathf.FloorToInt(vector2.y);
			IntVector2 intVector2 = new IntVector2(num8, num9);
			bool flag4 = false;
			if (!dungeon.data.CheckInBoundsAndValid(intVector2))
			{
				break;
			}
			CellData cellData = dungeon.data[intVector2];
			if (cellData.nearestRoom != this.m_room || cellData.isExitCell != flag)
			{
				break;
			}
			if (cellData.type != CellType.WALL)
			{
				flag4 = true;
			}
			if (flag4)
			{
				intVector = intVector2;
			}
			if (num6 < num7)
			{
				num8++;
				vector2.x += num6 * num + 0.1f * Mathf.Sign(num);
				vector2.y += num6 * num2 + 0.1f * Mathf.Sign(num2);
			}
			else
			{
				num9++;
				vector2.x += num7 * num + 0.1f * Mathf.Sign(num);
				vector2.y += num7 * num2 + 0.1f * Mathf.Sign(num2);
			}
		}
		return intVector.ToCenterVector2();
	}

	// Token: 0x0600866B RID: 34411 RVA: 0x0037A554 File Offset: 0x00378754
	protected override void Move()
	{
		base.Move();
	}

	// Token: 0x0600866C RID: 34412 RVA: 0x0037A55C File Offset: 0x0037875C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008B5D RID: 35677
	public float ProjectileRadius = 0.3125f;

	// Token: 0x04008B5E RID: 35678
	public VFXPool SpawnVFX;

	// Token: 0x04008B5F RID: 35679
	private RoomHandler m_room;
}
