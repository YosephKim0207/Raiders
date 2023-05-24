using System;

// Token: 0x0200177A RID: 6010
[Serializable]
public class GameUIAmmoType
{
	// Token: 0x04009306 RID: 37638
	public GameUIAmmoType.AmmoType ammoType;

	// Token: 0x04009307 RID: 37639
	public string customAmmoType = string.Empty;

	// Token: 0x04009308 RID: 37640
	public dfTiledSprite ammoBarFG;

	// Token: 0x04009309 RID: 37641
	public dfTiledSprite ammoBarBG;

	// Token: 0x0200177B RID: 6011
	public enum AmmoType
	{
		// Token: 0x0400930B RID: 37643
		SMALL_BULLET,
		// Token: 0x0400930C RID: 37644
		MEDIUM_BULLET,
		// Token: 0x0400930D RID: 37645
		BEAM,
		// Token: 0x0400930E RID: 37646
		GRENADE,
		// Token: 0x0400930F RID: 37647
		SHOTGUN,
		// Token: 0x04009310 RID: 37648
		SMALL_BLASTER,
		// Token: 0x04009311 RID: 37649
		MEDIUM_BLASTER,
		// Token: 0x04009312 RID: 37650
		NAIL,
		// Token: 0x04009313 RID: 37651
		MUSKETBALL,
		// Token: 0x04009314 RID: 37652
		ARROW,
		// Token: 0x04009315 RID: 37653
		MAGIC,
		// Token: 0x04009316 RID: 37654
		BLUE_SHOTGUN,
		// Token: 0x04009317 RID: 37655
		SKULL,
		// Token: 0x04009318 RID: 37656
		FISH,
		// Token: 0x04009319 RID: 37657
		CUSTOM
	}
}
