using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001361 RID: 4961
public class BriefcaseFullOfCashItem : PassiveItem
{
	// Token: 0x0600706C RID: 28780 RVA: 0x002C96CC File Offset: 0x002C78CC
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (this.m_pickedUpThisRun)
		{
			this.m_hasTriggered = true;
		}
		if (!this.m_pickedUpThisRun && !this.m_hasTriggered)
		{
			this.m_hasTriggered = true;
			player.carriedConsumables.Currency += this.CurrencyAmount;
			LootEngine.SpawnCurrency(player.CenterPosition, this.MetaCurrencyAmount, true, new Vector2?(Vector2.down), new float?(45f), 0.5f, 0.25f);
		}
		base.Pickup(player);
	}

	// Token: 0x0600706D RID: 28781 RVA: 0x002C9764 File Offset: 0x002C7964
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<BriefcaseFullOfCashItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x0600706E RID: 28782 RVA: 0x002C9788 File Offset: 0x002C7988
	protected override void OnDestroy()
	{
		BraveTime.ClearMultiplier(base.gameObject);
		if (this.m_pickedUp)
		{
		}
		base.OnDestroy();
	}

	// Token: 0x0600706F RID: 28783 RVA: 0x002C97A8 File Offset: 0x002C79A8
	public override void MidGameSerialize(List<object> data)
	{
		base.MidGameSerialize(data);
		data.Add(this.m_hasTriggered);
	}

	// Token: 0x06007070 RID: 28784 RVA: 0x002C97C4 File Offset: 0x002C79C4
	public override void MidGameDeserialize(List<object> data)
	{
		base.MidGameDeserialize(data);
		if (data.Count == 1)
		{
			this.m_hasTriggered = (bool)data[0];
		}
	}

	// Token: 0x04006FE2 RID: 28642
	public int CurrencyAmount = 200;

	// Token: 0x04006FE3 RID: 28643
	public int MetaCurrencyAmount = 3;

	// Token: 0x04006FE4 RID: 28644
	private bool m_hasTriggered;
}
