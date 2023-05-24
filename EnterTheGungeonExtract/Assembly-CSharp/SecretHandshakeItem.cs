using System;

// Token: 0x0200149D RID: 5277
public class SecretHandshakeItem : PassiveItem
{
	// Token: 0x0600780F RID: 30735 RVA: 0x002FFA6C File Offset: 0x002FDC6C
	private void Awake()
	{
	}

	// Token: 0x06007810 RID: 30736 RVA: 0x002FFA70 File Offset: 0x002FDC70
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		SecretHandshakeItem.NumActive++;
		base.Pickup(player);
	}

	// Token: 0x06007811 RID: 30737 RVA: 0x002FFA94 File Offset: 0x002FDC94
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		SecretHandshakeItem.NumActive--;
		debrisObject.GetComponent<SecretHandshakeItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06007812 RID: 30738 RVA: 0x002FFAC4 File Offset: 0x002FDCC4
	protected override void OnDestroy()
	{
		if (this.m_pickedUp)
		{
			SecretHandshakeItem.NumActive--;
		}
		base.OnDestroy();
	}

	// Token: 0x04007A36 RID: 31286
	public static int NumActive;
}
