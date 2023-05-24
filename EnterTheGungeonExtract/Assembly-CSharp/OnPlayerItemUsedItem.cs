using System;
using UnityEngine;

// Token: 0x0200144E RID: 5198
public class OnPlayerItemUsedItem : PassiveItem
{
	// Token: 0x06007608 RID: 30216 RVA: 0x002EF9A4 File Offset: 0x002EDBA4
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		player.OnUsedPlayerItem += this.DoEffect;
	}

	// Token: 0x06007609 RID: 30217 RVA: 0x002EF9CC File Offset: 0x002EDBCC
	private void DoEffect(PlayerController usingPlayer, PlayerItem usedItem)
	{
		if (Time.realtimeSinceStartup - this.m_lastUsedTime < this.InternalCooldown)
		{
			return;
		}
		this.m_lastUsedTime = Time.realtimeSinceStartup;
		if (UnityEngine.Random.value < this.ActivationChance)
		{
			if (this.TriggersBlank)
			{
				usingPlayer.ForceBlank(25f, 0.5f, false, true, null, true, -1f);
			}
			if (this.TriggersRadialBulletBurst)
			{
				this.RadialBurstSettings.DoBurst(usingPlayer, null, null);
			}
		}
	}

	// Token: 0x0600760A RID: 30218 RVA: 0x002EFA64 File Offset: 0x002EDC64
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		OnPlayerItemUsedItem component = debrisObject.GetComponent<OnPlayerItemUsedItem>();
		player.OnUsedPlayerItem -= this.DoEffect;
		component.m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x0600760B RID: 30219 RVA: 0x002EFA9C File Offset: 0x002EDC9C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040077C1 RID: 30657
	public float ActivationChance = 1f;

	// Token: 0x040077C2 RID: 30658
	public bool TriggersBlank;

	// Token: 0x040077C3 RID: 30659
	public bool TriggersRadialBulletBurst;

	// Token: 0x040077C4 RID: 30660
	[ShowInInspectorIf("TriggersRadialBulletBurst", false)]
	public RadialBurstInterface RadialBurstSettings;

	// Token: 0x040077C5 RID: 30661
	public float InternalCooldown = 10f;

	// Token: 0x040077C6 RID: 30662
	private float m_lastUsedTime = -1000f;
}
