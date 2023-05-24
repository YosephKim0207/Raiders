using System;
using System.Collections;
using Dungeonator;
using HutongGames.PlayMaker;
using UnityEngine;

// Token: 0x02001135 RID: 4405
public class CrestDoorController : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x0600614A RID: 24906 RVA: 0x002579E4 File Offset: 0x00255BE4
	private IEnumerator Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleGoToCathedral));
		TalkDoerLite cryoButton = base.transform.position.GetAbsoluteRoom().hierarchyParent.GetComponentInChildren<TalkDoerLite>();
		if (cryoButton && cryoButton.name.Contains("CryoButton"))
		{
			TalkDoerLite talkDoerLite = cryoButton;
			talkDoerLite.OnGenericFSMActionA = (Action)Delegate.Combine(talkDoerLite.OnGenericFSMActionA, new Action(this.SwitchToCryoElevator));
			TalkDoerLite talkDoerLite2 = cryoButton;
			talkDoerLite2.OnGenericFSMActionB = (Action)Delegate.Combine(talkDoerLite2.OnGenericFSMActionB, new Action(this.RescindCryoElevator));
			this.m_cryoBool = cryoButton.playmakerFsm.FsmVariables.GetFsmBool("IS_CRYO");
			this.m_normalBool = cryoButton.playmakerFsm.FsmVariables.GetFsmBool("IS_NORMAL");
			this.m_cryoBool.Value = false;
			this.m_normalBool.Value = true;
		}
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		bool hasCrest = false;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (playerController.healthHaver.HasCrest)
			{
				hasCrest = true;
				break;
			}
		}
		if (!hasCrest && cryoButton)
		{
			cryoButton.transform.position.GetAbsoluteRoom().DeregisterInteractable(cryoButton);
			RoomHandler.unassignedInteractableObjects.Remove(cryoButton);
			cryoButton.gameObject.SetActive(false);
			cryoButton.transform.parent.gameObject.SetActive(false);
			SpeculativeRigidbody[] componentsInChildren = cryoButton.gameObject.GetComponentsInChildren<SpeculativeRigidbody>(true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].enabled = false;
			}
		}
		yield break;
	}

	// Token: 0x0600614B RID: 24907 RVA: 0x00257A00 File Offset: 0x00255C00
	private void RescindCryoElevator()
	{
		this.m_cryoBool.Value = false;
		this.m_normalBool.Value = true;
		if (this.cryoAnimator && !string.IsNullOrEmpty(this.cyroDepartAnimation))
		{
			this.cryoAnimator.Play(this.cyroDepartAnimation);
		}
	}

	// Token: 0x0600614C RID: 24908 RVA: 0x00257A58 File Offset: 0x00255C58
	private void SwitchToCryoElevator()
	{
		this.m_cryoBool.Value = true;
		this.m_normalBool.Value = false;
		if (this.cryoAnimator && !string.IsNullOrEmpty(this.cryoArriveAnimation))
		{
			this.cryoAnimator.Play(this.cryoArriveAnimation);
		}
	}

	// Token: 0x0600614D RID: 24909 RVA: 0x00257AB0 File Offset: 0x00255CB0
	private void HandleGoToCathedral(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		if (!this.m_isOpen)
		{
			return;
		}
		if (specRigidbody.gameActor && specRigidbody.gameActor is PlayerController)
		{
			PlayerController playerController = specRigidbody.gameActor as PlayerController;
			if (playerController.IsDodgeRolling)
			{
				this.m_transitionTime = 0f;
				return;
			}
			this.m_transitionTime += BraveTime.DeltaTime;
			if (this.m_transitionTime > 0.5f)
			{
				Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
				playerController.CurrentInputState = PlayerInputState.NoInput;
				specRigidbody.Velocity.x = 0f;
				if (this.m_cryoBool != null && this.m_cryoBool.Value)
				{
					GameUIRoot.Instance.HideCoreUI(string.Empty);
					GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
					AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
					GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.CATHEDRALGEON);
					float num = 0.6f;
					GameManager.Instance.DelayedLoadCharacterSelect(num, true, true);
				}
				else
				{
					GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.CATHEDRALGEON);
					GameManager.Instance.DelayedLoadCustomLevel(0.5f, "tt_cathedral");
				}
			}
		}
	}

	// Token: 0x0600614E RID: 24910 RVA: 0x00257BF0 File Offset: 0x00255DF0
	private void LateUpdate()
	{
		if (this.m_transitionTime == this.m_previousTransitionTime)
		{
			this.m_transitionTime = 0f;
		}
		this.m_previousTransitionTime = this.m_transitionTime;
	}

	// Token: 0x0600614F RID: 24911 RVA: 0x00257C1C File Offset: 0x00255E1C
	private IEnumerator Open()
	{
		this.m_isOpen = true;
		this.CrestSprite.renderer.enabled = true;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (GameManager.Instance.AllPlayers[i] && GameManager.Instance.AllPlayers[i].passiveItems != null)
			{
				for (int j = 0; j < GameManager.Instance.AllPlayers[i].passiveItems.Count; j++)
				{
					if (GameManager.Instance.AllPlayers[i].passiveItems[j] is CathedralCrestItem)
					{
						GameManager.Instance.AllPlayers[i].RemovePassiveItem(GameManager.Instance.AllPlayers[i].passiveItems[j].PickupObjectId);
						break;
					}
				}
			}
		}
		float elapsed = 0f;
		GameManager.Instance.MainCameraController.DoContinuousScreenShake(this.SlideShake, this, false);
		tk2dSpriteAnimator vfxChild = this.SarcoRigidbody.GetComponentInChildren<tk2dSpriteAnimator>();
		vfxChild.renderer.enabled = true;
		vfxChild.PlayAndDisableObject(string.Empty, null);
		while (elapsed < 4f)
		{
			elapsed += BraveTime.DeltaTime;
			this.SarcoRigidbody.Velocity = new Vector2(0f, -0.5f);
			yield return null;
		}
		GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
		this.SarcoRigidbody.Velocity = Vector2.zero;
		yield break;
	}

	// Token: 0x06006150 RID: 24912 RVA: 0x00257C38 File Offset: 0x00255E38
	public float GetDistanceToPoint(Vector2 point)
	{
		Bounds bounds = this.AltarRigidbody.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + this.AltarRigidbody.sprite.transform.position, bounds.max + this.AltarRigidbody.sprite.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x06006151 RID: 24913 RVA: 0x00257D30 File Offset: 0x00255F30
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006152 RID: 24914 RVA: 0x00257D38 File Offset: 0x00255F38
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this || this.m_isOpen)
		{
			return;
		}
		SpriteOutlineManager.AddOutlineToSprite(this.AltarRigidbody.sprite, Color.white);
	}

	// Token: 0x06006153 RID: 24915 RVA: 0x00257D68 File Offset: 0x00255F68
	public void OnExitRange(PlayerController interactor)
	{
		if (!this || this.m_isOpen)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(this.AltarRigidbody.sprite, false);
	}

	// Token: 0x06006154 RID: 24916 RVA: 0x00257D94 File Offset: 0x00255F94
	public void Interact(PlayerController interactor)
	{
		if (!this.m_isOpen)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(this.AltarRigidbody.sprite, false);
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleShrineConversation(interactor));
		}
	}

	// Token: 0x06006155 RID: 24917 RVA: 0x00257DCC File Offset: 0x00255FCC
	private IEnumerator HandleShrineConversation(PlayerController interactor)
	{
		string targetDisplayKey = this.displayTextKey;
		TextBoxManager.ShowStoneTablet(this.talkPoint.position, this.talkPoint, -1f, StringTableManager.GetString(targetDisplayKey), true, false);
		int selectedResponse = -1;
		interactor.SetInputOverride("shrineConversation");
		yield return null;
		bool canUse = interactor.healthHaver.HasCrest;
		if (canUse)
		{
			string @string = StringTableManager.GetString(this.acceptOptionKey);
			GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, @string, StringTableManager.GetString(this.declineOptionKey));
		}
		else
		{
			GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, StringTableManager.GetString(this.declineOptionKey), string.Empty);
		}
		while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
		{
			yield return null;
		}
		interactor.ClearInputOverride("shrineConversation");
		TextBoxManager.ClearTextBox(this.talkPoint);
		if (canUse && selectedResponse == 0)
		{
			GameManager.Instance.Dungeon.StartCoroutine(this.Open());
		}
		yield break;
	}

	// Token: 0x06006156 RID: 24918 RVA: 0x00257DF0 File Offset: 0x00255FF0
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x04005C05 RID: 23557
	public SpeculativeRigidbody AltarRigidbody;

	// Token: 0x04005C06 RID: 23558
	public SpeculativeRigidbody SarcoRigidbody;

	// Token: 0x04005C07 RID: 23559
	public ScreenShakeSettings SlideShake;

	// Token: 0x04005C08 RID: 23560
	public string displayTextKey;

	// Token: 0x04005C09 RID: 23561
	public string acceptOptionKey;

	// Token: 0x04005C0A RID: 23562
	public string declineOptionKey;

	// Token: 0x04005C0B RID: 23563
	public tk2dSprite CrestSprite;

	// Token: 0x04005C0C RID: 23564
	public Transform talkPoint;

	// Token: 0x04005C0D RID: 23565
	public tk2dSpriteAnimator cryoAnimator;

	// Token: 0x04005C0E RID: 23566
	public string cryoArriveAnimation;

	// Token: 0x04005C0F RID: 23567
	public string cyroDepartAnimation;

	// Token: 0x04005C10 RID: 23568
	private bool m_isOpen;

	// Token: 0x04005C11 RID: 23569
	private FsmBool m_cryoBool;

	// Token: 0x04005C12 RID: 23570
	private FsmBool m_normalBool;

	// Token: 0x04005C13 RID: 23571
	private float m_transitionTime;

	// Token: 0x04005C14 RID: 23572
	private float m_previousTransitionTime;
}
