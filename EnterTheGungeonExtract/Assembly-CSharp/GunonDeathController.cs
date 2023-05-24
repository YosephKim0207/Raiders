using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200103E RID: 4158
public class GunonDeathController : BraveBehaviour
{
	// Token: 0x06005B45 RID: 23365 RVA: 0x0022F734 File Offset: 0x0022D934
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		base.healthHaver.OverrideKillCamTime = new float?(5f);
	}

	// Token: 0x06005B46 RID: 23366 RVA: 0x0022F770 File Offset: 0x0022D970
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005B47 RID: 23367 RVA: 0x0022F778 File Offset: 0x0022D978
	private void OnBossDeath(Vector2 dir)
	{
		base.aiAnimator.PlayUntilCancelled("death", true, null, -1f, false);
		base.StartCoroutine(this.HandleBossDeath());
		base.healthHaver.OnPreDeath -= this.OnBossDeath;
		AkSoundEngine.PostEvent("Play_BOSS_lichB_explode_01", base.gameObject);
	}

	// Token: 0x06005B48 RID: 23368 RVA: 0x0022F7D4 File Offset: 0x0022D9D4
	private IEnumerator HandleBossDeath()
	{
		PixelCollider collider = base.specRigidbody.HitboxPixelCollider;
		GameManager.Instance.MainCameraController.DoContinuousScreenShake(new ScreenShakeSettings(2f, 20f, 1f, 0f, Vector2.right), this, false);
		bool faded = false;
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
			if (!faded && (float)i * this.explosionMidDelay < 2f)
			{
				Pixelator.Instance.FadeToColor(2f, Color.white, false, 0f);
				faded = true;
			}
			yield return new WaitForSeconds(this.explosionMidDelay);
		}
		GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
		BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
		if (extantCam)
		{
			extantCam.ForceCancelSequence();
		}
		PlayerController[] allPlayers = GameManager.Instance.AllPlayers;
		for (int j = 0; j < allPlayers.Length; j++)
		{
			allPlayers[j].CurrentInputState = PlayerInputState.NoInput;
		}
		GameManager.Instance.PrimaryPlayer.IsVisible = false;
		GameManager.Instance.MainCameraController.SetManualControl(true, false);
		GameManager.Instance.MainCameraController.OverridePosition = base.sprite.WorldCenter;
		Pixelator.Instance.FadeToColor(0.5f, Color.white, true, 0f);
		base.aiAnimator.PlayUntilCancelled("postdeath", false, null, -1f, false);
		base.aiActor.ShadowObject.transform.localPosition += new Vector3(0f, 0.625f, 0f);
		yield return new WaitForSeconds(7.3f);
		Pixelator.Instance.FadeToColor(1f, new Color(0.8f, 0.8f, 0.8f), false, 0f);
		yield return new WaitForSeconds(1f);
		Pixelator.Instance.FadeToColor(0.6f, new Color(0.8f, 0.8f, 0.8f), true, 0f);
		yield return new WaitForSeconds(1.6f);
		Pixelator.Instance.FadeToBlack(2f, false, 0f);
		yield return new WaitForSeconds(2f);
		GameManager.Instance.PrimaryPlayer.IsVisible = true;
		BulletPastRoomController[] pastRooms = UnityEngine.Object.FindObjectsOfType<BulletPastRoomController>();
		for (int k = 0; k < pastRooms.Length; k++)
		{
			pastRooms[k].TriggerBulletmanEnding();
		}
		base.healthHaver.DeathAnimationComplete(null, null);
		yield break;
	}

	// Token: 0x040054DB RID: 21723
	public List<GameObject> explosionVfx;

	// Token: 0x040054DC RID: 21724
	public float explosionMidDelay = 0.3f;

	// Token: 0x040054DD RID: 21725
	public int explosionCount = 10;
}
