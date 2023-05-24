using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A61 RID: 2657
	[Tooltip("Tests if a Game Object's Rigid Body 2D is Kinematic.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class IsKinematic2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x06003888 RID: 14472 RVA: 0x00122140 File Offset: 0x00120340
		public override void Reset()
		{
			this.gameObject = null;
			this.trueEvent = null;
			this.falseEvent = null;
			this.store = null;
			this.everyFrame = false;
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x00122168 File Offset: 0x00120368
		public override void OnEnter()
		{
			this.DoIsKinematic();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x00122184 File Offset: 0x00120384
		public override void OnUpdate()
		{
			this.DoIsKinematic();
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x0012218C File Offset: 0x0012038C
		private void DoIsKinematic()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			bool isKinematic = base.rigidbody2d.isKinematic;
			this.store.Value = isKinematic;
			base.Fsm.Event((!isKinematic) ? this.falseEvent : this.trueEvent);
		}

		// Token: 0x04002AB4 RID: 10932
		[Tooltip("the GameObject with a Rigidbody2D attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002AB5 RID: 10933
		[Tooltip("Event Sent if Kinematic")]
		public FsmEvent trueEvent;

		// Token: 0x04002AB6 RID: 10934
		[Tooltip("Event sent if not Kinematic")]
		public FsmEvent falseEvent;

		// Token: 0x04002AB7 RID: 10935
		[Tooltip("Store the Kinematic state")]
		[UIHint(UIHint.Variable)]
		public FsmBool store;

		// Token: 0x04002AB8 RID: 10936
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
