using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A14 RID: 2580
	[Tooltip("Stop an iTween action.")]
	[ActionCategory("iTween")]
	public class iTweenStop : FsmStateAction
	{
		// Token: 0x0600374B RID: 14155 RVA: 0x0011CFA4 File Offset: 0x0011B1A4
		public override void Reset()
		{
			this.id = new FsmString
			{
				UseVariable = true
			};
			this.iTweenType = iTweenFSMType.all;
			this.includeChildren = false;
			this.inScene = false;
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x0011CFDC File Offset: 0x0011B1DC
		public override void OnEnter()
		{
			base.OnEnter();
			this.DoiTween();
			base.Finish();
		}

		// Token: 0x0600374D RID: 14157 RVA: 0x0011CFF0 File Offset: 0x0011B1F0
		private void DoiTween()
		{
			if (this.id.IsNone)
			{
				if (this.iTweenType == iTweenFSMType.all)
				{
					iTween.Stop();
				}
				else if (this.inScene)
				{
					iTween.Stop(Enum.GetName(typeof(iTweenFSMType), this.iTweenType));
				}
				else
				{
					GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
					if (ownerDefaultTarget == null)
					{
						return;
					}
					iTween.Stop(ownerDefaultTarget, Enum.GetName(typeof(iTweenFSMType), this.iTweenType), this.includeChildren);
				}
			}
			else
			{
				iTween.StopByName(this.id.Value);
			}
		}

		// Token: 0x0400291F RID: 10527
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002920 RID: 10528
		public FsmString id;

		// Token: 0x04002921 RID: 10529
		public iTweenFSMType iTweenType;

		// Token: 0x04002922 RID: 10530
		public bool includeChildren;

		// Token: 0x04002923 RID: 10531
		public bool inScene;
	}
}
