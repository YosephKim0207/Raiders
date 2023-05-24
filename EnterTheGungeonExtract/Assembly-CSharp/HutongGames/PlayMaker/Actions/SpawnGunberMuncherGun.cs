using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CB9 RID: 3257
	[Tooltip("Spawns a pickup (gun or item) in the world or gives it directly to the player.")]
	[ActionCategory(".NPCs")]
	public class SpawnGunberMuncherGun : FsmStateAction
	{
		// Token: 0x0600455F RID: 17759 RVA: 0x00167B48 File Offset: 0x00165D48
		public override void Reset()
		{
			this.spawnLocation = SpawnGunberMuncherGun.SpawnLocation.GiveToPlayer;
			this.spawnOffset = Vector2.zero;
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x00167B5C File Offset: 0x00165D5C
		public override string ErrorCheck()
		{
			return string.Empty;
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x00167B70 File Offset: 0x00165D70
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			GameObject gameObject = null;
			PlayerController playerController = ((!component.TalkingPlayer) ? GameManager.Instance.PrimaryPlayer : component.TalkingPlayer);
			if (this.spawnLocation == SpawnGunberMuncherGun.SpawnLocation.GiveToPlayer)
			{
				LootEngine.TryGivePrefabToPlayer(gameObject, playerController, false);
			}
			else
			{
				Vector2 vector;
				if (this.spawnLocation == SpawnGunberMuncherGun.SpawnLocation.AtPlayer || this.spawnLocation == SpawnGunberMuncherGun.SpawnLocation.OffsetFromPlayer)
				{
					vector = playerController.specRigidbody.UnitCenter;
				}
				else if (this.spawnLocation == SpawnGunberMuncherGun.SpawnLocation.AtTalkDoer || this.spawnLocation == SpawnGunberMuncherGun.SpawnLocation.OffsetFromTalkDoer)
				{
					vector = ((!(component.specRigidbody != null)) ? component.sprite.WorldCenter : component.specRigidbody.UnitCenter);
				}
				else
				{
					Debug.LogError("Tried to give an item to the player but no valid spawn location was selected.");
					vector = GameManager.Instance.PrimaryPlayer.CenterPosition;
				}
				if (this.spawnLocation == SpawnGunberMuncherGun.SpawnLocation.OffsetFromPlayer || this.spawnLocation == SpawnGunberMuncherGun.SpawnLocation.OffsetFromTalkDoer)
				{
					vector += this.spawnOffset;
				}
				LootEngine.SpewLoot(gameObject, vector);
			}
			base.Finish();
		}

		// Token: 0x0400378F RID: 14223
		[Tooltip("Where to spawn the item at.")]
		public SpawnGunberMuncherGun.SpawnLocation spawnLocation;

		// Token: 0x04003790 RID: 14224
		[Tooltip("Offset from the TalkDoer to spawn the item at.")]
		public Vector2 spawnOffset;

		// Token: 0x04003791 RID: 14225
		[NonSerialized]
		public Gun firstRecycledGun;

		// Token: 0x04003792 RID: 14226
		[NonSerialized]
		public Gun secondRecycledGun;

		// Token: 0x02000CBA RID: 3258
		public enum SpawnLocation
		{
			// Token: 0x04003794 RID: 14228
			GiveToPlayer,
			// Token: 0x04003795 RID: 14229
			AtPlayer,
			// Token: 0x04003796 RID: 14230
			AtTalkDoer,
			// Token: 0x04003797 RID: 14231
			OffsetFromPlayer,
			// Token: 0x04003798 RID: 14232
			OffsetFromTalkDoer
		}
	}
}
