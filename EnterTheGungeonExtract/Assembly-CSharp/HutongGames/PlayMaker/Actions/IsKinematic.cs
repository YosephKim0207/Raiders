using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009F6 RID: 2550
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Tests if a Game Object's Rigid Body is Kinematic.")]
	public class IsKinematic : ComponentAction<Rigidbody>
	{
		// Token: 0x060036B7 RID: 14007 RVA: 0x00117628 File Offset: 0x00115828
		public override void Reset()
		{
			this.gameObject = null;
			this.trueEvent = null;
			this.falseEvent = null;
			this.store = null;
			this.everyFrame = false;
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x00117650 File Offset: 0x00115850
		public override void OnEnter()
		{
			this.DoIsKinematic();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x0011766C File Offset: 0x0011586C
		public override void OnUpdate()
		{
			this.DoIsKinematic();
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x00117674 File Offset: 0x00115874
		private void DoIsKinematic()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				bool isKinematic = base.rigidbody.isKinematic;
				this.store.Value = isKinematic;
				base.Fsm.Event((!isKinematic) ? this.falseEvent : this.trueEvent);
			}
		}

		// Token: 0x04002817 RID: 10263
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002818 RID: 10264
		public FsmEvent trueEvent;

		// Token: 0x04002819 RID: 10265
		public FsmEvent falseEvent;

		// Token: 0x0400281A RID: 10266
		[UIHint(UIHint.Variable)]
		public FsmBool store;

		// Token: 0x0400281B RID: 10267
		public bool everyFrame;
	}
}
