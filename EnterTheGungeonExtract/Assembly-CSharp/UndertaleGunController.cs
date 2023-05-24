using System;
using UnityEngine;

// Token: 0x020014DF RID: 5343
public class UndertaleGunController : MonoBehaviour
{
	// Token: 0x06007982 RID: 31106 RVA: 0x00309CD8 File Offset: 0x00307ED8
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x06007983 RID: 31107 RVA: 0x00309CE8 File Offset: 0x00307EE8
	private void Update()
	{
		if (!this.m_initialized && this.m_gun.CurrentOwner)
		{
			this.Initialize();
		}
		else if (this.m_initialized && !this.m_gun.CurrentOwner)
		{
			this.Deinitialize();
		}
	}

	// Token: 0x06007984 RID: 31108 RVA: 0x00309D48 File Offset: 0x00307F48
	private void Initialize()
	{
		if (this.m_initialized)
		{
			return;
		}
		this.m_player = this.m_gun.CurrentOwner as PlayerController;
		this.m_player.OnDodgedProjectile += this.HandleDodgedProjectile;
		this.m_initialized = true;
	}

	// Token: 0x06007985 RID: 31109 RVA: 0x00309D98 File Offset: 0x00307F98
	private void Deinitialize()
	{
		if (!this.m_initialized)
		{
			return;
		}
		this.m_player.OnDodgedProjectile -= this.HandleDodgedProjectile;
		this.m_player = null;
		this.m_initialized = false;
	}

	// Token: 0x06007986 RID: 31110 RVA: 0x00309DCC File Offset: 0x00307FCC
	private void HandleDodgedProjectile(Projectile dodgedProjectile)
	{
		if (dodgedProjectile.Owner && dodgedProjectile.Owner is AIActor)
		{
			this.MakeEnemyNPC(dodgedProjectile.Owner as AIActor);
		}
	}

	// Token: 0x06007987 RID: 31111 RVA: 0x00309E00 File Offset: 0x00308000
	private void MakeEnemyNPC(AIActor enemy)
	{
		if (enemy.aiAnimator)
		{
			enemy.aiAnimator.PlayUntilCancelled("idle", false, null, -1f, false);
		}
		if (enemy.behaviorSpeculator)
		{
			UnityEngine.Object.Destroy(enemy.behaviorSpeculator);
		}
		if (enemy.aiActor)
		{
			UnityEngine.Object.Destroy(enemy.aiActor);
		}
	}

	// Token: 0x04007BFC RID: 31740
	private Gun m_gun;

	// Token: 0x04007BFD RID: 31741
	private PlayerController m_player;

	// Token: 0x04007BFE RID: 31742
	private bool m_initialized;
}
