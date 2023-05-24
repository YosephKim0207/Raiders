using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200080F RID: 2063
	public abstract class SingletonMonoBehavior<T, P> : MonoBehaviour where T : MonoBehaviour where P : MonoBehaviour
	{
		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06002BB5 RID: 11189 RVA: 0x000DD8D0 File Offset: 0x000DBAD0
		public static T Instance
		{
			get
			{
				return SingletonMonoBehavior<T, P>.GetInstance();
			}
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x000DD8D8 File Offset: 0x000DBAD8
		private static void CreateInstance()
		{
			GameObject gameObject;
			if (typeof(P) == typeof(MonoBehaviour))
			{
				gameObject = new GameObject();
				gameObject.name = typeof(T).Name;
			}
			else
			{
				P p = UnityEngine.Object.FindObjectOfType<P>();
				if (!p)
				{
					Debug.LogError("Could not find object with required component " + typeof(P).Name);
					return;
				}
				gameObject = p.gameObject;
			}
			Debug.Log("Creating instance of singleton component " + typeof(T).Name);
			SingletonMonoBehavior<T, P>.instance = gameObject.AddComponent<T>();
			SingletonMonoBehavior<T, P>.hasInstance = true;
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x000DD998 File Offset: 0x000DBB98
		private static T GetInstance()
		{
			object obj = SingletonMonoBehavior<T, P>.lockObject;
			T t;
			lock (obj)
			{
				if (SingletonMonoBehavior<T, P>.hasInstance)
				{
					t = SingletonMonoBehavior<T, P>.instance;
				}
				else
				{
					Type typeFromHandle = typeof(T);
					T[] array = UnityEngine.Object.FindObjectsOfType<T>();
					if (array.Length > 0)
					{
						SingletonMonoBehavior<T, P>.instance = array[0];
						SingletonMonoBehavior<T, P>.hasInstance = true;
						if (array.Length > 1)
						{
							Debug.LogWarning("Multiple instances of singleton " + typeFromHandle + " found; destroying all but the first.");
							for (int i = 1; i < array.Length; i++)
							{
								UnityEngine.Object.DestroyImmediate(array[i].gameObject);
							}
						}
						t = SingletonMonoBehavior<T, P>.instance;
					}
					else
					{
						SingletonPrefabAttribute singletonPrefabAttribute = Attribute.GetCustomAttribute(typeFromHandle, typeof(SingletonPrefabAttribute)) as SingletonPrefabAttribute;
						if (singletonPrefabAttribute == null)
						{
							SingletonMonoBehavior<T, P>.CreateInstance();
						}
						else
						{
							string name = singletonPrefabAttribute.Name;
							GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(BraveResources.Load<GameObject>(name, ".prefab"));
							if (gameObject == null)
							{
								Debug.LogError(string.Concat(new object[] { "Could not find prefab ", name, " for singleton of type ", typeFromHandle, "." }));
								SingletonMonoBehavior<T, P>.CreateInstance();
							}
							else
							{
								gameObject.name = name;
								SingletonMonoBehavior<T, P>.instance = gameObject.GetComponent<T>();
								if (SingletonMonoBehavior<T, P>.instance == null)
								{
									Debug.LogWarning(string.Concat(new object[] { "There wasn't a component of type \"", typeFromHandle, "\" inside prefab \"", name, "\"; creating one now." }));
									SingletonMonoBehavior<T, P>.instance = gameObject.AddComponent<T>();
									SingletonMonoBehavior<T, P>.hasInstance = true;
								}
							}
						}
						t = SingletonMonoBehavior<T, P>.instance;
					}
				}
			}
			return t;
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x000DDB78 File Offset: 0x000DBD78
		protected bool EnforceSingleton()
		{
			object obj = SingletonMonoBehavior<T, P>.lockObject;
			lock (obj)
			{
				if (SingletonMonoBehavior<T, P>.hasInstance)
				{
					T[] array = UnityEngine.Object.FindObjectsOfType<T>();
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].GetInstanceID() != SingletonMonoBehavior<T, P>.instance.GetInstanceID())
						{
							UnityEngine.Object.DestroyImmediate(array[i].gameObject);
						}
					}
				}
			}
			int instanceID = base.GetInstanceID();
			T t = SingletonMonoBehavior<T, P>.Instance;
			return instanceID == t.GetInstanceID();
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x000DDC30 File Offset: 0x000DBE30
		protected bool EnforceSingletonComponent()
		{
			object obj = SingletonMonoBehavior<T, P>.lockObject;
			lock (obj)
			{
				if (SingletonMonoBehavior<T, P>.hasInstance && base.GetInstanceID() != SingletonMonoBehavior<T, P>.instance.GetInstanceID())
				{
					UnityEngine.Object.DestroyImmediate(this);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x000DDC9C File Offset: 0x000DBE9C
		private void OnDestroy()
		{
			SingletonMonoBehavior<T, P>.hasInstance = false;
		}

		// Token: 0x04001DE7 RID: 7655
		private static T instance;

		// Token: 0x04001DE8 RID: 7656
		private static bool hasInstance;

		// Token: 0x04001DE9 RID: 7657
		private static object lockObject = new object();
	}
}
