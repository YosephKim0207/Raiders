using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FC9 RID: 4041
public abstract class BashelliskSegment : BraveBehaviour
{
	// Token: 0x06005810 RID: 22544 RVA: 0x0021A0B0 File Offset: 0x002182B0
	public virtual void UpdatePosition(PooledLinkedList<Vector2> path, LinkedListNode<Vector2> pathNode, float totalPathDist, float thisNodeDist)
	{
	}

	// Token: 0x06005811 RID: 22545 RVA: 0x0021A0B4 File Offset: 0x002182B4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400510F RID: 20751
	public Transform center;

	// Token: 0x04005110 RID: 20752
	public float attachRadius;

	// Token: 0x04005111 RID: 20753
	[NonSerialized]
	public BashelliskSegment next;

	// Token: 0x04005112 RID: 20754
	[NonSerialized]
	public BashelliskSegment previous;

	// Token: 0x04005113 RID: 20755
	[NonSerialized]
	public float PathDist;
}
