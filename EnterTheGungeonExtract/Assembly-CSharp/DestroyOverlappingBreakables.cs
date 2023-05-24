using System;
using System.Collections.Generic;

// Token: 0x02000E64 RID: 3684
public class DestroyOverlappingBreakables : BraveBehaviour
{
	// Token: 0x06004E64 RID: 20068 RVA: 0x001B1690 File Offset: 0x001AF890
	public void Update()
	{
		List<SpeculativeRigidbody> overlappingRigidbodies = PhysicsEngine.Instance.GetOverlappingRigidbodies(base.specRigidbody, null, false);
		for (int i = 0; i < overlappingRigidbodies.Count; i++)
		{
			SpeculativeRigidbody speculativeRigidbody = overlappingRigidbodies[i];
			if (speculativeRigidbody && speculativeRigidbody.minorBreakable)
			{
				speculativeRigidbody.minorBreakable.Break(speculativeRigidbody.UnitCenter - base.specRigidbody.UnitCenter);
			}
		}
		if (!this.everyFrame)
		{
			base.enabled = false;
		}
	}

	// Token: 0x040044C2 RID: 17602
	public bool everyFrame;
}
