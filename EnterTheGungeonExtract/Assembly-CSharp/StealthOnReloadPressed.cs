using System;
using UnityEngine;

// Token: 0x020014C1 RID: 5313
public class StealthOnReloadPressed : MonoBehaviour
{
	// Token: 0x060078C3 RID: 30915 RVA: 0x003049C4 File Offset: 0x00302BC4
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		if (this.OnlyOnClipEmpty)
		{
			Gun gun = this.m_gun;
			gun.OnAutoReload = (Action<PlayerController, Gun>)Delegate.Combine(gun.OnAutoReload, new Action<PlayerController, Gun>(this.HandleReloadPressedSimple));
		}
		else
		{
			Gun gun2 = this.m_gun;
			gun2.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun2.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReloadPressed));
		}
	}

	// Token: 0x060078C4 RID: 30916 RVA: 0x00304A3C File Offset: 0x00302C3C
	private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		this.BreakStealth(this.m_gun.CurrentOwner as PlayerController);
	}

	// Token: 0x060078C5 RID: 30917 RVA: 0x00304A54 File Offset: 0x00302C54
	private void BreakStealth(PlayerController obj)
	{
		obj.PlayEffectOnActor(this.poofVfx, Vector3.zero, false, true, false);
		obj.ChangeSpecialShaderFlag(1, 0f);
		obj.OnDidUnstealthyAction -= this.BreakStealth;
		obj.healthHaver.OnDamaged -= this.OnDamaged;
		obj.SetIsStealthed(false, "box");
		obj.SetCapableOfStealing(false, "StealthOnReloadPressed", null);
	}

	// Token: 0x060078C6 RID: 30918 RVA: 0x00304ACC File Offset: 0x00302CCC
	private void HandleReloadPressedSimple(PlayerController user, Gun sourceGun)
	{
		this.HandleReloadPressed(user, sourceGun, false);
	}

	// Token: 0x060078C7 RID: 30919 RVA: 0x00304AD8 File Offset: 0x00302CD8
	private void HandleReloadPressed(PlayerController user, Gun sourceGun, bool actual)
	{
		if (this.SynergyContingent && !user.HasActiveBonusSynergy(this.RequiredSynergy, false))
		{
			return;
		}
		if (this.SynergyContingent)
		{
			sourceGun.CanSneakAttack = true;
			sourceGun.SneakAttackDamageMultiplier = 4f;
		}
		if (this.OnlyOnClipEmpty || !this.m_gun.IsFiring)
		{
			user.PlayEffectOnActor(this.poofVfx, Vector3.zero, false, true, false);
			user.ChangeSpecialShaderFlag(1, 1f);
			user.OnDidUnstealthyAction += this.BreakStealth;
			user.healthHaver.OnDamaged += this.OnDamaged;
			user.SetIsStealthed(true, "box");
			user.SetCapableOfStealing(true, "StealthOnReloadPressed", null);
		}
	}

	// Token: 0x04007B0A RID: 31498
	public GameObject poofVfx;

	// Token: 0x04007B0B RID: 31499
	public bool OnlyOnClipEmpty = true;

	// Token: 0x04007B0C RID: 31500
	[Header("Synergues")]
	public bool SynergyContingent;

	// Token: 0x04007B0D RID: 31501
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04007B0E RID: 31502
	private Gun m_gun;
}
