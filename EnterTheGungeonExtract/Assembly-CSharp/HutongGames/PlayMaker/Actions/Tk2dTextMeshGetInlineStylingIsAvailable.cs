using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C3A RID: 3130
	[Tooltip("Check that inline styling can indeed be used ( the font needs to have texture gradients for inline styling to work). \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshGetInlineStylingIsAvailable : FsmStateAction
	{
		// Token: 0x06004374 RID: 17268 RVA: 0x0015D1BC File Offset: 0x0015B3BC
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x0015D1F4 File Offset: 0x0015B3F4
		public override void Reset()
		{
			this.gameObject = null;
			this.InlineStylingAvailable = null;
			this.everyframe = false;
		}

		// Token: 0x06004376 RID: 17270 RVA: 0x0015D20C File Offset: 0x0015B40C
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoGetInlineStylingAvailable();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x06004377 RID: 17271 RVA: 0x0015D22C File Offset: 0x0015B42C
		public override void OnUpdate()
		{
			this.DoGetInlineStylingAvailable();
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x0015D234 File Offset: 0x0015B434
		private void DoGetInlineStylingAvailable()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: ");
				return;
			}
			this.InlineStylingAvailable.Value = this._textMesh.inlineStyling && this._textMesh.font.textureGradients;
		}

		// Token: 0x04003591 RID: 13713
		[RequiredField]
		[CheckForComponent(typeof(tk2dTextMesh))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04003592 RID: 13714
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("Is inline styling available? true if inlineStyling is true AND the font texturGradients is true")]
		public FsmBool InlineStylingAvailable;

		// Token: 0x04003593 RID: 13715
		[ActionSection("")]
		[Tooltip("Repeat every frame.")]
		public bool everyframe;

		// Token: 0x04003594 RID: 13716
		private tk2dTextMesh _textMesh;
	}
}
