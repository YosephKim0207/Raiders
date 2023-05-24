using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020011A5 RID: 4517
public class MetalGearRatRoomController : MonoBehaviour
{
	// Token: 0x06006493 RID: 25747 RVA: 0x0026FE90 File Offset: 0x0026E090
	public IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		RoomHandler thisRoom = base.transform.position.GetAbsoluteRoom();
		RoomHandler targetRoom = null;
		for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
		{
			Transform hierarchyParent = GameManager.Instance.Dungeon.data.rooms[i].hierarchyParent;
			if (hierarchyParent && hierarchyParent.GetComponentInChildren<ResourcefulRatRewardRoomController>(true))
			{
				targetRoom = GameManager.Instance.Dungeon.data.rooms[i];
				break;
			}
		}
		thisRoom.TargetPitfallRoom = targetRoom;
		thisRoom.ForcePitfallForFliers = true;
		RoomHandler roomHandler = thisRoom;
		roomHandler.OnTargetPitfallRoom = (Action)Delegate.Combine(roomHandler.OnTargetPitfallRoom, new Action(this.HandlePitfallIntoReward));
		this.EnablePitfalls(false);
		yield break;
	}

	// Token: 0x06006494 RID: 25748 RVA: 0x0026FEAC File Offset: 0x0026E0AC
	private void HandlePitfallIntoReward()
	{
		GameManager.Instance.Dungeon.StartCoroutine(this.HandlePitfallIntoRewardCR());
	}

	// Token: 0x06006495 RID: 25749 RVA: 0x0026FEC4 File Offset: 0x0026E0C4
	private IEnumerator HandlePitfallIntoRewardCR()
	{
		int numPlayers = GameManager.Instance.AllPlayers.Length;
		for (int i = 0; i < numPlayers; i++)
		{
			GameManager.Instance.AllPlayers[i].SetInputOverride("lich transition");
		}
		Pixelator.Instance.FadeToBlack(1f, true, 0f);
		foreach (PlayerController playerController in GameManager.Instance.AllPlayers)
		{
			playerController.DoSpinfallSpawn(0.5f);
			playerController.WarpFollowersToPlayer(false);
		}
		float timer = 0f;
		float duration = 2f;
		while (timer < duration)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
		}
		if (GameManager.HasInstance)
		{
			for (int k = 0; k < numPlayers; k++)
			{
				GameManager.Instance.AllPlayers[k].ClearInputOverride("lich transition");
			}
		}
		yield break;
	}

	// Token: 0x06006496 RID: 25750 RVA: 0x0026FED8 File Offset: 0x0026E0D8
	public void EnablePitfalls(bool value)
	{
		this.floorCover.SetActive(!value);
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		IntVector2 intVector = absoluteRoom.area.basePosition + new IntVector2(19, 12);
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				IntVector2 intVector2 = intVector + new IntVector2(i, j);
				GameManager.Instance.Dungeon.data[intVector2].fallingPrevented = !value;
			}
		}
	}

	// Token: 0x06006497 RID: 25751 RVA: 0x0026FF74 File Offset: 0x0026E174
	public void TransformToDestroyedRoom()
	{
		this.brokenMetalGear.SetActive(true);
		this.EnablePitfalls(true);
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		if (absoluteRoom != null && absoluteRoom.DarkSoulsRoomResetDependencies != null)
		{
			absoluteRoom.DarkSoulsRoomResetDependencies.Clear();
		}
	}

	// Token: 0x04006035 RID: 24629
	public GameObject brokenMetalGear;

	// Token: 0x04006036 RID: 24630
	public GameObject floorCover;
}
