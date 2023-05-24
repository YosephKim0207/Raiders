using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020011BC RID: 4540
public class MysteryMimicManController : MonoBehaviour
{
	// Token: 0x06006548 RID: 25928 RVA: 0x00276834 File Offset: 0x00274A34
	private IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		if (!PassiveItem.IsFlagSetAtAll(typeof(MimicToothNecklaceItem)))
		{
			RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
			absoluteRoom.DeregisterInteractable(base.GetComponent<TalkDoerLite>());
			base.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		yield break;
	}
}
