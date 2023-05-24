using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A45 RID: 2629
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Set the log level for network messages. Default is Off.\n\nOff: Only report errors, otherwise silent.\n\nInformational: Report informational messages like connectivity events.\n\nFull: Full debug level logging down to each individual message being reported.")]
	public class NetworkSetLogLevel : FsmStateAction
	{
		// Token: 0x06003808 RID: 14344 RVA: 0x0011FEC4 File Offset: 0x0011E0C4
		public override void Reset()
		{
			this.logLevel = NetworkLogLevel.Off;
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x0011FED0 File Offset: 0x0011E0D0
		public override void OnEnter()
		{
			Network.logLevel = this.logLevel;
			base.Finish();
		}

		// Token: 0x04002A12 RID: 10770
		[Tooltip("The log level")]
		public NetworkLogLevel logLevel;
	}
}
