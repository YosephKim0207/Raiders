using System;
using System.Linq;
using UnityEngine;

// Token: 0x02000368 RID: 872
public static class GameObjectExtensions
{
	// Token: 0x06000E09 RID: 3593 RVA: 0x00043128 File Offset: 0x00041328
	public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
	{
		T t = gameObject.GetComponent<T>();
		if (t == null)
		{
			t = gameObject.AddComponent<T>();
		}
		return t;
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x00043158 File Offset: 0x00041358
	public static void SetLayerRecursively(this GameObject gameObject, int layer)
	{
		gameObject.layer = layer;
		Transform transform = gameObject.transform;
		if (transform.childCount > 0)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetLayerRecursively(layer);
			}
		}
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x000431A8 File Offset: 0x000413A8
	public static void SetComponentEnabledRecursively<T>(this GameObject gameObject, bool enabled) where T : MonoBehaviour
	{
		T[] componentsInChildren = gameObject.GetComponentsInChildren<T>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = enabled;
		}
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x000431E8 File Offset: 0x000413E8
	public static T[] GetInterfaces<T>(this GameObject gObj)
	{
		if (!typeof(T).IsInterface)
		{
			throw new SystemException("Specified type is not an interface!");
		}
		MonoBehaviour[] components = gObj.GetComponents<MonoBehaviour>();
		return (from a in components
			where a.GetType().GetInterfaces().Any((Type k) => k == typeof(T))
			select (T)((object)a)).ToArray<T>();
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x00043244 File Offset: 0x00041444
	public static T GetInterface<T>(this GameObject gObj)
	{
		if (!typeof(T).IsInterface)
		{
			throw new SystemException("Specified type is not an interface!");
		}
		return gObj.GetInterfaces<T>().FirstOrDefault<T>();
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x00043270 File Offset: 0x00041470
	public static T GetInterfaceInChildren<T>(this GameObject gObj)
	{
		if (!typeof(T).IsInterface)
		{
			throw new Exception("Specified type is not an interface!");
		}
		return gObj.GetInterfacesInChildren<T>().FirstOrDefault<T>();
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x0004329C File Offset: 0x0004149C
	public static T[] GetInterfacesInChildren<T>(this GameObject gObj)
	{
		if (!typeof(T).IsInterface)
		{
			throw new Exception("Specified type is not an interface!");
		}
		MonoBehaviour[] componentsInChildren = gObj.GetComponentsInChildren<MonoBehaviour>();
		return (from a in componentsInChildren
			where a.GetType().GetInterfaces().Any((Type k) => k == typeof(T))
			select (T)((object)a)).ToArray<T>();
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x000432F8 File Offset: 0x000414F8
	public static int GetPhysicsCollisionMask(this GameObject gameObject, int layer = -1)
	{
		if (layer == -1)
		{
			layer = gameObject.layer;
		}
		int num = 0;
		for (int i = 0; i < 32; i++)
		{
			num |= ((!Physics.GetIgnoreLayerCollision(layer, i)) ? 1 : 0) << i;
		}
		return num;
	}
}
