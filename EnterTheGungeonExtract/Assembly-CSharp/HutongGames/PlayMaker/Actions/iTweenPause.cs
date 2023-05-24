using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A02 RID: 2562
	[Tooltip("Pause an iTween action.")]
	[ActionCategory("iTween")]
	public class iTweenPause : FsmStateAction
	{
		// Token: 0x060036F1 RID: 14065 RVA: 0x0011A060 File Offset: 0x00118260
		public override void Reset()
		{
			this.iTweenType = iTweenFSMType.all;
			this.includeChildren = false;
			this.inScene = false;
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x0011A078 File Offset: 0x00118278
		public override void OnEnter()
		{
			base.OnEnter();
			this.DoiTween();
			base.Finish();
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x0011A08C File Offset: 0x0011828C
		private void DoiTween()
		{
			if (this.iTweenType == iTweenFSMType.all)
			{
				iTween.Pause();
			}
			else if (this.inScene)
			{
				iTween.Pause(Enum.GetName(typeof(iTweenFSMType), this.iTweenType));
			}
			else
			{
				GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
				if (ownerDefaultTarget == null)
				{
					return;
				}
				iTween.Pause(ownerDefaultTarget, Enum.GetName(typeof(iTweenFSMType), this.iTweenType), this.includeChildren);
			}
		}

		// Token: 0x04002898 RID: 10392
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002899 RID: 10393
		public iTweenFSMType iTweenType;

		// Token: 0x0400289A RID: 10394
		public bool includeChildren;

		// Token: 0x0400289B RID: 10395
		public bool inScene;
	}
}
