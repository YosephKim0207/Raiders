using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000DF5 RID: 3573
public class StandNearEnemies : MovementBehaviorBase
{
	// Token: 0x06004BAD RID: 19373 RVA: 0x0019C4DC File Offset: 0x0019A6DC
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
	}

	// Token: 0x06004BAE RID: 19374 RVA: 0x0019C4F4 File Offset: 0x0019A6F4
	public override BehaviorResult Update()
	{
		if (this.m_repathTimer > 0f)
		{
			Vector2? targetPos = this.m_targetPos;
			return (targetPos == null) ? BehaviorResult.Continue : BehaviorResult.SkipRemainingClassBehaviors;
		}
		this.UpdateTarget();
		Vector2? targetPos2 = this.m_targetPos;
		if (targetPos2 != null)
		{
			this.m_aiActor.PathfindToPosition(this.m_targetPos.Value, null, true, null, null, null, false);
			this.m_repathTimer = this.PathInterval;
		}
		Vector2? targetPos3 = this.m_targetPos;
		return (targetPos3 == null) ? BehaviorResult.Continue : BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x06004BAF RID: 19375 RVA: 0x0019C598 File Offset: 0x0019A798
	private void UpdateTarget()
	{
		this.m_aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref this.m_roomEnemies);
		for (int i = this.m_roomEnemies.Count - 1; i >= 0; i--)
		{
			AIActor aiactor = this.m_roomEnemies[i];
			if (aiactor.IsHarmlessEnemy || !aiactor.IsNormalEnemy || aiactor.healthHaver.IsDead || aiactor == this.m_aiActor)
			{
				this.m_roomEnemies.RemoveAt(i);
			}
		}
		if (this.m_roomEnemies.Count <= 0)
		{
			this.m_targetPos = null;
			return;
		}
		bool flag = false;
		while (!flag)
		{
			flag = true;
			Vector2 zero = Vector2.zero;
			int num = 0;
			for (int j = 0; j < this.m_roomEnemies.Count; j++)
			{
				Vector2 unitCenter = this.m_roomEnemies[j].specRigidbody.UnitCenter;
				BraveMathCollege.WeightedAverage(unitCenter, ref zero, ref num);
			}
			if (num == 1)
			{
				Vector2 normalized = (this.m_aiActor.specRigidbody.UnitCenter - zero).normalized;
				Vector2 vector = zero + normalized * this.DesiredDistance;
				BraveMathCollege.WeightedAverage(vector, ref zero, ref num);
			}
			int num2 = -1;
			float num3 = float.MinValue;
			for (int k = 0; k < this.m_roomEnemies.Count; k++)
			{
				Vector2 unitCenter2 = this.m_roomEnemies[k].specRigidbody.UnitCenter;
				float num4 = Vector2.Distance(unitCenter2, zero);
				if (num4 > this.DesiredDistance && num4 > num3)
				{
					num2 = k;
					num3 = num4;
				}
			}
			if (num2 >= 0)
			{
				this.m_roomEnemies.RemoveAt(num2);
				flag = false;
			}
			this.m_targetPos = new Vector2?(zero);
		}
	}

	// Token: 0x04004179 RID: 16761
	public float PathInterval = 0.25f;

	// Token: 0x0400417A RID: 16762
	public float DesiredDistance = 5f;

	// Token: 0x0400417B RID: 16763
	private float m_repathTimer;

	// Token: 0x0400417C RID: 16764
	private List<AIActor> m_roomEnemies = new List<AIActor>();

	// Token: 0x0400417D RID: 16765
	private Vector2? m_targetPos;
}
