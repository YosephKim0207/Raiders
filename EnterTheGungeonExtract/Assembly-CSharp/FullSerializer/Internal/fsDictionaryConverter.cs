using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FullSerializer.Internal
{
	// Token: 0x0200058A RID: 1418
	public class fsDictionaryConverter : fsConverter
	{
		// Token: 0x06002198 RID: 8600 RVA: 0x00093E80 File Offset: 0x00092080
		public override bool CanProcess(Type type)
		{
			return typeof(IDictionary).IsAssignableFrom(type);
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x00093E94 File Offset: 0x00092094
		public override object CreateInstance(fsData data, Type storageType)
		{
			return fsMetaType.Get(storageType).CreateInstance();
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x00093EA4 File Offset: 0x000920A4
		public override fsResult TryDeserialize(fsData data, ref object instance_, Type storageType)
		{
			IDictionary dictionary = (IDictionary)instance_;
			fsResult fsResult = fsResult.Success;
			Type type;
			Type type2;
			fsDictionaryConverter.GetKeyValueTypes(dictionary.GetType(), out type, out type2);
			if (data.IsList)
			{
				List<fsData> asList = data.AsList;
				for (int i = 0; i < asList.Count; i++)
				{
					fsData fsData = asList[i];
					fsResult fsResult2;
					fsResult = (fsResult2 = fsResult + base.CheckType(fsData, fsDataType.Object));
					if (fsResult2.Failed)
					{
						return fsResult;
					}
					fsData fsData2;
					fsResult fsResult3;
					fsResult = (fsResult3 = fsResult + base.CheckKey(fsData, "Key", out fsData2));
					if (fsResult3.Failed)
					{
						return fsResult;
					}
					fsData fsData3;
					fsResult fsResult4;
					fsResult = (fsResult4 = fsResult + base.CheckKey(fsData, "Value", out fsData3));
					if (fsResult4.Failed)
					{
						return fsResult;
					}
					object obj = null;
					object obj2 = null;
					fsResult fsResult5;
					fsResult = (fsResult5 = fsResult + this.Serializer.TryDeserialize(fsData2, type, ref obj));
					if (fsResult5.Failed)
					{
						return fsResult;
					}
					fsResult fsResult6;
					fsResult = (fsResult6 = fsResult + this.Serializer.TryDeserialize(fsData3, type2, ref obj2));
					if (fsResult6.Failed)
					{
						return fsResult;
					}
					this.AddItemToDictionary(dictionary, obj, obj2);
				}
			}
			else
			{
				if (data.IsDictionary)
				{
					foreach (KeyValuePair<string, fsData> keyValuePair in data.AsDictionary)
					{
						if (!fsSerializer.IsReservedKeyword(keyValuePair.Key))
						{
							fsData fsData4 = new fsData(keyValuePair.Key);
							fsData value = keyValuePair.Value;
							object obj3 = null;
							object obj4 = null;
							fsResult fsResult7;
							fsResult = (fsResult7 = fsResult + this.Serializer.TryDeserialize(fsData4, type, ref obj3));
							if (fsResult7.Failed)
							{
								return fsResult;
							}
							fsResult fsResult8;
							fsResult = (fsResult8 = fsResult + this.Serializer.TryDeserialize(value, type2, ref obj4));
							if (fsResult8.Failed)
							{
								return fsResult;
							}
							this.AddItemToDictionary(dictionary, obj3, obj4);
						}
					}
					return fsResult;
				}
				return base.FailExpectedType(data, new fsDataType[]
				{
					fsDataType.Array,
					fsDataType.Object
				});
			}
			return fsResult;
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x000940EC File Offset: 0x000922EC
		public override fsResult TrySerialize(object instance_, out fsData serialized, Type storageType)
		{
			serialized = fsData.Null;
			fsResult fsResult = fsResult.Success;
			IDictionary dictionary = (IDictionary)instance_;
			Type type;
			Type type2;
			fsDictionaryConverter.GetKeyValueTypes(dictionary.GetType(), out type, out type2);
			IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
			bool flag = true;
			List<fsData> list = new List<fsData>(dictionary.Count);
			List<fsData> list2 = new List<fsData>(dictionary.Count);
			while (enumerator.MoveNext())
			{
				fsData fsData;
				fsResult fsResult2;
				fsResult = (fsResult2 = fsResult + this.Serializer.TrySerialize(type, enumerator.Key, out fsData));
				if (fsResult2.Failed)
				{
					return fsResult;
				}
				fsData fsData2;
				fsResult fsResult3;
				fsResult = (fsResult3 = fsResult + this.Serializer.TrySerialize(type2, enumerator.Value, out fsData2));
				if (fsResult3.Failed)
				{
					return fsResult;
				}
				list.Add(fsData);
				list2.Add(fsData2);
				flag &= fsData.IsString;
			}
			if (flag)
			{
				serialized = fsData.CreateDictionary();
				Dictionary<string, fsData> asDictionary = serialized.AsDictionary;
				for (int i = 0; i < list.Count; i++)
				{
					fsData fsData3 = list[i];
					fsData fsData4 = list2[i];
					asDictionary[fsData3.AsString] = fsData4;
				}
			}
			else
			{
				serialized = fsData.CreateList(list.Count);
				List<fsData> asList = serialized.AsList;
				for (int j = 0; j < list.Count; j++)
				{
					fsData fsData5 = list[j];
					fsData fsData6 = list2[j];
					Dictionary<string, fsData> dictionary2 = new Dictionary<string, fsData>();
					dictionary2["Key"] = fsData5;
					dictionary2["Value"] = fsData6;
					asList.Add(new fsData(dictionary2));
				}
			}
			return fsResult;
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x000942A4 File Offset: 0x000924A4
		private fsResult AddItemToDictionary(IDictionary dictionary, object key, object value)
		{
			if (key != null && value != null)
			{
				dictionary[key] = value;
				return fsResult.Success;
			}
			Type @interface = fsReflectionUtility.GetInterface(dictionary.GetType(), typeof(ICollection<>));
			if (@interface == null)
			{
				return fsResult.Warn(dictionary.GetType() + " does not extend ICollection");
			}
			Type type = @interface.GetGenericArguments()[0];
			object obj = Activator.CreateInstance(type, new object[] { key, value });
			MethodInfo flattenedMethod = @interface.GetFlattenedMethod("Add");
			if (key == null)
			{
				return fsResult.Fail("Null entry in dictionary.");
			}
			flattenedMethod.Invoke(dictionary, new object[] { obj });
			return fsResult.Success;
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x00094350 File Offset: 0x00092550
		private static void GetKeyValueTypes(Type dictionaryType, out Type keyStorageType, out Type valueStorageType)
		{
			Type @interface = fsReflectionUtility.GetInterface(dictionaryType, typeof(IDictionary<, >));
			if (@interface != null)
			{
				Type[] genericArguments = @interface.GetGenericArguments();
				keyStorageType = genericArguments[0];
				valueStorageType = genericArguments[1];
			}
			else
			{
				keyStorageType = typeof(object);
				valueStorageType = typeof(object);
			}
		}
	}
}
