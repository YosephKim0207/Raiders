using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001005 RID: 4101
[RequireComponent(typeof(GenericIntroDoer))]
public class BulletKingIntroDoer : SpecificIntroDoer
{
	// Token: 0x060059B8 RID: 22968 RVA: 0x002243E8 File Offset: 0x002225E8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060059B9 RID: 22969 RVA: 0x002243F0 File Offset: 0x002225F0
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		BulletKingToadieController[] array = UnityEngine.Object.FindObjectsOfType<BulletKingToadieController>();
		for (int i = 0; i < array.Length; i++)
		{
			animators.Add(array[i].spriteAnimator);
			if (array[i].scepterAnimator)
			{
				animators.Add(array[i].scepterAnimator);
			}
		}
	}
}
