using System;

// Token: 0x02001377 RID: 4983
public class CogOfBattleItem : PassiveItem
{
	// Token: 0x060070E9 RID: 28905 RVA: 0x002CD064 File Offset: 0x002CB264
	private void Awake()
	{
		CogOfBattleItem.ACTIVE_RELOAD_DAMAGE_MULTIPLIER = this.DamageMultiplier;
	}

	// Token: 0x060070EA RID: 28906 RVA: 0x002CD074 File Offset: 0x002CB274
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_localOwner = player;
		if (player.IsPrimaryPlayer)
		{
			Gun.ActiveReloadActivated = true;
		}
		else
		{
			Gun.ActiveReloadActivatedPlayerTwo = true;
		}
		base.Pickup(player);
	}

	// Token: 0x060070EB RID: 28907 RVA: 0x002CD0AC File Offset: 0x002CB2AC
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_localOwner = null;
		if (player.IsPrimaryPlayer)
		{
			Gun.ActiveReloadActivated = false;
		}
		else
		{
			Gun.ActiveReloadActivatedPlayerTwo = false;
		}
		debrisObject.GetComponent<CogOfBattleItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x060070EC RID: 28908 RVA: 0x002CD0F4 File Offset: 0x002CB2F4
	protected override void OnDestroy()
	{
		if (this.m_localOwner != null)
		{
			if (this.m_localOwner.IsPrimaryPlayer)
			{
				Gun.ActiveReloadActivated = false;
			}
			else
			{
				Gun.ActiveReloadActivatedPlayerTwo = false;
			}
		}
		base.OnDestroy();
	}

	// Token: 0x0400706B RID: 28779
	public static float ACTIVE_RELOAD_DAMAGE_MULTIPLIER = 1.25f;

	// Token: 0x0400706C RID: 28780
	public float DamageMultiplier = 1.25f;

	// Token: 0x0400706D RID: 28781
	private PlayerController m_localOwner;
}
