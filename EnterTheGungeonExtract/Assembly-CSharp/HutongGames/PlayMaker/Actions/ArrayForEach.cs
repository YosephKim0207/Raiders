using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008DF RID: 2271
	[Tooltip("Iterate through the items in an Array and run an FSM on each item. NOTE: The FSM has to Finish before being run on the next item.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayForEach : RunFSMAction
	{
		// Token: 0x06003243 RID: 12867 RVA: 0x001088D8 File Offset: 0x00106AD8
		public override void Reset()
		{
			this.array = null;
			this.fsmTemplateControl = new FsmTemplateControl();
			this.runFsm = null;
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x001088F4 File Offset: 0x00106AF4
		public override void Awake()
		{
			if (this.array != null && this.fsmTemplateControl.fsmTemplate != null && Application.isPlaying)
			{
				this.runFsm = base.Fsm.CreateSubFsm(this.fsmTemplateControl);
			}
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x00108944 File Offset: 0x00106B44
		public override void OnEnter()
		{
			if (this.array == null || this.runFsm == null)
			{
				base.Finish();
				return;
			}
			this.currentIndex = 0;
			this.StartFsm();
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x00108970 File Offset: 0x00106B70
		public override void OnUpdate()
		{
			this.runFsm.Update();
			if (!this.runFsm.Finished)
			{
				return;
			}
			this.StartNextFsm();
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x00108994 File Offset: 0x00106B94
		public override void OnFixedUpdate()
		{
			this.runFsm.LateUpdate();
			if (!this.runFsm.Finished)
			{
				return;
			}
			this.StartNextFsm();
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x001089B8 File Offset: 0x00106BB8
		public override void OnLateUpdate()
		{
			this.runFsm.LateUpdate();
			if (!this.runFsm.Finished)
			{
				return;
			}
			this.StartNextFsm();
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x001089DC File Offset: 0x00106BDC
		private void StartNextFsm()
		{
			this.currentIndex++;
			this.StartFsm();
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x001089F4 File Offset: 0x00106BF4
		private void StartFsm()
		{
			while (this.currentIndex < this.array.Length)
			{
				this.DoStartFsm();
				if (!this.runFsm.Finished)
				{
					return;
				}
				this.currentIndex++;
			}
			base.Fsm.Event(this.finishEvent);
			base.Finish();
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x00108A58 File Offset: 0x00106C58
		private void DoStartFsm()
		{
			this.storeItem.SetValue(this.array.Values[this.currentIndex]);
			this.fsmTemplateControl.UpdateValues();
			this.fsmTemplateControl.ApplyOverrides(this.runFsm);
			this.runFsm.OnEnable();
			if (!this.runFsm.Started)
			{
				this.runFsm.Start();
			}
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x00108AC4 File Offset: 0x00106CC4
		protected override void CheckIfFinished()
		{
		}

		// Token: 0x0400235E RID: 9054
		[Tooltip("Array to iterate through.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmArray array;

		// Token: 0x0400235F RID: 9055
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the item in a variable")]
		[HideTypeFilter]
		[MatchElementType("array")]
		public FsmVar storeItem;

		// Token: 0x04002360 RID: 9056
		[ActionSection("Run FSM")]
		public FsmTemplateControl fsmTemplateControl = new FsmTemplateControl();

		// Token: 0x04002361 RID: 9057
		[Tooltip("Event to send after iterating through all items in the Array.")]
		public FsmEvent finishEvent;

		// Token: 0x04002362 RID: 9058
		private int currentIndex;
	}
}
