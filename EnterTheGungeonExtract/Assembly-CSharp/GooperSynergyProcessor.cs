using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020016EA RID: 5866
public class GooperSynergyProcessor : MonoBehaviour
{
	// Token: 0x0600885F RID: 34911 RVA: 0x00388A34 File Offset: 0x00386C34
	public void Awake()
	{
		this.m_item = base.GetComponent<PassiveItem>();
		this.m_manager = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopDefinition);
	}

	// Token: 0x06008860 RID: 34912 RVA: 0x00388A54 File Offset: 0x00386C54
	private void Initialize(PlayerController p)
	{
		if (this.m_initialized)
		{
			return;
		}
		this.m_initialized = true;
		p.OnIsRolling += this.HandleRollFrame;
		for (int i = 0; i < this.modifiers.Length; i++)
		{
			p.healthHaver.damageTypeModifiers.Add(this.modifiers[i]);
		}
		this.m_player = p;
	}

	// Token: 0x06008861 RID: 34913 RVA: 0x00388AC0 File Offset: 0x00386CC0
	private void Uninitialize()
	{
		if (!this.m_initialized)
		{
			return;
		}
		this.m_initialized = false;
		this.m_player.OnIsRolling -= this.HandleRollFrame;
		for (int i = 0; i < this.modifiers.Length; i++)
		{
			this.m_player.healthHaver.damageTypeModifiers.Remove(this.modifiers[i]);
		}
		this.m_player = null;
	}

	// Token: 0x06008862 RID: 34914 RVA: 0x00388B38 File Offset: 0x00386D38
	private void Update()
	{
		if (Dungeon.IsGenerating)
		{
			this.m_manager = null;
			return;
		}
		if (!GameManager.HasInstance || !GameManager.Instance.Dungeon)
		{
			this.m_manager = null;
			return;
		}
		if (!this.m_manager)
		{
			this.m_manager = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopDefinition);
		}
		if (this.m_initialized)
		{
			if ((this.m_item && !this.m_item.Owner) || !this.m_item.Owner.HasActiveBonusSynergy(this.RequiredSynergy, false))
			{
				this.Uninitialize();
				return;
			}
		}
		else if (this.m_item && this.m_item.Owner && this.m_item.Owner.HasActiveBonusSynergy(this.RequiredSynergy, false))
		{
			this.Initialize(this.m_item.Owner);
		}
	}

	// Token: 0x06008863 RID: 34915 RVA: 0x00388C48 File Offset: 0x00386E48
	private void HandleRollFrame(PlayerController p)
	{
		if (GameManager.Instance.IsFoyer)
		{
			return;
		}
		if (GameManager.Instance.Dungeon.IsEndTimes)
		{
			return;
		}
		this.m_manager.AddGoopCircle(p.specRigidbody.UnitCenter, this.goopRadius, -1, false, -1);
	}

	// Token: 0x04008DC2 RID: 36290
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008DC3 RID: 36291
	public GoopDefinition goopDefinition;

	// Token: 0x04008DC4 RID: 36292
	public float goopRadius;

	// Token: 0x04008DC5 RID: 36293
	public DamageTypeModifier[] modifiers;

	// Token: 0x04008DC6 RID: 36294
	private PassiveItem m_item;

	// Token: 0x04008DC7 RID: 36295
	private PlayerController m_player;

	// Token: 0x04008DC8 RID: 36296
	private DeadlyDeadlyGoopManager m_manager;

	// Token: 0x04008DC9 RID: 36297
	private bool m_initialized;
}
