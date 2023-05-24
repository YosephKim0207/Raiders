using System;
using System.Collections.Generic;
using FullInspector.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000556 RID: 1366
	public class fiGraphMetadata
	{
		// Token: 0x0600205F RID: 8287 RVA: 0x0008FB94 File Offset: 0x0008DD94
		public fiGraphMetadata()
			: this(null)
		{
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x0008FBA0 File Offset: 0x0008DDA0
		public fiGraphMetadata(fiUnityObjectReference targetObject)
			: this(null, string.Empty)
		{
			this._targetObject = targetObject;
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x0008FBB8 File Offset: 0x0008DDB8
		private fiGraphMetadata(fiGraphMetadata parentMetadata, string accessKey)
		{
			this._childrenInt = new CullableDictionary<int, fiGraphMetadata, IntDictionary<fiGraphMetadata>>();
			this._childrenString = new CullableDictionary<string, fiGraphMetadata, Dictionary<string, fiGraphMetadata>>();
			this._metadata = new CullableDictionary<Type, object, Dictionary<Type, object>>();
			this._parentMetadata = parentMetadata;
			if (this._parentMetadata == null)
			{
				this._precomputedData = new Dictionary<string, List<object>>();
			}
			else
			{
				this._precomputedData = this._parentMetadata._precomputedData;
			}
			this.RebuildAccessPath(accessKey);
			if (this._precomputedData.ContainsKey(this._accessPath))
			{
				foreach (object obj in this._precomputedData[this._accessPath])
				{
					this._metadata[obj.GetType()] = obj;
				}
			}
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x0008FCA4 File Offset: 0x0008DEA4
		public bool ShouldSerialize()
		{
			return !this._childrenInt.IsEmpty || !this._childrenString.IsEmpty;
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x0008FCC8 File Offset: 0x0008DEC8
		public void Serialize<TPersistentData>(out string[] keys_, out TPersistentData[] values_) where TPersistentData : IGraphMetadataItemPersistent
		{
			List<string> list = new List<string>();
			List<TPersistentData> list2 = new List<TPersistentData>();
			this.AddSerializeData<TPersistentData>(list, list2);
			keys_ = list.ToArray();
			values_ = list2.ToArray();
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x0008FCFC File Offset: 0x0008DEFC
		private void AddSerializeData<TPersistentData>(List<string> keys, List<TPersistentData> values) where TPersistentData : IGraphMetadataItemPersistent
		{
			foreach (KeyValuePair<Type, object> keyValuePair in this._metadata.Items)
			{
				if (keyValuePair.Key == typeof(TPersistentData) && ((IGraphMetadataItemPersistent)keyValuePair.Value).ShouldSerialize())
				{
					keys.Add(this._accessPath);
					values.Add((TPersistentData)((object)keyValuePair.Value));
				}
			}
			foreach (KeyValuePair<int, fiGraphMetadata> keyValuePair2 in this._childrenInt.Items)
			{
				keyValuePair2.Value.AddSerializeData<TPersistentData>(keys, values);
			}
			foreach (KeyValuePair<string, fiGraphMetadata> keyValuePair3 in this._childrenString.Items)
			{
				keyValuePair3.Value.AddSerializeData<TPersistentData>(keys, values);
			}
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x0008FE4C File Offset: 0x0008E04C
		public void Deserialize<TPersistentData>(string[] keys, TPersistentData[] values)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				string text = keys[i];
				List<object> list;
				if (!this._precomputedData.TryGetValue(text, out list))
				{
					list = new List<object>();
					this._precomputedData[text] = list;
				}
				list.Add(values[i]);
			}
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x0008FEAC File Offset: 0x0008E0AC
		public void BeginCullZone()
		{
			this._childrenInt.BeginCullZone();
			this._childrenString.BeginCullZone();
			this._metadata.BeginCullZone();
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x0008FED0 File Offset: 0x0008E0D0
		public void EndCullZone()
		{
			this._childrenInt.EndCullZone();
			this._childrenString.EndCullZone();
			this._metadata.EndCullZone();
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x0008FEF4 File Offset: 0x0008E0F4
		private UnityEngine.Object TargetObject
		{
			get
			{
				if (this._targetObject != null && this._targetObject.IsValid)
				{
					return this._targetObject.Target;
				}
				if (this._parentMetadata != null)
				{
					return this._parentMetadata.TargetObject;
				}
				return null;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06002069 RID: 8297 RVA: 0x0008FF40 File Offset: 0x0008E140
		public string Path
		{
			get
			{
				return this._accessPath;
			}
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x0008FF48 File Offset: 0x0008E148
		private void RebuildAccessPath(string accessKey)
		{
			this._accessPath = string.Empty;
			if (this._parentMetadata != null && !string.IsNullOrEmpty(this._parentMetadata._accessPath))
			{
				this._accessPath = this._accessPath + this._parentMetadata._accessPath + ".";
			}
			this._accessPath += accessKey;
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x0008FFB4 File Offset: 0x0008E1B4
		public void SetChild(int identifier, fiGraphMetadata metadata)
		{
			this._childrenInt[identifier] = metadata;
			metadata.RebuildAccessPath(identifier.ToString());
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x0008FFD8 File Offset: 0x0008E1D8
		public void SetChild(string identifier, fiGraphMetadata metadata)
		{
			this._childrenString[identifier] = metadata;
			metadata.RebuildAccessPath(identifier);
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x0008FFF0 File Offset: 0x0008E1F0
		public static void MigrateMetadata<T>(fiGraphMetadata metadata, T[] previous, T[] updated)
		{
			List<fiGraphMetadata.MetadataMigration> list = fiGraphMetadata.ComputeNeededMigrations<T>(metadata, previous, updated);
			string[] array = new string[list.Count];
			string[] array2 = new string[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				array[i] = metadata.Enter(list[i].OldIndex).Metadata._accessPath;
				array2[i] = metadata.Enter(list[i].NewIndex).Metadata._accessPath;
			}
			List<fiGraphMetadata> list2 = new List<fiGraphMetadata>(list.Count);
			for (int j = 0; j < list.Count; j++)
			{
				list2.Add(metadata._childrenInt[j]);
			}
			for (int k = 0; k < list.Count; k++)
			{
				metadata._childrenInt[list[k].NewIndex] = list2[k];
			}
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x00090100 File Offset: 0x0008E300
		private static List<fiGraphMetadata.MetadataMigration> ComputeNeededMigrations<T>(fiGraphMetadata metadata, T[] previous, T[] updated)
		{
			List<fiGraphMetadata.MetadataMigration> list = new List<fiGraphMetadata.MetadataMigration>();
			for (int i = 0; i < updated.Length; i++)
			{
				int num = Array.IndexOf<T>(previous, updated[i]);
				if (num != -1 && num != i)
				{
					list.Add(new fiGraphMetadata.MetadataMigration
					{
						NewIndex = i,
						OldIndex = num
					});
				}
			}
			return list;
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x00090164 File Offset: 0x0008E364
		public fiGraphMetadataChild Enter(int childIdentifier)
		{
			fiGraphMetadata fiGraphMetadata;
			if (!this._childrenInt.TryGetValue(childIdentifier, out fiGraphMetadata))
			{
				fiGraphMetadata = new fiGraphMetadata(this, childIdentifier.ToString());
				this._childrenInt[childIdentifier] = fiGraphMetadata;
			}
			return new fiGraphMetadataChild
			{
				Metadata = fiGraphMetadata
			};
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x000901B8 File Offset: 0x0008E3B8
		public fiGraphMetadataChild Enter(string childIdentifier)
		{
			fiGraphMetadata fiGraphMetadata;
			if (!this._childrenString.TryGetValue(childIdentifier, out fiGraphMetadata))
			{
				fiGraphMetadata = new fiGraphMetadata(this, childIdentifier);
				this._childrenString[childIdentifier] = fiGraphMetadata;
			}
			return new fiGraphMetadataChild
			{
				Metadata = fiGraphMetadata
			};
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x00090200 File Offset: 0x0008E400
		public T GetPersistentMetadata<T>() where T : IGraphMetadataItemPersistent, new()
		{
			bool flag;
			return this.GetPersistentMetadata<T>(out flag);
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x00090218 File Offset: 0x0008E418
		public T GetPersistentMetadata<T>(out bool wasCreated) where T : IGraphMetadataItemPersistent, new()
		{
			return this.GetCommonMetadata<T>(out wasCreated);
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x00090224 File Offset: 0x0008E424
		public T GetMetadata<T>() where T : IGraphMetadataItemNotPersistent, new()
		{
			bool flag;
			return this.GetMetadata<T>(out flag);
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x0009023C File Offset: 0x0008E43C
		public T GetMetadata<T>(out bool wasCreated) where T : IGraphMetadataItemNotPersistent, new()
		{
			return this.GetCommonMetadata<T>(out wasCreated);
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x00090248 File Offset: 0x0008E448
		private T GetCommonMetadata<T>(out bool wasCreated) where T : new()
		{
			object obj;
			if (!this._metadata.TryGetValue(typeof(T), out obj))
			{
				obj = new T();
				this._metadata[typeof(T)] = obj;
				wasCreated = true;
			}
			else
			{
				wasCreated = false;
			}
			return (T)((object)obj);
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x000902A4 File Offset: 0x0008E4A4
		public T GetInheritedMetadata<T>() where T : IGraphMetadataItemNotPersistent, new()
		{
			object obj;
			if (this._metadata.TryGetValue(typeof(T), out obj))
			{
				return (T)((object)obj);
			}
			if (this._parentMetadata == null)
			{
				return this.GetMetadata<T>();
			}
			return this._parentMetadata.GetInheritedMetadata<T>();
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000902F4 File Offset: 0x0008E4F4
		public bool TryGetMetadata<T>(out T metadata) where T : IGraphMetadataItemNotPersistent, new()
		{
			object obj;
			bool flag = this._metadata.TryGetValue(typeof(T), out obj);
			metadata = (T)((object)obj);
			return flag;
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x00090328 File Offset: 0x0008E528
		public bool TryGetInheritedMetadata<T>(out T metadata) where T : IGraphMetadataItemNotPersistent, new()
		{
			object obj;
			if (this._metadata.TryGetValue(typeof(T), out obj))
			{
				metadata = (T)((object)obj);
				return true;
			}
			if (this._parentMetadata == null)
			{
				metadata = default(T);
				return false;
			}
			return this._parentMetadata.TryGetInheritedMetadata<T>(out metadata);
		}

		// Token: 0x0400179C RID: 6044
		private Dictionary<string, List<object>> _precomputedData;

		// Token: 0x0400179D RID: 6045
		[ShowInInspector]
		private CullableDictionary<int, fiGraphMetadata, IntDictionary<fiGraphMetadata>> _childrenInt;

		// Token: 0x0400179E RID: 6046
		[ShowInInspector]
		private CullableDictionary<string, fiGraphMetadata, Dictionary<string, fiGraphMetadata>> _childrenString;

		// Token: 0x0400179F RID: 6047
		[ShowInInspector]
		private CullableDictionary<Type, object, Dictionary<Type, object>> _metadata;

		// Token: 0x040017A0 RID: 6048
		private fiGraphMetadata _parentMetadata;

		// Token: 0x040017A1 RID: 6049
		private fiUnityObjectReference _targetObject;

		// Token: 0x040017A2 RID: 6050
		private string _accessPath;

		// Token: 0x02000557 RID: 1367
		public struct MetadataMigration
		{
			// Token: 0x040017A3 RID: 6051
			public int NewIndex;

			// Token: 0x040017A4 RID: 6052
			public int OldIndex;
		}
	}
}
