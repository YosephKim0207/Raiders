using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DD5 RID: 3541
public class SpawnGoopBehavior : BasicAttackBehavior
{
	// Token: 0x06004B08 RID: 19208 RVA: 0x00195704 File Offset: 0x00193904
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		Vector2 vector = BraveUtility.RandomElement<Vector2>(this.roomOffsets);
		Vector2 vector2 = this.m_aiActor.ParentRoom.area.UnitBottomLeft + vector;
		DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopToUse);
		goopManagerForGoopType.TimedAddGoopCircle(vector2, this.goopRadius, this.goopSpeed, false);
		this.UpdateCooldowns();
		return BehaviorResult.SkipAllRemainingBehaviors;
	}

	// Token: 0x04004042 RID: 16450
	public List<Vector2> roomOffsets;

	// Token: 0x04004043 RID: 16451
	public GoopDefinition goopToUse;

	// Token: 0x04004044 RID: 16452
	public float goopRadius = 3f;

	// Token: 0x04004045 RID: 16453
	public float goopSpeed = 0.5f;
}
