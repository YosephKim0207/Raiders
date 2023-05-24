using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B47 RID: 2887
	[Tooltip("Turn GUILayout on/off. If you don't use GUILayout actions you can get some performace back by turning GUILayout off. This can make a difference on iOS platforms.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class UseGUILayout : FsmStateAction
	{
		// Token: 0x06003CA1 RID: 15521 RVA: 0x00130948 File Offset: 0x0012EB48
		public override void Reset()
		{
			this.turnOffGUIlayout = true;
		}

		// Token: 0x06003CA2 RID: 15522 RVA: 0x00130954 File Offset: 0x0012EB54
		public override void OnEnter()
		{
			base.Fsm.Owner.useGUILayout = !this.turnOffGUIlayout;
			base.Finish();
		}

		// Token: 0x04002EEF RID: 12015
		[RequiredField]
		public bool turnOffGUIlayout;
	}
}
