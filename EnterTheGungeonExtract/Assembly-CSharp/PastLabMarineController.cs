using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020011C8 RID: 4552
public class PastLabMarineController : MonoBehaviour
{
	// Token: 0x0600658C RID: 25996 RVA: 0x00277A3C File Offset: 0x00275C3C
	public void Engage()
	{
		this.m_bulletBank = base.GetComponent<AIBulletBank>();
		this.m_inCombat = true;
		this.AreaDoor.DoSeal(GameManager.Instance.Dungeon.data.Entrance);
		base.StartCoroutine(this.HandlePortal());
		HealthHaver healthHaver = StaticReferenceManager.AllHealthHavers.Find((HealthHaver h) => h.IsBoss);
		healthHaver.GetComponent<ObjectVisibilityManager>().ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
		healthHaver.GetComponent<GenericIntroDoer>().TriggerSequence(GameManager.Instance.PrimaryPlayer);
		for (int i = 0; i < this.BossCollisionExceptions.Length; i++)
		{
			healthHaver.specRigidbody.RegisterSpecificCollisionException(this.BossCollisionExceptions[i]);
		}
		base.StartCoroutine(this.HandleDialogue("#PRIMERDYNE_MARINE_ENTRY_01"));
	}

	// Token: 0x0600658D RID: 25997 RVA: 0x00277B10 File Offset: 0x00275D10
	private IEnumerator HandlePortal()
	{
		float ela = 0f;
		float dura = 0.5f;
		while (ela < dura)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		this.TerrorPortal.gameObject.SetActive(true);
		this.TerrorPortal.ignoreTimeScale = true;
		(this.TerrorPortal.Sprite as tk2dSprite).GenerateUV2 = true;
		this.TerrorPortal.sprite.usesOverrideMaterial = true;
		ela = 0f;
		dura = 5f;
		Vector4 localTime = new Vector4(0f, 0f, 0f, 0f);
		while (ela < dura)
		{
			float ivdt = GameManager.INVARIANT_DELTA_TIME;
			ela += ivdt;
			localTime += new Vector4(ivdt / 20f, ivdt, ivdt * 2f, ivdt * 3f);
			this.TerrorPortal.sprite.renderer.material.SetVector("_LocalTime", localTime);
			yield return null;
		}
		this.TerrorPortal.PlayAndDisableObject("portal_out", null);
		yield break;
	}

	// Token: 0x0600658E RID: 25998 RVA: 0x00277B2C File Offset: 0x00275D2C
	private IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		base.StartCoroutine(this.HandleInitialRoomLockdown());
		Pixelator.Instance.TriggerPastFadeIn();
		float shakeTimer = UnityEngine.Random.Range(this.MinTimeBetweenAmbientShakes, this.MaxTimeBetweenAmbientShakes);
		while (GameManager.Instance.PrimaryPlayer.CenterPosition.y < this.LeftRedMarine.transform.position.y)
		{
			shakeTimer -= BraveTime.DeltaTime;
			if (shakeTimer <= 0f)
			{
				GameManager.Instance.MainCameraController.DoScreenShake(this.AmbientScreenShakeSettings, null, false);
				shakeTimer += UnityEngine.Random.Range(this.MinTimeBetweenAmbientShakes, this.MaxTimeBetweenAmbientShakes);
			}
			yield return null;
		}
		this.Engage();
		yield break;
	}

	// Token: 0x0600658F RID: 25999 RVA: 0x00277B48 File Offset: 0x00275D48
	private IEnumerator HandleInitialRoomLockdown()
	{
		this.CellDoor.SetSealedSilently(true);
		yield return new WaitForSeconds(5f);
		this.CellDoor.SetSealedSilently(false);
		this.CellDoor.Open(false);
		yield break;
	}

	// Token: 0x06006590 RID: 26000 RVA: 0x00277B64 File Offset: 0x00275D64
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
		if (this.m_inCombat && !this.m_occupied)
		{
			if (UnityEngine.Random.value > 0.5f && this.m_idleCounter < 2)
			{
				base.StartCoroutine(this.HandleHide());
			}
			else
			{
				base.StartCoroutine(this.HandleShoot());
			}
		}
		if (!this.m_hasRemarkedOnDoorway && GameManager.Instance.PrimaryPlayer.transform.position.x > 70f)
		{
			this.m_hasRemarkedOnDoorway = true;
			this.MakeSoldierTalkAmbient("#PRIMERDYNE_MARINE_CANT_LEAVE", 5f, true);
		}
	}

	// Token: 0x06006591 RID: 26001 RVA: 0x00277C48 File Offset: 0x00275E48
	private IEnumerator HandleHide()
	{
		this.m_idleCounter++;
		this.m_occupied = true;
		this.LeftGreenMarine.Play("marine_green_left");
		this.RightGreenMarine.Play("marine_green_right");
		yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 3f));
		this.m_occupied = false;
		yield break;
	}

	// Token: 0x06006592 RID: 26002 RVA: 0x00277C64 File Offset: 0x00275E64
	private IEnumerator HandleShoot()
	{
		bool rightMarineFiring = UnityEngine.Random.value > 0.33f;
		bool leftMarineFiring = UnityEngine.Random.value <= 0.33f || UnityEngine.Random.value > 0.66f;
		this.m_idleCounter = 0;
		this.m_occupied = true;
		this.LeftGreenMarine.Play("marine_green_left");
		this.RightGreenMarine.Play("marine_green_right");
		if (leftMarineFiring)
		{
			this.LeftRedMarine.PlayForDuration("marine_red_left_fire", -1f, "marine_red_left", false);
		}
		if (rightMarineFiring)
		{
			this.RightRedMarine.PlayForDuration("marine_red_right_fire", -1f, "marine_red_right", false);
		}
		yield return new WaitForSeconds(0.15f);
		while (this.LeftRedMarine.IsPlaying("marine_red_left_fire") && this.LeftRedMarine.CurrentFrame < 28)
		{
			if (leftMarineFiring)
			{
				this.FireBullet(this.LeftRedMarineShootPoint, new Vector2(1f, 1.2f));
			}
			if (rightMarineFiring)
			{
				this.FireBullet(this.RightRedMarineShootPoint, new Vector2(-1f, 1.1f));
			}
			yield return new WaitForSeconds(0.15f);
		}
		yield return new WaitForSeconds(UnityEngine.Random.Range(0.25f, 1f));
		this.m_occupied = false;
		yield break;
	}

	// Token: 0x06006593 RID: 26003 RVA: 0x00277C80 File Offset: 0x00275E80
	private void FireBullet(Transform shootPoint, Vector2 dirVec)
	{
		GameObject gameObject = this.m_bulletBank.CreateProjectileFromBank(shootPoint.position, BraveMathCollege.Atan2Degrees(dirVec.normalized) + UnityEngine.Random.Range(-10f, 10f), "default", null, false, true, false);
		gameObject.GetComponent<Projectile>().collidesWithPlayer = false;
	}

	// Token: 0x06006594 RID: 26004 RVA: 0x00277CD8 File Offset: 0x00275ED8
	public void MakeSoldierTalkAmbient(string stringKey, float duration = 3f, bool isThoughtBubble = false)
	{
		this.DoAmbientTalk(GameManager.Instance.PrimaryPlayer.transform, new Vector3(0.75f, 1.5f, 0f), stringKey, duration, isThoughtBubble);
	}

	// Token: 0x06006595 RID: 26005 RVA: 0x00277D08 File Offset: 0x00275F08
	public void DoAmbientTalk(Transform baseTransform, Vector3 offset, string stringKey, float duration, bool isThoughtBubble = false)
	{
		if (isThoughtBubble)
		{
			Vector3 vector = baseTransform.position + offset;
			float num = -1f;
			string @string = StringTableManager.GetString(stringKey);
			bool flag = false;
			string characterAudioSpeechTag = GameManager.Instance.PrimaryPlayer.characterAudioSpeechTag;
			TextBoxManager.ShowThoughtBubble(vector, baseTransform, num, @string, flag, false, characterAudioSpeechTag);
		}
		else
		{
			TextBoxManager.ShowTextBox(baseTransform.position + offset, baseTransform, -1f, StringTableManager.GetString(stringKey), GameManager.Instance.PrimaryPlayer.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		}
		base.StartCoroutine(this.HandleManualTalkDuration(baseTransform, duration));
	}

	// Token: 0x06006596 RID: 26006 RVA: 0x00277DA0 File Offset: 0x00275FA0
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

	// Token: 0x06006597 RID: 26007 RVA: 0x00277DCC File Offset: 0x00275FCC
	private IEnumerator HandleDialogue(string stringKey)
	{
		this.m_occupied = true;
		TextBoxManager.ShowTextBox(this.SpeakPoints[0].position, this.SpeakPoints[0], 3f, StringTableManager.GetString(stringKey), string.Empty, true, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		yield return new WaitForSeconds(3f);
		this.m_occupied = false;
		yield break;
	}

	// Token: 0x06006598 RID: 26008 RVA: 0x00277DF0 File Offset: 0x00275FF0
	public void OnBossKilled()
	{
		this.m_inCombat = false;
		GameStatsManager.Instance.SetCharacterSpecificFlag(PlayableCharacters.Soldier, CharacterSpecificGungeonFlags.KILLED_PAST, true);
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_KILLED_PAST, 1f);
		GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_SOLDIER_PAST, true);
		base.StartCoroutine(this.HandleBossKilled());
	}

	// Token: 0x06006599 RID: 26009 RVA: 0x00277E44 File Offset: 0x00276044
	private IEnumerator HandleBossKilled()
	{
		yield return new WaitForSeconds(3.5f);
		GameManager.Instance.PrimaryPlayer.transform.position = this.AreaDoor.transform.position + new Vector3(0f, 11f, 0f);
		GameManager.Instance.PrimaryPlayer.specRigidbody.Reinitialize();
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.SecondaryPlayer.transform.position = GameManager.Instance.PrimaryPlayer.transform.position + new Vector3(2.5f, 0f, 0f);
			GameManager.Instance.SecondaryPlayer.specRigidbody.Reinitialize();
		}
		this.LeftGreenMarine.gameObject.SetActive(false);
		this.LeftRedMarine.gameObject.SetActive(false);
		this.RightRedMarine.gameObject.SetActive(false);
		this.RightGreenMarine.gameObject.SetActive(false);
		this.VictoryTalkDoer.gameObject.SetActive(true);
		PlayerController m_soldier = GameManager.Instance.PrimaryPlayer;
		PastCameraUtility.LockConversation(m_soldier.CenterPosition);
		GameManager.Instance.MainCameraController.SetManualControl(true, false);
		GameManager.Instance.MainCameraController.OverridePosition = m_soldier.CenterPosition;
		yield return new WaitForSeconds(0.5f);
		GameManager.Instance.MainCameraController.SetManualControl(true, false);
		PastCameraUtility.LockConversation(m_soldier.CenterPosition);
		Pixelator.Instance.FadeToColor(2f, Color.white, true, 0f);
		yield return new WaitForSeconds(2f);
		this.VictoryTalkDoer.Interact(GameManager.Instance.PrimaryPlayer);
		GameManager.Instance.MainCameraController.OverridePosition = m_soldier.CenterPosition;
		while (this.VictoryTalkDoer.IsTalking)
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
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
		ttcc.ClearDebris();
		yield return base.StartCoroutine(ttcc.HandleTimeTubeCredits(m_soldier.sprite.WorldCenter, false, null, -1, false));
		AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		yield break;
	}

	// Token: 0x04006143 RID: 24899
	public tk2dSpriteAnimator LeftRedMarine;

	// Token: 0x04006144 RID: 24900
	public tk2dSpriteAnimator LeftGreenMarine;

	// Token: 0x04006145 RID: 24901
	public tk2dSpriteAnimator RightRedMarine;

	// Token: 0x04006146 RID: 24902
	public tk2dSpriteAnimator RightGreenMarine;

	// Token: 0x04006147 RID: 24903
	public Transform LeftRedMarineShootPoint;

	// Token: 0x04006148 RID: 24904
	public Transform RightRedMarineShootPoint;

	// Token: 0x04006149 RID: 24905
	public DungeonDoorController AreaDoor;

	// Token: 0x0400614A RID: 24906
	public DungeonDoorController CellDoor;

	// Token: 0x0400614B RID: 24907
	public Transform[] SpeakPoints;

	// Token: 0x0400614C RID: 24908
	public TalkDoerLite VictoryTalkDoer;

	// Token: 0x0400614D RID: 24909
	public SpeculativeRigidbody[] BossCollisionExceptions;

	// Token: 0x0400614E RID: 24910
	public ScreenShakeSettings AmbientScreenShakeSettings;

	// Token: 0x0400614F RID: 24911
	public float MinTimeBetweenAmbientShakes = 3f;

	// Token: 0x04006150 RID: 24912
	public float MaxTimeBetweenAmbientShakes = 5f;

	// Token: 0x04006151 RID: 24913
	public tk2dSpriteAnimator TerrorPortal;

	// Token: 0x04006152 RID: 24914
	private bool m_inCombat;

	// Token: 0x04006153 RID: 24915
	private bool m_occupied;

	// Token: 0x04006154 RID: 24916
	private int m_idleCounter;

	// Token: 0x04006155 RID: 24917
	private AIBulletBank m_bulletBank;

	// Token: 0x04006156 RID: 24918
	private bool m_hasRemarkedOnDoorway;

	// Token: 0x04006157 RID: 24919
	private bool m_forceSkip;
}
