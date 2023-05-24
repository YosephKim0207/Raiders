using System;

// Token: 0x0200143F RID: 5183
public class MonsterBallItem : PlayerItem
{
	// Token: 0x060075A2 RID: 30114 RVA: 0x002ED938 File Offset: 0x002EBB38
	private void Awake()
	{
		this.m_idleSpriteId = base.sprite.spriteId;
	}

	// Token: 0x060075A3 RID: 30115 RVA: 0x002ED94C File Offset: 0x002EBB4C
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		base.sprite.SetSprite(this.m_idleSpriteId);
		if (this.m_containsEnemy)
		{
			base.IsCurrentlyActive = true;
			base.ClearCooldowns();
		}
	}

	// Token: 0x060075A4 RID: 30116 RVA: 0x002ED980 File Offset: 0x002EBB80
	protected override void DoEffect(PlayerController user)
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			return;
		}
		DebrisObject debrisObject = user.DropActiveItem(this, 10f, false);
		if (debrisObject)
		{
			MonsterBallItem component = debrisObject.GetComponent<MonsterBallItem>();
			component.spriteAnimator.Play("monster_ball_throw");
			component.m_containsEnemy = this.m_containsEnemy;
			component.m_storedEnemyGuid = this.m_storedEnemyGuid;
			DebrisObject debrisObject2 = debrisObject;
			debrisObject2.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject2.OnGrounded, new Action<DebrisObject>(this.HandleTossedBallGrounded));
		}
	}

	// Token: 0x060075A5 RID: 30117 RVA: 0x002EDA08 File Offset: 0x002EBC08
	private void HandleTossedBallGrounded(DebrisObject obj)
	{
		obj.OnGrounded = (Action<DebrisObject>)Delegate.Remove(obj.OnGrounded, new Action<DebrisObject>(this.HandleTossedBallGrounded));
		MonsterBallItem component = obj.GetComponent<MonsterBallItem>();
		component.spriteAnimator.Play("monster_ball_open");
		float num = -1f;
		AIActor nearestEnemy = obj.transform.position.GetAbsoluteRoom().GetNearestEnemy(obj.sprite.WorldCenter, out num, false, false);
		if (nearestEnemy && num < 10f)
		{
			component.m_containsEnemy = true;
			component.m_storedEnemyGuid = nearestEnemy.EnemyGuid;
			LootEngine.DoDefaultItemPoof(nearestEnemy.CenterPosition, false, false);
			nearestEnemy.EraseFromExistence(false);
		}
	}

	// Token: 0x060075A6 RID: 30118 RVA: 0x002EDAB8 File Offset: 0x002EBCB8
	protected override void DoActiveEffect(PlayerController user)
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			return;
		}
		DebrisObject debrisObject = user.DropActiveItem(this, 10f, false);
		if (debrisObject)
		{
			MonsterBallItem component = debrisObject.GetComponent<MonsterBallItem>();
			component.spriteAnimator.Play("monster_ball_throw");
			component.m_containsEnemy = this.m_containsEnemy;
			component.m_storedEnemyGuid = this.m_storedEnemyGuid;
			DebrisObject debrisObject2 = debrisObject;
			debrisObject2.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject2.OnGrounded, new Action<DebrisObject>(this.HandleActiveTossedBallGrounded));
		}
	}

	// Token: 0x060075A7 RID: 30119 RVA: 0x002EDB40 File Offset: 0x002EBD40
	private void HandleActiveTossedBallGrounded(DebrisObject obj)
	{
		obj.OnGrounded = (Action<DebrisObject>)Delegate.Remove(obj.OnGrounded, new Action<DebrisObject>(this.HandleActiveTossedBallGrounded));
		MonsterBallItem component = obj.GetComponent<MonsterBallItem>();
		component.spriteAnimator.Play("monster_ball_open");
		AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.m_storedEnemyGuid);
		IntVector2 bestRewardLocation = obj.transform.position.GetAbsoluteRoom().GetBestRewardLocation(orLoadByGuid.Clearance, obj.sprite.WorldCenter, true);
		AIActor aiactor = AIActor.Spawn(orLoadByGuid, bestRewardLocation, obj.transform.position.GetAbsoluteRoom(), true, AIActor.AwakenAnimationType.Default, true);
		aiactor.ApplyEffect(this.CharmEffect, 1f, null);
		component.m_containsEnemy = false;
		component.m_storedEnemyGuid = string.Empty;
		component.IsCurrentlyActive = false;
		component.ApplyCooldown(this.LastOwner);
	}

	// Token: 0x060075A8 RID: 30120 RVA: 0x002EDC0C File Offset: 0x002EBE0C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400776D RID: 30573
	public GameActorCharmEffect CharmEffect;

	// Token: 0x0400776E RID: 30574
	private bool m_containsEnemy;

	// Token: 0x0400776F RID: 30575
	private string m_storedEnemyGuid;

	// Token: 0x04007770 RID: 30576
	private int m_idleSpriteId = -1;
}
