using System;
using UnityEngine;

// Token: 0x0200147F RID: 5247
public class RandomProjectileReplacementItem : PassiveItem
{
	// Token: 0x06007750 RID: 30544 RVA: 0x002F91C0 File Offset: 0x002F73C0
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Combine(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModification));
	}

	// Token: 0x06007751 RID: 30545 RVA: 0x002F9200 File Offset: 0x002F7400
	private Projectile HandlePreFireProjectileModification(Gun sourceGun, Projectile sourceProjectile)
	{
		if ((sourceGun && sourceGun.IsHeroSword) || sourceGun.MovesPlayerForwardOnChargeFire)
		{
			return sourceProjectile;
		}
		float num = 1f / sourceGun.DefaultModule.cooldownTime;
		if (sourceGun.Volley != null)
		{
			float num2 = 0f;
			for (int i = 0; i < sourceGun.Volley.projectiles.Count; i++)
			{
				ProjectileModule projectileModule = sourceGun.Volley.projectiles[i];
				num2 += projectileModule.GetEstimatedShotsPerSecond(sourceGun.reloadTime);
			}
			if (num2 > 0f)
			{
				num = num2;
			}
		}
		float num3 = Mathf.Clamp01(this.ChancePerSecondToTrigger / num);
		num3 = Mathf.Max(0.0001f, num3);
		if (UnityEngine.Random.value > num3)
		{
			return sourceProjectile;
		}
		if (!string.IsNullOrEmpty(this.ReplacementAudioEvent))
		{
			AkSoundEngine.PostEvent(this.ReplacementAudioEvent, base.gameObject);
		}
		return this.ReplacementProjectile;
	}

	// Token: 0x06007752 RID: 30546 RVA: 0x002F92FC File Offset: 0x002F74FC
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<RandomProjectileReplacementItem>().m_pickedUpThisRun = true;
		player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Remove(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModification));
		return debrisObject;
	}

	// Token: 0x06007753 RID: 30547 RVA: 0x002F9348 File Offset: 0x002F7548
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			PlayerController player = this.m_player;
			player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Remove(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModification));
		}
	}

	// Token: 0x0400794F RID: 31055
	public float ChancePerSecondToTrigger = 0.01f;

	// Token: 0x04007950 RID: 31056
	public Projectile ReplacementProjectile;

	// Token: 0x04007951 RID: 31057
	public string ReplacementAudioEvent;

	// Token: 0x04007952 RID: 31058
	private PlayerController m_player;
}
