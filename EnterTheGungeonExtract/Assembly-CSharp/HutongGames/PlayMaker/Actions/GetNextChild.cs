using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200099D RID: 2461
	[Tooltip("Each time this action is called it gets the next child of a GameObject. This lets you quickly loop through all the children of an object to perform actions on them. NOTE: To find a specific child use Find Child.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetNextChild : FsmStateAction
	{
		// Token: 0x06003568 RID: 13672 RVA: 0x001131D0 File Offset: 0x001113D0
		public override void Reset()
		{
			this.gameObject = null;
			this.storeNextChild = null;
			this.loopEvent = null;
			this.finishedEvent = null;
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x001131F0 File Offset: 0x001113F0
		public override void OnEnter()
		{
			this.DoGetNextChild(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
			base.Finish();
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x00113210 File Offset: 0x00111410
		private void DoGetNextChild(GameObject parent)
		{
			if (parent == null)
			{
				return;
			}
			if (this.go != parent)
			{
				this.go = parent;
				this.nextChildIndex = 0;
			}
			if (this.nextChildIndex >= this.go.transform.childCount)
			{
				this.nextChildIndex = 0;
				base.Fsm.Event(this.finishedEvent);
				return;
			}
			this.storeNextChild.Value = parent.transform.GetChild(this.nextChildIndex).gameObject;
			if (this.nextChildIndex >= this.go.transform.childCount)
			{
				this.nextChildIndex = 0;
				base.Fsm.Event(this.finishedEvent);
				return;
			}
			this.nextChildIndex++;
			if (this.loopEvent != null)
			{
				base.Fsm.Event(this.loopEvent);
			}
		}

		// Token: 0x040026BE RID: 9918
		[Tooltip("The parent GameObject. Note, if GameObject changes, this action will reset and start again at the first child.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040026BF RID: 9919
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the next child in a GameObject variable.")]
		[RequiredField]
		public FsmGameObject storeNextChild;

		// Token: 0x040026C0 RID: 9920
		[Tooltip("Event to send to get the next child.")]
		public FsmEvent loopEvent;

		// Token: 0x040026C1 RID: 9921
		[Tooltip("Event to send when there are no more children.")]
		public FsmEvent finishedEvent;

		// Token: 0x040026C2 RID: 9922
		private GameObject go;

		// Token: 0x040026C3 RID: 9923
		private int nextChildIndex;
	}
}
