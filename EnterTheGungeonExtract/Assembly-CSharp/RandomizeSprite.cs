using System;
using System.Collections.Generic;

// Token: 0x02001696 RID: 5782
public class RandomizeSprite : BraveBehaviour
{
	// Token: 0x060086C9 RID: 34505 RVA: 0x0037DEF8 File Offset: 0x0037C0F8
	public void Start()
	{
		if (this.UseStaticIndex)
		{
			if (this.spriteNames.Count > 0)
			{
				base.sprite.SetSprite(this.spriteNames[RandomizeSprite.s_index % this.spriteNames.Count]);
			}
			if (this.animationNames.Count > 0)
			{
				base.spriteAnimator.Play(this.animationNames[RandomizeSprite.s_index % this.animationNames.Count]);
			}
			RandomizeSprite.s_index++;
			if (RandomizeSprite.s_index < 0)
			{
				RandomizeSprite.s_index = 0;
			}
		}
		else
		{
			if (this.spriteNames.Count > 0)
			{
				base.sprite.SetSprite(BraveUtility.RandomElement<string>(this.spriteNames));
			}
			if (this.animationNames.Count > 0)
			{
				base.spriteAnimator.Play(BraveUtility.RandomElement<string>(this.animationNames));
			}
		}
	}

	// Token: 0x060086CA RID: 34506 RVA: 0x0037DFF4 File Offset: 0x0037C1F4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008BF7 RID: 35831
	[CheckSprite(null)]
	public List<string> spriteNames;

	// Token: 0x04008BF8 RID: 35832
	[CheckAnimation(null)]
	public List<string> animationNames;

	// Token: 0x04008BF9 RID: 35833
	public bool UseStaticIndex;

	// Token: 0x04008BFA RID: 35834
	private static int s_index;
}
