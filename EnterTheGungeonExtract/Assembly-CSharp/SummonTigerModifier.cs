using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001682 RID: 5762
public class SummonTigerModifier : BraveBehaviour
{
	// Token: 0x06008662 RID: 34402 RVA: 0x00379E90 File Offset: 0x00378090
	private void Start()
	{
		Projectile projectile = base.projectile;
		projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
		base.projectile.OnDestruction += this.HandleDestruction;
	}

	// Token: 0x06008663 RID: 34403 RVA: 0x00379ED0 File Offset: 0x003780D0
	private void HandleDestruction(Projectile source)
	{
		if (!this.m_hasSummonedTiger)
		{
			this.SummonTiger(null);
		}
	}

	// Token: 0x06008664 RID: 34404 RVA: 0x00379EE4 File Offset: 0x003780E4
	private void HandleHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
	{
		if (!this.m_hasSummonedTiger)
		{
			this.SummonTiger(arg2);
		}
	}

	// Token: 0x06008665 RID: 34405 RVA: 0x00379EF8 File Offset: 0x003780F8
	private void SummonTiger(SpeculativeRigidbody optionalTarget)
	{
		this.m_hasSummonedTiger = true;
		RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
		Vector2? vector = null;
		if (optionalTarget != null)
		{
			vector = new Vector2?(optionalTarget.UnitCenter);
		}
		IntVector2 intVector = new IntVector2(4, 2);
		if (base.sprite)
		{
			intVector = Vector2.Scale(new Vector2(4f, 2f), base.sprite.scale.XY()).ToIntVector2(VectorConversions.Ceil);
		}
		IntVector2? intVector2 = absoluteRoomFromPosition.GetOffscreenCell(new IntVector2?(intVector), new CellTypes?(CellTypes.FLOOR), false, vector);
		if (intVector2 == null)
		{
			intVector2 = absoluteRoomFromPosition.GetRandomAvailableCell(new IntVector2?(intVector), new CellTypes?(CellTypes.FLOOR), false, null);
		}
		if (intVector2 != null)
		{
			if (optionalTarget != null)
			{
				this.ShootSingleProjectile(intVector2.Value.ToVector2(), BraveMathCollege.Atan2Degrees(optionalTarget.UnitCenter - intVector2.Value.ToVector2()));
			}
			else
			{
				this.ShootSingleProjectile(intVector2.Value.ToVector2(), BraveMathCollege.Atan2Degrees(absoluteRoomFromPosition.GetCenterCell().ToVector2() - intVector2.Value.ToVector2()));
			}
		}
	}

	// Token: 0x06008666 RID: 34406 RVA: 0x0037A064 File Offset: 0x00378264
	private void ShootSingleProjectile(Vector2 spawnPosition, float angle)
	{
		GameObject gameObject = SpawnManager.SpawnProjectile(this.TigerProjectilePrefab.gameObject, spawnPosition.ToVector3ZUp(spawnPosition.y), Quaternion.Euler(0f, 0f, angle), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Owner = base.projectile.Owner;
		component.Shooter = component.Owner.specRigidbody;
		if (component.Owner is PlayerController)
		{
			PlayerStats stats = (component.Owner as PlayerController).stats;
			component.baseData.damage *= stats.GetStatValue(PlayerStats.StatType.Damage);
			component.baseData.speed *= stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
			component.baseData.force *= stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
			(component.Owner as PlayerController).DoPostProcessProjectile(component);
		}
	}

	// Token: 0x04008B5B RID: 35675
	public Projectile TigerProjectilePrefab;

	// Token: 0x04008B5C RID: 35676
	private bool m_hasSummonedTiger;
}
