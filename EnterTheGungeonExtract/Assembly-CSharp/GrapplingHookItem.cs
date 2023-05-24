using System;
using UnityEngine;

// Token: 0x0200140D RID: 5133
public class GrapplingHookItem : PlayerItem
{
	// Token: 0x06007476 RID: 29814 RVA: 0x002E5844 File Offset: 0x002E3A44
	private void Awake()
	{
		this.InitializeModule();
	}

	// Token: 0x06007477 RID: 29815 RVA: 0x002E584C File Offset: 0x002E3A4C
	private void InitializeModule()
	{
		this.m_grappleModule = new GrappleModule();
		this.m_grappleModule.GrapplePrefab = this.GrapplePrefab;
		this.m_grappleModule.GrappleSpeed = this.GrappleSpeed;
		this.m_grappleModule.GrappleRetractSpeed = this.GrappleRetractSpeed;
		this.m_grappleModule.DamageToEnemies = this.DamageToEnemies;
		this.m_grappleModule.EnemyKnockbackForce = this.EnemyKnockbackForce;
		this.m_grappleModule.sourceGameObject = base.gameObject;
		GrappleModule grappleModule = this.m_grappleModule;
		grappleModule.FinishedCallback = (Action)Delegate.Combine(grappleModule.FinishedCallback, new Action(this.GrappleEndedNaturally));
	}

	// Token: 0x06007478 RID: 29816 RVA: 0x002E58F4 File Offset: 0x002E3AF4
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_OBJ_hook_shot_01", base.gameObject);
		base.IsCurrentlyActive = true;
		this.m_grappleModule.Trigger(user);
	}

	// Token: 0x06007479 RID: 29817 RVA: 0x002E591C File Offset: 0x002E3B1C
	protected void GrappleEndedNaturally()
	{
		base.IsCurrentlyActive = false;
	}

	// Token: 0x0600747A RID: 29818 RVA: 0x002E5928 File Offset: 0x002E3B28
	protected override void DoActiveEffect(PlayerController user)
	{
		this.m_grappleModule.MarkDone();
	}

	// Token: 0x0600747B RID: 29819 RVA: 0x002E5938 File Offset: 0x002E3B38
	protected override void OnPreDrop(PlayerController user)
	{
		this.m_grappleModule.ClearExtantGrapple();
		base.IsCurrentlyActive = false;
	}

	// Token: 0x0600747C RID: 29820 RVA: 0x002E594C File Offset: 0x002E3B4C
	public override void OnItemSwitched(PlayerController user)
	{
		this.m_grappleModule.ForceEndGrapple();
		this.m_grappleModule.ClearExtantGrapple();
		base.IsCurrentlyActive = false;
	}

	// Token: 0x0600747D RID: 29821 RVA: 0x002E596C File Offset: 0x002E3B6C
	protected override void OnDestroy()
	{
		this.m_grappleModule.ClearExtantGrapple();
		base.OnDestroy();
	}

	// Token: 0x04007641 RID: 30273
	public GameObject GrapplePrefab;

	// Token: 0x04007642 RID: 30274
	public float GrappleSpeed = 10f;

	// Token: 0x04007643 RID: 30275
	public float GrappleRetractSpeed = 10f;

	// Token: 0x04007644 RID: 30276
	public float DamageToEnemies = 10f;

	// Token: 0x04007645 RID: 30277
	public float EnemyKnockbackForce = 10f;

	// Token: 0x04007646 RID: 30278
	private GrappleModule m_grappleModule;
}
