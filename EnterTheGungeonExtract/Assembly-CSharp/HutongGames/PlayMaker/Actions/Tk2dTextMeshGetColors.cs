using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C37 RID: 3127
	[Tooltip("Get the colors of a TextMesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshGetColors : FsmStateAction
	{
		// Token: 0x06004363 RID: 17251 RVA: 0x0015CF34 File Offset: 0x0015B134
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x0015CF6C File Offset: 0x0015B16C
		public override void Reset()
		{
			this.gameObject = null;
			this.mainColor = null;
			this.gradientColor = null;
			this.useGradient = false;
			this.everyframe = false;
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x0015CF98 File Offset: 0x0015B198
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoGetColors();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x06004366 RID: 17254 RVA: 0x0015CFB8 File Offset: 0x0015B1B8
		public override void OnUpdate()
		{
			this.DoGetColors();
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x0015CFC0 File Offset: 0x0015B1C0
		private void DoGetColors()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: ");
				return;
			}
			this.useGradient.Value = this._textMesh.useGradient;
			this.mainColor.Value = this._textMesh.color;
			this.gradientColor.Value = this._textMesh.color2;
		}

		// Token: 0x04003584 RID: 13700
		[RequiredField]
		[CheckForComponent(typeof(tk2dTextMesh))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04003585 RID: 13701
		[UIHint(UIHint.Variable)]
		[Tooltip("Main color")]
		public FsmColor mainColor;

		// Token: 0x04003586 RID: 13702
		[Tooltip("Gradient color. Only used if gradient is true")]
		[UIHint(UIHint.Variable)]
		public FsmColor gradientColor;

		// Token: 0x04003587 RID: 13703
		[Tooltip("Use gradient.")]
		[UIHint(UIHint.Variable)]
		public FsmBool useGradient;

		// Token: 0x04003588 RID: 13704
		[ActionSection("")]
		[Tooltip("Repeat every frame.")]
		public bool everyframe;

		// Token: 0x04003589 RID: 13705
		private tk2dTextMesh _textMesh;
	}
}
