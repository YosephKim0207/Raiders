using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A44 RID: 2628
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Set the level prefix which will then be prefixed to all network ViewID numbers.\n\nThis prevents old network updates from straying into a new level from the previous level.\n\nThis can be set to any number and then incremented with each new level load. This doesn't add overhead to network traffic but just diminishes the pool of network ViewID numbers a little bit.")]
	public class NetworkSetLevelPrefix : FsmStateAction
	{
		// Token: 0x06003805 RID: 14341 RVA: 0x0011FE7C File Offset: 0x0011E07C
		public override void Reset()
		{
			this.levelPrefix = null;
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x0011FE88 File Offset: 0x0011E088
		public override void OnEnter()
		{
			if (this.levelPrefix.IsNone)
			{
				base.LogError("Network LevelPrefix not set");
				return;
			}
			Network.SetLevelPrefix(this.levelPrefix.Value);
			base.Finish();
		}

		// Token: 0x04002A11 RID: 10769
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The level prefix which will then be prefixed to all network ViewID numbers.")]
		public FsmInt levelPrefix;
	}
}
