using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001396 RID: 5014
public class Decoy : SpawnObjectItem
{
	// Token: 0x06007191 RID: 29073 RVA: 0x002D1DD8 File Offset: 0x002CFFD8
	private IEnumerator Start()
	{
		RoomHandler room = GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			base.specRigidbody.RegisterSpecificCollisionException(GameManager.Instance.AllPlayers[i].specRigidbody);
		}
		List<BaseShopController> allShops = StaticReferenceManager.AllShops;
		for (int j = 0; j < allShops.Count; j++)
		{
			if (allShops[j] && allShops[j].GetAbsoluteParentRoom() == room)
			{
				allShops[j].SetCapableOfBeingStolenFrom(true, "Decoy", null);
			}
		}
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreRigidbodyCollision));
		MajorBreakable component = base.GetComponent<MajorBreakable>();
		component.OnBreak = (Action)Delegate.Combine(component.OnBreak, new Action(this.OnBreak));
		if (!string.IsNullOrEmpty(this.GoopSynergySprite) && this.HasGoopSynergy && this.SpawningPlayer && this.SpawningPlayer.HasActiveBonusSynergy(this.GoopSynergy, false))
		{
			base.sprite.SetSprite(this.GoopSynergySprite);
		}
		if (!string.IsNullOrEmpty(this.FreezeAttackersSprite) && this.HasFreezeAttackersSynergy && this.SpawningPlayer && this.SpawningPlayer.HasActiveBonusSynergy(this.FreezeAttackersSynergy, false))
		{
			base.sprite.SetSprite(this.FreezeAttackersSprite);
		}
		while (!this.m_revealed)
		{
			this.AttractEnemies(room);
			yield return new WaitForSeconds(1f);
			if (this.DeathExplosionTimer >= 0f)
			{
				this.DeathExplosionTimer -= 1f;
				if (this.DeathExplosionTimer < 0f)
				{
					this.OnBreak();
				}
			}
		}
		this.ClearOverrides(room);
		yield break;
	}

	// Token: 0x06007192 RID: 29074 RVA: 0x002D1DF4 File Offset: 0x002CFFF4
	private void HandlePreRigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (this.m_revealed)
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
		if (this.HasFreezeAttackersSynergy && this.SpawningPlayer && this.SpawningPlayer.HasActiveBonusSynergy(this.FreezeAttackersSynergy, false) && otherRigidbody && otherRigidbody.projectile)
		{
			Projectile projectile = otherRigidbody.projectile;
			if (projectile.Owner is AIActor)
			{
				AIActor aiactor = projectile.Owner as AIActor;
				aiactor.ApplyEffect(this.FreezeSynergyEffect, 1f, null);
			}
			else if (projectile.Shooter && projectile.Shooter.aiActor)
			{
				AIActor aiActor = projectile.Shooter.aiActor;
				aiActor.ApplyEffect(this.FreezeSynergyEffect, 1f, null);
			}
		}
		if (this.HasDecoyOctopusSynergy && this.SpawningPlayer && this.SpawningPlayer.HasActiveBonusSynergy(CustomSynergyType.DECOY_OCTOPUS, false) && otherRigidbody && otherRigidbody.projectile)
		{
			Projectile projectile2 = otherRigidbody.projectile;
			string text = string.Empty;
			if (projectile2.Owner is AIActor)
			{
				AIActor aiactor2 = projectile2.Owner as AIActor;
				if (aiactor2.IsNormalEnemy && aiactor2.healthHaver && !aiactor2.healthHaver.IsBoss)
				{
					text = aiactor2.EnemyGuid;
				}
			}
			else if (projectile2.Shooter && projectile2.Shooter.aiActor)
			{
				AIActor aiActor2 = projectile2.Shooter.aiActor;
				if (aiActor2.IsNormalEnemy && aiActor2.healthHaver && !aiActor2.healthHaver.IsBoss)
				{
					text = aiActor2.EnemyGuid;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.OnBreak();
				AIActor aiactor3 = AIActor.Spawn(EnemyDatabase.GetOrLoadByGuid(text), base.transform.position.IntXY(VectorConversions.Floor), base.transform.position.GetAbsoluteRoom(), true, AIActor.AwakenAnimationType.Default, true);
				aiactor3.ApplyEffect(this.PermanentCharmEffect, 1f, null);
				projectile2.DieInAir(false, true, true, false);
				PhysicsEngine.SkipCollision = true;
			}
		}
	}

	// Token: 0x06007193 RID: 29075 RVA: 0x002D2060 File Offset: 0x002D0260
	private void OnBreak()
	{
		if (!this.m_revealed)
		{
			this.m_revealed = true;
			if (this.revealVFX != null)
			{
				this.revealVFX.SetActive(true);
			}
			if (this.ExplodesOnDeath)
			{
				if (this.DeathExplosion.damageToPlayer > 0f)
				{
					this.DeathExplosion.damageToPlayer = 0f;
				}
				Exploder.Explode(base.specRigidbody.UnitCenter, this.DeathExplosion, Vector2.zero, null, false, CoreDamageTypes.None, false);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				base.spriteAnimator.PlayAndDestroyObject(this.revealAnimationName, null);
			}
		}
		List<BaseShopController> allShops = StaticReferenceManager.AllShops;
		RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
		for (int i = 0; i < allShops.Count; i++)
		{
			if (allShops[i] && allShops[i].GetAbsoluteParentRoom() == roomFromPosition)
			{
				allShops[i].SetCapableOfBeingStolenFrom(false, "Decoy", null);
			}
		}
		if (this.HasGoopSynergy && this.SpawningPlayer && this.SpawningPlayer.HasActiveBonusSynergy(this.GoopSynergy, false))
		{
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.GoopSynergyGoop);
			goopManagerForGoopType.TimedAddGoopCircle(base.specRigidbody.UnitCenter, this.GoopSynergyRadius, 1f, false);
		}
	}

	// Token: 0x06007194 RID: 29076 RVA: 0x002D21EC File Offset: 0x002D03EC
	private void ClearOverrides(RoomHandler room)
	{
		List<AIActor> activeEnemies = room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (activeEnemies != null)
		{
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				if (activeEnemies[i].OverrideTarget == base.specRigidbody)
				{
					activeEnemies[i].OverrideTarget = null;
				}
			}
		}
	}

	// Token: 0x06007195 RID: 29077 RVA: 0x002D2248 File Offset: 0x002D0448
	private void AttractEnemies(RoomHandler room)
	{
		List<AIActor> activeEnemies = room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (activeEnemies != null)
		{
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				if (activeEnemies[i].OverrideTarget == null)
				{
					activeEnemies[i].OverrideTarget = base.specRigidbody;
				}
			}
		}
	}

	// Token: 0x06007196 RID: 29078 RVA: 0x002D22A4 File Offset: 0x002D04A4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040072EF RID: 29423
	public string revealAnimationName;

	// Token: 0x040072F0 RID: 29424
	public GameObject revealVFX;

	// Token: 0x040072F1 RID: 29425
	public bool ExplodesOnDeath;

	// Token: 0x040072F2 RID: 29426
	public float DeathExplosionTimer = -1f;

	// Token: 0x040072F3 RID: 29427
	public ExplosionData DeathExplosion;

	// Token: 0x040072F4 RID: 29428
	public bool AllowStealing = true;

	// Token: 0x040072F5 RID: 29429
	[Header("Synergues")]
	public bool HasGoopSynergy;

	// Token: 0x040072F6 RID: 29430
	public CustomSynergyType GoopSynergy;

	// Token: 0x040072F7 RID: 29431
	public GoopDefinition GoopSynergyGoop;

	// Token: 0x040072F8 RID: 29432
	public float GoopSynergyRadius;

	// Token: 0x040072F9 RID: 29433
	public string GoopSynergySprite;

	// Token: 0x040072FA RID: 29434
	public bool HasFreezeAttackersSynergy;

	// Token: 0x040072FB RID: 29435
	public CustomSynergyType FreezeAttackersSynergy;

	// Token: 0x040072FC RID: 29436
	public GameActorFreezeEffect FreezeSynergyEffect;

	// Token: 0x040072FD RID: 29437
	public string FreezeAttackersSprite;

	// Token: 0x040072FE RID: 29438
	public bool HasDecoyOctopusSynergy;

	// Token: 0x040072FF RID: 29439
	public GameActorCharmEffect PermanentCharmEffect;

	// Token: 0x04007300 RID: 29440
	private bool m_revealed;
}
