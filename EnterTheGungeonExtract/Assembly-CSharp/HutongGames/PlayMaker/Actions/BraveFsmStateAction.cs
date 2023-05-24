using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C8B RID: 3211
	public class BraveFsmStateAction : FsmStateAction
	{
		// Token: 0x060044C6 RID: 17606 RVA: 0x001638C0 File Offset: 0x00161AC0
		protected void SetReplacementString(string targetString)
		{
			FsmString fsmString = base.Fsm.Variables.GetFsmString("npcReplacementString");
			if (fsmString != null)
			{
				fsmString.Value = targetString;
			}
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x001638F0 File Offset: 0x00161AF0
		protected T GetActionInPreviousNode<T>() where T : FsmStateAction
		{
			for (int i = 0; i < base.Fsm.PreviousActiveState.Actions.Length; i++)
			{
				if (base.Fsm.PreviousActiveState.Actions[i] is T)
				{
					return base.Fsm.PreviousActiveState.Actions[i] as T;
				}
			}
			return (T)((object)null);
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x00163960 File Offset: 0x00161B60
		protected T FindActionOfType<T>() where T : FsmStateAction
		{
			for (int i = 0; i < base.Fsm.States.Length; i++)
			{
				for (int j = 0; j < base.Fsm.States[i].Actions.Length; j++)
				{
					if (base.Fsm.States[i].Actions[j] is T)
					{
						return base.Fsm.States[i].Actions[j] as T;
					}
				}
			}
			return (T)((object)null);
		}
	}
}
