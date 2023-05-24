using System;
using System.Collections.Generic;
using System.Linq;
using FullInspector.Internal;
using FullSerializer;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000618 RID: 1560
	public static class fiPersistentMetadata
	{
		// Token: 0x06002460 RID: 9312 RVA: 0x0009E1BC File Offset: 0x0009C3BC
		static fiPersistentMetadata()
		{
			for (int i = 0; i < fiPersistentMetadata.s_providers.Length; i++)
			{
				fiLog.Log(typeof(fiPersistentMetadata), "Using provider {0} to support metadata of type {1}", new object[]
				{
					fiPersistentMetadata.s_providers[i].GetType().CSharpName(),
					fiPersistentMetadata.s_providers[i].MetadataType.CSharpName()
				});
			}
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x0009E23C File Offset: 0x0009C43C
		public static fiGraphMetadata GetMetadataFor(UnityEngine.Object target_)
		{
			fiUnityObjectReference fiUnityObjectReference = new fiUnityObjectReference(target_);
			fiGraphMetadata fiGraphMetadata;
			if (!fiPersistentMetadata.s_metadata.TryGetValue(fiUnityObjectReference, out fiGraphMetadata))
			{
				fiGraphMetadata = new fiGraphMetadata(fiUnityObjectReference);
				fiPersistentMetadata.s_metadata[fiUnityObjectReference] = fiGraphMetadata;
				for (int i = 0; i < fiPersistentMetadata.s_providers.Length; i++)
				{
					fiPersistentMetadata.s_providers[i].RestoreData(fiUnityObjectReference.Target);
				}
			}
			return fiGraphMetadata;
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x0009E2A0 File Offset: 0x0009C4A0
		public static void Reset(UnityEngine.Object target_)
		{
			fiUnityObjectReference fiUnityObjectReference = new fiUnityObjectReference(target_);
			if (fiPersistentMetadata.s_metadata.ContainsKey(fiUnityObjectReference))
			{
				fiPersistentMetadata.s_metadata.Remove(fiUnityObjectReference);
				for (int i = 0; i < fiPersistentMetadata.s_providers.Length; i++)
				{
					fiPersistentMetadata.s_providers[i].Reset(fiUnityObjectReference.Target);
				}
			}
		}

		// Token: 0x04001927 RID: 6439
		private static readonly fiIPersistentMetadataProvider[] s_providers = fiRuntimeReflectionUtility.GetAssemblyInstances<fiIPersistentMetadataProvider>().ToArray<fiIPersistentMetadataProvider>();

		// Token: 0x04001928 RID: 6440
		private static Dictionary<fiUnityObjectReference, fiGraphMetadata> s_metadata = new Dictionary<fiUnityObjectReference, fiGraphMetadata>();
	}
}
