using System;
using System.Collections.Generic;
using System.Linq;
using FullSerializer.Internal;

namespace FullSerializer
{
	// Token: 0x0200059D RID: 1437
	public abstract class fsBaseConverter
	{
		// Token: 0x06002200 RID: 8704 RVA: 0x00095EE4 File Offset: 0x000940E4
		public virtual object CreateInstance(fsData data, Type storageType)
		{
			if (this.RequestCycleSupport(storageType))
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Please override CreateInstance for ",
					base.GetType().FullName,
					"; the object graph for ",
					storageType,
					" can contain potentially contain cycles, so separated instance creation is needed"
				}));
			}
			return storageType;
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x00095F3C File Offset: 0x0009413C
		public virtual bool RequestCycleSupport(Type storageType)
		{
			return storageType != typeof(string) && (storageType.Resolve().IsClass || storageType.Resolve().IsInterface);
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x00095F70 File Offset: 0x00094170
		public virtual bool RequestInheritanceSupport(Type storageType)
		{
			return !storageType.Resolve().IsSealed;
		}

		// Token: 0x06002203 RID: 8707
		public abstract fsResult TrySerialize(object instance, out fsData serialized, Type storageType);

		// Token: 0x06002204 RID: 8708
		public abstract fsResult TryDeserialize(fsData data, ref object instance, Type storageType);

		// Token: 0x06002205 RID: 8709 RVA: 0x00095F80 File Offset: 0x00094180
		protected fsResult FailExpectedType(fsData data, params fsDataType[] types)
		{
			object[] array = new object[7];
			array[0] = base.GetType().Name;
			array[1] = " expected one of ";
			array[2] = string.Join(", ", types.Select((fsDataType t) => t.ToString()).ToArray<string>());
			array[3] = " but got ";
			array[4] = data.Type;
			array[5] = " in ";
			array[6] = data;
			return fsResult.Fail(string.Concat(array));
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x0009600C File Offset: 0x0009420C
		protected fsResult CheckType(fsData data, fsDataType type)
		{
			if (data.Type != type)
			{
				return fsResult.Fail(string.Concat(new object[]
				{
					base.GetType().Name,
					" expected ",
					type,
					" but got ",
					data.Type,
					" in ",
					data
				}));
			}
			return fsResult.Success;
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x0009607C File Offset: 0x0009427C
		protected fsResult CheckKey(fsData data, string key, out fsData subitem)
		{
			return this.CheckKey(data.AsDictionary, key, out subitem);
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x0009608C File Offset: 0x0009428C
		protected fsResult CheckKey(Dictionary<string, fsData> data, string key, out fsData subitem)
		{
			if (!data.TryGetValue(key, out subitem))
			{
				return fsResult.Fail(string.Concat(new object[]
				{
					base.GetType().Name,
					" requires a <",
					key,
					"> key in the data ",
					data
				}));
			}
			return fsResult.Success;
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x000960E4 File Offset: 0x000942E4
		protected fsResult SerializeMember<T>(Dictionary<string, fsData> data, string name, T value)
		{
			fsData fsData;
			fsResult fsResult = this.Serializer.TrySerialize(typeof(T), value, out fsData);
			if (fsResult.Succeeded)
			{
				data[name] = fsData;
			}
			return fsResult;
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x00096124 File Offset: 0x00094324
		protected fsResult DeserializeMember<T>(Dictionary<string, fsData> data, string name, out T value)
		{
			fsData fsData;
			if (!data.TryGetValue(name, out fsData))
			{
				value = default(T);
				return fsResult.Fail("Unable to find member \"" + name + "\"");
			}
			object obj = null;
			fsResult fsResult = this.Serializer.TryDeserialize(fsData, typeof(T), ref obj);
			value = (T)((object)obj);
			return fsResult;
		}

		// Token: 0x0400182F RID: 6191
		public fsSerializer Serializer;
	}
}
