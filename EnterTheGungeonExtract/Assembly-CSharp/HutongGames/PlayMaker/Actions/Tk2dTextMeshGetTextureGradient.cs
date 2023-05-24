using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C41 RID: 3137
	[Tooltip("Set the texture gradient of the font currently applied to a TextMesh. \nChanges will not be updated if commit is OFF. This is so you can change multiple parameters without reconstructing the mesh repeatedly.\n Use tk2dtextMeshCommit or set commit to true on your last change for that mesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshGetTextureGradient : FsmStateAction
	{
		// Token: 0x0600439A RID: 17306 RVA: 0x0015D800 File Offset: 0x0015BA00
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x0015D838 File Offset: 0x0015BA38
		public override void Reset()
		{
			this.gameObject = null;
			this.textureGradient = 0;
			this.everyframe = false;
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x0015D854 File Offset: 0x0015BA54
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoGetTextureGradient();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x0015D874 File Offset: 0x0015BA74
		public override void OnUpdate()
		{
			this.DoGetTextureGradient();
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x0015D87C File Offset: 0x0015BA7C
		private void DoGetTextureGradient()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: " + this._textMesh.gameObject.name);
				return;
			}
			this.textureGradient.Value = this._textMesh.textureGradient;
		}

		// Token: 0x040035B5 RID: 13749
		[RequiredField]
		[CheckForComponent(typeof(tk2dTextMesh))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035B6 RID: 13750
		[UIHint(UIHint.Variable)]
		[Tooltip("The Gradient Id")]
		public FsmInt textureGradient;

		// Token: 0x040035B7 RID: 13751
		[ActionSection("")]
		[Tooltip("Repeat every frame.")]
		public bool everyframe;

		// Token: 0x040035B8 RID: 13752
		private tk2dTextMesh _textMesh;
	}
}
