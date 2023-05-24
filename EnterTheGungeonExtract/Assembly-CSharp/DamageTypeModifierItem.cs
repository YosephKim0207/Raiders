using System;

// Token: 0x02001394 RID: 5012
public class DamageTypeModifierItem : PassiveItem
{
	// Token: 0x0600718C RID: 29068 RVA: 0x002D1CA4 File Offset: 0x002CFEA4
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		for (int i = 0; i < this.modifiers.Length; i++)
		{
			player.healthHaver.damageTypeModifiers.Add(this.modifiers[i]);
		}
		base.Pickup(player);
	}

	// Token: 0x0600718D RID: 29069 RVA: 0x002D1CFC File Offset: 0x002CFEFC
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		for (int i = 0; i < this.modifiers.Length; i++)
		{
			player.healthHaver.damageTypeModifiers.Remove(this.modifiers[i]);
		}
		this.m_player = null;
		debrisObject.GetComponent<DamageTypeModifierItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x0600718E RID: 29070 RVA: 0x002D1D58 File Offset: 0x002CFF58
	protected override void OnDestroy()
	{
		if (this.m_player != null)
		{
			for (int i = 0; i < this.modifiers.Length; i++)
			{
				this.m_player.healthHaver.damageTypeModifiers.Remove(this.modifiers[i]);
			}
		}
		base.OnDestroy();
	}

	// Token: 0x040072EA RID: 29418
	public DamageTypeModifier[] modifiers;

	// Token: 0x040072EB RID: 29419
	private PlayerController m_player;
}
