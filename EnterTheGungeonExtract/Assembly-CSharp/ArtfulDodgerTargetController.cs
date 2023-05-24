using System;
using System.Collections;
using UnityEngine;

// Token: 0x020010F8 RID: 4344
public class ArtfulDodgerTargetController : DungeonPlaceableBehaviour
{
	// Token: 0x06005FC0 RID: 24512 RVA: 0x0024E064 File Offset: 0x0024C264
	private void Start()
	{
		this.m_artfulDodger = base.GetAbsoluteParentRoom().GetComponentsAbsoluteInRoom<ArtfulDodgerRoomController>()[0];
		this.m_artfulDodger.RegisterTarget(this);
		base.sprite = base.GetComponentInChildren<tk2dSprite>();
		base.specRigidbody.enabled = false;
		base.sprite.renderer.enabled = false;
		this.ShadowRenderer.enabled = false;
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
	}

	// Token: 0x06005FC1 RID: 24513 RVA: 0x0024E0F0 File Offset: 0x0024C2F0
	public void Activate()
	{
		base.StartCoroutine(this.HandleActivation());
	}

	// Token: 0x06005FC2 RID: 24514 RVA: 0x0024E100 File Offset: 0x0024C300
	private IEnumerator HandleActivation()
	{
		base.specRigidbody.enabled = true;
		yield return new WaitForSeconds(0.75f);
		PathMover m_pathMover = base.GetComponent<PathMover>();
		if (m_pathMover && m_pathMover.Path != null)
		{
			m_pathMover.Paused = false;
		}
		LootEngine.DoDefaultItemPoof(base.sprite.WorldCenter, false, false);
		base.sprite.renderer.enabled = true;
		this.ShadowRenderer.enabled = true;
		this.Sparkles.SetActive(true);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
		yield break;
	}

	// Token: 0x06005FC3 RID: 24515 RVA: 0x0024E11C File Offset: 0x0024C31C
	public void ExplodeJoyously()
	{
		if (!this.IsBroken)
		{
			if (this.HitVFX)
			{
				SpawnManager.SpawnVFX(this.HitVFX, base.sprite.WorldCenter, Quaternion.identity);
			}
			this.IsBroken = true;
			base.specRigidbody.enabled = false;
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
			base.sprite.renderer.enabled = false;
			this.ShadowRenderer.enabled = false;
			this.Sparkles.SetActive(false);
		}
	}

	// Token: 0x06005FC4 RID: 24516 RVA: 0x0024E1B0 File Offset: 0x0024C3B0
	public void DisappearSadly()
	{
		if (!this.IsBroken)
		{
			LootEngine.DoDefaultItemPoof(base.sprite.WorldCenter, false, false);
			this.IsBroken = true;
			base.specRigidbody.enabled = false;
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
			base.sprite.renderer.enabled = false;
			this.ShadowRenderer.enabled = false;
			this.Sparkles.SetActive(false);
		}
	}

	// Token: 0x06005FC5 RID: 24517 RVA: 0x0024E224 File Offset: 0x0024C424
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (!this.IsBroken && rigidbodyCollision.OtherRigidbody.projectile != null)
		{
			Projectile projectile = rigidbodyCollision.OtherRigidbody.projectile;
			if (projectile.name.StartsWith("ArtfulDodger") || (projectile.PossibleSourceGun && projectile.PossibleSourceGun.name.StartsWith("ArtfulDodger")))
			{
				ArtfulDodgerProjectileController component = projectile.GetComponent<ArtfulDodgerProjectileController>();
				if (component != null)
				{
					component.hitTarget = true;
				}
				this.ExplodeJoyously();
				PierceProjModifier component2 = projectile.GetComponent<PierceProjModifier>();
				if (component2 == null)
				{
					projectile.DieInAir(false, true, true, false);
				}
			}
		}
	}

	// Token: 0x06005FC6 RID: 24518 RVA: 0x0024E2DC File Offset: 0x0024C4DC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005A43 RID: 23107
	public GameObject HitVFX;

	// Token: 0x04005A44 RID: 23108
	public Renderer ShadowRenderer;

	// Token: 0x04005A45 RID: 23109
	[NonSerialized]
	public bool IsBroken;

	// Token: 0x04005A46 RID: 23110
	public GameObject Sparkles;

	// Token: 0x04005A47 RID: 23111
	private ArtfulDodgerRoomController m_artfulDodger;
}
