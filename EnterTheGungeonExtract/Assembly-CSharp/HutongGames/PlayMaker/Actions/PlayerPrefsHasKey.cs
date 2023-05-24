using System;
using Brave;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A7F RID: 2687
	[Tooltip("Returns true if key exists in the preferences.")]
	[ActionCategory("PlayerPrefs")]
	public class PlayerPrefsHasKey : FsmStateAction
	{
		// Token: 0x06003913 RID: 14611 RVA: 0x00124A94 File Offset: 0x00122C94
		public override void Reset()
		{
			this.key = string.Empty;
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x00124AA8 File Offset: 0x00122CA8
		public override void OnEnter()
		{
			base.Finish();
			if (!this.key.IsNone && !this.key.Value.Equals(string.Empty))
			{
				this.variable.Value = PlayerPrefs.HasKey(this.key.Value);
			}
			base.Fsm.Event((!this.variable.Value) ? this.falseEvent : this.trueEvent);
		}

		// Token: 0x04002B65 RID: 11109
		[RequiredField]
		public FsmString key;

		// Token: 0x04002B66 RID: 11110
		[Title("Store Result")]
		[UIHint(UIHint.Variable)]
		public FsmBool variable;

		// Token: 0x04002B67 RID: 11111
		[Tooltip("Event to send if key exists.")]
		public FsmEvent trueEvent;

		// Token: 0x04002B68 RID: 11112
		[Tooltip("Event to send if key does not exist.")]
		public FsmEvent falseEvent;
	}
}
