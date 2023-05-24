using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001727 RID: 5927
public class ProjectileTrapController : BasicTrapController
{
	// Token: 0x060089A5 RID: 35237 RVA: 0x00394208 File Offset: 0x00392408
	public override void Start()
	{
		base.Start();
		StaticReferenceManager.AllProjectileTraps.Add(this);
	}

	// Token: 0x060089A6 RID: 35238 RVA: 0x0039421C File Offset: 0x0039241C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		StaticReferenceManager.AllProjectileTraps.Remove(this);
	}

	// Token: 0x060089A7 RID: 35239 RVA: 0x00394230 File Offset: 0x00392430
	public override GameObject InstantiateObject(RoomHandler targetRoom, IntVector2 loc, bool deferConfiguration)
	{
		return base.InstantiateObject(targetRoom, loc, deferConfiguration);
	}

	// Token: 0x060089A8 RID: 35240 RVA: 0x0039423C File Offset: 0x0039243C
	protected override void TriggerTrap(SpeculativeRigidbody target)
	{
		base.TriggerTrap(target);
		if (this.projectileModule.shootStyle == ProjectileModule.ShootStyle.Beam)
		{
			Debug.LogWarning("Unsupported shootstyle Beam.");
		}
		else
		{
			Vector2 vector = DungeonData.GetIntVector2FromDirection(this.shootDirection).ToVector2();
			this.ShootProjectileInDirection(this.shootPoint.position, vector);
			this.shootVfx.SpawnAtLocalPosition(Vector3.zero, vector.ToAngle(), this.shootPoint, null, null, false, null, false);
		}
	}

	// Token: 0x060089A9 RID: 35241 RVA: 0x003942C8 File Offset: 0x003924C8
	private void ShootProjectileInDirection(Vector3 spawnPosition, Vector2 direction)
	{
		AkSoundEngine.PostEvent("Play_TRP_bullet_shot_01", base.gameObject);
		float num = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
		GameObject gameObject = SpawnManager.SpawnProjectile(this.projectileModule.GetCurrentProjectile().gameObject, spawnPosition, Quaternion.Euler(0f, 0f, num), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		if (this.overrideProjectileData != null)
		{
			component.baseData.SetAll(this.overrideProjectileData);
		}
		component.Shooter = base.specRigidbody;
		component.TrapOwner = this;
	}

	// Token: 0x04009004 RID: 36868
	public ProjectileModule projectileModule;

	// Token: 0x04009005 RID: 36869
	public ProjectileData overrideProjectileData;

	// Token: 0x04009006 RID: 36870
	public DungeonData.Direction shootDirection;

	// Token: 0x04009007 RID: 36871
	public VFXPool shootVfx;

	// Token: 0x04009008 RID: 36872
	public Transform shootPoint;
}
