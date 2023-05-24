using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008F8 RID: 2296
	[ActionCategory(ActionCategory.StateMachine)]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public abstract class BaseFsmVariableIndexAction : FsmStateAction
	{
		// Token: 0x060032A1 RID: 12961 RVA: 0x0010A088 File Offset: 0x00108288
		public override void Reset()
		{
			this.fsmNotFound = null;
			this.variableNotFound = null;
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x0010A098 File Offset: 0x00108298
		protected bool UpdateCache(GameObject go, string fsmName)
		{
			if (go == null)
			{
				return false;
			}
			if (this.fsm == null || this.cachedGameObject != go || this.cachedFsmName != fsmName)
			{
				this.fsm = ActionHelpers.GetGameObjectFsm(go, fsmName);
				this.cachedGameObject = go;
				this.cachedFsmName = fsmName;
				if (this.fsm == null)
				{
					base.LogWarning("Could not find FSM: " + fsmName);
					base.Fsm.Event(this.fsmNotFound);
				}
			}
			return true;
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x0010A138 File Offset: 0x00108338
		protected void DoVariableNotFound(string variableName)
		{
			base.LogWarning("Could not find variable: " + variableName);
			base.Fsm.Event(this.variableNotFound);
		}

		// Token: 0x040023C5 RID: 9157
		[Tooltip("The event to trigger if the index is out of range")]
		[ActionSection("Events")]
		public FsmEvent indexOutOfRange;

		// Token: 0x040023C6 RID: 9158
		[Tooltip("The event to send if the FSM is not found.")]
		public FsmEvent fsmNotFound;

		// Token: 0x040023C7 RID: 9159
		[Tooltip("The event to send if the Variable is not found.")]
		public FsmEvent variableNotFound;

		// Token: 0x040023C8 RID: 9160
		private GameObject cachedGameObject;

		// Token: 0x040023C9 RID: 9161
		private string cachedFsmName;

		// Token: 0x040023CA RID: 9162
		protected PlayMakerFSM fsm;
	}
}
