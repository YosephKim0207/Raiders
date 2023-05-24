using System;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C6C RID: 3180
	public class SherpaDetectItem : FsmStateAction
	{
		// Token: 0x0600445B RID: 17499 RVA: 0x001613E8 File Offset: 0x0015F5E8
		public override void Reset()
		{
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x001613EC File Offset: 0x0015F5EC
		public override string ErrorCheck()
		{
			return string.Empty;
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x00161400 File Offset: 0x0015F600
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			this.talkingPlayer = component.TalkingPlayer;
			this.DoCheck();
			base.Finish();
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x00161434 File Offset: 0x0015F634
		private bool CheckPlayerForItem(PickupObject targetItem, List<PickupObject> targets)
		{
			bool flag = false;
			if (targetItem is Gun)
			{
				for (int i = 0; i < this.talkingPlayer.inventory.AllGuns.Count; i++)
				{
					if (this.talkingPlayer.inventory.AllGuns[i].PickupObjectId == targetItem.PickupObjectId)
					{
						flag = true;
						targets.Add(this.talkingPlayer.inventory.AllGuns[i]);
					}
				}
			}
			else if (targetItem is PlayerItem)
			{
				for (int j = 0; j < this.talkingPlayer.activeItems.Count; j++)
				{
					if (this.talkingPlayer.activeItems[j].PickupObjectId == targetItem.PickupObjectId)
					{
						flag = true;
						targets.Add(this.talkingPlayer.activeItems[j]);
					}
				}
			}
			else if (targetItem is PassiveItem)
			{
				for (int k = 0; k < this.talkingPlayer.passiveItems.Count; k++)
				{
					if (this.talkingPlayer.passiveItems[k].PickupObjectId == targetItem.PickupObjectId)
					{
						flag = true;
						targets.Add(this.talkingPlayer.passiveItems[k]);
					}
				}
			}
			if (this.numToTake.Value > 1 && this.numToTake.Value > targets.Count)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x001615B8 File Offset: 0x0015F7B8
		private bool FindFlight(List<PickupObject> fliers)
		{
			for (int i = 0; i < this.talkingPlayer.activeItems.Count; i++)
			{
				PlayerItem playerItem = this.talkingPlayer.activeItems[i];
				bool flag = false;
				if (playerItem is JetpackItem)
				{
					flag = true;
				}
				if (flag)
				{
					fliers.Add(playerItem);
				}
			}
			for (int j = 0; j < this.talkingPlayer.passiveItems.Count; j++)
			{
				PassiveItem passiveItem = this.talkingPlayer.passiveItems[j];
				bool flag2 = false;
				if (passiveItem is WingsItem)
				{
					flag2 = true;
				}
				if (flag2)
				{
					fliers.Add(passiveItem);
				}
			}
			return fliers.Count > 0;
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x00161674 File Offset: 0x0015F874
		private bool FindGoopers(List<PickupObject> goopers)
		{
			for (int i = 0; i < this.talkingPlayer.inventory.AllGuns.Count; i++)
			{
				Gun gun = this.talkingPlayer.inventory.AllGuns[i];
				bool flag = false;
				for (int j = 0; j < gun.Volley.projectiles.Count; j++)
				{
					ProjectileModule projectileModule = gun.Volley.projectiles[j];
					for (int k = 0; k < projectileModule.projectiles.Count; k++)
					{
						if (projectileModule.projectiles[k].GetComponent<GoopModifier>() != null)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					goopers.Add(gun);
				}
			}
			for (int l = 0; l < this.talkingPlayer.activeItems.Count; l++)
			{
				PlayerItem playerItem = this.talkingPlayer.activeItems[l];
				bool flag2 = false;
				if (playerItem is SpawnObjectPlayerItem)
				{
					GameObject objectToSpawn = (playerItem as SpawnObjectPlayerItem).objectToSpawn;
					if (objectToSpawn.GetComponent<ThrownGoopItem>())
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					goopers.Add(playerItem);
				}
			}
			for (int m = 0; m < this.talkingPlayer.passiveItems.Count; m++)
			{
				PassiveItem passiveItem = this.talkingPlayer.passiveItems[m];
				bool flag3 = false;
				if (passiveItem is PassiveGooperItem)
				{
					flag3 = true;
				}
				if (flag3)
				{
					goopers.Add(passiveItem);
				}
			}
			return goopers.Count > 0;
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x00161830 File Offset: 0x0015FA30
		private bool FindExplosives(List<PickupObject> explosives)
		{
			for (int i = 0; i < this.talkingPlayer.inventory.AllGuns.Count; i++)
			{
				Gun gun = this.talkingPlayer.inventory.AllGuns[i];
				bool flag = false;
				for (int j = 0; j < gun.Volley.projectiles.Count; j++)
				{
					ProjectileModule projectileModule = gun.Volley.projectiles[j];
					for (int k = 0; k < projectileModule.projectiles.Count; k++)
					{
						if (projectileModule.projectiles[k].GetComponent<ExplosiveModifier>() != null)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					explosives.Add(gun);
				}
			}
			for (int l = 0; l < this.talkingPlayer.activeItems.Count; l++)
			{
				PlayerItem playerItem = this.talkingPlayer.activeItems[l];
				bool flag2 = false;
				if (playerItem is SpawnObjectPlayerItem || playerItem is RemoteMineItem)
				{
					GameObject gameObject = ((!(playerItem is SpawnObjectPlayerItem)) ? (playerItem as RemoteMineItem).objectToSpawn : (playerItem as SpawnObjectPlayerItem).objectToSpawn);
					if (gameObject.GetComponent<RemoteMineController>() || gameObject.GetComponent<ProximityMine>())
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					explosives.Add(playerItem);
				}
			}
			return explosives.Count > 0;
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x001619CC File Offset: 0x0015FBCC
		private void DoCheck()
		{
			bool flag = false;
			this.AllValidTargets = new List<PickupObject>();
			switch (this.detectType)
			{
			case SherpaDetectItem.DetectType.SPECIFIC_ITEM:
				flag = this.CheckPlayerForItem(PickupObjectDatabase.Instance.InternalGetById(this.pickupId.Value), this.AllValidTargets);
				break;
			case SherpaDetectItem.DetectType.SOMETHING_EXPLOSIVE:
				flag = this.FindExplosives(this.AllValidTargets);
				break;
			case SherpaDetectItem.DetectType.SOMETHING_GOOPY:
				flag = this.FindGoopers(this.AllValidTargets);
				break;
			case SherpaDetectItem.DetectType.SOMETHING_FLYING:
				flag = this.FindFlight(this.AllValidTargets);
				break;
			}
			if (flag)
			{
				base.Fsm.Event(this.SuccessEvent);
				base.Finish();
			}
			else
			{
				base.Fsm.Event(this.FailEvent);
				base.Finish();
			}
		}

		// Token: 0x04003669 RID: 13929
		public SherpaDetectItem.DetectType detectType;

		// Token: 0x0400366A RID: 13930
		[Tooltip("Specific item id to check for.")]
		public FsmInt pickupId;

		// Token: 0x0400366B RID: 13931
		public FsmInt numToTake = 1;

		// Token: 0x0400366C RID: 13932
		[Tooltip("The event to send if the preceeding tests all pass.")]
		public FsmEvent SuccessEvent;

		// Token: 0x0400366D RID: 13933
		[Tooltip("The event to send if the preceeding tests all fail.")]
		public FsmEvent FailEvent;

		// Token: 0x0400366E RID: 13934
		[NonSerialized]
		public List<PickupObject> AllValidTargets;

		// Token: 0x0400366F RID: 13935
		private PlayerController talkingPlayer;

		// Token: 0x02000C6D RID: 3181
		public enum DetectType
		{
			// Token: 0x04003671 RID: 13937
			SPECIFIC_ITEM,
			// Token: 0x04003672 RID: 13938
			SOMETHING_EXPLOSIVE,
			// Token: 0x04003673 RID: 13939
			SOMETHING_GOOPY,
			// Token: 0x04003674 RID: 13940
			SOMETHING_FLYING
		}
	}
}
