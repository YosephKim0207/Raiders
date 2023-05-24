using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02001179 RID: 4473
public class GuidePastController : MonoBehaviour
{
	// Token: 0x06006348 RID: 25416 RVA: 0x00268230 File Offset: 0x00266430
	private IEnumerator Start()
	{
		this.m_transform = base.transform;
		this.m_guide = GameManager.Instance.PrimaryPlayer;
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.m_coop = GameManager.Instance.SecondaryPlayer;
		}
		this.m_dog = ((this.m_guide.companions.Count <= 0) ? null : this.m_guide.companions[0]);
		PlayerCommentInteractable targetInteractable = this.TargetInteractable;
		targetInteractable.OnInteractionBegan = (Action)Delegate.Combine(targetInteractable.OnInteractionBegan, new Action(this.SetupCutscene));
		PlayerCommentInteractable targetInteractable2 = this.TargetInteractable;
		targetInteractable2.OnInteractionFinished = (Action)Delegate.Combine(targetInteractable2.OnInteractionFinished, new Action(this.HandleBossCutscene));
		List<HealthHaver> healthHavers = StaticReferenceManager.AllHealthHavers;
		for (int i = 0; i < healthHavers.Count; i++)
		{
			if (!healthHavers[i].IsBoss && healthHavers[i].name.Contains("DrWolf", true))
			{
				healthHavers[i].specRigidbody.CollideWithOthers = false;
				healthHavers[i].aiActor.IsGone = true;
				break;
			}
		}
		yield return null;
		Pixelator.Instance.TriggerPastFadeIn();
		for (int j = 0; j < StaticReferenceManager.AllEnemies.Count; j++)
		{
			if (StaticReferenceManager.AllEnemies[j].ActorName == "Dr. Wolf")
			{
				this.DrWolfEnemyRigidbody = StaticReferenceManager.AllEnemies[j].specRigidbody;
			}
		}
		this.DrWolfEnemyRigidbody.enabled = false;
		this.GetBoss().gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06006349 RID: 25417 RVA: 0x0026824C File Offset: 0x0026644C
	private void SetupCutscene()
	{
		PastCameraUtility.LockConversation(this.DrWolfTalkDoer.speakPoint.transform.position.XY() + new Vector2(0f, 15.5f));
		this.DrWolfTalkDoer.gameObject.SetActive(true);
	}

	// Token: 0x0600634A RID: 25418 RVA: 0x002682A0 File Offset: 0x002664A0
	private void HandleBossCutscene()
	{
		PlayerCommentInteractable[] array = UnityEngine.Object.FindObjectsOfType<PlayerCommentInteractable>();
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ForceDisable();
			}
		}
		base.StartCoroutine(this.BossCutscene_CR());
	}

	// Token: 0x0600634B RID: 25419 RVA: 0x002682E4 File Offset: 0x002664E4
	private IEnumerator BossCutscene_CR()
	{
		Vector2 wolfLastPos = this.DrWolfTalkDoer.specRigidbody.UnitCenter;
		this.DrWolfTalkDoer.PathfindToPosition(wolfLastPos + new Vector2(0f, 12f), null, null);
		while (this.DrWolfTalkDoer.CurrentPath != null)
		{
			this.DrWolfTalkDoer.specRigidbody.Velocity = this.DrWolfTalkDoer.GetPathVelocityContribution(wolfLastPos, 8);
			wolfLastPos = this.DrWolfTalkDoer.specRigidbody.UnitCenter;
			yield return null;
		}
		GameManager.Instance.PrimaryPlayer.ForceIdleFacePoint(Vector2.down, true);
		this.DrWolfTalkDoer.Interact(GameManager.Instance.PrimaryPlayer);
		while (this.DrWolfTalkDoer.IsTalking)
		{
			yield return null;
		}
		this.m_trapActive = true;
		base.StartCoroutine(this.HandleBulletTrap());
		base.StartCoroutine(this.HandleDogGoingNuts());
		wolfLastPos = this.DrWolfTalkDoer.specRigidbody.UnitCenter;
		CellValidator cellValidator = (IntVector2 a) => IntVector2.Distance(a, GameManager.Instance.PrimaryPlayer.CenterPosition.ToIntVector2(VectorConversions.Round)) > 8f;
		TalkDoerLite drWolfTalkDoer = this.DrWolfTalkDoer;
		Vector2 vector = wolfLastPos + new Vector2(-7f, 9.75f);
		CellValidator cellValidator2 = cellValidator;
		drWolfTalkDoer.PathfindToPosition(vector, null, cellValidator2);
		GameManager.Instance.MainCameraController.UpdateOverridePosition(GameManager.Instance.MainCameraController.OverridePosition + new Vector3(0f, 5f, 0f), 4f);
		while (this.DrWolfTalkDoer.CurrentPath != null)
		{
			this.DrWolfTalkDoer.specRigidbody.Velocity = this.DrWolfTalkDoer.GetPathVelocityContribution(wolfLastPos, 16);
			wolfLastPos = this.DrWolfTalkDoer.specRigidbody.UnitCenter;
			yield return null;
		}
		this.DrWolfTalkDoer.Interact(GameManager.Instance.PrimaryPlayer);
		while (this.DrWolfTalkDoer.IsTalking)
		{
			yield return null;
		}
		GameManager.Instance.PrimaryPlayer.ForceBlank(25f, 0.5f, false, true, null, true, -1f);
		yield return new WaitForSeconds(1f);
		this.m_trapActive = false;
		this.DrWolfTalkDoer.Interact(GameManager.Instance.PrimaryPlayer);
		while (this.DrWolfTalkDoer.IsTalking)
		{
			yield return null;
		}
		PastCameraUtility.UnlockConversation();
		this.DrWolfTalkDoer.specRigidbody.enabled = false;
		this.DrWolfTalkDoer.gameObject.SetActive(false);
		this.TriggerBoss();
		yield break;
	}

	// Token: 0x0600634C RID: 25420 RVA: 0x00268300 File Offset: 0x00266500
	private IEnumerator HandleDogGoingNuts()
	{
		if (this.m_dog == null)
		{
			this.m_dog = ((this.m_guide.companions.Count <= 0) ? null : this.m_guide.companions[0]);
		}
		this.m_dog.ClearPath();
		AkSoundEngine.PostEvent("Play_BOSS_energy_shield_01", GameManager.Instance.gameObject);
		AkSoundEngine.PostEvent("Play_PET_dog_bark_02", GameManager.Instance.gameObject);
		Vector2 trapPoint = GameManager.Instance.PrimaryPlayer.CenterPosition;
		int currentTargetPoint = 6;
		this.m_dog.behaviorSpeculator.InterruptAndDisable();
		float cachedMovementSpeed = this.m_dog.MovementSpeed;
		this.m_dog.MovementSpeed = cachedMovementSpeed;
		while (this.m_trapActive)
		{
			if (this.m_dog && this.m_dog.PathComplete)
			{
				Vector2 vector = trapPoint + (Quaternion.Euler(0f, 0f, (float)(45 * currentTargetPoint)) * Vector2.right).XY() * 5.5f;
				this.m_dog.PathfindToPosition(vector, null, true, null, null, null, false);
				currentTargetPoint = (currentTargetPoint + 1) % 8;
			}
			yield return null;
		}
		this.m_dog.MovementSpeed = cachedMovementSpeed;
		this.m_dog.behaviorSpeculator.enabled = true;
		yield break;
	}

	// Token: 0x0600634D RID: 25421 RVA: 0x0026831C File Offset: 0x0026651C
	private IEnumerator HandleBulletTrap()
	{
		float elapsed = 0f;
		float duration = 1f;
		Vector2 center = GameManager.Instance.PrimaryPlayer.CenterPosition;
		float lastAngle = 0f;
		GameObject fakeBulletPrefab = (GameObject)BraveResources.Load("Global Prefabs/FakeBullet", ".prefab");
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float newAngle = Mathf.Lerp(0f, 360f, elapsed / duration);
			int num = Mathf.CeilToInt(lastAngle);
			while ((float)num < newAngle)
			{
				if (num % 18 == 0)
				{
					for (int i = 0; i < 3; i++)
					{
						float num2 = (float)(2 + i);
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(fakeBulletPrefab, center.ToVector3ZUp(0f) + Quaternion.Euler(0f, 0f, (float)(num + i * 120)) * Vector3.right * num2, Quaternion.identity);
						this.m_fakeBullets.Add(gameObject);
					}
				}
				num++;
			}
			lastAngle = newAngle;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600634E RID: 25422 RVA: 0x00268338 File Offset: 0x00266538
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

	// Token: 0x0600634F RID: 25423 RVA: 0x002683A4 File Offset: 0x002665A4
	private void TriggerBoss()
	{
		if (this.m_hasTriggeredBoss)
		{
			return;
		}
		this.DrWolfEnemyRigidbody.enabled = true;
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

	// Token: 0x06006350 RID: 25424 RVA: 0x002684D4 File Offset: 0x002666D4
	public void OnBossKilled()
	{
		base.StartCoroutine(this.HandleBossKilled());
	}

	// Token: 0x06006351 RID: 25425 RVA: 0x002684E4 File Offset: 0x002666E4
	private IEnumerator HandleBossKilled()
	{
		GameStatsManager.Instance.SetCharacterSpecificFlag(PlayableCharacters.Guide, CharacterSpecificGungeonFlags.KILLED_PAST, true);
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_KILLED_PAST, 1f);
		GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_GUIDE_PAST, true);
		this.PhantomEndTalkDoer.gameObject.SetActive(true);
		this.PhantomEndTalkDoer.ShowOutlines = false;
		SpriteOutlineManager.RemoveOutlineFromSprite(this.PhantomEndTalkDoer.sprite, true);
		yield return null;
		SpriteOutlineManager.RemoveOutlineFromSprite(this.PhantomEndTalkDoer.sprite, true);
		while (GameManager.Instance.MainCameraController.ManualControl)
		{
			yield return null;
		}
		this.m_guide.WarpToPoint(this.TargetInteractable.specRigidbody.UnitCenter + new Vector2(-0.5f, -2.5f), false, false);
		this.m_guide.ForceIdleFacePoint(Vector2.down, true);
		if (this.m_coop != null)
		{
			this.m_coop.WarpToPoint(this.m_guide.specRigidbody.UnitCenter + new Vector2(-3f, 0f), false, false);
			this.m_coop.ForceIdleFacePoint(Vector2.down, true);
		}
		if (this.m_dog != null)
		{
			this.m_dog.CompanionWarp(this.m_guide.CenterPosition.ToVector3ZUp(0f) + new Vector3(1f, -0.5f, 0f));
		}
		yield return null;
		PastCameraUtility.LockConversation(this.m_guide.CenterPosition);
		GameManager.Instance.MainCameraController.OverridePosition = this.m_guide.CenterPosition;
		yield return new WaitForSeconds(1f);
		Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
		yield return new WaitForSeconds(1f);
		this.PhantomEndTalkDoer.Interact(this.m_guide);
		GameManager.Instance.MainCameraController.OverridePosition = this.m_guide.CenterPosition;
		this.m_guide.ForceIdleFacePoint(Vector2.down, true);
		Debug.Log("are we talking? " + this.PhantomEndTalkDoer.IsTalking);
		while (this.PhantomEndTalkDoer.IsTalking)
		{
			yield return null;
		}
		this.m_guide.ForceMoveInDirectionUntilThreshold(Vector2.down, this.m_guide.transform.position.y - 5f, 10f, 1f, null);
		float ela = 0f;
		while (ela < 0.5f)
		{
			GameManager.Instance.MainCameraController.OverridePosition = this.m_guide.CenterPosition;
			ela += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		Pixelator.Instance.FreezeFrame();
		BraveTime.RegisterTimeScaleMultiplier(0f, base.gameObject);
		ela = 0f;
		while (ela < ConvictPastController.FREEZE_FRAME_DURATION)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		BraveTime.ClearMultiplier(base.gameObject);
		TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
		ttcc.ClearDebris();
		Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
		yield return base.StartCoroutine(ttcc.HandleTimeTubeCredits(GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, false, null, -1, false));
		AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		yield break;
	}

	// Token: 0x06006352 RID: 25426 RVA: 0x00268500 File Offset: 0x00266700
	public void MakeGuideTalkAmbient(string stringKey, float duration = 3f, bool isThoughtBubble = false)
	{
		this.DoAmbientTalk(this.m_guide.transform, new Vector3(0.75f, 1.5f, 0f), stringKey, duration, isThoughtBubble, GameManager.Instance.PrimaryPlayer.characterAudioSpeechTag);
	}

	// Token: 0x06006353 RID: 25427 RVA: 0x0026853C File Offset: 0x0026673C
	public void MakeDogTalk(string stringKey, float duration = 3f)
	{
		this.DoAmbientTalk(this.m_dog.transform, new Vector3(0.25f, 1f, 0f), stringKey, duration, false, string.Empty);
	}

	// Token: 0x06006354 RID: 25428 RVA: 0x0026856C File Offset: 0x0026676C
	public void DoAmbientTalk(Transform baseTransform, Vector3 offset, string stringKey, float duration, bool isThoughtBubble = false, string audioTag = "")
	{
		if (isThoughtBubble)
		{
			Vector3 vector = baseTransform.position + offset;
			float num = -1f;
			string @string = StringTableManager.GetString(stringKey);
			bool flag = false;
			TextBoxManager.ShowThoughtBubble(vector, baseTransform, num, @string, flag, false, audioTag);
		}
		else
		{
			TextBoxManager.ShowTextBox(baseTransform.position + offset, baseTransform, -1f, StringTableManager.GetString(stringKey), audioTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		}
		base.StartCoroutine(this.HandleManualTalkDuration(baseTransform, duration));
	}

	// Token: 0x06006355 RID: 25429 RVA: 0x002685EC File Offset: 0x002667EC
	private IEnumerator HandleManualTalkDuration(Transform source, float duration)
	{
		float ela = 0f;
		while (ela < duration)
		{
			ela += BraveTime.DeltaTime;
			yield return null;
			if (this.m_forceSkip)
			{
				ela += duration;
			}
		}
		TextBoxManager.ClearTextBox(source);
		this.m_forceSkip = false;
		yield break;
	}

	// Token: 0x06006356 RID: 25430 RVA: 0x00268618 File Offset: 0x00266818
	private IEnumerator HandleIntroConversation()
	{
		if (this.m_dog == null)
		{
			this.m_dog = ((GameManager.Instance.PrimaryPlayer.companions.Count <= 0) ? null : GameManager.Instance.PrimaryPlayer.companions[0]);
		}
		if (this.m_dog != null)
		{
			this.MakeDogTalk("#DOG_YIPYIP", 3f);
			AkSoundEngine.PostEvent("Play_PET_dog_bark_02", GameManager.Instance.gameObject);
			yield return new WaitForSeconds(1f);
			this.MakeGuideTalkAmbient("#GUIDEPAST_ZIPIT", 3f, false);
			yield return new WaitForSeconds(3f);
		}
		yield break;
	}

	// Token: 0x06006357 RID: 25431 RVA: 0x00268634 File Offset: 0x00266834
	private IEnumerator HandleAntechamberConversation()
	{
		this.MakeGuideTalkAmbient("#GUIDEPAST_OMINOUS", 3f, false);
		yield return new WaitForSeconds(2f);
		if (this.m_dog != null)
		{
			this.MakeDogTalk("#DOG_YIP", 3f);
		}
		AkSoundEngine.PostEvent("Play_PET_dog_bark_02", GameManager.Instance.gameObject);
		yield break;
	}

	// Token: 0x06006358 RID: 25432 RVA: 0x00268650 File Offset: 0x00266850
	private void Update()
	{
		this.m_forceSkip = false;
		bool flag = BraveInput.GetInstanceForPlayer(0).WasAdvanceDialoguePressed();
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			flag |= BraveInput.GetInstanceForPlayer(1).WasAdvanceDialoguePressed();
		}
		if (flag)
		{
			this.m_forceSkip = true;
		}
		float num = ((!GameManager.IsBossIntro) ? BraveTime.DeltaTime : GameManager.INVARIANT_DELTA_TIME);
		if (this.SpinnyGreenMachinePart && num > 0f)
		{
			int num2 = Mathf.CeilToInt(15f * num);
			GlobalSparksDoer.DoRandomParticleBurst(num2, this.SpinnyGreenMachinePart.WorldCenter + new Vector2(-0.5f, -0.5f), this.SpinnyGreenMachinePart.WorldCenter + new Vector2(0.5f, 0.5f), Vector3.up * 3f, 30f, 0.5f, null, null, new Color?(Color.green), GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT);
		}
		if (this.ArtifactSprite.renderer.enabled)
		{
			GlobalSparksDoer.DoRandomParticleBurst(Mathf.CeilToInt(Mathf.Max(1f, 80f * num)), this.ArtifactSprite.WorldBottomLeft.ToVector3ZisY(0f), this.ArtifactSprite.WorldTopRight.ToVector3ZisY(0f), Vector3.up, 180f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
		}
		if (Time.timeSinceLevelLoad > 0.5f)
		{
			if (!this.m_hasTriggeredInitial)
			{
				if (this.m_guide.transform.position.y > this.m_initialTriggerHeight + this.m_transform.position.y)
				{
					this.m_hasTriggeredInitial = true;
					base.StartCoroutine(this.HandleIntroConversation());
				}
			}
			else if (!this.m_hasTriggeredAntechamber && this.m_guide.transform.position.y > this.m_antechamberTriggerHeight + this.m_transform.position.y)
			{
				this.m_hasTriggeredAntechamber = true;
				base.StartCoroutine(this.HandleAntechamberConversation());
			}
		}
	}

	// Token: 0x04005ECA RID: 24266
	public PlayerCommentInteractable TargetInteractable;

	// Token: 0x04005ECB RID: 24267
	public tk2dSprite ArtifactSprite;

	// Token: 0x04005ECC RID: 24268
	public TalkDoerLite DrWolfTalkDoer;

	// Token: 0x04005ECD RID: 24269
	public SpeculativeRigidbody DrWolfEnemyRigidbody;

	// Token: 0x04005ECE RID: 24270
	public TalkDoerLite PhantomEndTalkDoer;

	// Token: 0x04005ECF RID: 24271
	public tk2dSprite SpinnyGreenMachinePart;

	// Token: 0x04005ED0 RID: 24272
	private PlayerController m_guide;

	// Token: 0x04005ED1 RID: 24273
	private PlayerController m_coop;

	// Token: 0x04005ED2 RID: 24274
	private AIActor m_dog;

	// Token: 0x04005ED3 RID: 24275
	private Transform m_transform;

	// Token: 0x04005ED4 RID: 24276
	private List<GameObject> m_fakeBullets = new List<GameObject>();

	// Token: 0x04005ED5 RID: 24277
	private bool m_hasTriggeredBoss;

	// Token: 0x04005ED6 RID: 24278
	private bool m_trapActive;

	// Token: 0x04005ED7 RID: 24279
	private bool m_forceSkip;

	// Token: 0x04005ED8 RID: 24280
	private float m_initialTriggerHeight = 8f;

	// Token: 0x04005ED9 RID: 24281
	private bool m_hasTriggeredInitial;

	// Token: 0x04005EDA RID: 24282
	private float m_antechamberTriggerHeight = 29f;

	// Token: 0x04005EDB RID: 24283
	private bool m_hasTriggeredAntechamber;
}
