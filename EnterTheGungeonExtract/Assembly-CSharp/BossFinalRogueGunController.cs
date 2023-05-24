using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000FE4 RID: 4068
public abstract class BossFinalRogueGunController : BaseBehavior<FullSerializerSerializer>
{
	// Token: 0x060058BC RID: 22716 RVA: 0x0021E854 File Offset: 0x0021CA54
	public bool IsTimed()
	{
		return this.fireType == BossFinalRogueGunController.FireType.Timed;
	}

	// Token: 0x060058BD RID: 22717 RVA: 0x0021E860 File Offset: 0x0021CA60
	public virtual void Start()
	{
		if (this.fireType == BossFinalRogueGunController.FireType.Timed)
		{
			this.m_shotTimer = this.initialDelay;
		}
		this.ship.healthHaver.OnPreDeath += this.OnPreDeath;
	}

	// Token: 0x060058BE RID: 22718 RVA: 0x0021E898 File Offset: 0x0021CA98
	public virtual void Update()
	{
		if (!this.ship.aiActor.enabled)
		{
			return;
		}
		if (!this.ship.behaviorSpeculator.enabled)
		{
			return;
		}
		if (this.fireType == BossFinalRogueGunController.FireType.Timed && this.IsFinished)
		{
			this.m_shotTimer -= BraveTime.DeltaTime;
			if (this.m_shotTimer <= 0f)
			{
				this.Fire();
				this.m_shotTimer = this.delay;
			}
		}
	}

	// Token: 0x060058BF RID: 22719 RVA: 0x0021E920 File Offset: 0x0021CB20
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060058C0 RID: 22720 RVA: 0x0021E928 File Offset: 0x0021CB28
	private void OnPreDeath(Vector2 deathDir)
	{
		this.CeaseFire();
	}

	// Token: 0x060058C1 RID: 22721
	public abstract void Fire();

	// Token: 0x17000CB7 RID: 3255
	// (get) Token: 0x060058C2 RID: 22722
	public abstract bool IsFinished { get; }

	// Token: 0x060058C3 RID: 22723
	public abstract void CeaseFire();

	// Token: 0x040051E5 RID: 20965
	public BossFinalRogueController ship;

	// Token: 0x040051E6 RID: 20966
	public BossFinalRogueGunController.FireType fireType = BossFinalRogueGunController.FireType.Triggered;

	// Token: 0x040051E7 RID: 20967
	[InspectorShowIf("IsTimed")]
	public float initialDelay;

	// Token: 0x040051E8 RID: 20968
	[InspectorShowIf("IsTimed")]
	public float delay;

	// Token: 0x040051E9 RID: 20969
	private float m_shotTimer;

	// Token: 0x02000FE5 RID: 4069
	public enum FireType
	{
		// Token: 0x040051EB RID: 20971
		Triggered = 10,
		// Token: 0x040051EC RID: 20972
		Timed = 20
	}
}
