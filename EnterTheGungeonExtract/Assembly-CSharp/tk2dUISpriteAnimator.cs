using System;
using UnityEngine;

// Token: 0x02000C20 RID: 3104
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUISpriteAnimator")]
public class tk2dUISpriteAnimator : tk2dSpriteAnimator
{
	// Token: 0x060042DB RID: 17115 RVA: 0x0015AB3C File Offset: 0x00158D3C
	public override void LateUpdate()
	{
		base.UpdateAnimation(tk2dUITime.deltaTime);
	}

	// Token: 0x060042DC RID: 17116 RVA: 0x0015AB4C File Offset: 0x00158D4C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
