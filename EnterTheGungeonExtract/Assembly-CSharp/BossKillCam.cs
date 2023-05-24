using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200174A RID: 5962
public class BossKillCam : TimeInvariantMonoBehaviour
{
	// Token: 0x06008ABC RID: 35516 RVA: 0x0039CC30 File Offset: 0x0039AE30
	public void ForceCancelSequence()
	{
		Debug.Log("force ending sequence");
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.healthHaver.IsVulnerable = true;
			playerController.gameActor.SuppressEffectUpdates = false;
			playerController.ClearInputOverride("bossKillCam");
		}
		this.m_targetTimeScale = 1f;
		BraveTime.ClearMultiplier(base.gameObject);
		StickyFrictionManager.Instance.FrictionEnabled = true;
		this.m_isRunning = false;
		BossKillCam.BossDeathCamRunning = false;
		GameUIRoot.Instance.EndBossKillCam();
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06008ABD RID: 35517 RVA: 0x0039CCD4 File Offset: 0x0039AED4
	public void SetPhaseCountdown(float value)
	{
		this.m_phaseCountdown = value;
	}

	// Token: 0x06008ABE RID: 35518 RVA: 0x0039CCE0 File Offset: 0x0039AEE0
	public void TriggerSequence(Projectile p, SpeculativeRigidbody bossSRB)
	{
		this.m_projectile = p;
		this.m_bossRigidbody = bossSRB;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.healthHaver.IsVulnerable = false;
			playerController.gameActor.SuppressEffectUpdates = true;
			playerController.SetInputOverride("bossKillCam");
			playerController.IsOnFire = false;
			playerController.CurrentFireMeterValue = 0f;
			playerController.CurrentPoisonMeterValue = 0f;
			DeadlyDeadlyGoopManager.DelayedClearGoopsInRadius(playerController.specRigidbody.UnitCenter, 1f);
			playerController.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(5f);
		}
		this.m_targetTimeScale = 0.2f;
		StickyFrictionManager.Instance.FrictionEnabled = false;
		this.m_camera = GameManager.Instance.MainCameraController;
		this.m_camera.StopTrackingPlayer();
		this.m_camera.SetManualControl(true, false);
		this.m_camera.OverridePosition = this.m_camera.transform.position;
		GenericIntroDoer component = bossSRB.GetComponent<GenericIntroDoer>();
		if (component && component.cameraFocus)
		{
			this.m_camera.RemoveFocusPoint(component.cameraFocus);
		}
		this.m_cameraTransform = this.m_camera.transform;
		this.m_isRunning = true;
		BossKillCam.BossDeathCamRunning = true;
		if (this.m_projectile != null)
		{
			this.m_currentPhase = 0;
		}
		else
		{
			this.m_currentPhase = 1;
			Vector2? overrideKillCamPos = bossSRB.healthHaver.OverrideKillCamPos;
			Vector2 vector = ((overrideKillCamPos == null) ? bossSRB.UnitCenter : overrideKillCamPos.Value);
			this.m_suppressContinuousBulletDestruction = bossSRB.healthHaver.SuppressContinuousKillCamBulletDestruction;
			CutsceneMotion cutsceneMotion = new CutsceneMotion(this.m_cameraTransform, new Vector2?(vector), Vector2.Distance(this.m_cameraTransform.position.XY(), vector) / this.trackToBossTime, 0f);
			cutsceneMotion.camera = this.m_camera;
			this.activeMotions.Add(cutsceneMotion);
			this.m_phaseComplete = false;
		}
	}

	// Token: 0x06008ABF RID: 35519 RVA: 0x0039CEEC File Offset: 0x0039B0EC
	public static void ClearPerLevelData()
	{
		BossKillCam.hackGatlingGullOutroDoer = null;
	}

	// Token: 0x06008AC0 RID: 35520 RVA: 0x0039CEF4 File Offset: 0x0039B0F4
	private void EndSequence()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.healthHaver.IsVulnerable = true;
			playerController.gameActor.SuppressEffectUpdates = false;
			playerController.ClearInputOverride("bossKillCam");
			playerController.IsOnFire = false;
			playerController.CurrentFireMeterValue = 0f;
			playerController.CurrentPoisonMeterValue = 0f;
			DeadlyDeadlyGoopManager.DelayedClearGoopsInRadius(playerController.specRigidbody.UnitCenter, 1f);
		}
		this.m_targetTimeScale = 1f;
		BraveTime.ClearMultiplier(base.gameObject);
		StickyFrictionManager.Instance.FrictionEnabled = true;
		this.m_camera.StartTrackingPlayer();
		this.m_camera.SetManualControl(false, true);
		this.m_isRunning = false;
		BossKillCam.BossDeathCamRunning = false;
		GameUIRoot.Instance.EndBossKillCam();
		if (BossKillCam.hackGatlingGullOutroDoer != null)
		{
			BossKillCam.hackGatlingGullOutroDoer.TriggerSequence();
		}
		BossKillCam.hackGatlingGullOutroDoer = null;
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06008AC1 RID: 35521 RVA: 0x0039CFF8 File Offset: 0x0039B1F8
	protected override void InvariantUpdate(float realDeltaTime)
	{
		if (!this.m_isRunning)
		{
			return;
		}
		if (!this.m_suppressContinuousBulletDestruction)
		{
			StaticReferenceManager.DestroyAllEnemyProjectiles();
		}
		this.KillAllEnemies();
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.healthHaver.IsVulnerable = false;
			playerController.gameActor.SuppressEffectUpdates = true;
			playerController.SetInputOverride("bossKillCam");
			playerController.IsOnFire = false;
			playerController.CurrentFireMeterValue = 0f;
			playerController.CurrentPoisonMeterValue = 0f;
			DeadlyDeadlyGoopManager.DelayedClearGoopsInRadius(playerController.specRigidbody.UnitCenter, 1f);
		}
		if (Time.timeScale != this.m_targetTimeScale)
		{
			float num = ((this.m_targetTimeScale <= Time.timeScale) ? 1f : this.m_targetTimeScale);
			float num2 = ((this.m_targetTimeScale <= Time.timeScale) ? this.m_targetTimeScale : 0f);
			BraveTime.SetTimeScaleMultiplier(Mathf.Clamp(Time.timeScale + (this.m_targetTimeScale - Time.timeScale) * (realDeltaTime / 0.1f), num2, num), base.gameObject);
		}
		for (int j = 0; j < this.activeMotions.Count; j++)
		{
			CutsceneMotion cutsceneMotion = this.activeMotions[j];
			Vector2? lerpEnd = cutsceneMotion.lerpEnd;
			Vector2 vector = ((lerpEnd == null) ? GameManager.Instance.MainCameraController.GetIdealCameraPosition() : cutsceneMotion.lerpEnd.Value);
			float num3 = Vector2.Distance(vector, cutsceneMotion.lerpStart);
			float num4 = cutsceneMotion.speed * realDeltaTime;
			float num5 = num4 / num3;
			cutsceneMotion.lerpProgress = Mathf.Clamp01(cutsceneMotion.lerpProgress + num5);
			float num6 = cutsceneMotion.lerpProgress;
			if (cutsceneMotion.isSmoothStepped)
			{
				num6 = Mathf.SmoothStep(0f, 1f, num6);
			}
			Vector2 vector2 = Vector2.Lerp(cutsceneMotion.lerpStart, vector, num6);
			if (cutsceneMotion.camera != null)
			{
				cutsceneMotion.camera.OverridePosition = vector2.ToVector3ZUp(cutsceneMotion.zOffset);
			}
			else
			{
				cutsceneMotion.transform.position = BraveUtility.QuantizeVector(vector2.ToVector3ZUp(cutsceneMotion.zOffset), (float)PhysicsEngine.Instance.PixelsPerUnit);
			}
			if (cutsceneMotion.lerpProgress == 1f)
			{
				this.activeMotions.RemoveAt(j);
				j--;
				if (this.activeMotions.Count == 0)
				{
					this.m_currentPhase++;
					this.m_phaseComplete = true;
				}
			}
		}
		if (this.m_currentPhase == 0)
		{
			if (!this.m_bossRigidbody || this.m_bossRigidbody.healthHaver.IsDead)
			{
				this.m_currentPhase += 2;
				this.m_phaseComplete = true;
			}
			else
			{
				if (!this.m_projectile || !this.m_projectile.specRigidbody)
				{
					this.EndSequence();
					return;
				}
				this.m_camera.OverridePosition = this.m_projectile.specRigidbody.UnitCenter.ToVector3ZUp(0f);
			}
		}
		else if (this.m_currentPhase == 2 && this.m_bossRigidbody && this.m_bossRigidbody.healthHaver.TrackDuringDeath)
		{
			GameManager.Instance.MainCameraController.OverridePosition = this.m_bossRigidbody.GetUnitCenter(ColliderType.HitBox);
		}
		if (this.m_currentPhase <= 2)
		{
			float num7 = ((this.m_currentPhase >= 2) ? Mathf.Clamp01(this.m_phaseCountdown) : 1f);
			BraveInput.DoSustainedScreenShakeVibration(num7);
		}
		if (this.m_phaseComplete)
		{
			switch (this.m_currentPhase)
			{
			case 1:
				this.m_phaseComplete = false;
				break;
			case 2:
				this.m_targetTimeScale = 1f;
				if (this.m_bossRigidbody && this.m_bossRigidbody.healthHaver && this.m_bossRigidbody.healthHaver.OverrideKillCamTime != null)
				{
					this.m_phaseCountdown = this.m_bossRigidbody.healthHaver.OverrideKillCamTime.Value;
				}
				else
				{
					this.m_phaseCountdown = 3f;
				}
				this.m_phaseComplete = false;
				break;
			case 3:
			{
				GameManager.Instance.MainCameraController.ForceUpdateControllerCameraState(CameraController.ControllerCameraState.FollowPlayer);
				Vector2 coreCurrentBasePosition = this.m_camera.GetCoreCurrentBasePosition();
				CutsceneMotion cutsceneMotion2 = new CutsceneMotion(this.m_cameraTransform, null, Vector2.Distance(this.m_cameraTransform.position.XY(), coreCurrentBasePosition) / this.returnToPlayerTime, 0f);
				cutsceneMotion2.camera = this.m_camera;
				this.activeMotions.Add(cutsceneMotion2);
				this.m_phaseComplete = false;
				break;
			}
			case 4:
				this.EndSequence();
				return;
			}
		}
		if (this.m_phaseCountdown > 0f)
		{
			this.m_phaseCountdown -= realDeltaTime;
			if (this.m_phaseCountdown <= 0f)
			{
				this.m_phaseCountdown = 0f;
				this.m_currentPhase++;
				this.m_phaseComplete = true;
			}
		}
	}

	// Token: 0x06008AC2 RID: 35522 RVA: 0x0039D564 File Offset: 0x0039B764
	private void KillAllEnemies()
	{
		if (GameManager.Instance.BestActivePlayer)
		{
			RoomHandler currentRoom = GameManager.Instance.BestActivePlayer.CurrentRoom;
			currentRoom.ClearReinforcementLayers();
			List<AIActor> activeEnemies = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{
				List<AIActor> list = new List<AIActor>(activeEnemies);
				for (int i = 0; i < list.Count; i++)
				{
					AIActor aiactor = list[i];
					if (!aiactor.PreventAutoKillOnBossDeath)
					{
						SpawnEnemyOnDeath component = aiactor.GetComponent<SpawnEnemyOnDeath>();
						if (component)
						{
							UnityEngine.Object.Destroy(component);
						}
						aiactor.healthHaver.minimumHealth = 0f;
						aiactor.healthHaver.ApplyDamage(10000f, Vector2.zero, "Boss Kill", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
					}
				}
			}
		}
	}

	// Token: 0x06008AC3 RID: 35523 RVA: 0x0039D630 File Offset: 0x0039B830
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04009177 RID: 37239
	public static bool BossDeathCamRunning;

	// Token: 0x04009178 RID: 37240
	public float trackToBossTime = 0.75f;

	// Token: 0x04009179 RID: 37241
	public float returnToPlayerTime = 1f;

	// Token: 0x0400917A RID: 37242
	protected bool m_isRunning;

	// Token: 0x0400917B RID: 37243
	protected Projectile m_projectile;

	// Token: 0x0400917C RID: 37244
	protected SpeculativeRigidbody m_bossRigidbody;

	// Token: 0x0400917D RID: 37245
	protected CameraController m_camera;

	// Token: 0x0400917E RID: 37246
	protected Transform m_cameraTransform;

	// Token: 0x0400917F RID: 37247
	protected float m_phaseCountdown;

	// Token: 0x04009180 RID: 37248
	protected int m_currentPhase;

	// Token: 0x04009181 RID: 37249
	protected bool m_phaseComplete = true;

	// Token: 0x04009182 RID: 37250
	protected float m_targetTimeScale = 1f;

	// Token: 0x04009183 RID: 37251
	protected bool m_suppressContinuousBulletDestruction;

	// Token: 0x04009184 RID: 37252
	protected List<CutsceneMotion> activeMotions = new List<CutsceneMotion>();

	// Token: 0x04009185 RID: 37253
	public static GatlingGullOutroDoer hackGatlingGullOutroDoer;
}
