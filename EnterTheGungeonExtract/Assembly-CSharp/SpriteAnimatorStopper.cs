using System;
using System.Collections;
using UnityEngine;

// Token: 0x020016CB RID: 5835
public class SpriteAnimatorStopper : MonoBehaviour
{
	// Token: 0x060087C0 RID: 34752 RVA: 0x00384AA0 File Offset: 0x00382CA0
	private IEnumerator Start()
	{
		this.animator = base.GetComponent<tk2dSpriteAnimator>();
		yield return new WaitForSeconds(this.duration);
		this.animator.Stop();
		UnityEngine.Object.Destroy(this);
		yield break;
	}

	// Token: 0x04008CEF RID: 36079
	public float duration = 10f;

	// Token: 0x04008CF0 RID: 36080
	private tk2dSpriteAnimator animator;
}
