using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001241 RID: 4673
public class TonicWalletController : MonoBehaviour
{
	// Token: 0x060068B8 RID: 26808 RVA: 0x0029033C File Offset: 0x0028E53C
	private IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.TONIC_IS_LOADED))
		{
			AIAnimator component = base.GetComponent<AIAnimator>();
			component.IdleAnimation.AnimNames[0] = "super_idle_right";
			component.IdleAnimation.AnimNames[1] = "super_idle_left";
			component.TalkAnimation.AnimNames[0] = "super_talk_right";
			component.TalkAnimation.AnimNames[1] = "super_talk_left";
			component.OtherAnimations[0].anim.AnimNames[0] = "super_bless_right";
			component.OtherAnimations[0].anim.AnimNames[1] = "super_bless_left";
			component.OtherAnimations[1].anim.AnimNames[0] = "super_cool_right";
			component.OtherAnimations[1].anim.AnimNames[1] = "super_cool_left";
		}
		yield break;
	}
}
