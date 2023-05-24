using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000964 RID: 2404
	[Tooltip("Tests if a Game Object is visible.")]
	[ActionTarget(typeof(GameObject), "gameObject", false)]
	[ActionCategory(ActionCategory.Logic)]
	public class GameObjectIsVisible : ComponentAction<Renderer>
	{
		// Token: 0x06003471 RID: 13425 RVA: 0x00110124 File Offset: 0x0010E324
		public override void Reset()
		{
			this.gameObject = null;
			this.trueEvent = null;
			this.falseEvent = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003472 RID: 13426 RVA: 0x0011014C File Offset: 0x0010E34C
		public override void OnEnter()
		{
			this.DoIsVisible();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x00110168 File Offset: 0x0010E368
		public override void OnUpdate()
		{
			this.DoIsVisible();
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x00110170 File Offset: 0x0010E370
		private void DoIsVisible()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				bool isVisible = base.renderer.isVisible;
				this.storeResult.Value = isVisible;
				base.Fsm.Event((!isVisible) ? this.falseEvent : this.trueEvent);
			}
		}

		// Token: 0x0400259E RID: 9630
		[Tooltip("The GameObject to test.")]
		[CheckForComponent(typeof(Renderer))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400259F RID: 9631
		[Tooltip("Event to send if the GameObject is visible.")]
		public FsmEvent trueEvent;

		// Token: 0x040025A0 RID: 9632
		[Tooltip("Event to send if the GameObject is NOT visible.")]
		public FsmEvent falseEvent;

		// Token: 0x040025A1 RID: 9633
		[Tooltip("Store the result in a bool variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x040025A2 RID: 9634
		public bool everyFrame;
	}
}
