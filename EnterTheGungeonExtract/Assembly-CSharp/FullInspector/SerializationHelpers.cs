using System;
using System.Collections.Generic;
using FullInspector.Internal;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x020005CC RID: 1484
	public static class SerializationHelpers
	{
		// Token: 0x06002333 RID: 9011 RVA: 0x0009A984 File Offset: 0x00098B84
		public static T DeserializeFromContent<T, TSerializer>(string content) where TSerializer : BaseSerializer
		{
			return (T)((object)SerializationHelpers.DeserializeFromContent<TSerializer>(typeof(T), content));
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0009A99C File Offset: 0x00098B9C
		public static object DeserializeFromContent<TSerializer>(Type storageType, string content) where TSerializer : BaseSerializer
		{
			TSerializer tserializer = fiSingletons.Get<TSerializer>();
			NotSupportedSerializationOperator notSupportedSerializationOperator = fiSingletons.Get<NotSupportedSerializationOperator>();
			return tserializer.Deserialize(fsPortableReflection.AsMemberInfo(storageType), content, notSupportedSerializationOperator);
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x0009A9CC File Offset: 0x00098BCC
		public static string SerializeToContent<T, TSerializer>(T value) where TSerializer : BaseSerializer
		{
			return SerializationHelpers.SerializeToContent<TSerializer>(typeof(T), value);
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x0009A9E4 File Offset: 0x00098BE4
		public static string SerializeToContent<TSerializer>(Type storageType, object value) where TSerializer : BaseSerializer
		{
			TSerializer tserializer = fiSingletons.Get<TSerializer>();
			NotSupportedSerializationOperator notSupportedSerializationOperator = fiSingletons.Get<NotSupportedSerializationOperator>();
			return tserializer.Serialize(fsPortableReflection.AsMemberInfo(storageType), value, notSupportedSerializationOperator);
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0009AA14 File Offset: 0x00098C14
		public static T Clone<T, TSerializer>(T obj) where TSerializer : BaseSerializer
		{
			return (T)((object)SerializationHelpers.Clone<TSerializer>(typeof(T), obj));
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x0009AA30 File Offset: 0x00098C30
		public static object Clone<TSerializer>(Type storageType, object obj) where TSerializer : BaseSerializer
		{
			TSerializer tserializer = fiSingletons.Get<TSerializer>();
			ListSerializationOperator listSerializationOperator = fiSingletons.Get<ListSerializationOperator>();
			listSerializationOperator.SerializedObjects = new List<UnityEngine.Object>();
			string text = tserializer.Serialize(fsPortableReflection.AsMemberInfo(storageType), obj, listSerializationOperator);
			object obj2 = tserializer.Deserialize(fsPortableReflection.AsMemberInfo(storageType), text, listSerializationOperator);
			listSerializationOperator.SerializedObjects = null;
			return obj2;
		}
	}
}
