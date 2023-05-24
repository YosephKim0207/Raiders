using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000965 RID: 2405
	[Tooltip("Sends an Event based on a Game Object's Tag.")]
	[ActionCategory(ActionCategory.Logic)]
	public class GameObjectTagSwitch : FsmStateAction
	{
		// Token: 0x06003476 RID: 13430 RVA: 0x001101E0 File Offset: 0x0010E3E0
		public override void Reset()
		{
			this.gameObject = null;
			this.compareTo = new FsmString[1];
			this.sendEvent = new FsmEvent[1];
			this.everyFrame = false;
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x00110208 File Offset: 0x0010E408
		public override void OnEnter()
		{
			this.DoTagSwitch();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x00110224 File Offset: 0x0010E424
		public override void OnUpdate()
		{
			this.DoTagSwitch();
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x0011022C File Offset: 0x0010E42C
		private void DoTagSwitch()
		{
			GameObject value = this.gameObject.Value;
			if (value == null)
			{
				return;
			}
			for (int i = 0; i < this.compareTo.Length; i++)
			{
				if (value.tag == this.compareTo[i].Value)
				{
					base.Fsm.Event(this.sendEvent[i]);
					return;
				}
			}
		}

		// Token: 0x040025A3 RID: 9635
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The GameObject to test.")]
		public FsmGameObject gameObject;

		// Token: 0x040025A4 RID: 9636
		[CompoundArray("Tag Switches", "Compare Tag", "Send Event")]
		[UIHint(UIHint.Tag)]
		public FsmString[] compareTo;

		// Token: 0x040025A5 RID: 9637
		public FsmEvent[] sendEvent;

		// Token: 0x040025A6 RID: 9638
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
