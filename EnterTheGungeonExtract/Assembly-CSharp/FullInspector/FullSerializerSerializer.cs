using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FullInspector.Serializers.FullSerializer;
using FullSerializer;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000587 RID: 1415
	public class FullSerializerSerializer : BaseSerializer
	{
		// Token: 0x06002182 RID: 8578 RVA: 0x00093748 File Offset: 0x00091948
		static FullSerializerSerializer()
		{
			FullSerializerSerializer.AddConverter<UnityObjectConverter>();
			FullSerializerSerializer.AddProcessor<SerializationCallbackReceiverObjectProcessor>();
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x0009377C File Offset: 0x0009197C
		private static fsSerializer Serializer
		{
			get
			{
				if (FullSerializerSerializer._serializer == null)
				{
					object typeFromHandle = typeof(FullSerializerSerializer);
					lock (typeFromHandle)
					{
						FullSerializerSerializer._serializer = new fsSerializer();
						FullSerializerSerializer._serializers.Add(FullSerializerSerializer._serializer);
						foreach (Type type in FullSerializerSerializer._converters)
						{
							FullSerializerSerializer._serializer.AddConverter((fsConverter)Activator.CreateInstance(type));
						}
						foreach (Type type2 in FullSerializerSerializer._processors)
						{
							FullSerializerSerializer._serializer.AddProcessor((fsObjectProcessor)Activator.CreateInstance(type2));
						}
					}
				}
				return FullSerializerSerializer._serializer;
			}
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x00093890 File Offset: 0x00091A90
		public static void AddConverter<TConverter>() where TConverter : fsConverter, new()
		{
			object typeFromHandle = typeof(FullSerializerSerializer);
			lock (typeFromHandle)
			{
				FullSerializerSerializer._converters.Add(typeof(TConverter));
				foreach (fsSerializer fsSerializer in FullSerializerSerializer._serializers)
				{
					fsSerializer.AddConverter(new TConverter());
				}
			}
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x00093934 File Offset: 0x00091B34
		public static void AddProcessor<TProcessor>() where TProcessor : fsObjectProcessor, new()
		{
			object typeFromHandle = typeof(FullSerializerSerializer);
			lock (typeFromHandle)
			{
				FullSerializerSerializer._processors.Add(typeof(TProcessor));
				foreach (fsSerializer fsSerializer in FullSerializerSerializer._serializers)
				{
					fsSerializer.AddProcessor(new TProcessor());
				}
			}
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x000939D8 File Offset: 0x00091BD8
		public override string Serialize(MemberInfo storageType, object value, ISerializationOperator serializationOperator)
		{
			FullSerializerSerializer.Serializer.Context.Set<ISerializationOperator>(serializationOperator);
			fsData fsData;
			fsResult fsResult = FullSerializerSerializer.Serializer.TrySerialize(BaseSerializer.GetStorageType(storageType), value, out fsData);
			if (FullSerializerSerializer.EmitFailWarning(fsResult))
			{
				return null;
			}
			if (fiSettings.PrettyPrintSerializedJson)
			{
				return fsJsonPrinter.PrettyJson(fsData);
			}
			return fsJsonPrinter.CompressedJson(fsData);
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x00093A30 File Offset: 0x00091C30
		public override object Deserialize(MemberInfo storageType, string serializedState, ISerializationOperator serializationOperator)
		{
			fsData fsData;
			fsResult fsResult = fsJsonParser.Parse(serializedState, out fsData);
			if (FullSerializerSerializer.EmitFailWarning(fsResult))
			{
				return null;
			}
			FullSerializerSerializer.Serializer.Context.Set<ISerializationOperator>(serializationOperator);
			object obj = null;
			fsResult = FullSerializerSerializer.Serializer.TryDeserialize(fsData, BaseSerializer.GetStorageType(storageType), ref obj);
			if (FullSerializerSerializer.EmitFailWarning(fsResult))
			{
				return null;
			}
			return obj;
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06002189 RID: 8585 RVA: 0x00093A88 File Offset: 0x00091C88
		public override bool SupportsMultithreading
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x00093A8C File Offset: 0x00091C8C
		private static bool EmitFailWarning(fsResult result)
		{
			if (fiSettings.EmitWarnings && result.RawMessages.Any<string>())
			{
				Debug.LogWarning(result.FormattedMessages);
			}
			return result.Failed;
		}

		// Token: 0x0400181D RID: 6173
		[ThreadStatic]
		private static fsSerializer _serializer;

		// Token: 0x0400181E RID: 6174
		private static readonly List<fsSerializer> _serializers = new List<fsSerializer>();

		// Token: 0x0400181F RID: 6175
		private static readonly List<Type> _converters = new List<Type>();

		// Token: 0x04001820 RID: 6176
		private static readonly List<Type> _processors = new List<Type>();
	}
}
