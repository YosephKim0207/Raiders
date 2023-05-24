using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001042 RID: 4162
public class HelicopterDeathController : BraveBehaviour
{
	// Token: 0x06005B56 RID: 23382 RVA: 0x0022FD94 File Offset: 0x0022DF94
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
	}

	// Token: 0x06005B57 RID: 23383 RVA: 0x0022FDBC File Offset: 0x0022DFBC
	private void OnBossDeath(Vector2 dir)
	{
		base.StartCoroutine(this.HandleBossDeath());
		AkSoundEngine.PostEvent("Play_State_Volume_Lower_01", base.gameObject);
	}

	// Token: 0x06005B58 RID: 23384 RVA: 0x0022FDDC File Offset: 0x0022DFDC
	private IEnumerator HandleBossDeath()
	{
		base.aiAnimator.PlayUntilCancelled("death", true, null, -1f, false);
		base.healthHaver.OverrideKillCamTime = new float?(6f);
		yield return base.StartCoroutine(base.GetComponent<VoiceOverer>().HandlePlayerWonVO(4f));
		GameManager.Instance.StartCoroutine(this.HandleLittleExplosionsCR());
		GameManager.Instance.StartCoroutine(this.HandleBigExplosionsCR());
		GameManager.Instance.StartCoroutine(this.SinkCR());
		AkSoundEngine.PostEvent("Play_boss_helicopter_death_01", base.gameObject);
		yield break;
	}

	// Token: 0x06005B59 RID: 23385 RVA: 0x0022FDF8 File Offset: 0x0022DFF8
	private IEnumerator HandleLittleExplosionsCR()
	{
		int i = 0;
		while (i < this.explosionCount && !this.m_isDestroyed)
		{
			GameObject vfxObj = SpawnManager.SpawnVFX(this.explosionVfx, this.RandomExplosionPos(), Quaternion.identity);
			tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = 0.8f;
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			yield return new WaitForSeconds(this.explosionMidDelay);
			i++;
		}
		yield break;
	}

	// Token: 0x06005B5A RID: 23386 RVA: 0x0022FE14 File Offset: 0x0022E014
	private IEnumerator HandleBigExplosionsCR()
	{
		CameraController camera = GameManager.Instance.MainCameraController;
		camera.DoContinuousScreenShake(this.screenShake, this, false);
		AkSoundEngine.PostEvent("Stop_State_Volume_Lower_01", base.gameObject);
		yield return new WaitForSeconds((float)this.explosionCount * this.explosionMidDelay - (float)this.bigExplosionCount * this.bigExplosionMidDelay);
		int i = 0;
		while (i < this.bigExplosionCount && !this.m_isDestroyed)
		{
			GameObject vfxObj = SpawnManager.SpawnVFX(this.bigExplosionVfx, this.RandomExplosionPos(), Quaternion.identity);
			tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = 0.8f + UnityEngine.Random.value * 0.5f;
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			yield return new WaitForSeconds(this.bigExplosionMidDelay);
			i++;
		}
		camera.StopContinuousScreenShake(this);
		base.GetComponent<HelicopterIntroDoer>().IsCameraModified = false;
		camera.OverrideZoomScale = 1f;
		ExplosionDebrisLauncher[] debris = base.GetComponentsInChildren<ExplosionDebrisLauncher>();
		debris[0].Launch(BraveMathCollege.DegreesToVector(150f, 1f));
		debris[1].Launch(BraveMathCollege.DegreesToVector(30f, 1f));
		debris[2].Launch(BraveMathCollege.DegreesToVector(90f + BraveUtility.RandomSign() * 45f, 1f));
		debris[3].Launch(BraveMathCollege.DegreesToVector(90f + BraveUtility.RandomSign() * 45f, 1f));
		debris[4].Launch(BraveMathCollege.DegreesToVector(75f, 1f));
		debris[5].Launch(BraveMathCollege.DegreesToVector(105f, 1f));
		debris[6].Launch(BraveMathCollege.DegreesToVector(90f + BraveUtility.RandomSign() * 45f, 1f));
		debris[7].Launch(BraveMathCollege.DegreesToVector(135f, 1f));
		debris[8].Launch(BraveMathCollege.DegreesToVector(45f, 1f));
		base.aiActor.StealthDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		GameManager.Instance.Dungeon.StartCoroutine(this.HandleFlightPitfall());
		this.m_isDestroyed = true;
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06005B5B RID: 23387 RVA: 0x0022FE30 File Offset: 0x0022E030
	private IEnumerator HandleFlightPitfall()
	{
		yield return null;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (playerController)
			{
				playerController.LevelToLoadOnPitfall = "tt_forge";
			}
		}
		yield return new WaitForSeconds(1f);
		while (!Dungeon.IsGenerating && !GameManager.Instance.IsLoadingLevel)
		{
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				PlayerController playerController2 = GameManager.Instance.AllPlayers[j];
				if (playerController2 && playerController2.IsFlying && !playerController2.IsGhost && playerController2.CurrentRoom != null && playerController2.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
				{
					CellData cell = playerController2.CenterPosition.GetCell();
					if (cell != null && cell.type == CellType.PIT)
					{
						playerController2.ForceFall();
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06005B5C RID: 23388 RVA: 0x0022FE44 File Offset: 0x0022E044
	private IEnumerator SinkCR()
	{
		GameObject shadowObj = base.aiActor.ShadowObject;
		Vector2 velocity = new Vector2(0f, -0.4f);
		while (!this.m_isDestroyed)
		{
			base.specRigidbody.Velocity = velocity;
			shadowObj.transform.localPosition -= velocity * BraveTime.DeltaTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06005B5D RID: 23389 RVA: 0x0022FE60 File Offset: 0x0022E060
	private Vector2 RandomExplosionPos()
	{
		Vector2 vector = base.transform.position;
		int num = UnityEngine.Random.Range(0, 8);
		if (num == 0)
		{
			return vector + BraveUtility.RandomVector2(new Vector2(0.75f, 4.625f), new Vector2(3.875f, 5.25f));
		}
		if (num == 1)
		{
			return vector + BraveUtility.RandomVector2(new Vector2(5.625f, 4.625f), new Vector2(8.75f, 5.25f));
		}
		return vector + BraveUtility.RandomVector2(new Vector2(3.875f, 2f), new Vector2(5.625f, 8.375f));
	}

	// Token: 0x040054EE RID: 21742
	public ScreenShakeSettings screenShake;

	// Token: 0x040054EF RID: 21743
	public GameObject explosionVfx;

	// Token: 0x040054F0 RID: 21744
	private float explosionMidDelay = 0.1f;

	// Token: 0x040054F1 RID: 21745
	private int explosionCount = 35;

	// Token: 0x040054F2 RID: 21746
	public GameObject bigExplosionVfx;

	// Token: 0x040054F3 RID: 21747
	private float bigExplosionMidDelay = 0.2f;

	// Token: 0x040054F4 RID: 21748
	private int bigExplosionCount = 10;

	// Token: 0x040054F5 RID: 21749
	private bool m_isDestroyed;
}
