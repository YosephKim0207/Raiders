using System;
using System.Collections.Generic;

namespace FullInspector.Internal
{
	// Token: 0x020005CE RID: 1486
	public static class fiInstalledSerializerManager
	{
		// Token: 0x0600233B RID: 9019 RVA: 0x0009AA88 File Offset: 0x00098C88
		static fiInstalledSerializerManager()
		{
			List<Type> list = new List<Type>();
			List<Type> list2 = new List<Type>();
			fiInstalledSerializerManager.LoadedMetadata = new List<fiISerializerMetadata>();
			fiILoadedSerializers fiILoadedSerializers;
			if (fiInstalledSerializerManager.TryGetLoadedSerializerType(out fiILoadedSerializers))
			{
				fiInstalledSerializerManager._defaultMetadata = fiInstalledSerializerManager.GetProvider(fiILoadedSerializers.DefaultSerializerProvider);
				foreach (Type type in fiILoadedSerializers.AllLoadedSerializerProviders)
				{
					fiISerializerMetadata provider = fiInstalledSerializerManager.GetProvider(type);
					fiInstalledSerializerManager.LoadedMetadata.Add(provider);
					list.AddRange(provider.SerializationOptInAnnotationTypes);
					list2.AddRange(provider.SerializationOptOutAnnotationTypes);
				}
			}
			foreach (Type type2 in fiRuntimeReflectionUtility.AllSimpleTypesDerivingFrom(typeof(fiISerializerMetadata)))
			{
				fiISerializerMetadata provider2 = fiInstalledSerializerManager.GetProvider(type2);
				fiInstalledSerializerManager.LoadedMetadata.Add(provider2);
				list.AddRange(provider2.SerializationOptInAnnotationTypes);
				list2.AddRange(provider2.SerializationOptOutAnnotationTypes);
			}
			fiInstalledSerializerManager.SerializationOptInAnnotations = list.ToArray();
			fiInstalledSerializerManager.SerializationOptOutAnnotations = list2.ToArray();
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x0009ABB8 File Offset: 0x00098DB8
		private static fiISerializerMetadata GetProvider(Type type)
		{
			return (fiISerializerMetadata)Activator.CreateInstance(type);
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x0009ABC8 File Offset: 0x00098DC8
		public static bool TryGetLoadedSerializerType(out fiILoadedSerializers serializers)
		{
			string text = "FullInspector.Internal.fiLoadedSerializers";
			TypeCache.Reset();
			Type type = TypeCache.FindType(text);
			if (type == null)
			{
				serializers = null;
				return false;
			}
			serializers = (fiILoadedSerializers)Activator.CreateInstance(type);
			return true;
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x0600233E RID: 9022 RVA: 0x0009AC00 File Offset: 0x00098E00
		// (set) Token: 0x0600233F RID: 9023 RVA: 0x0009AC08 File Offset: 0x00098E08
		public static List<fiISerializerMetadata> LoadedMetadata { get; private set; }

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06002340 RID: 9024 RVA: 0x0009AC10 File Offset: 0x00098E10
		public static fiISerializerMetadata DefaultMetadata
		{
			get
			{
				if (fiInstalledSerializerManager._defaultMetadata == null)
				{
					throw new InvalidOperationException("Please register a default serializer. You should see a popup window on the next serialization reload.");
				}
				return fiInstalledSerializerManager._defaultMetadata;
			}
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x0009AC2C File Offset: 0x00098E2C
		public static bool IsLoaded(Guid serializerGuid)
		{
			if (fiInstalledSerializerManager.LoadedMetadata == null)
			{
				return false;
			}
			for (int i = 0; i < fiInstalledSerializerManager.LoadedMetadata.Count; i++)
			{
				if (fiInstalledSerializerManager.LoadedMetadata[i].SerializerGuid == serializerGuid)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002342 RID: 9026 RVA: 0x0009AC80 File Offset: 0x00098E80
		public static bool HasDefault
		{
			get
			{
				return fiInstalledSerializerManager._defaultMetadata != null;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06002343 RID: 9027 RVA: 0x0009AC90 File Offset: 0x00098E90
		// (set) Token: 0x06002344 RID: 9028 RVA: 0x0009AC98 File Offset: 0x00098E98
		public static Type[] SerializationOptInAnnotations { get; private set; }

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06002345 RID: 9029 RVA: 0x0009ACA0 File Offset: 0x00098EA0
		// (set) Token: 0x06002346 RID: 9030 RVA: 0x0009ACA8 File Offset: 0x00098EA8
		public static Type[] SerializationOptOutAnnotations { get; private set; }

		// Token: 0x04001895 RID: 6293
		public const string GeneratedTypeName = "fiLoadedSerializers";

		// Token: 0x04001897 RID: 6295
		private static fiISerializerMetadata _defaultMetadata;
	}
}
