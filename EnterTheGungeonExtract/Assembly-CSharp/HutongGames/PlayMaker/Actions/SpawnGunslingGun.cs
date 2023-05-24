using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CBB RID: 3259
	[ActionCategory(".NPCs")]
	public class SpawnGunslingGun : BraveFsmStateAction
	{
		// Token: 0x06004563 RID: 17763 RVA: 0x00167C98 File Offset: 0x00165E98
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			PlayerController playerController = ((!component.TalkingPlayer) ? GameManager.Instance.PrimaryPlayer : component.TalkingPlayer);
			SelectGunslingGun selectGunslingGun = base.FindActionOfType<SelectGunslingGun>();
			CheckGunslingChallengeComplete checkGunslingChallengeComplete = base.FindActionOfType<CheckGunslingChallengeComplete>();
			if (selectGunslingGun != null)
			{
				GameObject selectedObject = selectGunslingGun.SelectedObject;
				Gun gun = LootEngine.TryGiveGunToPlayer(selectedObject, playerController, false);
				if (gun)
				{
					gun.CanBeDropped = false;
					gun.CanBeSold = false;
					gun.IsMinusOneGun = true;
					if (checkGunslingChallengeComplete != null)
					{
						checkGunslingChallengeComplete.GunToUse = gun;
						checkGunslingChallengeComplete.GunToUsePrefab = selectGunslingGun.SelectedObject.GetComponent<Gun>();
					}
				}
			}
			base.Finish();
		}
	}
}
