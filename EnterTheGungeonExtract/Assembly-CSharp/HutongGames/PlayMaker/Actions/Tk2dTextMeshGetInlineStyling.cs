using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C39 RID: 3129
	[ActionCategory("2D Toolkit/TextMesh")]
	[Tooltip("Get the inline styling flag of a TextMesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	public class Tk2dTextMeshGetInlineStyling : FsmStateAction
	{
		// Token: 0x0600436E RID: 17262 RVA: 0x0015D104 File Offset: 0x0015B304
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x0015D13C File Offset: 0x0015B33C
		public override void Reset()
		{
			this.gameObject = null;
			this.inlineStyling = null;
			this.everyframe = false;
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x0015D154 File Offset: 0x0015B354
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoGetInlineStyling();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x0015D174 File Offset: 0x0015B374
		public override void OnUpdate()
		{
			this.DoGetInlineStyling();
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x0015D17C File Offset: 0x0015B37C
		private void DoGetInlineStyling()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: ");
				return;
			}
			this.inlineStyling.Value = this._textMesh.inlineStyling;
		}

		// Token: 0x0400358D RID: 13709
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[RequiredField]
		[CheckForComponent(typeof(tk2dTextMesh))]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400358E RID: 13710
		[RequiredField]
		[Tooltip("The max number of characters")]
		[UIHint(UIHint.Variable)]
		public FsmBool inlineStyling;

		// Token: 0x0400358F RID: 13711
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x04003590 RID: 13712
		private tk2dTextMesh _textMesh;
	}
}
