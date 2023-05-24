using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020013AF RID: 5039
public class FoldingTableItem : PlayerItem
{
	// Token: 0x0600722F RID: 29231 RVA: 0x002D6074 File Offset: 0x002D4274
	public override bool CanBeUsed(PlayerController user)
	{
		if (!user || user.InExitCell || user.CurrentRoom == null)
		{
			return false;
		}
		Vector2 vector = user.CenterPosition + (user.unadjustedAimPoint.XY() - user.CenterPosition).normalized;
		return user.CurrentRoom.GetNearestAvailableCell(vector, new IntVector2?(IntVector2.One), new CellTypes?(CellTypes.FLOOR), false, null) != null && base.CanBeUsed(user);
	}

	// Token: 0x06007230 RID: 29232 RVA: 0x002D6104 File Offset: 0x002D4304
	protected override void DoEffect(PlayerController user)
	{
		base.DoEffect(user);
		AkSoundEngine.PostEvent("Play_ITM_Folding_Table_Use_01", base.gameObject);
		Vector2 vector = user.CenterPosition + (user.unadjustedAimPoint.XY() - user.CenterPosition).normalized;
		IntVector2? nearestAvailableCell = user.CurrentRoom.GetNearestAvailableCell(vector, new IntVector2?(IntVector2.One), new CellTypes?(CellTypes.FLOOR), false, null);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TableToSpawn.gameObject, nearestAvailableCell.Value.ToVector2(), Quaternion.identity);
		SpeculativeRigidbody componentInChildren = gameObject.GetComponentInChildren<SpeculativeRigidbody>();
		FlippableCover component = gameObject.GetComponent<FlippableCover>();
		component.transform.position.XY().GetAbsoluteRoom().RegisterInteractable(component);
		component.ConfigureOnPlacement(component.transform.position.XY().GetAbsoluteRoom());
		componentInChildren.Initialize();
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(componentInChildren, null, false);
	}

	// Token: 0x04007396 RID: 29590
	public FlippableCover TableToSpawn;
}
