using System;
using UnityEngine;

// Token: 0x02000E53 RID: 3667
public class CollisionTriggerEvent : BraveBehaviour
{
	// Token: 0x06004E1D RID: 19997 RVA: 0x001AF030 File Offset: 0x001AD230
	public void Start()
	{
		if (this.onTriggerEnter)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTrigger));
		}
		if (this.onTriggerCollision)
		{
			SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
			specRigidbody2.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody2.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTrigger));
		}
		if (this.onTriggerExit)
		{
			SpeculativeRigidbody specRigidbody3 = base.specRigidbody;
			specRigidbody3.OnExitTrigger = (SpeculativeRigidbody.OnTriggerExitDelegate)Delegate.Combine(specRigidbody3.OnExitTrigger, new SpeculativeRigidbody.OnTriggerExitDelegate(this.OnTriggerSimple));
		}
	}

	// Token: 0x06004E1E RID: 19998 RVA: 0x001AF0D4 File Offset: 0x001AD2D4
	public void Update()
	{
		if (this.m_triggered)
		{
			this.m_timer -= BraveTime.DeltaTime;
			if (this.m_timer <= 0f)
			{
				this.DoEventStuff();
			}
		}
	}

	// Token: 0x06004E1F RID: 19999 RVA: 0x001AF10C File Offset: 0x001AD30C
	private void OnTrigger(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		this.OnTriggerSimple(specRigidbody, sourceSpecRigidbody);
	}

	// Token: 0x06004E20 RID: 20000 RVA: 0x001AF118 File Offset: 0x001AD318
	private void OnTriggerSimple(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody)
	{
		if (this.delay <= 0f)
		{
			this.DoEventStuff();
		}
		else
		{
			this.m_triggered = true;
			this.m_timer = this.delay;
		}
	}

	// Token: 0x06004E21 RID: 20001 RVA: 0x001AF148 File Offset: 0x001AD348
	private void DoEventStuff()
	{
		if (!string.IsNullOrEmpty(this.animationName) && base.spriteAnimator)
		{
			base.spriteAnimator.Play(this.animationName);
			if (this.destroyAfterAnimation)
			{
				base.gameObject.AddComponent<SpriteAnimatorKiller>();
			}
		}
		this.vfx.SpawnAtLocalPosition(this.vfxOffset, 0f, base.transform, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), false, null, false);
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06004E22 RID: 20002 RVA: 0x001AF1DC File Offset: 0x001AD3DC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04004469 RID: 17513
	public bool onTriggerEnter;

	// Token: 0x0400446A RID: 17514
	public bool onTriggerCollision;

	// Token: 0x0400446B RID: 17515
	public bool onTriggerExit;

	// Token: 0x0400446C RID: 17516
	public float delay;

	// Token: 0x0400446D RID: 17517
	public string animationName;

	// Token: 0x0400446E RID: 17518
	public bool destroyAfterAnimation;

	// Token: 0x0400446F RID: 17519
	public VFXPool vfx;

	// Token: 0x04004470 RID: 17520
	public Vector2 vfxOffset;

	// Token: 0x04004471 RID: 17521
	private bool m_triggered;

	// Token: 0x04004472 RID: 17522
	private float m_timer;
}
