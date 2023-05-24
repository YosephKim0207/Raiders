using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CD6 RID: 3286
	[ActionCategory(".NPCs")]
	public class ThievingRatGrabby : FsmStateAction
	{
		// Token: 0x060045CA RID: 17866 RVA: 0x0016A258 File Offset: 0x00168458
		public override void Awake()
		{
			base.Awake();
			this.m_talkDoer = base.Owner.GetComponent<TalkDoerLite>();
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x0016A274 File Offset: 0x00168474
		public override void OnEnter()
		{
			this.m_lastPosition = this.m_talkDoer.specRigidbody.UnitCenter;
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x0016A28C File Offset: 0x0016848C
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (this.m_talkDoer.CurrentPath != null)
			{
				if (!this.m_talkDoer.CurrentPath.WillReachFinalGoal)
				{
					this.m_talkDoer.transform.position = this.TargetObject.sprite.WorldCenter + new Vector2(0f, 1f);
					this.m_talkDoer.specRigidbody.Reinitialize();
					PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(this.m_talkDoer.specRigidbody, new int?(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider)), false);
					this.m_talkDoer.talkDoer.CurrentPath = null;
				}
				else
				{
					this.m_talkDoer.specRigidbody.Velocity = this.m_talkDoer.GetPathVelocityContribution(this.m_lastPosition, 32);
					this.m_lastPosition = this.m_talkDoer.specRigidbody.UnitCenter;
				}
			}
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x0016A380 File Offset: 0x00168580
		private IEnumerator HandleGrabby()
		{
			this.m_grabby = true;
			this.m_talkDoer.aiAnimator.PlayUntilFinished("laugh", false, null, -1f, false);
			string targetItemName = ((!this.TargetObject) ? string.Empty : ((!this.TargetObject.encounterTrackable) ? this.TargetObject.DisplayName : this.TargetObject.encounterTrackable.GetModifiedDisplayName()));
			yield return new WaitForSeconds(2f);
			this.m_talkDoer.aiAnimator.PlayUntilFinished("grabby", true, null, -1f, false);
			yield return new WaitForSeconds(0.55f);
			if (base.Fsm.ActiveState != base.State)
			{
				yield break;
			}
			base.Fsm.SuppressGlobalTransitions = true;
			if (this.TargetObject && this.TargetObject.GetComponentInParent<PlayerController>())
			{
				yield break;
			}
			if (this.TargetObject is IPlayerInteractable)
			{
				RoomHandler.unassignedInteractableObjects.Remove(this.TargetObject as IPlayerInteractable);
			}
			float elapsed = 0f;
			float duration = 0.25f;
			if (this.TargetObject && this.TargetObject.transform)
			{
				Vector3 startPosition = this.TargetObject.transform.position;
				while (elapsed < duration)
				{
					elapsed += BraveTime.DeltaTime;
					if (this.TargetObject && this.TargetObject.transform && this.TargetObject.sprite != null)
					{
						this.TargetObject.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f), elapsed / duration);
						this.TargetObject.transform.position = Vector3.Lerp(startPosition, this.m_talkDoer.transform.position + new Vector3(0.4375f, 0.375f, 0f), elapsed / duration);
					}
					yield return null;
				}
				if (PassiveItem.IsFlagSetAtAll(typeof(RingOfResourcefulRatItem)))
				{
					PickupObject.ItemQuality quality = this.TargetObject.quality;
					if (quality != PickupObject.ItemQuality.COMMON && quality != PickupObject.ItemQuality.SPECIAL && quality != PickupObject.ItemQuality.EXCLUDED)
					{
						PickupObject pickupObject = null;
						if (this.TargetObject is Gun)
						{
							pickupObject = LootEngine.GetItemOfTypeAndQuality<Gun>(quality, GameManager.Instance.RewardManager.GunsLootTable, false);
						}
						else if (this.TargetObject is PassiveItem)
						{
							pickupObject = LootEngine.GetItemOfTypeAndQuality<PassiveItem>(quality, GameManager.Instance.RewardManager.ItemsLootTable, false);
						}
						else if (this.TargetObject is PlayerItem)
						{
							pickupObject = LootEngine.GetItemOfTypeAndQuality<PlayerItem>(quality, GameManager.Instance.RewardManager.ItemsLootTable, false);
						}
						if (pickupObject)
						{
							DebrisObject debrisObject = LootEngine.SpawnItem(pickupObject.gameObject, startPosition, Vector2.up, 0f, true, false, false);
							PickupObject componentInChildren = debrisObject.GetComponentInChildren<PickupObject>();
							if (componentInChildren && !componentInChildren.IgnoredByRat)
							{
								componentInChildren.ClearIgnoredByRatFlagOnPickup = true;
								componentInChildren.IgnoredByRat = true;
							}
							for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
							{
								PassiveItem.DecrementFlag(GameManager.Instance.AllPlayers[i], typeof(RingOfResourcefulRatItem));
							}
						}
					}
				}
				if (this.TargetObject is Gun)
				{
					(this.TargetObject as Gun).GetRidOfMinimapIcon();
				}
				if (this.TargetObject is PlayerItem)
				{
					(this.TargetObject as PlayerItem).GetRidOfMinimapIcon();
				}
				if (this.TargetObject is PassiveItem)
				{
					(this.TargetObject as PassiveItem).GetRidOfMinimapIcon();
				}
				if (this.TargetObject is Gun && this.TargetObject.transform.parent != null)
				{
					UnityEngine.Object.Destroy(this.TargetObject.transform.parent.gameObject);
				}
				else
				{
					UnityEngine.Object.Destroy(this.TargetObject.gameObject);
				}
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.ITEMS_TAKEN_BY_RAT, 1f);
				yield return new WaitForSeconds(0.9f);
			}
			this.m_talkDoer.aiAnimator.PlayUntilFinished("grab_laugh", true, null, -1f, false);
			yield return new WaitForSeconds(1f);
			if (this.m_talkDoer)
			{
				NoteDoer component = this.notePrefab.InstantiateObject(this.m_talkDoer.GetAbsoluteParentRoom(), this.m_talkDoer.transform.position.IntXY(VectorConversions.Floor) - this.m_talkDoer.GetAbsoluteParentRoom().area.basePosition, false).GetComponent<NoteDoer>();
				component.stringKey = StringTableManager.GetLongString("#RESRAT_NOTE_ITEM").Replace("%ITEM", targetItemName);
				this.m_talkDoer.GetAbsoluteParentRoom().RegisterInteractable(component);
				component.alreadyLocalized = true;
			}
			base.Fsm.SuppressGlobalTransitions = false;
			base.Finish();
			yield break;
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x0016A39C File Offset: 0x0016859C
		public override void OnLateUpdate()
		{
			if (this.m_talkDoer.CurrentPath != null)
			{
				return;
			}
			if (!this.m_grabby)
			{
				this.m_talkDoer.StartCoroutine(this.HandleGrabby());
			}
		}

		// Token: 0x04003810 RID: 14352
		public PickupObject TargetObject;

		// Token: 0x04003811 RID: 14353
		public NoteDoer notePrefab;

		// Token: 0x04003812 RID: 14354
		private Vector2 m_lastPosition;

		// Token: 0x04003813 RID: 14355
		private TalkDoerLite m_talkDoer;

		// Token: 0x04003814 RID: 14356
		private bool m_grabby;
	}
}
