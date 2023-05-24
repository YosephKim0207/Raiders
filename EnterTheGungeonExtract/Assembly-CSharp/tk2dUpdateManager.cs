using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BA2 RID: 2978
[AddComponentMenu("")]
public class tk2dUpdateManager : MonoBehaviour
{
	// Token: 0x17000969 RID: 2409
	// (get) Token: 0x06003E69 RID: 15977 RVA: 0x0013BE2C File Offset: 0x0013A02C
	private static tk2dUpdateManager Instance
	{
		get
		{
			if (tk2dUpdateManager.inst == null)
			{
				tk2dUpdateManager.inst = UnityEngine.Object.FindObjectOfType(typeof(tk2dUpdateManager)) as tk2dUpdateManager;
				if (tk2dUpdateManager.inst == null)
				{
					GameObject gameObject = new GameObject("@tk2dUpdateManager");
					gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
					tk2dUpdateManager.inst = gameObject.AddComponent<tk2dUpdateManager>();
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
			}
			return tk2dUpdateManager.inst;
		}
	}

	// Token: 0x06003E6A RID: 15978 RVA: 0x0013BE9C File Offset: 0x0013A09C
	public static void QueueCommit(tk2dTextMesh textMesh)
	{
		tk2dUpdateManager.Instance.QueueCommitInternal(textMesh);
	}

	// Token: 0x06003E6B RID: 15979 RVA: 0x0013BEAC File Offset: 0x0013A0AC
	public static void FlushQueues()
	{
		tk2dUpdateManager.Instance.FlushQueuesInternal();
	}

	// Token: 0x06003E6C RID: 15980 RVA: 0x0013BEB8 File Offset: 0x0013A0B8
	private void OnEnable()
	{
		base.StartCoroutine(this.coSuperLateUpdate());
	}

	// Token: 0x06003E6D RID: 15981 RVA: 0x0013BEC8 File Offset: 0x0013A0C8
	private void LateUpdate()
	{
		this.FlushQueuesInternal();
	}

	// Token: 0x06003E6E RID: 15982 RVA: 0x0013BED0 File Offset: 0x0013A0D0
	private IEnumerator coSuperLateUpdate()
	{
		this.FlushQueuesInternal();
		yield break;
	}

	// Token: 0x06003E6F RID: 15983 RVA: 0x0013BEEC File Offset: 0x0013A0EC
	private void QueueCommitInternal(tk2dTextMesh textMesh)
	{
		this.textMeshes.Add(textMesh);
	}

	// Token: 0x06003E70 RID: 15984 RVA: 0x0013BEFC File Offset: 0x0013A0FC
	private void FlushQueuesInternal()
	{
		int count = this.textMeshes.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dTextMesh tk2dTextMesh = this.textMeshes[i];
			if (tk2dTextMesh != null)
			{
				tk2dTextMesh.DoNotUse__CommitInternal();
			}
		}
		this.textMeshes.Clear();
	}

	// Token: 0x04003112 RID: 12562
	private static tk2dUpdateManager inst;

	// Token: 0x04003113 RID: 12563
	[SerializeField]
	private List<tk2dTextMesh> textMeshes = new List<tk2dTextMesh>(64);
}
