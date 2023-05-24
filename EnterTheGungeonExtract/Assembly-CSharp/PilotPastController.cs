using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020011D1 RID: 4561
public class PilotPastController : MonoBehaviour
{
	// Token: 0x060065CC RID: 26060 RVA: 0x00278C3C File Offset: 0x00276E3C
	private IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		GameUIRoot.Instance.ToggleUICamera(false);
		this.HegemonyShip.renderer.enabled = false;
		for (int i = 0; i < this.AdditionalHegemonyShips.Length; i++)
		{
			this.AdditionalHegemonyShips[i].renderer.enabled = false;
		}
		if (this.InstantBossFight)
		{
			PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
			this.HegemonyShip.specRigidbody.enabled = false;
			this.HegemonyShip.gameObject.SetActive(false);
			this.FloatingCrap.SetActive(false);
			this.FriendTalker.gameObject.SetActive(false);
			this.TheRock.gameObject.SetActive(false);
			List<HealthHaver> allHealthHavers = StaticReferenceManager.AllHealthHavers;
			for (int j = 0; j < allHealthHavers.Count; j++)
			{
				if (allHealthHavers[j].IsBoss)
				{
					allHealthHavers[j].GetComponent<ObjectVisibilityManager>().ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
					allHealthHavers[j].GetComponent<GenericIntroDoer>().TriggerSequence(primaryPlayer);
					allHealthHavers[j].GetComponent<BossFinalRogueController>();
				}
			}
			yield break;
		}
		Pixelator.Instance.TriggerPastFadeIn();
		yield return null;
		SpriteOutlineManager.ToggleOutlineRenderers(this.HegemonyShip.sprite, false);
		this.m_pilot = GameManager.Instance.PrimaryPlayer;
		this.m_pilot.sprite.UpdateZDepth();
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.m_coop = GameManager.Instance.SecondaryPlayer;
			this.m_coop.transform.position += new Vector3(3f, -2f, 0f);
			this.m_coop.specRigidbody.Reinitialize();
			this.m_coop.sprite.UpdateZDepth();
		}
		this.m_pilot.SetInputOverride("past");
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.m_coop.SetInputOverride("past");
		}
		yield return new WaitForSeconds(0.4f);
		Pixelator.Instance.SetOcclusionDirty();
		PastCameraUtility.LockConversation((this.FriendTalker.speakPoint.transform.position.XY() + this.m_pilot.CenterPosition) / 2f);
		yield return null;
		Pixelator.Instance.SetOcclusionDirty();
		yield return new WaitForSeconds(5.6f);
		GameUIRoot.Instance.ToggleUICamera(true);
		this.FriendTalker.Interact(this.m_pilot);
		while (this.FriendTalker.IsTalking)
		{
			yield return null;
		}
		this.FriendTalker.spriteAnimator.Play();
		yield return new WaitForSeconds(0.5f);
		GameManager.Instance.MainCameraController.OverrideZoomScale = 0.5f;
		GameManager.Instance.MainCameraController.UpdateOverridePosition(GameManager.Instance.MainCameraController.OverridePosition + new Vector3(0f, 8.5f, 0f), 3f);
		ScreenShakeSettings arrivalSS = new ScreenShakeSettings(0.5f, 12f, 2f, 0f);
		GameManager.Instance.MainCameraController.DoContinuousScreenShake(arrivalSS, this, false);
		yield return new WaitForSeconds(2f);
		for (int k = 0; k < this.AdditionalHegemonyShips.Length; k++)
		{
			base.StartCoroutine(this.ArriveFromWarp(this.AdditionalHegemonyShips[k], UnityEngine.Random.Range(0.75f, 1.25f)));
		}
		yield return base.StartCoroutine(this.ArriveFromWarp(this.HegemonyShip.sprite, 1f));
		GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
		yield return new WaitForSeconds(1f);
		float elapsed = 0f;
		Tribool hasClamped = Tribool.Unready;
		this.HegemonyShip.Interact(this.m_pilot);
		while (this.HegemonyShip.IsTalking)
		{
			elapsed += BraveTime.DeltaTime;
			if (elapsed > 1f)
			{
				if (hasClamped == Tribool.Unready)
				{
					if (this.FriendTalker.spriteAnimator.CurrentFrame == 4)
					{
						this.TheRock.spriteAnimator.Stop();
						this.TheRock.transform.parent = this.FriendTalker.GetComponent<tk2dSpriteAttachPoint>().attachPoints[0];
						this.TheRock.PlaceAtLocalPositionByAnchor(Vector3.zero, tk2dBaseSprite.Anchor.LowerCenter);
						hasClamped = Tribool.op_Increment(hasClamped);
					}
				}
				else if (hasClamped == Tribool.Ready && this.FriendTalker.spriteAnimator.CurrentFrame == 8)
				{
					this.FriendTalker.spriteAnimator.Stop();
					hasClamped = Tribool.op_Increment(hasClamped);
				}
			}
			yield return null;
		}
		GameManager.Instance.MainCameraController.UpdateOverridePosition(GameManager.Instance.MainCameraController.OverridePosition + new Vector3(0f, -8.5f, 0f), 3f);
		yield return new WaitForSeconds(1.5f);
		GameManager.Instance.MainCameraController.OverrideZoomScale = 1f;
		yield return null;
		this.FriendTalker.Interact(this.m_pilot);
		while (this.FriendTalker.IsTalking)
		{
			yield return null;
		}
		PastCameraUtility.UnlockConversation();
		this.HegemonyShip.specRigidbody.enabled = false;
		this.HegemonyShip.gameObject.SetActive(false);
		for (int l = 0; l < StaticReferenceManager.AllHealthHavers.Count; l++)
		{
			if (StaticReferenceManager.AllHealthHavers[l].IsBoss)
			{
				StaticReferenceManager.AllHealthHavers[l].GetComponent<ObjectVisibilityManager>().ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
				StaticReferenceManager.AllHealthHavers[l].GetComponent<GenericIntroDoer>().TriggerSequence(GameManager.Instance.PrimaryPlayer);
			}
		}
		this.ToggleFriendAndJunk(false);
		yield break;
	}

	// Token: 0x060065CD RID: 26061 RVA: 0x00278C58 File Offset: 0x00276E58
	public void Update()
	{
		Material material = this.Quad.material;
		if (this.m_scrollPositionXId < 0)
		{
			this.m_scrollPositionXId = Shader.PropertyToID("_PositionX");
		}
		if (this.m_scrollPositionYId < 0)
		{
			this.m_scrollPositionYId = Shader.PropertyToID("_PositionY");
		}
		this.m_backgroundOffset += this.BackgroundScrollSpeed * BraveTime.DeltaTime;
		material.SetFloat(this.m_scrollPositionXId, this.m_backgroundOffset.x);
		material.SetFloat(this.m_scrollPositionYId, this.m_backgroundOffset.y);
	}

	// Token: 0x060065CE RID: 26062 RVA: 0x00278CF8 File Offset: 0x00276EF8
	public void ToggleFriendAndJunk(bool state)
	{
		base.StartCoroutine(this.HandleFriendAndJunkToggle(state));
	}

	// Token: 0x060065CF RID: 26063 RVA: 0x00278D08 File Offset: 0x00276F08
	private IEnumerator HandleFriendAndJunkToggle(bool state)
	{
		float elapsed = 0f;
		tk2dBaseSprite[] crapSprites = this.FloatingCrap.GetComponentsInChildren<tk2dBaseSprite>(true);
		if (state)
		{
			this.FriendTalker.renderer.enabled = true;
			this.TheRock.renderer.enabled = true;
			SpriteOutlineManager.ToggleOutlineRenderers(this.FriendTalker.sprite, true);
			for (int i = 0; i < crapSprites.Length; i++)
			{
				crapSprites[i].renderer.enabled = true;
			}
		}
		this.FriendTalker.specRigidbody.enabled = state;
		while (elapsed < 2f)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / 2f;
			if (state)
			{
				t = 1f - t;
			}
			Color targetcolor = Color.Lerp(Color.white, new Color(0.2f, 0.2f, 0.2f), t);
			this.FriendTalker.sprite.color = targetcolor;
			for (int j = 0; j < crapSprites.Length; j++)
			{
				crapSprites[j].color = targetcolor;
			}
			this.TheRock.sprite.color = targetcolor;
			yield return null;
		}
		if (!state)
		{
			this.FriendTalker.renderer.enabled = false;
			this.TheRock.renderer.enabled = false;
			SpriteOutlineManager.ToggleOutlineRenderers(this.FriendTalker.sprite, false);
			for (int k = 0; k < crapSprites.Length; k++)
			{
				crapSprites[k].renderer.enabled = false;
			}
		}
		yield break;
	}

	// Token: 0x060065D0 RID: 26064 RVA: 0x00278D2C File Offset: 0x00276F2C
	public IEnumerator EndPastSuccess()
	{
		GameStatsManager.Instance.SetCharacterSpecificFlag(PlayableCharacters.Pilot, CharacterSpecificGungeonFlags.KILLED_PAST, true);
		TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
		ttcc.ClearDebris();
		yield return base.StartCoroutine(ttcc.HandleTimeTubeCredits(GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, false, null, -1, false));
		AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		yield break;
	}

	// Token: 0x060065D1 RID: 26065 RVA: 0x00278D48 File Offset: 0x00276F48
	private void SetupCutscene()
	{
		PastCameraUtility.LockConversation(this.m_pilot.CenterPosition);
	}

	// Token: 0x060065D2 RID: 26066 RVA: 0x00278D5C File Offset: 0x00276F5C
	private void HandleBossCutscene()
	{
		base.StartCoroutine(this.BossCutscene_CR());
	}

	// Token: 0x060065D3 RID: 26067 RVA: 0x00278D6C File Offset: 0x00276F6C
	private IEnumerator BossCutscene_CR()
	{
		yield return null;
		this.TriggerBoss();
		yield break;
	}

	// Token: 0x060065D4 RID: 26068 RVA: 0x00278D88 File Offset: 0x00276F88
	private IEnumerator ArriveFromWarp(tk2dBaseSprite targetSprite, float duration)
	{
		AkSoundEngine.PostEvent("Play_BOSS_queenship_emerge_01", base.gameObject);
		Transform targetTransform = targetSprite.transform;
		targetSprite.renderer.enabled = true;
		float width = (Quaternion.Euler(0f, 0f, -1f * targetTransform.rotation.eulerAngles.z) * targetSprite.GetBounds().size).x;
		SpriteOutlineManager.ToggleOutlineRenderers(targetSprite, true);
		Vector3 targetPosition = targetTransform.position;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 1f, elapsed / duration, 1);
			targetTransform.position = targetTransform.position.WithX(targetPosition.x + Mathf.Lerp(width / 2f, 0f, t)).WithY(Mathf.Lerp(targetPosition.y + 60f, targetPosition.y, t));
			targetTransform.localScale = Quaternion.Euler(0f, 0f, -1f * targetTransform.rotation.eulerAngles.z) * Vector3.Lerp(new Vector3(0.1f, 10f, 1f), Vector3.one, t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060065D5 RID: 26069 RVA: 0x00278DB4 File Offset: 0x00276FB4
	public void OnBossKilled()
	{
		if (!this.m_pilot.gameObject.activeSelf)
		{
			this.m_pilot.ResurrectFromBossKill();
		}
		if (this.m_coop && !this.m_coop.gameObject.activeSelf)
		{
			this.m_coop.ResurrectFromBossKill();
		}
		base.StartCoroutine(this.HandleBossKilled());
	}

	// Token: 0x060065D6 RID: 26070 RVA: 0x00278E20 File Offset: 0x00277020
	private IEnumerator HandleBossKilled()
	{
		GameStatsManager.Instance.SetCharacterSpecificFlag(PlayableCharacters.Pilot, CharacterSpecificGungeonFlags.KILLED_PAST, true);
		GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_ROGUE_PAST, true);
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_KILLED_PAST, 1f);
		this.ToggleFriendAndJunk(true);
		this.m_pilot.SetInputOverride("past");
		if (this.m_coop != null)
		{
			this.m_coop.SetInputOverride("past");
		}
		this.m_pilot.WarpToPoint(this.FriendTalker.specRigidbody.UnitCenter + new Vector2(3f, 0f), false, false);
		if (this.m_coop != null)
		{
			this.m_coop.WarpToPoint(this.m_pilot.specRigidbody.UnitCenter + new Vector2(3f, 0f), false, false);
		}
		this.FriendTalker.gameObject.SetActive(true);
		while (GameManager.Instance.MainCameraController.ManualControl)
		{
			yield return null;
		}
		GameManager.Instance.MainCameraController.OverrideZoomScale = 1f;
		yield return null;
		PastCameraUtility.LockConversation(this.m_pilot.CenterPosition);
		GameManager.Instance.MainCameraController.OverridePosition = this.m_pilot.CenterPosition;
		yield return new WaitForSeconds(1f);
		this.FriendTalker.Interact(this.m_pilot);
		GameManager.Instance.MainCameraController.OverridePosition = this.m_pilot.CenterPosition;
		while (this.FriendTalker.IsTalking)
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		Pixelator.Instance.FreezeFrame();
		BraveTime.RegisterTimeScaleMultiplier(0f, base.gameObject);
		float ela = 0f;
		while (ela < ConvictPastController.FREEZE_FRAME_DURATION)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		BraveTime.ClearMultiplier(base.gameObject);
		TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
		Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
		yield return base.StartCoroutine(ttcc.HandleTimeTubeCredits(GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, false, null, -1, false));
		AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		yield break;
	}

	// Token: 0x060065D7 RID: 26071 RVA: 0x00278E3C File Offset: 0x0027703C
	private HealthHaver GetBoss()
	{
		List<HealthHaver> allHealthHavers = StaticReferenceManager.AllHealthHavers;
		for (int i = 0; i < allHealthHavers.Count; i++)
		{
			if (allHealthHavers[i].IsBoss)
			{
				GenericIntroDoer component = allHealthHavers[i].GetComponent<GenericIntroDoer>();
				if (component && component.triggerType == GenericIntroDoer.TriggerType.BossTriggerZone)
				{
					return allHealthHavers[i];
				}
			}
		}
		return null;
	}

	// Token: 0x060065D8 RID: 26072 RVA: 0x00278EA8 File Offset: 0x002770A8
	private void TriggerBoss()
	{
		if (this.m_hasTriggeredBoss)
		{
			return;
		}
		List<HealthHaver> allHealthHavers = StaticReferenceManager.AllHealthHavers;
		for (int i = 0; i < allHealthHavers.Count; i++)
		{
			if (allHealthHavers[i].IsBoss)
			{
				GenericIntroDoer component = allHealthHavers[i].GetComponent<GenericIntroDoer>();
				if (component && component.triggerType == GenericIntroDoer.TriggerType.BossTriggerZone)
				{
					component.gameObject.SetActive(true);
					ObjectVisibilityManager component2 = component.GetComponent<ObjectVisibilityManager>();
					component2.ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
					if (!SpriteOutlineManager.HasOutline(component.aiAnimator.sprite))
					{
						SpriteOutlineManager.AddOutlineToSprite(component.aiAnimator.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
					}
					component.aiAnimator.renderer.enabled = false;
					SpriteOutlineManager.ToggleOutlineRenderers(component.aiAnimator.sprite, false);
					component.aiAnimator.ChildAnimator.renderer.enabled = false;
					SpriteOutlineManager.ToggleOutlineRenderers(component.aiAnimator.ChildAnimator.sprite, false);
					component.TriggerSequence(GameManager.Instance.PrimaryPlayer);
					this.m_hasTriggeredBoss = true;
					return;
				}
			}
		}
	}

	// Token: 0x04006187 RID: 24967
	public bool InstantBossFight;

	// Token: 0x04006188 RID: 24968
	public TalkDoerLite FriendTalker;

	// Token: 0x04006189 RID: 24969
	public TalkDoerLite HegemonyShip;

	// Token: 0x0400618A RID: 24970
	public tk2dSprite[] AdditionalHegemonyShips;

	// Token: 0x0400618B RID: 24971
	public GameObject FloatingCrap;

	// Token: 0x0400618C RID: 24972
	public tk2dSprite TheRock;

	// Token: 0x0400618D RID: 24973
	public Renderer Quad;

	// Token: 0x0400618E RID: 24974
	public Vector2 BackgroundScrollSpeed;

	// Token: 0x0400618F RID: 24975
	private PlayerController m_pilot;

	// Token: 0x04006190 RID: 24976
	private PlayerController m_coop;

	// Token: 0x04006191 RID: 24977
	private bool m_hasTriggeredBoss;

	// Token: 0x04006192 RID: 24978
	private Vector2 m_backgroundOffset;

	// Token: 0x04006193 RID: 24979
	private int m_scrollPositionXId = -1;

	// Token: 0x04006194 RID: 24980
	private int m_scrollPositionYId = -1;
}
