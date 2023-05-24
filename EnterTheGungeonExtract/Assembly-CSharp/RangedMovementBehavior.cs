using System;
using FullInspector;

// Token: 0x02000E0B RID: 3595
public abstract class RangedMovementBehavior : MovementBehaviorBase
{
	// Token: 0x06004C18 RID: 19480 RVA: 0x0019F1C0 File Offset: 0x0019D3C0
	protected bool InRange()
	{
		if (!this.SpecifyRange)
		{
			return true;
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return false;
		}
		float distanceToTarget = this.m_aiActor.DistanceToTarget;
		return distanceToTarget >= this.MinActiveRange && distanceToTarget <= this.MaxActiveRange;
	}

	// Token: 0x040041ED RID: 16877
	public bool SpecifyRange;

	// Token: 0x040041EE RID: 16878
	[InspectorShowIf("SpecifyRange")]
	public float MinActiveRange;

	// Token: 0x040041EF RID: 16879
	[InspectorShowIf("SpecifyRange")]
	public float MaxActiveRange;
}
