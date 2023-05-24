using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C46 RID: 3142
	[Tooltip("Set the inlineStyling flag of a TextMesh. \nChanges will not be updated if commit is OFF. This is so you can change multiple parameters without reconstructing the mesh repeatedly.\n Use tk2dtextMeshCommit or set commit to true on your last change for that mesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshSetInlineStyling : FsmStateAction
	{
		// Token: 0x060043B7 RID: 17335 RVA: 0x0015DEC4 File Offset: 0x0015C0C4
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x0015DEFC File Offset: 0x0015C0FC
		public override void Reset()
		{
			this.gameObject = null;
			this.inlineStyling = true;
			this.commit = true;
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x0015DF20 File Offset: 0x0015C120
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoSetInlineStyling();
			base.Finish();
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x0015DF34 File Offset: 0x0015C134
		private void DoSetInlineStyling()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: ");
				return;
			}
			if (this._textMesh.inlineStyling != this.inlineStyling.Value)
			{
				this._textMesh.inlineStyling = this.inlineStyling.Value;
				if (this.commit.Value)
				{
					this._textMesh.Commit();
				}
			}
		}

		// Token: 0x040035CC RID: 13772
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[CheckForComponent(typeof(tk2dTextMesh))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035CD RID: 13773
		[Tooltip("Does the text features inline styling?")]
		[UIHint(UIHint.FsmBool)]
		public FsmBool inlineStyling;

		// Token: 0x040035CE RID: 13774
		[UIHint(UIHint.FsmString)]
		[Tooltip("Commit changes")]
		public FsmBool commit;

		// Token: 0x040035CF RID: 13775
		private tk2dTextMesh _textMesh;
	}
}
