using System;
using System.Collections.Generic;

// Token: 0x0200156A RID: 5482
public class MidGameActiveItemData
{
	// Token: 0x06007D87 RID: 32135 RVA: 0x0032D0F0 File Offset: 0x0032B2F0
	public MidGameActiveItemData(PlayerItem p)
	{
		this.PickupID = p.PickupObjectId;
		this.IsOnCooldown = p.IsOnCooldown;
		this.DamageCooldown = p.CurrentDamageCooldown;
		this.RoomCooldown = p.CurrentRoomCooldown;
		this.TimeCooldown = p.CurrentTimeCooldown;
		this.NumberOfUses = p.numberOfUses;
		this.SerializedData = new List<object>();
		p.MidGameSerialize(this.SerializedData);
	}

	// Token: 0x040080AC RID: 32940
	public int PickupID = -1;

	// Token: 0x040080AD RID: 32941
	public bool IsOnCooldown;

	// Token: 0x040080AE RID: 32942
	public float DamageCooldown;

	// Token: 0x040080AF RID: 32943
	public int RoomCooldown;

	// Token: 0x040080B0 RID: 32944
	public float TimeCooldown;

	// Token: 0x040080B1 RID: 32945
	public int NumberOfUses;

	// Token: 0x040080B2 RID: 32946
	public List<object> SerializedData;
}
