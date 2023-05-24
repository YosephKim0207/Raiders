using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C49 RID: 3145
	[ActionCategory("2D Toolkit/TextMesh")]
	[Tooltip("Set the textMesh properties in one go just for convenience. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	public class Tk2dTextMeshSetProperties : FsmStateAction
	{
		// Token: 0x060043C8 RID: 17352 RVA: 0x0015E1B0 File Offset: 0x0015C3B0
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x060043C9 RID: 17353 RVA: 0x0015E1E8 File Offset: 0x0015C3E8
		public override void Reset()
		{
			this.gameObject = null;
			this.text = null;
			this.inlineStyling = null;
			this.textureGradient = null;
			this.mainColor = null;
			this.gradientColor = null;
			this.useGradient = null;
			this.anchor = TextAnchor.LowerLeft;
			this.scale = null;
			this.kerning = null;
			this.maxChars = null;
			this.NumDrawnCharacters = null;
			this.commit = true;
		}

		// Token: 0x060043CA RID: 17354 RVA: 0x0015E258 File Offset: 0x0015C458
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoSetProperties();
			base.Finish();
		}

		// Token: 0x060043CB RID: 17355 RVA: 0x0015E26C File Offset: 0x0015C46C
		private void DoSetProperties()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: " + this._textMesh.gameObject.name);
				return;
			}
			bool flag = false;
			if (this._textMesh.text != this.text.Value)
			{
				this._textMesh.text = this.text.Value;
				flag = true;
			}
			if (this._textMesh.inlineStyling != this.inlineStyling.Value)
			{
				this._textMesh.inlineStyling = this.inlineStyling.Value;
				flag = true;
			}
			if (this._textMesh.textureGradient != this.textureGradient.Value)
			{
				this._textMesh.textureGradient = this.textureGradient.Value;
				flag = true;
			}
			if (this._textMesh.useGradient != this.useGradient.Value)
			{
				this._textMesh.useGradient = this.useGradient.Value;
				flag = true;
			}
			if (this._textMesh.color != this.mainColor.Value)
			{
				this._textMesh.color = this.mainColor.Value;
				flag = true;
			}
			if (this._textMesh.color2 != this.gradientColor.Value)
			{
				this._textMesh.color2 = this.gradientColor.Value;
				flag = true;
			}
			this.scale.Value = this._textMesh.scale;
			this.kerning.Value = this._textMesh.kerning;
			this.maxChars.Value = this._textMesh.maxChars;
			this.NumDrawnCharacters.Value = this._textMesh.NumDrawnCharacters();
			this.textureGradient.Value = this._textMesh.textureGradient;
			if (this.commit.Value && flag)
			{
				this._textMesh.Commit();
			}
		}

		// Token: 0x040035DA RID: 13786
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[CheckForComponent(typeof(tk2dTextMesh))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035DB RID: 13787
		[UIHint(UIHint.Variable)]
		[Tooltip("The Text")]
		public FsmString text;

		// Token: 0x040035DC RID: 13788
		[Tooltip("InlineStyling")]
		[UIHint(UIHint.Variable)]
		public FsmBool inlineStyling;

		// Token: 0x040035DD RID: 13789
		[Tooltip("anchor")]
		public TextAnchor anchor;

		// Token: 0x040035DE RID: 13790
		[Tooltip("The anchor as a string (text Anchor setting will be ignore if set). \npossible values ( case insensitive): LowerLeft,LowerCenter,LowerRight,MiddleLeft,MiddleCenter,MiddleRight,UpperLeft,UpperCenter or UpperRight ")]
		[UIHint(UIHint.FsmString)]
		public FsmString OrTextAnchorString;

		// Token: 0x040035DF RID: 13791
		[UIHint(UIHint.Variable)]
		[Tooltip("Kerning")]
		public FsmBool kerning;

		// Token: 0x040035E0 RID: 13792
		[UIHint(UIHint.Variable)]
		[Tooltip("maxChars")]
		public FsmInt maxChars;

		// Token: 0x040035E1 RID: 13793
		[UIHint(UIHint.Variable)]
		[Tooltip("number of drawn characters")]
		public FsmInt NumDrawnCharacters;

		// Token: 0x040035E2 RID: 13794
		[Tooltip("The Main Color")]
		[UIHint(UIHint.Variable)]
		public FsmColor mainColor;

		// Token: 0x040035E3 RID: 13795
		[UIHint(UIHint.Variable)]
		[Tooltip("The Gradient Color. Only used if gradient is true")]
		public FsmColor gradientColor;

		// Token: 0x040035E4 RID: 13796
		[UIHint(UIHint.Variable)]
		[Tooltip("Use gradient")]
		public FsmBool useGradient;

		// Token: 0x040035E5 RID: 13797
		[UIHint(UIHint.Variable)]
		[Tooltip("Texture gradient")]
		public FsmInt textureGradient;

		// Token: 0x040035E6 RID: 13798
		[Tooltip("Scale")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 scale;

		// Token: 0x040035E7 RID: 13799
		[Tooltip("Commit changes")]
		[UIHint(UIHint.FsmString)]
		public FsmBool commit;

		// Token: 0x040035E8 RID: 13800
		private tk2dTextMesh _textMesh;
	}
}
