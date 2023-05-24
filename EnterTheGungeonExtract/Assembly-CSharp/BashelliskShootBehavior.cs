using System;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;

// Token: 0x02000D7D RID: 3453
[InspectorDropdownName("Bosses/Bashellisk/ShootBehavior")]
public class BashelliskShootBehavior : BasicAttackBehavior
{
	// Token: 0x06004927 RID: 18727 RVA: 0x00186340 File Offset: 0x00184540
	private bool ShowSegmentCount()
	{
		return this.SegmentPercentage <= 0f;
	}

	// Token: 0x06004928 RID: 18728 RVA: 0x00186354 File Offset: 0x00184554
	private bool ShowSegmentPercentage()
	{
		return this.SegmentCount <= 0;
	}

	// Token: 0x06004929 RID: 18729 RVA: 0x00186364 File Offset: 0x00184564
	public override void Start()
	{
		base.Start();
		this.m_bashellisk = this.m_aiActor.GetComponent<BashelliskHeadController>();
	}

	// Token: 0x0600492A RID: 18730 RVA: 0x00186380 File Offset: 0x00184580
	public override void Upkeep()
	{
		base.Upkeep();
		this.m_segmentDelayTimer -= this.m_deltaTime * this.m_behaviorSpeculator.CooldownScale;
	}

	// Token: 0x0600492B RID: 18731 RVA: 0x001863A8 File Offset: 0x001845A8
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
		this.SelectSegments();
		this.m_waitingToStart = false;
		if (this.WaitForAllSegmentsFree)
		{
			this.m_waitingToStart = true;
			this.m_segmentDelayTimer = 0f;
		}
		else if (this.SegmentDelay <= 0f)
		{
			while (this.m_nextSegmentNode != null)
			{
				this.FireNextSegment();
			}
		}
		else
		{
			this.FireNextSegment();
			this.m_segmentDelayTimer = this.SegmentDelay;
		}
		this.m_updateEveryFrame = true;
		return (!this.StopDuring) ? BehaviorResult.RunContinuousInClass : BehaviorResult.RunContinuous;
	}

	// Token: 0x0600492C RID: 18732 RVA: 0x00186458 File Offset: 0x00184658
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_waitingToStart)
		{
			for (LinkedListNode<BashelliskBodyController> linkedListNode = this.m_segments.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				if (linkedListNode.Value.IsShooting)
				{
					return ContinuousBehaviorResult.Continue;
				}
			}
			this.m_segmentDelayTimer = 0f;
			this.m_waitingToStart = false;
		}
		while (this.m_segmentDelayTimer <= 0f && this.m_nextSegmentNode != null)
		{
			this.m_segmentDelayTimer += this.SegmentDelay;
			this.FireNextSegment();
		}
		if (this.m_nextSegmentNode == null)
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600492D RID: 18733 RVA: 0x00186500 File Offset: 0x00184700
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x0600492E RID: 18734 RVA: 0x00186518 File Offset: 0x00184718
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x0600492F RID: 18735 RVA: 0x0018651C File Offset: 0x0018471C
	private void SelectSegments()
	{
		int num = this.SegmentCount;
		if (this.SegmentCount <= 0)
		{
			num = Mathf.RoundToInt(this.SegmentPercentage * (float)(this.m_bashellisk.Body.Count - 1));
		}
		num = Mathf.Max(1, num);
		this.m_segments.Clear();
		for (LinkedListNode<BashelliskSegment> linkedListNode = this.m_bashellisk.Body.First.Next; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			this.m_segments.AddLast(linkedListNode.Value as BashelliskBodyController);
		}
		if (this.segmentOrder == BashelliskShootBehavior.SegmentOrder.Sequence)
		{
			while (this.m_segments.Count > num)
			{
				this.m_segments.RemoveLast();
			}
		}
		else if (this.segmentOrder == BashelliskShootBehavior.SegmentOrder.Random)
		{
			for (int i = 0; i < num; i++)
			{
				int num2 = UnityEngine.Random.Range(i, this.m_segments.Count);
				LinkedListNode<BashelliskBodyController> byIndexSlow = this.m_segments.GetByIndexSlow(num2);
				this.m_segments.Remove(byIndexSlow, false);
				this.m_segments.AddFirst(byIndexSlow);
			}
			while (this.m_segments.Count > num)
			{
				this.m_segments.RemoveLast();
			}
		}
		else if (this.segmentOrder == BashelliskShootBehavior.SegmentOrder.RandomSequential)
		{
			while (this.m_segments.Count > num)
			{
				int num3 = UnityEngine.Random.Range(0, this.m_segments.Count);
				this.m_segments.Remove(this.m_segments.GetByIndexSlow(num3), true);
			}
		}
		for (LinkedListNode<BashelliskBodyController> linkedListNode2 = this.m_segments.First; linkedListNode2 != null; linkedListNode2 = linkedListNode2.Next)
		{
			linkedListNode2.Value.shootDirection = this.shootDirection;
			linkedListNode2.Value.UpdateShootDirection();
		}
		this.m_nextSegmentNode = this.m_segments.First;
	}

	// Token: 0x06004930 RID: 18736 RVA: 0x00186700 File Offset: 0x00184900
	private void FireNextSegment()
	{
		this.m_nextSegmentNode.Value.Fire(this.BulletScript);
		this.m_nextSegmentNode = this.m_nextSegmentNode.Next;
	}

	// Token: 0x04003D3D RID: 15677
	public BashelliskBodyController.ShootDirection shootDirection;

	// Token: 0x04003D3E RID: 15678
	public BulletScriptSelector BulletScript;

	// Token: 0x04003D3F RID: 15679
	public BashelliskShootBehavior.SegmentOrder segmentOrder = BashelliskShootBehavior.SegmentOrder.Sequence;

	// Token: 0x04003D40 RID: 15680
	[InspectorShowIf("ShowSegmentCount")]
	public int SegmentCount;

	// Token: 0x04003D41 RID: 15681
	[InspectorShowIf("ShowSegmentPercentage")]
	public float SegmentPercentage;

	// Token: 0x04003D42 RID: 15682
	public float SegmentDelay;

	// Token: 0x04003D43 RID: 15683
	public bool StopDuring;

	// Token: 0x04003D44 RID: 15684
	public bool WaitForAllSegmentsFree;

	// Token: 0x04003D45 RID: 15685
	private BashelliskHeadController m_bashellisk;

	// Token: 0x04003D46 RID: 15686
	private PooledLinkedList<BashelliskBodyController> m_segments = new PooledLinkedList<BashelliskBodyController>();

	// Token: 0x04003D47 RID: 15687
	private LinkedListNode<BashelliskBodyController> m_nextSegmentNode;

	// Token: 0x04003D48 RID: 15688
	private bool m_waitingToStart;

	// Token: 0x04003D49 RID: 15689
	private float m_segmentDelayTimer;

	// Token: 0x02000D7E RID: 3454
	public enum SegmentOrder
	{
		// Token: 0x04003D4B RID: 15691
		Sequence = 10,
		// Token: 0x04003D4C RID: 15692
		Random = 20,
		// Token: 0x04003D4D RID: 15693
		RandomSequential = 30
	}
}
