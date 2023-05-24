using System;

// Token: 0x020014AD RID: 5293
public class SkeletonKeyItem : PassiveItem
{
	// Token: 0x0600785E RID: 30814 RVA: 0x0030200C File Offset: 0x0030020C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].carriedConsumables.InfiniteKeys = true;
		}
		base.Pickup(player);
	}

	// Token: 0x0600785F RID: 30815 RVA: 0x00302060 File Offset: 0x00300260
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<SkeletonKeyItem>().m_pickedUpThisRun = true;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].carriedConsumables.InfiniteKeys = false;
		}
		return debrisObject;
	}

	// Token: 0x06007860 RID: 30816 RVA: 0x003020B8 File Offset: 0x003002B8
	protected override void OnDestroy()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].carriedConsumables.InfiniteKeys = false;
		}
		base.OnDestroy();
	}
}
