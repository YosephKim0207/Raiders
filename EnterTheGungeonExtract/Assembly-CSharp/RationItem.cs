using System;
using UnityEngine;

// Token: 0x02001480 RID: 5248
public class RationItem : PlayerItem
{
	// Token: 0x06007755 RID: 30549 RVA: 0x002F939C File Offset: 0x002F759C
	protected override void DoEffect(PlayerController user)
	{
		user.healthHaver.ApplyHealing(this.healingAmount);
		AkSoundEngine.PostEvent("Play_OBJ_med_kit_01", base.gameObject);
		if (this.healVFX != null)
		{
			user.PlayEffectOnActor(this.healVFX, Vector3.zero, true, false, false);
		}
	}

	// Token: 0x06007756 RID: 30550 RVA: 0x002F93F4 File Offset: 0x002F75F4
	public void DoHealOnDeath(PlayerController user)
	{
		this.DoEffect(user);
	}

	// Token: 0x06007757 RID: 30551 RVA: 0x002F9400 File Offset: 0x002F7600
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007953 RID: 31059
	public float healingAmount = 2f;

	// Token: 0x04007954 RID: 31060
	public GameObject healVFX;
}
