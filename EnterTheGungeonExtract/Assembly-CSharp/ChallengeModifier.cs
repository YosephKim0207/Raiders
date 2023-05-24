using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200126B RID: 4715
public abstract class ChallengeModifier : MonoBehaviour
{
	// Token: 0x060069A2 RID: 27042 RVA: 0x002965E8 File Offset: 0x002947E8
	public void ShatterIcon(dfAnimationClip ChallengeBurstClip)
	{
		if (!this.IconShattered && this.IconSprite)
		{
			this.IconShattered = true;
			dfSpriteAnimation dfSpriteAnimation = this.IconSprite.gameObject.AddComponent<dfSpriteAnimation>();
			dfSpriteAnimation.Target = new dfComponentMemberInfo();
			dfComponentMemberInfo target = dfSpriteAnimation.Target;
			target.Component = this.IconSprite;
			target.MemberName = "SpriteName";
			dfSpriteAnimation.Clip = ChallengeBurstClip;
			dfSpriteAnimation.Length = 0.2f;
			dfSpriteAnimation.LoopType = dfTweenLoopType.Once;
			dfSpriteAnimation.Play();
			UnityEngine.Object.Destroy(this.IconLabel.gameObject, 0.2f);
			UnityEngine.Object.Destroy(this.IconSprite.gameObject, 0.2f);
		}
	}

	// Token: 0x060069A3 RID: 27043 RVA: 0x0029669C File Offset: 0x0029489C
	public virtual bool IsValid(RoomHandler room)
	{
		return true;
	}

	// Token: 0x04006610 RID: 26128
	[SerializeField]
	public string DisplayName;

	// Token: 0x04006611 RID: 26129
	[SerializeField]
	public string AlternateLanguageDisplayName;

	// Token: 0x04006612 RID: 26130
	[SerializeField]
	public string AtlasSpriteName;

	// Token: 0x04006613 RID: 26131
	[SerializeField]
	public bool ValidInBossChambers = true;

	// Token: 0x04006614 RID: 26132
	[SerializeField]
	public List<ChallengeModifier> MutuallyExclusive;

	// Token: 0x04006615 RID: 26133
	[NonSerialized]
	public dfSprite IconSprite;

	// Token: 0x04006616 RID: 26134
	[NonSerialized]
	public dfLabel IconLabel;

	// Token: 0x04006617 RID: 26135
	[NonSerialized]
	public bool IconShattered;
}
