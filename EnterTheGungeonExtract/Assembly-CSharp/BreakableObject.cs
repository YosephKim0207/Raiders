using System;
using UnityEngine;

// Token: 0x02000E13 RID: 3603
public class BreakableObject : MonoBehaviour
{
	// Token: 0x06004C5F RID: 19551 RVA: 0x001A06D8 File Offset: 0x0019E8D8
	private void Start()
	{
		this.m_srb = base.GetComponent<SpeculativeRigidbody>();
		SpeculativeRigidbody srb = this.m_srb;
		srb.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(srb.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
	}

	// Token: 0x06004C60 RID: 19552 RVA: 0x001A0710 File Offset: 0x0019E910
	private void OnRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		this.Break();
	}

	// Token: 0x06004C61 RID: 19553 RVA: 0x001A0718 File Offset: 0x0019E918
	private void Break()
	{
		tk2dSpriteAnimator component = base.GetComponent<tk2dSpriteAnimator>();
		if (this.breakAnimName != string.Empty)
		{
			component.Play(this.breakAnimName);
		}
		else
		{
			component.Play();
		}
		UnityEngine.Object.Destroy(this.m_srb);
	}

	// Token: 0x0400423F RID: 16959
	public string breakAnimName = string.Empty;

	// Token: 0x04004240 RID: 16960
	private SpeculativeRigidbody m_srb;
}
