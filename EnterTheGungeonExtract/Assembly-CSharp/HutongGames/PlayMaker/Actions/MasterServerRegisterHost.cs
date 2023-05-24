using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A27 RID: 2599
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Register this server on the master server.\n\nIf the master server address information has not been changed the default Unity master server will be used.")]
	public class MasterServerRegisterHost : FsmStateAction
	{
		// Token: 0x0600379A RID: 14234 RVA: 0x0011E9AC File Offset: 0x0011CBAC
		public override void Reset()
		{
			this.gameTypeName = null;
			this.gameName = null;
			this.comment = null;
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x0011E9C4 File Offset: 0x0011CBC4
		public override void OnEnter()
		{
			this.DoMasterServerRegisterHost();
			base.Finish();
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x0011E9D4 File Offset: 0x0011CBD4
		private void DoMasterServerRegisterHost()
		{
			MasterServer.RegisterHost(this.gameTypeName.Value, this.gameName.Value, this.comment.Value);
		}

		// Token: 0x040029A0 RID: 10656
		[Tooltip("The unique game type name.")]
		[RequiredField]
		public FsmString gameTypeName;

		// Token: 0x040029A1 RID: 10657
		[Tooltip("The game name.")]
		[RequiredField]
		public FsmString gameName;

		// Token: 0x040029A2 RID: 10658
		[Tooltip("Optional comment")]
		public FsmString comment;
	}
}
