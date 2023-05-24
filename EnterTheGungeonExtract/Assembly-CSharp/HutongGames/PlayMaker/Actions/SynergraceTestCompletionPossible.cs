using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C72 RID: 3186
	[Tooltip("Sends Events based on synergy completion possibility.")]
	[ActionCategory(".Brave")]
	public class SynergraceTestCompletionPossible : BraveFsmStateAction
	{
		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06004473 RID: 17523 RVA: 0x00161F00 File Offset: 0x00160100
		public bool Success
		{
			get
			{
				return this.m_success;
			}
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x00161F08 File Offset: 0x00160108
		public override void Reset()
		{
			this.successType = SynergraceTestCompletionPossible.SuccessType.SetMode;
			this.Event = null;
			this.mode = string.Empty;
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x00161F28 File Offset: 0x00160128
		public override void OnEnter()
		{
			this.DoCheck();
			if (!this.everyFrame.Value)
			{
				base.Finish();
			}
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x00161F48 File Offset: 0x00160148
		public override void OnUpdate()
		{
			this.DoCheck();
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x00161F50 File Offset: 0x00160150
		private void DoCheck()
		{
			this.m_success = false;
			GenericLootTable genericLootTable = ((UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable);
			GenericLootTable genericLootTable2 = ((!(genericLootTable == GameManager.Instance.RewardManager.GunsLootTable)) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable);
			SynercacheManager.UseCachedSynergyIDs = true;
			GameObject gameObject = GameManager.Instance.RewardManager.GetItemForPlayer(GameManager.Instance.BestActivePlayer, genericLootTable, PickupObject.ItemQuality.A, null, false, null, false, null, true, RewardManager.RewardSource.UNSPECIFIED);
			if (gameObject)
			{
				PickupObject component = gameObject.GetComponent<PickupObject>();
				bool flag = false;
				if (!component || !RewardManager.AnyPlayerHasItemInSynergyContainingOtherItem(component, ref flag))
				{
					gameObject = null;
				}
			}
			if (!gameObject)
			{
				gameObject = GameManager.Instance.RewardManager.GetItemForPlayer(GameManager.Instance.BestActivePlayer, genericLootTable2, PickupObject.ItemQuality.A, null, false, null, false, null, true, RewardManager.RewardSource.UNSPECIFIED);
			}
			if (gameObject)
			{
				PickupObject component2 = gameObject.GetComponent<PickupObject>();
				bool flag2 = false;
				if (component2 && RewardManager.AnyPlayerHasItemInSynergyContainingOtherItem(component2, ref flag2))
				{
					this.m_success = true;
					this.SelectedPickupGameObject = gameObject;
				}
			}
			SynercacheManager.UseCachedSynergyIDs = false;
			if (this.m_success)
			{
				if (this.successType == SynergraceTestCompletionPossible.SuccessType.SendEvent)
				{
					base.Fsm.Event(this.Event);
				}
				else if (this.successType == SynergraceTestCompletionPossible.SuccessType.SetMode)
				{
					FsmString fsmString = base.Fsm.Variables.GetFsmString("currentMode");
					fsmString.Value = this.mode.Value;
				}
				base.Finish();
			}
		}

		// Token: 0x0400367D RID: 13949
		public SynergraceTestCompletionPossible.SuccessType successType;

		// Token: 0x0400367E RID: 13950
		[Tooltip("The event to send if the proceeding tests all pass.")]
		public new FsmEvent Event;

		// Token: 0x0400367F RID: 13951
		[Tooltip("The name of the mode to set 'currentMode' to if the proceeding tests all pass.")]
		public FsmString mode;

		// Token: 0x04003680 RID: 13952
		public FsmBool everyFrame;

		// Token: 0x04003681 RID: 13953
		[NonSerialized]
		public GameObject SelectedPickupGameObject;

		// Token: 0x04003682 RID: 13954
		private bool m_success;

		// Token: 0x02000C73 RID: 3187
		public enum SuccessType
		{
			// Token: 0x04003684 RID: 13956
			SetMode,
			// Token: 0x04003685 RID: 13957
			SendEvent
		}
	}
}
