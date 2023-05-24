using System;

// Token: 0x02001474 RID: 5236
public class RadialCharmItem : AffectEnemiesInRadiusItem
{
	// Token: 0x06007707 RID: 30471 RVA: 0x002F79A0 File Offset: 0x002F5BA0
	protected override void AffectEnemy(AIActor target)
	{
		if (this.DoCharm)
		{
			target.ApplyEffect(this.CharmEffect, 1f, null);
			if (this.HasProjectileSynergy && this.LastOwner && this.LastOwner.HasActiveBonusSynergy(this.ProjectileSynergyRequired, false))
			{
				VolleyUtility.FireVolley(this.SynergyVolley, this.LastOwner.CenterPosition, target.CenterPosition - this.LastOwner.CenterPosition, this.LastOwner, false);
			}
		}
	}

	// Token: 0x06007708 RID: 30472 RVA: 0x002F7A30 File Offset: 0x002F5C30
	protected override void AffectShop(BaseShopController target)
	{
		if (this.DoCharm)
		{
			FakeGameActorEffectHandler componentInChildren = target.GetComponentInChildren<FakeGameActorEffectHandler>();
			componentInChildren.ApplyEffect(this.CharmEffect);
			target.SetCapableOfBeingStolenFrom(true, "RadialCharmItem", new float?(this.CharmEffect.duration));
		}
	}

	// Token: 0x06007709 RID: 30473 RVA: 0x002F7A78 File Offset: 0x002F5C78
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400790A RID: 30986
	public bool DoCharm = true;

	// Token: 0x0400790B RID: 30987
	[ShowInInspectorIf("DoCharm", false)]
	public GameActorCharmEffect CharmEffect;

	// Token: 0x0400790C RID: 30988
	public bool HasProjectileSynergy;

	// Token: 0x0400790D RID: 30989
	[LongNumericEnum]
	public CustomSynergyType ProjectileSynergyRequired;

	// Token: 0x0400790E RID: 30990
	public ProjectileVolleyData SynergyVolley;
}
