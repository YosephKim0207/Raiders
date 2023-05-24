using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009F7 RID: 2551
	[Tooltip("Tests if a Game Object's Rigid Body is sleeping.")]
	[ActionCategory(ActionCategory.Physics)]
	public class IsSleeping : ComponentAction<Rigidbody>
	{
		// Token: 0x060036BC RID: 14012 RVA: 0x001176E4 File Offset: 0x001158E4
		public override void Reset()
		{
			this.gameObject = null;
			this.trueEvent = null;
			this.falseEvent = null;
			this.store = null;
			this.everyFrame = false;
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x0011770C File Offset: 0x0011590C
		public override void OnEnter()
		{
			this.DoIsSleeping();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x00117728 File Offset: 0x00115928
		public override void OnUpdate()
		{
			this.DoIsSleeping();
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x00117730 File Offset: 0x00115930
		private void DoIsSleeping()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				bool flag = base.rigidbody.IsSleeping();
				this.store.Value = flag;
				base.Fsm.Event((!flag) ? this.falseEvent : this.trueEvent);
			}
		}

		// Token: 0x0400281C RID: 10268
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400281D RID: 10269
		public FsmEvent trueEvent;

		// Token: 0x0400281E RID: 10270
		public FsmEvent falseEvent;

		// Token: 0x0400281F RID: 10271
		[UIHint(UIHint.Variable)]
		public FsmBool store;

		// Token: 0x04002820 RID: 10272
		public bool everyFrame;
	}
}
