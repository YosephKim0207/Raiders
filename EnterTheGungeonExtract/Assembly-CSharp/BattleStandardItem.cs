using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001351 RID: 4945
public class BattleStandardItem : PassiveItem
{
	// Token: 0x06007026 RID: 28710 RVA: 0x002C7228 File Offset: 0x002C5428
	protected override void Update()
	{
		if (this.m_owner != null && this.m_pickedUp)
		{
			if (this.m_instanceOverheadSprite)
			{
				if (Time.frameCount % 10 == 0 && this.m_instanceOverhead && this.m_instanceOverhead.transform.parent == null)
				{
					this.DisengageEffect(this.m_owner);
					this.EngageEffect(this.m_owner);
				}
				if (this.m_owner.IsFalling)
				{
					this.m_hiddenForFall = true;
					this.m_instanceOverheadSprite.renderer.enabled = false;
				}
				else
				{
					if (this.m_hiddenForFall)
					{
						this.m_hiddenForFall = false;
						this.m_instanceOverheadSprite.renderer.enabled = true;
					}
					if (this.m_isBackfacing != this.m_owner.IsBackfacing())
					{
						this.m_isBackfacing = !this.m_isBackfacing;
						if (this.m_isBackfacing)
						{
							this.m_instanceOverheadSprite.transform.localPosition = this.m_instanceOverheadSprite.transform.localPosition.WithY(this.m_instanceOverheadSprite.transform.localPosition.y - 0.25f);
							this.m_instanceOverheadSprite.SetSprite("battle_standard_back_001");
						}
						else
						{
							this.m_instanceOverheadSprite.transform.localPosition = this.m_instanceOverheadSprite.transform.localPosition.WithY(this.m_instanceOverheadSprite.transform.localPosition.y + 0.25f);
							this.m_instanceOverheadSprite.SetSprite("battle_standard_001");
						}
					}
					if (this.m_instanceOverheadSprite.FlipX != this.m_owner.sprite.FlipX)
					{
						this.m_instanceOverheadSprite.FlipX = this.m_owner.sprite.FlipX;
						this.m_instanceOverheadSprite.transform.localPosition = this.m_instanceOverheadSprite.transform.localPosition.WithX(this.m_instanceOverheadSprite.transform.localPosition.x * -1f);
					}
					if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
					{
						this.DisengageEffect(this.m_owner);
					}
				}
			}
			else if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.END_TIMES)
			{
				this.EngageEffect(this.m_owner);
			}
		}
	}

	// Token: 0x06007027 RID: 28711 RVA: 0x002C749C File Offset: 0x002C569C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		BattleStandardItem.BattleStandardCharmDurationMultiplier = this.CharmDurationMultiplier;
		BattleStandardItem.BattleStandardCompanionDamageMultiplier = this.CompanionDamageMultiplier;
		if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
		{
			PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
		}
		if (!PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player].Add(base.GetType(), 1);
		}
		else
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = PassiveItem.ActiveFlagItems[player][base.GetType()] + 1;
		}
		this.EngageEffect(player);
		base.Pickup(player);
	}

	// Token: 0x06007028 RID: 28712 RVA: 0x002C7560 File Offset: 0x002C5760
	protected void EngageEffect(PlayerController user)
	{
		if (!this.m_instanceOverhead)
		{
			this.m_instanceOverhead = user.RegisterAttachedObject(this.OverheadVFXSprite, "jetpack", 0.1f);
		}
		this.m_instanceOverheadSprite = this.m_instanceOverhead.GetComponentInChildren<tk2dSprite>();
	}

	// Token: 0x06007029 RID: 28713 RVA: 0x002C75A0 File Offset: 0x002C57A0
	protected void DisengageEffect(PlayerController user)
	{
		if (this.m_instanceOverhead)
		{
			user.DeregisterAttachedObject(this.m_instanceOverhead, true);
			this.m_instanceOverhead = null;
			this.m_instanceOverheadSprite = null;
		}
	}

	// Token: 0x0600702A RID: 28714 RVA: 0x002C75D0 File Offset: 0x002C57D0
	public override DebrisObject Drop(PlayerController player)
	{
		this.DisengageEffect(player);
		DebrisObject debrisObject = base.Drop(player);
		if (PassiveItem.ActiveFlagItems.ContainsKey(player) && PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[player][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[player].Remove(base.GetType());
			}
		}
		debrisObject.GetComponent<BattleStandardItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x0600702B RID: 28715 RVA: 0x002C768C File Offset: 0x002C588C
	protected override void OnDestroy()
	{
		if (this.m_owner)
		{
			this.DisengageEffect(this.m_owner);
		}
		BraveTime.ClearMultiplier(base.gameObject);
		if (this.m_pickedUp && PassiveItem.ActiveFlagItems.ContainsKey(this.m_owner) && PassiveItem.ActiveFlagItems[this.m_owner].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[this.m_owner].Remove(base.GetType());
			}
		}
		base.OnDestroy();
	}

	// Token: 0x04006F7C RID: 28540
	public static float BattleStandardCharmDurationMultiplier = 2f;

	// Token: 0x04006F7D RID: 28541
	public static float BattleStandardCompanionDamageMultiplier = 2f;

	// Token: 0x04006F7E RID: 28542
	public float CharmDurationMultiplier = 2f;

	// Token: 0x04006F7F RID: 28543
	public float CompanionDamageMultiplier = 2f;

	// Token: 0x04006F80 RID: 28544
	public GameObject OverheadVFXSprite;

	// Token: 0x04006F81 RID: 28545
	private GameObject m_instanceOverhead;

	// Token: 0x04006F82 RID: 28546
	private tk2dSprite m_instanceOverheadSprite;

	// Token: 0x04006F83 RID: 28547
	private bool m_hiddenForFall;

	// Token: 0x04006F84 RID: 28548
	private bool m_isBackfacing;
}
