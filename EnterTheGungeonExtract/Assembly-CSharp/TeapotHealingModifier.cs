using System;
using UnityEngine;

// Token: 0x020013E9 RID: 5097
public class TeapotHealingModifier : MonoBehaviour
{
	// Token: 0x060073A6 RID: 29606 RVA: 0x002E0044 File Offset: 0x002DE244
	private void Awake()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		this.m_projectile.allowSelfShooting = true;
		this.m_projectile.collidesWithEnemies = true;
		this.m_projectile.collidesWithPlayer = true;
		this.m_projectile.UpdateCollisionMask();
		SpeculativeRigidbody specRigidbody = this.m_projectile.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreCollision));
	}

	// Token: 0x060073A7 RID: 29607 RVA: 0x002E00B8 File Offset: 0x002DE2B8
	private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (otherRigidbody)
		{
			PlayerController component = otherRigidbody.GetComponent<PlayerController>();
			if (component && component != this.m_projectile.Owner && !component.IsGhost)
			{
				if (this.m_projectile.PossibleSourceGun)
				{
					component.healthHaver.ApplyHealing(0.5f);
					AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", base.gameObject);
					GameObject gameObject = BraveResources.Load<GameObject>("Global VFX/VFX_Healing_Sparkles_001", ".prefab");
					if (gameObject != null)
					{
						component.PlayEffectOnActor(gameObject, Vector3.zero, true, false, false);
					}
					this.m_projectile.PossibleSourceGun.LoseAmmo(this.AmmoCost);
					this.m_projectile.DieInAir(false, true, true, false);
				}
				PhysicsEngine.SkipCollision = true;
			}
			else if (component)
			{
				PhysicsEngine.SkipCollision = true;
			}
		}
	}

	// Token: 0x0400753C RID: 30012
	public int AmmoCost = 24;

	// Token: 0x0400753D RID: 30013
	private Projectile m_projectile;
}
