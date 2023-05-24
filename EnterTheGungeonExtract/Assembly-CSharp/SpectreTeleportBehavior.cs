using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using Pathfinding;
using UnityEngine;

// Token: 0x02000D56 RID: 3414
public class SpectreTeleportBehavior : BasicAttackBehavior
{
	// Token: 0x06004821 RID: 18465 RVA: 0x0017CCD0 File Offset: 0x0017AED0
	private bool ShowShadowAnimationNames()
	{
		return this.shadowSupport == SpectreTeleportBehavior.ShadowSupport.Animate;
	}

	// Token: 0x06004822 RID: 18466 RVA: 0x0017CCDC File Offset: 0x0017AEDC
	public override void Start()
	{
		base.Start();
		PhysicsEngine.Instance.OnPostRigidbodyMovement += this.OnPostRigidbodyMovement;
		for (int i = 0; i < this.HauntCopies.Count; i++)
		{
			this.HauntCopies[i].aiActor = this.m_aiActor;
			this.HauntCopies[i].healthHaver = this.m_aiActor.healthHaver;
		}
	}

	// Token: 0x06004823 RID: 18467 RVA: 0x0017CD54 File Offset: 0x0017AF54
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004824 RID: 18468 RVA: 0x0017CD6C File Offset: 0x0017AF6C
	public override BehaviorResult Update()
	{
		base.Update();
		if (this.m_allSpectres == null)
		{
			this.m_allSpectres = new List<SpectreTeleportBehavior.SpecterInfo>(this.HauntCopies.Count + 1);
			tk2dBaseSprite component = this.m_aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
			this.m_allSpectres.Add(new SpectreTeleportBehavior.SpecterInfo
			{
				aiAnimator = this.m_aiAnimator,
				shadowSprite = component
			});
			for (int i = 0; i < this.HauntCopies.Count; i++)
			{
				tk2dBaseSprite tk2dBaseSprite = UnityEngine.Object.Instantiate<tk2dBaseSprite>(component);
				tk2dBaseSprite.transform.parent = this.HauntCopies[i].transform;
				tk2dBaseSprite.transform.localPosition = component.transform.localPosition;
				this.HauntCopies[i].sprite.AttachRenderer(tk2dBaseSprite);
				tk2dBaseSprite.UpdateZDepth();
				this.m_allSpectres.Add(new SpectreTeleportBehavior.SpecterInfo
				{
					aiAnimator = this.HauntCopies[i],
					shadowSprite = tk2dBaseSprite
				});
			}
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		this.State = SpectreTeleportBehavior.TeleportState.TeleportOut;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004825 RID: 18469 RVA: 0x0017CEAC File Offset: 0x0017B0AC
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.State == SpectreTeleportBehavior.TeleportState.TeleportOut)
		{
			if (this.shadowSupport == SpectreTeleportBehavior.ShadowSupport.Fade)
			{
				for (int i = 0; i < this.m_allSpectres.Count; i++)
				{
					if (this.m_allSpectres[i].shadowSprite)
					{
						this.m_allSpectres[i].shadowSprite.color = this.m_allSpectres[i].shadowSprite.color.WithAlpha(1f - this.m_aiAnimator.CurrentClipProgress);
					}
				}
			}
			if (!this.m_aiAnimator.IsPlaying(this.teleportOutAnim))
			{
				this.State = ((this.GoneTime <= 0f) ? SpectreTeleportBehavior.TeleportState.HauntIn : SpectreTeleportBehavior.TeleportState.Gone1);
			}
		}
		else if (this.State == SpectreTeleportBehavior.TeleportState.Gone1)
		{
			if (this.m_timer <= 0f)
			{
				this.State = SpectreTeleportBehavior.TeleportState.HauntIn;
			}
		}
		else if (this.State == SpectreTeleportBehavior.TeleportState.HauntIn)
		{
			if (this.shadowSupport == SpectreTeleportBehavior.ShadowSupport.Fade)
			{
				for (int j = 0; j < this.m_allSpectres.Count; j++)
				{
					if (this.m_allSpectres[j].shadowSprite)
					{
						this.m_allSpectres[j].shadowSprite.color = this.m_allSpectres[j].shadowSprite.color.WithAlpha(this.m_aiAnimator.CurrentClipProgress);
					}
				}
			}
			this.m_aiShooter.ToggleGunAndHandRenderers(false, "SpectreTeleportBehavior");
			if (!this.m_aiAnimator.IsPlaying(this.teleportInAttackAnim))
			{
				this.State = SpectreTeleportBehavior.TeleportState.Haunt;
			}
		}
		else if (this.State == SpectreTeleportBehavior.TeleportState.Haunt)
		{
			if (this.m_timer <= 0f)
			{
				this.State = SpectreTeleportBehavior.TeleportState.HauntOut;
			}
		}
		else if (this.State == SpectreTeleportBehavior.TeleportState.HauntOut)
		{
			if (this.shadowSupport == SpectreTeleportBehavior.ShadowSupport.Fade)
			{
				for (int k = 0; k < this.m_allSpectres.Count; k++)
				{
					if (this.m_allSpectres[k].shadowSprite)
					{
						this.m_allSpectres[k].shadowSprite.color = this.m_allSpectres[k].shadowSprite.color.WithAlpha(1f - this.m_aiAnimator.CurrentClipProgress);
					}
				}
			}
			if (!this.m_aiAnimator.IsPlaying(this.teleportOutAttackAnim))
			{
				this.State = ((this.GoneTime <= 0f) ? SpectreTeleportBehavior.TeleportState.TeleportIn : SpectreTeleportBehavior.TeleportState.Gone2);
			}
		}
		else if (this.State == SpectreTeleportBehavior.TeleportState.Gone2)
		{
			if (this.m_timer <= 0f)
			{
				this.State = SpectreTeleportBehavior.TeleportState.TeleportIn;
			}
		}
		else if (this.State == SpectreTeleportBehavior.TeleportState.TeleportIn)
		{
			if (this.shadowSupport == SpectreTeleportBehavior.ShadowSupport.Fade)
			{
				for (int l = 0; l < this.m_allSpectres.Count; l++)
				{
					if (this.m_allSpectres[l].shadowSprite)
					{
						this.m_allSpectres[l].shadowSprite.color = this.m_allSpectres[l].shadowSprite.color.WithAlpha(this.m_aiAnimator.CurrentClipProgress);
					}
				}
			}
			this.m_aiShooter.ToggleGunAndHandRenderers(false, "SpectreTeleportBehavior");
			if (!this.m_aiAnimator.IsPlaying(this.teleportInAnim))
			{
				this.State = SpectreTeleportBehavior.TeleportState.None;
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004826 RID: 18470 RVA: 0x0017D244 File Offset: 0x0017B444
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004827 RID: 18471 RVA: 0x0017D25C File Offset: 0x0017B45C
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004828 RID: 18472 RVA: 0x0017D260 File Offset: 0x0017B460
	public override void OnActorPreDeath()
	{
		for (int i = 0; i < this.HauntCopies.Count; i++)
		{
			this.HauntCopies[i].PlayUntilCancelled("die", true, null, -1f, false);
		}
		PhysicsEngine.Instance.OnPostRigidbodyMovement -= this.OnPostRigidbodyMovement;
		base.OnActorPreDeath();
	}

	// Token: 0x06004829 RID: 18473 RVA: 0x0017D2C4 File Offset: 0x0017B4C4
	public void OnPostRigidbodyMovement()
	{
		if (this.State == SpectreTeleportBehavior.TeleportState.HauntIn || this.State == SpectreTeleportBehavior.TeleportState.Haunt || this.State == SpectreTeleportBehavior.TeleportState.HauntOut)
		{
			Vector2 unitCenter = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
			float num = 360f / (float)this.m_allSpectres.Count;
			for (int i = 0; i < this.m_allSpectres.Count; i++)
			{
				Vector2 vector = BraveMathCollege.DegreesToVector(this.m_hauntAngle + (float)i * num, this.HauntDistance);
				this.m_allSpectres[i].transform.position = unitCenter + vector + this.m_centerOffset;
				this.m_allSpectres[i].specRigidbody.Reinitialize();
			}
		}
	}

	// Token: 0x17000A6D RID: 2669
	// (get) Token: 0x0600482A RID: 18474 RVA: 0x0017D390 File Offset: 0x0017B590
	// (set) Token: 0x0600482B RID: 18475 RVA: 0x0017D398 File Offset: 0x0017B598
	private SpectreTeleportBehavior.TeleportState State
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

	// Token: 0x0600482C RID: 18476 RVA: 0x0017D3BC File Offset: 0x0017B5BC
	private void BeginState(SpectreTeleportBehavior.TeleportState state)
	{
		if (state == SpectreTeleportBehavior.TeleportState.TeleportOut || state == SpectreTeleportBehavior.TeleportState.HauntOut)
		{
			if (this.teleportRequiresTransparency)
			{
				this.m_cachedShader = this.m_aiActor.renderer.material.shader;
			}
			for (int i = 0; i < this.m_allSpectres.Count; i++)
			{
				if (this.teleportRequiresTransparency)
				{
					this.m_allSpectres[i].sprite.usesOverrideMaterial = true;
					this.m_allSpectres[i].renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
				}
				string text = ((state != SpectreTeleportBehavior.TeleportState.TeleportOut) ? this.teleportOutAttackAnim : this.teleportOutAnim);
				this.m_allSpectres[i].aiAnimator.PlayUntilCancelled(text, true, null, -1f, false);
				if (this.shadowSupport == SpectreTeleportBehavior.ShadowSupport.Animate && this.m_allSpectres[i].shadowSprite)
				{
					this.m_allSpectres[i].shadowSprite.spriteAnimator.PlayAndForceTime(this.shadowOutAnim, this.m_aiAnimator.CurrentClipLength);
				}
				if (!this.AttackableDuringAnimation)
				{
					this.m_allSpectres[i].specRigidbody.CollideWithOthers = false;
				}
			}
			this.m_aiShooter.ToggleGunAndHandRenderers(false, "SpectreTeleportBehavior");
			this.m_aiActor.ClearPath();
		}
		else if (state == SpectreTeleportBehavior.TeleportState.Gone1 || state == SpectreTeleportBehavior.TeleportState.Gone2)
		{
			this.m_timer = this.GoneTime;
			for (int j = 0; j < this.m_allSpectres.Count; j++)
			{
				this.m_allSpectres[j].specRigidbody.CollideWithOthers = false;
				this.m_allSpectres[j].sprite.renderer.enabled = false;
			}
		}
		else if (state == SpectreTeleportBehavior.TeleportState.Haunt)
		{
			this.Fire();
			this.m_timer = this.HauntTime;
			for (int k = 0; k < this.m_allSpectres.Count; k++)
			{
				this.m_allSpectres[k].specRigidbody.CollideWithOthers = true;
				this.m_allSpectres[k].specRigidbody.CollideWithTileMap = false;
				this.m_allSpectres[k].aiAnimator.LockFacingDirection = true;
				this.m_allSpectres[k].aiAnimator.FacingDirection = -90f;
			}
		}
		else if (state == SpectreTeleportBehavior.TeleportState.TeleportIn || state == SpectreTeleportBehavior.TeleportState.HauntIn)
		{
			if (state == SpectreTeleportBehavior.TeleportState.TeleportIn)
			{
				this.DoTeleport();
				this.m_aiAnimator.PlayUntilFinished(this.teleportInAnim, true, null, -1f, false);
			}
			else
			{
				Vector2 unitCenter = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
				this.m_hauntAngle = (float)UnityEngine.Random.Range(0, 360);
				this.m_centerOffset = this.m_aiActor.transform.position.XY() - this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox);
				float num = 360f / (float)this.m_allSpectres.Count;
				for (int l = 0; l < this.m_allSpectres.Count; l++)
				{
					Vector2 vector = BraveMathCollege.DegreesToVector(this.m_hauntAngle + (float)l * num, this.HauntDistance);
					if (l > 0)
					{
						this.m_allSpectres[l].gameObject.SetActive(true);
						this.m_allSpectres[l].specRigidbody.enabled = true;
					}
					this.m_allSpectres[l].transform.position = unitCenter + vector + this.m_centerOffset;
					this.m_allSpectres[l].specRigidbody.Reinitialize();
					this.m_allSpectres[l].aiAnimator.PlayUntilFinished(this.teleportInAttackAnim, true, null, -1f, false);
				}
			}
			for (int m = 0; m < this.m_allSpectres.Count; m++)
			{
				if (this.m_allSpectres[m].shadowSprite)
				{
					if (this.shadowSupport == SpectreTeleportBehavior.ShadowSupport.Animate)
					{
						this.m_allSpectres[m].shadowSprite.spriteAnimator.PlayAndForceTime(this.shadowInAnim, this.m_aiAnimator.CurrentClipLength);
					}
					this.m_allSpectres[m].shadowSprite.renderer.enabled = true;
				}
				if (this.AttackableDuringAnimation)
				{
					this.m_allSpectres[m].specRigidbody.CollideWithOthers = true;
				}
				this.m_allSpectres[m].sprite.renderer.enabled = true;
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_allSpectres[m].sprite, true);
			}
		}
	}

	// Token: 0x0600482D RID: 18477 RVA: 0x0017D8A4 File Offset: 0x0017BAA4
	private void EndState(SpectreTeleportBehavior.TeleportState state)
	{
		if (state == SpectreTeleportBehavior.TeleportState.TeleportOut || state == SpectreTeleportBehavior.TeleportState.HauntOut)
		{
			for (int i = 0; i < this.m_allSpectres.Count; i++)
			{
				if (this.m_allSpectres[i].shadowSprite)
				{
					this.m_allSpectres[i].renderer.enabled = false;
				}
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_allSpectres[i].sprite, false);
			}
			if (state == SpectreTeleportBehavior.TeleportState.HauntOut)
			{
				for (int j = 1; j < this.m_allSpectres.Count; j++)
				{
					this.m_allSpectres[j].gameObject.SetActive(false);
					this.m_allSpectres[j].specRigidbody.enabled = false;
				}
			}
		}
		else if (state == SpectreTeleportBehavior.TeleportState.TeleportIn || state == SpectreTeleportBehavior.TeleportState.HauntIn)
		{
			for (int k = 0; k < this.m_allSpectres.Count; k++)
			{
				if (this.teleportRequiresTransparency)
				{
					this.m_allSpectres[k].sprite.usesOverrideMaterial = false;
					this.m_allSpectres[k].renderer.material.shader = this.m_cachedShader;
				}
				if (this.shadowSupport == SpectreTeleportBehavior.ShadowSupport.Fade && this.m_allSpectres[k].shadowSprite)
				{
					this.m_allSpectres[k].shadowSprite.color = this.m_allSpectres[k].shadowSprite.color.WithAlpha(1f);
				}
				this.m_allSpectres[k].specRigidbody.CollideWithOthers = true;
			}
			if (state == SpectreTeleportBehavior.TeleportState.TeleportIn)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(true, "SpectreTeleportBehavior");
			}
		}
		else if (state == SpectreTeleportBehavior.TeleportState.Haunt)
		{
			for (int l = 0; l < this.m_allSpectres.Count; l++)
			{
				this.m_allSpectres[l].specRigidbody.CollideWithTileMap = true;
				this.m_allSpectres[l].aiAnimator.LockFacingDirection = false;
			}
		}
	}

	// Token: 0x0600482E RID: 18478 RVA: 0x0017DACC File Offset: 0x0017BCCC
	private void Fire()
	{
		if (!this.m_bulletSource)
		{
			this.m_bulletSource = this.ShootPoint.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_bulletSource.BulletScript = this.hauntBulletScript;
		this.m_bulletSource.Initialize();
	}

	// Token: 0x0600482F RID: 18479 RVA: 0x0017DB2C File Offset: 0x0017BD2C
	private void DoTeleport()
	{
		IntVector2? targetCenter = null;
		if (this.m_aiActor.TargetRigidbody)
		{
			targetCenter = new IntVector2?(this.m_aiActor.TargetRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
		}
		Vector2 vector = BraveUtility.ViewportToWorldpoint(new Vector2(0f, 0f), ViewportType.Gameplay);
		Vector2 vector2 = BraveUtility.ViewportToWorldpoint(new Vector2(1f, 1f), ViewportType.Gameplay);
		IntVector2 bottomLeft = vector.ToIntVector2(VectorConversions.Ceil);
		IntVector2 topRight = vector2.ToIntVector2(VectorConversions.Floor) - IntVector2.One;
		CellValidator cellValidator = delegate(IntVector2 c)
		{
			for (int i = 0; i < this.m_aiActor.Clearance.x; i++)
			{
				for (int j = 0; j < this.m_aiActor.Clearance.y; j++)
				{
					if (GameManager.Instance.Dungeon.data.isTopWall(c.x + i, c.y + j))
					{
						return false;
					}
					if (this.State == SpectreTeleportBehavior.TeleportState.TeleportIn && targetCenter != null && IntVector2.DistanceSquared(targetCenter.Value, c.x + i, c.y + j) < 16f)
					{
						return false;
					}
					if (this.State == SpectreTeleportBehavior.TeleportState.HauntIn && targetCenter != null && IntVector2.DistanceSquared(targetCenter.Value, c.x + i, c.y + j) > 4f)
					{
						return false;
					}
				}
			}
			if (c.x < bottomLeft.x || c.y < bottomLeft.y || c.x + this.m_aiActor.Clearance.x - 1 > topRight.x || c.y + this.m_aiActor.Clearance.y - 1 > topRight.y)
			{
				return false;
			}
			if (this.AvoidWalls)
			{
				int k = -1;
				int l;
				for (l = -1; l < this.m_aiActor.Clearance.y + 1; l++)
				{
					if (GameManager.Instance.Dungeon.data.isWall(c.x + k, c.y + l))
					{
						return false;
					}
				}
				k = this.m_aiActor.Clearance.x;
				for (l = -1; l < this.m_aiActor.Clearance.y + 1; l++)
				{
					if (GameManager.Instance.Dungeon.data.isWall(c.x + k, c.y + l))
					{
						return false;
					}
				}
				l = -1;
				for (k = -1; k < this.m_aiActor.Clearance.x + 1; k++)
				{
					if (GameManager.Instance.Dungeon.data.isWall(c.x + k, c.y + l))
					{
						return false;
					}
				}
				l = this.m_aiActor.Clearance.y;
				for (k = -1; k < this.m_aiActor.Clearance.x + 1; k++)
				{
					if (GameManager.Instance.Dungeon.data.isWall(c.x + k, c.y + l))
					{
						return false;
					}
				}
			}
			return true;
		};
		Vector2 vector3 = this.m_aiActor.specRigidbody.UnitCenter - this.m_aiActor.transform.position.XY();
		IntVector2? randomAvailableCell = this.m_aiActor.ParentRoom.GetRandomAvailableCell(new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(this.m_aiActor.PathableTiles), false, cellValidator);
		if (randomAvailableCell != null)
		{
			this.m_aiActor.transform.position = Pathfinder.GetClearanceOffset(randomAvailableCell.Value, this.m_aiActor.Clearance) - vector3;
			this.m_aiActor.specRigidbody.Reinitialize();
		}
		else
		{
			Debug.LogWarning("TELEPORT FAILED!", this.m_aiActor);
		}
	}

	// Token: 0x04003BA3 RID: 15267
	public bool AttackableDuringAnimation;

	// Token: 0x04003BA4 RID: 15268
	public bool AvoidWalls;

	// Token: 0x04003BA5 RID: 15269
	public float GoneTime = 1f;

	// Token: 0x04003BA6 RID: 15270
	public float HauntTime = 1f;

	// Token: 0x04003BA7 RID: 15271
	public float HauntDistance = 5f;

	// Token: 0x04003BA8 RID: 15272
	public List<AIAnimator> HauntCopies;

	// Token: 0x04003BA9 RID: 15273
	[InspectorCategory("Attack")]
	public GameObject ShootPoint;

	// Token: 0x04003BAA RID: 15274
	[InspectorCategory("Attack")]
	public BulletScriptSelector hauntBulletScript;

	// Token: 0x04003BAB RID: 15275
	[InspectorCategory("Visuals")]
	public string teleportOutAnim = "teleport_out";

	// Token: 0x04003BAC RID: 15276
	[InspectorCategory("Visuals")]
	public string teleportInAttackAnim = "teleport_in";

	// Token: 0x04003BAD RID: 15277
	[InspectorCategory("Visuals")]
	public string teleportOutAttackAnim = "teleport_out";

	// Token: 0x04003BAE RID: 15278
	[InspectorCategory("Visuals")]
	public string teleportInAnim = "teleport_in";

	// Token: 0x04003BAF RID: 15279
	[InspectorCategory("Visuals")]
	public bool teleportRequiresTransparency;

	// Token: 0x04003BB0 RID: 15280
	[InspectorCategory("Visuals")]
	public SpectreTeleportBehavior.ShadowSupport shadowSupport;

	// Token: 0x04003BB1 RID: 15281
	[InspectorCategory("Visuals")]
	[InspectorShowIf("ShowShadowAnimationNames")]
	public string shadowOutAnim;

	// Token: 0x04003BB2 RID: 15282
	[InspectorCategory("Visuals")]
	[InspectorShowIf("ShowShadowAnimationNames")]
	public string shadowInAnim;

	// Token: 0x04003BB3 RID: 15283
	private Shader m_cachedShader;

	// Token: 0x04003BB4 RID: 15284
	private List<SpectreTeleportBehavior.SpecterInfo> m_allSpectres;

	// Token: 0x04003BB5 RID: 15285
	private BulletScriptSource m_bulletSource;

	// Token: 0x04003BB6 RID: 15286
	private float m_timer;

	// Token: 0x04003BB7 RID: 15287
	private float m_hauntAngle;

	// Token: 0x04003BB8 RID: 15288
	private Vector2 m_centerOffset;

	// Token: 0x04003BB9 RID: 15289
	private SpectreTeleportBehavior.TeleportState m_state;

	// Token: 0x02000D57 RID: 3415
	public enum ShadowSupport
	{
		// Token: 0x04003BBB RID: 15291
		None,
		// Token: 0x04003BBC RID: 15292
		Fade,
		// Token: 0x04003BBD RID: 15293
		Animate
	}

	// Token: 0x02000D58 RID: 3416
	private enum TeleportState
	{
		// Token: 0x04003BBF RID: 15295
		None,
		// Token: 0x04003BC0 RID: 15296
		TeleportOut,
		// Token: 0x04003BC1 RID: 15297
		Gone1,
		// Token: 0x04003BC2 RID: 15298
		HauntIn,
		// Token: 0x04003BC3 RID: 15299
		Haunt,
		// Token: 0x04003BC4 RID: 15300
		HauntOut,
		// Token: 0x04003BC5 RID: 15301
		Gone2,
		// Token: 0x04003BC6 RID: 15302
		TeleportIn
	}

	// Token: 0x02000D59 RID: 3417
	private class SpecterInfo
	{
		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06004831 RID: 18481 RVA: 0x0017DCC0 File Offset: 0x0017BEC0
		public GameObject gameObject
		{
			get
			{
				return this.aiAnimator.gameObject;
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06004832 RID: 18482 RVA: 0x0017DCD0 File Offset: 0x0017BED0
		public Transform transform
		{
			get
			{
				return this.aiAnimator.transform;
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06004833 RID: 18483 RVA: 0x0017DCE0 File Offset: 0x0017BEE0
		public Renderer renderer
		{
			get
			{
				return this.aiAnimator.renderer;
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06004834 RID: 18484 RVA: 0x0017DCF0 File Offset: 0x0017BEF0
		public SpeculativeRigidbody specRigidbody
		{
			get
			{
				return this.aiAnimator.specRigidbody;
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06004835 RID: 18485 RVA: 0x0017DD00 File Offset: 0x0017BF00
		public tk2dBaseSprite sprite
		{
			get
			{
				return this.aiAnimator.sprite;
			}
		}

		// Token: 0x04003BC7 RID: 15303
		public AIAnimator aiAnimator;

		// Token: 0x04003BC8 RID: 15304
		public tk2dBaseSprite shadowSprite;
	}
}
