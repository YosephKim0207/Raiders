using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CC4 RID: 3268
	[Tooltip("Stops an NPC's PathMover component.")]
	[ActionCategory(".NPCs")]
	public class StopPathMoving : FsmStateAction
	{
		// Token: 0x06004579 RID: 17785 RVA: 0x00168480 File Offset: 0x00166680
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

		// Token: 0x0600457A RID: 17786 RVA: 0x00168508 File Offset: 0x00166708
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.GameObject);
			if (!ownerDefaultTarget)
			{
				return;
			}
			PathMover component = ownerDefaultTarget.GetComponent<PathMover>();
			if (!component)
			{
				return;
			}
			component.Paused = true;
			if (this.ReenableCollideWithOthers.Value && component.specRigidbody)
			{
				component.specRigidbody.CollideWithOthers = true;
			}
			base.Finish();
		}

		// Token: 0x040037BB RID: 14267
		public FsmOwnerDefault GameObject;

		// Token: 0x040037BC RID: 14268
		public FsmBool ReenableCollideWithOthers;
	}
}
