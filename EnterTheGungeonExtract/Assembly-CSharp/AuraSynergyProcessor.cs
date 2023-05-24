using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020016DB RID: 5851
public class AuraSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008819 RID: 34841 RVA: 0x00386BE4 File Offset: 0x00384DE4
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReload));
	}

	// Token: 0x0600881A RID: 34842 RVA: 0x00386C1C File Offset: 0x00384E1C
	private void HandleReload(PlayerController sourcePlayer, Gun arg2, bool arg3)
	{
		if (!sourcePlayer || !sourcePlayer.HasActiveBonusSynergy(this.RequiredSynergy, false))
		{
			return;
		}
		if (this.TriggeredOnReload)
		{
			base.StartCoroutine(this.HandleReloadTrigger());
		}
	}

	// Token: 0x0600881B RID: 34843 RVA: 0x00386C54 File Offset: 0x00384E54
	private IEnumerator HandleReloadTrigger()
	{
		float elapsed = 0f;
		while (this.m_gun && this.m_gun.IsReloading && (!this.HasOverrideDuration || elapsed < this.OverrideDuration))
		{
			if (!this.m_gun.enabled || !this.m_gun.CurrentOwner)
			{
				yield break;
			}
			if (Dungeon.IsGenerating)
			{
				yield break;
			}
			elapsed += BraveTime.DeltaTime;
			PlayerController playerOwner = this.m_gun.CurrentOwner as PlayerController;
			if (!playerOwner || playerOwner.CurrentRoom == null)
			{
				yield break;
			}
			playerOwner.CurrentRoom.ApplyActionToNearbyEnemies(playerOwner.CenterPosition, this.AuraRadius, new Action<AIActor, float>(this.ProcessEnemy));
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600881C RID: 34844 RVA: 0x00386C70 File Offset: 0x00384E70
	private void ProcessEnemy(AIActor enemy, float distance)
	{
		if (this.DoPoison)
		{
			enemy.ApplyEffect(this.PoisonEffect, 1f, null);
		}
		if (this.DoFreeze)
		{
			enemy.ApplyEffect(this.FreezeEffect, BraveTime.DeltaTime, null);
		}
		if (this.DoBurn)
		{
			enemy.ApplyEffect(this.FireEffect, 1f, null);
		}
		if (this.DoCharm)
		{
			enemy.ApplyEffect(this.CharmEffect, 1f, null);
		}
		if (this.DoSlow)
		{
			enemy.ApplyEffect(this.SpeedEffect, 1f, null);
		}
		if (this.DoStun && enemy.behaviorSpeculator)
		{
			if (enemy.behaviorSpeculator.IsStunned)
			{
				enemy.behaviorSpeculator.UpdateStun(this.StunDuration);
			}
			else
			{
				enemy.behaviorSpeculator.Stun(this.StunDuration, true);
			}
		}
	}

	// Token: 0x04008D57 RID: 36183
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008D58 RID: 36184
	public bool TriggeredOnReload;

	// Token: 0x04008D59 RID: 36185
	public float AuraRadius = 5f;

	// Token: 0x04008D5A RID: 36186
	public bool HasOverrideDuration;

	// Token: 0x04008D5B RID: 36187
	public float OverrideDuration = 0.05f;

	// Token: 0x04008D5C RID: 36188
	public bool DoPoison;

	// Token: 0x04008D5D RID: 36189
	public GameActorHealthEffect PoisonEffect;

	// Token: 0x04008D5E RID: 36190
	public bool DoFreeze;

	// Token: 0x04008D5F RID: 36191
	public GameActorFreezeEffect FreezeEffect;

	// Token: 0x04008D60 RID: 36192
	public bool DoBurn;

	// Token: 0x04008D61 RID: 36193
	public GameActorFireEffect FireEffect;

	// Token: 0x04008D62 RID: 36194
	public bool DoCharm;

	// Token: 0x04008D63 RID: 36195
	public GameActorCharmEffect CharmEffect;

	// Token: 0x04008D64 RID: 36196
	public bool DoSlow;

	// Token: 0x04008D65 RID: 36197
	public GameActorSpeedEffect SpeedEffect;

	// Token: 0x04008D66 RID: 36198
	public bool DoStun;

	// Token: 0x04008D67 RID: 36199
	public float StunDuration = 1f;

	// Token: 0x04008D68 RID: 36200
	private Gun m_gun;
}
