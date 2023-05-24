using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001068 RID: 4200
public class MetalGearRatDeathController : BraveBehaviour
{
	// Token: 0x06005C61 RID: 23649 RVA: 0x002364B4 File Offset: 0x002346B4
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		base.healthHaver.OverrideKillCamTime = new float?(3.5f);
	}

	// Token: 0x06005C62 RID: 23650 RVA: 0x002364F0 File Offset: 0x002346F0
	protected override void OnDestroy()
	{
		if (ChallengeManager.CHALLENGE_MODE_ACTIVE && this.m_challengesSuppressed)
		{
			ChallengeManager.Instance.SuppressChallengeStart = false;
			this.m_challengesSuppressed = false;
		}
		base.OnDestroy();
	}

	// Token: 0x06005C63 RID: 23651 RVA: 0x00236520 File Offset: 0x00234720
	private void OnBossDeath(Vector2 dir)
	{
		base.aiAnimator.PlayUntilCancelled("death", false, null, -1f, false);
		base.aiAnimator.PlayVfx("death", null, null, null);
		GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());
		GameManager.Instance.StartCoroutine(this.OnDeathCR());
	}

	// Token: 0x06005C64 RID: 23652 RVA: 0x00236594 File Offset: 0x00234794
	private IEnumerator OnDeathExplosionsCR()
	{
		PixelCollider collider = base.specRigidbody.HitboxPixelCollider;
		for (int i = 0; i < this.explosionCount; i++)
		{
			Vector2 minPos = collider.UnitBottomLeft;
			Vector2 maxPos = collider.UnitTopRight;
			GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
			Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.2f, 0.2f));
			GameObject vfxObj = SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
			tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = 0.8f;
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			yield return new WaitForSeconds(this.explosionMidDelay);
		}
		yield break;
	}

	// Token: 0x06005C65 RID: 23653 RVA: 0x002365B0 File Offset: 0x002347B0
	private IEnumerator OnDeathCR()
	{
		SuperReaperController.PreventShooting = true;
		foreach (PlayerController playerController in GameManager.Instance.AllPlayers)
		{
			playerController.SetInputOverride("metal gear death");
		}
		yield return new WaitForSeconds(2f);
		Pixelator.Instance.FadeToColor(0.75f, Color.white, false, 0f);
		Minimap.Instance.TemporarilyPreventMinimap = true;
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
		yield return new WaitForSeconds(3f);
		MetalGearRatIntroDoer introDoer = base.GetComponent<MetalGearRatIntroDoer>();
		introDoer.ModifyCamera(false);
		yield return new WaitForSeconds(0.75f);
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
		MetalGearRatRoomController room = UnityEngine.Object.FindObjectOfType<MetalGearRatRoomController>();
		room.TransformToDestroyedRoom();
		GameManager.Instance.PrimaryPlayer.WarpToPoint(room.transform.position + new Vector3(17.25f, 14.5f), false, false);
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer)
		{
			GameManager.Instance.SecondaryPlayer.WarpToPoint(room.transform.position + new Vector3(27.5f, 14.5f), false, false);
		}
		base.aiActor.ToggleRenderers(false);
		GameObject punchoutMinigame = UnityEngine.Object.Instantiate<GameObject>(this.PunchoutMinigamePrefab);
		PunchoutController punchoutController = punchoutMinigame.GetComponent<PunchoutController>();
		punchoutController.Init();
		yield return null;
		foreach (PlayerController playerController2 in GameManager.Instance.AllPlayers)
		{
			playerController2.ClearInputOverride("metal gear death");
		}
		Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
		yield return new WaitForSeconds(1f);
		Minimap.Instance.TemporarilyPreventMinimap = false;
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04005601 RID: 22017
	public GameObject PunchoutMinigamePrefab;

	// Token: 0x04005602 RID: 22018
	public List<GameObject> explosionVfx;

	// Token: 0x04005603 RID: 22019
	public float explosionMidDelay = 0.3f;

	// Token: 0x04005604 RID: 22020
	public int explosionCount = 10;

	// Token: 0x04005605 RID: 22021
	private bool m_challengesSuppressed;
}
