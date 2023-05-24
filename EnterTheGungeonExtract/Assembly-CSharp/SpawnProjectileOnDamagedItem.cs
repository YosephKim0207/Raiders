using System;
using UnityEngine;

// Token: 0x020014B7 RID: 5303
public class SpawnProjectileOnDamagedItem : PassiveItem
{
	// Token: 0x06007895 RID: 30869 RVA: 0x00303640 File Offset: 0x00301840
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		player.OnReceivedDamage += this.PlayerWasDamaged;
		base.Pickup(player);
	}

	// Token: 0x06007896 RID: 30870 RVA: 0x00303668 File Offset: 0x00301868
	private void PlayerWasDamaged(PlayerController obj)
	{
		if (UnityEngine.Random.value < this.chanceToSpawn)
		{
			int num = UnityEngine.Random.Range(this.minNumToSpawn, this.maxNumToSpawn + 1);
			float num2 = 360f / (float)num;
			float num3 = UnityEngine.Random.Range(0f, num2);
			Projectile projectile = this.projectileToSpawn;
			if (this.CanBeModifiedBySynergy && obj && obj.HasActiveBonusSynergy(this.SynergyToCheck, false))
			{
				projectile = this.synergyProjectile;
			}
			for (int i = 0; i < num; i++)
			{
				float num4 = ((!this.randomAngle) ? (num3 + num2 * (float)i) : ((float)UnityEngine.Random.Range(0, 360)));
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, num4), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				component.Owner = obj;
				component.Shooter = obj.specRigidbody;
			}
		}
	}

	// Token: 0x06007897 RID: 30871 RVA: 0x0030376C File Offset: 0x0030196C
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		player.OnReceivedDamage -= this.PlayerWasDamaged;
		debrisObject.GetComponent<SpawnProjectileOnDamagedItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06007898 RID: 30872 RVA: 0x003037A0 File Offset: 0x003019A0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007ACC RID: 31436
	public float chanceToSpawn = 1f;

	// Token: 0x04007ACD RID: 31437
	public int minNumToSpawn = 1;

	// Token: 0x04007ACE RID: 31438
	public int maxNumToSpawn = 1;

	// Token: 0x04007ACF RID: 31439
	public Projectile projectileToSpawn;

	// Token: 0x04007AD0 RID: 31440
	public bool CanBeModifiedBySynergy;

	// Token: 0x04007AD1 RID: 31441
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04007AD2 RID: 31442
	public Projectile synergyProjectile;

	// Token: 0x04007AD3 RID: 31443
	public bool randomAngle = true;
}
