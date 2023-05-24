using System;
using System.Collections.Generic;
using FullInspector.Internal;
using FullSerializer;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000601 RID: 1537
	public class Facade<T>
	{
		// Token: 0x060023F3 RID: 9203 RVA: 0x0009D084 File Offset: 0x0009B284
		public void PopulateInstance(ref T instance)
		{
			if (instance.GetType() != this.InstanceType)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"PopulateInstance: Actual Facade type is different (instance.GetType() = ",
					instance.GetType().CSharpName(),
					", InstanceType = ",
					this.InstanceType.CSharpName(),
					")"
				}));
			}
			Type serializerType = fiInstalledSerializerManager.DefaultMetadata.SerializerType;
			BaseSerializer baseSerializer = (BaseSerializer)fiSingletons.Get(serializerType);
			ListSerializationOperator listSerializationOperator = new ListSerializationOperator
			{
				SerializedObjects = this.ObjectReferences
			};
			InspectedType inspectedType = InspectedType.Get(instance.GetType());
			foreach (KeyValuePair<string, string> keyValuePair in this.FacadeMembers)
			{
				string key = keyValuePair.Key;
				InspectedProperty propertyByName = inspectedType.GetPropertyByName(key);
				if (propertyByName != null)
				{
					try
					{
						object obj = baseSerializer.Deserialize(propertyByName.StorageType.Resolve(), keyValuePair.Value, listSerializationOperator);
						propertyByName.Write(instance, obj);
					}
					catch (Exception ex)
					{
						Debug.LogError(string.Concat(new object[] { "Skipping property ", key, " in facade due to deserialization exception.\n", ex }));
					}
				}
			}
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x0009D208 File Offset: 0x0009B408
		public T ConstructInstance()
		{
			T t = (T)((object)Activator.CreateInstance(this.InstanceType));
			this.PopulateInstance(ref t);
			return t;
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x0009D230 File Offset: 0x0009B430
		public T ConstructInstance(GameObject context)
		{
			T t;
			if (typeof(Component).IsAssignableFrom(this.InstanceType))
			{
				t = (T)((object)context.AddComponent(this.InstanceType));
			}
			else
			{
				t = (T)((object)Activator.CreateInstance(this.InstanceType));
			}
			this.PopulateInstance(ref t);
			return t;
		}

		// Token: 0x040018FF RID: 6399
		public Type InstanceType;

		// Token: 0x04001900 RID: 6400
		public Dictionary<string, string> FacadeMembers = new Dictionary<string, string>();

		// Token: 0x04001901 RID: 6401
		public List<UnityEngine.Object> ObjectReferences = new List<UnityEngine.Object>();
	}
}
