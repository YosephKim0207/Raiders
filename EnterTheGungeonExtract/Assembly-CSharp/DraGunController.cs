using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001013 RID: 4115
public class DraGunController : BraveBehaviour
{
	// Token: 0x17000D06 RID: 3334
	// (get) Token: 0x06005A11 RID: 23057 RVA: 0x00226A54 File Offset: 0x00224C54
	// (set) Token: 0x06005A12 RID: 23058 RVA: 0x00226A5C File Offset: 0x00224C5C
	public float? OverrideTargetX { get; set; }

	// Token: 0x17000D07 RID: 3335
	// (get) Token: 0x06005A13 RID: 23059 RVA: 0x00226A68 File Offset: 0x00224C68
	// (set) Token: 0x06005A14 RID: 23060 RVA: 0x00226A70 File Offset: 0x00224C70
	public bool TrackPlayerWithHead { get; set; }

	// Token: 0x17000D08 RID: 3336
	// (get) Token: 0x06005A15 RID: 23061 RVA: 0x00226A7C File Offset: 0x00224C7C
	// (set) Token: 0x06005A16 RID: 23062 RVA: 0x00226A84 File Offset: 0x00224C84
	public bool IsNearDeath { get; set; }

	// Token: 0x17000D09 RID: 3337
	// (get) Token: 0x06005A17 RID: 23063 RVA: 0x00226A90 File Offset: 0x00224C90
	// (set) Token: 0x06005A18 RID: 23064 RVA: 0x00226A98 File Offset: 0x00224C98
	public bool IsTransitioning { get; set; }

	// Token: 0x17000D0A RID: 3338
	// (get) Token: 0x06005A19 RID: 23065 RVA: 0x00226AA4 File Offset: 0x00224CA4
	// (set) Token: 0x06005A1A RID: 23066 RVA: 0x00226AAC File Offset: 0x00224CAC
	public bool SpotlightEnabled { get; set; }

	// Token: 0x17000D0B RID: 3339
	// (get) Token: 0x06005A1B RID: 23067 RVA: 0x00226AB8 File Offset: 0x00224CB8
	// (set) Token: 0x06005A1C RID: 23068 RVA: 0x00226AC0 File Offset: 0x00224CC0
	public Vector2 SpotlightPos { get; set; }

	// Token: 0x17000D0C RID: 3340
	// (get) Token: 0x06005A1D RID: 23069 RVA: 0x00226ACC File Offset: 0x00224CCC
	// (set) Token: 0x06005A1E RID: 23070 RVA: 0x00226AD4 File Offset: 0x00224CD4
	public float SpotlightSpeed { get; set; }

	// Token: 0x17000D0D RID: 3341
	// (get) Token: 0x06005A1F RID: 23071 RVA: 0x00226AE0 File Offset: 0x00224CE0
	// (set) Token: 0x06005A20 RID: 23072 RVA: 0x00226AE8 File Offset: 0x00224CE8
	public float SpotlightSmoothTime { get; set; }

	// Token: 0x17000D0E RID: 3342
	// (get) Token: 0x06005A21 RID: 23073 RVA: 0x00226AF4 File Offset: 0x00224CF4
	// (set) Token: 0x06005A22 RID: 23074 RVA: 0x00226AFC File Offset: 0x00224CFC
	public bool HasDoneIntro
	{
		get
		{
			return this.m_hasDoneIntro;
		}
		set
		{
			if (!this.m_hasDoneIntro && value)
			{
				this.RestrictMotion(true);
			}
			this.m_hasDoneIntro = value;
		}
	}

	// Token: 0x17000D0F RID: 3343
	// (get) Token: 0x06005A23 RID: 23075 RVA: 0x00226B20 File Offset: 0x00224D20
	// (set) Token: 0x06005A24 RID: 23076 RVA: 0x00226B28 File Offset: 0x00224D28
	public bool HasConvertedBaby { get; set; }

	// Token: 0x06005A25 RID: 23077 RVA: 0x00226B34 File Offset: 0x00224D34
	public void Start()
	{
		this.TrackPlayerWithHead = true;
		base.specRigidbody.Initialize();
		float unitBottom = base.specRigidbody.PrimaryPixelCollider.UnitBottom;
		foreach (TileSpriteClipper tileSpriteClipper in base.GetComponentsInChildren<TileSpriteClipper>(true))
		{
			tileSpriteClipper.clipMode = TileSpriteClipper.ClipMode.ClipBelowY;
			tileSpriteClipper.clipY = unitBottom;
		}
		if (!this.isAdvanced)
		{
			this.m_transitionDummy = base.transform.Find("TransitionDummy").GetComponent<tk2dSpriteAnimator>();
			base.healthHaver.minimumHealth = this.NearDeathTriggerHealth;
			base.healthHaver.OnPreDeath += this.OnPreDeath;
		}
		if (this.wings && this.wings.GetComponent<tk2dSpriteAnimator>())
		{
			this.m_wingsAnimator = this.wings.GetComponent<tk2dSpriteAnimator>();
		}
		TrailRenderer[] componentsInChildren2 = this.head.GetComponentsInChildren<TrailRenderer>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].sortingLayerName = "Foreground";
		}
	}

	// Token: 0x06005A26 RID: 23078 RVA: 0x00226C4C File Offset: 0x00224E4C
	private void HandleFlaps()
	{
		if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.HIGH)
		{
			return;
		}
		if (!this.m_embers)
		{
			this.m_embers = GlobalSparksDoer.GetEmbersController();
		}
		if (this.m_isFlapping)
		{
			this.m_elapsedFlap += BraveTime.DeltaTime * 2f;
		}
		if (!this.m_isFlapping)
		{
			this.m_elapsedFlap -= BraveTime.DeltaTime / 3f;
		}
		this.m_elapsedFlap = Mathf.Clamp01(this.m_elapsedFlap);
		if (this.m_elapsedFlap <= 0f)
		{
			this.m_embers.AdditionalVortices.Clear();
		}
		else
		{
			Vector4 vector = new Vector4(this.wings.transform.position.x + 6f, this.wings.transform.position.y + 5f, 15f * this.m_elapsedFlap, -11f * this.m_elapsedFlap);
			Vector4 vector2 = new Vector4(this.wings.transform.position.x + 22f, this.wings.transform.position.y + 5f, 15f * this.m_elapsedFlap, 11f * this.m_elapsedFlap);
			if (this.m_embers.AdditionalVortices.Count < 1)
			{
				this.m_embers.AdditionalVortices.Add(vector);
			}
			else
			{
				this.m_embers.AdditionalVortices[0] = vector;
			}
			if (this.m_embers.AdditionalVortices.Count < 2)
			{
				this.m_embers.AdditionalVortices.Add(vector2);
			}
			else
			{
				this.m_embers.AdditionalVortices[1] = vector2;
			}
		}
	}

	// Token: 0x06005A27 RID: 23079 RVA: 0x00226E38 File Offset: 0x00225038
	public void Update()
	{
		if (this.OverrideTargetX != null)
		{
			this.head.TargetX = new float?(base.specRigidbody.HitboxPixelCollider.UnitCenter.x + this.OverrideTargetX.Value);
		}
		else if (base.aiActor.TargetRigidbody)
		{
			this.head.TargetX = new float?(base.aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox).x);
		}
		this.head.UpdateHead();
		if (this.m_wingsAnimator)
		{
			if (!this.IsTransitioning)
			{
				if (this.m_wingsAnimator.IsPlaying("wing_flap"))
				{
					this.m_isFlapping = true;
				}
				else
				{
					this.m_isFlapping = false;
				}
			}
			this.HandleFlaps();
		}
		if (!this.isAdvanced && !this.IsNearDeath && base.healthHaver.GetCurrentHealth() <= this.NearDeathTriggerHealth)
		{
			base.StartCoroutine(this.ConvertToNearDeath());
		}
		if (this.SpotlightEnabled)
		{
			this.m_elapsedSpotlight += BraveTime.DeltaTime;
			if (base.aiActor.TargetRigidbody)
			{
				Vector2 unitCenter = base.aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
				this.SpotlightPos = Vector2.SmoothDamp(this.SpotlightPos, unitCenter, ref this.SpotlightVelocity, this.SpotlightSmoothTime, this.SpotlightSpeed, BraveTime.DeltaTime);
			}
			Vector2 vector = this.headShootPoint.transform.position;
			float num = (this.SpotlightPos - vector).ToAngle();
			if (!this.m_spotlight)
			{
				GameObject gameObject = new GameObject("dragunSpotlight");
				this.m_spotlight = gameObject.AddComponent<AdditionalBraveLight>();
				this.m_spotlight.CustomLightMaterial = this.SpotlightMaterial;
				this.m_spotlight.UsesCustomMaterial = true;
				this.m_spotlight.LightColor = new Color(1f, 0.28627452f, 0.41960785f);
				this.m_spotlightSprite = UnityEngine.Object.Instantiate<GameObject>(this.SpotlightSprite);
				this.m_spotlightSprite.transform.parent = gameObject.transform;
				this.m_spotlightSprite.transform.localPosition = Vector3.zero;
			}
			else if (!this.m_spotlight.gameObject.activeSelf)
			{
				this.m_spotlight.gameObject.SetActive(true);
			}
			float num2 = 5f;
			float num3 = (float)((GameManager.Options.LightingQuality != GameOptions.GenericHighMedLowOption.LOW) ? 92 : 50);
			this.m_spotlight.LightIntensity = Mathf.Lerp(0f, num3, Mathf.Clamp01(this.m_elapsedSpotlight / num2));
			this.m_spotlight.LightRadius = this.SpotlightRadius * 2f + 1.25f;
			this.m_spotlight.CustomLightMaterial.SetVector("_LightOrigin", new Vector4(vector.x, vector.y, 0f, 0f));
			this.m_spotlight.transform.position = this.SpotlightPos.ToVector3ZisY(0f);
			this.m_spotlightSprite.transform.localScale = new Vector3(this.SpotlightShrink, this.SpotlightShrink, 1f);
		}
		else
		{
			this.m_elapsedSpotlight = 0f;
			if (this.m_spotlight && this.m_spotlight.gameObject.activeSelf)
			{
				this.m_spotlight.gameObject.SetActive(false);
			}
		}
		if (!this.isAdvanced && base.aiActor.HasBeenEngaged && !this.HasConvertedBaby)
		{
			this.m_babyCheckTimer -= BraveTime.DeltaTime;
			if (this.m_babyCheckTimer <= 0f)
			{
				BabyDragunController babyDragunController = UnityEngine.Object.FindObjectOfType<BabyDragunController>();
				if (babyDragunController)
				{
					babyDragunController.BecomeEnemy(this);
					this.HasConvertedBaby = true;
				}
				this.m_babyCheckTimer = 1f;
			}
		}
	}

	// Token: 0x06005A28 RID: 23080 RVA: 0x00227260 File Offset: 0x00225460
	protected override void OnDestroy()
	{
		if (this.m_embers)
		{
			this.m_embers.AdditionalVortices.Clear();
		}
		if (this.PitCausticsMaterial)
		{
			this.PitCausticsMaterial.SetFloat("_LightCausticPower", 4f);
			this.PitCausticsMaterial.SetFloat("_ValueMaximum", 50f);
			this.PitCausticsMaterial.SetFloat("_ValueMinimum", 0f);
		}
		if (this.m_spotlight)
		{
			UnityEngine.Object.Destroy(this.m_spotlight.gameObject);
		}
		this.RestrictMotion(false);
		this.ModifyCamera(false);
		this.BlockPitTiles(false);
		SilencerInstance.s_MaxRadiusLimiter = null;
		if (base.healthHaver)
		{
			base.healthHaver.OnPreDeath -= this.OnPreDeath;
		}
		base.OnDestroy();
	}

	// Token: 0x06005A29 RID: 23081 RVA: 0x0022734C File Offset: 0x0022554C
	private void OnPreDeath(Vector2 finalDirection)
	{
		this.RestrictMotion(false);
	}

	// Token: 0x06005A2A RID: 23082 RVA: 0x00227358 File Offset: 0x00225558
	private void PlayerMovementRestrictor(SpeculativeRigidbody playerSpecRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		if (pixelOffset.y < prevPixelOffset.y)
		{
			int num = playerSpecRigidbody.PixelColliders[0].MinY + pixelOffset.y;
			if (num < this.m_minPlayerY)
			{
				validLocation = false;
			}
		}
		if (pixelOffset.y > prevPixelOffset.y)
		{
			int num2 = playerSpecRigidbody.PixelColliders[0].MaxY + pixelOffset.y;
			if (num2 >= this.m_maxPlayerY)
			{
				validLocation = false;
			}
		}
	}

	// Token: 0x06005A2B RID: 23083 RVA: 0x002273E8 File Offset: 0x002255E8
	public void RestrictMotion(bool value)
	{
		if (this.m_isMotionRestricted == value)
		{
			return;
		}
		if (value)
		{
			if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach)
			{
				return;
			}
			this.m_minPlayerY = (base.aiActor.ParentRoom.area.basePosition.y + DraGunRoomPlaceable.HallHeight) * 16 + 8;
			this.m_maxPlayerY = (base.aiActor.ParentRoom.area.basePosition.y + base.aiActor.ParentRoom.area.dimensions.y - 1) * 16;
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				SpeculativeRigidbody specRigidbody = GameManager.Instance.AllPlayers[i].specRigidbody;
				specRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.PlayerMovementRestrictor));
			}
		}
		else
		{
			if (!GameManager.HasInstance || GameManager.IsReturningToBreach)
			{
				return;
			}
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[j];
				if (playerController)
				{
					SpeculativeRigidbody specRigidbody2 = playerController.specRigidbody;
					specRigidbody2.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Remove(specRigidbody2.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.PlayerMovementRestrictor));
				}
			}
		}
		this.m_isMotionRestricted = value;
	}

	// Token: 0x06005A2C RID: 23084 RVA: 0x00227564 File Offset: 0x00225764
	public void ModifyCamera(bool value)
	{
		if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach)
		{
			return;
		}
		CameraController mainCameraController = GameManager.Instance.MainCameraController;
		if (!mainCameraController)
		{
			return;
		}
		if (value)
		{
			mainCameraController.OverrideZoomScale = 0.75f;
			mainCameraController.LockToRoom = true;
			mainCameraController.AddFocusPoint(this.head.gameObject);
		}
		else
		{
			mainCameraController.OverrideZoomScale = 1f;
			mainCameraController.LockToRoom = false;
			mainCameraController.RemoveFocusPoint(this.head.gameObject);
		}
	}

	// Token: 0x06005A2D RID: 23085 RVA: 0x00227600 File Offset: 0x00225800
	public void BlockPitTiles(bool value)
	{
		if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach || GameManager.Instance.Dungeon == null)
		{
			return;
		}
		IntVector2 basePosition = base.aiActor.ParentRoom.area.basePosition;
		IntVector2 intVector = base.aiActor.ParentRoom.area.basePosition + base.aiActor.ParentRoom.area.dimensions - IntVector2.One;
		DungeonData data = GameManager.Instance.Dungeon.data;
		for (int i = basePosition.x; i <= intVector.x; i++)
		{
			for (int j = basePosition.y; j <= intVector.y; j++)
			{
				CellData cellData = data[i, j];
				if (cellData != null && cellData.type == CellType.PIT)
				{
					cellData.IsPlayerInaccessible = value;
				}
			}
		}
	}

	// Token: 0x06005A2E RID: 23086 RVA: 0x00227710 File Offset: 0x00225910
	public bool MaybeConvertToGold()
	{
		if (this.HasConvertedBaby)
		{
			base.StartCoroutine(this.ConvertToGold());
			return true;
		}
		return false;
	}

	// Token: 0x06005A2F RID: 23087 RVA: 0x00227730 File Offset: 0x00225930
	private IEnumerator ConvertToNearDeath()
	{
		base.healthHaver.IsVulnerable = false;
		this.IsNearDeath = true;
		Pixelator.Instance.FadeToColor(0.01f, Color.white, false, 0f);
		base.behaviorSpeculator.InterruptAndDisable();
		StaticReferenceManager.DestroyAllEnemyProjectiles();
		yield return null;
		StaticReferenceManager.DestroyAllEnemyProjectiles();
		List<AIActor> enemies = base.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < enemies.Count; i++)
		{
			if (enemies[i].name.Contains("knife", true))
			{
				enemies[i].healthHaver.ApplyDamage(1000f, Vector2.zero, "Dragun Near-Death", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
			}
		}
		GameManager.Instance.PreventPausing = true;
		yield return new WaitForSeconds(0.5f);
		StaticReferenceManager.DestroyAllEnemyProjectiles();
		base.aiActor.ToggleRenderers(false);
		this.head.OverrideDesiredPosition = new Vector2?(base.transform.position + new Vector3(3.63f, 10.8f));
		Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
		GameManager.Instance.DungeonMusicController.SwitchToDragunTwo();
		this.m_transitionDummy.gameObject.SetActive(true);
		this.m_transitionDummy.GetComponent<Renderer>().enabled = true;
		this.m_transitionDummy.Play("hit_react");
		while (this.m_transitionDummy.IsPlaying("hit_react"))
		{
			yield return null;
		}
		float roarElapsed = 0f;
		this.IsTransitioning = true;
		this.m_transitionDummy.Play("roar");
		base.aiActor.GetComponent<DragunCracktonMap>().ConvertToCrackton();
		while (this.m_transitionDummy.IsPlaying("roar"))
		{
			roarElapsed += GameManager.INVARIANT_DELTA_TIME;
			if (roarElapsed > 0.5f)
			{
				this.m_isFlapping = true;
			}
			yield return null;
		}
		this.m_isFlapping = false;
		this.IsTransitioning = false;
		this.m_transitionDummy.Play("blank");
		this.m_transitionDummy.gameObject.SetActive(false);
		base.aiActor.ToggleRenderers(true);
		this.head.OverrideDesiredPosition = null;
		base.healthHaver.minimumHealth = 0f;
		base.healthHaver.DamageableColliders = new List<PixelCollider>();
		base.healthHaver.DamageableColliders.Add(base.aiActor.specRigidbody.PixelColliders[1]);
		base.behaviorSpeculator.enabled = true;
		GameManager.Instance.PreventPausing = false;
		yield break;
	}

	// Token: 0x06005A30 RID: 23088 RVA: 0x0022774C File Offset: 0x0022594C
	private IEnumerator ConvertToGold()
	{
		base.healthHaver.IsVulnerable = false;
		base.aiAnimator.PlayVfx("heart_heal", null, null, null);
		if (GameManager.HasInstance && GameManager.Instance.MainCameraController)
		{
			CameraController mainCameraController = GameManager.Instance.MainCameraController;
			mainCameraController.OverridePosition = base.specRigidbody.UnitCenter + new Vector2(0f, 4f);
			mainCameraController.SetManualControl(true, true);
		}
		GameUIRoot.Instance.HideCoreUI("dragun_transition");
		GameUIRoot.Instance.ToggleLowerPanels(false, false, "dragun_transition");
		yield return new WaitForSeconds(3f);
		base.behaviorSpeculator.InterruptAndDisable();
		StaticReferenceManager.DestroyAllEnemyProjectiles();
		yield return null;
		StaticReferenceManager.DestroyAllEnemyProjectiles();
		List<AIActor> enemies = base.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < enemies.Count; i++)
		{
			if (enemies[i].name.Contains("knife", true))
			{
				enemies[i].healthHaver.ApplyDamage(1000f, Vector2.zero, "Dragun Near-Death", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
			}
		}
		GameManager.Instance.PreventPausing = true;
		StaticReferenceManager.DestroyAllEnemyProjectiles();
		base.aiActor.ToggleRenderers(false);
		this.head.OverrideDesiredPosition = new Vector2?(base.transform.position + new Vector3(3.63f, 10.8f));
		GameManager.Instance.DungeonMusicController.SwitchToDragunTwo();
		GameManager.Instance.PreventPausing = false;
		base.aiAnimator.StopVfx("heart_heal");
		AIActor advancedDraGun = AIActor.Spawn(this.AdvancedDraGunPrefab, base.specRigidbody.UnitBottomLeft, base.aiActor.ParentRoom, false, AIActor.AwakenAnimationType.Default, false);
		advancedDraGun.transform.position = base.transform.position;
		advancedDraGun.specRigidbody.Reinitialize();
		base.healthHaver.EndBossState(false);
		UnityEngine.Object.Destroy(base.gameObject);
		GameUIRoot.Instance.ShowCoreUI("dragun_transition");
		GameUIRoot.Instance.ToggleLowerPanels(true, true, "dragun_transition");
		advancedDraGun.GetComponent<ObjectVisibilityManager>().ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
		advancedDraGun.GetComponent<GenericIntroDoer>().TriggerSequence(GameManager.Instance.BestActivePlayer);
		advancedDraGun.GetComponent<DragunCracktonMap>().PreGold();
		if (this.HasConvertedBaby)
		{
			BabyDragunController babyDragunController = UnityEngine.Object.FindObjectOfType<BabyDragunController>();
			if (babyDragunController)
			{
				UnityEngine.Object.Destroy(babyDragunController.gameObject);
			}
		}
		yield break;
	}

	// Token: 0x06005A31 RID: 23089 RVA: 0x00227768 File Offset: 0x00225968
	public void HandleDarkRoomEffects(bool enabling, float duration)
	{
		base.StartCoroutine(this.HandleDarkRoomEffectsCR(enabling, duration));
	}

	// Token: 0x06005A32 RID: 23090 RVA: 0x0022777C File Offset: 0x0022597C
	private IEnumerator HandleDarkRoomEffectsCR(bool enabling, float duration)
	{
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.Clamp01(elapsed / duration);
			if (!enabling)
			{
				t = 1f - t;
			}
			t = Mathf.Pow(t, 4f);
			this.PitCausticsMaterial.SetFloat("_LightCausticPower", Mathf.Lerp(4f, 80f, t));
			this.PitCausticsMaterial.SetFloat("_ValueMaximum", Mathf.Lerp(50f, 5000f, t));
			this.PitCausticsMaterial.SetFloat("_ValueMinimum", Mathf.Lerp(0f, 0.5f, t));
			yield return null;
		}
		this.PitCausticsMaterial.SetFloat("_LightCausticPower", (float)((!enabling) ? 4 : 80));
		this.PitCausticsMaterial.SetFloat("_ValueMaximum", (float)((!enabling) ? 50 : 5000));
		this.PitCausticsMaterial.SetFloat("_ValueMinimum", (!enabling) ? 0f : 0.5f);
		yield break;
	}

	// Token: 0x0400537B RID: 21371
	public bool isAdvanced;

	// Token: 0x0400537C RID: 21372
	public DraGunHeadController head;

	// Token: 0x0400537D RID: 21373
	public GameObject neck;

	// Token: 0x0400537E RID: 21374
	public GameObject wings;

	// Token: 0x0400537F RID: 21375
	public GameObject headShootPoint;

	// Token: 0x04005380 RID: 21376
	public GameObject leftArm;

	// Token: 0x04005381 RID: 21377
	public GameObject rightArm;

	// Token: 0x04005382 RID: 21378
	public float NearDeathTriggerHealth = 150f;

	// Token: 0x04005383 RID: 21379
	public GameObject skyRocket;

	// Token: 0x04005384 RID: 21380
	public GameObject skyBoulder;

	// Token: 0x04005385 RID: 21381
	public AIActor AdvancedDraGunPrefab;

	// Token: 0x0400538E RID: 21390
	[NonSerialized]
	public Vector2 SpotlightVelocity;

	// Token: 0x0400538F RID: 21391
	[NonSerialized]
	public float SpotlightRadius = 3f;

	// Token: 0x04005390 RID: 21392
	public float SpotlightShrink;

	// Token: 0x04005391 RID: 21393
	public GameObject SpotlightSprite;

	// Token: 0x04005392 RID: 21394
	[Header("For Brents")]
	public Material SpotlightMaterial;

	// Token: 0x04005393 RID: 21395
	public Material PitCausticsMaterial;

	// Token: 0x04005395 RID: 21397
	private float m_elapsedFlap;

	// Token: 0x04005396 RID: 21398
	private bool m_isFlapping;

	// Token: 0x04005397 RID: 21399
	private float m_babyCheckTimer;

	// Token: 0x04005398 RID: 21400
	private AdditionalBraveLight m_spotlight;

	// Token: 0x04005399 RID: 21401
	private GameObject m_spotlightSprite;

	// Token: 0x0400539A RID: 21402
	private float m_elapsedSpotlight;

	// Token: 0x0400539B RID: 21403
	private bool m_isMotionRestricted;

	// Token: 0x0400539C RID: 21404
	private tk2dSpriteAnimator m_wingsAnimator;

	// Token: 0x0400539D RID: 21405
	private EmbersController m_embers;

	// Token: 0x0400539E RID: 21406
	private tk2dSpriteAnimator m_transitionDummy;

	// Token: 0x0400539F RID: 21407
	private bool m_hasDoneIntro;

	// Token: 0x040053A0 RID: 21408
	private int m_minPlayerY;

	// Token: 0x040053A1 RID: 21409
	private int m_maxPlayerY;
}
