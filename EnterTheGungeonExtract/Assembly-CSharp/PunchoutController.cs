using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;
using Dungeonator;
using InControl;
using UnityEngine;

// Token: 0x02001586 RID: 5510
public class PunchoutController : MonoBehaviour
{
	// Token: 0x170012A5 RID: 4773
	// (get) Token: 0x06007E3C RID: 32316 RVA: 0x003303FC File Offset: 0x0032E5FC
	// (set) Token: 0x06007E3D RID: 32317 RVA: 0x00330404 File Offset: 0x0032E604
	public float Timer { get; set; }

	// Token: 0x170012A6 RID: 4774
	// (get) Token: 0x06007E3E RID: 32318 RVA: 0x00330410 File Offset: 0x0032E610
	// (set) Token: 0x06007E3F RID: 32319 RVA: 0x00330418 File Offset: 0x0032E618
	public float HideUiAmount { get; set; }

	// Token: 0x170012A7 RID: 4775
	// (get) Token: 0x06007E40 RID: 32320 RVA: 0x00330424 File Offset: 0x0032E624
	// (set) Token: 0x06007E41 RID: 32321 RVA: 0x0033042C File Offset: 0x0032E62C
	public float HideControlsUiAmount { get; set; }

	// Token: 0x170012A8 RID: 4776
	// (get) Token: 0x06007E42 RID: 32322 RVA: 0x00330438 File Offset: 0x0032E638
	// (set) Token: 0x06007E43 RID: 32323 RVA: 0x00330440 File Offset: 0x0032E640
	public float HideTutorialUiAmount { get; set; }

	// Token: 0x170012A9 RID: 4777
	// (get) Token: 0x06007E44 RID: 32324 RVA: 0x0033044C File Offset: 0x0032E64C
	public bool ShouldDoTutorial
	{
		get
		{
			return !GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_BOXING_GLOVE);
		}
	}

	// Token: 0x06007E45 RID: 32325 RVA: 0x00330460 File Offset: 0x0032E660
	public IEnumerator Start()
	{
		if (!this.m_isInitialized)
		{
			yield return null;
			yield return null;
			yield return null;
			yield return null;
			yield return null;
		}
		this.InitPunchout();
		SpriteOutlineManager.AddOutlineToSprite(this.CoopCultist, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		this.Timer = this.TimerStartTime;
		PunchoutController.IsActive = true;
		yield break;
	}

	// Token: 0x06007E46 RID: 32326 RVA: 0x0033047C File Offset: 0x0032E67C
	public void Update()
	{
		this.UiManager.RenderCamera.enabled = !GameManager.Instance.IsPaused;
		this.Player.ManualUpdate();
		this.Opponent.ManualUpdate();
		if (this.HideControlsUiAmount <= 0f && !this.m_isFadingControlsUi && !(this.Opponent.state is PunchoutAIActor.IntroState))
		{
			base.StartCoroutine(this.ControlsUiFadeOutCR());
		}
		GameManager.Instance.MainCameraController.OverridePosition = this.m_cameraCenterPos + this.Player.CameraOffset + this.Opponent.CameraOffset;
		if (this.Opponent.state is PunchoutAIActor.IntroState)
		{
			this.Timer = this.TimerStartTime;
		}
		else if (!this.Opponent.IsDead && !(this.Opponent.state is PunchoutAIActor.WinState))
		{
			this.Timer = Mathf.Max(0f, this.Timer - BraveTime.DeltaTime);
		}
		this.UpdateTimer();
		if (PunchoutController.InTutorial)
		{
			PunchoutController.TutorialUiUpdateTimer -= BraveTime.DeltaTime;
			if (PunchoutController.TutorialUiUpdateTimer < 0f)
			{
				this.UpdateTutorialText();
				PunchoutController.TutorialUiUpdateTimer = 0.5f;
			}
			if (!this.m_tutorialSuperReady)
			{
				if (PunchoutController.TutorialControls[5] == PunchoutController.TutorialControlState.Completed)
				{
					this.m_tutorialSuperReady = true;
					this.Player.AddStar();
					PunchoutController.TutorialUiUpdateTimer = 0f;
				}
			}
			else if (PunchoutController.TutorialControls[6] == PunchoutController.TutorialControlState.Completed && this.Player.state == null)
			{
				PunchoutController.InTutorial = false;
				base.StartCoroutine(this.TutorialUiFadeCR());
			}
		}
		if (this.Timer <= 0f)
		{
			if (this.TimerAnimator.IsIdle())
			{
				this.TimerAnimator.PlayUntilCancelled("explode", false, null, -1f, false);
				this.TimerTextMin1.gameObject.SetActive(false);
				this.TimerTextMin2.gameObject.SetActive(false);
				this.TimerColon.gameObject.SetActive(false);
				this.TimerTextSec1.gameObject.SetActive(false);
				this.TimerTextSec2.gameObject.SetActive(false);
			}
			if (this.Opponent.state == null)
			{
				this.Opponent.state = new PunchoutAIActor.EscapeState();
				this.Player.Exhaust(new float?(4f));
			}
		}
		this.UpdateUI();
	}

	// Token: 0x06007E47 RID: 32327 RVA: 0x0033070C File Offset: 0x0032E90C
	private void OnDestroy()
	{
		PunchoutController.IsActive = false;
		PunchoutController.OverrideControlsButton = false;
	}

	// Token: 0x06007E48 RID: 32328 RVA: 0x0033071C File Offset: 0x0032E91C
	public void Init()
	{
		switch (GameManager.Instance.PrimaryPlayer.characterIdentity)
		{
		case PlayableCharacters.Pilot:
			this.Player.SwapPlayer(new int?(3), false);
			goto IL_11C;
		case PlayableCharacters.Convict:
			this.Player.SwapPlayer(new int?(0), false);
			goto IL_11C;
		case PlayableCharacters.Robot:
			this.Player.SwapPlayer(new int?(5), false);
			goto IL_11C;
		case PlayableCharacters.Soldier:
			this.Player.SwapPlayer(new int?(2), false);
			goto IL_11C;
		case PlayableCharacters.Guide:
			this.Player.SwapPlayer(new int?(1), false);
			goto IL_11C;
		case PlayableCharacters.Bullet:
			this.Player.SwapPlayer(new int?(4), false);
			goto IL_11C;
		case PlayableCharacters.Eevee:
			this.Player.SwapPlayer(new int?(7), false);
			goto IL_11C;
		case PlayableCharacters.Gunslinger:
			this.Player.SwapPlayer(new int?(6), false);
			goto IL_11C;
		}
		this.Player.SwapPlayer(new int?(UnityEngine.Random.Range(0, 8)), false);
		IL_11C:
		this.CoopCultist.gameObject.SetActive(GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER);
		base.StartCoroutine(this.UiFadeInCR());
		this.m_isInitialized = true;
	}

	// Token: 0x06007E49 RID: 32329 RVA: 0x00330878 File Offset: 0x0032EA78
	public void Reset()
	{
		this.Timer = this.TimerStartTime;
		this.TimerAnimator.EndAnimation();
		this.TimerTextMin1.gameObject.SetActive(true);
		this.TimerTextMin2.gameObject.SetActive(true);
		this.TimerColon.gameObject.SetActive(true);
		this.TimerTextSec1.gameObject.SetActive(true);
		this.TimerTextSec2.gameObject.SetActive(true);
		this.Player.SwapPlayer(new int?(UnityEngine.Random.Range(0, 8)), false);
		BraveTime.ClearMultiplier(this.Player.gameObject);
		base.StartCoroutine(this.UiFadeInCR());
		this.HideControlsUiAmount = 0f;
		PunchoutController.OverrideControlsButton = true;
		PunchoutController.InTutorial = this.ShouldDoTutorial;
		this.HideTutorialUiAmount = (float)((!PunchoutController.InTutorial) ? 1 : 0);
		PunchoutController.TutorialControls = new PunchoutController.TutorialControlState[]
		{
			PunchoutController.TutorialControlState.Shown,
			PunchoutController.TutorialControlState.Hidden,
			PunchoutController.TutorialControlState.Hidden,
			PunchoutController.TutorialControlState.Hidden,
			PunchoutController.TutorialControlState.Hidden,
			PunchoutController.TutorialControlState.Hidden,
			PunchoutController.TutorialControlState.Hidden
		};
		this.m_tutorialSuperReady = false;
		PunchoutController.TutorialUiUpdateTimer = 0f;
		this.UiManager.Invalidate();
		this.Opponent.Reset();
	}

	// Token: 0x06007E4A RID: 32330 RVA: 0x00330998 File Offset: 0x0032EB98
	private IEnumerator UiFadeInCR()
	{
		this.HideUiAmount = 1f;
		this.UpdateUI();
		yield return new WaitForSeconds(1f);
		float ela = 0f;
		while (ela < 0.2f)
		{
			ela += BraveTime.DeltaTime;
			float t = Mathf.Lerp(0f, 1f, ela / 0.2f);
			this.HideUiAmount = 1f - t;
			this.UiManager.Invalidate();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06007E4B RID: 32331 RVA: 0x003309B4 File Offset: 0x0032EBB4
	private IEnumerator ControlsUiFadeOutCR()
	{
		this.m_isFadingControlsUi = true;
		this.HideControlsUiAmount = 0f;
		PunchoutController.OverrideControlsButton = true;
		this.UpdateUI();
		yield return new WaitForSeconds(1f);
		float ela = 0f;
		while (ela < 0.2f)
		{
			ela += BraveTime.DeltaTime;
			float t = Mathf.Lerp(0f, 1f, ela / 0.2f);
			this.HideControlsUiAmount = t;
			this.UiManager.Invalidate();
			yield return null;
		}
		PunchoutController.OverrideControlsButton = false;
		this.m_isFadingControlsUi = false;
		yield break;
	}

	// Token: 0x06007E4C RID: 32332 RVA: 0x003309D0 File Offset: 0x0032EBD0
	private IEnumerator TutorialUiFadeCR()
	{
		this.HideTutorialUiAmount = 0f;
		this.UpdateUI();
		yield return new WaitForSeconds(1f);
		float ela = 0f;
		while (ela < 0.5f)
		{
			ela += BraveTime.DeltaTime;
			float t = Mathf.Lerp(0f, 1f, ela / 0.5f);
			this.HideTutorialUiAmount = t;
			this.UiManager.Invalidate();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06007E4D RID: 32333 RVA: 0x003309EC File Offset: 0x0032EBEC
	public void DoWinFade(bool skipDelay)
	{
		base.StartCoroutine(this.DoWinFadeCR(skipDelay));
	}

	// Token: 0x06007E4E RID: 32334 RVA: 0x003309FC File Offset: 0x0032EBFC
	private IEnumerator DoWinFadeCR(bool skipDelay)
	{
		if (!skipDelay)
		{
			yield return new WaitForSeconds(5f);
		}
		CameraController camera = GameManager.Instance.MainCameraController;
		Pixelator.Instance.FadeToColor(2.5f, Color.white, false, 0f);
		yield return new WaitForSeconds(0.5f);
		float ela = 0f;
		float duration = 2f;
		Vector2 startPos = camera.OverridePosition;
		Vector2 endPos = startPos + new Vector2(0f, 4f);
		while (ela < duration)
		{
			ela += BraveTime.DeltaTime;
			float t = Mathf.Lerp(0f, 1f, ela / (duration * 2f));
			camera.OverridePosition = Vector2Extensions.SmoothStep(startPos, endPos, t);
			t = Mathf.Lerp(0f, 1f, ela / 0.2f);
			this.HideUiAmount = t;
			this.UiManager.Invalidate();
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		GameStatsManager.Instance.SetFlag(GungeonFlags.RESOURCEFUL_RAT_PUNCHOUT_BEATEN, true);
		PickupObject.RatBeatenAtPunchout = true;
		this.PlaceNPC();
		Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
		this.TeardownPunchout();
		yield break;
	}

	// Token: 0x06007E4F RID: 32335 RVA: 0x00330A20 File Offset: 0x0032EC20
	public void DoLoseFade(bool skipDelay)
	{
		base.StartCoroutine(this.DoLoseFadeCR(skipDelay));
	}

	// Token: 0x06007E50 RID: 32336 RVA: 0x00330A30 File Offset: 0x0032EC30
	private IEnumerator DoLoseFadeCR(bool skipDelay)
	{
		if (!skipDelay)
		{
			yield return new WaitForSeconds(2f);
		}
		float ela = 0f;
		float duration = 3f;
		Material vignetteMaterial = Pixelator.Instance.FadeMaterial;
		while (ela < duration)
		{
			ela += BraveTime.DeltaTime;
			float t = Mathf.Lerp(0f, 1f, ela / duration);
			vignetteMaterial.SetColor("_VignetteColor", Color.black);
			vignetteMaterial.SetFloat("_VignettePower", Mathf.Lerp(0.5f, 10f, t));
			t = Mathf.Lerp(0f, 1f, ela / 0.2f);
			this.HideUiAmount = t;
			this.UiManager.Invalidate();
			yield return null;
		}
		Pixelator.Instance.FadeToColor(1f, Color.black, false, 0f);
		yield return new WaitForSeconds(1.5f);
		this.PlaceNote(this.PlayerLostNotePrefab.gameObject);
		Pixelator.Instance.FadeToColor(1f, Color.black, true, 0f);
		vignetteMaterial.SetColor("_VignetteColor", Color.black);
		vignetteMaterial.SetFloat("_VignettePower", 1f);
		this.TeardownPunchout();
		yield break;
	}

	// Token: 0x06007E51 RID: 32337 RVA: 0x00330A54 File Offset: 0x0032EC54
	private void PlaceNPC()
	{
		if (this.PlayerWonRatNPC)
		{
			RoomHandler currentRoom = GameManager.Instance.BestActivePlayer.CurrentRoom;
			bool flag = false;
			IntVector2 intVector = currentRoom.GetCenteredVisibleClearSpot(3, 3, out flag, true);
			intVector = intVector - currentRoom.area.basePosition + IntVector2.One;
			if (flag)
			{
				GameObject gameObject = DungeonPlaceableUtility.InstantiateDungeonPlaceable(this.PlayerWonRatNPC.gameObject, currentRoom, intVector, false, AIActor.AwakenAnimationType.Default, false);
				if (gameObject)
				{
					IPlayerInteractable[] interfacesInChildren = gameObject.GetInterfacesInChildren<IPlayerInteractable>();
					for (int i = 0; i < interfacesInChildren.Length; i++)
					{
						currentRoom.RegisterInteractable(interfacesInChildren[i]);
					}
				}
			}
		}
	}

	// Token: 0x06007E52 RID: 32338 RVA: 0x00330B00 File Offset: 0x0032ED00
	private void PlaceNote(GameObject notePrefab)
	{
		if (notePrefab != null)
		{
			RoomHandler currentRoom = GameManager.Instance.BestActivePlayer.CurrentRoom;
			bool flag = false;
			IntVector2 intVector = currentRoom.GetCenteredVisibleClearSpot(3, 3, out flag, true);
			intVector = intVector - currentRoom.area.basePosition + IntVector2.One;
			if (flag)
			{
				GameObject gameObject = DungeonPlaceableUtility.InstantiateDungeonPlaceable(notePrefab.gameObject, currentRoom, intVector, false, AIActor.AwakenAnimationType.Default, false);
				if (gameObject)
				{
					IPlayerInteractable[] interfacesInChildren = gameObject.GetInterfacesInChildren<IPlayerInteractable>();
					for (int i = 0; i < interfacesInChildren.Length; i++)
					{
						currentRoom.RegisterInteractable(interfacesInChildren[i]);
					}
				}
			}
		}
	}

	// Token: 0x06007E53 RID: 32339 RVA: 0x00330BA4 File Offset: 0x0032EDA4
	public void DoBombFade()
	{
		base.StartCoroutine(this.DoBombFadeCR());
	}

	// Token: 0x06007E54 RID: 32340 RVA: 0x00330BB4 File Offset: 0x0032EDB4
	private IEnumerator DoBombFadeCR()
	{
		float fadeOutTime = 1.66f;
		Pixelator.Instance.FadeToColor(fadeOutTime, Color.white, false, 0f);
		float ela = 0f;
		while (ela < fadeOutTime)
		{
			ela += BraveTime.DeltaTime;
			float t = Mathf.Lerp(0f, 1f, ela / 0.2f);
			this.HideUiAmount = t;
			this.UiManager.Invalidate();
			yield return null;
		}
		GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Normal, Vibration.Strength.Hard);
		AkSoundEngine.PostEvent("Play_OBJ_nuke_blast_01", base.gameObject);
		float timer = 0f;
		float duration = 0.1f;
		while (timer < duration)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			Pixelator.Instance.FadeColor = Color.Lerp(Color.white, Color.yellow, Mathf.Clamp01(timer / duration));
		}
		timer = 0f;
		duration = 0.1f;
		while (timer < duration)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			Pixelator.Instance.FadeColor = Color.Lerp(Color.yellow, Color.red, Mathf.Clamp01(timer / duration));
		}
		timer = 0f;
		duration = 0.1f;
		while (timer < duration)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			Pixelator.Instance.FadeColor = Color.Lerp(Color.red, Color.yellow, Mathf.Clamp01(timer / duration));
		}
		timer = 0f;
		duration = 0.1f;
		while (timer < duration)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			Pixelator.Instance.FadeColor = Color.Lerp(Color.yellow, Color.white, Mathf.Clamp01(timer / duration));
		}
		yield return new WaitForSeconds(1.5f);
		Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
		this.TeardownPunchout();
		yield break;
	}

	// Token: 0x06007E55 RID: 32341 RVA: 0x00330BD0 File Offset: 0x0032EDD0
	private void InitPunchout()
	{
		if (Minimap.HasInstance)
		{
			Minimap.Instance.TemporarilyPreventMinimap = true;
			GameUIRoot.Instance.HideCoreUI("punchout");
			GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
			this.m_cameraCenterPos = GameManager.Instance.BestActivePlayer.specRigidbody.GetUnitCenter(ColliderType.HitBox) + new Vector2(0f, -25f);
			base.transform.position = this.m_cameraCenterPos - PhysicsEngine.PixelToUnit(new IntVector2(240, 130));
			foreach (tk2dBaseSprite tk2dBaseSprite in base.GetComponentsInChildren<tk2dBaseSprite>())
			{
				tk2dBaseSprite.UpdateZDepth();
			}
			CameraController mainCameraController = GameManager.Instance.MainCameraController;
			mainCameraController.OverridePosition = this.m_cameraCenterPos;
			mainCameraController.SetManualControl(true, false);
			mainCameraController.LockToRoom = true;
			mainCameraController.SetZoomScaleImmediate(1.6f);
			foreach (PlayerController playerController in GameManager.Instance.AllPlayers)
			{
				playerController.SetInputOverride("punchout");
				playerController.healthHaver.IsVulnerable = false;
				playerController.SuppressEffectUpdates = true;
				playerController.IsOnFire = false;
				playerController.CurrentFireMeterValue = 0f;
				playerController.CurrentPoisonMeterValue = 0f;
				if (playerController.specRigidbody)
				{
					DeadlyDeadlyGoopManager.DelayedClearGoopsInRadius(playerController.specRigidbody.UnitCenter, 1f);
				}
			}
			GameManager.Instance.DungeonMusicController.SwitchToBossMusic("Play_MUS_RatPunch_Intro_01", base.gameObject);
			foreach (ParticleSystem particleSystem in base.transform.Find("arena").GetComponentsInChildren<ParticleSystem>())
			{
				particleSystem.transform.position = particleSystem.transform.position.XY().ToVector3ZisY(0f);
			}
			foreach (Light light in base.transform.Find("arena").GetComponentsInChildren<Light>())
			{
				light.transform.position = light.transform.position.XY().ToVector3ZisY(-18f);
			}
		}
		else
		{
			AkSoundEngine.PostEvent("Play_MUS_RatPunch_Intro_01", base.gameObject);
		}
		PunchoutController.InTutorial = this.ShouldDoTutorial;
		this.HideTutorialUiAmount = (float)((!PunchoutController.InTutorial) ? 1 : 0);
		PunchoutController.TutorialControls = new PunchoutController.TutorialControlState[]
		{
			PunchoutController.TutorialControlState.Shown,
			PunchoutController.TutorialControlState.Hidden,
			PunchoutController.TutorialControlState.Hidden,
			PunchoutController.TutorialControlState.Hidden,
			PunchoutController.TutorialControlState.Hidden,
			PunchoutController.TutorialControlState.Hidden,
			PunchoutController.TutorialControlState.Hidden
		};
		PunchoutController.OverrideControlsButton = true;
	}

	// Token: 0x06007E56 RID: 32342 RVA: 0x00330E8C File Offset: 0x0032F08C
	private void TeardownPunchout()
	{
		if (this.m_isInitialized)
		{
			Minimap.Instance.TemporarilyPreventMinimap = false;
			GameUIRoot.Instance.ShowCoreUI("punchout");
			GameUIRoot.Instance.ShowCoreUI(string.Empty);
			GameUIRoot.Instance.ToggleLowerPanels(true, false, string.Empty);
			CameraController mainCameraController = GameManager.Instance.MainCameraController;
			mainCameraController.SetManualControl(false, false);
			mainCameraController.LockToRoom = false;
			mainCameraController.SetZoomScaleImmediate(1f);
			foreach (PlayerController playerController in GameManager.Instance.AllPlayers)
			{
				playerController.ClearInputOverride("punchout");
				playerController.healthHaver.IsVulnerable = true;
				playerController.SuppressEffectUpdates = false;
				playerController.IsOnFire = false;
				playerController.CurrentFireMeterValue = 0f;
				playerController.CurrentPoisonMeterValue = 0f;
			}
			GameManager.Instance.DungeonMusicController.EndBossMusic();
			MetalGearRatRoomController metalGearRatRoomController = UnityEngine.Object.FindObjectOfType<MetalGearRatRoomController>();
			if (metalGearRatRoomController)
			{
				GameObject gameObject = PickupObjectDatabase.GetById(GlobalItemIds.RatKey).gameObject;
				Vector3 position = metalGearRatRoomController.transform.position;
				if (this.Opponent.NumKeysDropped >= 1)
				{
					LootEngine.SpawnItem(gameObject, position + new Vector3(14.25f, 17f), Vector2.zero, 0f, true, false, false);
				}
				if (this.Opponent.NumKeysDropped >= 2)
				{
					LootEngine.SpawnItem(gameObject, position + new Vector3(13.25f, 14.5f), Vector2.zero, 0f, true, false, false);
				}
				if (this.Opponent.NumKeysDropped >= 3)
				{
					LootEngine.SpawnItem(gameObject, position + new Vector3(14.25f, 12f), Vector2.zero, 0f, true, false, false);
				}
				if (this.Opponent.NumKeysDropped >= 4)
				{
					LootEngine.SpawnItem(gameObject, position + new Vector3(30.25f, 17f), Vector2.zero, 0f, true, false, false);
				}
				if (this.Opponent.NumKeysDropped >= 5)
				{
					LootEngine.SpawnItem(gameObject, position + new Vector3(31.25f, 14.5f), Vector2.zero, 0f, true, false, false);
				}
				if (this.Opponent.NumKeysDropped >= 6)
				{
					LootEngine.SpawnItem(gameObject, position + new Vector3(30.25f, 12f), Vector2.zero, 0f, true, false, false);
				}
				Vector2 vector = position + new Vector3(22.25f, 14.5f);
				foreach (int num in this.Opponent.DroppedRewardIds)
				{
					float num2 = (float)((!BraveUtility.RandomBool()) ? 180 : 0) + UnityEngine.Random.Range(-30f, 30f);
					GameObject gameObject2 = PickupObjectDatabase.GetById(num).gameObject;
					LootEngine.SpawnItem(gameObject2, vector + new Vector2(11f, 0f).Rotate(num2), Vector2.zero, 0f, true, false, false);
				}
			}
			GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_BOXING_GLOVE, true);
			BraveTime.ClearMultiplier(this.Player.gameObject);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			this.Reset();
		}
	}

	// Token: 0x06007E57 RID: 32343 RVA: 0x0033121C File Offset: 0x0032F41C
	private void UpdateUI()
	{
		string text = this.ControlsLabel.ForceGetLocalizedValue("#MAINMENU_CONTROLS");
		if (text == "CONTROLS")
		{
			text = "Controls";
		}
		this.ControlsLabel.Text = text + " (" + StringTableManager.EvaluateReplacementToken("%CONTROL_PAUSE") + ")";
		float scaleTileScale = Pixelator.Instance.ScaleTileScale;
		this.PlayerHealthBarBase.transform.localScale = new Vector3(scaleTileScale, scaleTileScale, 1f);
		this.RatHealthBarBase.transform.localScale = new Vector3(scaleTileScale, scaleTileScale, 1f);
		this.ControlsLabel.transform.localScale = new Vector3(scaleTileScale, scaleTileScale, 1f);
		this.TutorialLabel.transform.localScale = new Vector3(scaleTileScale, scaleTileScale, 1f);
		float num = (this.PlayerHealthBarBase.Height + 8f) * scaleTileScale * this.HideUiAmount;
		this.PlayerHealthBarBase.Position = new Vector3(4f * scaleTileScale, -8f * scaleTileScale + num);
		this.RatHealthBarBase.Position = new Vector3(this.UiPanel.Size.x - this.RatHealthBarBase.Size.x - 4f * scaleTileScale, -8f * scaleTileScale + num);
		float num2 = -(this.ControlsLabel.Height * scaleTileScale + 8f) * scaleTileScale * this.HideControlsUiAmount;
		this.ControlsLabel.Position = new Vector3(this.UiPanel.Size.x - this.ControlsLabel.Size.x * scaleTileScale - 4f * scaleTileScale, -this.UiPanel.Size.y + this.ControlsLabel.Size.y + 4f * scaleTileScale + num2);
		float num3 = -(this.TutorialLabel.Width + 4f) * scaleTileScale * this.HideTutorialUiAmount;
		this.TutorialLabel.Position = new Vector3(scaleTileScale * 4f + num3, -this.UiPanel.Size.y + this.TutorialLabel.Size.y + 4f * scaleTileScale);
	}

	// Token: 0x06007E58 RID: 32344 RVA: 0x00331474 File Offset: 0x0032F674
	private void UpdateTutorialText()
	{
		StringBuilder stringBuilder = new StringBuilder();
		this.HandleTutorialLine(stringBuilder, 0, "#OPTIONS_PUNCHOUT_DODGELEFT", GungeonActions.GungeonActionType.PunchoutDodgeLeft);
		this.HandleTutorialLine(stringBuilder, 1, "#OPTIONS_PUNCHOUT_DODGERIGHT", GungeonActions.GungeonActionType.PunchoutDodgeRight);
		this.HandleTutorialLine(stringBuilder, 2, "#OPTIONS_PUNCHOUT_BLOCK", GungeonActions.GungeonActionType.PunchoutBlock);
		this.HandleTutorialLine(stringBuilder, 3, "#OPTIONS_PUNCHOUT_DUCK", GungeonActions.GungeonActionType.PunchoutDuck);
		this.HandleTutorialLine(stringBuilder, 4, "#OPTIONS_PUNCHOUT_PUNCHLEFT", GungeonActions.GungeonActionType.PunchoutPunchLeft);
		this.HandleTutorialLine(stringBuilder, 5, "#OPTIONS_PUNCHOUT_PUNCHRIGHT", GungeonActions.GungeonActionType.PunchoutPunchRight);
		this.HandleTutorialLine(stringBuilder, 6, "#OPTIONS_PUNCHOUT_SUPER", GungeonActions.GungeonActionType.PunchoutSuper);
		this.TutorialLabel.Text = stringBuilder.ToString();
	}

	// Token: 0x06007E59 RID: 32345 RVA: 0x00331504 File Offset: 0x0032F704
	public static void InputWasPressed(int action)
	{
		if (PunchoutController.TutorialControls[action] == PunchoutController.TutorialControlState.Shown)
		{
			PunchoutController.TutorialControls[action] = PunchoutController.TutorialControlState.Completed;
			if (action < PunchoutController.TutorialControls.Length - 1)
			{
				PunchoutController.TutorialControls[action + 1] = PunchoutController.TutorialControlState.Shown;
			}
			PunchoutController.TutorialUiUpdateTimer = 0f;
		}
	}

	// Token: 0x06007E5A RID: 32346 RVA: 0x00331540 File Offset: 0x0032F740
	private void HandleTutorialLine(StringBuilder str, int i, string commandName, GungeonActions.GungeonActionType action)
	{
		if (PunchoutController.TutorialControls[i] == PunchoutController.TutorialControlState.Hidden)
		{
			str.AppendLine();
			return;
		}
		bool flag = PunchoutController.TutorialControls[i] == PunchoutController.TutorialControlState.Completed;
		if (flag)
		{
			str.Append("[color green]");
		}
		str.Append(this.TutorialLabel.ForceGetLocalizedValue(commandName));
		if (flag)
		{
			str.Append("[/color]");
		}
		str.Append(" (").Append(this.GetTutorialText(action)).AppendLine(")");
	}

	// Token: 0x06007E5B RID: 32347 RVA: 0x003315C8 File Offset: 0x0032F7C8
	private string GetTutorialText(GungeonActions.GungeonActionType action)
	{
		BraveInput primaryPlayerInstance = BraveInput.PrimaryPlayerInstance;
		if (primaryPlayerInstance.IsKeyboardAndMouse(false))
		{
			return StringTableManager.GetBindingText(action);
		}
		if (primaryPlayerInstance != null)
		{
			ReadOnlyCollection<BindingSource> bindings = primaryPlayerInstance.ActiveActions.GetActionFromType(action).Bindings;
			if (bindings.Count > 0)
			{
				for (int i = 0; i < bindings.Count; i++)
				{
					DeviceBindingSource deviceBindingSource = bindings[i] as DeviceBindingSource;
					if (deviceBindingSource != null && deviceBindingSource.Control != InputControlType.None)
					{
						return UIControllerButtonHelper.GetUnifiedControllerButtonTag(deviceBindingSource.Control, BraveInput.PlayerOneCurrentSymbology, null);
					}
				}
			}
		}
		return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.Start, BraveInput.PlayerOneCurrentSymbology, null);
	}

	// Token: 0x06007E5C RID: 32348 RVA: 0x00331674 File Offset: 0x0032F874
	private void UpdateTimer()
	{
		int num = Mathf.CeilToInt(this.Timer);
		int num2 = (int)((float)num / 60f);
		num -= num2 * 60;
		this.TimerTextMin1.text = (num2 / 10).ToString();
		this.TimerTextMin2.text = (num2 % 10).ToString();
		this.TimerTextSec1.text = (num / 10).ToString();
		this.TimerTextSec2.text = (num % 10).ToString();
	}

	// Token: 0x0400811D RID: 33053
	public static bool IsActive;

	// Token: 0x0400811E RID: 33054
	public static bool OverrideControlsButton;

	// Token: 0x0400811F RID: 33055
	public static bool InTutorial;

	// Token: 0x04008120 RID: 33056
	public static PunchoutController.TutorialControlState[] TutorialControls = new PunchoutController.TutorialControlState[7];

	// Token: 0x04008121 RID: 33057
	public static float TutorialUiUpdateTimer;

	// Token: 0x04008122 RID: 33058
	public PunchoutPlayerController Player;

	// Token: 0x04008123 RID: 33059
	public PunchoutAIActor Opponent;

	// Token: 0x04008124 RID: 33060
	public tk2dSprite CoopCultist;

	// Token: 0x04008125 RID: 33061
	public AIAnimator TimerAnimator;

	// Token: 0x04008126 RID: 33062
	public tk2dTextMesh TimerTextMin1;

	// Token: 0x04008127 RID: 33063
	public tk2dTextMesh TimerTextMin2;

	// Token: 0x04008128 RID: 33064
	public tk2dTextMesh TimerColon;

	// Token: 0x04008129 RID: 33065
	public tk2dTextMesh TimerTextSec1;

	// Token: 0x0400812A RID: 33066
	public tk2dTextMesh TimerTextSec2;

	// Token: 0x0400812B RID: 33067
	public dfGUIManager UiManager;

	// Token: 0x0400812C RID: 33068
	public dfPanel UiPanel;

	// Token: 0x0400812D RID: 33069
	public dfSprite PlayerHealthBarBase;

	// Token: 0x0400812E RID: 33070
	public dfSprite RatHealthBarBase;

	// Token: 0x0400812F RID: 33071
	public dfLabel ControlsLabel;

	// Token: 0x04008130 RID: 33072
	public dfLabel TutorialLabel;

	// Token: 0x04008131 RID: 33073
	[Header("Rewards")]
	public float NormalHitRewardChance = 1f;

	// Token: 0x04008132 RID: 33074
	[PickupIdentifier]
	public int[] NormalHitRewards;

	// Token: 0x04008133 RID: 33075
	public int MaxGlassGuonStones = 3;

	// Token: 0x04008134 RID: 33076
	[Header("Post-Punchout Stuff")]
	public DungeonPlaceableBehaviour PlayerLostNotePrefab;

	// Token: 0x04008135 RID: 33077
	public TalkDoerLite PlayerWonRatNPC;

	// Token: 0x04008136 RID: 33078
	[Header("Constants")]
	public float TimerStartTime = 120f;

	// Token: 0x0400813B RID: 33083
	private bool m_isFadingControlsUi;

	// Token: 0x0400813C RID: 33084
	private Vector2 m_cameraCenterPos;

	// Token: 0x0400813D RID: 33085
	private bool m_isInitialized;

	// Token: 0x0400813E RID: 33086
	private bool m_tutorialSuperReady;

	// Token: 0x02001587 RID: 5511
	public enum TutorialControlState
	{
		// Token: 0x04008140 RID: 33088
		Hidden,
		// Token: 0x04008141 RID: 33089
		Shown,
		// Token: 0x04008142 RID: 33090
		Completed
	}
}
