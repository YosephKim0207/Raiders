using System;

// Token: 0x0200134E RID: 4942
public class AutoblankVestItem : PassiveItem
{
	// Token: 0x0600700E RID: 28686 RVA: 0x002C66EC File Offset: 0x002C48EC
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		HealthHaver healthHaver = player.healthHaver;
		healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
	}

	// Token: 0x0600700F RID: 28687 RVA: 0x002C6728 File Offset: 0x002C4928
	private bool HasElderBlank()
	{
		return this.m_owner.HasActiveItem(this.ElderBlankID);
	}

	// Token: 0x06007010 RID: 28688 RVA: 0x002C6744 File Offset: 0x002C4944
	private void HandleEffect(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
	{
		if (args == EventArgs.Empty)
		{
			return;
		}
		if (args.ModifiedDamage <= 0f)
		{
			return;
		}
		if (!source.IsVulnerable)
		{
			return;
		}
		if (this.m_owner && this.HasElderBlank())
		{
			for (int i = 0; i < this.m_owner.activeItems.Count; i++)
			{
				if (this.m_owner.activeItems[i].PickupObjectId == this.ElderBlankID && !this.m_owner.activeItems[i].IsOnCooldown)
				{
					source.TriggerInvulnerabilityPeriod(-1f);
					this.m_owner.ForceBlank(25f, 0.5f, false, true, null, true, -1f);
					this.m_owner.activeItems[i].ForceApplyCooldown(this.m_owner);
					args.ModifiedDamage = 0f;
					return;
				}
			}
		}
		if (this.m_owner && this.m_owner.Blanks > 0 && !this.m_owner.IsFalling)
		{
			source.TriggerInvulnerabilityPeriod(-1f);
			this.m_owner.ForceConsumableBlank();
			args.ModifiedDamage = 0f;
		}
	}

	// Token: 0x06007011 RID: 28689 RVA: 0x002C68A0 File Offset: 0x002C4AA0
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		AutoblankVestItem component = debrisObject.GetComponent<AutoblankVestItem>();
		HealthHaver healthHaver = player.healthHaver;
		healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Remove(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
		component.m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06007012 RID: 28690 RVA: 0x002C68EC File Offset: 0x002C4AEC
	protected override void OnDestroy()
	{
		if (this.m_owner)
		{
			HealthHaver healthHaver = this.m_owner.healthHaver;
			healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Remove(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleEffect));
		}
		base.OnDestroy();
	}

	// Token: 0x04006F61 RID: 28513
	[PickupIdentifier]
	public int ElderBlankID;
}
