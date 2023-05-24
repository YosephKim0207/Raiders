using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C3B RID: 3131
	[Tooltip("Get the maximum characters number of a TextMesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshGetMaxChars : FsmStateAction
	{
		// Token: 0x0600437A RID: 17274 RVA: 0x0015D294 File Offset: 0x0015B494
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x0015D2CC File Offset: 0x0015B4CC
		public override void Reset()
		{
			this.gameObject = null;
			this.maxChars = null;
			this.everyframe = false;
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x0015D2E4 File Offset: 0x0015B4E4
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoGetMaxChars();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x0600437D RID: 17277 RVA: 0x0015D304 File Offset: 0x0015B504
		public override void OnUpdate()
		{
			this.DoGetMaxChars();
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x0015D30C File Offset: 0x0015B50C
		private void DoGetMaxChars()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: ");
				return;
			}
			this.maxChars.Value = this._textMesh.maxChars;
		}

		// Token: 0x04003595 RID: 13717
		[CheckForComponent(typeof(tk2dTextMesh))]
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04003596 RID: 13718
		[UIHint(UIHint.Variable)]
		[Tooltip("The max number of characters")]
		public FsmInt maxChars;

		// Token: 0x04003597 RID: 13719
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x04003598 RID: 13720
		private tk2dTextMesh _textMesh;
	}
}
