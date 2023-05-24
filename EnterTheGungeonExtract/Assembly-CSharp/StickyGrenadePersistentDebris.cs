using System;
using UnityEngine;

// Token: 0x02000E32 RID: 3634
public class StickyGrenadePersistentDebris : BraveBehaviour
{
	// Token: 0x06004CE2 RID: 19682 RVA: 0x001A4B84 File Offset: 0x001A2D84
	public void InitializeSelf(StickyGrenadeBuff source)
	{
		this.explosionData = source.explosionData;
		Projectile component = source.GetComponent<Projectile>();
		if (component.PossibleSourceGun != null)
		{
			this.m_attachedGun = component.PossibleSourceGun;
			this.m_player = component.PossibleSourceGun.CurrentOwner as PlayerController;
			Gun possibleSourceGun = component.PossibleSourceGun;
			possibleSourceGun.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(possibleSourceGun.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.ExplodeOnReload));
			if (this.m_player)
			{
				this.m_player.GunChanged += this.GunChanged;
			}
		}
		else if (component && component.Owner && component.Owner.CurrentGun)
		{
			this.m_attachedGun = component.Owner.CurrentGun;
			this.m_player = component.Owner as PlayerController;
			Gun currentGun = component.Owner.CurrentGun;
			currentGun.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(currentGun.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.ExplodeOnReload));
			if (this.m_player)
			{
				this.m_player.GunChanged += this.GunChanged;
			}
		}
	}

	// Token: 0x06004CE3 RID: 19683 RVA: 0x001A4CD0 File Offset: 0x001A2ED0
	private void Disconnect()
	{
		if (this.m_player)
		{
			this.m_player.GunChanged -= this.GunChanged;
		}
		if (this.m_attachedGun)
		{
			Gun attachedGun = this.m_attachedGun;
			attachedGun.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Remove(attachedGun.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.ExplodeOnReload));
		}
	}

	// Token: 0x06004CE4 RID: 19684 RVA: 0x001A4D3C File Offset: 0x001A2F3C
	private void GunChanged(Gun arg1, Gun arg2, bool newGun)
	{
		this.Disconnect();
		this.DoEffect();
	}

	// Token: 0x06004CE5 RID: 19685 RVA: 0x001A4D4C File Offset: 0x001A2F4C
	private void ExplodeOnReload(PlayerController arg1, Gun arg2, bool actual)
	{
		this.Disconnect();
		this.DoEffect();
	}

	// Token: 0x06004CE6 RID: 19686 RVA: 0x001A4D5C File Offset: 0x001A2F5C
	private void DoEffect()
	{
		this.explosionData.force = 0f;
		if (base.sprite)
		{
			Exploder.Explode(base.sprite.WorldCenter, this.explosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);
		}
		else
		{
			Exploder.Explode(base.transform.position.XY(), this.explosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004CE7 RID: 19687 RVA: 0x001A4DE8 File Offset: 0x001A2FE8
	protected override void OnDestroy()
	{
		this.Disconnect();
		base.OnDestroy();
	}

	// Token: 0x040042F7 RID: 17143
	public ExplosionData explosionData;

	// Token: 0x040042F8 RID: 17144
	private PlayerController m_player;

	// Token: 0x040042F9 RID: 17145
	private Gun m_attachedGun;
}
