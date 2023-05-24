using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using Pathfinding;
using UnityEngine;

// Token: 0x02000DD2 RID: 3538
[InspectorDropdownName("Bosses/MineFlayer/ShellGameBehavior")]
public class MineFlayerShellGameBehavior : BasicAttackBehavior
{
	// Token: 0x06004AF8 RID: 19192 RVA: 0x0019489C File Offset: 0x00192A9C
	private bool ShowShadowAnimationNames()
	{
		return this.shadowSupport == MineFlayerShellGameBehavior.ShadowSupport.Animate;
	}

	// Token: 0x06004AF9 RID: 19193 RVA: 0x001948A8 File Offset: 0x00192AA8
	public override void Start()
	{
		base.Start();
		tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
	}

	// Token: 0x06004AFA RID: 19194 RVA: 0x001948DC File Offset: 0x00192ADC
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004AFB RID: 19195 RVA: 0x001948F4 File Offset: 0x00192AF4
	public override BehaviorResult Update()
	{
		base.Update();
		if (this.m_shadowSprite == null)
		{
			this.m_shadowSprite = this.m_aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		this.State = MineFlayerShellGameBehavior.ShellGameState.Disappear;
		this.m_aiActor.healthHaver.minimumHealth = 1f;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004AFC RID: 19196 RVA: 0x00194978 File Offset: 0x00192B78
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.State == MineFlayerShellGameBehavior.ShellGameState.Disappear)
		{
			if (this.shadowSupport == MineFlayerShellGameBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f - this.m_aiAnimator.CurrentClipProgress);
			}
			if (!this.m_aiAnimator.IsPlaying(this.disappearAnim))
			{
				this.State = ((this.MaxGoneTime <= 0f) ? MineFlayerShellGameBehavior.ShellGameState.Reappear : MineFlayerShellGameBehavior.ShellGameState.Gone);
			}
		}
		else if (this.State == MineFlayerShellGameBehavior.ShellGameState.Gone)
		{
			if (this.m_timer <= 0f)
			{
				this.State = MineFlayerShellGameBehavior.ShellGameState.Reappear;
			}
		}
		else if (this.State == MineFlayerShellGameBehavior.ShellGameState.Reappear)
		{
			if (this.shadowSupport == MineFlayerShellGameBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(this.m_aiAnimator.CurrentClipProgress);
			}
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "MeduziUnderwaterBehavior");
			}
			Vector2? reappearPosition = this.m_reappearPosition;
			if (reappearPosition != null)
			{
				this.m_aiActor.specRigidbody.CollideWithTileMap = false;
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(this.m_aiActor.specRigidbody, null, false);
				Vector2 vector = this.m_reappearPosition.Value - this.m_aiActor.specRigidbody.UnitBottomLeft;
				this.m_aiActor.BehaviorOverridesVelocity = true;
				this.m_aiActor.BehaviorVelocity = vector / (this.m_aiAnimator.CurrentClipLength * (1f - this.m_aiAnimator.CurrentClipProgress));
			}
			if (!this.m_aiAnimator.IsPlaying(this.reappearAnim))
			{
				this.State = MineFlayerShellGameBehavior.ShellGameState.None;
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004AFD RID: 19197 RVA: 0x00194B50 File Offset: 0x00192D50
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_aiActor.healthHaver.minimumHealth = 0f;
		if (this.requiresTransparency && this.m_cachedShader)
		{
			this.m_aiActor.sprite.usesOverrideMaterial = false;
			this.m_aiActor.renderer.material.shader = this.m_cachedShader;
			this.m_cachedShader = null;
		}
		this.m_aiActor.sprite.renderer.enabled = true;
		if (this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(false, "teleport");
		}
		this.m_aiActor.specRigidbody.CollideWithOthers = true;
		this.m_aiActor.IsGone = false;
		if (this.m_aiShooter)
		{
			this.m_aiShooter.ToggleGunAndHandRenderers(true, "MeduziUnderwaterBehavior");
		}
		this.m_aiAnimator.EndAnimationIf(this.disappearAnim);
		this.m_aiAnimator.EndAnimationIf(this.reappearAnim);
		if (this.shadowSupport == MineFlayerShellGameBehavior.ShadowSupport.Fade)
		{
			this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f);
		}
		else if (this.shadowSupport == MineFlayerShellGameBehavior.ShadowSupport.Animate)
		{
			tk2dSpriteAnimationClip clipByName = this.m_shadowSprite.spriteAnimator.GetClipByName(this.shadowReappearAnim);
			this.m_shadowSprite.spriteAnimator.Play(clipByName, (float)(clipByName.frames.Length - 1), clipByName.fps, false);
		}
		this.m_spawnedActors.Clear();
		this.m_correctBellHit = false;
		SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, true);
		Vector2? reappearPosition = this.m_reappearPosition;
		if (reappearPosition != null)
		{
			this.m_aiActor.specRigidbody.CollideWithTileMap = true;
			this.m_aiActor.transform.position += this.m_reappearPosition.Value - this.m_aiActor.specRigidbody.UnitBottomLeft;
			this.m_aiActor.specRigidbody.Reinitialize();
			this.m_aiActor.BehaviorOverridesVelocity = false;
			this.m_reappearPosition = null;
		}
		this.m_state = MineFlayerShellGameBehavior.ShellGameState.None;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004AFE RID: 19198 RVA: 0x00194DA8 File Offset: 0x00192FA8
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004AFF RID: 19199 RVA: 0x00194DAC File Offset: 0x00192FAC
	public void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.m_shouldFire && clip.GetFrame(frame).eventInfo == "fire")
		{
			if (this.State == MineFlayerShellGameBehavior.ShellGameState.Reappear)
			{
				SpawnManager.SpawnBulletScript(this.m_aiActor, this.reappearInBulletScript, null, null, false, null);
			}
			else if (this.State == MineFlayerShellGameBehavior.ShellGameState.Disappear)
			{
				SpawnManager.SpawnBulletScript(this.m_aiActor, this.disappearBulletScript, null, null, false, null);
			}
			this.m_shouldFire = false;
		}
		else if (this.State == MineFlayerShellGameBehavior.ShellGameState.Disappear && clip.GetFrame(frame).eventInfo == "collider_off")
		{
			this.m_aiActor.specRigidbody.CollideWithOthers = false;
			this.m_aiActor.IsGone = true;
		}
		else if (this.State == MineFlayerShellGameBehavior.ShellGameState.Reappear && clip.GetFrame(frame).eventInfo == "collider_on")
		{
			this.m_aiActor.specRigidbody.CollideWithOthers = true;
			this.m_aiActor.IsGone = false;
		}
	}

	// Token: 0x06004B00 RID: 19200 RVA: 0x00194EE0 File Offset: 0x001930E0
	private void OnMyBellDeath(Vector2 obj)
	{
		if (this.State != MineFlayerShellGameBehavior.ShellGameState.Reappear)
		{
			this.m_correctBellHit = true;
			this.State = MineFlayerShellGameBehavior.ShellGameState.Reappear;
		}
	}

	// Token: 0x17000A95 RID: 2709
	// (get) Token: 0x06004B01 RID: 19201 RVA: 0x00194EFC File Offset: 0x001930FC
	// (set) Token: 0x06004B02 RID: 19202 RVA: 0x00194F04 File Offset: 0x00193104
	private MineFlayerShellGameBehavior.ShellGameState State
	{
		get
		{
			return this.m_state;
		}
		set
		{
			this.EndState(this.m_state);
			this.m_state = value;
			this.BeginState(this.m_state);
		}
	}

	// Token: 0x06004B03 RID: 19203 RVA: 0x00194F28 File Offset: 0x00193128
	private void BeginState(MineFlayerShellGameBehavior.ShellGameState state)
	{
		if (state == MineFlayerShellGameBehavior.ShellGameState.Disappear)
		{
			if (this.disappearBulletScript != null && !this.disappearBulletScript.IsNull)
			{
				this.m_shouldFire = true;
			}
			if (this.requiresTransparency)
			{
				this.m_cachedShader = this.m_aiActor.renderer.material.shader;
				this.m_aiActor.sprite.usesOverrideMaterial = true;
				this.m_aiActor.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
			}
			this.m_aiAnimator.PlayUntilCancelled(this.disappearAnim, true, null, -1f, false);
			if (this.shadowSupport == MineFlayerShellGameBehavior.ShadowSupport.Animate)
			{
				this.m_shadowSprite.spriteAnimator.PlayAndForceTime(this.shadowDisappearAnim, this.m_aiAnimator.CurrentClipLength);
			}
			this.m_aiActor.ClearPath();
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "MeduziUnderwaterBehavior");
			}
		}
		else if (state == MineFlayerShellGameBehavior.ShellGameState.Gone)
		{
			this.m_timer = this.MaxGoneTime;
			this.m_aiActor.specRigidbody.CollideWithOthers = false;
			this.m_aiActor.IsGone = true;
			this.m_aiActor.sprite.renderer.enabled = false;
			Vector2 vector = this.m_aiActor.specRigidbody.UnitCenter + Vector2.right;
			for (int i = 0; i < this.enemiesToSpawn; i++)
			{
				AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.enemyGuid);
				AIActor aiactor = AIActor.Spawn(orLoadByGuid, vector, this.m_aiActor.ParentRoom, true, AIActor.AwakenAnimationType.Default, true);
				this.m_spawnedActors.Add(aiactor);
			}
			this.m_myBell = BraveUtility.RandomElement<AIActor>(this.m_spawnedActors);
			this.m_myBell.healthHaver.OnPreDeath += this.OnMyBellDeath;
			this.m_myBell.OnCorpseVFX.type = VFXPoolType.None;
			this.m_myBell.healthHaver.spawnBulletScript = false;
		}
		else if (state == MineFlayerShellGameBehavior.ShellGameState.Reappear)
		{
			if (this.m_myBell)
			{
				this.m_aiActor.specRigidbody.AlignWithRigidbodyBottomCenter(this.m_myBell.specRigidbody, new IntVector2?(new IntVector2(-6, -2)));
				if (PhysicsEngine.Instance.OverlapCast(this.m_aiActor.specRigidbody, null, true, true, null, null, false, null, null, new SpeculativeRigidbody[0]))
				{
					this.DoReposition();
				}
				else
				{
					this.m_reappearPosition = null;
				}
			}
			for (int j = 0; j < this.m_spawnedActors.Count; j++)
			{
				AIActor aiactor2 = this.m_spawnedActors[j];
				if (aiactor2 && aiactor2.healthHaver && aiactor2.healthHaver.IsAlive)
				{
					if (this.m_correctBellHit)
					{
						aiactor2.healthHaver.spawnBulletScript = false;
					}
					aiactor2.healthHaver.ApplyDamage(1E+10f, Vector2.zero, "Bell Death", CoreDamageTypes.None, DamageCategory.Unstoppable, false, null, false);
				}
			}
			if (this.reappearInBulletScript != null && !this.reappearInBulletScript.IsNull)
			{
				this.m_shouldFire = true;
			}
			this.m_aiAnimator.PlayUntilFinished(this.reappearAnim, true, null, -1f, false);
			if (this.shadowSupport == MineFlayerShellGameBehavior.ShadowSupport.Animate)
			{
				this.m_shadowSprite.spriteAnimator.PlayAndForceTime(this.shadowReappearAnim, this.m_aiAnimator.CurrentClipLength);
			}
			this.m_shadowSprite.renderer.enabled = true;
			this.m_aiActor.sprite.renderer.enabled = true;
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "MeduziUnderwaterBehavior");
			}
			SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, true);
		}
	}

	// Token: 0x06004B04 RID: 19204 RVA: 0x0019531C File Offset: 0x0019351C
	private void EndState(MineFlayerShellGameBehavior.ShellGameState state)
	{
		if (state == MineFlayerShellGameBehavior.ShellGameState.Disappear)
		{
			this.m_shadowSprite.renderer.enabled = false;
			SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, false);
			if (this.disappearBulletScript != null && !this.disappearBulletScript.IsNull && this.m_shouldFire)
			{
				SpawnManager.SpawnBulletScript(this.m_aiActor, this.disappearBulletScript, null, null, false, null);
				this.m_shouldFire = false;
			}
		}
		else if (state == MineFlayerShellGameBehavior.ShellGameState.Gone)
		{
			this.m_aiActor.BehaviorOverridesVelocity = false;
		}
		else if (state == MineFlayerShellGameBehavior.ShellGameState.Reappear)
		{
			if (this.requiresTransparency)
			{
				this.m_aiActor.sprite.usesOverrideMaterial = false;
				this.m_aiActor.renderer.material.shader = this.m_cachedShader;
			}
			if (this.shadowSupport == MineFlayerShellGameBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f);
			}
			this.m_aiActor.specRigidbody.CollideWithOthers = true;
			this.m_aiActor.IsGone = false;
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(true, "MeduziUnderwaterBehavior");
			}
			if (this.reappearInBulletScript != null && !this.reappearInBulletScript.IsNull && this.m_shouldFire)
			{
				SpawnManager.SpawnBulletScript(this.m_aiActor, this.reappearInBulletScript, null, null, false, null);
				this.m_shouldFire = false;
			}
			Vector2? reappearPosition = this.m_reappearPosition;
			if (reappearPosition != null)
			{
				this.m_aiActor.specRigidbody.CollideWithTileMap = true;
				this.m_aiActor.transform.position += this.m_reappearPosition.Value - this.m_aiActor.specRigidbody.UnitBottomLeft;
				this.m_aiActor.specRigidbody.Reinitialize();
				this.m_aiActor.BehaviorOverridesVelocity = false;
				this.m_reappearPosition = null;
			}
		}
	}

	// Token: 0x06004B05 RID: 19205 RVA: 0x0019554C File Offset: 0x0019374C
	private void DoReposition()
	{
		CellValidator cellValidator = delegate(IntVector2 c)
		{
			for (int i = 0; i < this.m_aiActor.Clearance.x; i++)
			{
				for (int j = 0; j < this.m_aiActor.Clearance.y; j++)
				{
					if (GameManager.Instance.Dungeon.data.isWall(c.x + i, c.y + j))
					{
						return false;
					}
					if (GameManager.Instance.Dungeon.data.isTopWall(c.x + i, c.y + j))
					{
						return false;
					}
				}
			}
			return true;
		};
		IntVector2? nearestAvailableCell = this.m_aiActor.ParentRoom.GetNearestAvailableCell(this.m_aiActor.specRigidbody.UnitBottomLeft, new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(this.m_aiActor.PathableTiles), false, cellValidator);
		if (nearestAvailableCell != null)
		{
			Vector2 vector = Pathfinder.GetClearanceOffset(nearestAvailableCell.Value, this.m_aiActor.Clearance).WithY((float)nearestAvailableCell.Value.y);
			vector -= new Vector2(this.m_aiActor.specRigidbody.UnitDimensions.x / 2f, 0f);
			this.m_reappearPosition = new Vector2?(vector);
		}
		else
		{
			this.m_reappearPosition = null;
		}
	}

	// Token: 0x04004025 RID: 16421
	public float MaxGoneTime = 5f;

	// Token: 0x04004026 RID: 16422
	[InspectorCategory("Attack")]
	public BulletScriptSelector disappearBulletScript;

	// Token: 0x04004027 RID: 16423
	[InspectorCategory("Attack")]
	public BulletScriptSelector reappearInBulletScript;

	// Token: 0x04004028 RID: 16424
	[InspectorCategory("Visuals")]
	public string disappearAnim = "teleport_out";

	// Token: 0x04004029 RID: 16425
	[InspectorCategory("Visuals")]
	public string reappearAnim = "teleport_in";

	// Token: 0x0400402A RID: 16426
	[InspectorCategory("Visuals")]
	public bool requiresTransparency;

	// Token: 0x0400402B RID: 16427
	[InspectorCategory("Visuals")]
	public MineFlayerShellGameBehavior.ShadowSupport shadowSupport;

	// Token: 0x0400402C RID: 16428
	[InspectorCategory("Visuals")]
	[InspectorShowIf("ShowShadowAnimationNames")]
	public string shadowDisappearAnim;

	// Token: 0x0400402D RID: 16429
	[InspectorShowIf("ShowShadowAnimationNames")]
	[InspectorCategory("Visuals")]
	public string shadowReappearAnim;

	// Token: 0x0400402E RID: 16430
	public int enemiesToSpawn;

	// Token: 0x0400402F RID: 16431
	[EnemyIdentifier]
	public string enemyGuid;

	// Token: 0x04004030 RID: 16432
	private tk2dBaseSprite m_shadowSprite;

	// Token: 0x04004031 RID: 16433
	private Shader m_cachedShader;

	// Token: 0x04004032 RID: 16434
	private float m_timer;

	// Token: 0x04004033 RID: 16435
	private bool m_shouldFire;

	// Token: 0x04004034 RID: 16436
	private List<AIActor> m_spawnedActors = new List<AIActor>();

	// Token: 0x04004035 RID: 16437
	private AIActor m_myBell;

	// Token: 0x04004036 RID: 16438
	private bool m_correctBellHit;

	// Token: 0x04004037 RID: 16439
	private Vector2? m_reappearPosition;

	// Token: 0x04004038 RID: 16440
	private MineFlayerShellGameBehavior.ShellGameState m_state;

	// Token: 0x02000DD3 RID: 3539
	public enum ShadowSupport
	{
		// Token: 0x0400403A RID: 16442
		None,
		// Token: 0x0400403B RID: 16443
		Fade,
		// Token: 0x0400403C RID: 16444
		Animate
	}

	// Token: 0x02000DD4 RID: 3540
	private enum ShellGameState
	{
		// Token: 0x0400403E RID: 16446
		None,
		// Token: 0x0400403F RID: 16447
		Disappear,
		// Token: 0x04004040 RID: 16448
		Gone,
		// Token: 0x04004041 RID: 16449
		Reappear
	}
}
