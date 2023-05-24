using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

// Token: 0x02001129 RID: 4393
public class ConvictPastController : MonoBehaviour
{
	// Token: 0x060060FF RID: 24831 RVA: 0x00254A88 File Offset: 0x00252C88
	private IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		NightclubCrowdController nightclubCrowdController = this.crowdController;
		nightclubCrowdController.OnPanic = (Action)Delegate.Combine(nightclubCrowdController.OnPanic, new Action(this.HandlePrematurePanic));
		if (this.InstantBossFight)
		{
			PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
			primaryPlayer.transform.position = new Vector2(31f, 27f);
			primaryPlayer.specRigidbody.Reinitialize();
			this.BaldoBossTalkDoer.specRigidbody.enabled = false;
			this.BaldoBossTalkDoer.gameObject.SetActive(false);
			List<HealthHaver> allHealthHavers = StaticReferenceManager.AllHealthHavers;
			for (int i = 0; i < allHealthHavers.Count; i++)
			{
				if (allHealthHavers[i].IsBoss)
				{
					allHealthHavers[i].GetComponent<ObjectVisibilityManager>().ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
					allHealthHavers[i].GetComponent<GenericIntroDoer>().TriggerSequence(primaryPlayer);
				}
			}
			yield break;
		}
		GameManager.Instance.PrimaryPlayer.SetInputOverride("past");
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.SecondaryPlayer.SetInputOverride("past");
		}
		GameManager.Instance.PrimaryPlayer.ForceIdleFacePoint(Vector2.down, true);
		for (int j = 0; j < this.HmonSoldiers.Length; j++)
		{
			if (j == 0)
			{
				Fsm fsm = this.HmonSoldiers[0].playmakerFsm.Fsm;
				for (int k = 0; k < fsm.States.Length; k++)
				{
					FsmState fsmState = fsm.States[k];
					for (int l = 0; l < fsmState.Actions.Length; l++)
					{
						if (fsmState.Actions[l] is BecomeHostile)
						{
							switch (l)
							{
							case 0:
								(fsmState.Actions[l] as BecomeHostile).alternativeTarget = this.HmonSoldiers[0];
								break;
							case 1:
								(fsmState.Actions[l] as BecomeHostile).alternativeTarget = this.HmonSoldiers[1];
								break;
							case 2:
								(fsmState.Actions[l] as BecomeHostile).alternativeTarget = this.HmonSoldiers[2];
								break;
							case 3:
								(fsmState.Actions[l] as BecomeHostile).alternativeTarget = this.HmonSoldiers[3];
								break;
							}
						}
					}
				}
			}
			this.HmonSoldiers[j].renderer.enabled = true;
			this.HmonSoldiers[j].specRigidbody.enabled = true;
			this.HmonSoldiers[j].specRigidbody.Reinitialize();
		}
		Pixelator.Instance.FadeToBlack(0.25f, true, 0.05f);
		Pixelator.Instance.TriggerPastFadeIn();
		yield return new WaitForSeconds(0.4f);
		Pixelator.Instance.SetOcclusionDirty();
		PastCameraUtility.LockConversation(this.InitialTalkDoer.speakPoint.transform.position.XY() + new Vector2(0f, 3f));
		yield return null;
		Pixelator.Instance.SetOcclusionDirty();
		yield return new WaitForSeconds(6f);
		yield return base.StartCoroutine(this.DoAmbientTalk(GameManager.Instance.PrimaryPlayer.transform, new Vector3(0.75f, 1.5f, 0f), "#CONVICTPAST_THOUGHTS_01", -1f, true));
		yield return new WaitForSeconds(1f);
		this.InitialTalkDoer.Interact(GameManager.Instance.PrimaryPlayer);
		while (this.InitialTalkDoer.IsTalking)
		{
			yield return null;
		}
		GameManager.Instance.MainCameraController.UpdateOverridePosition(GameManager.Instance.MainCameraController.OverridePosition + new Vector3(0f, -2.5f, 0f), 5f);
		this.InitialTalkDoer.PathfindToPosition(this.InitialTalkDoer.specRigidbody.UnitCenter + new Vector2(-7f, -3f), null, null);
		Vector2 lastPositionFlunky = this.InitialTalkDoer.specRigidbody.UnitCenter;
		this.BaldoTalkDoer.PathfindToPosition(this.BaldoTalkDoer.specRigidbody.UnitCenter + new Vector2(0f, 25f), null, null);
		Vector2 lastPosition = this.BaldoTalkDoer.specRigidbody.UnitCenter;
		Vector2[] HmonTargetPositions = new Vector2[]
		{
			new Vector2(-5f, 24f),
			new Vector2(-2f, 22f),
			new Vector2(2.5f, 22f),
			new Vector2(5f, 24f)
		};
		Vector2[] hmonLastPositions = new Vector2[this.HmonSoldiers.Length];
		for (int m = 0; m < this.HmonSoldiers.Length; m++)
		{
			this.HmonSoldiers[m].PathfindToPosition(this.HmonSoldiers[0].specRigidbody.UnitCenter + HmonTargetPositions[m], null, null);
			hmonLastPositions[m] = this.HmonSoldiers[m].specRigidbody.UnitCenter;
		}
		bool hasPath = true;
		while (hasPath)
		{
			hasPath = false;
			if (this.InitialTalkDoer.CurrentPath != null)
			{
				hasPath = true;
				this.InitialTalkDoer.specRigidbody.Velocity = this.InitialTalkDoer.GetPathVelocityContribution(lastPositionFlunky, 16);
				lastPositionFlunky = this.InitialTalkDoer.specRigidbody.UnitCenter;
			}
			if (this.BaldoTalkDoer.CurrentPath != null)
			{
				hasPath = true;
				this.BaldoTalkDoer.specRigidbody.Velocity = this.BaldoTalkDoer.GetPathVelocityContribution(lastPosition, 16);
				lastPosition = this.BaldoTalkDoer.specRigidbody.UnitCenter;
			}
			for (int n = 0; n < this.HmonSoldiers.Length; n++)
			{
				if (this.HmonSoldiers[n].CurrentPath != null)
				{
					hasPath = true;
					this.HmonSoldiers[n].specRigidbody.Velocity = this.HmonSoldiers[n].GetPathVelocityContribution(hmonLastPositions[n], 16);
					hmonLastPositions[n] = this.HmonSoldiers[n].specRigidbody.UnitCenter;
				}
			}
			yield return null;
		}
		this.BaldoTalkDoer.specRigidbody.Velocity = Vector2.zero;
		this.BaldoTalkDoer.Interact(GameManager.Instance.PrimaryPlayer);
		while (this.BaldoTalkDoer.IsTalking)
		{
			yield return null;
		}
		this.BaldoTalkDoer.PathfindToPosition(this.BaldoTalkDoer.specRigidbody.UnitCenter + new Vector2(-0.5f, -25f), null, null);
		base.StartCoroutine(this.DoPath(this.BaldoTalkDoer, true));
		yield return new WaitForSeconds(1f);
		this.HmonSoldiers[0].Interact(GameManager.Instance.PrimaryPlayer);
		while (this.HmonSoldiers[0].IsTalking)
		{
			yield return null;
		}
		PastCameraUtility.UnlockConversation();
		this.DeskAnimator.sprite.HeightOffGround = -4.5f;
		this.DeskAnimator.sprite.UpdateZDepth();
		this.DeskAnimator.Play();
		foreach (MinorBreakable minorBreakable in this.DeskAnimator.GetComponentsInChildren<MinorBreakable>())
		{
			minorBreakable.Break(Vector2.down);
		}
		AkSoundEngine.PostEvent("Play_MUS_Ending_State_02", GameManager.Instance.gameObject);
		yield return new WaitForSeconds(0.4f);
		this.DeskAnimatorPoof.SetActive(true);
		for (;;)
		{
			bool shouldBreak = false;
			for (int num2 = 0; num2 < GameManager.Instance.AllPlayers.Length; num2++)
			{
				if (GameManager.Instance.AllPlayers[num2].CenterPosition.x < this.BaldoBossTalkDoer.specRigidbody.UnitBottomCenter.x + 20f)
				{
					shouldBreak = true;
				}
			}
			if (shouldBreak)
			{
				break;
			}
			yield return null;
		}
		for (;;)
		{
			bool shouldBreak2 = false;
			for (int num3 = 0; num3 < GameManager.Instance.AllPlayers.Length; num3++)
			{
				Vector2 centerPosition = GameManager.Instance.AllPlayers[num3].CenterPosition;
				if (centerPosition.y > this.BaldoBossTalkDoer.specRigidbody.UnitBottomCenter.y - 7f)
				{
					shouldBreak2 = true;
				}
				if (Mathf.Abs(this.BaldoBossTalkDoer.specRigidbody.UnitBottomCenter.x - centerPosition.x) < 3f && centerPosition.y > this.BaldoBossTalkDoer.specRigidbody.UnitBottomCenter.y - 13f)
				{
					shouldBreak2 = true;
				}
			}
			if (shouldBreak2)
			{
				break;
			}
			yield return null;
		}
		this.MainDoorBlocker.gameObject.SetActive(true);
		this.MainDoorBlocker.enabled = true;
		this.MainDoorBlocker.Reinitialize();
		yield return base.StartCoroutine(this.HandleBossStart(0f));
		yield break;
	}

	// Token: 0x06006100 RID: 24832 RVA: 0x00254AA4 File Offset: 0x00252CA4
	private void HandlePrematurePanic()
	{
		if (!this.m_hasStartedBossSequence)
		{
			base.StartCoroutine(this.HandleBossStart(3f));
		}
	}

	// Token: 0x06006101 RID: 24833 RVA: 0x00254AC4 File Offset: 0x00252CC4
	private IEnumerator HandleBossStart(float initialDelay = 0f)
	{
		if (initialDelay > 0f)
		{
			yield return new WaitForSeconds(initialDelay);
		}
		if (this.m_hasStartedBossSequence)
		{
			yield break;
		}
		this.m_hasStartedBossSequence = true;
		PastCameraUtility.LockConversation(this.BaldoBossTalkDoer.speakPoint.transform.position.XY() + new Vector2(0f, -1f));
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
			PlayerController secondaryPlayer = GameManager.Instance.SecondaryPlayer;
			if (primaryPlayer.transform.position.x > secondaryPlayer.transform.position.x && primaryPlayer.transform.position.x > this.BaldoBossTalkDoer.specRigidbody.UnitBottomCenter.x + 16f)
			{
				primaryPlayer.ReuniteWithOtherPlayer(secondaryPlayer, false);
			}
			else if (secondaryPlayer.transform.position.x > this.BaldoBossTalkDoer.specRigidbody.UnitBottomCenter.x + 16f)
			{
				secondaryPlayer.ReuniteWithOtherPlayer(primaryPlayer, false);
			}
		}
		this.BaldoBossTalkDoer.Interact(GameManager.Instance.PrimaryPlayer);
		while (this.BaldoBossTalkDoer.IsTalking)
		{
			yield return null;
		}
		PastCameraUtility.UnlockConversation();
		TextBoxManager.ClearTextBoxImmediate(this.BaldoBossTalkDoer.speakPoint);
		this.BaldoBossTalkDoer.specRigidbody.enabled = false;
		this.BaldoBossTalkDoer.gameObject.SetActive(false);
		List<HealthHaver> healthHavers = StaticReferenceManager.AllHealthHavers;
		for (int i = 0; i < healthHavers.Count; i++)
		{
			if (healthHavers[i].IsBoss)
			{
				healthHavers[i].specRigidbody.transform.position = this.BaldoBossTalkDoer.transform.position;
				healthHavers[i].specRigidbody.Reinitialize();
				healthHavers[i].GetComponent<ObjectVisibilityManager>().ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
				healthHavers[i].GetComponent<GenericIntroDoer>().TriggerSequence(GameManager.Instance.PrimaryPlayer);
			}
		}
		yield break;
	}

	// Token: 0x06006102 RID: 24834 RVA: 0x00254AE8 File Offset: 0x00252CE8
	private IEnumerator DoPath(TalkDoerLite source, bool doDestroy)
	{
		Vector2 lastPos = source.specRigidbody.UnitCenter;
		while (source.CurrentPath != null)
		{
			source.specRigidbody.Velocity = source.GetPathVelocityContribution(lastPos, 16);
			lastPos = source.specRigidbody.UnitCenter;
			yield return null;
		}
		if (doDestroy)
		{
			UnityEngine.Object.Destroy(source.gameObject);
		}
		yield break;
	}

	// Token: 0x06006103 RID: 24835 RVA: 0x00254B0C File Offset: 0x00252D0C
	public void OnBossKilled(Transform bossTransform)
	{
		base.StartCoroutine(this.HandleBossKilled(bossTransform));
	}

	// Token: 0x06006104 RID: 24836 RVA: 0x00254B1C File Offset: 0x00252D1C
	private IEnumerator EnableHeadlights()
	{
		float ela = 0f;
		float dura = 0.5f;
		this.CarHeadlightsRenderer.enabled = true;
		Color targetColor = this.CarHeadlightsRenderer.material.GetColor("_TintColor");
		Color startColor = targetColor.WithAlpha(0f);
		while (ela < dura)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			float t = ela / dura;
			this.CarHeadlightsRenderer.material.SetColor("_TintColor", Color.Lerp(startColor, targetColor, t));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006105 RID: 24837 RVA: 0x00254B38 File Offset: 0x00252D38
	private IEnumerator HandleBossKilled(Transform bossTransform)
	{
		GameStatsManager.Instance.SetCharacterSpecificFlag(PlayableCharacters.Convict, CharacterSpecificGungeonFlags.KILLED_PAST, true);
		GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_CONVICT_PAST, true);
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_KILLED_PAST, 1f);
		if (this.PhantomEndTalkDoer != null)
		{
			this.PhantomEndTalkDoer.gameObject.SetActive(true);
			this.PhantomEndTalkDoer.ShowOutlines = false;
		}
		GameUIRoot.Instance.ToggleLowerPanels(false, true, string.Empty);
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		this.ExitDoorRigidbody.enabled = false;
		this.BaldoBossTalkDoer.specRigidbody.enabled = true;
		this.BaldoBossTalkDoer.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.Projectile));
		this.BaldoBossTalkDoer.gameObject.SetActive(true);
		this.BaldoBossTalkDoer.transform.position = bossTransform.position;
		this.BaldoBossTalkDoer.specRigidbody.Reinitialize();
		this.BaldoBossTalkDoer.aiAnimator.PlayUntilCancelled("die", false, null, -1f, false);
		this.BaldoBossTalkDoer.sprite.IsPerpendicular = false;
		this.BaldoBossTalkDoer.sprite.HeightOffGround = -1f;
		this.BaldoBossTalkDoer.sprite.UpdateZDepth();
		dfControl goToCarPanel = GameUIRoot.Instance.Manager.AddPrefab(BraveResources.Load("Global Prefabs/GoToCarPanel", ".prefab") as GameObject);
		GameUIRoot.Instance.AddControlToMotionGroups(goToCarPanel, DungeonData.Direction.WEST, true);
		GameUIRoot.Instance.MoveNonCoreGroupOnscreen(goToCarPanel, true);
		while (GameManager.Instance.MainCameraController.ManualControl)
		{
			yield return null;
		}
		yield return new WaitForSeconds(2f);
		GameUIRoot.Instance.MoveNonCoreGroupOnscreen(goToCarPanel, false);
		bool atCar = false;
		bool hasOpened = false;
		bool isClosing = false;
		bool coopInCar = false;
		while (!atCar)
		{
			if (!hasOpened && Vector2.Distance(this.Car.Sprite.WorldCenter, GameManager.Instance.PrimaryPlayer.CenterPosition) < 5f)
			{
				hasOpened = true;
				this.Car.Play("getaway_car_open");
			}
			if (hasOpened && !isClosing)
			{
				bool flag = false;
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					bool flag2 = Vector2.Distance(this.Car.Sprite.WorldCenter, GameManager.Instance.PrimaryPlayer.CenterPosition) < 0.8f;
					if (flag2)
					{
						float num = Vector2.Distance(this.Car.Sprite.WorldCenter, GameManager.Instance.SecondaryPlayer.CenterPosition);
						if (num < 0.8f)
						{
							flag = true;
							coopInCar = true;
						}
						else if (num > 4f)
						{
							flag = true;
							coopInCar = false;
						}
					}
				}
				else
				{
					flag = Vector2.Distance(this.Car.Sprite.WorldCenter, GameManager.Instance.PrimaryPlayer.CenterPosition) < 0.8f;
				}
				if (flag)
				{
					isClosing = true;
					this.Car.Play("getaway_car_close");
					GameUIRoot.Instance.RemoveControlFromMotionGroups(goToCarPanel);
					UnityEngine.Object.Destroy(goToCarPanel.gameObject);
					for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
					{
						if (GameManager.Instance.AllPlayers[i])
						{
							GameManager.Instance.AllPlayers[i].CurrentInputState = PlayerInputState.NoInput;
							GameManager.Instance.AllPlayers[i].ForceStopDodgeRoll();
							GameManager.Instance.AllPlayers[i].specRigidbody.Velocity = Vector2.zero;
						}
					}
				}
			}
			if (isClosing && !this.Car.IsPlaying("getaway_car_close"))
			{
				atCar = true;
				break;
			}
			yield return null;
		}
		for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
		{
			if (GameManager.Instance.AllPlayers[j])
			{
				GameManager.Instance.AllPlayers[j].CurrentInputState = PlayerInputState.NoInput;
			}
		}
		ParticleSystem[] carExhausts = this.Car.GetComponentsInChildren<ParticleSystem>(true);
		for (int k = 0; k < carExhausts.Length; k++)
		{
			carExhausts[k].gameObject.SetActive(true);
		}
		base.StartCoroutine(this.EnableHeadlights());
		float elapsed = 0f;
		float duration = 2f;
		Vector3 startPos = this.Car.transform.position;
		Vector3 playerStartPos = GameManager.Instance.PrimaryPlayer.transform.position;
		GameManager.Instance.MainCameraController.SetManualControl(true, true);
		GameManager.Instance.MainCameraController.OverridePosition = this.Car.Sprite.WorldCenter;
		GameManager.Instance.PrimaryPlayer.IsVisible = false;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && coopInCar)
		{
			GameManager.Instance.SecondaryPlayer.IsVisible = false;
		}
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			Vector3 offset = Vector3.Lerp(Vector3.zero, Vector3.right * 30f, BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, elapsed / duration));
			this.Car.transform.position = startPos + offset;
			GameManager.Instance.PrimaryPlayer.transform.position = playerStartPos + offset;
			GameManager.Instance.PrimaryPlayer.specRigidbody.Reinitialize();
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && coopInCar)
			{
				GameManager.Instance.SecondaryPlayer.transform.position = playerStartPos + offset;
				GameManager.Instance.SecondaryPlayer.specRigidbody.Reinitialize();
			}
			GameManager.Instance.MainCameraController.OverridePosition = this.Car.Sprite.WorldCenter;
			yield return null;
		}
		GameManager.Instance.MainCameraController.SetManualControl(true, false);
		GameManager.Instance.MainCameraController.OverridePosition = GameManager.Instance.MainCameraController.transform.position;
		Pixelator.Instance.FreezeFrame();
		BraveTime.RegisterTimeScaleMultiplier(0f, base.gameObject);
		float ela = 0f;
		while (ela < ConvictPastController.FREEZE_FRAME_DURATION)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		BraveTime.ClearMultiplier(base.gameObject);
		PastCameraUtility.LockConversation(GameManager.Instance.PrimaryPlayer.CenterPosition);
		TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
		Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
		ttcc.ClearDebris();
		ttcc.ForceNoTimefallForCoop = !coopInCar;
		yield return base.StartCoroutine(ttcc.HandleTimeTubeCredits(GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, false, null, -1, false));
		AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		yield break;
	}

	// Token: 0x06006106 RID: 24838 RVA: 0x00254B5C File Offset: 0x00252D5C
	public IEnumerator DoAmbientTalk(Transform baseTransform, Vector3 offset, string stringKey, float duration, bool isThoughtBubble)
	{
		if (isThoughtBubble)
		{
			TextBoxManager.ShowThoughtBubble(baseTransform.position + offset, baseTransform, duration, StringTableManager.GetString(stringKey), false, true, GameManager.Instance.PrimaryPlayer.characterAudioSpeechTag);
		}
		else
		{
			TextBoxManager.ShowTextBox(baseTransform.position + offset, baseTransform, duration, StringTableManager.GetString(stringKey), GameManager.Instance.PrimaryPlayer.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, true, false);
		}
		bool advancedPressed = false;
		while (!advancedPressed)
		{
			advancedPressed = BraveInput.GetInstanceForPlayer(0).WasAdvanceDialoguePressed();
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				advancedPressed |= BraveInput.GetInstanceForPlayer(1).WasAdvanceDialoguePressed();
			}
			yield return null;
		}
		TextBoxManager.ClearTextBox(baseTransform);
		yield break;
	}

	// Token: 0x06006107 RID: 24839 RVA: 0x00254B98 File Offset: 0x00252D98
	private void Update()
	{
	}

	// Token: 0x04005B9F RID: 23455
	public bool InstantBossFight;

	// Token: 0x04005BA0 RID: 23456
	public TalkDoerLite InitialTalkDoer;

	// Token: 0x04005BA1 RID: 23457
	public TalkDoerLite BaldoTalkDoer;

	// Token: 0x04005BA2 RID: 23458
	public TalkDoerLite BaldoBossTalkDoer;

	// Token: 0x04005BA3 RID: 23459
	public NightclubCrowdController crowdController;

	// Token: 0x04005BA4 RID: 23460
	public TalkDoerLite[] HmonSoldiers;

	// Token: 0x04005BA5 RID: 23461
	public tk2dSpriteAnimator DeskAnimator;

	// Token: 0x04005BA6 RID: 23462
	public GameObject DeskAnimatorPoof;

	// Token: 0x04005BA7 RID: 23463
	public TalkDoerLite PhantomEndTalkDoer;

	// Token: 0x04005BA8 RID: 23464
	public tk2dSpriteAnimator Car;

	// Token: 0x04005BA9 RID: 23465
	public Renderer CarHeadlightsRenderer;

	// Token: 0x04005BAA RID: 23466
	public SpeculativeRigidbody ExitDoorRigidbody;

	// Token: 0x04005BAB RID: 23467
	public SpeculativeRigidbody MainDoorBlocker;

	// Token: 0x04005BAC RID: 23468
	private bool m_hasStartedBossSequence;

	// Token: 0x04005BAD RID: 23469
	public static float FREEZE_FRAME_DURATION = 2f;
}
