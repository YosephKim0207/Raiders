using System;
using UnityEngine;

// Token: 0x02000BE8 RID: 3048
public static class tk2dUtil
{
	// Token: 0x170009D0 RID: 2512
	// (get) Token: 0x060040AB RID: 16555 RVA: 0x0014BA20 File Offset: 0x00149C20
	// (set) Token: 0x060040AC RID: 16556 RVA: 0x0014BA28 File Offset: 0x00149C28
	public static bool UndoEnabled
	{
		get
		{
			return tk2dUtil.undoEnabled;
		}
		set
		{
			tk2dUtil.undoEnabled = value;
		}
	}

	// Token: 0x060040AD RID: 16557 RVA: 0x0014BA30 File Offset: 0x00149C30
	public static void BeginGroup(string name)
	{
		tk2dUtil.undoEnabled = true;
		tk2dUtil.label = name;
	}

	// Token: 0x060040AE RID: 16558 RVA: 0x0014BA40 File Offset: 0x00149C40
	public static void EndGroup()
	{
		tk2dUtil.label = string.Empty;
	}

	// Token: 0x060040AF RID: 16559 RVA: 0x0014BA4C File Offset: 0x00149C4C
	public static void DestroyImmediate(UnityEngine.Object obj)
	{
		if (obj == null)
		{
			return;
		}
		UnityEngine.Object.DestroyImmediate(obj);
	}

	// Token: 0x060040B0 RID: 16560 RVA: 0x0014BA64 File Offset: 0x00149C64
	public static GameObject CreateGameObject(string name)
	{
		return new GameObject(name);
	}

	// Token: 0x060040B1 RID: 16561 RVA: 0x0014BA7C File Offset: 0x00149C7C
	public static Mesh CreateMesh()
	{
		return new Mesh();
	}

	// Token: 0x060040B2 RID: 16562 RVA: 0x0014BA90 File Offset: 0x00149C90
	public static T AddComponent<T>(GameObject go) where T : Component
	{
		return go.AddComponent<T>();
	}

	// Token: 0x060040B3 RID: 16563 RVA: 0x0014BAA8 File Offset: 0x00149CA8
	public static void SetActive(GameObject go, bool active)
	{
		if (active == go.activeSelf)
		{
			return;
		}
		go.SetActive(active);
	}

	// Token: 0x060040B4 RID: 16564 RVA: 0x0014BAC0 File Offset: 0x00149CC0
	public static void SetTransformParent(Transform t, Transform parent)
	{
		t.parent = parent;
	}

	// Token: 0x0400338A RID: 13194
	private static string label = string.Empty;

	// Token: 0x0400338B RID: 13195
	private static bool undoEnabled;
}
