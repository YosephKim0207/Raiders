using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C54 RID: 3156
	public class CheckGunslingChallengeComplete : BraveFsmStateAction
	{
		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060043F6 RID: 17398 RVA: 0x0015EF0C File Offset: 0x0015D10C
		// (set) Token: 0x060043F7 RID: 17399 RVA: 0x0015EF14 File Offset: 0x0015D114
		public RoomHandler ChallengeRoom
		{
			get
			{
				return this.m_challengeRoom;
			}
			set
			{
				this.m_challengeRoom = value;
			}
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x0015EF20 File Offset: 0x0015D120
		public override void Awake()
		{
			base.Awake();
			this.m_talkDoer = base.Owner.GetComponent<TalkDoerLite>();
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x0015EF3C File Offset: 0x0015D13C
		public override void OnEnter()
		{
			base.OnEnter();
			this.ChallengeType = (GunslingChallengeType)base.Fsm.Variables.GetFsmInt("ChallengeType").Value;
			this.m_challengeRoom = this.m_talkDoer.GetAbsoluteParentRoom().injectionTarget;
			this.m_challengeRoom.IsGunslingKingChallengeRoom = true;
			if (!this.m_hasAlreadyRegisteredIcon)
			{
				this.m_hasAlreadyRegisteredIcon = true;
				this.m_extantIcon = Minimap.Instance.RegisterRoomIcon(this.m_challengeRoom, ResourceCache.Acquire("Global Prefabs/Minimap_King_Icon") as GameObject, true);
			}
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (this.ChallengeType == GunslingChallengeType.NO_DAMAGE)
				{
					GameManager.Instance.AllPlayers[i].healthHaver.OnDamaged += this.HandlePlayerDamagedFailed;
				}
				if (this.ChallengeType == GunslingChallengeType.SPECIFIC_GUN)
				{
					GameManager.Instance.AllPlayers[i].PostProcessProjectile += this.HandlePlayerFiredProjectile;
					GameManager.Instance.AllPlayers[i].PostProcessBeam += this.HandlePlayerFiredBeam;
				}
			}
			if (this.ChallengeType == GunslingChallengeType.DAISUKE_CHALLENGES)
			{
				ChallengeManager.ChallengeModeType = ChallengeModeType.GunslingKingTemporary;
				ChallengeManager.Instance.GunslingTargetRoom = this.m_challengeRoom;
			}
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x0015F07C File Offset: 0x0015D27C
		private void Succeed()
		{
			if (this.m_hasSucceeded)
			{
				return;
			}
			if (this.m_extantIcon)
			{
				Minimap.Instance.DeregisterRoomIcon(this.m_challengeRoom, this.m_extantIcon);
			}
			this.m_hasSucceeded = true;
			switch (this.ChallengeType)
			{
			case GunslingChallengeType.NO_DAMAGE:
				GameStatsManager.Instance.SetFlag(GungeonFlags.GUNSLING_KING_CHALLENGE_TYPE_ONE_COMPLETE, true);
				break;
			case GunslingChallengeType.NO_DODGE_ROLL:
				GameStatsManager.Instance.SetFlag(GungeonFlags.GUNSLING_KING_CHALLENGE_TYPE_TWO_COMPLETE, true);
				break;
			case GunslingChallengeType.SPECIFIC_GUN:
				GameStatsManager.Instance.SetFlag(GungeonFlags.GUNSLING_KING_CHALLENGE_TYPE_THREE_COMPLETE, true);
				break;
			case GunslingChallengeType.DAISUKE_CHALLENGES:
				GameStatsManager.Instance.SetFlag(GungeonFlags.GUNSLING_KING_CHALLENGE_TYPE_FOUR_COMPLETE, true);
				break;
			}
			this.GetRidOfSuppliedGun();
			int num = 0;
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLING_KING_CHALLENGE_TYPE_ONE_COMPLETE))
			{
				num++;
			}
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLING_KING_CHALLENGE_TYPE_TWO_COMPLETE))
			{
				num++;
			}
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLING_KING_CHALLENGE_TYPE_THREE_COMPLETE))
			{
				num++;
			}
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLING_KING_CHALLENGE_TYPE_FOUR_COMPLETE))
			{
				num++;
			}
			if (num >= 3)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.GUNSLING_KING_ACTIVE_IN_FOYER, true);
			}
			this.InformManservantSuccess();
			base.Fsm.Event(this.SuccessEvent);
			tk2dSprite component = (ResourceCache.Acquire("Global VFX/GunslingKing_VictoryIcon") as GameObject).GetComponent<tk2dSprite>();
			GameUIRoot.Instance.notificationController.DoCustomNotification(StringTableManager.GetString("#GUNKING_SUCCESS_HEADER"), StringTableManager.GetString("#GUNKING_SUCCESS_BODY"), component.Collection, component.spriteId, UINotificationController.NotificationColor.GOLD, false, false);
			base.Finish();
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x0015F21C File Offset: 0x0015D41C
		private void Fail()
		{
			this.m_success = false;
			if (this.m_extantIcon)
			{
				Minimap.Instance.DeregisterRoomIcon(this.m_challengeRoom, this.m_extantIcon);
			}
			this.GetRidOfSuppliedGun();
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.GUNSLING_KING_CHALLENGES_FAILED, 1f);
			base.Fsm.Event(this.FailEvent);
			tk2dSprite component = (ResourceCache.Acquire("Global VFX/GunslingKing_DefeatIcon") as GameObject).GetComponent<tk2dSprite>();
			GameUIRoot.Instance.notificationController.DoCustomNotification(StringTableManager.GetString("#GUNKING_FAIL_HEADER"), StringTableManager.GetString("#GUNKING_FAIL_BODY"), component.Collection, component.spriteId, UINotificationController.NotificationColor.SILVER, false, false);
			base.Finish();
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x0015F2CC File Offset: 0x0015D4CC
		private void GetRidOfSuppliedGun()
		{
			if (this.GunToUsePrefab != null && this.GunToUsePrefab.encounterTrackable)
			{
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					PlayerController playerController = GameManager.Instance.AllPlayers[i];
					for (int j = 0; j < playerController.inventory.AllGuns.Count; j++)
					{
						Gun gun = playerController.inventory.AllGuns[j];
						if (gun && gun.encounterTrackable && gun.IsMinusOneGun && gun.encounterTrackable.journalData.GetPrimaryDisplayName(false) == this.GunToUsePrefab.encounterTrackable.journalData.GetPrimaryDisplayName(false))
						{
							playerController.inventory.DestroyGun(gun);
							break;
						}
					}
				}
				this.GunToUse = null;
				this.GunToUsePrefab = null;
			}
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x0015F3D4 File Offset: 0x0015D5D4
		private void HandlePlayerFiredBeam(BeamController obj)
		{
			if (this.gunId == -1)
			{
				this.gunId = base.FindActionOfType<SelectGunslingGun>().SelectedObject.GetComponent<PickupObject>().PickupObjectId;
			}
			if (obj && obj.Gun)
			{
				if (obj.Gun.CurrentOwner is PlayerController)
				{
					PlayerController playerController = obj.Gun.CurrentOwner as PlayerController;
					if (playerController.CurrentRoom != this.ChallengeRoom)
					{
						return;
					}
				}
				if (obj.Gun.PickupObjectId != this.gunId)
				{
					this.Fail();
				}
			}
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x0015F478 File Offset: 0x0015D678
		private void HandlePlayerFiredProjectile(Projectile obj, float effectChanceScalar)
		{
			if (this.gunId == -1)
			{
				this.gunId = base.FindActionOfType<SelectGunslingGun>().SelectedObject.GetComponent<PickupObject>().PickupObjectId;
			}
			if (obj.Owner is PlayerController)
			{
				PlayerController playerController = obj.Owner as PlayerController;
				if (playerController.CurrentRoom != this.ChallengeRoom)
				{
					return;
				}
			}
			if (obj.Owner.CurrentGun.PickupObjectId != this.gunId)
			{
				this.Fail();
			}
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x0015F4FC File Offset: 0x0015D6FC
		public override void OnExit()
		{
			base.OnExit();
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (this.ChallengeType == GunslingChallengeType.NO_DAMAGE)
				{
					GameManager.Instance.AllPlayers[i].healthHaver.OnDamaged -= this.HandlePlayerDamagedFailed;
				}
				if (this.ChallengeType == GunslingChallengeType.SPECIFIC_GUN)
				{
					GameManager.Instance.AllPlayers[i].PostProcessProjectile -= this.HandlePlayerFiredProjectile;
					GameManager.Instance.AllPlayers[i].PostProcessBeam -= this.HandlePlayerFiredBeam;
				}
			}
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x0015F5A0 File Offset: 0x0015D7A0
		private void HandlePlayerDamagedFailed(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
		{
			if (!GameManager.HasInstance)
			{
				return;
			}
			PlayerController playerController = GameManager.Instance.PrimaryPlayer;
			if (playerController && playerController.healthHaver.IsAlive && playerController.CurrentRoom == this.m_challengeRoom)
			{
				this.Fail();
			}
			playerController = GameManager.Instance.SecondaryPlayer;
			if (playerController && playerController.healthHaver.IsAlive && playerController.CurrentRoom == this.m_challengeRoom)
			{
				this.Fail();
			}
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x0015F634 File Offset: 0x0015D834
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (this.ChallengeType == GunslingChallengeType.NO_DODGE_ROLL && this.m_success)
			{
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					if (GameManager.Instance.AllPlayers[i].CurrentRoom == this.m_challengeRoom && GameManager.Instance.AllPlayers[i].IsDodgeRolling)
					{
						this.Fail();
					}
				}
			}
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x0015F6B4 File Offset: 0x0015D8B4
		private void InformManservantSuccess()
		{
			List<TalkDoerLite> componentsAbsoluteInRoom = this.m_talkDoer.GetAbsoluteParentRoom().GetComponentsAbsoluteInRoom<TalkDoerLite>();
			for (int i = 0; i < componentsAbsoluteInRoom.Count; i++)
			{
				if (!(componentsAbsoluteInRoom[i] == this.m_talkDoer))
				{
					for (int j = 0; j < componentsAbsoluteInRoom[i].playmakerFsms.Length; j++)
					{
						if (componentsAbsoluteInRoom[i].playmakerFsms[j].FsmName.Contains("Dungeon"))
						{
							componentsAbsoluteInRoom[i].playmakerFsms[j].FsmVariables.FindFsmString("currentMode").Value = "modeQuest";
						}
					}
				}
			}
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x0015F770 File Offset: 0x0015D970
		public override void OnLateUpdate()
		{
			base.OnLateUpdate();
			if (!this.m_challengeRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
			{
				if (this.m_success)
				{
					this.Succeed();
				}
				else
				{
					this.Fail();
				}
				base.Finish();
			}
		}

		// Token: 0x04003610 RID: 13840
		public GunslingChallengeType ChallengeType;

		// Token: 0x04003611 RID: 13841
		public Gun GunToUsePrefab;

		// Token: 0x04003612 RID: 13842
		public Gun GunToUse;

		// Token: 0x04003613 RID: 13843
		public FsmEvent SuccessEvent;

		// Token: 0x04003614 RID: 13844
		public FsmEvent FailEvent;

		// Token: 0x04003615 RID: 13845
		private RoomHandler m_challengeRoom;

		// Token: 0x04003616 RID: 13846
		private TalkDoerLite m_talkDoer;

		// Token: 0x04003617 RID: 13847
		private bool m_success = true;

		// Token: 0x04003618 RID: 13848
		private GameObject m_extantIcon;

		// Token: 0x04003619 RID: 13849
		private bool m_hasAlreadyRegisteredIcon;

		// Token: 0x0400361A RID: 13850
		private bool m_hasSucceeded;

		// Token: 0x0400361B RID: 13851
		private int gunId = -1;
	}
}
