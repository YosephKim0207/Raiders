using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using InControl;
using UnityEngine;

// Token: 0x02001224 RID: 4644
public class TalkDoer : DungeonPlaceableBehaviour, IPlayerInteractable
{
	// Token: 0x060067C8 RID: 26568 RVA: 0x0028A3F0 File Offset: 0x002885F0
	private void Start()
	{
		EncounterTrackable component = base.GetComponent<EncounterTrackable>();
		if (!string.IsNullOrEmpty(this.FirstMeetingEverModule) && component != null && GameStatsManager.Instance.QueryEncounterable(component) == 0)
		{
			this.defaultModule = this.GetModuleFromName(this.FirstMeetingEverModule);
		}
		else if (!string.IsNullOrEmpty(this.FirstMeetingSessionModule))
		{
			this.defaultModule = this.GetModuleFromName(this.FirstMeetingSessionModule);
		}
		else if (!string.IsNullOrEmpty(this.RepeatMeetingSessionModule))
		{
			this.defaultModule = this.GetModuleFromName(this.RepeatMeetingSessionModule);
		}
		else
		{
			this.defaultModule = this.GetModuleFromName("start");
		}
		if (base.specRigidbody != null)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
		}
		if (base.name.Contains("Truth_Knower"))
		{
			RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			List<Chest> componentsInRoom = roomFromPosition.GetComponentsInRoom<Chest>();
			for (int i = 0; i < componentsInRoom.Count; i++)
			{
				if (componentsInRoom[i].name.ToLowerInvariant().Contains("truth"))
				{
					MajorBreakable majorBreakable = componentsInRoom[i].majorBreakable;
					majorBreakable.OnBreak = (Action)Delegate.Combine(majorBreakable.OnBreak, new Action(this.OnTruthChestBroken));
				}
			}
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
	}

	// Token: 0x060067C9 RID: 26569 RVA: 0x0028A590 File Offset: 0x00288790
	private void Update()
	{
		if (this.m_isTalking && Vector2.Distance(this.talkingPlayer.sprite.WorldCenter, base.sprite.WorldCenter) > this.conversationBreakRadius)
		{
			this.ForceEndConversation();
		}
	}

	// Token: 0x060067CA RID: 26570 RVA: 0x0028A5D0 File Offset: 0x002887D0
	private void OnRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (this.m_isTalking)
		{
			return;
		}
		Projectile component = rigidbodyCollision.OtherRigidbody.GetComponent<Projectile>();
		if (component != null && component.Owner is PlayerController && !this.m_isDealingWithBetrayal)
		{
			this.hitCount++;
			if (this.usesCustomBetrayalLogic)
			{
				if (this.hitCount < 2)
				{
					if (this.OnBetrayalWarning != null)
					{
						this.OnBetrayalWarning();
					}
				}
				else if (this.hitCount < 3 && this.OnBetrayal != null)
				{
					this.OnBetrayal();
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(this.hitAnimName))
				{
					base.spriteAnimator.PlayForDuration(this.hitAnimName, -1f, this.fallbackAnimName, false);
				}
				if (!this.DoesVanish || this.hitCount < 2)
				{
					if (!string.IsNullOrEmpty(this.betrayalSpeechKey))
					{
						base.StartCoroutine(this.HandleBetrayal());
					}
				}
				else
				{
					this.Vanish();
				}
			}
		}
	}

	// Token: 0x060067CB RID: 26571 RVA: 0x0028A6F0 File Offset: 0x002888F0
	private void OnTruthChestBroken()
	{
		base.StartCoroutine(this.HandleBetrayal());
		base.StartCoroutine(this.DelayedVanish(2f));
	}

	// Token: 0x060067CC RID: 26572 RVA: 0x0028A714 File Offset: 0x00288914
	private IEnumerator DelayedVanish(float delay)
	{
		yield return new WaitForSeconds(delay);
		this.Vanish();
		yield break;
	}

	// Token: 0x060067CD RID: 26573 RVA: 0x0028A738 File Offset: 0x00288938
	private void Vanish()
	{
		RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
		roomFromPosition.DeregisterInteractable(this);
		TextBoxManager.ClearTextBox(this.speakPoint);
		if (base.specRigidbody != null)
		{
			base.specRigidbody.enabled = false;
		}
		tk2dSpriteAnimationClip clipByName = base.spriteAnimator.GetClipByName(this.vanishAnimName);
		for (int i = 0; i < this.itemsToLeaveBehind.Count; i++)
		{
			this.itemsToLeaveBehind[i].transform.parent = base.transform.parent;
		}
		if (clipByName != null)
		{
			base.spriteAnimator.PlayAndDestroyObject(this.vanishAnimName, null);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060067CE RID: 26574 RVA: 0x0028A80C File Offset: 0x00288A0C
	private IEnumerator HandleBetrayal()
	{
		this.m_isDealingWithBetrayal = true;
		TextBoxManager.ClearTextBox(this.speakPoint);
		yield return null;
		string displayString = ((!this.betrayalSpeechSequential) ? StringTableManager.GetString(this.betrayalSpeechKey) : StringTableManager.GetStringSequential(this.betrayalSpeechKey, ref this.betrayalSpeechIndex, false));
		if (string.IsNullOrEmpty(this.audioCharacterSpeechTag))
		{
			TextBoxManager.ShowTextBox(this.speakPoint.position + new Vector3(0f, 0f, -5f), this.speakPoint, -1f, displayString, string.Empty, true, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		}
		else
		{
			TextBoxManager.ShowTextBox(this.speakPoint.position + new Vector3(0f, 0f, -5f), this.speakPoint, -1f, displayString, this.audioCharacterSpeechTag, true, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		}
		yield return new WaitForSeconds(4f);
		TextBoxManager.ClearTextBox(this.speakPoint);
		this.m_isDealingWithBetrayal = false;
		yield break;
	}

	// Token: 0x060067CF RID: 26575 RVA: 0x0028A828 File Offset: 0x00288A28
	private TalkModule GetModuleFromName(string ID)
	{
		for (int i = 0; i < this.modules.Count; i++)
		{
			if (this.modules[i].moduleID == ID)
			{
				return this.modules[i];
			}
		}
		return null;
	}

	// Token: 0x060067D0 RID: 26576 RVA: 0x0028A87C File Offset: 0x00288A7C
	private void ProcessResponseAction(TalkResult result)
	{
		TalkResult.TalkResultAction action = result.action;
		switch (action)
		{
		case TalkResult.TalkResultAction.CHANGE_DEFAULT_MODULE:
			this.defaultModule = this.GetModuleFromName(result.actionData);
			break;
		case TalkResult.TalkResultAction.OPEN_TRUTH_CHEST:
			base.StartCoroutine(this.DelayedChestOpen(3f));
			break;
		case TalkResult.TalkResultAction.VANISH:
			base.StartCoroutine(this.DelayedVanish(3f));
			break;
		case TalkResult.TalkResultAction.TOSS_CURRENT_GUN_IN_POT:
		{
			WitchCauldronController component = base.transform.parent.GetComponent<WitchCauldronController>();
			if (component != null)
			{
				component.TossPlayerEquippedGun(this.talkingPlayer);
			}
			break;
		}
		case TalkResult.TalkResultAction.RENDER_SILENT:
			base.StartCoroutine(this.MakeUninteractable(float.Parse(result.actionData)));
			break;
		case TalkResult.TalkResultAction.CHANGE_DEFAULT_MODULE_OF_OTHER_TALKDOER:
			result.objectData.GetComponent<TalkDoer>().defaultModule = result.objectData.GetComponent<TalkDoer>().GetModuleFromName(result.actionData);
			break;
		case TalkResult.TalkResultAction.SPAWN_ITEM:
			LootEngine.SpewLoot(result.objectData, (!(base.specRigidbody != null)) ? base.sprite.WorldCenter : base.specRigidbody.UnitCenter);
			break;
		case TalkResult.TalkResultAction.MAKE_TALKDOER_INTERACTABLE:
			result.objectData.GetComponent<TalkDoer>().OverrideNoninteractable = false;
			break;
		case TalkResult.TalkResultAction.SPAWN_ITEM_FROM_TABLE:
		{
			GameObject gameObject = result.lootTableData.SelectByWeightWithoutDuplicatesFullPrereqs(null, true, false);
			LootEngine.SpewLoot(gameObject, (!(base.specRigidbody != null)) ? base.sprite.WorldCenter : base.specRigidbody.UnitCenter);
			break;
		}
		default:
			if (action == TalkResult.TalkResultAction.CUSTOM_ACTION)
			{
				this.ProcessCustomAction(result.customActionID, result.actionData, result.objectData);
			}
			break;
		}
	}

	// Token: 0x060067D1 RID: 26577 RVA: 0x0028AA44 File Offset: 0x00288C44
	private IEnumerator MakeUninteractable(float duration)
	{
		this.m_uninteractable = true;
		yield return new WaitForSeconds(duration);
		this.m_uninteractable = false;
		yield break;
	}

	// Token: 0x060067D2 RID: 26578 RVA: 0x0028AA68 File Offset: 0x00288C68
	private void OpenTruthChest()
	{
		RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
		List<Chest> componentsInRoom = roomFromPosition.GetComponentsInRoom<Chest>();
		for (int i = 0; i < componentsInRoom.Count; i++)
		{
			if (componentsInRoom[i].name.ToLowerInvariant().Contains("truth"))
			{
				componentsInRoom[i].IsLocked = false;
				componentsInRoom[i].IsSealed = false;
				tk2dSpriteAnimator componentInChildren = componentsInRoom[i].transform.Find("lock").GetComponentInChildren<tk2dSpriteAnimator>();
				if (componentInChildren != null)
				{
					componentInChildren.StopAndResetFrame();
					base.StartCoroutine(this.PlayDelayedTruthChestLockOpen(componentInChildren, 1f));
				}
			}
		}
	}

	// Token: 0x060067D3 RID: 26579 RVA: 0x0028AB30 File Offset: 0x00288D30
	private IEnumerator DelayedChestOpen(float delay)
	{
		this.m_hack_isOpeningTruthChest = true;
		yield return new WaitForSeconds(delay);
		this.OpenTruthChest();
		this.m_hack_isOpeningTruthChest = false;
		yield break;
	}

	// Token: 0x060067D4 RID: 26580 RVA: 0x0028AB54 File Offset: 0x00288D54
	private IEnumerator PlayDelayedTruthChestLockOpen(tk2dSpriteAnimator lockAnimator, float delay)
	{
		yield return new WaitForSeconds(delay);
		lockAnimator.PlayAndDestroyObject("truth_lock_open", null);
		yield break;
	}

	// Token: 0x060067D5 RID: 26581 RVA: 0x0028AB78 File Offset: 0x00288D78
	private void ProcessCustomAction(string customActionID, string actionData, GameObject objectData)
	{
		Debug.LogError("Custom action: " + customActionID + " is not implemented!");
	}

	// Token: 0x060067D6 RID: 26582 RVA: 0x0028AB90 File Offset: 0x00288D90
	private void BeginConversation(PlayerController player)
	{
		this.m_isTalking = true;
		GameUIRoot.Instance.InitializeConversationPortrait(player);
		EncounterTrackable component = base.GetComponent<EncounterTrackable>();
		if (this.numTimesSpokenTo == 0 && component != null)
		{
			GameStatsManager.Instance.HandleEncounteredObject(component);
		}
		this.numTimesSpokenTo++;
		base.StartCoroutine(this.HandleConversationModule(this.defaultModule));
		if ((this.defaultModule.moduleID == this.FirstMeetingSessionModule || this.defaultModule.moduleID == this.FirstMeetingEverModule) && !string.IsNullOrEmpty(this.RepeatMeetingSessionModule))
		{
			this.defaultModule = this.GetModuleFromName(this.RepeatMeetingSessionModule);
		}
	}

	// Token: 0x060067D7 RID: 26583 RVA: 0x0028AC54 File Offset: 0x00288E54
	private void ForceEndConversation()
	{
		TextBoxManager.ClearTextBox(this.speakPoint);
		base.StopAllCoroutines();
		if (this.m_hack_isOpeningTruthChest)
		{
			this.OpenTruthChest();
			this.m_hack_isOpeningTruthChest = false;
		}
		this.EndConversation();
	}

	// Token: 0x060067D8 RID: 26584 RVA: 0x0028AC88 File Offset: 0x00288E88
	private void EndConversation()
	{
		this.m_isTalking = false;
		if (!string.IsNullOrEmpty(this.fallbackAnimName))
		{
			base.spriteAnimator.Play(this.fallbackAnimName);
		}
	}

	// Token: 0x060067D9 RID: 26585 RVA: 0x0028ACB4 File Offset: 0x00288EB4
	public void ForceTimedSpeech(string words, float initialDelay, float duration, TextBoxManager.BoxSlideOrientation slideOrientation)
	{
		Debug.Log("starting forced timed speech: " + words);
		base.StartCoroutine(this.HandleForcedTimedSpeech(words, initialDelay, duration, slideOrientation));
	}

	// Token: 0x060067DA RID: 26586 RVA: 0x0028ACD8 File Offset: 0x00288ED8
	private IEnumerator HandleForcedTimedSpeech(string words, float initialDelay, float duration, TextBoxManager.BoxSlideOrientation slideOrientation)
	{
		this.m_isDoingForcedSpeech = true;
		while (initialDelay > 0f)
		{
			initialDelay -= BraveTime.DeltaTime;
			if (!this.m_isDoingForcedSpeech)
			{
				Debug.Log("breaking forced timed speech: " + words);
				yield break;
			}
			yield return null;
		}
		TextBoxManager.ClearTextBox(this.speakPoint);
		if (!string.IsNullOrEmpty(this.defaultSpeechAnimName))
		{
			base.spriteAnimator.Play(this.defaultSpeechAnimName);
		}
		if (string.IsNullOrEmpty(this.audioCharacterSpeechTag))
		{
			TextBoxManager.ShowTextBox(this.speakPoint.position + new Vector3(0f, 0f, -4f), this.speakPoint, -1f, words, string.Empty, true, slideOrientation, false, false);
		}
		else
		{
			TextBoxManager.ShowTextBox(this.speakPoint.position + new Vector3(0f, 0f, -4f), this.speakPoint, -1f, words, this.audioCharacterSpeechTag, false, slideOrientation, false, false);
		}
		if (duration > 0f)
		{
			while (duration > 0f && this.m_isDoingForcedSpeech)
			{
				duration -= BraveTime.DeltaTime;
				yield return null;
			}
		}
		else
		{
			while (this.m_isDoingForcedSpeech)
			{
				yield return null;
			}
		}
		Debug.Log("ending forced timed speech: " + words);
		TextBoxManager.ClearTextBox(this.speakPoint);
		if (!string.IsNullOrEmpty(this.fallbackAnimName))
		{
			base.spriteAnimator.Play(this.fallbackAnimName);
		}
		this.m_isDoingForcedSpeech = false;
		yield break;
	}

	// Token: 0x060067DB RID: 26587 RVA: 0x0028AD10 File Offset: 0x00288F10
	private IEnumerator HandleConversationModule(TalkModule module)
	{
		int textIndex = 0;
		yield return null;
		if (module.usesAnimation)
		{
			if (!string.IsNullOrEmpty(module.animationName))
			{
				if (module.animationDuration > 0f)
				{
					base.spriteAnimator.PlayForDuration(module.animationName, module.animationDuration, this.fallbackAnimName, false);
				}
				else
				{
					base.spriteAnimator.Play(module.animationName);
				}
			}
			else if (module.animationDuration > 0f)
			{
				base.spriteAnimator.PlayForDuration(this.defaultSpeechAnimName, module.animationDuration, this.fallbackAnimName, false);
			}
			else
			{
				base.spriteAnimator.Play(this.defaultSpeechAnimName);
			}
		}
		string overrideResponseValue = string.Empty;
		string overrideResponseValue2 = string.Empty;
		string overrideFollowupModule = string.Empty;
		string overrideFollowupModule2 = string.Empty;
		while (textIndex < module.stringKeys.Length)
		{
			if (textIndex > 0)
			{
				TextBoxManager.ClearTextBox(this.speakPoint);
				if (module.usesAnimation && !string.IsNullOrEmpty(module.additionalAnimationName))
				{
					base.spriteAnimator.Play(module.additionalAnimationName);
				}
			}
			string stringKey = module.stringKeys[textIndex];
			if (stringKey == "$anim")
			{
				if (module.animationDuration > 0f)
				{
					yield return new WaitForSeconds(module.animationDuration);
				}
				else
				{
					tk2dSpriteAnimationClip clip = base.spriteAnimator.GetClipByName(module.animationName);
					yield return new WaitForSeconds((float)clip.frames.Length / clip.fps);
				}
			}
			else
			{
				string displayString;
				if (module.sequentialStrings)
				{
					displayString = StringTableManager.GetStringSequential(stringKey, ref module.sequentialStringLastIndex, false);
				}
				else
				{
					displayString = StringTableManager.GetString(stringKey);
				}
				if (displayString.Contains("$"))
				{
					string[] array = displayString.Split(new char[] { '$' });
					displayString = array[0];
					overrideResponseValue = array[1];
					overrideResponseValue2 = array[2];
					if (array.Length == 4)
					{
						overrideFollowupModule = "#" + array[3];
						overrideFollowupModule2 = overrideFollowupModule;
					}
					else if (array.Length == 5)
					{
						overrideFollowupModule = "#" + array[3];
						overrideFollowupModule2 = "#" + array[4];
					}
				}
				else if (displayString.Contains("&"))
				{
					string[] array2 = displayString.Split(new char[] { '&' });
					displayString = array2[0];
					if (this.echo1 != null)
					{
						this.echo1.ForceTimedSpeech(array2[1], 1f, 4f, TextBoxManager.BoxSlideOrientation.FORCE_RIGHT);
					}
					if (this.echo2 != null && array2.Length > 2)
					{
						this.echo2.ForceTimedSpeech(array2[2], 2f, 4f, TextBoxManager.BoxSlideOrientation.FORCE_LEFT);
					}
				}
				if (string.IsNullOrEmpty(this.audioCharacterSpeechTag))
				{
					TextBoxManager.ShowTextBox(this.speakPoint.position + new Vector3(0f, 0f, -5f), this.speakPoint, -1f, displayString, string.Empty, true, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
				}
				else
				{
					TextBoxManager.ShowTextBox(this.speakPoint.position + new Vector3(0f, 0f, -5f), this.speakPoint, -1f, displayString, this.audioCharacterSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
				}
				if (module.responses.Count > 0 && textIndex == module.stringKeys.Length - 1)
				{
					yield return base.StartCoroutine(this.WaitForTextRevealed());
					break;
				}
				yield return base.StartCoroutine(this.WaitForPlayer());
			}
			if (this.echo1 != null)
			{
				this.echo1.m_isDoingForcedSpeech = false;
			}
			if (this.echo2 != null)
			{
				this.echo2.m_isDoingForcedSpeech = false;
			}
			textIndex++;
			yield return new WaitForSeconds(0.05f);
		}
		if (module.moduleResultActions.Count > 0)
		{
			for (int i = 0; i < module.moduleResultActions.Count; i++)
			{
				this.ProcessResponseAction(module.moduleResultActions[i]);
			}
		}
		if (module.responses.Count > 0)
		{
			if (module.responses.Count == 1 && string.IsNullOrEmpty(module.responses[0].response))
			{
				if (this.alwaysWaitsForInput)
				{
					yield return base.StartCoroutine(this.WaitForPlayer());
					if (this.echo1 != null)
					{
						this.echo1.m_isDoingForcedSpeech = false;
					}
					if (this.echo2 != null)
					{
						this.echo2.m_isDoingForcedSpeech = false;
					}
				}
				base.StartCoroutine(this.HandleConversationModule(this.GetModuleFromName(module.responses[0].followupModuleID)));
			}
			else
			{
				base.StartCoroutine(this.HandleResponses(module, overrideResponseValue, overrideResponseValue2, overrideFollowupModule, overrideFollowupModule2));
			}
		}
		else if (module.responses.Count == 0 && !string.IsNullOrEmpty(module.noResponseFollowupModule))
		{
			base.StartCoroutine(this.HandleConversationModule(this.GetModuleFromName(module.noResponseFollowupModule)));
		}
		else
		{
			TextBoxManager.ClearTextBox(this.speakPoint);
			this.EndConversation();
		}
		yield break;
	}

	// Token: 0x060067DC RID: 26588 RVA: 0x0028AD34 File Offset: 0x00288F34
	private IEnumerator HandleResponses(TalkModule module, string overrideResponse1, string overrideResponse2, string overrideFollowupModule1, string overrideFollowupModule2)
	{
		int selectedResponse = -1;
		this.talkingPlayer.SetInputOverride("talkDoerResponse");
		GameUIRoot.Instance.DisplayPlayerConversationOptions(this.talkingPlayer, module, overrideResponse1, overrideResponse2);
		while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
		{
			yield return null;
		}
		this.talkingPlayer.ClearInputOverride("talkDoerResponse");
		TextBoxManager.ClearTextBox(this.speakPoint);
		TalkResponse response = module.responses[selectedResponse];
		TalkModule nextModule = null;
		for (int i = 0; i < response.resultActions.Count; i++)
		{
			this.ProcessResponseAction(response.resultActions[i]);
		}
		if (selectedResponse == 0 && !string.IsNullOrEmpty(overrideFollowupModule1))
		{
			nextModule = new TalkModule();
			nextModule.CopyFrom(module);
			nextModule.stringKeys = new string[] { overrideFollowupModule1 };
		}
		else if (selectedResponse == 1 && !string.IsNullOrEmpty(overrideFollowupModule2))
		{
			nextModule = new TalkModule();
			nextModule.CopyFrom(module);
			nextModule.stringKeys = new string[] { overrideFollowupModule2 };
		}
		else if (!string.IsNullOrEmpty(response.followupModuleID))
		{
			nextModule = this.GetModuleFromName(response.followupModuleID);
		}
		if (nextModule != null)
		{
			base.StartCoroutine(this.HandleConversationModule(nextModule));
			yield break;
		}
		this.EndConversation();
		yield break;
	}

	// Token: 0x060067DD RID: 26589 RVA: 0x0028AD74 File Offset: 0x00288F74
	private IEnumerator WaitForTextRevealed()
	{
		while (TextBoxManager.TextBoxCanBeAdvanced(this.speakPoint))
		{
			if (BraveInput.WasSelectPressed(InputManager.ActiveDevice))
			{
				TextBoxManager.AdvanceTextBox(this.speakPoint);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060067DE RID: 26590 RVA: 0x0028AD90 File Offset: 0x00288F90
	private IEnumerator WaitForPlayer()
	{
		for (;;)
		{
			if (BraveInput.WasSelectPressed(InputManager.ActiveDevice))
			{
				if (!TextBoxManager.TextBoxCanBeAdvanced(this.speakPoint))
				{
					break;
				}
				TextBoxManager.AdvanceTextBox(this.speakPoint);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060067DF RID: 26591 RVA: 0x0028ADAC File Offset: 0x00288FAC
	public float GetDistanceToPoint(Vector2 point)
	{
		if (!this)
		{
			return 1000f;
		}
		if (this.m_uninteractable || this.OverrideNoninteractable)
		{
			return 1000f;
		}
		if (this.usesOverrideInteractionRegion)
		{
			return BraveMathCollege.DistToRectangle(point, base.transform.position.XY() + this.overrideRegionOffset * 0.0625f, this.overrideRegionDimensions * 0.0625f);
		}
		Bounds bounds = base.sprite.GetBounds();
		return BraveMathCollege.DistToRectangle(point, base.sprite.transform.position + bounds.center - bounds.extents, bounds.size);
	}

	// Token: 0x060067E0 RID: 26592 RVA: 0x0028AE78 File Offset: 0x00289078
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x060067E1 RID: 26593 RVA: 0x0028AE80 File Offset: 0x00289080
	public void OnEnteredRange(PlayerController interactor)
	{
		if (this.m_isTalking)
		{
			return;
		}
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x060067E2 RID: 26594 RVA: 0x0028AECC File Offset: 0x002890CC
	public void OnExitRange(PlayerController interactor)
	{
		if (this.m_isTalking)
		{
			return;
		}
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x060067E3 RID: 26595 RVA: 0x0028AF18 File Offset: 0x00289118
	public void Interact(PlayerController interactor)
	{
		if (this.m_isTalking)
		{
			return;
		}
		this.talkingPlayer = interactor;
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
		this.BeginConversation(interactor);
	}

	// Token: 0x060067E4 RID: 26596 RVA: 0x0028AF50 File Offset: 0x00289150
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060067E5 RID: 26597 RVA: 0x0028AF5C File Offset: 0x0028915C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040063E0 RID: 25568
	public List<TalkModule> modules;

	// Token: 0x040063E1 RID: 25569
	public Transform speakPoint;

	// Token: 0x040063E2 RID: 25570
	public string audioCharacterSpeechTag = string.Empty;

	// Token: 0x040063E3 RID: 25571
	[Header("Interactable Region")]
	public bool usesOverrideInteractionRegion;

	// Token: 0x040063E4 RID: 25572
	[ShowInInspectorIf("usesOverrideInteractionRegion", false)]
	public Vector2 overrideRegionOffset = Vector2.zero;

	// Token: 0x040063E5 RID: 25573
	[ShowInInspectorIf("usesOverrideInteractionRegion", false)]
	public Vector2 overrideRegionDimensions = Vector2.zero;

	// Token: 0x040063E6 RID: 25574
	[Header("Core Speech")]
	public string FirstMeetingEverModule = string.Empty;

	// Token: 0x040063E7 RID: 25575
	public string FirstMeetingSessionModule = string.Empty;

	// Token: 0x040063E8 RID: 25576
	public string RepeatMeetingSessionModule = string.Empty;

	// Token: 0x040063E9 RID: 25577
	[Header("Other Options")]
	[CheckAnimation(null)]
	public string fallbackAnimName = "idle";

	// Token: 0x040063EA RID: 25578
	[CheckAnimation(null)]
	public string defaultSpeechAnimName = "talk";

	// Token: 0x040063EB RID: 25579
	public bool usesCustomBetrayalLogic;

	// Token: 0x040063EC RID: 25580
	public string betrayalSpeechKey = string.Empty;

	// Token: 0x040063ED RID: 25581
	public bool betrayalSpeechSequential;

	// Token: 0x040063EE RID: 25582
	private int betrayalSpeechIndex = -1;

	// Token: 0x040063EF RID: 25583
	public string hitAnimName;

	// Token: 0x040063F0 RID: 25584
	public bool DoesVanish = true;

	// Token: 0x040063F1 RID: 25585
	public string vanishAnimName = "exit";

	// Token: 0x040063F2 RID: 25586
	public Action OnBetrayalWarning;

	// Token: 0x040063F3 RID: 25587
	public Action OnBetrayal;

	// Token: 0x040063F4 RID: 25588
	public List<GameObject> itemsToLeaveBehind;

	// Token: 0x040063F5 RID: 25589
	public bool alwaysWaitsForInput;

	// Token: 0x040063F6 RID: 25590
	public TalkDoer echo1;

	// Token: 0x040063F7 RID: 25591
	public TalkDoer echo2;

	// Token: 0x040063F8 RID: 25592
	public float conversationBreakRadius = 5f;

	// Token: 0x040063F9 RID: 25593
	public List<CharacterTalkModuleOverride> characterOverrides;

	// Token: 0x040063FA RID: 25594
	public bool OverrideNoninteractable;

	// Token: 0x040063FB RID: 25595
	protected TalkModule defaultModule;

	// Token: 0x040063FC RID: 25596
	protected bool m_isTalking;

	// Token: 0x040063FD RID: 25597
	protected bool m_uninteractable;

	// Token: 0x040063FE RID: 25598
	protected int numTimesSpokenTo;

	// Token: 0x040063FF RID: 25599
	protected int hitCount;

	// Token: 0x04006400 RID: 25600
	protected bool m_isDealingWithBetrayal;

	// Token: 0x04006401 RID: 25601
	private bool m_hack_isOpeningTruthChest;

	// Token: 0x04006402 RID: 25602
	private bool m_isDoingForcedSpeech;

	// Token: 0x04006403 RID: 25603
	protected PlayerController talkingPlayer;
}
