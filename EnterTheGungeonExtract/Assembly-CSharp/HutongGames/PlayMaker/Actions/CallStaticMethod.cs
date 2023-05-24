using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000907 RID: 2311
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Call a static method in a class.")]
	public class CallStaticMethod : FsmStateAction
	{
		// Token: 0x060032E1 RID: 13025 RVA: 0x0010B0C0 File Offset: 0x001092C0
		public override void OnEnter()
		{
			this.parametersArray = new object[this.parameters.Length];
			this.DoMethodCall();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x0010B0EC File Offset: 0x001092EC
		public override void OnUpdate()
		{
			this.DoMethodCall();
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x0010B0F4 File Offset: 0x001092F4
		private void DoMethodCall()
		{
			if (this.className == null || string.IsNullOrEmpty(this.className.Value))
			{
				base.Finish();
				return;
			}
			if (this.cachedClassName != this.className.Value || this.cachedMethodName != this.methodName.Value)
			{
				this.errorString = string.Empty;
				if (!this.DoCache())
				{
					Debug.LogError(this.errorString);
					base.Finish();
					return;
				}
			}
			object obj;
			if (this.cachedParameterInfo.Length == 0)
			{
				obj = this.cachedMethodInfo.Invoke(null, null);
			}
			else
			{
				for (int i = 0; i < this.parameters.Length; i++)
				{
					FsmVar fsmVar = this.parameters[i];
					fsmVar.UpdateValue();
					this.parametersArray[i] = fsmVar.GetValue();
				}
				obj = this.cachedMethodInfo.Invoke(null, this.parametersArray);
			}
			this.storeResult.SetValue(obj);
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x0010B200 File Offset: 0x00109400
		private bool DoCache()
		{
			this.cachedType = ReflectionUtils.GetGlobalType(this.className.Value);
			if (this.cachedType == null)
			{
				this.errorString = this.errorString + "Class is invalid: " + this.className.Value + "\n";
				base.Finish();
				return false;
			}
			this.cachedClassName = this.className.Value;
			List<Type> list = new List<Type>(this.parameters.Length);
			foreach (FsmVar fsmVar in this.parameters)
			{
				list.Add(fsmVar.RealType);
			}
			this.cachedMethodInfo = this.cachedType.GetMethod(this.methodName.Value, list.ToArray());
			if (this.cachedMethodInfo == null)
			{
				this.errorString = this.errorString + "Invalid Method Name or Parameters: " + this.methodName.Value + "\n";
				base.Finish();
				return false;
			}
			this.cachedMethodName = this.methodName.Value;
			this.cachedParameterInfo = this.cachedMethodInfo.GetParameters();
			return true;
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x0010B324 File Offset: 0x00109524
		public override string ErrorCheck()
		{
			this.errorString = string.Empty;
			this.DoCache();
			if (!string.IsNullOrEmpty(this.errorString))
			{
				return this.errorString;
			}
			if (this.parameters.Length != this.cachedParameterInfo.Length)
			{
				return string.Concat(new object[]
				{
					"Parameter count does not match method.\nMethod has ",
					this.cachedParameterInfo.Length,
					" parameters.\nYou specified ",
					this.parameters.Length,
					" paramaters."
				});
			}
			for (int i = 0; i < this.parameters.Length; i++)
			{
				FsmVar fsmVar = this.parameters[i];
				Type realType = fsmVar.RealType;
				Type parameterType = this.cachedParameterInfo[i].ParameterType;
				if (!object.ReferenceEquals(realType, parameterType))
				{
					return string.Concat(new object[]
					{
						"Parameters do not match method signature.\nParameter ",
						i + 1,
						" (",
						realType,
						") should be of type: ",
						parameterType
					});
				}
			}
			if (object.ReferenceEquals(this.cachedMethodInfo.ReturnType, typeof(void)))
			{
				if (!string.IsNullOrEmpty(this.storeResult.variableName))
				{
					return "Method does not have return.\nSpecify 'none' in Store Result.";
				}
			}
			else if (!object.ReferenceEquals(this.cachedMethodInfo.ReturnType, this.storeResult.RealType))
			{
				return "Store Result is of the wrong type.\nIt should be of type: " + this.cachedMethodInfo.ReturnType;
			}
			return string.Empty;
		}

		// Token: 0x04002410 RID: 9232
		[Tooltip("Full path to the class that contains the static method.")]
		public FsmString className;

		// Token: 0x04002411 RID: 9233
		[Tooltip("The static method to call.")]
		public FsmString methodName;

		// Token: 0x04002412 RID: 9234
		[Tooltip("Method paramters. NOTE: these must match the method's signature!")]
		public FsmVar[] parameters;

		// Token: 0x04002413 RID: 9235
		[Tooltip("Store the result of the method call.")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Store Result")]
		public FsmVar storeResult;

		// Token: 0x04002414 RID: 9236
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002415 RID: 9237
		private Type cachedType;

		// Token: 0x04002416 RID: 9238
		private string cachedClassName;

		// Token: 0x04002417 RID: 9239
		private string cachedMethodName;

		// Token: 0x04002418 RID: 9240
		private MethodInfo cachedMethodInfo;

		// Token: 0x04002419 RID: 9241
		private ParameterInfo[] cachedParameterInfo;

		// Token: 0x0400241A RID: 9242
		private object[] parametersArray;

		// Token: 0x0400241B RID: 9243
		private string errorString;
	}
}
