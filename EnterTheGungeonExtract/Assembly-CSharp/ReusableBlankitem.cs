using System;
using UnityEngine;

// Token: 0x0200148B RID: 5259
public class ReusableBlankitem : PlayerItem
{
	// Token: 0x06007790 RID: 30608 RVA: 0x002FA868 File Offset: 0x002F8A68
	protected override void DoEffect(PlayerController user)
	{
		user.ForceBlank(25f, 0.5f, false, true, null, true, -1f);
		if (user.HasActiveBonusSynergy(CustomSynergyType.BULLET_KILN, false))
		{
			int num = this.GlassGuonsToGive;
			int num2 = 0;
			PickupObject component = this.GlassGuonStone.GetComponent<PickupObject>();
			for (int i = 0; i < user.passiveItems.Count; i++)
			{
				if (user.passiveItems[i].PickupObjectId == component.PickupObjectId)
				{
					num2++;
				}
			}
			num = Mathf.Min(num, this.MaxGlassGuons - num2);
			for (int j = 0; j < num; j++)
			{
				EncounterTrackable.SuppressNextNotification = true;
				LootEngine.GivePrefabToPlayer(this.GlassGuonStone, user);
				EncounterTrackable.SuppressNextNotification = false;
			}
		}
		base.DoEffect(user);
	}

	// Token: 0x04007985 RID: 31109
	public GameObject GlassGuonStone;

	// Token: 0x04007986 RID: 31110
	public int GlassGuonsToGive = 1;

	// Token: 0x04007987 RID: 31111
	public int MaxGlassGuons = 4;
}
