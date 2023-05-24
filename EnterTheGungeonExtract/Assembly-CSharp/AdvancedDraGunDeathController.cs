using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000FAF RID: 4015
public class AdvancedDraGunDeathController : BraveBehaviour
{
	// Token: 0x0600575C RID: 22364 RVA: 0x0021547C File Offset: 0x0021367C
	public void Awake()
	{
		this.m_dragunController = base.GetComponent<DraGunController>();
		this.m_roarDummy = base.aiActor.transform.Find("RoarDummy").GetComponent<tk2dSpriteAnimator>();
	}

	// Token: 0x0600575D RID: 22365 RVA: 0x002154AC File Offset: 0x002136AC
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		base.healthHaver.OverrideKillCamTime = new float?(16.5f);
	}

	// Token: 0x0600575E RID: 22366 RVA: 0x002154E8 File Offset: 0x002136E8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600575F RID: 22367 RVA: 0x002154F0 File Offset: 0x002136F0
	private void OnBossDeath(Vector2 dir)
	{
		base.behaviorSpeculator.enabled = false;
		GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());
	}

	// Token: 0x06005760 RID: 22368 RVA: 0x00215510 File Offset: 0x00213710
	private IEnumerator OnDeathExplosionsCR()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].SetInputOverride("DraGunDeathController");
		}
		GameManager.Instance.PreventPausing = true;
		GameUIRoot.Instance.HideCoreUI("dragun");
		GameUIRoot.Instance.ToggleLowerPanels(false, false, "dragun");
		Pixelator.Instance.FadeToColor(0.5f, Color.white, true, 0f);
		base.healthHaver.OverrideKillCamPos = new Vector2?(base.specRigidbody.UnitCenter + new Vector2(0f, 6f));
		base.aiActor.ToggleRenderers(false);
		this.m_dragunController.head.OverrideDesiredPosition = new Vector2?(base.aiActor.transform.position + new Vector3(3.63f, 11.8f));
		this.m_roarDummy.gameObject.SetActive(true);
		this.m_roarDummy.GetComponent<Renderer>().enabled = true;
		this.m_roarDummy.sprite.usesOverrideMaterial = false;
		this.m_roarDummy.Play("death");
		base.aiAnimator.PlayVfx("roar_shake", null, null, null);
		while (this.m_roarDummy.IsPlaying("death"))
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		GameManager.Instance.PreventPausing = true;
		Animation leftArm = this.m_dragunController.leftArm.GetComponent<Animation>();
		AIAnimator leftHand = this.m_dragunController.leftArm.transform.Find("LeftHand").GetComponent<AIAnimator>();
		Animation rightArm = this.m_dragunController.rightArm.GetComponent<Animation>();
		AIAnimator rightHand = this.m_dragunController.rightArm.transform.Find("RightHand").GetComponent<AIAnimator>();
		AIAnimator head = this.m_dragunController.head.aiAnimator;
		foreach (Renderer renderer in this.m_dragunController.leftArm.GetComponentsInChildren<Renderer>())
		{
			renderer.enabled = true;
		}
		leftArm.Play("DraGunLeftAdvancedDeath");
		leftHand.spriteAnimator.enabled = true;
		leftHand.PlayUntilCancelled("predeath", false, null, -1f, false);
		foreach (Renderer renderer2 in this.m_dragunController.rightArm.GetComponentsInChildren<Renderer>())
		{
			renderer2.enabled = true;
		}
		rightArm.Play("DraGunRightAdvancedDeath");
		rightHand.spriteAnimator.enabled = true;
		rightHand.PlayUntilCancelled("predeath", false, null, -1f, false);
		this.m_dragunController.head.renderer.enabled = true;
		head.spriteAnimator.enabled = true;
		head.PlayUntilCancelled("predeath", false, null, -1f, false);
		head.LockFacingDirection = true;
		this.m_roarDummy.sprite.SetSprite("dragun_gold_death_body_001");
		leftArm.transform.Find("LeftArm (5)").GetComponentInChildren<Renderer>().enabled = false;
		leftArm.transform.Find("LeftArm (6)").GetComponentInChildren<Renderer>().enabled = false;
		rightArm.transform.Find("RightArm (5)").GetComponentInChildren<Renderer>().enabled = false;
		rightArm.transform.Find("RightArm (6)").GetComponentInChildren<Renderer>().enabled = false;
		base.StartCoroutine(this.ExplodeHand(leftHand, 180f));
		yield return new WaitForSeconds(2f);
		base.StartCoroutine(this.ExplodeHand(rightHand, 0f));
		yield return new WaitForSeconds(1.25f);
		base.StartCoroutine(this.ExplodeBall(leftArm, "LeftArm (4)", 180f, 0.5f));
		yield return new WaitForSeconds(0.5f);
		base.StartCoroutine(this.ExplodeBall(rightArm, "RightArm (4)", 0f, 0.5f));
		yield return new WaitForSeconds(0.5f);
		base.StartCoroutine(this.ExplodeBall(leftArm, "LeftArm (3)", 180f, 0.4f));
		yield return new WaitForSeconds(0.4f);
		base.StartCoroutine(this.ExplodeBall(rightArm, "RightArm (3)", 0f, 0.4f));
		yield return new WaitForSeconds(0.4f);
		base.StartCoroutine(this.ExplodeBall(leftArm, "LeftArm (2)", 180f, 0.3f));
		yield return new WaitForSeconds(0.3f);
		base.StartCoroutine(this.ExplodeBall(rightArm, "RightArm (2)", 0f, 0.3f));
		yield return new WaitForSeconds(0.3f);
		base.StartCoroutine(this.ExplodeBall(leftArm, "LeftArm (1)", 180f, 0.2f));
		yield return new WaitForSeconds(0.2f);
		base.StartCoroutine(this.ExplodeBall(rightArm, "RightArm (1)", 0f, 0.9f));
		yield return new WaitForSeconds(0.9f);
		base.StartCoroutine(this.ExplodeShoulder(leftArm, "LeftShoulder", 180f, 0.9f));
		yield return new WaitForSeconds(0.9f);
		base.StartCoroutine(this.ExplodeShoulder(rightArm, "RightShoulder", -90f, 1f));
		yield return new WaitForSeconds(1f);
		head.sprite.usesOverrideMaterial = false;
		head.PlayUntilCancelled("death", false, null, -1f, false);
		Vector2 shardPos = head.sprite.WorldCenter;
		yield return new WaitForSeconds(0.7f);
		this.m_roarDummy.Play("death_body_head_explode");
		base.aiAnimator.PlayVfx("death_big_shake", null, null, null);
		foreach (SpawnShardsOnDeath spawnShardsOnDeath in this.m_dragunController.head.GetComponentsInChildren<SpawnShardsOnDeath>())
		{
			spawnShardsOnDeath.HandleShardSpawns(Vector2.zero, new Vector2?(shardPos));
		}
		yield return new WaitForSeconds(1.67f);
		base.aiAnimator.PlayVfx("death_slow_shake", null, null, null);
		yield return new WaitForSeconds(0.66f);
		base.StartCoroutine(this.FadeBodyCR(1.33f));
		yield return new WaitForSeconds(0.67f);
		head.renderer.enabled = false;
		Pixelator.Instance.FadeToColor(0.5f, Color.white, false, 0f);
		yield return new WaitForSeconds(0.75f);
		this.m_dragunController.ModifyCamera(false);
		this.m_dragunController.BlockPitTiles(false);
		yield return new WaitForSeconds(0.75f);
		this.m_dragunController.IsTransitioning = false;
		this.SpawnBones(this.fingerDebris, UnityEngine.Random.Range(3, 6), new Vector2(2f, 4f), new Vector3(-24f, -15f));
		this.SpawnBones(this.fingerDebris, UnityEngine.Random.Range(3, 6), new Vector2(24f, 4f), new Vector3(-2f, -15f));
		this.SpawnBones(this.neckDebris, UnityEngine.Random.Range(1, 3), new Vector2(2f, 4f), new Vector3(-24f, -15f));
		this.SpawnBones(this.neckDebris, UnityEngine.Random.Range(1, 3), new Vector2(24f, 4f), new Vector3(-2f, -15f));
		Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
		DraGunRoomPlaceable dragunRoomController = base.aiActor.ParentRoom.GetComponentsAbsoluteInRoom<DraGunRoomPlaceable>()[0];
		dragunRoomController.DraGunKilled = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		UnityEngine.Object.Destroy(base.gameObject);
		yield return new WaitForSeconds(0.75f);
		dragunRoomController.ExtendDeathBridge();
		for (int m = 0; m < GameManager.Instance.AllPlayers.Length; m++)
		{
			GameManager.Instance.AllPlayers[m].ClearInputOverride("DraGunDeathController");
		}
		if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH)
		{
			GameUIRoot.Instance.ShowCoreUI("dragun");
			GameUIRoot.Instance.ToggleLowerPanels(true, false, "dragun");
		}
		yield return null;
		GameManager.Instance.PreventPausing = false;
		for (int n = 0; n < GameManager.Instance.AllPlayers.Length; n++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[n];
			if (playerController && playerController.passiveItems != null)
			{
				for (int num = 0; num < playerController.passiveItems.Count; num++)
				{
					CompanionItem companionItem = playerController.passiveItems[num] as CompanionItem;
					if (companionItem && companionItem.ExtantCompanion)
					{
						GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_SUNLIGHT_SPEAR_UNLOCK, true);
						if (!GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_DRAGUN_WITH_TURTLE))
						{
							CompanionController component = companionItem.ExtantCompanion.GetComponent<CompanionController>();
							if (component && component.companionID == CompanionController.CompanionIdentifier.SUPER_SPACE_TURTLE)
							{
								GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_DRAGUN_WITH_TURTLE, true);
							}
						}
					}
				}
			}
		}
		if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
		{
			GameManager.Instance.MainCameraController.SetManualControl(true, true);
			GameStatsManager.Instance.SetFlag(GungeonFlags.SHERPA_BOSSRUSH_COMPLETE, true);
			GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_BOSSRUSH, true);
			GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
			GameUIRoot.Instance.HideCoreUI(string.Empty);
			for (int num2 = 0; num2 < GameManager.Instance.AllPlayers.Length; num2++)
			{
				GameManager.Instance.AllPlayers[num2].SetInputOverride("game complete");
			}
			Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
			AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		}
		if (GameStatsManager.Instance.IsRainbowRun)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.BOWLER_RAINBOW_RUN_COMPLETE, true);
		}
		yield break;
	}

	// Token: 0x06005761 RID: 22369 RVA: 0x0021552C File Offset: 0x0021372C
	private IEnumerator ExplodeHand(AIAnimator hand, float headDirection)
	{
		AIAnimator headAnimator = this.m_dragunController.head.aiAnimator;
		headAnimator.FacingDirection = headDirection;
		headAnimator.EndAnimation();
		headAnimator.PlayUntilCancelled("predeath", false, null, -1f, false);
		hand.sprite.usesOverrideMaterial = false;
		hand.PlayUntilCancelled("death", false, null, -1f, false);
		yield return new WaitForSeconds(0.75f);
		base.aiAnimator.PlayVfx("death_small_shake", null, null, null);
		foreach (SpawnShardsOnDeath spawnShardsOnDeath in hand.GetComponentsInChildren<SpawnShardsOnDeath>())
		{
			spawnShardsOnDeath.HandleShardSpawns(Vector2.zero, new Vector2?(hand.sprite.WorldCenter));
		}
		yield break;
	}

	// Token: 0x06005762 RID: 22370 RVA: 0x00215558 File Offset: 0x00213758
	private IEnumerator ExplodeBall(Animation arm, string ballName, float headDirection, float postDelay)
	{
		tk2dSprite ballSprite = arm.transform.Find(ballName).GetComponentInChildren<tk2dSprite>();
		ballSprite.spriteAnimator.enabled = true;
		ballSprite.usesOverrideMaterial = false;
		ballSprite.spriteAnimator.Play("arm_death_explode");
		yield return new WaitForSeconds(0.47f);
		base.aiAnimator.PlayVfx("death_small_shake", null, null, null);
		AIAnimator headAnimator = this.m_dragunController.head.aiAnimator;
		headAnimator.FacingDirection = headDirection;
		headAnimator.EndAnimation();
		if (postDelay < 0.3f)
		{
			AIAnimator aianimator = headAnimator;
			string text = "predeath";
			float num = postDelay - 0.05f;
			aianimator.PlayUntilCancelled(text, false, null, num, false);
		}
		else
		{
			headAnimator.PlayUntilCancelled("predeath", false, null, -1f, false);
		}
		yield break;
	}

	// Token: 0x06005763 RID: 22371 RVA: 0x00215590 File Offset: 0x00213790
	private IEnumerator ExplodeShoulder(Animation arm, string ballName, float headDirection, float postDelay)
	{
		tk2dSprite ballSprite = arm.transform.Find(ballName).GetComponentInChildren<tk2dSprite>();
		ballSprite.spriteAnimator.enabled = true;
		ballSprite.usesOverrideMaterial = false;
		ballSprite.spriteAnimator.Play((headDirection <= 0f) ? "death_shoulder_left_explode" : "death_shoulder_right_explode");
		yield return new WaitForSeconds(0.42f);
		base.aiAnimator.PlayVfx("death_small_shake", null, null, null);
		AIAnimator headAnimator = this.m_dragunController.head.aiAnimator;
		headAnimator.FacingDirection = headDirection;
		headAnimator.EndAnimation();
		if (postDelay < 0.3f)
		{
			AIAnimator aianimator = headAnimator;
			string text = "predeath";
			float num = postDelay - 0.05f;
			aianimator.PlayUntilCancelled(text, false, null, num, false);
		}
		else
		{
			headAnimator.PlayUntilCancelled("predeath", false, null, -1f, false);
		}
		yield break;
	}

	// Token: 0x06005764 RID: 22372 RVA: 0x002155C8 File Offset: 0x002137C8
	private IEnumerator FadeBodyCR(float duration)
	{
		float timer = 0f;
		while (timer < duration)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			this.m_roarDummy.sprite.renderer.material.SetColor("_OverrideColor", new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, Mathf.Clamp01(timer / duration))));
		}
		yield break;
	}

	// Token: 0x06005765 RID: 22373 RVA: 0x002155EC File Offset: 0x002137EC
	private void SpawnBones(GameObject bonePrefab, int count, Vector2 min, Vector2 max)
	{
		Vector2 vector = base.aiActor.ParentRoom.area.basePosition.ToVector2() + min + new Vector2(0f, (float)DraGunRoomPlaceable.HallHeight);
		Vector2 vector2 = base.aiActor.ParentRoom.area.basePosition.ToVector2() + base.aiActor.ParentRoom.area.dimensions.ToVector2() + max;
		for (int i = 0; i < count; i++)
		{
			Vector2 vector3 = BraveUtility.RandomVector2(vector, vector2);
			GameObject gameObject = SpawnManager.SpawnVFX(bonePrefab, vector3, Quaternion.identity);
			if (gameObject)
			{
				gameObject.transform.parent = SpawnManager.Instance.VFX;
				DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
				orAddComponent.decayOnBounce = 0.5f;
				orAddComponent.bounceCount = 1;
				orAddComponent.canRotate = true;
				float num = UnityEngine.Random.Range(-80f, -100f);
				Vector2 vector4 = BraveMathCollege.DegreesToVector(num, 1f) * UnityEngine.Random.Range(0.1f, 3f);
				Vector3 vector5 = new Vector3(vector4.x, (vector4.y >= 0f) ? 0f : vector4.y, (vector4.y <= 0f) ? 0f : vector4.y);
				if (orAddComponent.minorBreakable)
				{
					orAddComponent.minorBreakable.enabled = true;
				}
				orAddComponent.Trigger(vector5, UnityEngine.Random.Range(1f, 2f), 1f);
				if (orAddComponent.specRigidbody)
				{
					DebrisObject debrisObject = orAddComponent;
					debrisObject.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnGrounded, new Action<DebrisObject>(this.HandleComplexDebris));
				}
			}
		}
	}

	// Token: 0x06005766 RID: 22374 RVA: 0x002157D8 File Offset: 0x002139D8
	private void HandleComplexDebris(DebrisObject debrisObject)
	{
		GameManager.Instance.StartCoroutine(this.DelayedSpriteFixer(debrisObject.sprite));
		SpeculativeRigidbody specRigidbody = debrisObject.specRigidbody;
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(specRigidbody, null, false);
		UnityEngine.Object.Destroy(debrisObject);
		specRigidbody.RegenerateCache();
	}

	// Token: 0x06005767 RID: 22375 RVA: 0x00215824 File Offset: 0x00213A24
	private IEnumerator DelayedSpriteFixer(tk2dBaseSprite sprite)
	{
		yield return null;
		sprite.HeightOffGround = -1f;
		sprite.IsPerpendicular = true;
		sprite.UpdateZDepth();
		yield break;
	}

	// Token: 0x04005063 RID: 20579
	public GameObject fingerDebris;

	// Token: 0x04005064 RID: 20580
	public GameObject neckDebris;

	// Token: 0x04005065 RID: 20581
	private DraGunController m_dragunController;

	// Token: 0x04005066 RID: 20582
	private tk2dSpriteAnimator m_roarDummy;
}
