using System;

// Token: 0x02001446 RID: 5190
public class ObsidianShellItem : PassiveItem
{
	// Token: 0x060075D7 RID: 30167 RVA: 0x002EEAD0 File Offset: 0x002ECCD0
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
	}

	// Token: 0x060075D8 RID: 30168 RVA: 0x002EEAE8 File Offset: 0x002ECCE8
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<ObsidianShellItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x060075D9 RID: 30169 RVA: 0x002EEB0C File Offset: 0x002ECD0C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
