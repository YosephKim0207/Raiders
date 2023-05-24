using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A60 RID: 2656
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Is the rigidbody2D constrained from rotating?Note: Prefer SetRigidBody2dConstraints when working in Unity 5")]
	public class IsFixedAngle2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x06003883 RID: 14467 RVA: 0x0012207C File Offset: 0x0012027C
		public override void Reset()
		{
			this.gameObject = null;
			this.trueEvent = null;
			this.falseEvent = null;
			this.store = null;
			this.everyFrame = false;
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x001220A4 File Offset: 0x001202A4
		public override void OnEnter()
		{
			this.DoIsFixedAngle();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x001220C0 File Offset: 0x001202C0
		public override void OnUpdate()
		{
			this.DoIsFixedAngle();
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x001220C8 File Offset: 0x001202C8
		private void DoIsFixedAngle()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			bool flag = (base.rigidbody2d.constraints & RigidbodyConstraints2D.FreezeRotation) != RigidbodyConstraints2D.None;
			this.store.Value = flag;
			base.Fsm.Event((!flag) ? this.falseEvent : this.trueEvent);
		}

		// Token: 0x04002AAF RID: 10927
		[Tooltip("The GameObject with the Rigidbody2D attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002AB0 RID: 10928
		[Tooltip("Event sent if the Rigidbody2D does have fixed angle")]
		public FsmEvent trueEvent;

		// Token: 0x04002AB1 RID: 10929
		[Tooltip("Event sent if the Rigidbody2D doesn't have fixed angle")]
		public FsmEvent falseEvent;

		// Token: 0x04002AB2 RID: 10930
		[Tooltip("Store the fixedAngle flag")]
		[UIHint(UIHint.Variable)]
		public FsmBool store;

		// Token: 0x04002AB3 RID: 10931
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
