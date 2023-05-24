using System;
using System.Collections;
using UnityEngine;

// Token: 0x020013AB RID: 5035
public class CrisisStoneItem : PassiveItem
{
	// Token: 0x0600721A RID: 29210 RVA: 0x002D58A0 File Offset: 0x002D3AA0
	protected override void Update()
	{
		base.Update();
		if (this.m_owner && this.m_owner.CurrentGun)
		{
			if (this.m_owner.CurrentGun.ClipShotsRemaining == 0 && !this.m_hasPlayedAudioForOutOfAmmo)
			{
				this.m_hasPlayedAudioForOutOfAmmo = true;
				AkSoundEngine.PostEvent(this.ReloadAudioEvent, base.gameObject);
			}
			else if (this.m_hasPlayedAudioForOutOfAmmo && this.m_owner.CurrentGun.ClipShotsRemaining > 0)
			{
				this.m_hasPlayedAudioForOutOfAmmo = false;
			}
		}
	}

	// Token: 0x0600721B RID: 29211 RVA: 0x002D5940 File Offset: 0x002D3B40
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		this.m_owner = player;
		HealthHaver healthHaver = player.healthHaver;
		healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleDamageModification));
		SpeculativeRigidbody specRigidbody = player.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		player.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Combine(player.OnReloadedGun, new Action<PlayerController, Gun>(this.HandleReloadedGun));
	}

	// Token: 0x0600721C RID: 29212 RVA: 0x002D59CC File Offset: 0x002D3BCC
	private void HandleReloadedGun(PlayerController sourcePlayer, Gun sourceGun)
	{
		if (sourceGun && sourceGun.IsHeroSword)
		{
			return;
		}
		sourcePlayer.StartCoroutine(this.HandleWallVFX(sourcePlayer, sourceGun));
		AkSoundEngine.PostEvent("Play_ITM_Crisis_Stone_Shield_01", base.gameObject);
	}

	// Token: 0x0600721D RID: 29213 RVA: 0x002D5A08 File Offset: 0x002D3C08
	private IEnumerator HandleWallVFX(PlayerController sourcePlayer, Gun sourceGun)
	{
		GameObject instanceVFX = sourcePlayer.PlayEffectOnActor(this.WallVFX, new Vector3(0f, -0.5f, 0f), true, false, false);
		float reloadTime = sourceGun.AdjustedReloadTime;
		while (sourceGun && sourceGun.IsReloading && sourceGun.ClipShotsRemaining == 0)
		{
			reloadTime -= BraveTime.DeltaTime;
			if (reloadTime < 0.15f)
			{
				break;
			}
			yield return null;
		}
		SpawnManager.Despawn(instanceVFX);
		yield break;
	}

	// Token: 0x0600721E RID: 29214 RVA: 0x002D5A34 File Offset: 0x002D3C34
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (this.m_owner && this.m_owner.CurrentGun && this.m_owner.CurrentGun.IsReloading && this.m_owner.CurrentGun.ClipShotsRemaining == 0 && !this.m_owner.CurrentGun.IsHeroSword && rigidbodyCollision.OtherRigidbody)
		{
			Projectile component = rigidbodyCollision.OtherRigidbody.GetComponent<Projectile>();
			if (component)
			{
				this.ImpactVFX.SpawnAtPosition(rigidbodyCollision.Contact, 0f, null, null, null, null, false, null, null, false);
				AkSoundEngine.PostEvent("Play_ITM_Crisis_Stone_Impact_01", base.gameObject);
			}
		}
	}

	// Token: 0x0600721F RID: 29215 RVA: 0x002D5B18 File Offset: 0x002D3D18
	private void HandleDamageModification(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
	{
		if (args == EventArgs.Empty || args.ModifiedDamage <= 0f || !source.IsVulnerable)
		{
			return;
		}
		if (this.m_owner && this.m_owner.CurrentGun && this.m_owner.CurrentGun.IsReloading && this.m_owner.CurrentGun.ClipShotsRemaining == 0 && !this.m_owner.CurrentGun.IsHeroSword)
		{
			args.ModifiedDamage = 0f;
		}
	}

	// Token: 0x06007220 RID: 29216 RVA: 0x002D5BBC File Offset: 0x002D3DBC
	protected override void DisableEffect(PlayerController disablingPlayer)
	{
		if (disablingPlayer)
		{
			HealthHaver healthHaver = disablingPlayer.healthHaver;
			healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Remove(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleDamageModification));
		}
		if (disablingPlayer)
		{
			SpeculativeRigidbody specRigidbody = disablingPlayer.specRigidbody;
			specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		}
		if (disablingPlayer)
		{
			disablingPlayer.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Remove(disablingPlayer.OnReloadedGun, new Action<PlayerController, Gun>(this.HandleReloadedGun));
		}
		base.DisableEffect(disablingPlayer);
	}

	// Token: 0x04007383 RID: 29571
	public string ReloadAudioEvent;

	// Token: 0x04007384 RID: 29572
	public VFXPool ImpactVFX;

	// Token: 0x04007385 RID: 29573
	public GameObject WallVFX;

	// Token: 0x04007386 RID: 29574
	private bool m_hasPlayedAudioForOutOfAmmo;
}
