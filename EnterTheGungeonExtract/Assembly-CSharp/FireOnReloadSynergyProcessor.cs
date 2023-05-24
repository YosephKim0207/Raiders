using System;
using UnityEngine;

// Token: 0x020016E5 RID: 5861
public class FireOnReloadSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008851 RID: 34897 RVA: 0x00387FB8 File Offset: 0x003861B8
	private void Awake()
	{
		Gun component = base.GetComponent<Gun>();
		if (component != null)
		{
			Gun gun = component;
			gun.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReloaded));
		}
		else
		{
			this.m_item = base.GetComponent<PassiveItem>();
			if (this.m_item != null)
			{
				PassiveItem item = this.m_item;
				item.OnPickedUp = (Action<PlayerController>)Delegate.Combine(item.OnPickedUp, new Action<PlayerController>(this.Hookup));
			}
		}
	}

	// Token: 0x06008852 RID: 34898 RVA: 0x00388044 File Offset: 0x00386244
	private void Hookup(PlayerController acquiringPlayer)
	{
		acquiringPlayer.OnReloadPressed = (Action<PlayerController, Gun>)Delegate.Combine(acquiringPlayer.OnReloadPressed, new Action<PlayerController, Gun>(this.HandleReloadedPlayer));
	}

	// Token: 0x06008853 RID: 34899 RVA: 0x00388068 File Offset: 0x00386268
	private void HandleReloadedPlayer(PlayerController usingPlayer, Gun usedGun)
	{
		if (!this.m_item || !this.m_item.Owner)
		{
			usingPlayer.OnReloadPressed = (Action<PlayerController, Gun>)Delegate.Remove(usingPlayer.OnReloadPressed, new Action<PlayerController, Gun>(this.HandleReloadedPlayer));
			return;
		}
		this.HandleReloaded(usingPlayer, usedGun, false);
	}

	// Token: 0x06008854 RID: 34900 RVA: 0x003880C8 File Offset: 0x003862C8
	private void HandleReloaded(PlayerController usingPlayer, Gun usedGun, bool manual)
	{
		if (this.OnlyOnEmptyClip && usedGun.ClipShotsRemaining > 0)
		{
			return;
		}
		if (usedGun.IsReloading && usingPlayer && (this.RequiresNoSynergy || usingPlayer.HasActiveBonusSynergy(this.SynergyToCheck, false)))
		{
			if (usedGun && usedGun.HasFiredReloadSynergy)
			{
				return;
			}
			usedGun.HasFiredReloadSynergy = true;
			if (this.DoesRadialBurst)
			{
				AkSoundEngine.SetSwitch("WPN_Guns", this.SwitchGroup, base.gameObject);
				AkSoundEngine.PostEvent(this.SFX, base.gameObject);
				this.RadialBurstSettings.DoBurst(usingPlayer, null, null);
				AkSoundEngine.SetSwitch("WPN_Guns", usedGun.gunSwitchGroup, base.gameObject);
			}
			if (this.DoesDirectedBurst)
			{
				AkSoundEngine.SetSwitch("WPN_Guns", this.SwitchGroup, base.gameObject);
				AkSoundEngine.PostEvent(this.SFX, base.gameObject);
				this.DirectedBurstSettings.DoBurst(usingPlayer, usedGun.CurrentAngle);
				AkSoundEngine.SetSwitch("WPN_Guns", usedGun.gunSwitchGroup, base.gameObject);
			}
		}
	}

	// Token: 0x04008DA1 RID: 36257
	public bool RequiresNoSynergy;

	// Token: 0x04008DA2 RID: 36258
	[LongNumericEnum]
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008DA3 RID: 36259
	public bool OnlyOnEmptyClip;

	// Token: 0x04008DA4 RID: 36260
	public bool DoesRadialBurst = true;

	// Token: 0x04008DA5 RID: 36261
	public RadialBurstInterface RadialBurstSettings;

	// Token: 0x04008DA6 RID: 36262
	public bool DoesDirectedBurst;

	// Token: 0x04008DA7 RID: 36263
	public DirectedBurstInterface DirectedBurstSettings;

	// Token: 0x04008DA8 RID: 36264
	public string SwitchGroup;

	// Token: 0x04008DA9 RID: 36265
	public string SFX;

	// Token: 0x04008DAA RID: 36266
	private Gun m_gun;

	// Token: 0x04008DAB RID: 36267
	private PassiveItem m_item;
}
