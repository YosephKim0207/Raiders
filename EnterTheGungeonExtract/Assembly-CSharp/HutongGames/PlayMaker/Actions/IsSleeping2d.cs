using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A62 RID: 2658
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Tests if a Game Object's Rigidbody 2D is sleeping.")]
	public class IsSleeping2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x0600388D RID: 14477 RVA: 0x001221FC File Offset: 0x001203FC
		public override void Reset()
		{
			this.gameObject = null;
			this.trueEvent = null;
			this.falseEvent = null;
			this.store = null;
			this.everyFrame = false;
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x00122224 File Offset: 0x00120424
		public override void OnEnter()
		{
			this.DoIsSleeping();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x00122240 File Offset: 0x00120440
		public override void OnUpdate()
		{
			this.DoIsSleeping();
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x00122248 File Offset: 0x00120448
		private void DoIsSleeping()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			bool flag = base.rigidbody2d.IsSleeping();
			this.store.Value = flag;
			base.Fsm.Event((!flag) ? this.falseEvent : this.trueEvent);
		}

		// Token: 0x04002AB9 RID: 10937
		[Tooltip("The GameObject with the Rigidbody2D attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002ABA RID: 10938
		[Tooltip("Event sent if sleeping")]
		public FsmEvent trueEvent;

		// Token: 0x04002ABB RID: 10939
		[Tooltip("Event sent if not sleeping")]
		public FsmEvent falseEvent;

		// Token: 0x04002ABC RID: 10940
		[Tooltip("Store the value in a Boolean variable")]
		[UIHint(UIHint.Variable)]
		public FsmBool store;

		// Token: 0x04002ABD RID: 10941
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
