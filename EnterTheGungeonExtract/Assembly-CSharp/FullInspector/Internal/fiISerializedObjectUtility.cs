using System;
using System.Collections.Generic;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x02000560 RID: 1376
	public static class fiISerializedObjectUtility
	{
		// Token: 0x060020B9 RID: 8377 RVA: 0x00090D0C File Offset: 0x0008EF0C
		private static bool SaveStateForProperty(ISerializedObject obj, InspectedProperty property, BaseSerializer serializer, ISerializationOperator serializationOperator, out string serializedValue, ref bool success)
		{
			object obj2 = property.Read(obj);
			bool flag;
			try
			{
				if (obj2 == null)
				{
					serializedValue = null;
				}
				else
				{
					serializedValue = serializer.Serialize(property.MemberInfo, obj2, serializationOperator);
				}
				flag = true;
			}
			catch (Exception ex)
			{
				success = false;
				serializedValue = null;
				Debug.LogError(string.Concat(new object[] { "Exception caught when serializing property <", property.Name, "> in <", obj, "> with value ", obj2, "\n", ex }));
				flag = false;
			}
			return flag;
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x00090DB0 File Offset: 0x0008EFB0
		public static bool SaveState<TSerializer>(ISerializedObject obj) where TSerializer : BaseSerializer
		{
			bool flag = true;
			ISerializationCallbacks serializationCallbacks = obj as ISerializationCallbacks;
			if (serializationCallbacks != null)
			{
				serializationCallbacks.OnBeforeSerialize();
			}
			TSerializer tserializer = fiSingletons.Get<TSerializer>();
			ListSerializationOperator listSerializationOperator = fiSingletons.Get<ListSerializationOperator>();
			listSerializationOperator.SerializedObjects = new List<UnityEngine.Object>();
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			if (fiUtility.IsEditor || obj.SerializedStateKeys == null || obj.SerializedStateKeys.Count == 0)
			{
				List<InspectedProperty> properties = InspectedType.Get(obj.GetType()).GetProperties(InspectedMemberFilters.FullInspectorSerializedProperties);
				for (int i = 0; i < properties.Count; i++)
				{
					InspectedProperty inspectedProperty = properties[i];
					string text;
					if (fiISerializedObjectUtility.SaveStateForProperty(obj, inspectedProperty, tserializer, listSerializationOperator, out text, ref flag))
					{
						list.Add(inspectedProperty.Name);
						list2.Add(text);
					}
				}
			}
			else
			{
				InspectedType inspectedType = InspectedType.Get(obj.GetType());
				for (int j = 0; j < obj.SerializedStateKeys.Count; j++)
				{
					InspectedProperty inspectedProperty2 = inspectedType.GetPropertyByName(obj.SerializedStateKeys[j]) ?? inspectedType.GetPropertyByFormerlySerializedName(obj.SerializedStateKeys[j]);
					if (inspectedProperty2 != null)
					{
						string text2;
						if (fiISerializedObjectUtility.SaveStateForProperty(obj, inspectedProperty2, tserializer, listSerializationOperator, out text2, ref flag))
						{
							list.Add(inspectedProperty2.Name);
							list2.Add(text2);
						}
					}
				}
			}
			if (fiISerializedObjectUtility.AreListsDifferent(obj.SerializedStateKeys, list))
			{
				obj.SerializedStateKeys = list;
			}
			if (fiISerializedObjectUtility.AreListsDifferent(obj.SerializedStateValues, list2))
			{
				obj.SerializedStateValues = list2;
			}
			if (fiISerializedObjectUtility.AreListsDifferent(obj.SerializedObjectReferences, listSerializationOperator.SerializedObjects))
			{
				obj.SerializedObjectReferences = listSerializationOperator.SerializedObjects;
			}
			if (obj is ScriptableObject)
			{
				fiLateBindings.EditorUtility.SetDirty((ScriptableObject)obj);
			}
			if (serializationCallbacks != null)
			{
				serializationCallbacks.OnAfterSerialize();
			}
			return flag;
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x00090FA0 File Offset: 0x0008F1A0
		private static bool AreListsDifferent(IList<string> a, IList<string> b)
		{
			if (a == null)
			{
				return true;
			}
			if (a.Count != b.Count)
			{
				return true;
			}
			int count = a.Count;
			for (int i = 0; i < count; i++)
			{
				if (a[i] != b[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x00090FFC File Offset: 0x0008F1FC
		private static bool AreListsDifferent(IList<UnityEngine.Object> a, IList<UnityEngine.Object> b)
		{
			if (a == null)
			{
				return true;
			}
			if (a.Count != b.Count)
			{
				return true;
			}
			int count = a.Count;
			for (int i = 0; i < count; i++)
			{
				if (!object.ReferenceEquals(a[i], b[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x00091058 File Offset: 0x0008F258
		public static bool RestoreState<TSerializer>(ISerializedObject obj) where TSerializer : BaseSerializer
		{
			bool flag = true;
			ISerializationCallbacks serializationCallbacks = obj as ISerializationCallbacks;
			if (serializationCallbacks != null)
			{
				serializationCallbacks.OnBeforeDeserialize();
			}
			if (obj.SerializedStateKeys == null)
			{
				obj.SerializedStateKeys = new List<string>();
			}
			if (obj.SerializedStateValues == null)
			{
				obj.SerializedStateValues = new List<string>();
			}
			if (obj.SerializedObjectReferences == null)
			{
				obj.SerializedObjectReferences = new List<UnityEngine.Object>();
			}
			if (obj.SerializedStateKeys.Count != obj.SerializedStateValues.Count && fiSettings.EmitWarnings)
			{
				Debug.LogWarning("Serialized key count does not equal value count; possible data corruption / bad manual edit?", obj as UnityEngine.Object);
			}
			if (obj.SerializedStateKeys.Count == 0)
			{
				if (fiSettings.AutomaticReferenceInstantation)
				{
					fiISerializedObjectUtility.InstantiateReferences(obj, null);
				}
				return flag;
			}
			TSerializer tserializer = fiSingletons.Get<TSerializer>();
			ListSerializationOperator listSerializationOperator = fiSingletons.Get<ListSerializationOperator>();
			listSerializationOperator.SerializedObjects = obj.SerializedObjectReferences;
			InspectedType inspectedType = InspectedType.Get(obj.GetType());
			for (int i = 0; i < obj.SerializedStateKeys.Count; i++)
			{
				string text = obj.SerializedStateKeys[i];
				string text2 = obj.SerializedStateValues[i];
				InspectedProperty inspectedProperty = inspectedType.GetPropertyByName(text) ?? inspectedType.GetPropertyByFormerlySerializedName(text);
				if (inspectedProperty == null)
				{
					if (fiSettings.EmitWarnings)
					{
						Debug.LogWarning(string.Concat(new object[]
						{
							"Unable to find serialized property with name=",
							text,
							" on type ",
							obj.GetType()
						}), obj as UnityEngine.Object);
					}
				}
				else
				{
					object obj2 = null;
					if (!string.IsNullOrEmpty(text2))
					{
						try
						{
							obj2 = tserializer.Deserialize(inspectedProperty.MemberInfo, text2, listSerializationOperator);
						}
						catch (Exception ex)
						{
							flag = false;
							Debug.LogError(string.Concat(new object[] { "Exception caught when deserializing property <", text, "> in <", obj, ">\n", ex }), obj as UnityEngine.Object);
						}
					}
					try
					{
						inspectedProperty.Write(obj, obj2);
					}
					catch (Exception ex2)
					{
						flag = false;
						if (fiSettings.EmitWarnings)
						{
							Debug.LogWarning("Caught exception when updating property value; see next message for the exception", obj as UnityEngine.Object);
							Debug.LogError(ex2);
						}
					}
				}
			}
			if (serializationCallbacks != null)
			{
				serializationCallbacks.OnAfterDeserialize();
			}
			obj.IsRestored = true;
			return flag;
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x000912B8 File Offset: 0x0008F4B8
		private static void InstantiateReferences(object obj, InspectedType metadata)
		{
			if (metadata == null)
			{
				metadata = InspectedType.Get(obj.GetType());
			}
			if (metadata.IsCollection)
			{
				return;
			}
			List<InspectedProperty> properties = metadata.GetProperties(InspectedMemberFilters.InspectableMembers);
			for (int i = 0; i < properties.Count; i++)
			{
				InspectedProperty inspectedProperty = properties[i];
				if (inspectedProperty.StorageType.Resolve().IsClass)
				{
					if (!inspectedProperty.StorageType.Resolve().IsAbstract)
					{
						object obj2 = inspectedProperty.Read(obj);
						if (obj2 == null)
						{
							InspectedType inspectedType = InspectedType.Get(inspectedProperty.StorageType);
							if (inspectedType.HasDefaultConstructor)
							{
								object obj3 = inspectedType.CreateInstance();
								inspectedProperty.Write(obj, obj3);
								fiISerializedObjectUtility.InstantiateReferences(obj3, inspectedType);
							}
						}
					}
				}
			}
		}
	}
}
