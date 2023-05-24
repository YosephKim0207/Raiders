using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C4C RID: 3148
	[Tooltip("Set the texture gradient of the font currently applied to a TextMesh. \nChanges will not be updated if commit is OFF. This is so you can change multiple parameters without reconstructing the mesh repeatedly.\n Use tk2dtextMeshCommit or set commit to true on your last change for that mesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshSetTextureGradient : FsmStateAction
	{
		// Token: 0x060043D9 RID: 17369 RVA: 0x0015E6C8 File Offset: 0x0015C8C8
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x060043DA RID: 17370 RVA: 0x0015E700 File Offset: 0x0015C900
		public override void Reset()
		{
			this.gameObject = null;
			this.textureGradient = 0;
			this.commit = true;
			this.everyframe = false;
		}

		// Token: 0x060043DB RID: 17371 RVA: 0x0015E728 File Offset: 0x0015C928
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoSetTextureGradient();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x060043DC RID: 17372 RVA: 0x0015E748 File Offset: 0x0015C948
		public override void OnUpdate()
		{
			this.DoSetTextureGradient();
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x0015E750 File Offset: 0x0015C950
		private void DoSetTextureGradient()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: " + this._textMesh.gameObject.name);
				return;
			}
			if (this._textMesh.textureGradient != this.textureGradient.Value)
			{
				this._textMesh.textureGradient = this.textureGradient.Value;
				if (this.commit.Value)
				{
					this._textMesh.Commit();
				}
			}
		}

		// Token: 0x040035F3 RID: 13811
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[CheckForComponent(typeof(tk2dTextMesh))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035F4 RID: 13812
		[UIHint(UIHint.FsmInt)]
		[Tooltip("The Gradient Id")]
		public FsmInt textureGradient;

		// Token: 0x040035F5 RID: 13813
		[UIHint(UIHint.FsmString)]
		[Tooltip("Commit changes")]
		public FsmBool commit;

		// Token: 0x040035F6 RID: 13814
		[ActionSection("")]
		[Tooltip("Repeat every frame.")]
		public bool everyframe;

		// Token: 0x040035F7 RID: 13815
		private tk2dTextMesh _textMesh;
	}
}
