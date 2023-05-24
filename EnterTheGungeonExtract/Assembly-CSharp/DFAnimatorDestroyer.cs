using System;
using UnityEngine;

// Token: 0x0200175C RID: 5980
public class DFAnimatorDestroyer : MonoBehaviour
{
	// Token: 0x06008B34 RID: 35636 RVA: 0x0039F59C File Offset: 0x0039D79C
	private void Start()
	{
		this.m_animator = base.GetComponent<dfSpriteAnimation>();
	}

	// Token: 0x06008B35 RID: 35637 RVA: 0x0039F5AC File Offset: 0x0039D7AC
	private void Update()
	{
		if (!this.m_animator.IsPlaying && !this.m_animator.AutoRun)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (this.m_animator.IsPlaying)
		{
			this.m_animator.AutoRun = false;
		}
	}

	// Token: 0x04009203 RID: 37379
	protected dfSpriteAnimation m_animator;
}
