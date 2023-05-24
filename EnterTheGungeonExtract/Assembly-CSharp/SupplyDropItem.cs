using System;

// Token: 0x020014C6 RID: 5318
public class SupplyDropItem : PlayerItem
{
	// Token: 0x060078DE RID: 30942 RVA: 0x00305614 File Offset: 0x00303814
	public override bool CanBeUsed(PlayerController user)
	{
		if (this.IsAmmoDrop)
		{
			if (user.HasActiveBonusSynergy(this.improvementSynergy, false))
			{
				return true;
			}
			if (user.CurrentGun == null || user.CurrentGun.InfiniteAmmo || !user.CurrentGun.CanGainAmmo || user.CurrentGun.CurrentAmmo == user.CurrentGun.AdjustedMaxAmmo)
			{
				return false;
			}
		}
		if (user.CurrentRoom != null)
		{
			if (user.InExitCell)
			{
				return false;
			}
			if (user.CurrentRoom.area.IsProceduralRoom && user.CurrentRoom.area.proceduralCells != null)
			{
				return false;
			}
		}
		return base.CanBeUsed(user);
	}

	// Token: 0x060078DF RID: 30943 RVA: 0x003056DC File Offset: 0x003038DC
	protected override void DoEffect(PlayerController user)
	{
		IntVector2 intVector = user.SpawnEmergencyCrate(this.itemTableToUse);
		if (user.HasActiveBonusSynergy(this.improvementSynergy, false))
		{
			GameManager.Instance.Dungeon.data[intVector].PreventRewardSpawn = true;
			IntVector2 intVector2 = user.SpawnEmergencyCrate(this.synergyItemTableToUse01);
			GameManager.Instance.Dungeon.data[intVector2].PreventRewardSpawn = true;
			user.SpawnEmergencyCrate(this.synergyItemTableToUse02);
			GameManager.Instance.Dungeon.data[intVector].PreventRewardSpawn = false;
			GameManager.Instance.Dungeon.data[intVector2].PreventRewardSpawn = false;
		}
		AkSoundEngine.PostEvent("Play_OBJ_supplydrop_activate_01", base.gameObject);
	}

	// Token: 0x060078E0 RID: 30944 RVA: 0x003057A0 File Offset: 0x003039A0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007B22 RID: 31522
	public GenericLootTable itemTableToUse;

	// Token: 0x04007B23 RID: 31523
	public CustomSynergyType improvementSynergy;

	// Token: 0x04007B24 RID: 31524
	public GenericLootTable synergyItemTableToUse01;

	// Token: 0x04007B25 RID: 31525
	public GenericLootTable synergyItemTableToUse02;

	// Token: 0x04007B26 RID: 31526
	public bool IsAmmoDrop;
}
