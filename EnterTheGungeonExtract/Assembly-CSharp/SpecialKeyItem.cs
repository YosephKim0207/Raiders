using System;

// Token: 0x020014B8 RID: 5304
public class SpecialKeyItem : PassiveItem
{
	// Token: 0x0600789A RID: 30874 RVA: 0x003037B0 File Offset: 0x003019B0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600789B RID: 30875 RVA: 0x003037B8 File Offset: 0x003019B8
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		GameUIRoot.Instance.UpdatePlayerConsumables(player.carriedConsumables);
	}

	// Token: 0x04007AD4 RID: 31444
	public SpecialKeyItem.SpecialKeyType keyType;

	// Token: 0x020014B9 RID: 5305
	public enum SpecialKeyType
	{
		// Token: 0x04007AD6 RID: 31446
		RESOURCEFUL_RAT_LAIR
	}
}
