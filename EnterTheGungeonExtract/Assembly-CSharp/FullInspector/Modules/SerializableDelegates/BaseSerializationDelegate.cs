using System;
using System.Reflection;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector.Modules.SerializableDelegates
{
	// Token: 0x0200061D RID: 1565
	public class BaseSerializationDelegate
	{
		// Token: 0x06002473 RID: 9331 RVA: 0x0009E440 File Offset: 0x0009C640
		public BaseSerializationDelegate()
			: this(null, string.Empty)
		{
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x0009E450 File Offset: 0x0009C650
		public BaseSerializationDelegate(UnityEngine.Object methodContainer, string methodName)
		{
			this.MethodContainer = methodContainer;
			this.MethodName = methodName;
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002475 RID: 9333 RVA: 0x0009E468 File Offset: 0x0009C668
		public bool CanInvoke
		{
			get
			{
				return this.MethodContainer != null && !string.IsNullOrEmpty(this.MethodName) && this.MethodContainer.GetType().GetFlattenedMethod(this.MethodName) != null;
			}
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x0009E4B8 File Offset: 0x0009C6B8
		protected object DoInvoke(params object[] parameters)
		{
			if (this.MethodContainer == null)
			{
				throw new InvalidOperationException("Attempt to invoke delegate without a method container");
			}
			if (string.IsNullOrEmpty(this.MethodName))
			{
				throw new InvalidOperationException("Attempt to invoke delegate without a selected method");
			}
			MethodInfo flattenedMethod = this.MethodContainer.GetType().GetFlattenedMethod(this.MethodName);
			if (flattenedMethod == null)
			{
				throw new InvalidOperationException(string.Concat(new object[] { "Unable to locate method ", this.MethodName, " in container ", this.MethodContainer }));
			}
			return flattenedMethod.Invoke(this.MethodContainer, parameters);
		}

		// Token: 0x0400192E RID: 6446
		public UnityEngine.Object MethodContainer;

		// Token: 0x0400192F RID: 6447
		public string MethodName;
	}
}
