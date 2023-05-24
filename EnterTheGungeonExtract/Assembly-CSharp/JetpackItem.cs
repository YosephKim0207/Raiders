using System;
using UnityEngine;

// Token: 0x02001426 RID: 5158
public class JetpackItem : PlayerItem
{
	// Token: 0x0600750D RID: 29965 RVA: 0x002E9900 File Offset: 0x002E7B00
	protected override void DoEffect(PlayerController user)
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			return;
		}
		this.PreventCooldownBar = true;
		AkSoundEngine.PostEvent("Play_OBJ_jetpack_start_01", base.gameObject);
		base.IsCurrentlyActive = true;
		user.SetIsFlying(true, "jetpack", true, false);
		this.instanceJetpack = user.RegisterAttachedObject(this.prefabToAttachToPlayer, "jetpack", 0f);
		this.instanceJetpackSprite = this.instanceJetpack.GetComponent<tk2dSprite>();
	}

	// Token: 0x0600750E RID: 29966 RVA: 0x002E9978 File Offset: 0x002E7B78
	protected override void DoActiveEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Stop_OBJ_jetpack_loop_01", base.gameObject);
		base.IsCurrentlyActive = false;
		user.SetIsFlying(false, "jetpack", true, false);
		user.DeregisterAttachedObject(this.instanceJetpack, true);
		this.instanceJetpackSprite = null;
		user.stats.RecalculateStats(user, false, false);
	}

	// Token: 0x0600750F RID: 29967 RVA: 0x002E99D0 File Offset: 0x002E7BD0
	public override void Update()
	{
		base.Update();
		if (base.IsCurrentlyActive)
		{
			DeadlyDeadlyGoopManager.IgniteGoopsCircle(this.instanceJetpackSprite.WorldBottomCenter, 0.5f);
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
			{
				this.DoActiveEffect(this.LastOwner);
			}
		}
	}

	// Token: 0x06007510 RID: 29968 RVA: 0x002E9A20 File Offset: 0x002E7C20
	protected override void OnPreDrop(PlayerController user)
	{
		if (base.IsCurrentlyActive)
		{
			this.DoActiveEffect(user);
		}
	}

	// Token: 0x06007511 RID: 29969 RVA: 0x002E9A34 File Offset: 0x002E7C34
	public override void OnItemSwitched(PlayerController user)
	{
		if (base.IsCurrentlyActive)
		{
			this.DoActiveEffect(user);
		}
	}

	// Token: 0x06007512 RID: 29970 RVA: 0x002E9A48 File Offset: 0x002E7C48
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040076E3 RID: 30435
	public GameObject prefabToAttachToPlayer;

	// Token: 0x040076E4 RID: 30436
	private GameObject instanceJetpack;

	// Token: 0x040076E5 RID: 30437
	private tk2dSprite instanceJetpackSprite;
}
