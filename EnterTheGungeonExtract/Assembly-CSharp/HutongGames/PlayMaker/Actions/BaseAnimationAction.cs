using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008F6 RID: 2294
	public abstract class BaseAnimationAction : ComponentAction<Animation>
	{
		// Token: 0x0600329B RID: 12955 RVA: 0x00109F5C File Offset: 0x0010815C
		public override void OnActionTargetInvoked(object targetObject)
		{
			AnimationClip animationClip = targetObject as AnimationClip;
			if (animationClip == null)
			{
				return;
			}
			Animation component = base.Owner.GetComponent<Animation>();
			if (component != null)
			{
				component.AddClip(animationClip, animationClip.name);
			}
		}
	}
}
