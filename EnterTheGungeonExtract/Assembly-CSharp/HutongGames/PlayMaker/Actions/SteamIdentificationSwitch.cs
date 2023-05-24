using System;
using Steamworks;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C6F RID: 3183
	[ActionCategory(ActionCategory.Logic)]
	public class SteamIdentificationSwitch : FsmStateAction
	{
		// Token: 0x0600446A RID: 17514 RVA: 0x00161CE4 File Offset: 0x0015FEE4
		public override void Reset()
		{
			this.targetIDs = new FsmString[1];
			this.sendEvent = new FsmEvent[1];
			this.everyFrame = false;
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x00161D08 File Offset: 0x0015FF08
		public override void OnEnter()
		{
			this.DoIDSwitch();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x00161D24 File Offset: 0x0015FF24
		public override void OnUpdate()
		{
			this.DoIDSwitch();
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x00161D2C File Offset: 0x0015FF2C
		private void DoIDSwitch()
		{
			bool flag = false;
			ulong num = 0UL;
			if (GameManager.Instance.platformInterface is PlatformInterfaceSteam && SteamManager.Initialized)
			{
				num = SteamUser.GetSteamID().m_SteamID;
				flag = true;
			}
			if (flag)
			{
				for (int i = 0; i < this.targetIDs.Length; i++)
				{
					if (this.targetIDs[i].Value == num.ToString())
					{
						base.Fsm.Event(this.sendEvent[i]);
						return;
					}
				}
			}
			base.Fsm.Event(this.defaultEvent);
		}

		// Token: 0x04003679 RID: 13945
		[CompoundArray("Int Switches", "Compare Int", "Send Event")]
		public FsmString[] targetIDs;

		// Token: 0x0400367A RID: 13946
		public FsmEvent[] sendEvent;

		// Token: 0x0400367B RID: 13947
		public bool everyFrame;

		// Token: 0x0400367C RID: 13948
		public FsmEvent defaultEvent;
	}
}
