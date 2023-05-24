using System;
using System.Collections;
using UnityEngine;

// Token: 0x020013B9 RID: 5049
public class GunnerGunController : MonoBehaviour
{
	// Token: 0x06007271 RID: 29297 RVA: 0x002D8080 File Offset: 0x002D6280
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x06007272 RID: 29298 RVA: 0x002D8090 File Offset: 0x002D6290
	private void HandleReceivedDamage(PlayerController p)
	{
		float num = 0f;
		bool flag = p.CurrentGun && p.CurrentGun == this.m_gun;
		if (flag)
		{
			num = this.ChanceToSkull;
		}
		if (p && p.HasActiveBonusSynergy(CustomSynergyType.WHALE_OF_A_TIME, false))
		{
			num = this.ChanceToTriggerSynergy;
		}
		bool flag2 = UnityEngine.Random.value < num;
		if (flag2 && !this.m_extantSkull && (!flag || (this.m_gun.ammo >= this.AmmoLossOnDamage && this.m_gun.ammo > 0)))
		{
			if (flag)
			{
				this.m_gun.LoseAmmo(this.AmmoLossOnDamage);
			}
			this.m_extantSkull = SpawnManager.SpawnDebris(this.SkullPrefab, p.CenterPosition.ToVector3ZisY(0f), Quaternion.identity);
			DebrisObject component = this.m_extantSkull.GetComponent<DebrisObject>();
			component.FlagAsPickup();
			component.Trigger((UnityEngine.Random.insideUnitCircle.normalized * 20f).ToVector3ZUp(3f), 1f, 0f);
			SpeculativeRigidbody component2 = this.m_extantSkull.GetComponent<SpeculativeRigidbody>();
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(component2, null, false);
			component2.RegisterTemporaryCollisionException(p.specRigidbody, 0.25f, null);
			SpeculativeRigidbody speculativeRigidbody = component2;
			speculativeRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(speculativeRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleSkullTrigger));
			component2.StartCoroutine(this.HandleLifespan(this.m_extantSkull));
		}
		else if (this.m_extantSkull)
		{
			LootEngine.DoDefaultPurplePoof(this.m_extantSkull.transform.position + new Vector3(0.75f, 0.5f, 0f), false);
			UnityEngine.Object.Destroy(this.m_extantSkull.gameObject);
			this.m_extantSkull = null;
		}
	}

	// Token: 0x06007273 RID: 29299 RVA: 0x002D829C File Offset: 0x002D649C
	private IEnumerator HandleLifespan(GameObject source)
	{
		float elapsed = 0f;
		while (elapsed < this.Lifespan)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		if (this.m_extantSkull && source && this.m_extantSkull == source)
		{
			LootEngine.DoDefaultPurplePoof(this.m_extantSkull.transform.position + new Vector3(0.75f, 0.5f, 0f), false);
			UnityEngine.Object.Destroy(this.m_extantSkull.gameObject);
			this.m_extantSkull = null;
		}
		yield break;
	}

	// Token: 0x06007274 RID: 29300 RVA: 0x002D82C0 File Offset: 0x002D64C0
	private void HandleSkullTrigger(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (specRigidbody)
		{
			PlayerController component = specRigidbody.GetComponent<PlayerController>();
			if (component && ((component.CurrentGun && component.CurrentGun == this.m_gun) || component.HasActiveBonusSynergy(CustomSynergyType.WHALE_OF_A_TIME, false)))
			{
				sourceSpecRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Remove(sourceSpecRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleSkullTrigger));
				tk2dSpriteAnimator component2 = this.m_extantSkull.GetComponent<tk2dSpriteAnimator>();
				component2.PlayAndDestroyObject("gonner_skull_pickup_vfx", null);
				this.m_extantSkull = null;
				if (component.characterIdentity == PlayableCharacters.Robot)
				{
					component.healthHaver.Armor = component.healthHaver.Armor + 1f;
					AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", base.gameObject);
					GameObject gameObject = BraveResources.Load<GameObject>("Global VFX/VFX_Healing_Sparkles_001", ".prefab");
					if (gameObject != null)
					{
						component.PlayEffectOnActor(gameObject, Vector3.zero, true, false, false);
					}
				}
				else
				{
					component.healthHaver.ApplyHealing(0.5f);
					AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", base.gameObject);
					GameObject gameObject2 = BraveResources.Load<GameObject>("Global VFX/VFX_Healing_Sparkles_001", ".prefab");
					if (gameObject2 != null)
					{
						component.PlayEffectOnActor(gameObject2, Vector3.zero, true, false, false);
					}
				}
			}
		}
	}

	// Token: 0x06007275 RID: 29301 RVA: 0x002D841C File Offset: 0x002D661C
	private void Update()
	{
		if (this.m_initialized && (!this.m_gun.CurrentOwner || this.m_gun.CurrentOwner.CurrentGun != this.m_gun))
		{
			this.Disengage();
		}
		else if (!this.m_initialized && this.m_gun.CurrentOwner && this.m_gun.CurrentOwner.CurrentGun == this.m_gun)
		{
			this.Engage();
		}
	}

	// Token: 0x06007276 RID: 29302 RVA: 0x002D84BC File Offset: 0x002D66BC
	private void OnDestroy()
	{
		this.Disengage();
	}

	// Token: 0x06007277 RID: 29303 RVA: 0x002D84C4 File Offset: 0x002D66C4
	private void Engage()
	{
		this.m_initialized = true;
		this.m_player = this.m_gun.CurrentOwner as PlayerController;
		this.m_player.OnReceivedDamage += this.HandleReceivedDamage;
	}

	// Token: 0x06007278 RID: 29304 RVA: 0x002D84FC File Offset: 0x002D66FC
	private void Disengage()
	{
		if (this.m_player)
		{
			this.m_player.OnReceivedDamage -= this.HandleReceivedDamage;
		}
		if (this.m_extantSkull)
		{
			LootEngine.DoDefaultPurplePoof(this.m_extantSkull.transform.position + new Vector3(0.75f, 0.5f, 0f), false);
			UnityEngine.Object.Destroy(this.m_extantSkull.gameObject);
			this.m_extantSkull = null;
		}
		this.m_player = null;
		this.m_initialized = false;
	}

	// Token: 0x040073C8 RID: 29640
	public float ChanceToSkull = 1f;

	// Token: 0x040073C9 RID: 29641
	public GameObject SkullPrefab;

	// Token: 0x040073CA RID: 29642
	public float Lifespan = 4f;

	// Token: 0x040073CB RID: 29643
	public int AmmoLossOnDamage;

	// Token: 0x040073CC RID: 29644
	public float ChanceToTriggerSynergy = 0.25f;

	// Token: 0x040073CD RID: 29645
	private Gun m_gun;

	// Token: 0x040073CE RID: 29646
	private bool m_initialized;

	// Token: 0x040073CF RID: 29647
	private PlayerController m_player;

	// Token: 0x040073D0 RID: 29648
	private GameObject m_extantSkull;
}
