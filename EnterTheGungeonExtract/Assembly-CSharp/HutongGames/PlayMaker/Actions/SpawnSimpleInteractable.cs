using System;
using Dungeonator;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CBF RID: 3263
	[ActionCategory(ActionCategory.Events)]
	public class SpawnSimpleInteractable : FsmStateAction
	{
		// Token: 0x06004569 RID: 17769 RVA: 0x0016807C File Offset: 0x0016627C
		public override void Reset()
		{
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x00168080 File Offset: 0x00166280
		public override void OnEnter()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ThingToSpawn, base.Owner.transform.position, Quaternion.identity);
			IPlayerInteractable[] interfaces = gameObject.GetInterfaces<IPlayerInteractable>();
			IPlaceConfigurable[] interfaces2 = gameObject.GetInterfaces<IPlaceConfigurable>();
			RoomHandler absoluteRoom = base.Owner.transform.position.GetAbsoluteRoom();
			for (int i = 0; i < interfaces.Length; i++)
			{
				absoluteRoom.RegisterInteractable(interfaces[i]);
			}
			for (int j = 0; j < interfaces2.Length; j++)
			{
				interfaces2[j].ConfigureOnPlacement(absoluteRoom);
			}
			base.Finish();
		}

		// Token: 0x040037AA RID: 14250
		public GameObject ThingToSpawn;
	}
}
