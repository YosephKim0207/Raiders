using System;
using UnityEngine;

// Token: 0x0200136F RID: 4975
public class ChestBrokenItem : PassiveItem
{
	// Token: 0x060070B9 RID: 28857 RVA: 0x002CBCBC File Offset: 0x002C9EBC
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		player.OnChestBroken = (Action<PlayerController, Chest>)Delegate.Combine(player.OnChestBroken, new Action<PlayerController, Chest>(this.HandleChestBroken));
	}

	// Token: 0x060070BA RID: 28858 RVA: 0x002CBCF4 File Offset: 0x002C9EF4
	private void HandleChestBroken(PlayerController arg1, Chest arg2)
	{
		if (UnityEngine.Random.value < this.ActivationChance)
		{
			arg1.healthHaver.ApplyHealing(this.HealAmount);
			arg1.PlayEffectOnActor(this.HealVFX, Vector3.zero, true, false, false);
		}
	}

	// Token: 0x060070BB RID: 28859 RVA: 0x002CBD2C File Offset: 0x002C9F2C
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		ChestBrokenItem component = debrisObject.GetComponent<ChestBrokenItem>();
		if (player)
		{
			player.OnChestBroken = (Action<PlayerController, Chest>)Delegate.Remove(player.OnChestBroken, new Action<PlayerController, Chest>(this.HandleChestBroken));
		}
		if (component)
		{
			component.m_pickedUpThisRun = true;
		}
		return debrisObject;
	}

	// Token: 0x060070BC RID: 28860 RVA: 0x002CBD88 File Offset: 0x002C9F88
	protected override void OnDestroy()
	{
		if (this.m_owner)
		{
			PlayerController owner = this.m_owner;
			owner.OnChestBroken = (Action<PlayerController, Chest>)Delegate.Remove(owner.OnChestBroken, new Action<PlayerController, Chest>(this.HandleChestBroken));
		}
		base.OnDestroy();
	}

	// Token: 0x04007045 RID: 28741
	public float ActivationChance = 1f;

	// Token: 0x04007046 RID: 28742
	public float HealAmount = 0.5f;

	// Token: 0x04007047 RID: 28743
	public GameObject HealVFX;
}
