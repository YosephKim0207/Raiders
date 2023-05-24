using System;
using System.Reflection;
using FullInspector.Internal;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x020005DA RID: 1498
	public class InspectedMethod
	{
		// Token: 0x0600236D RID: 9069 RVA: 0x0009B09C File Offset: 0x0009929C
		public InspectedMethod(MethodInfo method)
		{
			this.Method = method;
			foreach (ParameterInfo parameterInfo in method.GetParameters())
			{
				if (!parameterInfo.IsOptional)
				{
					this.HasArguments = true;
					break;
				}
			}
			this.DisplayLabel = new GUIContent();
			InspectorNameAttribute attribute = fsPortableReflection.GetAttribute<InspectorNameAttribute>(method);
			if (attribute != null)
			{
				this.DisplayLabel.text = attribute.DisplayName;
			}
			if (string.IsNullOrEmpty(this.DisplayLabel.text))
			{
				this.DisplayLabel.text = fiDisplayNameMapper.Map(method.Name);
			}
			InspectorTooltipAttribute attribute2 = fsPortableReflection.GetAttribute<InspectorTooltipAttribute>(method);
			if (attribute2 != null)
			{
				this.DisplayLabel.tooltip = attribute2.Tooltip;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x0600236E RID: 9070 RVA: 0x0009B168 File Offset: 0x00099368
		// (set) Token: 0x0600236F RID: 9071 RVA: 0x0009B170 File Offset: 0x00099370
		public MethodInfo Method { get; private set; }

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002370 RID: 9072 RVA: 0x0009B17C File Offset: 0x0009937C
		// (set) Token: 0x06002371 RID: 9073 RVA: 0x0009B184 File Offset: 0x00099384
		public GUIContent DisplayLabel { get; private set; }

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002372 RID: 9074 RVA: 0x0009B190 File Offset: 0x00099390
		// (set) Token: 0x06002373 RID: 9075 RVA: 0x0009B198 File Offset: 0x00099398
		public bool HasArguments { get; private set; }

		// Token: 0x06002374 RID: 9076 RVA: 0x0009B1A4 File Offset: 0x000993A4
		public void Invoke(object instance)
		{
			try
			{
				object[] array = null;
				ParameterInfo[] parameters = this.Method.GetParameters();
				if (parameters.Length != 0)
				{
					array = new object[parameters.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = parameters[i].DefaultValue;
					}
				}
				this.Method.Invoke(instance, array);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}
	}
}
