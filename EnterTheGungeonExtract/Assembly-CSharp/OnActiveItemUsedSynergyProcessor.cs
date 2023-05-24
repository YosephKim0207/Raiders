using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001704 RID: 5892
public class OnActiveItemUsedSynergyProcessor : MonoBehaviour
{
	// Token: 0x060088F8 RID: 35064 RVA: 0x0038D214 File Offset: 0x0038B414
	public void Awake()
	{
		this.m_item = base.GetComponent<PlayerItem>();
		PlayerItem item = this.m_item;
		item.OnActivationStatusChanged = (Action<PlayerItem>)Delegate.Combine(item.OnActivationStatusChanged, new Action<PlayerItem>(this.HandleActivationStatusChanged));
		PlayerItem item2 = this.m_item;
		item2.OnPreDropEvent = (Action)Delegate.Combine(item2.OnPreDropEvent, new Action(this.HandlePreDrop));
	}

	// Token: 0x060088F9 RID: 35065 RVA: 0x0038D27C File Offset: 0x0038B47C
	private void HandlePreDrop()
	{
		if (this.CreatesHoveringGun)
		{
			this.DisableAllHoveringGuns();
		}
	}

	// Token: 0x060088FA RID: 35066 RVA: 0x0038D290 File Offset: 0x0038B490
	private void Update()
	{
		this.m_internalCooldown -= BraveTime.DeltaTime;
		if (this.CreatesHoveringGun && this.m_hovers.Count > 0 && (!this.m_item || !this.m_item.LastOwner || this.m_item.LastOwner.CurrentItem != this.m_item || this.m_item.LastOwner.IsGhost))
		{
			this.DisableAllHoveringGuns();
		}
	}

	// Token: 0x060088FB RID: 35067 RVA: 0x0038D32C File Offset: 0x0038B52C
	private void HandleActivationStatusChanged(PlayerItem sourceItem)
	{
		if (this.m_item.LastOwner && this.m_item.LastOwner.HasActiveBonusSynergy(this.SynergyToCheck, false))
		{
			if (sourceItem.IsCurrentlyActive)
			{
				this.HandleActivated();
			}
			else
			{
				this.HandleDeactivated();
			}
		}
	}

	// Token: 0x060088FC RID: 35068 RVA: 0x0038D388 File Offset: 0x0038B588
	private void HandleActivated()
	{
		if (this.FiresOnActivation && this.m_internalCooldown <= 0f)
		{
			this.ActivationBurst.DoBurst(this.m_item.LastOwner, null, null);
			this.m_internalCooldown = this.ActivationBurstCooldown;
		}
		if (this.CreatesHoveringGun)
		{
			this.EnableHoveringGun(0);
		}
	}

	// Token: 0x060088FD RID: 35069 RVA: 0x0038D3F8 File Offset: 0x0038B5F8
	private void HandleDeactivated()
	{
		if (this.CreatesHoveringGun)
		{
			this.DisableAllHoveringGuns();
		}
	}

	// Token: 0x060088FE RID: 35070 RVA: 0x0038D40C File Offset: 0x0038B60C
	private void EnableHoveringGun(int index)
	{
		if (this.m_hoverInitialized.Count > index && this.m_hoverInitialized[index])
		{
			return;
		}
		PlayerController lastOwner = this.m_item.LastOwner;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(ResourceCache.Acquire("Global Prefabs/HoveringGun") as GameObject, lastOwner.CenterPosition.ToVector3ZisY(0f), Quaternion.identity);
		gameObject.transform.parent = lastOwner.transform;
		while (this.m_hovers.Count < index + 1)
		{
			this.m_hovers.Add(null);
			this.m_hoverInitialized.Add(false);
		}
		this.m_hovers[index] = gameObject.GetComponent<HoveringGunController>();
		this.m_hovers[index].Position = this.PositionType;
		this.m_hovers[index].Aim = this.AimType;
		this.m_hovers[index].Trigger = this.FireType;
		Gun currentGun = lastOwner.CurrentGun;
		this.m_hovers[index].CooldownTime = 10f;
		this.m_hovers[index].ShootDuration = this.HoverDuration;
		this.m_hovers[index].Initialize(currentGun, lastOwner);
		this.m_hoverInitialized[index] = true;
	}

	// Token: 0x060088FF RID: 35071 RVA: 0x0038D560 File Offset: 0x0038B760
	private void DisableHoveringGun(int index)
	{
		if (this.m_hovers[index])
		{
			UnityEngine.Object.Destroy(this.m_hovers[index].gameObject);
		}
	}

	// Token: 0x06008900 RID: 35072 RVA: 0x0038D590 File Offset: 0x0038B790
	private void DisableAllHoveringGuns()
	{
		for (int i = 0; i < this.m_hovers.Count; i++)
		{
			if (this.m_hovers[i])
			{
				UnityEngine.Object.Destroy(this.m_hovers[i].gameObject);
			}
		}
		this.m_hovers.Clear();
		this.m_hoverInitialized.Clear();
	}

	// Token: 0x06008901 RID: 35073 RVA: 0x0038D5FC File Offset: 0x0038B7FC
	private void OnDestroy()
	{
		if (this.CreatesHoveringGun)
		{
			this.DisableAllHoveringGuns();
		}
	}

	// Token: 0x04008EBC RID: 36540
	[LongNumericEnum]
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008EBD RID: 36541
	public bool FiresOnActivation;

	// Token: 0x04008EBE RID: 36542
	[ShowInInspectorIf("FiresOnActivation", false)]
	public RadialBurstInterface ActivationBurst;

	// Token: 0x04008EBF RID: 36543
	[ShowInInspectorIf("FiresOnActivation", false)]
	public float ActivationBurstCooldown;

	// Token: 0x04008EC0 RID: 36544
	public bool CreatesHoveringGun;

	// Token: 0x04008EC1 RID: 36545
	[ShowInInspectorIf("CreatesHoveringGun", false)]
	public HoveringGunController.HoverPosition PositionType;

	// Token: 0x04008EC2 RID: 36546
	[ShowInInspectorIf("CreatesHoveringGun", false)]
	public HoveringGunController.AimType AimType;

	// Token: 0x04008EC3 RID: 36547
	[ShowInInspectorIf("CreatesHoveringGun", false)]
	public HoveringGunController.FireType FireType;

	// Token: 0x04008EC4 RID: 36548
	[ShowInInspectorIf("CreatesHoveringGun", false)]
	public float HoverDuration = 5f;

	// Token: 0x04008EC5 RID: 36549
	private PlayerItem m_item;

	// Token: 0x04008EC6 RID: 36550
	private float m_internalCooldown;

	// Token: 0x04008EC7 RID: 36551
	private List<HoveringGunController> m_hovers = new List<HoveringGunController>();

	// Token: 0x04008EC8 RID: 36552
	private List<bool> m_hoverInitialized = new List<bool>();
}
