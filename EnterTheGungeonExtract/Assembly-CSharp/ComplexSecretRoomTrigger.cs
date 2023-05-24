using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using InControl;
using UnityEngine;

// Token: 0x02001125 RID: 4389
public class ComplexSecretRoomTrigger : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x060060E0 RID: 24800 RVA: 0x00254320 File Offset: 0x00252520
	public void Initialize(RoomHandler room)
	{
		this.parentRoom = room;
		this.parentRoom.RegisterInteractable(this);
		List<PickupObject> list = new List<PickupObject>();
		for (int i = 0; i < this.RequiredObjectIds.Count; i++)
		{
			this.SuppliedObjects.Add(false);
			list.Add(PickupObjectDatabase.GetById(this.RequiredObjectIds[i]));
		}
		GameManager.Instance.Dungeon.data.DistributeComplexSecretPuzzleItems(list, this.parentRoom, false, 0f);
	}

	// Token: 0x060060E1 RID: 24801 RVA: 0x002543A8 File Offset: 0x002525A8
	public float GetDistanceToPoint(Vector2 point)
	{
		return Vector2.Distance(point, base.sprite.WorldCenter);
	}

	// Token: 0x060060E2 RID: 24802 RVA: 0x002543BC File Offset: 0x002525BC
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x060060E3 RID: 24803 RVA: 0x002543C4 File Offset: 0x002525C4
	public void OnEnteredRange(PlayerController interactor)
	{
		if (this.referencedSecretRoom.IsOpen)
		{
			return;
		}
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x060060E4 RID: 24804 RVA: 0x00254400 File Offset: 0x00252600
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
	}

	// Token: 0x060060E5 RID: 24805 RVA: 0x0025441C File Offset: 0x0025261C
	public void Interact(PlayerController interactor)
	{
		if (this.m_isInteracting)
		{
			return;
		}
		this.m_isInteracting = true;
		base.StartCoroutine(this.HandleDialog(interactor));
	}

	// Token: 0x060060E6 RID: 24806 RVA: 0x00254440 File Offset: 0x00252640
	private IEnumerator HandleDialog(PlayerController player)
	{
		string displayString = StringTableManager.GetString(this.introStringKey);
		TextBoxManager.ShowTextBox(this.speakPoint.position + new Vector3(0f, 0f, -5f), this.speakPoint, -1f, displayString, string.Empty, true, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		yield return base.StartCoroutine(this.WaitForPlayer());
		TextBoxManager.ClearTextBox(this.speakPoint);
		for (int i = 0; i < this.RequiredObjectIds.Count; i++)
		{
			if (!this.SuppliedObjects[i])
			{
				for (int j = 0; j < player.additionalItems.Count; j++)
				{
					yield return null;
					PickupObject requiredObject = PickupObjectDatabase.GetById(this.RequiredObjectIds[i]);
					if (player.additionalItems[j].DisplayName == requiredObject.DisplayName)
					{
						TextBoxManager.ShowTextBox(this.speakPoint.position + new Vector3(0f, 0f, -5f), this.speakPoint, -1f, StringTableManager.GetString(this.RequiredItemSupplyKeys[i]), string.Empty, true, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
						yield return base.StartCoroutine(this.WaitForPlayerYesNo(player, i, j));
						break;
					}
				}
			}
		}
		bool allComplete = true;
		for (int k = 0; k < this.SuppliedObjects.Count; k++)
		{
			if (!this.SuppliedObjects[k])
			{
				allComplete = false;
				break;
			}
		}
		if (allComplete)
		{
			this.parentRoom.DeregisterInteractable(this);
			if (!this.referencedSecretRoom.IsOpen)
			{
				this.referencedSecretRoom.OpenDoor();
			}
		}
		this.m_isInteracting = false;
		yield break;
	}

	// Token: 0x060060E7 RID: 24807 RVA: 0x00254464 File Offset: 0x00252664
	private IEnumerator WaitForPlayerYesNo(PlayerController player, int i, int j)
	{
		yield return null;
		int selectedResponse = -1;
		TalkModule yesNoModule = new TalkModule();
		yesNoModule.responses = new List<TalkResponse>();
		TalkResponse yesResponse = new TalkResponse();
		yesResponse.response = "#YES";
		TalkResponse noResponse = new TalkResponse();
		noResponse.response = "#NO";
		yesNoModule.responses.Add(yesResponse);
		yesNoModule.responses.Add(noResponse);
		GameUIRoot.Instance.DisplayPlayerConversationOptions(player, yesNoModule, string.Empty, string.Empty);
		while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
		{
			yield return null;
		}
		TextBoxManager.ClearTextBox(this.speakPoint);
		if (selectedResponse == 0)
		{
			this.SuppliedObjects[i] = true;
			player.UsePuzzleItem(player.additionalItems[j]);
		}
		yield break;
	}

	// Token: 0x060060E8 RID: 24808 RVA: 0x00254494 File Offset: 0x00252694
	private IEnumerator WaitForPlayer()
	{
		yield return null;
		while (!BraveInput.WasSelectPressed(InputManager.ActiveDevice))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060060E9 RID: 24809 RVA: 0x002544A8 File Offset: 0x002526A8
	protected void AttemptSupplyObjects(PlayerController player)
	{
	}

	// Token: 0x060060EA RID: 24810 RVA: 0x002544AC File Offset: 0x002526AC
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060060EB RID: 24811 RVA: 0x002544B8 File Offset: 0x002526B8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005B7F RID: 23423
	public string introStringKey;

	// Token: 0x04005B80 RID: 23424
	[PickupIdentifier]
	public List<int> RequiredObjectIds;

	// Token: 0x04005B81 RID: 23425
	public List<string> RequiredItemSupplyKeys;

	// Token: 0x04005B82 RID: 23426
	public Transform speakPoint;

	// Token: 0x04005B83 RID: 23427
	private List<bool> SuppliedObjects = new List<bool>();

	// Token: 0x04005B84 RID: 23428
	public SecretRoomManager referencedSecretRoom;

	// Token: 0x04005B85 RID: 23429
	public RoomHandler parentRoom;

	// Token: 0x04005B86 RID: 23430
	private bool m_isInteracting;
}
