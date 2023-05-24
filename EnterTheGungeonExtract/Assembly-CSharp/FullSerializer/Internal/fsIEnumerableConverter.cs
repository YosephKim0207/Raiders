using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FullSerializer.Internal
{
	// Token: 0x0200058D RID: 1421
	public class fsIEnumerableConverter : fsConverter
	{
		// Token: 0x060021AE RID: 8622 RVA: 0x0009469C File Offset: 0x0009289C
		public override bool CanProcess(Type type)
		{
			return typeof(IEnumerable).IsAssignableFrom(type) && fsIEnumerableConverter.GetAddMethod(type) != null;
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x000946C4 File Offset: 0x000928C4
		public override object CreateInstance(fsData data, Type storageType)
		{
			return fsMetaType.Get(storageType).CreateInstance();
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000946D4 File Offset: 0x000928D4
		public override fsResult TrySerialize(object instance_, out fsData serialized, Type storageType)
		{
			IEnumerable enumerable = (IEnumerable)instance_;
			fsResult success = fsResult.Success;
			Type elementType = fsIEnumerableConverter.GetElementType(storageType);
			serialized = fsData.CreateList(fsIEnumerableConverter.HintSize(enumerable));
			List<fsData> asList = serialized.AsList;
			IEnumerator enumerator = enumerable.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					fsData fsData;
					fsResult fsResult = this.Serializer.TrySerialize(elementType, obj, out fsData);
					success.AddMessages(fsResult);
					if (!fsResult.Failed)
					{
						asList.Add(fsData);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
			return success;
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x00094790 File Offset: 0x00092990
		public override fsResult TryDeserialize(fsData data, ref object instance_, Type storageType)
		{
			IEnumerable enumerable = (IEnumerable)instance_;
			fsResult fsResult = fsResult.Success;
			fsResult fsResult2;
			fsResult = (fsResult2 = fsResult + base.CheckType(data, fsDataType.Array));
			if (fsResult2.Failed)
			{
				return fsResult;
			}
			Type elementType = fsIEnumerableConverter.GetElementType(storageType);
			MethodInfo addMethod = fsIEnumerableConverter.GetAddMethod(storageType);
			MethodInfo flattenedMethod = storageType.GetFlattenedMethod("get_Item");
			MethodInfo flattenedMethod2 = storageType.GetFlattenedMethod("set_Item");
			if (flattenedMethod2 == null)
			{
				fsIEnumerableConverter.TryClear(storageType, enumerable);
			}
			int num = fsIEnumerableConverter.TryGetExistingSize(storageType, enumerable);
			List<fsData> asList = data.AsList;
			for (int i = 0; i < asList.Count; i++)
			{
				fsData fsData = asList[i];
				object obj = null;
				if (flattenedMethod != null && i < num)
				{
					obj = flattenedMethod.Invoke(enumerable, new object[] { i });
				}
				fsResult fsResult3 = this.Serializer.TryDeserialize(fsData, elementType, ref obj);
				fsResult.AddMessages(fsResult3);
				if (!fsResult3.Failed)
				{
					if (flattenedMethod2 != null && i < num)
					{
						flattenedMethod2.Invoke(enumerable, new object[] { i, obj });
					}
					else
					{
						addMethod.Invoke(enumerable, new object[] { obj });
					}
				}
			}
			return fsResult;
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000948DC File Offset: 0x00092ADC
		private static int HintSize(IEnumerable collection)
		{
			if (collection is ICollection)
			{
				return ((ICollection)collection).Count;
			}
			return 0;
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000948F8 File Offset: 0x00092AF8
		private static Type GetElementType(Type objectType)
		{
			if (objectType.HasElementType)
			{
				return objectType.GetElementType();
			}
			Type @interface = fsReflectionUtility.GetInterface(objectType, typeof(IEnumerable<>));
			if (@interface != null)
			{
				return @interface.GetGenericArguments()[0];
			}
			return typeof(object);
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x00094944 File Offset: 0x00092B44
		private static void TryClear(Type type, object instance)
		{
			MethodInfo flattenedMethod = type.GetFlattenedMethod("Clear");
			if (flattenedMethod != null)
			{
				flattenedMethod.Invoke(instance, null);
			}
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x0009496C File Offset: 0x00092B6C
		private static int TryGetExistingSize(Type type, object instance)
		{
			PropertyInfo flattenedProperty = type.GetFlattenedProperty("Count");
			if (flattenedProperty != null)
			{
				return (int)flattenedProperty.GetGetMethod().Invoke(instance, null);
			}
			return 0;
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000949A0 File Offset: 0x00092BA0
		private static MethodInfo GetAddMethod(Type type)
		{
			Type @interface = fsReflectionUtility.GetInterface(type, typeof(ICollection<>));
			if (@interface != null)
			{
				MethodInfo declaredMethod = @interface.GetDeclaredMethod("Add");
				if (declaredMethod != null)
				{
					return declaredMethod;
				}
			}
			MethodInfo methodInfo;
			if ((methodInfo = type.GetFlattenedMethod("Add")) == null)
			{
				methodInfo = type.GetFlattenedMethod("Push") ?? type.GetFlattenedMethod("Enqueue");
			}
			return methodInfo;
		}
	}
}
