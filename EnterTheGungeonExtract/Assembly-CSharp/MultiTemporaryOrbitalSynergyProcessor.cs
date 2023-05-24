using System;
using UnityEngine;

// Token: 0x02001702 RID: 5890
public class MultiTemporaryOrbitalSynergyProcessor : MonoBehaviour
{
	// Token: 0x060088E9 RID: 35049 RVA: 0x0038C94C File Offset: 0x0038AB4C
	private void Start()
	{
		this.m_gun = base.GetComponent<Gun>();
		this.m_layer = new MultiTemporaryOrbitalLayer();
		this.m_layer.collisionDamage = 3f;
	}

	// Token: 0x060088EA RID: 35050 RVA: 0x0038C978 File Offset: 0x0038AB78
	private void Update()
	{
		if (!this.m_attached)
		{
			if (this.m_gun && this.m_gun.CurrentOwner && this.m_gun.OwnerHasSynergy(this.RequiredSynergy))
			{
				PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
				PlayerController playerController2 = playerController;
				playerController2.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(playerController2.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.HandleEnemyDamaged));
				this.m_lastPlayer = playerController;
				this.m_attached = true;
			}
		}
		else if (!this.m_gun || !this.m_gun.CurrentOwner || !this.m_gun.OwnerHasSynergy(this.RequiredSynergy))
		{
			if (this.m_lastPlayer)
			{
				PlayerController lastPlayer = this.m_lastPlayer;
				lastPlayer.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(lastPlayer.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.HandleEnemyDamaged));
			}
			this.m_lastPlayer = null;
			this.m_attached = false;
		}
		if (this.m_hasBeenInitialized)
		{
			this.m_layer.Update();
		}
	}

	// Token: 0x060088EB RID: 35051 RVA: 0x0038CAA8 File Offset: 0x0038ACA8
	private void OnDestroy()
	{
		if (this.m_attached)
		{
			if (this.m_layer != null)
			{
				this.m_layer.Disconnect();
			}
			if (this.m_lastPlayer)
			{
				PlayerController lastPlayer = this.m_lastPlayer;
				lastPlayer.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(lastPlayer.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.HandleEnemyDamaged));
			}
			this.m_lastPlayer = null;
			this.m_attached = false;
		}
	}

	// Token: 0x060088EC RID: 35052 RVA: 0x0038CB1C File Offset: 0x0038AD1C
	private void HandleEnemyDamaged(float dmg, bool fatal, HealthHaver target)
	{
		if (this.m_gun && this.m_gun.CurrentOwner is PlayerController && (this.m_gun.CurrentOwner as PlayerController).CurrentGun == this.m_gun && fatal)
		{
			this.m_layer.targetNumberOrbitals = Mathf.Min(20, this.m_layer.targetNumberOrbitals + 1);
			if (!this.m_hasBeenInitialized)
			{
				this.m_layer.Initialize(this.m_lastPlayer, this.OrbitalPrefab);
				this.m_hasBeenInitialized = true;
			}
		}
	}

	// Token: 0x04008EA8 RID: 36520
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008EA9 RID: 36521
	public GameObject OrbitalPrefab;

	// Token: 0x04008EAA RID: 36522
	private MultiTemporaryOrbitalLayer m_layer;

	// Token: 0x04008EAB RID: 36523
	private bool m_hasBeenInitialized;

	// Token: 0x04008EAC RID: 36524
	private Gun m_gun;

	// Token: 0x04008EAD RID: 36525
	private bool m_attached;

	// Token: 0x04008EAE RID: 36526
	private PlayerController m_lastPlayer;
}
