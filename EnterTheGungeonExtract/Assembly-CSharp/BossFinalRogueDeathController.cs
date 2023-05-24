using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FED RID: 4077
public class BossFinalRogueDeathController : BraveBehaviour
{
	// Token: 0x060058F8 RID: 22776 RVA: 0x0021F408 File Offset: 0x0021D608
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
	}

	// Token: 0x060058F9 RID: 22777 RVA: 0x0021F430 File Offset: 0x0021D630
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060058FA RID: 22778 RVA: 0x0021F438 File Offset: 0x0021D638
	private void OnBossDeath(Vector2 dir)
	{
		base.behaviorSpeculator.enabled = false;
		base.aiActor.BehaviorOverridesVelocity = true;
		base.aiActor.BehaviorVelocity = Vector2.zero;
		base.aiAnimator.PlayUntilCancelled("die", false, null, -1f, false);
		base.StartCoroutine(this.Drift());
		base.StartCoroutine(this.OnDeathExplosionsCR());
	}

	// Token: 0x060058FB RID: 22779 RVA: 0x0021F4A0 File Offset: 0x0021D6A0
	private IEnumerator Drift()
	{
		BossFinalRogueController bossController = base.GetComponent<BossFinalRogueController>();
		Vector2 initialLockPos = bossController.CameraPos;
		bossController.EndCameraLock();
		while (base.gameObject)
		{
			GameManager.Instance.MainCameraController.OverridePosition = initialLockPos;
			base.transform.position = base.transform.position + new Vector3(1f, -1f, 0f) * BraveTime.DeltaTime;
			base.specRigidbody.Reinitialize();
			yield return null;
		}
		yield break;
	}

	// Token: 0x060058FC RID: 22780 RVA: 0x0021F4BC File Offset: 0x0021D6BC
	private IEnumerator OnDeathExplosionsCR()
	{
		yield return null;
		BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
		if (extantCam)
		{
			extantCam.ForceCancelSequence();
		}
		GameManager.Instance.MainCameraController.DoContinuousScreenShake(new ScreenShakeSettings(2f, 20f, 1f, 0f, Vector2.right), this, false);
		for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
		{
			GameManager.Instance.AllPlayers[k].SetInputOverride("past");
		}
		PixelCollider collider = base.specRigidbody.HitboxPixelCollider;
		for (int i = 0; i < this.explosionCount; i++)
		{
			Vector2 minPos = collider.UnitBottomLeft;
			Vector2 maxPos = collider.UnitTopRight;
			GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
			Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.5f, 0.5f));
			GameObject vfxObj = SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
			tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = 3f;
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			if (i < this.explosionCount - 1)
			{
				yield return new WaitForSeconds(this.explosionMidDelay);
			}
		}
		for (int j = 0; j < this.bigExplosionCount; j++)
		{
			Vector2 minPos2 = collider.UnitBottomLeft;
			Vector2 maxPos2 = collider.UnitTopRight;
			GameObject vfxPrefab2 = BraveUtility.RandomElement<GameObject>(this.bigExplosionVfx);
			Vector2 pos2 = BraveUtility.RandomVector2(minPos2, maxPos2, new Vector2(1f, 1f));
			GameObject vfxObj2 = SpawnManager.SpawnVFX(vfxPrefab2, pos2, Quaternion.identity);
			tk2dBaseSprite vfxSprite2 = vfxObj2.GetComponent<tk2dBaseSprite>();
			vfxSprite2.HeightOffGround = 3f;
			base.sprite.AttachRenderer(vfxSprite2);
			base.sprite.UpdateZDepth();
			if (j < this.bigExplosionCount - 1)
			{
				yield return new WaitForSeconds(this.bigExplosionMidDelay);
			}
			else if (this.DeathStarExplosionVFX != null)
			{
				GameObject deathStarObj = SpawnManager.SpawnVFX(this.DeathStarExplosionVFX, collider.UnitCenter, Quaternion.identity);
				tk2dBaseSprite deathStarSprite = deathStarObj.GetComponent<tk2dBaseSprite>();
				deathStarSprite.HeightOffGround = 3f;
				base.sprite.AttachRenderer(deathStarSprite);
				base.sprite.UpdateZDepth();
				AkSoundEngine.PostEvent("Play_BOSS_queenship_explode_01", base.gameObject);
				base.sprite.renderer.enabled = false;
				for (int l = 0; l < base.healthHaver.bodySprites.Count; l++)
				{
					if (base.healthHaver.bodySprites[l])
					{
						base.healthHaver.bodySprites[l].renderer.enabled = false;
					}
				}
				yield return new WaitForSeconds(1f);
				Pixelator.Instance.FadeToColor(2f, Color.white, false, 0f);
				yield return new WaitForSeconds(2f);
				Pixelator.Instance.FadeToColor(2f, Color.white, true, 1f);
			}
			else
			{
				Pixelator.Instance.FadeToColor(3f, Color.white, true, 1f);
			}
		}
		GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
		base.healthHaver.DeathAnimationComplete(null, null);
		UnityEngine.Object.Destroy(base.gameObject);
		PilotPastController ppc = UnityEngine.Object.FindObjectOfType<PilotPastController>();
		GameManager.Instance.MainCameraController.SetManualControl(false, true);
		ppc.OnBossKilled();
		yield break;
	}

	// Token: 0x04005216 RID: 21014
	public List<GameObject> explosionVfx;

	// Token: 0x04005217 RID: 21015
	public float explosionMidDelay = 0.3f;

	// Token: 0x04005218 RID: 21016
	public int explosionCount = 10;

	// Token: 0x04005219 RID: 21017
	[Space(12f)]
	public List<GameObject> bigExplosionVfx;

	// Token: 0x0400521A RID: 21018
	public float bigExplosionMidDelay = 0.3f;

	// Token: 0x0400521B RID: 21019
	public int bigExplosionCount = 10;

	// Token: 0x0400521C RID: 21020
	public GameObject DeathStarExplosionVFX;
}
