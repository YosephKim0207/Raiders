using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000E06 RID: 3590
public class RedBarrelAwareness : OverrideBehaviorBase
{
	// Token: 0x06004C05 RID: 19461 RVA: 0x0019ECC0 File Offset: 0x0019CEC0
	public override void Start()
	{
		base.Start();
		GameManager.Instance.Dungeon.StartCoroutine(this.Initialize());
	}

	// Token: 0x06004C06 RID: 19462 RVA: 0x0019ECE0 File Offset: 0x0019CEE0
	private IEnumerator Initialize()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		DungeonData dungeonData = GameManager.Instance.Dungeon.data;
		RoomHandler room = dungeonData.GetAbsoluteRoomFromPosition(this.m_aiActor.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
		List<MinorBreakable> minorBreakables = StaticReferenceManager.AllMinorBreakables;
		this.m_roomRedBarrels = new List<MinorBreakable>();
		for (int i = 0; i < minorBreakables.Count; i++)
		{
			if (minorBreakables[i].explodesOnBreak)
			{
				if (dungeonData.GetAbsoluteRoomFromPosition(minorBreakables[i].transform.position.IntXY(VectorConversions.Floor)) == room)
				{
					this.m_roomRedBarrels.Add(minorBreakables[i]);
				}
			}
		}
		yield break;
	}

	// Token: 0x06004C07 RID: 19463 RVA: 0x0019ECFC File Offset: 0x0019CEFC
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (this.m_aiActor.OverrideTarget != null && !this.m_aiActor.OverrideTarget)
		{
			this.m_aiActor.OverrideTarget = null;
		}
		for (int i = 0; i < this.m_roomRedBarrels.Count; i++)
		{
			MinorBreakable minorBreakable = this.m_roomRedBarrels[i];
			if (!minorBreakable)
			{
				this.m_roomRedBarrels.RemoveAt(i);
				i--;
			}
			else if (minorBreakable.IsBroken)
			{
				if (this.m_aiActor.OverrideTarget == minorBreakable.specRigidbody)
				{
					this.m_aiActor.OverrideTarget = null;
				}
				this.m_roomRedBarrels.RemoveAt(i);
				i--;
			}
		}
		if (this.AvoidRedBarrels)
		{
			behaviorResult = this.HandleAvoidance();
		}
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (this.ShootRedBarrels)
		{
			behaviorResult = this.HandleShooting();
		}
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (this.PushRedBarrels)
		{
			behaviorResult = this.HandlePushing();
		}
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		return behaviorResult;
	}

	// Token: 0x06004C08 RID: 19464 RVA: 0x0019EE2C File Offset: 0x0019D02C
	protected BehaviorResult HandleAvoidance()
	{
		return BehaviorResult.Continue;
	}

	// Token: 0x06004C09 RID: 19465 RVA: 0x0019EE30 File Offset: 0x0019D030
	protected BehaviorResult HandleShooting()
	{
		if (this.m_aiActor.TargetRigidbody == null)
		{
			return BehaviorResult.Continue;
		}
		float desiredCombatDistance = this.m_aiActor.DesiredCombatDistance;
		for (int i = 0; i < this.m_roomRedBarrels.Count; i++)
		{
			Vector2 unitCenter = this.m_roomRedBarrels[i].specRigidbody.UnitCenter;
			if (!GameManager.Instance.Dungeon.data.isTopWall((int)unitCenter.x, (int)unitCenter.y))
			{
				float num = Vector2.Distance(unitCenter, this.m_aiActor.TargetRigidbody.UnitCenter);
				if (num <= this.m_roomRedBarrels[i].explosionData.GetDefinedDamageRadius())
				{
					float num2 = Vector2.Distance(unitCenter, this.m_aiActor.specRigidbody.UnitCenter);
					if (num2 <= desiredCombatDistance * 1.25f)
					{
						this.m_aiActor.OverrideTarget = this.m_roomRedBarrels[i].specRigidbody;
					}
				}
			}
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004C0A RID: 19466 RVA: 0x0019EF44 File Offset: 0x0019D144
	protected BehaviorResult HandlePushing()
	{
		return BehaviorResult.Continue;
	}

	// Token: 0x040041DF RID: 16863
	public bool AvoidRedBarrels = true;

	// Token: 0x040041E0 RID: 16864
	public bool ShootRedBarrels = true;

	// Token: 0x040041E1 RID: 16865
	public bool PushRedBarrels = true;

	// Token: 0x040041E2 RID: 16866
	protected List<MinorBreakable> m_roomRedBarrels;
}
