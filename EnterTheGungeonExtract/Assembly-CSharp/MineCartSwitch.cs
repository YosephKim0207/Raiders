using System;
using System.Collections.Generic;

// Token: 0x020011B0 RID: 4528
public class MineCartSwitch : DungeonPlaceableBehaviour
{
	// Token: 0x060064F9 RID: 25849 RVA: 0x00273AB8 File Offset: 0x00271CB8
	private void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
	}

	// Token: 0x060064FA RID: 25850 RVA: 0x00273AE4 File Offset: 0x00271CE4
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (rigidbodyCollision.OtherRigidbody.projectile != null)
		{
			List<PathMover> componentsInRoom = base.GetAbsoluteParentRoom().GetComponentsInRoom<PathMover>();
			for (int i = 0; i < componentsInRoom.Count; i++)
			{
				componentsInRoom[i].IsUsingAlternateTargets = !componentsInRoom[i].IsUsingAlternateTargets;
			}
		}
	}

	// Token: 0x060064FB RID: 25851 RVA: 0x00273B48 File Offset: 0x00271D48
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04006097 RID: 24727
	[DwarfConfigurable]
	public float PrimaryPathIndex;

	// Token: 0x04006098 RID: 24728
	[DwarfConfigurable]
	public float TogglePathIndex = 1f;
}
