using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020010C8 RID: 4296
public class SuperReaperController : BraveBehaviour
{
	// Token: 0x17000DE8 RID: 3560
	// (get) Token: 0x06005E9A RID: 24218 RVA: 0x0024559C File Offset: 0x0024379C
	public static SuperReaperController Instance
	{
		get
		{
			return SuperReaperController.m_instance;
		}
	}

	// Token: 0x06005E9B RID: 24219 RVA: 0x002455A4 File Offset: 0x002437A4
	private void Start()
	{
		SuperReaperController.m_instance = this;
		this.m_shootTimer = this.ShootTimer;
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTriggerEntered));
		base.aiAnimator.PlayUntilCancelled("idle", false, null, -1f, false);
		base.aiAnimator.PlayUntilFinished("intro", false, null, -1f, false);
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
		for (int i = 0; i < EncounterDatabase.Instance.Entries.Count; i++)
		{
			if (EncounterDatabase.Instance.Entries[i].journalData.PrimaryDisplayName == "#SREAPER_ENCNAME")
			{
				GameStatsManager.Instance.HandleEncounteredObjectRaw(EncounterDatabase.Instance.Entries[i].myGuid);
			}
		}
		this.m_currentTargetPlayer = GameManager.Instance.GetRandomActivePlayer();
		if (base.encounterTrackable)
		{
			GameStatsManager.Instance.HandleEncounteredObject(base.encounterTrackable);
		}
	}

	// Token: 0x06005E9C RID: 24220 RVA: 0x002456DC File Offset: 0x002438DC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		SuperReaperController.m_instance = null;
	}

	// Token: 0x06005E9D RID: 24221 RVA: 0x002456EC File Offset: 0x002438EC
	private void HandleTriggerEntered(SpeculativeRigidbody targetRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		Projectile projectile = targetRigidbody.projectile;
		if (projectile)
		{
			projectile.HandleKnockback(base.specRigidbody, targetRigidbody.GetComponent<PlayerController>(), false, false);
		}
	}

	// Token: 0x06005E9E RID: 24222 RVA: 0x00245720 File Offset: 0x00243920
	private void HandleAnimationEvent(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2, int arg3)
	{
		tk2dSpriteAnimationFrame frame = arg2.GetFrame(arg3);
		if (frame.eventInfo == "fire")
		{
			this.SpawnProjectiles();
		}
	}

	// Token: 0x06005E9F RID: 24223 RVA: 0x00245750 File Offset: 0x00243950
	private void Update()
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			return;
		}
		if (BossKillCam.BossDeathCamRunning || GameManager.Instance.PreventPausing)
		{
			return;
		}
		if (TimeTubeCreditsController.IsTimeTubing)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.HandleMotion();
		if (!SuperReaperController.PreventShooting)
		{
			this.HandleAttacks();
		}
		this.UpdateBlackPhantomParticles();
	}

	// Token: 0x06005EA0 RID: 24224 RVA: 0x002457DC File Offset: 0x002439DC
	private void HandleAttacks()
	{
		if (base.aiAnimator.IsPlaying("intro"))
		{
			return;
		}
		CellData cellData = GameManager.Instance.Dungeon.data[this.ShootPoint.position.IntXY(VectorConversions.Floor)];
		if (cellData != null && cellData.type != CellType.WALL)
		{
			this.m_shootTimer -= BraveTime.DeltaTime;
			if (this.m_shootTimer <= 0f)
			{
				base.aiAnimator.PlayUntilFinished("attack", false, null, -1f, false);
				this.m_shootTimer = this.ShootTimer;
			}
		}
	}

	// Token: 0x06005EA1 RID: 24225 RVA: 0x00245880 File Offset: 0x00243A80
	private void SpawnProjectiles()
	{
		if (GameManager.Instance.PreventPausing || BossKillCam.BossDeathCamRunning)
		{
			return;
		}
		if (SuperReaperController.PreventShooting)
		{
			return;
		}
		CellData cellData = GameManager.Instance.Dungeon.data[this.ShootPoint.position.IntXY(VectorConversions.Floor)];
		if (cellData == null || cellData.type == CellType.WALL)
		{
			return;
		}
		if (!this.m_bulletSource)
		{
			this.m_bulletSource = this.ShootPoint.gameObject.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletSource.BulletManager = base.bulletBank;
		this.m_bulletSource.BulletScript = this.BulletScript;
		this.m_bulletSource.Initialize();
	}

	// Token: 0x06005EA2 RID: 24226 RVA: 0x00245940 File Offset: 0x00243B40
	private void UpdateBlackPhantomParticles()
	{
		if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW && !base.aiAnimator.IsPlaying("intro"))
		{
			Vector3 vector = base.specRigidbody.UnitBottomLeft.ToVector3ZisY(0f);
			Vector3 vector2 = base.specRigidbody.UnitTopRight.ToVector3ZisY(0f);
			float num = (vector2.y - vector.y) * (vector2.x - vector.x);
			float num2 = (float)this.c_particlesPerSecond * num;
			int num3 = Mathf.CeilToInt(Mathf.Max(1f, num2 * BraveTime.DeltaTime));
			int num4 = num3;
			Vector3 vector3 = vector;
			Vector3 vector4 = vector2;
			Vector3 up = Vector3.up;
			float num5 = 120f;
			float num6 = 0.5f;
			float? num7 = new float?(UnityEngine.Random.Range(1f, 1.65f));
			GlobalSparksDoer.DoRandomParticleBurst(num4, vector3, vector4, up, num5, num6, null, num7, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
		}
	}

	// Token: 0x06005EA3 RID: 24227 RVA: 0x00245A4C File Offset: 0x00243C4C
	private void HandleMotion()
	{
		base.specRigidbody.Velocity = Vector2.zero;
		if (base.aiAnimator.IsPlaying("intro"))
		{
			return;
		}
		if (this.m_currentTargetPlayer == null)
		{
			return;
		}
		if (this.m_currentTargetPlayer.healthHaver.IsDead || this.m_currentTargetPlayer.IsGhost)
		{
			this.m_currentTargetPlayer = GameManager.Instance.GetRandomActivePlayer();
		}
		Vector2 centerPosition = this.m_currentTargetPlayer.CenterPosition;
		Vector2 vector = centerPosition - base.specRigidbody.UnitCenter;
		float magnitude = vector.magnitude;
		float num = Mathf.Lerp(this.MinSpeed, this.MaxSpeed, (magnitude - this.MinSpeedDistance) / (this.MaxSpeedDistance - this.MinSpeedDistance));
		base.specRigidbody.Velocity = vector.normalized * num;
		base.specRigidbody.Velocity += this.knockbackComponent;
	}

	// Token: 0x040058CA RID: 22730
	private static SuperReaperController m_instance;

	// Token: 0x040058CB RID: 22731
	public static bool PreventShooting;

	// Token: 0x040058CC RID: 22732
	public BulletScriptSelector BulletScript;

	// Token: 0x040058CD RID: 22733
	public Transform ShootPoint;

	// Token: 0x040058CE RID: 22734
	public float ShootTimer = 3f;

	// Token: 0x040058CF RID: 22735
	public float MinSpeed = 3f;

	// Token: 0x040058D0 RID: 22736
	public float MaxSpeed = 10f;

	// Token: 0x040058D1 RID: 22737
	public float MinSpeedDistance = 10f;

	// Token: 0x040058D2 RID: 22738
	public float MaxSpeedDistance = 50f;

	// Token: 0x040058D3 RID: 22739
	[NonSerialized]
	public Vector2 knockbackComponent;

	// Token: 0x040058D4 RID: 22740
	private PlayerController m_currentTargetPlayer;

	// Token: 0x040058D5 RID: 22741
	private BulletScriptSource m_bulletSource;

	// Token: 0x040058D6 RID: 22742
	private float m_shootTimer;

	// Token: 0x040058D7 RID: 22743
	private int c_particlesPerSecond = 50;
}
