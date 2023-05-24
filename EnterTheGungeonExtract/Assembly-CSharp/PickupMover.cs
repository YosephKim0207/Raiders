using System;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x020015BD RID: 5565
public class PickupMover : BraveBehaviour
{
	// Token: 0x06007FD5 RID: 32725 RVA: 0x0033A250 File Offset: 0x00338450
	public void Start()
	{
		this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
		if (this.m_room == null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else if (this.moveIfRoomUnclear)
		{
			this.m_shouldPath = true;
		}
		else if (!this.m_room.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
		{
			this.m_shouldPath = true;
		}
		else
		{
			RoomHandler room = this.m_room;
			room.OnEnemiesCleared = (Action)Delegate.Combine(room.OnEnemiesCleared, new Action(this.RoomCleared));
		}
		this.m_lastPosition = base.specRigidbody.UnitCenter;
	}

	// Token: 0x06007FD6 RID: 32726 RVA: 0x0033A30C File Offset: 0x0033850C
	private void TestRadius(PlayerController targetPlayer)
	{
		if (!targetPlayer)
		{
			return;
		}
		if (this.m_radiusValid && !this.stopPathingOnContact)
		{
			return;
		}
		if (targetPlayer.CurrentRoom != this.m_room)
		{
			this.m_radiusValid = true;
		}
		if (this.minRadius <= 0f)
		{
			this.m_radiusValid = true;
		}
		else if (Vector2.Distance(targetPlayer.CenterPosition, base.sprite.WorldCenter) > this.minRadius)
		{
			this.m_radiusValid = true;
		}
		else if (this.stopPathingOnContact)
		{
			this.m_shouldPath = false;
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06007FD7 RID: 32727 RVA: 0x0033A3B8 File Offset: 0x003385B8
	public void Update()
	{
		this.m_pathTimer -= BraveTime.DeltaTime;
		Vector2 unitCenter = base.specRigidbody.UnitCenter;
		PlayerController activePlayerClosestToPoint = GameManager.Instance.GetActivePlayerClosestToPoint(unitCenter, true);
		this.TestRadius(activePlayerClosestToPoint);
		if (this.m_shouldPath && this.m_radiusValid)
		{
			if (base.debris)
			{
				base.debris.enabled = false;
			}
			if (!activePlayerClosestToPoint || activePlayerClosestToPoint.IsFalling || !activePlayerClosestToPoint.specRigidbody.CollideWithOthers)
			{
				this.m_currentSpeed = 0f;
			}
			else
			{
				if (this.m_pathTimer <= 0f)
				{
					IntVector2 intVector = unitCenter.ToIntVector2(VectorConversions.Floor);
					IntVector2 intVector2 = activePlayerClosestToPoint.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
					bool flag = Pathfinder.Instance.IsValidPathCell(intVector) && Pathfinder.Instance.IsValidPathCell(intVector2);
					if (flag && Pathfinder.Instance.GetPath(intVector, intVector2, out this.m_currentPath, new IntVector2?(IntVector2.One), CellTypes.FLOOR | CellTypes.PIT, null, null, false))
					{
						if (this.m_currentPath.Count == 0)
						{
							this.m_currentPath = null;
						}
						else
						{
							this.m_currentPath.Smooth(base.specRigidbody.UnitCenter, base.specRigidbody.UnitDimensions / 2f, CellTypes.FLOOR | CellTypes.PIT, false, IntVector2.One);
						}
					}
					this.m_pathTimer = this.pathInterval;
				}
				this.m_currentSpeed = Mathf.Min(this.m_currentSpeed + this.acceleration * BraveTime.DeltaTime, this.maxSpeed);
			}
			base.specRigidbody.Velocity = this.GetPathVelocityContribution(activePlayerClosestToPoint);
		}
		else
		{
			if (!Mathf.Approximately(base.transform.position.x % 0.0625f, 0f))
			{
				base.transform.position = base.transform.position.Quantize(0.0625f);
				base.specRigidbody.Reinitialize();
			}
			this.m_currentSpeed = 0f;
			base.specRigidbody.Velocity = Vector2.zero;
		}
		this.m_lastPosition = unitCenter;
	}

	// Token: 0x06007FD8 RID: 32728 RVA: 0x0033A5E0 File Offset: 0x003387E0
	public void RoomCleared()
	{
		this.m_shouldPath = true;
		RoomHandler room = this.m_room;
		room.OnEnemiesCleared = (Action)Delegate.Remove(room.OnEnemiesCleared, new Action(this.RoomCleared));
	}

	// Token: 0x06007FD9 RID: 32729 RVA: 0x0033A610 File Offset: 0x00338810
	private Vector2 GetPathVelocityContribution(PlayerController targetPlayer)
	{
		if (!targetPlayer)
		{
			return Vector2.zero;
		}
		if (this.m_currentPath == null || this.m_currentPath.Count == 0)
		{
			return this.m_currentSpeed * (targetPlayer.specRigidbody.UnitCenter - base.specRigidbody.UnitCenter).normalized;
		}
		Vector2 unitCenter = base.specRigidbody.UnitCenter;
		Vector2 firstCenterVector = this.m_currentPath.GetFirstCenterVector2();
		bool flag = false;
		if (Vector2.Distance(unitCenter, firstCenterVector) < PhysicsEngine.PixelToUnit(1))
		{
			flag = true;
		}
		else
		{
			Vector2 vector = BraveMathCollege.ClosestPointOnLineSegment(firstCenterVector, this.m_lastPosition, unitCenter);
			if (Vector2.Distance(firstCenterVector, vector) < PhysicsEngine.PixelToUnit(1))
			{
				flag = true;
			}
		}
		if (flag)
		{
			this.m_currentPath.RemoveFirst();
			if (this.m_currentPath.Count == 0)
			{
				this.m_currentPath = null;
				return Vector2.zero;
			}
		}
		return this.m_currentSpeed * (firstCenterVector - unitCenter).normalized;
	}

	// Token: 0x06007FDA RID: 32730 RVA: 0x0033A718 File Offset: 0x00338918
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008255 RID: 33365
	public float pathInterval = 0.25f;

	// Token: 0x04008256 RID: 33366
	public float acceleration = 2.5f;

	// Token: 0x04008257 RID: 33367
	public float maxSpeed = 15f;

	// Token: 0x04008258 RID: 33368
	public float minRadius;

	// Token: 0x04008259 RID: 33369
	[NonSerialized]
	public bool moveIfRoomUnclear;

	// Token: 0x0400825A RID: 33370
	[NonSerialized]
	public bool stopPathingOnContact;

	// Token: 0x0400825B RID: 33371
	private RoomHandler m_room;

	// Token: 0x0400825C RID: 33372
	private bool m_radiusValid;

	// Token: 0x0400825D RID: 33373
	private bool m_shouldPath;

	// Token: 0x0400825E RID: 33374
	private Path m_currentPath;

	// Token: 0x0400825F RID: 33375
	private float m_pathTimer;

	// Token: 0x04008260 RID: 33376
	private Vector2 m_lastPosition;

	// Token: 0x04008261 RID: 33377
	private float m_currentSpeed;

	// Token: 0x04008262 RID: 33378
	private const CellTypes c_pathableTiles = CellTypes.FLOOR | CellTypes.PIT;
}
