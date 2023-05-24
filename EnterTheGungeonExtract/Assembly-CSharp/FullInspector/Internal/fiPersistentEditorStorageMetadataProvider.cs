using System;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x02000616 RID: 1558
	public abstract class fiPersistentEditorStorageMetadataProvider<TItem, TStorage> : fiIPersistentMetadataProvider where TItem : new()where TStorage : fiIGraphMetadataStorage, new()
	{
		// Token: 0x0600245A RID: 9306 RVA: 0x0009E184 File Offset: 0x0009C384
		public void RestoreData(UnityEngine.Object target)
		{
			TStorage tstorage = fiPersistentEditorStorage.Read<TStorage>(target);
			tstorage.RestoreData(target);
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x0009E1A8 File Offset: 0x0009C3A8
		public void Reset(UnityEngine.Object target)
		{
			fiPersistentEditorStorage.Reset<TStorage>(target);
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x0600245C RID: 9308 RVA: 0x0009E1B0 File Offset: 0x0009C3B0
		public Type MetadataType
		{
			get
			{
				return typeof(TItem);
			}
		}
	}
}
