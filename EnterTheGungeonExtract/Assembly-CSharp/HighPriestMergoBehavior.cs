using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000DC0 RID: 3520
[InspectorDropdownName("Bosses/HighPriest/MergoBehavior")]
public class HighPriestMergoBehavior : BasicAttackBehavior
{
	// Token: 0x06004AA8 RID: 19112 RVA: 0x00191514 File Offset: 0x0018F714
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06004AA9 RID: 19113 RVA: 0x0019151C File Offset: 0x0018F71C
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004AAA RID: 19114 RVA: 0x00191534 File Offset: 0x0018F734
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		this.m_shadowSprite = this.m_aiActor.ShadowObject.GetComponent<tk2dSprite>();
		this.m_state = HighPriestMergoBehavior.State.OutAnim;
		if (!this.m_aiActor.IsBlackPhantom)
		{
			this.m_aiActor.sprite.usesOverrideMaterial = true;
			this.m_aiActor.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
		}
		SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, false);
		this.m_aiAnimator.PlayUntilCancelled(this.teleportOutAnim, false, null, -1f, false);
		this.m_aiActor.specRigidbody.enabled = false;
		this.m_aiActor.ClearPath();
		this.m_aiActor.knockbackDoer.SetImmobile(true, "CrosshairBehavior");
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004AAB RID: 19115 RVA: 0x00191620 File Offset: 0x0018F820
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_state == HighPriestMergoBehavior.State.OutAnim)
		{
			this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(Mathf.Lerp(1f, 0f, this.m_aiAnimator.CurrentClipProgress));
			if (!this.m_aiAnimator.IsPlaying(this.teleportOutAnim))
			{
				this.m_state = HighPriestMergoBehavior.State.Fading;
				this.m_aiActor.ToggleRenderers(false);
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(0f);
				this.m_timer = this.darknessFadeTime;
				this.m_aiActor.ParentRoom.BecomeTerrifyingDarkRoom(1f, 0.1f, 1f, "Play_ENM_darken_world_01");
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.m_state == HighPriestMergoBehavior.State.Fading)
		{
			if (this.m_timer <= 0f)
			{
				this.m_state = HighPriestMergoBehavior.State.Firing;
				this.m_timer = this.fireTime;
				this.m_mainShotTimer = this.fireMainMidTime;
				this.m_wallShotTimer = this.fireWallMidTime;
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.m_state == HighPriestMergoBehavior.State.Firing)
		{
			if (this.m_timer <= 0f)
			{
				this.m_state = HighPriestMergoBehavior.State.Unfading;
				this.m_timer = this.darknessFadeTime;
				this.m_aiActor.ParentRoom.EndTerrifyingDarkRoom(1f, 0.1f, 1f, "Play_ENM_lighten_world_01");
				return ContinuousBehaviorResult.Continue;
			}
			this.m_mainShotTimer -= this.m_deltaTime;
			if (this.m_mainShotTimer < 0f)
			{
				this.ShootBulletScript();
				this.m_mainShotTimer += this.fireMainMidTime;
			}
			this.m_wallShotTimer -= this.m_deltaTime;
			if (this.m_wallShotTimer < 0f)
			{
				this.ShootWallBulletScript();
				this.m_wallShotTimer += this.fireWallMidTime;
			}
		}
		else if (this.m_state == HighPriestMergoBehavior.State.Unfading)
		{
			if (this.m_timer <= 0f)
			{
				this.m_state = HighPriestMergoBehavior.State.InAnim;
				this.m_aiActor.ToggleRenderers(true);
				this.m_aiAnimator.PlayUntilFinished(this.teleportInAnim, false, null, -1f, false);
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.m_state == HighPriestMergoBehavior.State.InAnim)
		{
			this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(Mathf.Lerp(0f, 1f, this.m_aiAnimator.CurrentClipProgress));
			if (!this.m_aiAnimator.IsPlaying(this.teleportInAnim))
			{
				if (!this.m_aiActor.IsBlackPhantom)
				{
					this.m_aiActor.sprite.usesOverrideMaterial = false;
					this.m_aiActor.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUber");
				}
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, true);
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f);
				this.m_aiActor.specRigidbody.enabled = true;
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004AAC RID: 19116 RVA: 0x00191944 File Offset: 0x0018FB44
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_aiAnimator.EndAnimationIf(this.teleportInAnim);
		this.m_aiAnimator.EndAnimationIf(this.teleportOutAnim);
		this.m_aiActor.ToggleRenderers(true);
		this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f);
		if (!this.m_aiActor.IsBlackPhantom)
		{
			this.m_aiActor.sprite.usesOverrideMaterial = false;
			this.m_aiActor.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUber");
		}
		SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, true);
		this.m_aiActor.ParentRoom.EndTerrifyingDarkRoom(1f, 0.1f, 1f, "Play_ENM_lighten_world_01");
		this.m_aiActor.specRigidbody.enabled = true;
		this.m_state = HighPriestMergoBehavior.State.Idle;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004AAD RID: 19117 RVA: 0x00191A44 File Offset: 0x0018FC44
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004AAE RID: 19118 RVA: 0x00191A48 File Offset: 0x0018FC48
	public override void OnActorPreDeath()
	{
		if (this.m_state == HighPriestMergoBehavior.State.Fading || this.m_state == HighPriestMergoBehavior.State.Firing)
		{
			this.m_aiActor.ParentRoom.EndTerrifyingDarkRoom(1f, 0.1f, 1f, "Play_ENM_lighten_world_01");
		}
		base.OnActorPreDeath();
	}

	// Token: 0x06004AAF RID: 19119 RVA: 0x00191A98 File Offset: 0x0018FC98
	private void ShootBulletScript()
	{
		BulletScriptSource bulletScriptSource = null;
		for (int i = 0; i < this.m_shootBulletSources.Count; i++)
		{
			if (this.m_shootBulletSources[i].IsEnded)
			{
				bulletScriptSource = this.m_shootBulletSources[i];
				break;
			}
		}
		if (bulletScriptSource == null)
		{
			bulletScriptSource = new GameObject("Mergo shoot point").AddComponent<BulletScriptSource>();
			this.m_shootBulletSources.Add(bulletScriptSource);
		}
		bulletScriptSource.transform.position = this.RandomShootPoint();
		bulletScriptSource.BulletManager = this.m_aiActor.bulletBank;
		bulletScriptSource.BulletScript = this.shootBulletScript;
		bulletScriptSource.Initialize();
	}

	// Token: 0x06004AB0 RID: 19120 RVA: 0x00191B4C File Offset: 0x0018FD4C
	private void ShootWallBulletScript()
	{
		float num;
		Vector2 vector = this.RandomWallPoint(out num);
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (!playerController || playerController.healthHaver.IsDead)
			{
				return;
			}
			if (Vector2.Distance(vector, playerController.CenterPosition) < 8f)
			{
				return;
			}
		}
		if (!this.m_wallBulletSource)
		{
			this.m_wallBulletSource = new GameObject("Mergo wall shoot point").AddComponent<BulletScriptSource>();
		}
		this.m_wallBulletSource.transform.position = vector;
		this.m_wallBulletSource.transform.rotation = Quaternion.Euler(0f, 0f, num);
		this.m_wallBulletSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_wallBulletSource.BulletScript = this.wallBulletScript;
		this.m_wallBulletSource.Initialize();
	}

	// Token: 0x06004AB1 RID: 19121 RVA: 0x00191C4C File Offset: 0x0018FE4C
	private Vector2 RandomShootPoint()
	{
		Vector2 center = this.m_aiActor.ParentRoom.area.Center;
		if (this.m_aiActor.TargetRigidbody)
		{
			this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
		}
		float num = this.fireMainDist + UnityEngine.Random.Range(-this.fireMainDistVariance, this.fireMainDistVariance);
		List<Vector2> list = new List<Vector2>();
		DungeonData data = GameManager.Instance.Dungeon.data;
		for (int i = 0; i < 36; i++)
		{
			Vector2 vector = center + BraveMathCollege.DegreesToVector((float)(i * 10), num);
			if (!data.isWall((int)vector.x, (int)vector.y) && !data.isTopWall((int)vector.x, (int)vector.y))
			{
				list.Add(vector);
			}
		}
		return BraveUtility.RandomElement<Vector2>(list);
	}

	// Token: 0x06004AB2 RID: 19122 RVA: 0x00191D38 File Offset: 0x0018FF38
	private Vector2 RandomWallPoint(out float rotation)
	{
		float num = 4f;
		CellArea area = this.m_aiActor.ParentRoom.area;
		Vector2 vector = area.basePosition.ToVector2() + new Vector2(0.5f, 1.5f);
		Vector2 vector2 = (area.basePosition + area.dimensions).ToVector2() - new Vector2(0.5f, 0.5f);
		if (BraveUtility.RandomBool())
		{
			if (BraveUtility.RandomBool())
			{
				rotation = -90f;
				return new Vector2(UnityEngine.Random.Range(vector.x + 5f, vector2.x - 5f), vector2.y + num + 2f);
			}
			rotation = 90f;
			return new Vector2(UnityEngine.Random.Range(vector.x + 5f, vector2.x - 5f), vector.y - num);
		}
		else
		{
			if (BraveUtility.RandomBool())
			{
				rotation = 0f;
				return new Vector2(vector.x - num, UnityEngine.Random.Range(vector.y + 5f, vector2.y - 5f));
			}
			rotation = 180f;
			return new Vector2(vector2.x + num, UnityEngine.Random.Range(vector.y + 5f, vector2.y - 5f));
		}
	}

	// Token: 0x04003F82 RID: 16258
	public BulletScriptSelector shootBulletScript;

	// Token: 0x04003F83 RID: 16259
	public BulletScriptSelector wallBulletScript;

	// Token: 0x04003F84 RID: 16260
	public float darknessFadeTime = 1f;

	// Token: 0x04003F85 RID: 16261
	public float fireTime = 8f;

	// Token: 0x04003F86 RID: 16262
	public float fireMainMidTime = 0.8f;

	// Token: 0x04003F87 RID: 16263
	public float fireMainDist = 16f;

	// Token: 0x04003F88 RID: 16264
	public float fireMainDistVariance = 3f;

	// Token: 0x04003F89 RID: 16265
	public float fireWallMidTime = 0.5f;

	// Token: 0x04003F8A RID: 16266
	[InspectorCategory("Visuals")]
	public string teleportOutAnim;

	// Token: 0x04003F8B RID: 16267
	[InspectorCategory("Visuals")]
	public string teleportInAnim;

	// Token: 0x04003F8C RID: 16268
	private const float c_wallBuffer = 5f;

	// Token: 0x04003F8D RID: 16269
	private HighPriestMergoBehavior.State m_state;

	// Token: 0x04003F8E RID: 16270
	private tk2dBaseSprite m_shadowSprite;

	// Token: 0x04003F8F RID: 16271
	private float m_timer;

	// Token: 0x04003F90 RID: 16272
	private float m_mainShotTimer;

	// Token: 0x04003F91 RID: 16273
	private float m_wallShotTimer;

	// Token: 0x04003F92 RID: 16274
	private List<BulletScriptSource> m_shootBulletSources = new List<BulletScriptSource>();

	// Token: 0x04003F93 RID: 16275
	private BulletScriptSource m_wallBulletSource;

	// Token: 0x02000DC1 RID: 3521
	private enum State
	{
		// Token: 0x04003F95 RID: 16277
		Idle,
		// Token: 0x04003F96 RID: 16278
		OutAnim,
		// Token: 0x04003F97 RID: 16279
		Fading,
		// Token: 0x04003F98 RID: 16280
		Firing,
		// Token: 0x04003F99 RID: 16281
		Unfading,
		// Token: 0x04003F9A RID: 16282
		InAnim
	}
}
