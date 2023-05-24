using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013EA RID: 5098
public class TripleTapEffect : MonoBehaviour
{
	// Token: 0x060073A9 RID: 29609 RVA: 0x002E01CC File Offset: 0x002DE3CC
	private void Start()
	{
		this.m_companion = base.GetComponent<AIActor>();
		PlayerController companionOwner = this.m_companion.CompanionOwner;
		if (companionOwner)
		{
			this.m_player = companionOwner;
			this.m_player.PostProcessProjectile += this.PostProcessProjectile;
		}
	}

	// Token: 0x060073AA RID: 29610 RVA: 0x002E021C File Offset: 0x002DE41C
	private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
	{
		if (sourceProjectile.PlayerProjectileSourceGameTimeslice == -1f)
		{
			return;
		}
		if (this.m_slicesFired.ContainsKey(sourceProjectile.PlayerProjectileSourceGameTimeslice))
		{
			this.m_slicesFired[sourceProjectile.PlayerProjectileSourceGameTimeslice] = this.m_slicesFired[sourceProjectile.PlayerProjectileSourceGameTimeslice] + 1;
		}
		else
		{
			this.m_slicesFired.Add(sourceProjectile.PlayerProjectileSourceGameTimeslice, 1);
		}
		sourceProjectile.OnDestruction += this.HandleProjectileDestruction;
	}

	// Token: 0x060073AB RID: 29611 RVA: 0x002E02A0 File Offset: 0x002DE4A0
	private void HandleProjectileDestruction(Projectile source)
	{
		if (source.PlayerProjectileSourceGameTimeslice == -1f)
		{
			return;
		}
		if (!this.m_slicesFired.ContainsKey(source.PlayerProjectileSourceGameTimeslice))
		{
			return;
		}
		if (this.m_player && source)
		{
			if (source.HasImpactedEnemy)
			{
				this.m_slicesFired.Remove(source.PlayerProjectileSourceGameTimeslice);
				if (this.m_player.HasActiveBonusSynergy(CustomSynergyType.GET_IT_ITS_BOWLING, false))
				{
					this.m_shotCounter = Mathf.Min(this.RequiredSequentialShots, this.m_shotCounter + source.NumberHealthHaversHit);
				}
				else
				{
					this.m_shotCounter++;
				}
				if (this.m_shotCounter >= this.RequiredSequentialShots)
				{
					this.m_shotCounter -= this.RequiredSequentialShots;
					if (source.PossibleSourceGun && !source.PossibleSourceGun.InfiniteAmmo && source.PossibleSourceGun.CanGainAmmo)
					{
						source.PossibleSourceGun.GainAmmo(this.AmmoToGain);
					}
				}
			}
			else
			{
				this.m_slicesFired[source.PlayerProjectileSourceGameTimeslice] = this.m_slicesFired[source.PlayerProjectileSourceGameTimeslice] - 1;
				if (this.m_slicesFired[source.PlayerProjectileSourceGameTimeslice] == 0)
				{
					this.m_shotCounter = 0;
				}
			}
		}
	}

	// Token: 0x060073AC RID: 29612 RVA: 0x002E0400 File Offset: 0x002DE600
	private void OnDestroy()
	{
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
		}
	}

	// Token: 0x0400753E RID: 30014
	public int RequiredSequentialShots = 3;

	// Token: 0x0400753F RID: 30015
	public int AmmoToGain = 1;

	// Token: 0x04007540 RID: 30016
	private int m_shotCounter;

	// Token: 0x04007541 RID: 30017
	private AIActor m_companion;

	// Token: 0x04007542 RID: 30018
	private PlayerController m_player;

	// Token: 0x04007543 RID: 30019
	private Dictionary<float, int> m_slicesFired = new Dictionary<float, int>();
}
