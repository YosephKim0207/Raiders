using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200139E RID: 5022
public class BankMaskItem : PassiveItem, IPaydayItem
{
	// Token: 0x060071C7 RID: 29127 RVA: 0x002D36D0 File Offset: 0x002D18D0
	public void StoreData(string id1, string id2, string id3)
	{
		this.ID01 = id1;
		this.ID02 = id2;
		this.ID03 = id3;
		this.HasSetOrder = true;
	}

	// Token: 0x060071C8 RID: 29128 RVA: 0x002D36F0 File Offset: 0x002D18F0
	public bool HasCachedData()
	{
		return this.HasSetOrder;
	}

	// Token: 0x060071C9 RID: 29129 RVA: 0x002D36F8 File Offset: 0x002D18F8
	public string GetID(int placement)
	{
		if (placement == 0)
		{
			return this.ID01;
		}
		if (placement == 1)
		{
			return this.ID02;
		}
		return this.ID03;
	}

	// Token: 0x060071CA RID: 29130 RVA: 0x002D371C File Offset: 0x002D191C
	public override void MidGameSerialize(List<object> data)
	{
		base.MidGameSerialize(data);
		data.Add(this.HasSetOrder);
		data.Add(this.ID01);
		data.Add(this.ID02);
		data.Add(this.ID03);
	}

	// Token: 0x060071CB RID: 29131 RVA: 0x002D375C File Offset: 0x002D195C
	public override void MidGameDeserialize(List<object> data)
	{
		base.MidGameDeserialize(data);
		if (data.Count == 4)
		{
			this.HasSetOrder = (bool)data[0];
			this.ID01 = (string)data[1];
			this.ID02 = (string)data[2];
			this.ID03 = (string)data[3];
		}
	}

	// Token: 0x060071CC RID: 29132 RVA: 0x002D37C4 File Offset: 0x002D19C4
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		player.OverrideAnimationLibrary = this.OverrideAnimLib;
		player.OverridePlayerSwitchState = PlayableCharacters.Pilot.ToString();
		player.SetOverrideShader(ShaderCache.Acquire(player.LocalShaderName));
		if (player.characterIdentity == PlayableCharacters.Eevee)
		{
			player.GetComponent<CharacterAnimationRandomizer>().AddOverrideAnimLibrary(this.OverrideAnimLib);
		}
		player.ChangeHandsToCustomType(this.OverrideHandSprite.Collection, this.OverrideHandSprite.spriteId);
		if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
		{
			PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
		}
		if (!PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player].Add(base.GetType(), 1);
		}
		else
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = PassiveItem.ActiveFlagItems[player][base.GetType()] + 1;
		}
	}

	// Token: 0x060071CD RID: 29133 RVA: 0x002D38D8 File Offset: 0x002D1AD8
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		player.OverrideAnimationLibrary = null;
		player.OverridePlayerSwitchState = null;
		player.ClearOverrideShader();
		if (player.characterIdentity == PlayableCharacters.Eevee)
		{
			player.GetComponent<CharacterAnimationRandomizer>().RemoveOverrideAnimLibrary(this.OverrideAnimLib);
		}
		player.RevertHandsToBaseType();
		if (PassiveItem.ActiveFlagItems.ContainsKey(player) && PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[player][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[player].Remove(base.GetType());
			}
		}
		debrisObject.GetComponent<BankMaskItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x060071CE RID: 29134 RVA: 0x002D39C4 File Offset: 0x002D1BC4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_pickedUp && this.m_owner)
		{
			this.m_owner.RevertHandsToBaseType();
			this.m_owner.OverrideAnimationLibrary = null;
			this.m_owner.OverridePlayerSwitchState = null;
			this.m_owner.ClearOverrideShader();
			if (this.m_owner.characterIdentity == PlayableCharacters.Eevee)
			{
				this.m_owner.GetComponent<CharacterAnimationRandomizer>().RemoveOverrideAnimLibrary(this.OverrideAnimLib);
			}
			if (PassiveItem.ActiveFlagItems.ContainsKey(this.m_owner) && PassiveItem.ActiveFlagItems[this.m_owner].ContainsKey(base.GetType()))
			{
				PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] - 1);
				if (PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] == 0)
				{
					PassiveItem.ActiveFlagItems[this.m_owner].Remove(base.GetType());
				}
			}
		}
	}

	// Token: 0x0400733A RID: 29498
	public tk2dSpriteAnimation OverrideAnimLib;

	// Token: 0x0400733B RID: 29499
	public tk2dSprite OverrideHandSprite;

	// Token: 0x0400733C RID: 29500
	[NonSerialized]
	public bool HasSetOrder;

	// Token: 0x0400733D RID: 29501
	[NonSerialized]
	public string ID01;

	// Token: 0x0400733E RID: 29502
	[NonSerialized]
	public string ID02;

	// Token: 0x0400733F RID: 29503
	[NonSerialized]
	public string ID03;
}
