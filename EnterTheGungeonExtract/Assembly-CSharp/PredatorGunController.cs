using System;
using UnityEngine;

// Token: 0x0200146B RID: 5227
public class PredatorGunController : MonoBehaviour
{
	// Token: 0x060076DA RID: 30426 RVA: 0x002F5FB0 File Offset: 0x002F41B0
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(gun.PostProcessProjectile, new Action<Projectile>(this.PostProcessProjectile));
	}

	// Token: 0x060076DB RID: 30427 RVA: 0x002F5FE8 File Offset: 0x002F41E8
	private void Update()
	{
		if (this.m_gun.CurrentOwner && this.m_gun.CurrentOwner is PlayerController)
		{
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			if (playerController.CurrentRoom != null)
			{
				float num = -1f;
				AIActor nearestEnemy = playerController.CurrentRoom.GetNearestEnemy(playerController.unadjustedAimPoint.XY(), out num, true, true);
				if (nearestEnemy)
				{
					this.ProcessNearestEnemy(nearestEnemy);
				}
				else
				{
					this.ProcessNearestEnemy(null);
				}
			}
		}
	}

	// Token: 0x060076DC RID: 30428 RVA: 0x002F607C File Offset: 0x002F427C
	private void ProcessNearestEnemy(AIActor hitEnemy)
	{
		if (hitEnemy)
		{
			if (this.m_lastLockOnEnemy != hitEnemy)
			{
				if (this.m_extantLockOnSprite)
				{
					SpawnManager.Despawn(this.m_extantLockOnSprite);
				}
				this.m_extantLockOnSprite = hitEnemy.PlayEffectOnActor((GameObject)BraveResources.Load("Global VFX/VFX_LockOn_Predator", ".prefab"), Vector3.zero, true, true, true);
				this.m_lastLockOnEnemy = hitEnemy;
			}
		}
		else if (this.m_extantLockOnSprite)
		{
			SpawnManager.Despawn(this.m_extantLockOnSprite);
		}
	}

	// Token: 0x060076DD RID: 30429 RVA: 0x002F6114 File Offset: 0x002F4314
	private void PostProcessProjectile(Projectile p)
	{
		if (this.m_lastLockOnEnemy)
		{
			LockOnHomingModifier lockOnHomingModifier = p.GetComponent<LockOnHomingModifier>();
			if (!lockOnHomingModifier)
			{
				lockOnHomingModifier = p.gameObject.AddComponent<LockOnHomingModifier>();
				lockOnHomingModifier.HomingRadius = 0f;
				lockOnHomingModifier.AngularVelocity = 0f;
			}
			lockOnHomingModifier.HomingRadius += this.HomingRadius;
			lockOnHomingModifier.AngularVelocity += this.HomingAngularVelocity;
			lockOnHomingModifier.LockOnVFX = this.LockOnVFX;
			lockOnHomingModifier.AssignTargetManually(this.m_lastLockOnEnemy);
		}
	}

	// Token: 0x040078CA RID: 30922
	public float HomingRadius = 5f;

	// Token: 0x040078CB RID: 30923
	public float HomingAngularVelocity = 360f;

	// Token: 0x040078CC RID: 30924
	public GameObject LockOnVFX;

	// Token: 0x040078CD RID: 30925
	private AIActor m_lastLockOnEnemy;

	// Token: 0x040078CE RID: 30926
	private GameObject m_extantLockOnSprite;

	// Token: 0x040078CF RID: 30927
	private Gun m_gun;
}
