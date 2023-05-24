using System;
using FullInspector;
using Pathfinding;
using UnityEngine;

// Token: 0x02000D8C RID: 3468
[InspectorDropdownName("Bosses/BossFinalMarine/PortalBehavior")]
public class BossFinalMarinePortalBehavior : BasicAttackBehavior
{
	// Token: 0x06004971 RID: 18801 RVA: 0x00188410 File Offset: 0x00186610
	private bool ShowShadowAnimationNames()
	{
		return this.shadowSupport == BossFinalMarinePortalBehavior.ShadowSupport.Animate;
	}

	// Token: 0x06004972 RID: 18802 RVA: 0x0018841C File Offset: 0x0018661C
	public override void Start()
	{
		base.Start();
		this.m_portalController = UnityEngine.Object.FindObjectOfType<DimensionFogController>();
	}

	// Token: 0x06004973 RID: 18803 RVA: 0x00188430 File Offset: 0x00186630
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004974 RID: 18804 RVA: 0x00188448 File Offset: 0x00186648
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
		this.State = BossFinalMarinePortalBehavior.TeleportState.TeleportOut;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004975 RID: 18805 RVA: 0x001884B8 File Offset: 0x001866B8
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.State == BossFinalMarinePortalBehavior.TeleportState.TeleportOut)
		{
			if (this.shadowSupport == BossFinalMarinePortalBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f - this.m_aiAnimator.CurrentClipProgress);
			}
			if (!this.m_aiAnimator.IsPlaying(this.teleportOutAnim))
			{
				this.State = BossFinalMarinePortalBehavior.TeleportState.Gone;
			}
		}
		else if (this.State == BossFinalMarinePortalBehavior.TeleportState.Gone)
		{
			if (this.m_timer <= 0f)
			{
				this.State = BossFinalMarinePortalBehavior.TeleportState.TeleportIn;
			}
		}
		else if (this.State == BossFinalMarinePortalBehavior.TeleportState.TeleportIn)
		{
			if (this.shadowSupport == BossFinalMarinePortalBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(this.m_aiAnimator.CurrentClipProgress);
			}
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "BossFinalMarinePortalBehavior");
			}
			if (!this.m_aiAnimator.IsPlaying(this.teleportInAnim))
			{
				this.State = BossFinalMarinePortalBehavior.TeleportState.PostTeleport;
			}
		}
		else if (this.State == BossFinalMarinePortalBehavior.TeleportState.PostTeleport && !this.m_aiAnimator.IsPlaying(this.portalAnim))
		{
			this.State = BossFinalMarinePortalBehavior.TeleportState.None;
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004976 RID: 18806 RVA: 0x00188608 File Offset: 0x00186808
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004977 RID: 18807 RVA: 0x00188620 File Offset: 0x00186820
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x17000A8C RID: 2700
	// (get) Token: 0x06004978 RID: 18808 RVA: 0x00188624 File Offset: 0x00186824
	// (set) Token: 0x06004979 RID: 18809 RVA: 0x0018862C File Offset: 0x0018682C
	private BossFinalMarinePortalBehavior.TeleportState State
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

	// Token: 0x0600497A RID: 18810 RVA: 0x00188650 File Offset: 0x00186850
	private void BeginState(BossFinalMarinePortalBehavior.TeleportState state)
	{
		if (state == BossFinalMarinePortalBehavior.TeleportState.TeleportOut)
		{
			if (this.teleportRequiresTransparency)
			{
				this.m_cachedShader = this.m_aiActor.renderer.material.shader;
				this.m_aiActor.sprite.usesOverrideMaterial = true;
				this.m_aiActor.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
			}
			this.m_aiAnimator.PlayUntilCancelled(this.teleportOutAnim, true, null, -1f, false);
			if (this.shadowSupport == BossFinalMarinePortalBehavior.ShadowSupport.Animate)
			{
				this.m_shadowSprite.spriteAnimator.PlayAndForceTime(this.shadowOutAnim, this.m_aiAnimator.CurrentClipLength);
			}
			this.m_aiActor.ClearPath();
			if (!this.AttackableDuringAnimation)
			{
				this.m_aiActor.specRigidbody.CollideWithOthers = false;
				this.m_aiActor.IsGone = true;
			}
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "BossFinalMarinePortalBehavior");
			}
			if (!this.hasOutlinesDuringAnim)
			{
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, false);
			}
		}
		else if (state == BossFinalMarinePortalBehavior.TeleportState.Gone)
		{
			if (this.GoneTime <= 0f)
			{
				this.State = BossFinalMarinePortalBehavior.TeleportState.TeleportIn;
				return;
			}
			this.m_timer = this.GoneTime;
			this.m_aiActor.specRigidbody.CollideWithOthers = false;
			this.m_aiActor.IsGone = true;
			this.m_aiActor.sprite.renderer.enabled = false;
		}
		else if (state == BossFinalMarinePortalBehavior.TeleportState.TeleportIn)
		{
			this.DoTeleport();
			this.m_aiAnimator.PlayUntilFinished(this.teleportInAnim, true, null, -1f, false);
			if (this.shadowSupport == BossFinalMarinePortalBehavior.ShadowSupport.Animate)
			{
				this.m_shadowSprite.spriteAnimator.PlayAndForceTime(this.shadowInAnim, this.m_aiAnimator.CurrentClipLength);
			}
			this.m_shadowSprite.renderer.enabled = true;
			if (this.AttackableDuringAnimation)
			{
				this.m_aiActor.specRigidbody.CollideWithOthers = true;
				this.m_aiActor.IsGone = false;
			}
			this.m_aiActor.sprite.renderer.enabled = true;
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "BossFinalMarinePortalBehavior");
			}
			if (this.hasOutlinesDuringAnim)
			{
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, true);
			}
		}
		else if (state == BossFinalMarinePortalBehavior.TeleportState.PostTeleport)
		{
			this.m_aiAnimator.PlayUntilFinished(this.portalAnim, true, null, -1f, false);
			this.m_portalController.targetRadius = this.PortalSize;
		}
	}

	// Token: 0x0600497B RID: 18811 RVA: 0x001888F0 File Offset: 0x00186AF0
	private void EndState(BossFinalMarinePortalBehavior.TeleportState state)
	{
		if (state == BossFinalMarinePortalBehavior.TeleportState.TeleportOut)
		{
			this.m_shadowSprite.renderer.enabled = false;
			if (this.hasOutlinesDuringAnim)
			{
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, false);
			}
		}
		else if (state == BossFinalMarinePortalBehavior.TeleportState.TeleportIn)
		{
			if (this.teleportRequiresTransparency)
			{
				this.m_aiActor.sprite.usesOverrideMaterial = false;
				this.m_aiActor.renderer.material.shader = this.m_cachedShader;
			}
			if (this.shadowSupport == BossFinalMarinePortalBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f);
			}
			this.m_aiActor.specRigidbody.CollideWithOthers = true;
			this.m_aiActor.IsGone = false;
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(true, "BossFinalMarinePortalBehavior");
			}
			if (!this.hasOutlinesDuringAnim)
			{
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, true);
			}
		}
	}

	// Token: 0x0600497C RID: 18812 RVA: 0x001889FC File Offset: 0x00186BFC
	private void DoTeleport()
	{
		Vector2 vector = this.m_aiActor.specRigidbody.UnitCenter - this.m_aiActor.transform.position.XY();
		IntVector2 intVector = (this.roomMin + (this.roomMax - this.roomMin + Vector2.one) / 2f).ToIntVector2(VectorConversions.Floor);
		this.m_aiActor.transform.position = Pathfinder.GetClearanceOffset(intVector, this.m_aiActor.Clearance) - vector;
		this.m_aiActor.specRigidbody.Reinitialize();
	}

	// Token: 0x04003DC3 RID: 15811
	public bool AttackableDuringAnimation;

	// Token: 0x04003DC4 RID: 15812
	public float GoneTime = 1f;

	// Token: 0x04003DC5 RID: 15813
	public float PortalSize = 16f;

	// Token: 0x04003DC6 RID: 15814
	[InspectorCategory("Visuals")]
	public string teleportOutAnim = "teleport_out";

	// Token: 0x04003DC7 RID: 15815
	[InspectorCategory("Visuals")]
	public string teleportInAnim = "teleport_in";

	// Token: 0x04003DC8 RID: 15816
	[InspectorCategory("Visuals")]
	public bool teleportRequiresTransparency;

	// Token: 0x04003DC9 RID: 15817
	[InspectorCategory("Visuals")]
	public bool hasOutlinesDuringAnim = true;

	// Token: 0x04003DCA RID: 15818
	[InspectorCategory("Visuals")]
	public string portalAnim = "weak_attack_charge";

	// Token: 0x04003DCB RID: 15819
	[InspectorCategory("Visuals")]
	public BossFinalMarinePortalBehavior.ShadowSupport shadowSupport;

	// Token: 0x04003DCC RID: 15820
	[InspectorShowIf("ShowShadowAnimationNames")]
	[InspectorCategory("Visuals")]
	public string shadowOutAnim;

	// Token: 0x04003DCD RID: 15821
	[InspectorCategory("Visuals")]
	[InspectorShowIf("ShowShadowAnimationNames")]
	public string shadowInAnim;

	// Token: 0x04003DCE RID: 15822
	public bool ManuallyDefineRoom;

	// Token: 0x04003DCF RID: 15823
	[InspectorShowIf("ManuallyDefineRoom")]
	[InspectorIndent]
	public Vector2 roomMin;

	// Token: 0x04003DD0 RID: 15824
	[InspectorIndent]
	[InspectorShowIf("ManuallyDefineRoom")]
	public Vector2 roomMax;

	// Token: 0x04003DD1 RID: 15825
	private DimensionFogController m_portalController;

	// Token: 0x04003DD2 RID: 15826
	private tk2dBaseSprite m_shadowSprite;

	// Token: 0x04003DD3 RID: 15827
	private Shader m_cachedShader;

	// Token: 0x04003DD4 RID: 15828
	private float m_timer;

	// Token: 0x04003DD5 RID: 15829
	private BossFinalMarinePortalBehavior.TeleportState m_state;

	// Token: 0x02000D8D RID: 3469
	public enum ShadowSupport
	{
		// Token: 0x04003DD7 RID: 15831
		None,
		// Token: 0x04003DD8 RID: 15832
		Fade,
		// Token: 0x04003DD9 RID: 15833
		Animate
	}

	// Token: 0x02000D8E RID: 3470
	private enum TeleportState
	{
		// Token: 0x04003DDB RID: 15835
		None,
		// Token: 0x04003DDC RID: 15836
		TeleportOut,
		// Token: 0x04003DDD RID: 15837
		Gone,
		// Token: 0x04003DDE RID: 15838
		TeleportIn,
		// Token: 0x04003DDF RID: 15839
		PostTeleport
	}
}
