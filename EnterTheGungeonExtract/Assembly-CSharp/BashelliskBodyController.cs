using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FBD RID: 4029
public class BashelliskBodyController : BashelliskSegment
{
	// Token: 0x17000C89 RID: 3209
	// (get) Token: 0x060057C1 RID: 22465 RVA: 0x00217F90 File Offset: 0x00216190
	// (set) Token: 0x060057C2 RID: 22466 RVA: 0x00217F98 File Offset: 0x00216198
	public BashelliskBodyController.ShootDirection shootDirection { get; set; }

	// Token: 0x17000C8A RID: 3210
	// (get) Token: 0x060057C3 RID: 22467 RVA: 0x00217FA4 File Offset: 0x002161A4
	public bool IsShooting
	{
		get
		{
			return this.State != BashelliskBodyController.BodyState.Idle;
		}
	}

	// Token: 0x17000C8B RID: 3211
	// (get) Token: 0x060057C4 RID: 22468 RVA: 0x00217FB4 File Offset: 0x002161B4
	// (set) Token: 0x060057C5 RID: 22469 RVA: 0x00217FBC File Offset: 0x002161BC
	public float BaseShootDirection { get; private set; }

	// Token: 0x17000C8C RID: 3212
	// (get) Token: 0x060057C6 RID: 22470 RVA: 0x00217FC8 File Offset: 0x002161C8
	// (set) Token: 0x060057C7 RID: 22471 RVA: 0x00217FD0 File Offset: 0x002161D0
	public bool IsBroken { get; set; }

	// Token: 0x060057C8 RID: 22472 RVA: 0x00217FDC File Offset: 0x002161DC
	public void Start()
	{
		if (base.majorBreakable)
		{
			MajorBreakable majorBreakable = base.majorBreakable;
			majorBreakable.OnBreak = (Action)Delegate.Combine(majorBreakable.OnBreak, new Action(this.OnBreak));
		}
	}

	// Token: 0x060057C9 RID: 22473 RVA: 0x00218018 File Offset: 0x00216218
	public void Init(BashelliskHeadController headController)
	{
		this.m_head = headController;
		base.healthHaver = this.m_head.healthHaver;
		base.aiActor = this.m_head.aiActor;
		base.aiActor.healthHaver.bodySprites.Add(base.sprite);
	}

	// Token: 0x060057CA RID: 22474 RVA: 0x0021806C File Offset: 0x0021626C
	public void Update()
	{
		this.UpdateState();
	}

	// Token: 0x060057CB RID: 22475 RVA: 0x00218074 File Offset: 0x00216274
	protected override void OnDestroy()
	{
		MajorBreakable majorBreakable = base.majorBreakable;
		majorBreakable.OnBreak = (Action)Delegate.Remove(majorBreakable.OnBreak, new Action(this.OnBreak));
		base.OnDestroy();
	}

	// Token: 0x060057CC RID: 22476 RVA: 0x002180A4 File Offset: 0x002162A4
	public void Fire(BulletScriptSelector bulletScript)
	{
		if (this.IsBroken)
		{
			return;
		}
		this.m_bulletScript = bulletScript;
		this.State = BashelliskBodyController.BodyState.Intro;
	}

	// Token: 0x060057CD RID: 22477 RVA: 0x002180C0 File Offset: 0x002162C0
	public void UpdateShootDirection()
	{
		float num = 0f;
		if (this.shootDirection == BashelliskBodyController.ShootDirection.NextSegment)
		{
			num = (this.previous.center.position - this.center.position).XY().ToAngle();
		}
		else if (this.shootDirection == BashelliskBodyController.ShootDirection.Head)
		{
			num = this.m_head.aiAnimator.FacingDirection;
		}
		else if (this.shootDirection == BashelliskBodyController.ShootDirection.Average)
		{
			float num2 = 0f;
			int num3 = 0;
			BraveMathCollege.WeightedAverage(this.m_head.aiAnimator.FacingDirection, ref num2, ref num3);
			for (LinkedListNode<BashelliskSegment> linkedListNode = this.m_head.Body.First.Next; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				BraveMathCollege.WeightedAverage(((BashelliskBodyController)linkedListNode.Value).BaseShootDirection, ref num2, ref num3);
			}
			num = num2;
		}
		this.shootPoint.transform.rotation = Quaternion.Euler(0f, 0f, num);
	}

	// Token: 0x060057CE RID: 22478 RVA: 0x002181C4 File Offset: 0x002163C4
	public void OnBreak()
	{
		this.IsBroken = true;
		if (this.m_state != BashelliskBodyController.BodyState.Idle)
		{
			this.m_state = BashelliskBodyController.BodyState.Idle;
			if (this.m_bulletSource)
			{
				this.m_bulletSource.ForceStop();
			}
		}
		base.aiAnimator.SetBaseAnim("broken", false);
		base.aiAnimator.EndAnimation();
	}

	// Token: 0x060057CF RID: 22479 RVA: 0x00218224 File Offset: 0x00216424
	public override void UpdatePosition(PooledLinkedList<Vector2> path, LinkedListNode<Vector2> pathNode, float totalPathDist, float thisNodeDist)
	{
		float num = this.PathDist - this.previous.PathDist;
		float num2 = this.previous.attachRadius + this.attachRadius;
		bool flag = false;
		if (num < num2)
		{
			num2 = num;
			flag = true;
		}
		float num3 = -thisNodeDist;
		while (pathNode.Next != null)
		{
			float num4 = Vector2.Distance(pathNode.Next.Value, pathNode.Value);
			if (num3 + num4 >= num2)
			{
				float num5 = num2 - num3;
				if (!flag)
				{
					Vector2 vector = Vector2.Lerp(pathNode.Value, pathNode.Next.Value, num5 / num4);
					base.transform.position = vector - this.center.localPosition;
					base.specRigidbody.Reinitialize();
				}
				base.sprite.UpdateZDepth();
				this.BaseShootDirection = (this.previous.center.position - this.center.position).XY().ToAngle();
				this.PathDist = totalPathDist + num2;
				if (this.next)
				{
					this.next.UpdatePosition(path, pathNode, totalPathDist + num2, num5);
				}
				else
				{
					while (path.Last != pathNode.Next)
					{
						path.RemoveLast();
					}
				}
				this.UpdateShootDirection();
				return;
			}
			num3 += num4;
			pathNode = pathNode.Next;
		}
	}

	// Token: 0x17000C8D RID: 3213
	// (get) Token: 0x060057D0 RID: 22480 RVA: 0x0021838C File Offset: 0x0021658C
	// (set) Token: 0x060057D1 RID: 22481 RVA: 0x00218394 File Offset: 0x00216594
	private BashelliskBodyController.BodyState State
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

	// Token: 0x060057D2 RID: 22482 RVA: 0x002183B8 File Offset: 0x002165B8
	private void BeginState(BashelliskBodyController.BodyState state)
	{
		if (state == BashelliskBodyController.BodyState.Intro)
		{
			base.aiAnimator.PlayUntilCancelled("gun_out", false, null, -1f, false);
		}
		else if (state == BashelliskBodyController.BodyState.Shooting)
		{
			if (!this.m_bulletSource)
			{
				this.m_bulletSource = this.shootPoint.GetOrAddComponent<BulletScriptSource>();
			}
			this.m_bulletSource.BulletManager = this.m_head.bulletBank;
			this.m_bulletSource.BulletScript = this.m_bulletScript;
			this.m_bulletSource.Initialize();
		}
		else if (state == BashelliskBodyController.BodyState.Outro)
		{
			base.aiAnimator.PlayUntilFinished("gun_in", false, null, -1f, false);
		}
	}

	// Token: 0x060057D3 RID: 22483 RVA: 0x0021846C File Offset: 0x0021666C
	private void UpdateState()
	{
		if (this.State == BashelliskBodyController.BodyState.Intro)
		{
			if (!base.aiAnimator.IsPlaying("gun_out"))
			{
				this.State = BashelliskBodyController.BodyState.Shooting;
			}
		}
		else if (this.State == BashelliskBodyController.BodyState.Shooting)
		{
			if (this.m_bulletSource.IsEnded)
			{
				this.State = BashelliskBodyController.BodyState.Outro;
			}
		}
		else if (this.State == BashelliskBodyController.BodyState.Outro && !base.aiAnimator.IsPlaying("gun_in"))
		{
			this.State = BashelliskBodyController.BodyState.Idle;
		}
	}

	// Token: 0x060057D4 RID: 22484 RVA: 0x002184F8 File Offset: 0x002166F8
	private void EndState(BashelliskBodyController.BodyState state)
	{
	}

	// Token: 0x040050C2 RID: 20674
	public GameObject shootPoint;

	// Token: 0x040050C6 RID: 20678
	private BashelliskHeadController m_head;

	// Token: 0x040050C7 RID: 20679
	private BashelliskBodyController.BodyState m_state;

	// Token: 0x040050C8 RID: 20680
	private BulletScriptSelector m_bulletScript;

	// Token: 0x040050C9 RID: 20681
	private BulletScriptSource m_bulletSource;

	// Token: 0x02000FBE RID: 4030
	public enum ShootDirection
	{
		// Token: 0x040050CB RID: 20683
		NextSegment,
		// Token: 0x040050CC RID: 20684
		Head,
		// Token: 0x040050CD RID: 20685
		Average
	}

	// Token: 0x02000FBF RID: 4031
	private enum BodyState
	{
		// Token: 0x040050CF RID: 20687
		Idle,
		// Token: 0x040050D0 RID: 20688
		Intro,
		// Token: 0x040050D1 RID: 20689
		Shooting,
		// Token: 0x040050D2 RID: 20690
		Outro
	}
}
