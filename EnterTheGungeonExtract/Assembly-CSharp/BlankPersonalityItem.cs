using System;
using UnityEngine;

// Token: 0x020013A5 RID: 5029
public class BlankPersonalityItem : PassiveItem
{
	// Token: 0x060071F0 RID: 29168 RVA: 0x002D46B4 File Offset: 0x002D28B4
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		this.m_owner = player;
		player.OnReceivedDamage += this.HandleDamageReceived;
	}

	// Token: 0x060071F1 RID: 29169 RVA: 0x002D46D8 File Offset: 0x002D28D8
	private void HandleDamageReceived(PlayerController source)
	{
		source.ForceBlank(25f, 0.5f, false, true, null, true, -1f);
		if (this.ReturnAmmoToAllGunsPercentage > 0f && source.inventory != null && source.inventory.AllGuns != null)
		{
			for (int i = 0; i < source.inventory.AllGuns.Count; i++)
			{
				Gun gun = source.inventory.AllGuns[i];
				if (!gun.InfiniteAmmo && gun.CanGainAmmo)
				{
					gun.GainAmmo(Mathf.CeilToInt((float)gun.AdjustedMaxAmmo * 0.01f * this.ReturnAmmoToAllGunsPercentage));
				}
			}
		}
	}

	// Token: 0x060071F2 RID: 29170 RVA: 0x002D479C File Offset: 0x002D299C
	protected override void DisableEffect(PlayerController disablingPlayer)
	{
		if (disablingPlayer)
		{
			disablingPlayer.OnReceivedDamage -= this.HandleDamageReceived;
		}
		base.DisableEffect(disablingPlayer);
	}

	// Token: 0x04007359 RID: 29529
	[Range(0f, 100f)]
	public float ReturnAmmoToAllGunsPercentage = 5f;
}
