using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A06 RID: 2566
	[ActionCategory("iTween")]
	[Tooltip("Resume an iTween action.")]
	public class iTweenResume : FsmStateAction
	{
		// Token: 0x06003704 RID: 14084 RVA: 0x0011A8CC File Offset: 0x00118ACC
		public override void Reset()
		{
			this.iTweenType = iTweenFSMType.all;
			this.includeChildren = false;
			this.inScene = false;
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x0011A8E4 File Offset: 0x00118AE4
		public override void OnEnter()
		{
			base.OnEnter();
			this.DoiTween();
			base.Finish();
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x0011A8F8 File Offset: 0x00118AF8
		private void DoiTween()
		{
			if (this.iTweenType == iTweenFSMType.all)
			{
				iTween.Resume();
			}
			else if (this.inScene)
			{
				iTween.Resume(Enum.GetName(typeof(iTweenFSMType), this.iTweenType));
			}
			else
			{
				GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
				if (ownerDefaultTarget == null)
				{
					return;
				}
				iTween.Resume(ownerDefaultTarget, Enum.GetName(typeof(iTweenFSMType), this.iTweenType), this.includeChildren);
			}
		}

		// Token: 0x040028B1 RID: 10417
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040028B2 RID: 10418
		public iTweenFSMType iTweenType;

		// Token: 0x040028B3 RID: 10419
		public bool includeChildren;

		// Token: 0x040028B4 RID: 10420
		public bool inScene;
	}
}
