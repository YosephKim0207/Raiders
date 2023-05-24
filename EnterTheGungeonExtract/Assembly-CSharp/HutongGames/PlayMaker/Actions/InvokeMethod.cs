using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009F5 RID: 2549
	[Tooltip("Invokes a Method in a Behaviour attached to a Game Object. See Unity InvokeMethod docs.")]
	[ActionCategory(ActionCategory.ScriptControl)]
	public class InvokeMethod : FsmStateAction
	{
		// Token: 0x060036B2 RID: 14002 RVA: 0x00117490 File Offset: 0x00115690
		public override void Reset()
		{
			this.gameObject = null;
			this.behaviour = null;
			this.methodName = string.Empty;
			this.delay = null;
			this.repeating = false;
			this.repeatDelay = 1f;
			this.cancelOnExit = false;
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x001174EC File Offset: 0x001156EC
		public override void OnEnter()
		{
			this.DoInvokeMethod(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
			base.Finish();
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x0011750C File Offset: 0x0011570C
		private void DoInvokeMethod(GameObject go)
		{
			if (go == null)
			{
				return;
			}
			this.component = go.GetComponent(ReflectionUtils.GetGlobalType(this.behaviour.Value)) as MonoBehaviour;
			if (this.component == null)
			{
				base.LogWarning("InvokeMethod: " + go.name + " missing behaviour: " + this.behaviour.Value);
				return;
			}
			if (this.repeating.Value)
			{
				this.component.InvokeRepeating(this.methodName.Value, this.delay.Value, this.repeatDelay.Value);
			}
			else
			{
				this.component.Invoke(this.methodName.Value, this.delay.Value);
			}
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x001175E4 File Offset: 0x001157E4
		public override void OnExit()
		{
			if (this.component == null)
			{
				return;
			}
			if (this.cancelOnExit.Value)
			{
				this.component.CancelInvoke(this.methodName.Value);
			}
		}

		// Token: 0x0400280F RID: 10255
		[RequiredField]
		[Tooltip("The game object that owns the behaviour.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002810 RID: 10256
		[Tooltip("The behaviour that contains the method.")]
		[UIHint(UIHint.Script)]
		[RequiredField]
		public FsmString behaviour;

		// Token: 0x04002811 RID: 10257
		[UIHint(UIHint.Method)]
		[Tooltip("The name of the method to invoke.")]
		[RequiredField]
		public FsmString methodName;

		// Token: 0x04002812 RID: 10258
		[Tooltip("Optional time delay in seconds.")]
		[HasFloatSlider(0f, 10f)]
		public FsmFloat delay;

		// Token: 0x04002813 RID: 10259
		[Tooltip("Call the method repeatedly.")]
		public FsmBool repeating;

		// Token: 0x04002814 RID: 10260
		[Tooltip("Delay between repeated calls in seconds.")]
		[HasFloatSlider(0f, 10f)]
		public FsmFloat repeatDelay;

		// Token: 0x04002815 RID: 10261
		[Tooltip("Stop calling the method when the state is exited.")]
		public FsmBool cancelOnExit;

		// Token: 0x04002816 RID: 10262
		private MonoBehaviour component;
	}
}
