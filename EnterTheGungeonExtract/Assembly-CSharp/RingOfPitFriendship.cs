using System;

// Token: 0x02001493 RID: 5267
public class RingOfPitFriendship : PassiveItem
{
	// Token: 0x060077D6 RID: 30678 RVA: 0x002FD82C File Offset: 0x002FBA2C
	private void Awake()
	{
		this.boolKey = "ringPitFriend" + Guid.NewGuid().ToString();
	}

	// Token: 0x060077D7 RID: 30679 RVA: 0x002FD85C File Offset: 0x002FBA5C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		this.m_currentOwner = player;
		player.ImmuneToPits.SetOverride(this.boolKey, true, null);
	}

	// Token: 0x060077D8 RID: 30680 RVA: 0x002FD8A0 File Offset: 0x002FBAA0
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<RingOfPitFriendship>().m_pickedUpThisRun = true;
		player.ImmuneToPits.SetOverride(this.boolKey, false, null);
		this.m_currentOwner = null;
		return debrisObject;
	}

	// Token: 0x060077D9 RID: 30681 RVA: 0x002FD8E4 File Offset: 0x002FBAE4
	protected override void OnDestroy()
	{
		if (this.m_currentOwner != null)
		{
			this.m_currentOwner.ImmuneToPits.SetOverride(this.boolKey, false, null);
		}
		base.OnDestroy();
	}

	// Token: 0x04007A02 RID: 31234
	private string boolKey = string.Empty;

	// Token: 0x04007A03 RID: 31235
	private PlayerController m_currentOwner;
}
