using System;
using Dungeonator;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C92 RID: 3218
	public class CheckRoomVisited : FsmStateAction
	{
		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x060044EB RID: 17643 RVA: 0x0016445C File Offset: 0x0016265C
		// (set) Token: 0x060044EC RID: 17644 RVA: 0x00164464 File Offset: 0x00162664
		public RoomHandler targetRoom
		{
			get
			{
				return this.m_targetRoom;
			}
			set
			{
				this.m_targetRoom = value;
			}
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x00164470 File Offset: 0x00162670
		public override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x00164478 File Offset: 0x00162678
		public override void OnEnter()
		{
			if (this.targetRoom != null)
			{
				if (this.targetRoom.visibility == RoomHandler.VisibilityStatus.OBSCURED)
				{
					base.Fsm.Event(this.HasNotVisited);
				}
				else
				{
					base.Fsm.Event(this.HasVisited);
				}
			}
			base.Finish();
		}

		// Token: 0x040036F6 RID: 14070
		[Tooltip("Event sent if there are.")]
		public FsmEvent HasVisited;

		// Token: 0x040036F7 RID: 14071
		[Tooltip("Event sent if there aren't.")]
		public FsmEvent HasNotVisited;

		// Token: 0x040036F8 RID: 14072
		private RoomHandler m_targetRoom;
	}
}
