using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000DBC RID: 3516
[InspectorDropdownName("Bosses/GiantPowderSkull/MergoBehavior")]
public class GiantPowderSkullMergoBehavior : BasicAttackBehavior
{
	// Token: 0x06004A96 RID: 19094 RVA: 0x001906E0 File Offset: 0x0018E8E0
	public override void Start()
	{
		base.Start();
		PowderSkullParticleController componentInChildren = this.m_aiActor.GetComponentInChildren<PowderSkullParticleController>();
		this.m_mainParticleSystem = componentInChildren.GetComponent<ParticleSystem>();
		this.m_trailParticleSystem = componentInChildren.RotationChild.GetComponentInChildren<ParticleSystem>();
	}

	// Token: 0x06004A97 RID: 19095 RVA: 0x0019071C File Offset: 0x0018E91C
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004A98 RID: 19096 RVA: 0x00190734 File Offset: 0x0018E934
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
		this.m_state = GiantPowderSkullMergoBehavior.State.Fading;
		this.m_aiActor.healthHaver.minimumHealth = 1f;
		this.m_timer = this.darknessFadeTime;
		this.m_aiActor.ParentRoom.BecomeTerrifyingDarkRoom(this.darknessFadeTime, this.darknessAmount, this.playerLightAmount, "Play_ENM_darken_world_01");
		BraveUtility.EnableEmission(this.m_mainParticleSystem, false);
		BraveUtility.EnableEmission(this.m_trailParticleSystem, false);
		this.m_aiActor.ClearPath();
		this.m_aiActor.knockbackDoer.SetImmobile(true, "CrosshairBehavior");
		this.m_roomMin = this.m_aiActor.ParentRoom.area.UnitBottomLeft;
		this.m_roomMax = this.m_aiActor.ParentRoom.area.UnitTopRight;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004A99 RID: 19097 RVA: 0x0019083C File Offset: 0x0018EA3C
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		this.UpdateRoomParticles();
		if (this.m_state == GiantPowderSkullMergoBehavior.State.Fading)
		{
			if (this.m_timer <= 0f)
			{
				this.m_state = GiantPowderSkullMergoBehavior.State.OutAnim;
				this.m_aiAnimator.PlayUntilCancelled(this.teleportOutAnim, false, null, -1f, false);
				this.m_aiActor.specRigidbody.enabled = false;
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.m_state == GiantPowderSkullMergoBehavior.State.OutAnim)
		{
			if (!this.m_aiAnimator.IsPlaying(this.teleportOutAnim))
			{
				this.m_state = GiantPowderSkullMergoBehavior.State.Firing;
				this.m_timer = this.fireTime;
				this.m_mainShotTimer = this.fireMainMidTime;
				this.m_shadowSprite.renderer.enabled = false;
				this.m_aiActor.ToggleRenderers(false);
				this.roomParticleSystem.GetComponent<Renderer>().enabled = true;
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.m_state == GiantPowderSkullMergoBehavior.State.Firing)
		{
			if (this.m_timer <= 0f)
			{
				this.m_aiActor.TeleportSomewhere(new IntVector2?(new IntVector2(5, 5)), false);
				this.m_state = GiantPowderSkullMergoBehavior.State.Unfading;
				this.m_timer = this.darknessFadeTime;
				RoomHandler parentRoom = this.m_aiActor.ParentRoom;
				float num = this.darknessAmount;
				parentRoom.EndTerrifyingDarkRoom(1f, num, this.playerLightAmount, "Play_ENM_lighten_world_01");
				this.m_aiActor.ToggleRenderers(true);
				this.m_aiAnimator.PlayUntilFinished(this.teleportInAnim, false, null, -1f, false);
				this.m_shadowSprite.renderer.enabled = true;
				this.m_aiActor.ToggleRenderers(true);
				return ContinuousBehaviorResult.Continue;
			}
			this.m_mainShotTimer -= this.m_deltaTime;
			if (this.m_mainShotTimer < 0f)
			{
				this.ShootBulletScript();
				this.m_mainShotTimer += this.fireMainMidTime;
			}
		}
		else if (this.m_state == GiantPowderSkullMergoBehavior.State.Unfading)
		{
			if (!this.m_aiAnimator.IsPlaying(this.teleportInAnim) && !this.m_aiActor.specRigidbody.enabled)
			{
				this.m_aiActor.specRigidbody.enabled = true;
				BraveUtility.EnableEmission(this.m_mainParticleSystem, true);
				BraveUtility.EnableEmission(this.m_trailParticleSystem, true);
			}
			if (this.m_timer <= 0f && !this.m_aiAnimator.IsPlaying(this.teleportInAnim))
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A9A RID: 19098 RVA: 0x00190AA0 File Offset: 0x0018ECA0
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
		for (int i = allProjectiles.Count - 1; i >= 0; i--)
		{
			if (allProjectiles[i].Owner is AIActor && allProjectiles[i].name.Contains("cannon", true))
			{
				allProjectiles[i].DieInAir(false, true, true, false);
			}
		}
		this.m_aiActor.healthHaver.minimumHealth = 0f;
		this.m_aiAnimator.EndAnimationIf(this.teleportInAnim);
		this.m_aiAnimator.EndAnimationIf(this.teleportOutAnim);
		this.m_shadowSprite.renderer.enabled = true;
		this.m_aiActor.ToggleRenderers(true);
		this.m_aiActor.specRigidbody.enabled = true;
		BraveUtility.EnableEmission(this.m_mainParticleSystem, true);
		BraveUtility.EnableEmission(this.m_trailParticleSystem, true);
		this.roomParticleSystem.GetComponent<Renderer>().enabled = false;
		RoomHandler parentRoom = this.m_aiActor.ParentRoom;
		float num = this.darknessAmount;
		parentRoom.EndTerrifyingDarkRoom(1f, num, this.playerLightAmount, "Play_ENM_lighten_world_01");
		this.m_state = GiantPowderSkullMergoBehavior.State.Idle;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004A9B RID: 19099 RVA: 0x00190BE0 File Offset: 0x0018EDE0
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004A9C RID: 19100 RVA: 0x00190BE4 File Offset: 0x0018EDE4
	public override void OnActorPreDeath()
	{
		if (this.m_state == GiantPowderSkullMergoBehavior.State.Fading || this.m_state == GiantPowderSkullMergoBehavior.State.Firing)
		{
			RoomHandler parentRoom = this.m_aiActor.ParentRoom;
			float num = this.darknessAmount;
			parentRoom.EndTerrifyingDarkRoom(1f, num, this.playerLightAmount, "Play_ENM_lighten_world_01");
		}
		base.OnActorPreDeath();
	}

	// Token: 0x06004A9D RID: 19101 RVA: 0x00190C38 File Offset: 0x0018EE38
	private void ShootBulletScript()
	{
		if (!this.m_shootBulletSource)
		{
			this.m_shootBulletSource = new GameObject("Mergo shoot point").AddComponent<BulletScriptSource>();
		}
		this.m_shootBulletSource.transform.position = this.RandomShootPoint();
		this.m_shootBulletSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_shootBulletSource.BulletScript = this.shootBulletScript;
		this.m_shootBulletSource.Initialize();
	}

	// Token: 0x06004A9E RID: 19102 RVA: 0x00190CB8 File Offset: 0x0018EEB8
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

	// Token: 0x06004A9F RID: 19103 RVA: 0x00190DA4 File Offset: 0x0018EFA4
	private void UpdateRoomParticles()
	{
		if (this.m_state == GiantPowderSkullMergoBehavior.State.Idle || this.m_state == GiantPowderSkullMergoBehavior.State.Unfading)
		{
			return;
		}
		if (this.m_state == GiantPowderSkullMergoBehavior.State.Fading && this.m_timer > this.darknessFadeTime / 2f)
		{
			float num = (1f - this.m_timer / this.darknessFadeTime) * 2f;
			int num2 = Mathf.RoundToInt(200f * this.m_deltaTime);
			for (int i = 0; i < num2; i++)
			{
				float num3 = (float)UnityEngine.Random.Range(0, 360);
				float num4 = UnityEngine.Random.Range(num * 15f - 2f, num * 15f);
				Vector3 vector = this.roomParticleSystem.transform.position + BraveMathCollege.DegreesToVector(num3, num4);
				vector.z = vector.y;
				float startLifetime = this.roomParticleSystem.startLifetime;
				ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
				{
					position = vector,
					velocity = BraveMathCollege.DegreesToVector((float)UnityEngine.Random.Range(0, 360), this.roomParticleSystem.startSpeed),
					startLifetime = startLifetime,
					startSize = this.roomParticleSystem.startSize,
					rotation = this.roomParticleSystem.startRotation,
					startColor = this.roomParticleSystem.startColor
				};
				this.roomParticleSystem.Emit(emitParams, 1);
			}
		}
		else
		{
			int num5 = Mathf.RoundToInt(840f * this.m_deltaTime);
			for (int j = 0; j < num5; j++)
			{
				Vector3 vector2 = BraveUtility.RandomVector2(this.m_roomMin, this.m_roomMax, new Vector2(0.5f, 0.5f));
				vector2.z = vector2.y;
				float startLifetime2 = this.roomParticleSystem.startLifetime;
				ParticleSystem.EmitParams emitParams2 = new ParticleSystem.EmitParams
				{
					position = vector2,
					velocity = BraveMathCollege.DegreesToVector((float)UnityEngine.Random.Range(0, 360), this.roomParticleSystem.startSpeed),
					startLifetime = startLifetime2,
					startSize = this.roomParticleSystem.startSize,
					rotation = this.roomParticleSystem.startRotation,
					startColor = this.roomParticleSystem.startColor
				};
				this.roomParticleSystem.Emit(emitParams2, 1);
			}
		}
	}

	// Token: 0x04003F58 RID: 16216
	public BulletScriptSelector shootBulletScript;

	// Token: 0x04003F59 RID: 16217
	public float darknessFadeTime = 1f;

	// Token: 0x04003F5A RID: 16218
	public float darknessAmount = 0.3f;

	// Token: 0x04003F5B RID: 16219
	public float playerLightAmount = 0.5f;

	// Token: 0x04003F5C RID: 16220
	public float fireTime = 8f;

	// Token: 0x04003F5D RID: 16221
	public float fireMainMidTime = 0.8f;

	// Token: 0x04003F5E RID: 16222
	public float fireMainDist = 16f;

	// Token: 0x04003F5F RID: 16223
	public float fireMainDistVariance = 3f;

	// Token: 0x04003F60 RID: 16224
	[InspectorCategory("Visuals")]
	public string teleportOutAnim;

	// Token: 0x04003F61 RID: 16225
	[InspectorCategory("Visuals")]
	public string teleportInAnim;

	// Token: 0x04003F62 RID: 16226
	[InspectorCategory("Visuals")]
	public ParticleSystem roomParticleSystem;

	// Token: 0x04003F63 RID: 16227
	private GiantPowderSkullMergoBehavior.State m_state;

	// Token: 0x04003F64 RID: 16228
	private tk2dBaseSprite m_shadowSprite;

	// Token: 0x04003F65 RID: 16229
	private ParticleSystem m_mainParticleSystem;

	// Token: 0x04003F66 RID: 16230
	private ParticleSystem m_trailParticleSystem;

	// Token: 0x04003F67 RID: 16231
	private Vector2 m_roomMin;

	// Token: 0x04003F68 RID: 16232
	private Vector2 m_roomMax;

	// Token: 0x04003F69 RID: 16233
	private float m_timer;

	// Token: 0x04003F6A RID: 16234
	private float m_mainShotTimer;

	// Token: 0x04003F6B RID: 16235
	private BulletScriptSource m_shootBulletSource;

	// Token: 0x02000DBD RID: 3517
	public enum State
	{
		// Token: 0x04003F6D RID: 16237
		Idle,
		// Token: 0x04003F6E RID: 16238
		Fading,
		// Token: 0x04003F6F RID: 16239
		OutAnim,
		// Token: 0x04003F70 RID: 16240
		Firing,
		// Token: 0x04003F71 RID: 16241
		Unfading,
		// Token: 0x04003F72 RID: 16242
		InAnim
	}
}
