using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001052 RID: 4178
public class LichDeathController : BraveBehaviour
{
	// Token: 0x17000D6A RID: 3434
	// (get) Token: 0x06005BD0 RID: 23504 RVA: 0x00232F58 File Offset: 0x00231158
	// (set) Token: 0x06005BD1 RID: 23505 RVA: 0x00232F60 File Offset: 0x00231160
	public bool IsDoubleFinalDeath { get; set; }

	// Token: 0x06005BD2 RID: 23506 RVA: 0x00232F6C File Offset: 0x0023116C
	public IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		GameManager.Instance.Dungeon.StartCoroutine(this.LateStart());
		yield break;
	}

	// Token: 0x06005BD3 RID: 23507 RVA: 0x00232F88 File Offset: 0x00231188
	public IEnumerator LateStart()
	{
		yield return null;
		List<AIActor> allActors = StaticReferenceManager.AllEnemies;
		for (int i = 0; i < allActors.Count; i++)
		{
			if (allActors[i])
			{
				MegalichDeathController component = allActors[i].GetComponent<MegalichDeathController>();
				if (component)
				{
					this.m_megalich = component;
					break;
				}
			}
		}
		RoomHandler lichRoom = base.aiActor.ParentRoom;
		RoomHandler megalichRoom = this.m_megalich.aiActor.ParentRoom;
		megalichRoom.AddDarkSoulsRoomResetDependency(lichRoom);
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		yield break;
	}

	// Token: 0x06005BD4 RID: 23508 RVA: 0x00232FA4 File Offset: 0x002311A4
	protected override void OnDestroy()
	{
		if (ChallengeManager.CHALLENGE_MODE_ACTIVE && this.m_challengesSuppressed)
		{
			ChallengeManager.Instance.SuppressChallengeStart = false;
			this.m_challengesSuppressed = false;
		}
		base.OnDestroy();
	}

	// Token: 0x06005BD5 RID: 23509 RVA: 0x00232FD4 File Offset: 0x002311D4
	private void OnBossDeath(Vector2 dir)
	{
		if (LichIntroDoer.DoubleLich)
		{
			this.IsDoubleFinalDeath = true;
			foreach (AIActor aiactor in base.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
			{
				if (aiactor.healthHaver.IsBoss && !aiactor.healthHaver.IsSubboss && aiactor != base.aiActor && aiactor.healthHaver.IsAlive)
				{
					this.IsDoubleFinalDeath = false;
					break;
				}
			}
		}
		if (!LichIntroDoer.DoubleLich || this.IsDoubleFinalDeath)
		{
			AkSoundEngine.PostEvent("Play_MUS_Lich_Transition_01", GameManager.Instance.gameObject);
		}
		if (this.IsDoubleFinalDeath)
		{
			base.aiAnimator.PlayUntilCancelled("death_real", true, null, -1f, false);
			base.healthHaver.OverrideKillCamTime = new float?(11.5f);
			GameManager.Instance.StartCoroutine(this.HandleDoubleLichPostDeathCR());
			GameManager.Instance.StartCoroutine(this.HandleDoubleLichExtraExplosionsCR());
		}
		else
		{
			base.aiAnimator.PlayUntilCancelled("death", true, null, -1f, false);
			GameManager.Instance.StartCoroutine(this.HandlePostDeathCR());
		}
	}

	// Token: 0x06005BD6 RID: 23510 RVA: 0x00233148 File Offset: 0x00231348
	private IEnumerator HandlePostDeathCR()
	{
		if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
		{
			ChallengeManager.Instance.ForceStop();
		}
		tk2dBaseSprite shadowSprite = base.aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
		while (base.aiAnimator.IsPlaying("death"))
		{
			shadowSprite.color = shadowSprite.color.WithAlpha(1f - base.aiAnimator.CurrentClipProgress);
			float progress = base.aiAnimator.CurrentClipProgress;
			if (progress < 0.4f)
			{
				GlobalSparksDoer.DoRandomParticleBurst((int)(200f * Time.deltaTime), base.transform.position + new Vector3(4.5f, 4.5f), base.transform.position + new Vector3(5f, 5.5f), new Vector3(0f, 1f, 0f), 90f, 0.75f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
			}
			yield return null;
		}
		base.renderer.enabled = false;
		shadowSprite.color = shadowSprite.color.WithAlpha(0f);
		base.aiActor.StealthDeath = true;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		if (base.aiActor)
		{
			UnityEngine.Object.Destroy(base.aiActor);
		}
		if (base.healthHaver)
		{
			UnityEngine.Object.Destroy(base.healthHaver);
		}
		if (base.behaviorSpeculator)
		{
			UnityEngine.Object.Destroy(base.behaviorSpeculator);
		}
		if (base.aiAnimator.ChildAnimator)
		{
			UnityEngine.Object.Destroy(base.aiAnimator.ChildAnimator.gameObject);
		}
		if (base.aiAnimator)
		{
			UnityEngine.Object.Destroy(base.aiAnimator);
		}
		if (base.specRigidbody)
		{
			UnityEngine.Object.Destroy(base.specRigidbody);
		}
		base.RegenerateCache();
		if (LichIntroDoer.DoubleLich)
		{
			yield break;
		}
		yield return new WaitForSeconds(5f);
		GameManager.Instance.MainCameraController.DoContinuousScreenShake(this.hellDragScreenShake, this, false);
		yield return new WaitForSeconds(3f);
		SuperReaperController.PreventShooting = true;
		yield return new WaitForSeconds(1f);
		List<HellDraggerArbitrary> arbitraryGrabbers = new List<HellDraggerArbitrary>();
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (playerController && playerController.healthHaver.IsAlive)
			{
				playerController.CurrentInputState = PlayerInputState.NoInput;
				HellDraggerArbitrary component = UnityEngine.Object.Instantiate<GameObject>(this.HellDragVFX).GetComponent<HellDraggerArbitrary>();
				component.Do(playerController);
				arbitraryGrabbers.Add(component);
			}
		}
		yield return new WaitForSeconds(1f);
		GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
		yield return new WaitForSeconds(1.5f);
		if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
		{
			ChallengeManager.Instance.SuppressChallengeStart = true;
			this.m_challengesSuppressed = true;
		}
		AIActor megalich = this.m_megalich.aiActor;
		RoomHandler megalichRoom = GameManager.Instance.Dungeon.data.rooms.Find((RoomHandler r) => r.GetRoomName() == "LichRoom02");
		int numPlayers = GameManager.Instance.AllPlayers.Length;
		megalich.visibilityManager.SuppressPlayerEnteredRoom = true;
		Pixelator.Instance.FadeToBlack(0.1f, false, 0f);
		yield return new WaitForSeconds(0.1f);
		for (int j = 0; j < arbitraryGrabbers.Count; j++)
		{
			UnityEngine.Object.Destroy(arbitraryGrabbers[j].gameObject);
		}
		CameraController camera = GameManager.Instance.MainCameraController;
		camera.SetZoomScaleImmediate(0.75f);
		camera.LockToRoom = true;
		for (int k = 0; k < numPlayers; k++)
		{
			GameManager.Instance.AllPlayers[k].SetInputOverride("lich transition");
		}
		PlayerController player = GameManager.Instance.PrimaryPlayer;
		Vector2 targetPoint = megalichRoom.area.basePosition.ToVector2() + new Vector2(19.5f, 5f);
		if (player)
		{
			player.WarpToPoint(targetPoint, false, false);
			player.DoSpinfallSpawn(3f);
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(player);
			if (otherPlayer)
			{
				otherPlayer.ReuniteWithOtherPlayer(player, false);
				otherPlayer.DoSpinfallSpawn(3f);
			}
		}
		Vector2 idealCameraPosition = megalich.GetComponent<GenericIntroDoer>().BossCenter;
		camera.SetManualControl(true, false);
		camera.OverridePosition = idealCameraPosition + new Vector2(0f, 4f);
		Pixelator.Instance.FadeToBlack(0.5f, true, 0f);
		float timer = 0f;
		float duration = 3f;
		while (timer < duration)
		{
			yield return null;
			timer += Time.deltaTime;
			camera.OverridePosition = idealCameraPosition + new Vector2(0f, Mathf.SmoothStep(4f, 0f, timer / duration));
		}
		yield return new WaitForSeconds(2.5f);
		for (int l = 0; l < numPlayers; l++)
		{
			GameManager.Instance.AllPlayers[l].ClearInputOverride("lich transition");
		}
		if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
		{
			ChallengeManager.Instance.SuppressChallengeStart = false;
			this.m_challengesSuppressed = false;
			ChallengeManager.Instance.EnteredCombat();
		}
		megalich.visibilityManager.ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
		megalich.GetComponent<GenericIntroDoer>().TriggerSequence(player);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06005BD7 RID: 23511 RVA: 0x00233164 File Offset: 0x00231364
	private IEnumerator HandleDoubleLichExtraExplosionsCR()
	{
		PixelCollider collider = base.specRigidbody.HitboxPixelCollider;
		for (int i = 0; i < this.explosionCount; i++)
		{
			Vector2 minPos = collider.UnitBottomLeft;
			Vector2 maxPos = collider.UnitTopRight;
			Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.2f, 0.2f));
			GameObject vfxObj = SpawnManager.SpawnVFX(this.explosionVfx, pos, Quaternion.identity);
			tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = 0.8f;
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			yield return new WaitForSeconds(this.explosionMidDelay);
		}
		yield break;
	}

	// Token: 0x06005BD8 RID: 23512 RVA: 0x00233180 File Offset: 0x00231380
	private IEnumerator HandleDoubleLichPostDeathCR()
	{
		SuperReaperController.PreventShooting = true;
		GameStatsManager.Instance.SetFlag(GungeonFlags.GUNSLINGER_PAST_KILLED, true);
		GameStatsManager.Instance.SetFlag(GungeonFlags.GUNSLINGER_UNLOCKED, true);
		GameManager.Instance.MainCameraController.DoContinuousScreenShake(this.dualLichShake1, this, false);
		yield return new WaitForSeconds((float)this.explosionCount * this.explosionMidDelay - (float)this.bigExplosionCount * this.bigExplosionMidDelay);
		GameManager.Instance.MainCameraController.DoContinuousScreenShake(this.dualLichShake2, this, false);
		PixelCollider collider = base.specRigidbody.HitboxPixelCollider;
		for (int i = 0; i < this.bigExplosionCount; i++)
		{
			Vector2 minPos = collider.UnitBottomLeft;
			Vector2 maxPos = collider.UnitTopRight;
			Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.2f, 0.2f));
			GameObject vfxObj = SpawnManager.SpawnVFX(this.bigExplosionVfx, pos, Quaternion.identity);
			tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = 0.8f;
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			yield return new WaitForSeconds(this.bigExplosionMidDelay);
		}
		Pixelator.Instance.DoMinimap = false;
		BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
		if (extantCam)
		{
			extantCam.ForceCancelSequence();
		}
		yield return new WaitForSeconds(1f);
		Vector2 lichCenter = base.aiAnimator.sprite.WorldCenter;
		Pixelator.Instance.DoFinalNonFadedLayer = true;
		GameManager.Instance.MainCameraController.DoScreenShake(1.25f, 8f, 0.5f, 0.75f, null);
		GameObject gameObject = SpawnManager.SpawnVFX(this.bigExplosionVfx, collider.UnitCenter, Quaternion.identity);
		tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
		component.HeightOffGround = 0.8f;
		base.sprite.AttachRenderer(component);
		base.sprite.UpdateZDepth();
		yield return new WaitForSeconds(0.15f);
		base.aiActor.StealthDeath = true;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		UnityEngine.Object.Destroy(base.gameObject);
		Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
		yield return new WaitForSeconds(0.15f);
		for (int j = 0; j < StaticReferenceManager.AllDebris.Count; j++)
		{
			if (StaticReferenceManager.AllDebris[j])
			{
				Vector2 vector = StaticReferenceManager.AllDebris[j].transform.position.XY();
				if (GameManager.Instance.MainCameraController.PointIsVisible(vector))
				{
					StaticReferenceManager.AllDebris[j].TriggerDestruction(false);
				}
			}
		}
		GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
		TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
		Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
		yield return new WaitForSeconds(0.4f);
		yield return GameManager.Instance.StartCoroutine(ttcc.HandleTimeTubeCredits(lichCenter, false, null, -1, true));
		ttcc.CleanupLich();
		Pixelator.Instance.DoFinalNonFadedLayer = true;
		BraveCameraUtility.OverrideAspect = new float?(1.7777778f);
		yield return GameManager.Instance.StartCoroutine(this.HandlePastBeingShot());
		yield break;
	}

	// Token: 0x06005BD9 RID: 23513 RVA: 0x0023319C File Offset: 0x0023139C
	private IEnumerator HandlePastBeingShot()
	{
		Minimap.Instance.TemporarilyPreventMinimap = true;
		Pixelator.Instance.LerpToLetterbox(1f, 2.5f);
		yield return new WaitForSeconds(7.5f);
		TitleDioramaController tdc = UnityEngine.Object.FindObjectOfType<TitleDioramaController>();
		float elapsed = 0f;
		float duration = 10f;
		Transform targetXform = tdc.PastIslandSprite.transform.parent;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			tdc.SkyRenderer.material.SetFloat("_SkyBoost", Mathf.Lerp(0.88f, 0f, t));
			tdc.SkyRenderer.material.SetColor("_OverrideColor", Color.Lerp(new Color(1f, 0.55f, 0.2f, 1f), new Color(0.05f, 0.08f, 0.15f, 1f), t));
			tdc.SkyRenderer.material.SetFloat("_CurvePower", Mathf.Lerp(0.3f, -0.25f, t));
			tdc.SkyRenderer.material.SetFloat("_DitherCohesionFactor", Mathf.Lerp(0.3f, 1f, t));
			tdc.SkyRenderer.material.SetFloat("_StepValue", Mathf.Lerp(0.2f, 0.01f, t));
			targetXform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0f, -60f, 0f), BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, t));
			yield return null;
		}
		AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		yield break;
	}

	// Token: 0x06005BDA RID: 23514 RVA: 0x002331B0 File Offset: 0x002313B0
	private IEnumerator HandleSplashBody(PlayerController sourcePlayer, bool isPrimaryPlayer, TitleDioramaController diorama)
	{
		AkSoundEngine.PostEvent("Play_CHR_forever_fall_01", GameManager.Instance.gameObject);
		if (sourcePlayer.healthHaver.IsDead)
		{
			yield break;
		}
		GameObject timefallCorpseInstance = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/TimefallCorpse", ".prefab"), sourcePlayer.sprite.transform.position, Quaternion.identity);
		timefallCorpseInstance.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
		tk2dSpriteAnimator targetTimefallAnimator = timefallCorpseInstance.GetComponent<tk2dSpriteAnimator>();
		SpriteOutlineManager.AddOutlineToSprite(targetTimefallAnimator.Sprite, Color.black);
		tk2dSpriteAnimation timefallSpecificLibrary = ((!(sourcePlayer is PlayerSpaceshipController)) ? sourcePlayer.sprite.spriteAnimator.Library : (sourcePlayer as PlayerSpaceshipController).TimefallCorpseLibrary);
		targetTimefallAnimator.Library = timefallSpecificLibrary;
		targetTimefallAnimator.Library = timefallSpecificLibrary;
		tk2dSpriteAnimationClip clip = targetTimefallAnimator.GetClipByName("timefall");
		if (clip != null)
		{
			targetTimefallAnimator.Play("timefall");
		}
		float elapsed = 0f;
		float duration = 3f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			Vector3 startPoint = diorama.VFX_Splash.transform.position + new Vector3(-8f, 40f, 0f);
			Vector3 endPoint = diorama.VFX_Splash.GetComponent<tk2dBaseSprite>().WorldCenter.ToVector3ZUp(startPoint.z);
			targetTimefallAnimator.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(elapsed / duration));
			timefallCorpseInstance.transform.localScale = Vector3.Lerp(Vector3.one * 1.25f, new Vector3(0.125f, 0.125f, 0.125f), Mathf.Clamp01(elapsed / duration));
			yield return null;
		}
		AkSoundEngine.PostEvent("Play_CHR_final_splash_01", GameManager.Instance.gameObject);
		diorama.VFX_Splash.SetActive(true);
		diorama.VFX_Splash.GetComponent<tk2dSpriteAnimator>().PlayAndDisableObject(string.Empty, null);
		diorama.VFX_Splash.GetComponent<tk2dSprite>().UpdateZDepth();
		UnityEngine.Object.Destroy(timefallCorpseInstance);
		yield break;
	}

	// Token: 0x04005564 RID: 21860
	public GameObject HellDragVFX;

	// Token: 0x04005565 RID: 21861
	public ScreenShakeSettings hellDragScreenShake;

	// Token: 0x04005566 RID: 21862
	public ScreenShakeSettings dualLichShake1;

	// Token: 0x04005567 RID: 21863
	public ScreenShakeSettings dualLichShake2;

	// Token: 0x04005568 RID: 21864
	public GameObject explosionVfx;

	// Token: 0x04005569 RID: 21865
	private float explosionMidDelay = 0.1f;

	// Token: 0x0400556A RID: 21866
	private int explosionCount = 55;

	// Token: 0x0400556B RID: 21867
	public GameObject bigExplosionVfx;

	// Token: 0x0400556C RID: 21868
	private float bigExplosionMidDelay = 0.2f;

	// Token: 0x0400556D RID: 21869
	private int bigExplosionCount = 15;

	// Token: 0x0400556F RID: 21871
	private MegalichDeathController m_megalich;

	// Token: 0x04005570 RID: 21872
	private bool m_challengesSuppressed;
}
