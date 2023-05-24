using System;
using UnityEngine;

// Token: 0x020016C9 RID: 5833
public class SpriteAnimatorChanger : MonoBehaviour
{
	// Token: 0x060087B0 RID: 34736 RVA: 0x003843D0 File Offset: 0x003825D0
	public void Awake()
	{
		this.m_animator = base.GetComponent<tk2dSpriteAnimator>();
	}

	// Token: 0x060087B1 RID: 34737 RVA: 0x003843E0 File Offset: 0x003825E0
	public void Update()
	{
		this.m_timer += BraveTime.DeltaTime;
		if (this.m_timer > this.time)
		{
			this.m_animator.Play(this.newAnimation);
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x04008CDD RID: 36061
	public float time;

	// Token: 0x04008CDE RID: 36062
	public string newAnimation;

	// Token: 0x04008CDF RID: 36063
	private tk2dSpriteAnimator m_animator;

	// Token: 0x04008CE0 RID: 36064
	private float m_timer;
}
