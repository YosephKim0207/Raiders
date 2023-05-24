using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B22 RID: 2850
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Start a Coroutine in a Behaviour on a Game Object. See Unity StartCoroutine docs.")]
	public class StartCoroutine : FsmStateAction
	{
		// Token: 0x06003C0B RID: 15371 RVA: 0x0012E5D8 File Offset: 0x0012C7D8
		public override void Reset()
		{
			this.gameObject = null;
			this.behaviour = null;
			this.functionCall = null;
			this.stopOnExit = false;
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x0012E5F8 File Offset: 0x0012C7F8
		public override void OnEnter()
		{
			this.DoStartCoroutine();
			base.Finish();
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x0012E608 File Offset: 0x0012C808
		private void DoStartCoroutine()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this.component = ownerDefaultTarget.GetComponent(ReflectionUtils.GetGlobalType(this.behaviour.Value)) as MonoBehaviour;
			if (this.component == null)
			{
				base.LogWarning("StartCoroutine: " + ownerDefaultTarget.name + " missing behaviour: " + this.behaviour.Value);
				return;
			}
			string parameterType = this.functionCall.ParameterType;
			switch (parameterType)
			{
			case "None":
				this.component.StartCoroutine(this.functionCall.FunctionName);
				return;
			case "int":
				this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.IntParameter.Value);
				return;
			case "float":
				this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.FloatParameter.Value);
				return;
			case "string":
				this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.StringParameter.Value);
				return;
			case "bool":
				this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.BoolParameter.Value);
				return;
			case "Vector2":
				this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.Vector2Parameter.Value);
				return;
			case "Vector3":
				this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.Vector3Parameter.Value);
				return;
			case "Rect":
				this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.RectParamater.Value);
				return;
			case "GameObject":
				this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.GameObjectParameter.Value);
				return;
			case "Material":
				this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.MaterialParameter.Value);
				break;
			case "Texture":
				this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.TextureParameter.Value);
				break;
			case "Quaternion":
				this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.QuaternionParameter.Value);
				break;
			case "Object":
				this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.ObjectParameter.Value);
				return;
			}
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x0012E9D0 File Offset: 0x0012CBD0
		public override void OnExit()
		{
			if (this.component == null)
			{
				return;
			}
			if (this.stopOnExit)
			{
				this.component.StopCoroutine(this.functionCall.FunctionName);
			}
		}

		// Token: 0x04002E3A RID: 11834
		[RequiredField]
		[Tooltip("The game object that owns the Behaviour.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002E3B RID: 11835
		[Tooltip("The Behaviour that contains the method to start as a coroutine.")]
		[UIHint(UIHint.Behaviour)]
		[RequiredField]
		public FsmString behaviour;

		// Token: 0x04002E3C RID: 11836
		[UIHint(UIHint.Coroutine)]
		[Tooltip("The name of the coroutine method.")]
		[RequiredField]
		public FunctionCall functionCall;

		// Token: 0x04002E3D RID: 11837
		[Tooltip("Stop the coroutine when the state is exited.")]
		public bool stopOnExit;

		// Token: 0x04002E3E RID: 11838
		private MonoBehaviour component;
	}
}
