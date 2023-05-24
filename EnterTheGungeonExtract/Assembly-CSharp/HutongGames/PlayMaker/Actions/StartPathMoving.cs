using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CC3 RID: 3267
	[ActionCategory(".NPCs")]
	[Tooltip("Starts an NPC's PathMover component.")]
	public class StartPathMoving : FsmStateAction
	{
		// Token: 0x06004576 RID: 17782 RVA: 0x00168384 File Offset: 0x00166584
		public override string ErrorCheck()
		{
			string text = string.Empty;
			GameObject gameObject = ((this.GameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.GameObject.GameObject.Value : base.Owner);
			if (gameObject)
			{
				if (!gameObject.GetComponent<PathMover>())
				{
					text += "Must have a PathMover component.\n";
				}
			}
			else if (!this.GameObject.GameObject.UseVariable)
			{
				return "No object specified";
			}
			return text;
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x0016840C File Offset: 0x0016660C
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.GameObject);
			PathMover component = ownerDefaultTarget.GetComponent<PathMover>();
			if (component)
			{
				component.Paused = false;
				if (this.DisableCollideWithOthers.Value && component.specRigidbody)
				{
					component.specRigidbody.CollideWithOthers = false;
				}
			}
			base.Finish();
		}

		// Token: 0x040037B9 RID: 14265
		public FsmOwnerDefault GameObject;

		// Token: 0x040037BA RID: 14266
		public FsmBool DisableCollideWithOthers;
	}
}
