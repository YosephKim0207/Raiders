using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008F7 RID: 2295
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public abstract class BaseFsmVariableAction : FsmStateAction
	{
		// Token: 0x0600329D RID: 12957 RVA: 0x00109FAC File Offset: 0x001081AC
		public override void Reset()
		{
			this.fsmNotFound = null;
			this.variableNotFound = null;
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x00109FBC File Offset: 0x001081BC
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

		// Token: 0x0600329F RID: 12959 RVA: 0x0010A05C File Offset: 0x0010825C
		protected void DoVariableNotFound(string variableName)
		{
			base.LogWarning("Could not find variable: " + variableName);
			base.Fsm.Event(this.variableNotFound);
		}

		// Token: 0x040023C0 RID: 9152
		[Tooltip("The event to send if the FSM is not found.")]
		[ActionSection("Events")]
		public FsmEvent fsmNotFound;

		// Token: 0x040023C1 RID: 9153
		[Tooltip("The event to send if the Variable is not found.")]
		public FsmEvent variableNotFound;

		// Token: 0x040023C2 RID: 9154
		private GameObject cachedGameObject;

		// Token: 0x040023C3 RID: 9155
		private string cachedFsmName;

		// Token: 0x040023C4 RID: 9156
		protected PlayMakerFSM fsm;
	}
}
