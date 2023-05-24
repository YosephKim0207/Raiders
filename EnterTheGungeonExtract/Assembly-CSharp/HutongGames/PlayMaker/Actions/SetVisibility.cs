using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B1D RID: 2845
	[Tooltip("Sets the visibility of a GameObject. Note: this action sets the GameObject Renderer's enabled state.")]
	[ActionCategory(ActionCategory.Material)]
	public class SetVisibility : ComponentAction<Renderer>
	{
		// Token: 0x06003BF3 RID: 15347 RVA: 0x0012DD68 File Offset: 0x0012BF68
		public override void Reset()
		{
			this.gameObject = null;
			this.toggle = false;
			this.visible = false;
			this.resetOnExit = true;
			this.initialVisibility = false;
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x0012DD98 File Offset: 0x0012BF98
		public override void OnEnter()
		{
			this.DoSetVisibility(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
			base.Finish();
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x0012DDB8 File Offset: 0x0012BFB8
		private void DoSetVisibility(GameObject go)
		{
			if (!base.UpdateCache(go))
			{
				return;
			}
			this.initialVisibility = base.renderer.enabled;
			if (!this.toggle.Value)
			{
				base.renderer.enabled = this.visible.Value;
				return;
			}
			base.renderer.enabled = !base.renderer.enabled;
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x0012DE24 File Offset: 0x0012C024
		public override void OnExit()
		{
			if (this.resetOnExit)
			{
				this.ResetVisibility();
			}
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x0012DE38 File Offset: 0x0012C038
		private void ResetVisibility()
		{
			if (base.renderer != null)
			{
				base.renderer.enabled = this.initialVisibility;
			}
		}

		// Token: 0x04002E12 RID: 11794
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002E13 RID: 11795
		[Tooltip("Should the object visibility be toggled?\nHas priority over the 'visible' setting")]
		public FsmBool toggle;

		// Token: 0x04002E14 RID: 11796
		[Tooltip("Should the object be set to visible or invisible?")]
		public FsmBool visible;

		// Token: 0x04002E15 RID: 11797
		[Tooltip("Resets to the initial visibility when it leaves the state")]
		public bool resetOnExit;

		// Token: 0x04002E16 RID: 11798
		private bool initialVisibility;
	}
}
