using System;
using UnityEngine;

// Token: 0x02001647 RID: 5703
public class GoopModifier : BraveBehaviour
{
	// Token: 0x17001401 RID: 5121
	// (get) Token: 0x06008529 RID: 34089 RVA: 0x0036E844 File Offset: 0x0036CA44
	public DeadlyDeadlyGoopManager Manager
	{
		get
		{
			if (this.m_manager == null)
			{
				this.m_manager = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopDefinition);
			}
			return this.m_manager;
		}
	}

	// Token: 0x0600852A RID: 34090 RVA: 0x0036E870 File Offset: 0x0036CA70
	public void Start()
	{
		if (this.IsSynergyContingent)
		{
			if (!base.projectile || !base.projectile.PossibleSourceGun || !(base.projectile.PossibleSourceGun.CurrentOwner is PlayerController))
			{
				base.enabled = false;
				return;
			}
			PlayerController playerController = base.projectile.PossibleSourceGun.CurrentOwner as PlayerController;
			if (!playerController.HasActiveBonusSynergy(this.RequiredSynergy, false))
			{
				base.enabled = false;
				return;
			}
			this.SynergyViable = true;
		}
		if (base.GetComponent<BeamController>())
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600852B RID: 34091 RVA: 0x0036E924 File Offset: 0x0036CB24
	public void Update()
	{
		if (this.m_manager == null)
		{
			this.m_manager = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopDefinition);
		}
		if (this.IsSynergyContingent && base.projectile && base.projectile.OverrideMotionModule != null && base.projectile.OverrideMotionModule is OrbitProjectileMotionModule)
		{
			base.enabled = false;
			return;
		}
		if (this.SpawnGoopInFlight)
		{
			this.elapsed += BraveTime.DeltaTime;
			this.m_totalElapsed += BraveTime.DeltaTime;
			if ((!this.UsesInitialDelay || this.m_totalElapsed > this.InitialDelay) && this.elapsed >= this.InFlightSpawnFrequency)
			{
				this.elapsed -= this.InFlightSpawnFrequency;
				Vector2 vector = base.projectile.sprite.WorldCenter + this.spawnOffset - base.projectile.transform.position.XY();
				this.m_manager.AddGoopLine(base.projectile.sprite.WorldCenter + this.spawnOffset, base.projectile.LastPosition.XY() + vector, this.InFlightSpawnRadius);
				if (this.goopDefinition.CanBeFrozen && (base.projectile.damageTypes | CoreDamageTypes.Ice) == base.projectile.damageTypes)
				{
					this.Manager.FreezeGoopCircle(base.projectile.sprite.WorldCenter, this.InFlightSpawnRadius);
				}
			}
		}
	}

	// Token: 0x0600852C RID: 34092 RVA: 0x0036EAD0 File Offset: 0x0036CCD0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600852D RID: 34093 RVA: 0x0036EAD8 File Offset: 0x0036CCD8
	public void SpawnCollisionGoop(Vector2 pos)
	{
		if (this.IsSynergyContingent && !this.SynergyViable)
		{
			return;
		}
		if (this.SpawnGoopOnCollision)
		{
			this.Manager.TimedAddGoopCircle(pos, this.CollisionSpawnRadius, 0.5f, false);
			if (this.goopDefinition.CanBeFrozen && (base.projectile.damageTypes | CoreDamageTypes.Ice) == base.projectile.damageTypes)
			{
				this.Manager.FreezeGoopCircle(pos, this.CollisionSpawnRadius);
			}
		}
	}

	// Token: 0x0600852E RID: 34094 RVA: 0x0036EB60 File Offset: 0x0036CD60
	public void SpawnCollisionGoop(CollisionData lcr)
	{
		if (this.IsSynergyContingent && !this.SynergyViable)
		{
			return;
		}
		if (this.SpawnGoopOnCollision)
		{
			this.Manager.TimedAddGoopCircle(lcr.Contact, this.CollisionSpawnRadius, 0.5f, false);
			if (this.goopDefinition.CanBeFrozen && (base.projectile.damageTypes | CoreDamageTypes.Ice) == base.projectile.damageTypes)
			{
				this.Manager.FreezeGoopCircle(lcr.Contact, this.CollisionSpawnRadius);
			}
		}
	}

	// Token: 0x04008936 RID: 35126
	public GoopDefinition goopDefinition;

	// Token: 0x04008937 RID: 35127
	public bool IsSynergyContingent;

	// Token: 0x04008938 RID: 35128
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008939 RID: 35129
	public bool SpawnGoopInFlight;

	// Token: 0x0400893A RID: 35130
	[ShowInInspectorIf("SpawnGoopInFlight", true)]
	public float InFlightSpawnFrequency = 0.05f;

	// Token: 0x0400893B RID: 35131
	[ShowInInspectorIf("SpawnGoopInFlight", true)]
	public float InFlightSpawnRadius = 1f;

	// Token: 0x0400893C RID: 35132
	public bool SpawnGoopOnCollision;

	// Token: 0x0400893D RID: 35133
	[ShowInInspectorIf("SpawnGoopOnCollision", true)]
	public bool OnlyGoopOnEnemyCollision;

	// Token: 0x0400893E RID: 35134
	[ShowInInspectorIf("SpawnGoopOnCollision", true)]
	public float CollisionSpawnRadius = 3f;

	// Token: 0x0400893F RID: 35135
	public bool SpawnAtBeamEnd;

	// Token: 0x04008940 RID: 35136
	[ShowInInspectorIf("SpawnAtBeamEnd", true)]
	public float BeamEndRadius = 1f;

	// Token: 0x04008941 RID: 35137
	public Vector2 spawnOffset = new Vector2(0f, -0.5f);

	// Token: 0x04008942 RID: 35138
	public bool UsesInitialDelay;

	// Token: 0x04008943 RID: 35139
	public float InitialDelay = 0.25f;

	// Token: 0x04008944 RID: 35140
	private float m_totalElapsed;

	// Token: 0x04008945 RID: 35141
	[NonSerialized]
	public bool SynergyViable;

	// Token: 0x04008946 RID: 35142
	private DeadlyDeadlyGoopManager m_manager;

	// Token: 0x04008947 RID: 35143
	private float elapsed;
}
