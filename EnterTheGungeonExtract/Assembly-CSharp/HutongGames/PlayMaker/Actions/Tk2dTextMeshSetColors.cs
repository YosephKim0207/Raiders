using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C44 RID: 3140
	[Tooltip("Set the colors of a TextMesh. \nChanges will not be updated if commit is OFF. This is so you can change multiple parameters without reconstructing the mesh repeatedly.\n Use tk2dtextMeshCommit or set commit to true on your last change for that mesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshSetColors : FsmStateAction
	{
		// Token: 0x060043AC RID: 17324 RVA: 0x0015DC0C File Offset: 0x0015BE0C
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x0015DC44 File Offset: 0x0015BE44
		public override void Reset()
		{
			this.gameObject = null;
			this.mainColor = null;
			this.gradientColor = null;
			this.useGradient = false;
			this.commit = true;
			this.everyframe = false;
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x0015DC7C File Offset: 0x0015BE7C
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoSetColors();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x060043AF RID: 17327 RVA: 0x0015DC9C File Offset: 0x0015BE9C
		public override void OnUpdate()
		{
			this.DoSetColors();
		}

		// Token: 0x060043B0 RID: 17328 RVA: 0x0015DCA4 File Offset: 0x0015BEA4
		private void DoSetColors()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: " + this._textMesh.gameObject.name);
				return;
			}
			bool flag = false;
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
			if (this.commit.Value && flag)
			{
				this._textMesh.Commit();
			}
		}

		// Token: 0x040035C1 RID: 13761
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[CheckForComponent(typeof(tk2dTextMesh))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035C2 RID: 13762
		[UIHint(UIHint.FsmColor)]
		[Tooltip("Main color")]
		public FsmColor mainColor;

		// Token: 0x040035C3 RID: 13763
		[UIHint(UIHint.FsmColor)]
		[Tooltip("Gradient color. Only used if gradient is true")]
		public FsmColor gradientColor;

		// Token: 0x040035C4 RID: 13764
		[UIHint(UIHint.FsmBool)]
		[Tooltip("Use gradient.")]
		public FsmBool useGradient;

		// Token: 0x040035C5 RID: 13765
		[UIHint(UIHint.FsmString)]
		[Tooltip("Commit changes")]
		public FsmBool commit;

		// Token: 0x040035C6 RID: 13766
		[ActionSection("")]
		[Tooltip("Repeat every frame.")]
		public bool everyframe;

		// Token: 0x040035C7 RID: 13767
		private tk2dTextMesh _textMesh;
	}
}
