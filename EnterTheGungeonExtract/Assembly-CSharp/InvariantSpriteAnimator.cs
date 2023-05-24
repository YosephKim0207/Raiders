using System;
using UnityEngine;

// Token: 0x02001330 RID: 4912
[RequireComponent(typeof(tk2dSpriteAnimator))]
public class InvariantSpriteAnimator : BraveBehaviour
{
	// Token: 0x06006F5C RID: 28508 RVA: 0x002C25CC File Offset: 0x002C07CC
	public void Awake()
	{
		base.spriteAnimator.ignoreTimeScale = true;
	}
}
