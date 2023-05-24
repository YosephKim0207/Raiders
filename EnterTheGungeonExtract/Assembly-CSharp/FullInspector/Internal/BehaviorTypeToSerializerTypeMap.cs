using System;
using System.Collections.Generic;
using FullSerializer.Internal;

namespace FullInspector.Internal
{
	// Token: 0x02000544 RID: 1348
	public static class BehaviorTypeToSerializerTypeMap
	{
		// Token: 0x06002013 RID: 8211 RVA: 0x0008EF88 File Offset: 0x0008D188
		public static void Register(Type behaviorType, Type serializerType)
		{
			BehaviorTypeToSerializerTypeMap._mappings.Add(new BehaviorTypeToSerializerTypeMap.SerializationMapping
			{
				BehaviorType = behaviorType,
				SerializerType = serializerType
			});
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x0008EFB8 File Offset: 0x0008D1B8
		public static Type GetSerializerType(Type behaviorType)
		{
			for (int i = 0; i < BehaviorTypeToSerializerTypeMap._mappings.Count; i++)
			{
				BehaviorTypeToSerializerTypeMap.SerializationMapping serializationMapping = BehaviorTypeToSerializerTypeMap._mappings[i];
				if (serializationMapping.BehaviorType.Resolve().IsAssignableFrom(behaviorType.Resolve()))
				{
					return serializationMapping.SerializerType;
				}
			}
			return fiInstalledSerializerManager.DefaultMetadata.SerializerType;
		}

		// Token: 0x0400177C RID: 6012
		private static List<BehaviorTypeToSerializerTypeMap.SerializationMapping> _mappings = new List<BehaviorTypeToSerializerTypeMap.SerializationMapping>();

		// Token: 0x02000545 RID: 1349
		private struct SerializationMapping
		{
			// Token: 0x0400177D RID: 6013
			public Type BehaviorType;

			// Token: 0x0400177E RID: 6014
			public Type SerializerType;
		}
	}
}
