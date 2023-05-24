using System;
using Dungeonator;

// Token: 0x02001499 RID: 5273
public class SackKnightController : CompanionController
{
	// Token: 0x06007804 RID: 30724 RVA: 0x002FEB30 File Offset: 0x002FCD30
	public static bool HasJunkan()
	{
		if (GameManager.HasInstance && GameManager.Instance.PrimaryPlayer)
		{
			for (int i = 0; i < GameManager.Instance.PrimaryPlayer.passiveItems.Count; i++)
			{
				PassiveItem passiveItem = GameManager.Instance.PrimaryPlayer.passiveItems[i];
				if (passiveItem is CompanionItem && (passiveItem as CompanionItem).ExtantCompanion && (passiveItem as CompanionItem).ExtantCompanion.GetComponent<SackKnightController>())
				{
					return true;
				}
			}
		}
		if (GameManager.HasInstance && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer)
		{
			for (int j = 0; j < GameManager.Instance.SecondaryPlayer.passiveItems.Count; j++)
			{
				PassiveItem passiveItem2 = GameManager.Instance.SecondaryPlayer.passiveItems[j];
				if (passiveItem2 is CompanionItem && (passiveItem2 as CompanionItem).ExtantCompanion && (passiveItem2 as CompanionItem).ExtantCompanion.GetComponent<SackKnightController>())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06007805 RID: 30725 RVA: 0x002FEC78 File Offset: 0x002FCE78
	protected override void HandleRoomCleared(PlayerController callingPlayer)
	{
		if (this.CurrentForm >= SackKnightController.SackKnightPhase.KNIGHT && callingPlayer.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS && callingPlayer.CurrentRoom.area.PrototypeRoomBossSubcategory != PrototypeDungeonRoom.RoomBossSubCategory.MINI_BOSS)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_SER_JUNKAN_UNLOCKED, true);
		}
	}

	// Token: 0x06007806 RID: 30726 RVA: 0x002FECD0 File Offset: 0x002FCED0
	public override void Update()
	{
		if (!GameManager.Instance || GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		if (this.m_owner && !Chest.HasDroppedSerJunkanThisSession)
		{
			Chest.HasDroppedSerJunkanThisSession = true;
		}
		this.UpdateAnimationNamesBasedOnSacks();
		if (this.HasStealthMode && this.m_owner)
		{
			if (this.m_owner.IsStealthed && !this.m_isStealthed)
			{
				this.m_isStealthed = true;
				base.aiAnimator.IdleAnimation.AnimNames[0] = "sst_dis_idle_right";
				base.aiAnimator.IdleAnimation.AnimNames[1] = "sst_dis_idle_left";
				base.aiAnimator.MoveAnimation.AnimNames[0] = "sst_dis_move_right";
				base.aiAnimator.MoveAnimation.AnimNames[1] = "sst_dis_move_left";
			}
			else if (!this.m_owner.IsStealthed && this.m_isStealthed)
			{
				this.m_isStealthed = false;
				base.aiAnimator.IdleAnimation.AnimNames[0] = "sst_idle_right";
				base.aiAnimator.IdleAnimation.AnimNames[1] = "sst_idle_left";
				base.aiAnimator.MoveAnimation.AnimNames[0] = "sst_move_right";
				base.aiAnimator.MoveAnimation.AnimNames[1] = "sst_move_left";
			}
		}
		IntVector2 intVector = base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
		if (GameManager.Instance.Dungeon.data.CheckInBounds(intVector))
		{
			CellData cellData = GameManager.Instance.Dungeon.data[intVector];
			if (cellData != null)
			{
				this.m_lastCellType = cellData.type;
			}
		}
	}

	// Token: 0x06007807 RID: 30727 RVA: 0x002FEE94 File Offset: 0x002FD094
	private void UpdateAnimationNamesBasedOnSacks()
	{
		if (this.m_owner)
		{
			int num = 0;
			bool flag = false;
			for (int i = 0; i < this.m_owner.passiveItems.Count; i++)
			{
				if (this.m_owner.passiveItems[i] is BasicStatPickup)
				{
					BasicStatPickup basicStatPickup = this.m_owner.passiveItems[i] as BasicStatPickup;
					if (basicStatPickup.IsJunk)
					{
						num++;
					}
					if (basicStatPickup.IsJunk && basicStatPickup.GivesCurrency)
					{
						flag = true;
					}
				}
			}
			AIAnimator aiAnimator = base.aiAnimator;
			if (flag)
			{
				if (this.CurrentForm != SackKnightController.SackKnightPhase.MECHAJUNKAN)
				{
					base.specRigidbody.PixelColliders[0].ManualOffsetX = 30;
					base.specRigidbody.PixelColliders[0].ManualOffsetY = 3;
					base.specRigidbody.PixelColliders[0].ManualWidth = 17;
					base.specRigidbody.PixelColliders[0].ManualHeight = 16;
					base.specRigidbody.PixelColliders[1].ManualOffsetX = 30;
					base.specRigidbody.PixelColliders[1].ManualOffsetY = 3;
					base.specRigidbody.PixelColliders[1].ManualWidth = 17;
					base.specRigidbody.PixelColliders[1].ManualHeight = 28;
					base.specRigidbody.PixelColliders[0].Regenerate(base.transform, true, true);
					base.specRigidbody.PixelColliders[1].Regenerate(base.transform, true, true);
					base.specRigidbody.Reinitialize();
					base.aiActor.ShadowObject.transform.position = base.specRigidbody.UnitBottomCenter;
				}
				this.CurrentForm = SackKnightController.SackKnightPhase.MECHAJUNKAN;
				aiAnimator.IdleAnimation.AnimNames[0] = "junk_g_idle_right";
				aiAnimator.IdleAnimation.AnimNames[1] = "junk_g_idle_left";
				aiAnimator.MoveAnimation.AnimNames[0] = "junk_g_move_right";
				aiAnimator.MoveAnimation.AnimNames[1] = "junk_g_move_left";
				aiAnimator.TalkAnimation.AnimNames[0] = "junk_g_talk_right";
				aiAnimator.TalkAnimation.AnimNames[1] = "junk_g_talk_left";
				aiAnimator.OtherAnimations[0].anim.AnimNames[0] = "junk_g_sword_right";
				aiAnimator.OtherAnimations[0].anim.AnimNames[1] = "junk_g_sword_left";
			}
			else if (num < 1)
			{
				this.CurrentForm = SackKnightController.SackKnightPhase.PEASANT;
				aiAnimator.IdleAnimation.AnimNames[0] = "junk_idle_right";
				aiAnimator.IdleAnimation.AnimNames[1] = "junk_idle_left";
				aiAnimator.MoveAnimation.AnimNames[0] = "junk_move_right";
				aiAnimator.MoveAnimation.AnimNames[1] = "junk_move_left";
				aiAnimator.TalkAnimation.AnimNames[0] = "junk_talk_right";
				aiAnimator.TalkAnimation.AnimNames[1] = "junk_talk_left";
				aiAnimator.OtherAnimations[0].anim.AnimNames[0] = "junk_attack_right";
				aiAnimator.OtherAnimations[0].anim.AnimNames[1] = "junk_attack_left";
			}
			else if (num == 1)
			{
				this.CurrentForm = SackKnightController.SackKnightPhase.SQUIRE;
				aiAnimator.IdleAnimation.AnimNames[0] = "junk_h_idle_right";
				aiAnimator.IdleAnimation.AnimNames[1] = "junk_h_idle_left";
				aiAnimator.MoveAnimation.AnimNames[0] = "junk_h_move_right";
				aiAnimator.MoveAnimation.AnimNames[1] = "junk_h_move_left";
				aiAnimator.TalkAnimation.AnimNames[0] = "junk_h_talk_right";
				aiAnimator.TalkAnimation.AnimNames[1] = "junk_h_talk_left";
				aiAnimator.OtherAnimations[0].anim.AnimNames[0] = "junk_h_attack_right";
				aiAnimator.OtherAnimations[0].anim.AnimNames[1] = "junk_h_attack_left";
			}
			else if (num == 2)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_GOLD_JUNK, true);
				this.CurrentForm = SackKnightController.SackKnightPhase.HEDGE_KNIGHT;
				aiAnimator.IdleAnimation.AnimNames[0] = "junk_sh_idle_right";
				aiAnimator.IdleAnimation.AnimNames[1] = "junk_sh_idle_left";
				aiAnimator.MoveAnimation.AnimNames[0] = "junk_sh_move_right";
				aiAnimator.MoveAnimation.AnimNames[1] = "junk_sh_move_left";
				aiAnimator.TalkAnimation.AnimNames[0] = "junk_sh_talk_right";
				aiAnimator.TalkAnimation.AnimNames[1] = "junk_sh_talk_left";
				aiAnimator.OtherAnimations[0].anim.AnimNames[0] = "junk_sh_attack_right";
				aiAnimator.OtherAnimations[0].anim.AnimNames[1] = "junk_sh_attack_left";
			}
			else if (num == 3)
			{
				this.CurrentForm = SackKnightController.SackKnightPhase.KNIGHT;
				aiAnimator.IdleAnimation.AnimNames[0] = "junk_shs_idle_right";
				aiAnimator.IdleAnimation.AnimNames[1] = "junk_shs_idle_left";
				aiAnimator.MoveAnimation.AnimNames[0] = "junk_shs_move_right";
				aiAnimator.MoveAnimation.AnimNames[1] = "junk_shs_move_left";
				aiAnimator.TalkAnimation.AnimNames[0] = "junk_shs_talk_right";
				aiAnimator.TalkAnimation.AnimNames[1] = "junk_shs_talk_left";
				aiAnimator.OtherAnimations[0].anim.AnimNames[0] = "junk_shs_attack_right";
				aiAnimator.OtherAnimations[0].anim.AnimNames[1] = "junk_shs_attack_left";
			}
			else if (num == 4)
			{
				this.CurrentForm = SackKnightController.SackKnightPhase.KNIGHT_LIEUTENANT;
				aiAnimator.IdleAnimation.AnimNames[0] = "junk_shsp_idle_right";
				aiAnimator.IdleAnimation.AnimNames[1] = "junk_shsp_idle_left";
				aiAnimator.MoveAnimation.AnimNames[0] = "junk_shsp_move_right";
				aiAnimator.MoveAnimation.AnimNames[1] = "junk_shsp_move_left";
				aiAnimator.TalkAnimation.AnimNames[0] = "junk_shsp_talk_right";
				aiAnimator.TalkAnimation.AnimNames[1] = "junk_shsp_talk_left";
				aiAnimator.OtherAnimations[0].anim.AnimNames[0] = "junk_shsp_attack_right";
				aiAnimator.OtherAnimations[0].anim.AnimNames[1] = "junk_shsp_attack_left";
			}
			else if (num == 5)
			{
				this.CurrentForm = SackKnightController.SackKnightPhase.KNIGHT_COMMANDER;
				aiAnimator.IdleAnimation.AnimNames[0] = "junk_shspc_idle_right";
				aiAnimator.IdleAnimation.AnimNames[1] = "junk_shspc_idle_left";
				aiAnimator.MoveAnimation.AnimNames[0] = "junk_shspc_move_right";
				aiAnimator.MoveAnimation.AnimNames[1] = "junk_shspc_move_left";
				aiAnimator.TalkAnimation.AnimNames[0] = "junk_shspc_talk_right";
				aiAnimator.TalkAnimation.AnimNames[1] = "junk_shspc_talk_left";
				aiAnimator.OtherAnimations[0].anim.AnimNames[0] = "junk_shspc_attack_right";
				aiAnimator.OtherAnimations[0].anim.AnimNames[1] = "junk_shspc_attack_left";
			}
			else if (num == 6)
			{
				this.CurrentForm = SackKnightController.SackKnightPhase.HOLY_KNIGHT;
				aiAnimator.IdleAnimation.AnimNames[0] = "junk_shspcg_idle_right";
				aiAnimator.IdleAnimation.AnimNames[1] = "junk_shspcg_idle_left";
				aiAnimator.MoveAnimation.AnimNames[0] = "junk_shspcg_move_right";
				aiAnimator.MoveAnimation.AnimNames[1] = "junk_shspcg_move_left";
				aiAnimator.TalkAnimation.AnimNames[0] = "junk_shspcg_talk_right";
				aiAnimator.TalkAnimation.AnimNames[1] = "junk_shspcg_talk_left";
				aiAnimator.OtherAnimations[0].anim.AnimNames[0] = "junk_shspcg_attack_right";
				aiAnimator.OtherAnimations[0].anim.AnimNames[1] = "junk_shspcg_attack_left";
			}
			else if (num > 6)
			{
				this.CurrentForm = SackKnightController.SackKnightPhase.ANGELIC_KNIGHT;
				GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_SER_JUNKAN_MAXLVL, true);
				aiAnimator.IdleAnimation.AnimNames[0] = "junk_a_idle_right";
				aiAnimator.IdleAnimation.AnimNames[1] = "junk_a_idle_left";
				aiAnimator.MoveAnimation.AnimNames[0] = "junk_a_idle_right";
				aiAnimator.MoveAnimation.AnimNames[1] = "junk_a_idle_left";
				aiAnimator.TalkAnimation.AnimNames[0] = "junk_a_talk_right";
				aiAnimator.TalkAnimation.AnimNames[1] = "junk_a_talk_left";
				aiAnimator.OtherAnimations[0].anim.AnimNames[0] = "junk_a_attack_right";
				aiAnimator.OtherAnimations[0].anim.AnimNames[1] = "junk_a_attack_left";
				if (!base.aiActor.IsFlying)
				{
					base.aiActor.SetIsFlying(true, "angel", true, true);
				}
			}
			if (this.CurrentForm != SackKnightController.SackKnightPhase.ANGELIC_KNIGHT && base.aiActor.IsFlying)
			{
				base.aiActor.SetIsFlying(false, "angel", true, true);
			}
		}
	}

	// Token: 0x04007A1B RID: 31259
	public const bool c_usesJunkNotArmor = true;

	// Token: 0x04007A1C RID: 31260
	public SackKnightController.SackKnightPhase CurrentForm;

	// Token: 0x0200149A RID: 5274
	public enum SackKnightPhase
	{
		// Token: 0x04007A1E RID: 31262
		PEASANT,
		// Token: 0x04007A1F RID: 31263
		SQUIRE,
		// Token: 0x04007A20 RID: 31264
		HEDGE_KNIGHT,
		// Token: 0x04007A21 RID: 31265
		KNIGHT,
		// Token: 0x04007A22 RID: 31266
		KNIGHT_LIEUTENANT,
		// Token: 0x04007A23 RID: 31267
		KNIGHT_COMMANDER,
		// Token: 0x04007A24 RID: 31268
		HOLY_KNIGHT,
		// Token: 0x04007A25 RID: 31269
		ANGELIC_KNIGHT,
		// Token: 0x04007A26 RID: 31270
		MECHAJUNKAN
	}
}
