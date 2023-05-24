using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A46 RID: 2630
	[Tooltip("Set the maximum amount of connections/players allowed.\n\nThis cannot be set higher than the connection count given in Launch Server.\n\nSetting it to 0 means no new connections can be made but the existing ones stay connected.\n\nSetting it to -1 means the maximum connections count is set to the same number of current open connections. In that case, if a players drops then the slot is still open for him.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkSetMaximumConnections : FsmStateAction
	{
		// Token: 0x0600380B RID: 14347 RVA: 0x0011FEEC File Offset: 0x0011E0EC
		public override void Reset()
		{
			this.maximumConnections = 32;
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x0011FEFC File Offset: 0x0011E0FC
		public override void OnEnter()
		{
			if (this.maximumConnections.Value < -1)
			{
				base.LogWarning("Network Maximum connections can not be less than -1");
				this.maximumConnections.Value = -1;
			}
			Network.maxConnections = this.maximumConnections.Value;
			base.Finish();
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x0011FF3C File Offset: 0x0011E13C
		public override string ErrorCheck()
		{
			if (this.maximumConnections.Value < -1)
			{
				return "Network Maximum connections can not be less than -1";
			}
			return string.Empty;
		}

		// Token: 0x04002A13 RID: 10771
		[Tooltip("The maximum amount of connections/players allowed.")]
		public FsmInt maximumConnections;
	}
}
