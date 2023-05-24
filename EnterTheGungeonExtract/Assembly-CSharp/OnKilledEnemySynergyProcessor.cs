using System;
using UnityEngine;

// Token: 0x02001706 RID: 5894
public class OnKilledEnemySynergyProcessor : MonoBehaviour
{
	// Token: 0x06008909 RID: 35081 RVA: 0x0038D860 File Offset: 0x0038BA60
	private void Awake()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		Projectile projectile = this.m_projectile;
		projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
	}

	// Token: 0x0600890A RID: 35082 RVA: 0x0038D898 File Offset: 0x0038BA98
	private void HandleHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody hitRigidbody, bool killed)
	{
		if ((!this.UsesCooldown || Time.time - OnKilledEnemySynergyProcessor.m_lastActiveTime > this.Cooldown) && (killed || this.TriggersEvenOnJustDamagedEnemy) && hitRigidbody && this.m_projectile.PossibleSourceGun && this.m_projectile.PossibleSourceGun.OwnerHasSynergy(this.SynergyToCheck))
		{
			if (this.UsesCooldown)
			{
				OnKilledEnemySynergyProcessor.m_lastActiveTime = Time.time;
			}
			if (this.DoesRadialBurst)
			{
				this.RadialBurst.DoBurst(this.m_projectile.PossibleSourceGun.CurrentOwner as PlayerController, new Vector2?(hitRigidbody.UnitCenter), null);
			}
			if (this.DoesRadialSlow)
			{
				this.RadialSlow.DoRadialSlow(hitRigidbody.UnitCenter, hitRigidbody.UnitCenter.GetAbsoluteRoom());
			}
			if (this.AddsDroppedCurrency)
			{
				LootEngine.SpawnCurrency(hitRigidbody.UnitCenter, UnityEngine.Random.Range(this.MinCurrency, this.MaxCurrency + 1), false);
			}
			if (this.SpawnsObject)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.ObjectToSpawn, hitRigidbody.UnitCenter, Quaternion.identity);
			}
		}
	}

	// Token: 0x04008ED5 RID: 36565
	[LongNumericEnum]
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008ED6 RID: 36566
	public bool DoesRadialBurst;

	// Token: 0x04008ED7 RID: 36567
	public RadialBurstInterface RadialBurst;

	// Token: 0x04008ED8 RID: 36568
	public bool DoesRadialSlow;

	// Token: 0x04008ED9 RID: 36569
	public RadialSlowInterface RadialSlow;

	// Token: 0x04008EDA RID: 36570
	public bool UsesCooldown;

	// Token: 0x04008EDB RID: 36571
	public float Cooldown;

	// Token: 0x04008EDC RID: 36572
	public bool AddsDroppedCurrency;

	// Token: 0x04008EDD RID: 36573
	public int MinCurrency;

	// Token: 0x04008EDE RID: 36574
	public int MaxCurrency = 5;

	// Token: 0x04008EDF RID: 36575
	public bool TriggersEvenOnJustDamagedEnemy;

	// Token: 0x04008EE0 RID: 36576
	public bool SpawnsObject;

	// Token: 0x04008EE1 RID: 36577
	public GameObject ObjectToSpawn;

	// Token: 0x04008EE2 RID: 36578
	private static float m_lastActiveTime;

	// Token: 0x04008EE3 RID: 36579
	private Projectile m_projectile;
}
