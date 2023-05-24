using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001243 RID: 4675
public class TorchController : BraveBehaviour
{
	// Token: 0x17000F8E RID: 3982
	// (get) Token: 0x060068C0 RID: 26816 RVA: 0x002904FC File Offset: 0x0028E6FC
	// (set) Token: 0x060068C1 RID: 26817 RVA: 0x00290504 File Offset: 0x0028E704
	public CellData Cell { get; set; }

	// Token: 0x060068C2 RID: 26818 RVA: 0x00290510 File Offset: 0x0028E710
	public void Start()
	{
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.OnEnterTrigger));
		}
		if (base.sprite.FlipX)
		{
			this.douseOffset.transform.localPosition = this.douseOffset.transform.localPosition.Scale(-1f, 1f, 1f);
		}
		if (base.specRigidbody)
		{
			RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
			if (absoluteRoom.IsWinchesterArcadeRoom)
			{
				base.specRigidbody.enabled = false;
			}
		}
	}

	// Token: 0x060068C3 RID: 26819 RVA: 0x002905D0 File Offset: 0x0028E7D0
	protected override void OnDestroy()
	{
		if (base.specRigidbody != null)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.OnEnterTrigger));
		}
		base.OnDestroy();
	}

	// Token: 0x060068C4 RID: 26820 RVA: 0x00290610 File Offset: 0x0028E810
	public void BeamCollision(Projectile p)
	{
		this.AnyCollision(p);
	}

	// Token: 0x060068C5 RID: 26821 RVA: 0x0029061C File Offset: 0x0028E81C
	private void AnyCollision(Projectile p)
	{
		if (this.m_isLit && ((p.damageTypes & CoreDamageTypes.Water) == CoreDamageTypes.Water || this.anyProjectileBreaks))
		{
			this.m_isLit = false;
			for (int i = 0; i < this.renderers.Length; i++)
			{
				this.renderers[i].enabled = false;
				base.visibilityManager.AddIgnoredRenderer(this.renderers[i]);
			}
			if (this.Cell != null && this.Cell.cellVisualData.lightObject != null)
			{
				this.Cell.cellVisualData.lightObject.SetActive(false);
			}
			Vector3 vector = ((!this.douseOffset) ? base.specRigidbody.UnitCenter : this.douseOffset.transform.position);
			VFXPool vfxpool = this.douseVfx;
			Vector3 vector2 = vector;
			tk2dBaseSprite sprite = base.sprite;
			vfxpool.SpawnAtPosition(vector2, 0f, null, null, null, null, true, null, sprite, false);
			if (this.createsShardsOnDouse)
			{
				for (int j = 0; j < this.douseShards.Length; j++)
				{
					this.douseShards[j].SpawnShards(this.douseOffset.position + (p.LastVelocity * -1f).normalized.ToVector3ZUp(0f), p.LastVelocity * -1f, -30f, 30f, 0f, 0.5f, 1f, null);
				}
			}
			if (this.disappearAfterDouse)
			{
				if (!this.canBeRelit)
				{
					if (!string.IsNullOrEmpty(this.dousedAnim))
					{
						this.flameAnimator.PlayAndDestroyObject(this.dousedAnim, null);
					}
					else
					{
						UnityEngine.Object.Destroy(this.flameAnimator.gameObject);
					}
				}
				else if (!string.IsNullOrEmpty(this.dousedAnim))
				{
					this.flameAnimator.PlayAndDisableRenderer(this.dousedAnim);
				}
				else
				{
					this.flameAnimator.GetComponent<Renderer>().enabled = false;
				}
			}
			else if (!string.IsNullOrEmpty(this.dousedAnim))
			{
				this.flameAnimator.Play(this.dousedAnim);
			}
			return;
		}
		if (!this.m_isLit && this.canBeRelit && (p.damageTypes & CoreDamageTypes.Fire) == CoreDamageTypes.Fire)
		{
			this.m_isLit = true;
			for (int k = 0; k < this.renderers.Length; k++)
			{
				this.renderers[k].enabled = true;
				base.visibilityManager.RemoveIgnoredRenderer(this.renderers[k]);
			}
			if (this.Cell != null && this.Cell.cellVisualData.lightObject != null)
			{
				this.Cell.cellVisualData.lightObject.SetActive(true);
			}
			this.douseVfx.DestroyAll();
			this.flameAnimator.GetComponent<Renderer>().enabled = true;
			this.flameAnimator.Play(this.flameAnim);
			return;
		}
	}

	// Token: 0x060068C6 RID: 26822 RVA: 0x00290964 File Offset: 0x0028EB64
	private void OnEnterTrigger(SpeculativeRigidbody mySpecRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (collisionData.OtherRigidbody.projectile)
		{
			if (this.m_isLit)
			{
				this.sparkVfx.SpawnAtPosition(base.specRigidbody.UnitCenter, 0f, null, null, null, null, false, null, null, false);
				if (this.deployGoop)
				{
					DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopToDeploy).TimedAddGoopCircle(base.specRigidbody.UnitBottomCenter, this.goopRadius, 0.5f, false);
				}
				if (this.igniteGoop)
				{
					for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
					{
						Vector2 vector = new Vector2(base.specRigidbody.UnitCenter.x, base.transform.position.y);
						StaticReferenceManager.AllGoops[i].IgniteGoopCircle(vector, this.igniteRadius);
					}
				}
			}
			this.AnyCollision(collisionData.OtherRigidbody.projectile);
		}
	}

	// Token: 0x0400650E RID: 25870
	[Header("VFX")]
	public VFXPool sparkVfx;

	// Token: 0x0400650F RID: 25871
	public VFXPool douseVfx;

	// Token: 0x04006510 RID: 25872
	public Transform douseOffset;

	// Token: 0x04006511 RID: 25873
	[Header("Animations")]
	public tk2dSpriteAnimator flameAnimator;

	// Token: 0x04006512 RID: 25874
	public string flameAnim;

	// Token: 0x04006513 RID: 25875
	public string dousedAnim;

	// Token: 0x04006514 RID: 25876
	public Renderer[] renderers;

	// Token: 0x04006515 RID: 25877
	[Header("Other")]
	public bool canBeRelit = true;

	// Token: 0x04006516 RID: 25878
	public bool anyProjectileBreaks;

	// Token: 0x04006517 RID: 25879
	public bool disappearAfterDouse;

	// Token: 0x04006518 RID: 25880
	public bool igniteGoop = true;

	// Token: 0x04006519 RID: 25881
	[ShowInInspectorIf("igniteGoop", false)]
	public float igniteRadius = 0.5f;

	// Token: 0x0400651A RID: 25882
	public bool deployGoop;

	// Token: 0x0400651B RID: 25883
	[ShowInInspectorIf("deployGoop", false)]
	public GoopDefinition goopToDeploy;

	// Token: 0x0400651C RID: 25884
	[ShowInInspectorIf("deployGoop", false)]
	public float goopRadius = 3f;

	// Token: 0x0400651D RID: 25885
	public bool createsShardsOnDouse;

	// Token: 0x0400651E RID: 25886
	public ShardCluster[] douseShards;

	// Token: 0x04006520 RID: 25888
	private bool m_isLit = true;
}
