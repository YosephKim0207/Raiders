using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001017 RID: 4119
public class DraGunDeathController : BraveBehaviour
{
	// Token: 0x06005A46 RID: 23110 RVA: 0x0022818C File Offset: 0x0022638C
	public void Awake()
	{
		this.m_dragunController = base.GetComponent<DraGunController>();
		this.m_deathDummy = base.transform.Find("DeathDummy").GetComponent<tk2dSpriteAnimator>();
	}

	// Token: 0x06005A47 RID: 23111 RVA: 0x002281B8 File Offset: 0x002263B8
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		base.healthHaver.OverrideKillCamTime = new float?(6.25f);
	}

	// Token: 0x06005A48 RID: 23112 RVA: 0x002281F4 File Offset: 0x002263F4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005A49 RID: 23113 RVA: 0x002281FC File Offset: 0x002263FC
	private void OnBossDeath(Vector2 dir)
	{
		base.behaviorSpeculator.enabled = false;
		GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());
		AkSoundEngine.PostEvent("Play_BOSS_dragun_thunder_01", base.gameObject);
	}

	// Token: 0x06005A4A RID: 23114 RVA: 0x0022822C File Offset: 0x0022642C
	private IEnumerator LerpCrackEmission(float startVal, float targetVal, float duration)
	{
		Material targetMaterial = this.m_deathDummy.GetComponent<Renderer>().material;
		float ela = 0f;
		while (ela < duration)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			float t = ela / duration;
			t *= t;
			targetMaterial.SetFloat("_CrackAmount", Mathf.Lerp(startVal, targetVal, t));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06005A4B RID: 23115 RVA: 0x0022825C File Offset: 0x0022645C
	private IEnumerator LerpCrackColor(Color targetVal, float duration)
	{
		Material targetMaterial = this.m_deathDummy.GetComponent<Renderer>().material;
		Color startVal = targetMaterial.GetColor("_CrackBaseColor");
		float ela = 0f;
		while (ela < duration)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			float t = ela / duration;
			t *= t;
			targetMaterial.SetColor("_CrackBaseColor", Color.Lerp(startVal, targetVal, t));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06005A4C RID: 23116 RVA: 0x00228288 File Offset: 0x00226488
	private IEnumerator OnDeathExplosionsCR()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].SetInputOverride("DraGunDeathController");
		}
		GameManager.Instance.PreventPausing = true;
		GameUIRoot.Instance.HideCoreUI("dragun");
		GameUIRoot.Instance.ToggleLowerPanels(false, false, "dragun");
		base.healthHaver.OverrideKillCamPos = new Vector2?(base.specRigidbody.UnitCenter + new Vector2(0f, 6f));
		base.aiAnimator.PlayUntilCancelled("heart_burst", false, null, -1f, false);
		while (base.aiAnimator.IsPlaying("heart_burst"))
		{
			yield return null;
		}
		base.aiAnimator.EndAnimationIf("heart_burst");
		base.aiAnimator.PlayVfx("heart_burst", null, null, null);
		Pixelator.Instance.FadeToColor(0.75f, Color.white, true, 0f);
		yield return new WaitForSeconds(0.3f);
		GameManager.Instance.PreventPausing = true;
		base.aiActor.ToggleRenderers(false);
		this.m_deathDummy.gameObject.SetActive(true);
		this.m_deathDummy.GetComponent<Renderer>().enabled = true;
		this.m_dragunController.IsTransitioning = true;
		this.m_deathDummy.Play("die");
		base.StartCoroutine(this.LerpCrackEmission(1f, 250f, 3f));
		yield return new WaitForSeconds(3f);
		GameManager.Instance.PreventPausing = true;
		base.StartCoroutine(this.LerpCrackColor(Color.white, 3f));
		base.StartCoroutine(this.LerpCrackEmission(250f, 50000f, 3f));
		yield return new WaitForSeconds(1.5f);
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
		this.SpawnBones(this.skullDebris, 1, new Vector2(8f, 6f), new Vector2(-22f, -23f));
		Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
		DraGunRoomPlaceable dragunRoomController = base.aiActor.ParentRoom.GetComponentsAbsoluteInRoom<DraGunRoomPlaceable>()[0];
		dragunRoomController.DraGunKilled = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		UnityEngine.Object.Destroy(base.gameObject);
		yield return new WaitForSeconds(0.75f);
		dragunRoomController.ExtendDeathBridge();
		for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
		{
			GameManager.Instance.AllPlayers[j].ClearInputOverride("DraGunDeathController");
		}
		if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH)
		{
			GameUIRoot.Instance.ShowCoreUI("dragun");
			GameUIRoot.Instance.ToggleLowerPanels(true, false, "dragun");
		}
		yield return null;
		GameManager.Instance.PreventPausing = false;
		for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[k];
			if (playerController && playerController.passiveItems != null)
			{
				for (int l = 0; l < playerController.passiveItems.Count; l++)
				{
					CompanionItem companionItem = playerController.passiveItems[l] as CompanionItem;
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
			for (int m = 0; m < GameManager.Instance.AllPlayers.Length; m++)
			{
				GameManager.Instance.AllPlayers[m].SetInputOverride("game complete");
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

	// Token: 0x06005A4D RID: 23117 RVA: 0x002282A4 File Offset: 0x002264A4
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

	// Token: 0x06005A4E RID: 23118 RVA: 0x00228490 File Offset: 0x00226690
	private void HandleComplexDebris(DebrisObject debrisObject)
	{
		GameManager.Instance.StartCoroutine(this.DelayedSpriteFixer(debrisObject.sprite));
		SpeculativeRigidbody specRigidbody = debrisObject.specRigidbody;
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(specRigidbody, null, false);
		UnityEngine.Object.Destroy(debrisObject);
		specRigidbody.RegenerateCache();
	}

	// Token: 0x06005A4F RID: 23119 RVA: 0x002284DC File Offset: 0x002266DC
	private IEnumerator DelayedSpriteFixer(tk2dBaseSprite sprite)
	{
		yield return null;
		sprite.HeightOffGround = -1f;
		sprite.IsPerpendicular = true;
		sprite.UpdateZDepth();
		yield break;
	}

	// Token: 0x040053B6 RID: 21430
	public List<GameObject> explosionVfx;

	// Token: 0x040053B7 RID: 21431
	public float explosionMidDelay = 0.3f;

	// Token: 0x040053B8 RID: 21432
	public int explosionCount = 10;

	// Token: 0x040053B9 RID: 21433
	public GameObject skullDebris;

	// Token: 0x040053BA RID: 21434
	public GameObject fingerDebris;

	// Token: 0x040053BB RID: 21435
	public GameObject neckDebris;

	// Token: 0x040053BC RID: 21436
	private DraGunController m_dragunController;

	// Token: 0x040053BD RID: 21437
	private tk2dSpriteAnimator m_deathDummy;
}
