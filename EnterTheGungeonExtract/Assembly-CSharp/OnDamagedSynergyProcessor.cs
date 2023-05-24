using System;
using UnityEngine;

// Token: 0x02001705 RID: 5893
public class OnDamagedSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008903 RID: 35075 RVA: 0x0038D618 File Offset: 0x0038B818
	public void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		this.m_item = base.GetComponent<PassiveItem>();
	}

	// Token: 0x06008904 RID: 35076 RVA: 0x0038D634 File Offset: 0x0038B834
	public void Update()
	{
		PlayerController owner = this.GetOwner();
		if (!this.m_actionsLinked && owner)
		{
			this.m_cachedLinkedPlayer = owner;
			this.m_cachedArmor = owner.healthHaver.Armor;
			owner.OnReceivedDamage += this.HandleOwnerDamaged;
			this.m_actionsLinked = true;
		}
		else if (this.m_actionsLinked && !owner && this.m_cachedLinkedPlayer)
		{
			this.m_cachedLinkedPlayer.OnReceivedDamage -= this.HandleOwnerDamaged;
			this.m_cachedLinkedPlayer = null;
			this.m_actionsLinked = false;
		}
		if (this.m_actionsLinked && this.m_cachedLinkedPlayer)
		{
			this.m_cachedArmor = this.m_cachedLinkedPlayer.healthHaver.Armor;
		}
	}

	// Token: 0x06008905 RID: 35077 RVA: 0x0038D710 File Offset: 0x0038B910
	private void HandleOwnerDamaged(PlayerController sourcePlayer)
	{
		if (sourcePlayer.HasActiveBonusSynergy(this.RequiredSynergy, false))
		{
			if (this.OnlyArmorDamage && this.m_cachedArmor == sourcePlayer.healthHaver.Armor)
			{
				return;
			}
			if (!string.IsNullOrEmpty(this.OnTriggeredAudioEvent))
			{
				AkSoundEngine.PostEvent(this.OnTriggeredAudioEvent, sourcePlayer.gameObject);
			}
			if (this.DoesRadialBurst)
			{
				this.RadialBurst.DoBurst(sourcePlayer, null, null);
			}
			if (this.DoesRadialSlow)
			{
				this.RadialSlow.DoRadialSlow(sourcePlayer.CenterPosition, sourcePlayer.CurrentRoom);
			}
		}
	}

	// Token: 0x06008906 RID: 35078 RVA: 0x0038D7C0 File Offset: 0x0038B9C0
	private PlayerController GetOwner()
	{
		if (this.m_gun)
		{
			return this.m_gun.CurrentOwner as PlayerController;
		}
		if (this.m_item)
		{
			return this.m_item.Owner;
		}
		return null;
	}

	// Token: 0x06008907 RID: 35079 RVA: 0x0038D800 File Offset: 0x0038BA00
	public void OnDestroy()
	{
		if (this.m_actionsLinked && this.m_cachedLinkedPlayer)
		{
			this.m_cachedLinkedPlayer.OnReceivedDamage -= this.HandleOwnerDamaged;
			this.m_cachedLinkedPlayer = null;
			this.m_actionsLinked = false;
		}
	}

	// Token: 0x04008EC9 RID: 36553
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008ECA RID: 36554
	public bool OnlyArmorDamage;

	// Token: 0x04008ECB RID: 36555
	public bool DoesRadialBurst;

	// Token: 0x04008ECC RID: 36556
	public RadialBurstInterface RadialBurst;

	// Token: 0x04008ECD RID: 36557
	public bool DoesRadialSlow;

	// Token: 0x04008ECE RID: 36558
	public RadialSlowInterface RadialSlow;

	// Token: 0x04008ECF RID: 36559
	public string OnTriggeredAudioEvent;

	// Token: 0x04008ED0 RID: 36560
	private bool m_actionsLinked;

	// Token: 0x04008ED1 RID: 36561
	private PlayerController m_cachedLinkedPlayer;

	// Token: 0x04008ED2 RID: 36562
	private Gun m_gun;

	// Token: 0x04008ED3 RID: 36563
	private PassiveItem m_item;

	// Token: 0x04008ED4 RID: 36564
	private float m_cachedArmor;
}
