using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C3E RID: 3134
	[Tooltip("Get the textMesh properties in one go just for convenience. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshGetProperties : FsmStateAction
	{
		// Token: 0x06004389 RID: 17289 RVA: 0x0015D424 File Offset: 0x0015B624
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x0015D45C File Offset: 0x0015B65C
		public override void Reset()
		{
			this.gameObject = null;
			this.text = null;
			this.inlineStyling = null;
			this.textureGradient = null;
			this.mainColor = null;
			this.gradientColor = null;
			this.useGradient = null;
			this.anchor = null;
			this.scale = null;
			this.kerning = null;
			this.maxChars = null;
			this.NumDrawnCharacters = null;
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x0015D4C0 File Offset: 0x0015B6C0
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoGetProperties();
			base.Finish();
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x0015D4D4 File Offset: 0x0015B6D4
		private void DoGetProperties()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: " + this._textMesh.gameObject.name);
				return;
			}
			this.text.Value = this._textMesh.text;
			this.inlineStyling.Value = this._textMesh.inlineStyling;
			this.textureGradient.Value = this._textMesh.textureGradient;
			this.mainColor.Value = this._textMesh.color;
			this.gradientColor.Value = this._textMesh.color2;
			this.useGradient.Value = this._textMesh.useGradient;
			this.anchor.Value = this._textMesh.anchor.ToString();
			this.scale.Value = this._textMesh.scale;
			this.kerning.Value = this._textMesh.kerning;
			this.maxChars.Value = this._textMesh.maxChars;
			this.NumDrawnCharacters.Value = this._textMesh.NumDrawnCharacters();
			this.textureGradient.Value = this._textMesh.textureGradient;
		}

		// Token: 0x0400359F RID: 13727
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[CheckForComponent(typeof(tk2dTextMesh))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035A0 RID: 13728
		[Tooltip("The Text")]
		[UIHint(UIHint.Variable)]
		public FsmString text;

		// Token: 0x040035A1 RID: 13729
		[UIHint(UIHint.Variable)]
		[Tooltip("InlineStyling")]
		public FsmBool inlineStyling;

		// Token: 0x040035A2 RID: 13730
		[Tooltip("Anchor")]
		[UIHint(UIHint.Variable)]
		public FsmString anchor;

		// Token: 0x040035A3 RID: 13731
		[UIHint(UIHint.Variable)]
		[Tooltip("Kerning")]
		public FsmBool kerning;

		// Token: 0x040035A4 RID: 13732
		[Tooltip("maxChars")]
		[UIHint(UIHint.Variable)]
		public FsmInt maxChars;

		// Token: 0x040035A5 RID: 13733
		[UIHint(UIHint.Variable)]
		[Tooltip("number of drawn characters")]
		public FsmInt NumDrawnCharacters;

		// Token: 0x040035A6 RID: 13734
		[Tooltip("The Main Color")]
		[UIHint(UIHint.Variable)]
		public FsmColor mainColor;

		// Token: 0x040035A7 RID: 13735
		[Tooltip("The Gradient Color. Only used if gradient is true")]
		[UIHint(UIHint.Variable)]
		public FsmColor gradientColor;

		// Token: 0x040035A8 RID: 13736
		[UIHint(UIHint.Variable)]
		[Tooltip("Use gradient")]
		public FsmBool useGradient;

		// Token: 0x040035A9 RID: 13737
		[Tooltip("Texture gradient")]
		[UIHint(UIHint.Variable)]
		public FsmInt textureGradient;

		// Token: 0x040035AA RID: 13738
		[Tooltip("Scale")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 scale;

		// Token: 0x040035AB RID: 13739
		private tk2dTextMesh _textMesh;
	}
}
