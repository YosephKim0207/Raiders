using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000906 RID: 2310
	[Tooltip("Call a method in a behaviour.")]
	[ActionCategory(ActionCategory.ScriptControl)]
	public class CallMethod : FsmStateAction
	{
		// Token: 0x060032D8 RID: 13016 RVA: 0x0010AB40 File Offset: 0x00108D40
		public override void Reset()
		{
			this.behaviour = null;
			this.methodName = null;
			this.parameters = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x0010AB68 File Offset: 0x00108D68
		public override void OnEnter()
		{
			this.parametersArray = new object[this.parameters.Length];
			this.DoMethodCall();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x0010AB94 File Offset: 0x00108D94
		public override void OnUpdate()
		{
			this.DoMethodCall();
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x0010AB9C File Offset: 0x00108D9C
		private void DoMethodCall()
		{
			if (this.behaviour.Value == null)
			{
				base.Finish();
				return;
			}
			if (this.NeedToUpdateCache() && !this.DoCache())
			{
				Debug.LogError(this.errorString);
				base.Finish();
				return;
			}
			object obj;
			if (this.cachedParameterInfo.Length == 0)
			{
				obj = this.cachedMethodInfo.Invoke(this.cachedBehaviour.Value, null);
			}
			else
			{
				for (int i = 0; i < this.parameters.Length; i++)
				{
					FsmVar fsmVar = this.parameters[i];
					fsmVar.UpdateValue();
					if (fsmVar.Type == VariableType.Array)
					{
						fsmVar.UpdateValue();
						object[] array = fsmVar.GetValue() as object[];
						Type elementType = this.cachedParameterInfo[i].ParameterType.GetElementType();
						Array array2 = Array.CreateInstance(elementType, array.Length);
						for (int j = 0; j < array.Length; j++)
						{
							array2.SetValue(array[j], j);
						}
						this.parametersArray[i] = array2;
					}
					else
					{
						fsmVar.UpdateValue();
						this.parametersArray[i] = fsmVar.GetValue();
					}
				}
				obj = this.cachedMethodInfo.Invoke(this.cachedBehaviour.Value, this.parametersArray);
			}
			if (this.storeResult != null && !this.storeResult.IsNone && this.storeResult.Type != VariableType.Unknown)
			{
				this.storeResult.SetValue(obj);
			}
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x0010AD1C File Offset: 0x00108F1C
		private bool NeedToUpdateCache()
		{
			return this.cachedBehaviour == null || this.cachedMethodName == null || this.cachedBehaviour.Value != this.behaviour.Value || this.cachedBehaviour.Name != this.behaviour.Name || this.cachedMethodName.Value != this.methodName.Value || this.cachedMethodName.Name != this.methodName.Name;
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x0010ADC0 File Offset: 0x00108FC0
		private void ClearCache()
		{
			this.cachedBehaviour = null;
			this.cachedMethodName = null;
			this.cachedType = null;
			this.cachedMethodInfo = null;
			this.cachedParameterInfo = null;
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x0010ADE8 File Offset: 0x00108FE8
		private bool DoCache()
		{
			this.ClearCache();
			this.errorString = string.Empty;
			this.cachedBehaviour = new FsmObject(this.behaviour);
			this.cachedMethodName = new FsmString(this.methodName);
			if (this.cachedBehaviour.Value == null)
			{
				if (!this.behaviour.UsesVariable || Application.isPlaying)
				{
					this.errorString += "Behaviour is invalid!\n";
				}
				base.Finish();
				return false;
			}
			this.cachedType = this.behaviour.Value.GetType();
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
			this.cachedParameterInfo = this.cachedMethodInfo.GetParameters();
			return true;
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x0010AF38 File Offset: 0x00109138
		public override string ErrorCheck()
		{
			if (Application.isPlaying)
			{
				return this.errorString;
			}
			if (!this.DoCache())
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

		// Token: 0x04002404 RID: 9220
		[ObjectType(typeof(MonoBehaviour))]
		[Tooltip("Store the component in an Object variable.\nNOTE: Set theObject variable's Object Type to get a component of that type. E.g., set Object Type to UnityEngine.AudioListener to get the AudioListener component on the camera.")]
		public FsmObject behaviour;

		// Token: 0x04002405 RID: 9221
		[Tooltip("Name of the method to call on the component")]
		public FsmString methodName;

		// Token: 0x04002406 RID: 9222
		[Tooltip("Method paramters. NOTE: these must match the method's signature!")]
		public FsmVar[] parameters;

		// Token: 0x04002407 RID: 9223
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result of the method call.")]
		[ActionSection("Store Result")]
		public FsmVar storeResult;

		// Token: 0x04002408 RID: 9224
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002409 RID: 9225
		private FsmObject cachedBehaviour;

		// Token: 0x0400240A RID: 9226
		private FsmString cachedMethodName;

		// Token: 0x0400240B RID: 9227
		private Type cachedType;

		// Token: 0x0400240C RID: 9228
		private MethodInfo cachedMethodInfo;

		// Token: 0x0400240D RID: 9229
		private ParameterInfo[] cachedParameterInfo;

		// Token: 0x0400240E RID: 9230
		private object[] parametersArray;

		// Token: 0x0400240F RID: 9231
		private string errorString;
	}
}
