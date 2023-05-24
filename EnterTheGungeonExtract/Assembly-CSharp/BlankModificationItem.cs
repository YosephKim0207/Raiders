using System;

// Token: 0x02001356 RID: 4950
public class BlankModificationItem : PassiveItem
{
	// Token: 0x06007044 RID: 28740 RVA: 0x002C85E0 File Offset: 0x002C67E0
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.EngageEffect(player);
		base.Pickup(player);
	}

	// Token: 0x06007045 RID: 28741 RVA: 0x002C85FC File Offset: 0x002C67FC
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.DisengageEffect(player);
		debrisObject.GetComponent<BlankModificationItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06007046 RID: 28742 RVA: 0x002C8628 File Offset: 0x002C6828
	protected override void OnDestroy()
	{
		if (this.m_pickedUp)
		{
			this.DisengageEffect(this.m_owner);
		}
		base.OnDestroy();
	}

	// Token: 0x06007047 RID: 28743 RVA: 0x002C8648 File Offset: 0x002C6848
	protected void EngageEffect(PlayerController user)
	{
	}

	// Token: 0x06007048 RID: 28744 RVA: 0x002C864C File Offset: 0x002C684C
	protected void DisengageEffect(PlayerController user)
	{
	}

	// Token: 0x04006FB6 RID: 28598
	public float BlankForceMultiplier = 1f;

	// Token: 0x04006FB7 RID: 28599
	public float BlankStunTime = 1f;

	// Token: 0x04006FB8 RID: 28600
	public bool MakeBlankDealDamage;

	// Token: 0x04006FB9 RID: 28601
	public float BlankDamageRadius = 10f;

	// Token: 0x04006FBA RID: 28602
	public float BlankDamage = 20f;

	// Token: 0x04006FBB RID: 28603
	public float BlankFireChance;

	// Token: 0x04006FBC RID: 28604
	public GameActorFireEffect BlankFireEffect;

	// Token: 0x04006FBD RID: 28605
	public float BlankPoisonChance;

	// Token: 0x04006FBE RID: 28606
	public GameActorHealthEffect BlankPoisonEffect;

	// Token: 0x04006FBF RID: 28607
	public float BlankFreezeChance;

	// Token: 0x04006FC0 RID: 28608
	public GameActorFreezeEffect BlankFreezeEffect;

	// Token: 0x04006FC1 RID: 28609
	public float RegainAmmoFraction;

	// Token: 0x04006FC2 RID: 28610
	public bool BlankReflectsEnemyBullets;
}
