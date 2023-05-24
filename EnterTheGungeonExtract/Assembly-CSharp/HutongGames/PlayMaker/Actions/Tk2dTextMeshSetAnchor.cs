using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C43 RID: 3139
	[ActionCategory("2D Toolkit/TextMesh")]
	[Tooltip("Set the anchor of a TextMesh. \nChanges will not be updated if commit is OFF. This is so you can change multiple parameters without reconstructing the mesh repeatedly.\n Use tk2dtextMeshCommit or set commit to true on your last change for that mesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	public class Tk2dTextMeshSetAnchor : FsmStateAction
	{
		// Token: 0x060043A5 RID: 17317 RVA: 0x0015D968 File Offset: 0x0015BB68
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x0015D9A0 File Offset: 0x0015BBA0
		public override void Reset()
		{
			this.gameObject = null;
			this.textAnchor = TextAnchor.LowerLeft;
			this.OrTextAnchorString = string.Empty;
			this.commit = true;
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x0015D9CC File Offset: 0x0015BBCC
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoSetAnchor();
			base.Finish();
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x0015D9E0 File Offset: 0x0015BBE0
		private void DoSetAnchor()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: " + this._textMesh.gameObject.name);
				return;
			}
			bool flag = false;
			TextAnchor textAnchor = this.textAnchor;
			if (this.OrTextAnchorString.Value != string.Empty)
			{
				bool flag2 = false;
				TextAnchor textAnchorFromString = this.getTextAnchorFromString(this.OrTextAnchorString.Value, out flag2);
				if (flag2)
				{
					textAnchor = textAnchorFromString;
				}
			}
			if (this._textMesh.anchor != textAnchor)
			{
				this._textMesh.anchor = textAnchor;
				flag = true;
			}
			if (this.commit.Value && flag)
			{
				this._textMesh.Commit();
			}
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x0015DAA0 File Offset: 0x0015BCA0
		public override string ErrorCheck()
		{
			if (this.OrTextAnchorString.Value != string.Empty)
			{
				bool flag = false;
				this.getTextAnchorFromString(this.OrTextAnchorString.Value, out flag);
				if (!flag)
				{
					return "Text Anchor string '" + this.OrTextAnchorString.Value + "' is not valid. Use (case insensitive): LowerLeft,LowerCenter,LowerRight,MiddleLeft,MiddleCenter,MiddleRight,UpperLeft,UpperCenter or UpperRight";
				}
			}
			return null;
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x0015DB00 File Offset: 0x0015BD00
		private TextAnchor getTextAnchorFromString(string textAnchorString, out bool isValid)
		{
			isValid = true;
			string text = textAnchorString.ToLower();
			switch (text)
			{
			case "lowerleft":
				return TextAnchor.LowerLeft;
			case "lowercenter":
				return TextAnchor.LowerCenter;
			case "lowerright":
				return TextAnchor.LowerRight;
			case "middleleft":
				return TextAnchor.MiddleLeft;
			case "middlecenter":
				return TextAnchor.MiddleCenter;
			case "middleright":
				return TextAnchor.MiddleRight;
			case "upperleft":
				return TextAnchor.UpperLeft;
			case "uppercenter":
				return TextAnchor.UpperCenter;
			case "upperright":
				return TextAnchor.UpperRight;
			}
			isValid = false;
			return TextAnchor.LowerLeft;
		}

		// Token: 0x040035BB RID: 13755
		[CheckForComponent(typeof(tk2dTextMesh))]
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035BC RID: 13756
		[Tooltip("The anchor")]
		public TextAnchor textAnchor;

		// Token: 0x040035BD RID: 13757
		[UIHint(UIHint.FsmString)]
		[Tooltip("The anchor as a string (text Anchor setting will be ignore if set). \npossible values ( case insensitive): LowerLeft,LowerCenter,LowerRight,MiddleLeft,MiddleCenter,MiddleRight,UpperLeft,UpperCenter or UpperRight ")]
		public FsmString OrTextAnchorString;

		// Token: 0x040035BE RID: 13758
		[UIHint(UIHint.FsmBool)]
		[Tooltip("Commit changes")]
		public FsmBool commit;

		// Token: 0x040035BF RID: 13759
		private tk2dTextMesh _textMesh;
	}
}
