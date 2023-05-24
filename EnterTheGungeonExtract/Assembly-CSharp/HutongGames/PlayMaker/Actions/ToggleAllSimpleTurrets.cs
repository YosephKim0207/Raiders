using System;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CD8 RID: 3288
	public class ToggleAllSimpleTurrets : FsmStateAction
	{
		// Token: 0x060045D6 RID: 17878 RVA: 0x0016AB10 File Offset: 0x00168D10
		public override void Reset()
		{
			this.toggle = false;
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x0016AB20 File Offset: 0x00168D20
		public override void OnEnter()
		{
			List<SimpleTurretController> componentsAbsoluteInRoom = base.Owner.GetComponent<TalkDoerLite>().ParentRoom.GetComponentsAbsoluteInRoom<SimpleTurretController>();
			for (int i = 0; i < componentsAbsoluteInRoom.Count; i++)
			{
				if (this.toggle.Value)
				{
					componentsAbsoluteInRoom[i].ActivateManual();
				}
				else
				{
					componentsAbsoluteInRoom[i].DeactivateManual();
				}
			}
			base.Finish();
		}

		// Token: 0x0400381D RID: 14365
		public FsmBool toggle;
	}
}
