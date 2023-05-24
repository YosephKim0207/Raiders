using System;
using System.Collections.Generic;
using System.Linq;
using FullSerializer;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x02000614 RID: 1556
	public class fiPersistentEditorStorage
	{
		// Token: 0x06002453 RID: 9299 RVA: 0x0009DFA0 File Offset: 0x0009C1A0
		public static void Reset<T>(UnityEngine.Object key_)
		{
			fiUnityObjectReference fiUnityObjectReference = new fiUnityObjectReference(key_);
			fiBaseStorageComponent<T> fiBaseStorageComponent;
			if (fiLateBindings.EditorUtility.IsPersistent(fiUnityObjectReference.Target))
			{
				fiBaseStorageComponent = fiPersistentEditorStorage.GetStorageDictionary<T>(fiPersistentEditorStorage.SceneStorage);
			}
			else
			{
				fiBaseStorageComponent = fiPersistentEditorStorage.GetStorageDictionary<T>(fiPersistentEditorStorage.SceneStorage);
			}
			fiBaseStorageComponent.Data.Remove(fiUnityObjectReference.Target);
			fiLateBindings.EditorUtility.SetDirty(fiBaseStorageComponent);
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x0009DFF8 File Offset: 0x0009C1F8
		public static T Read<T>(UnityEngine.Object key_) where T : new()
		{
			fiUnityObjectReference fiUnityObjectReference = new fiUnityObjectReference(key_);
			fiBaseStorageComponent<T> fiBaseStorageComponent;
			if (fiLateBindings.EditorUtility.IsPersistent(fiUnityObjectReference.Target))
			{
				fiBaseStorageComponent = fiPersistentEditorStorage.GetStorageDictionary<T>(fiPersistentEditorStorage.SceneStorage);
			}
			else
			{
				fiBaseStorageComponent = fiPersistentEditorStorage.GetStorageDictionary<T>(fiPersistentEditorStorage.SceneStorage);
			}
			if (fiBaseStorageComponent.Data.ContainsKey(fiUnityObjectReference.Target))
			{
				return fiBaseStorageComponent.Data[fiUnityObjectReference.Target];
			}
			T t = new T();
			fiBaseStorageComponent.Data[fiUnityObjectReference.Target] = t;
			T t2 = t;
			fiLateBindings.EditorUtility.SetDirty(fiBaseStorageComponent);
			return t2;
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x0009E080 File Offset: 0x0009C280
		private static fiBaseStorageComponent<T> GetStorageDictionary<T>(GameObject container)
		{
			Type type;
			if (!fiPersistentEditorStorage._cachedRealComponentTypes.TryGetValue(typeof(fiBaseStorageComponent<T>), out type))
			{
				type = fiRuntimeReflectionUtility.AllSimpleTypesDerivingFrom(typeof(fiBaseStorageComponent<T>)).FirstOrDefault<Type>();
				fiPersistentEditorStorage._cachedRealComponentTypes[typeof(fiBaseStorageComponent<T>)] = type;
			}
			if (type == null)
			{
				throw new InvalidOperationException("Unable to find derived component type for " + typeof(fiBaseStorageComponent<T>).CSharpName());
			}
			Component component = container.GetComponent(type);
			if (component == null)
			{
				component = container.AddComponent(type);
			}
			return (fiBaseStorageComponent<T>)component;
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002456 RID: 9302 RVA: 0x0009E11C File Offset: 0x0009C31C
		public static GameObject SceneStorage
		{
			get
			{
				if (fiPersistentEditorStorage._cachedSceneStorage == null)
				{
					fiPersistentEditorStorage._cachedSceneStorage = GameObject.Find("fiPersistentEditorStorage");
					if (fiPersistentEditorStorage._cachedSceneStorage == null)
					{
						fiPersistentEditorStorage._cachedSceneStorage = fiLateBindings.EditorUtility.CreateGameObjectWithHideFlags("fiPersistentEditorStorage", HideFlags.HideInHierarchy);
					}
				}
				return fiPersistentEditorStorage._cachedSceneStorage;
			}
		}

		// Token: 0x04001924 RID: 6436
		private static Dictionary<Type, Type> _cachedRealComponentTypes = new Dictionary<Type, Type>();

		// Token: 0x04001925 RID: 6437
		private const string SceneStorageName = "fiPersistentEditorStorage";

		// Token: 0x04001926 RID: 6438
		private static GameObject _cachedSceneStorage;
	}
}
