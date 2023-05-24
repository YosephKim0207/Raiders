using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

// Token: 0x0200044F RID: 1103
[AddComponentMenu("Daikon Forge/Examples/Object Pooling/Object Pool Manager")]
[Serializable]
public class dfPoolManager : MonoBehaviour, ILevelLoadedListener
{
	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06001971 RID: 6513 RVA: 0x0007743C File Offset: 0x0007563C
	// (remove) Token: 0x06001972 RID: 6514 RVA: 0x00077474 File Offset: 0x00075674
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfPoolManager.PoolManagerLoadingEvent LoadingStarted;

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x06001973 RID: 6515 RVA: 0x000774AC File Offset: 0x000756AC
	// (remove) Token: 0x06001974 RID: 6516 RVA: 0x000774E4 File Offset: 0x000756E4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfPoolManager.PoolManagerLoadingEvent LoadingComplete;

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x06001975 RID: 6517 RVA: 0x0007751C File Offset: 0x0007571C
	// (remove) Token: 0x06001976 RID: 6518 RVA: 0x00077554 File Offset: 0x00075754
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfPoolManager.PoolManagerProgressEvent LoadingProgress;

	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x06001977 RID: 6519 RVA: 0x0007758C File Offset: 0x0007578C
	// (set) Token: 0x06001978 RID: 6520 RVA: 0x00077594 File Offset: 0x00075794
	public static dfPoolManager Pool { get; private set; }

	// Token: 0x06001979 RID: 6521 RVA: 0x0007759C File Offset: 0x0007579C
	private void Awake()
	{
		if (dfPoolManager.Pool != null)
		{
			throw new Exception("Cannot have more than one instance of the " + base.GetType().Name + " class");
		}
		dfPoolManager.Pool = this;
		if (this.AutoPreload)
		{
			this.Preload();
		}
	}

	// Token: 0x0600197A RID: 6522 RVA: 0x000775F0 File Offset: 0x000757F0
	private void OnDestroy()
	{
		this.ClearAllPools();
	}

	// Token: 0x0600197B RID: 6523 RVA: 0x000775F8 File Offset: 0x000757F8
	public void BraveOnLevelWasLoaded()
	{
		this.ClearAllPools();
	}

	// Token: 0x0600197C RID: 6524 RVA: 0x00077600 File Offset: 0x00075800
	public void ClearAllPools()
	{
		this.poolsPreloaded = false;
		for (int i = 0; i < this.objectPools.Count; i++)
		{
			this.objectPools[i].Clear();
		}
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x00077644 File Offset: 0x00075844
	public void Preload()
	{
		if (this.poolsPreloaded)
		{
			return;
		}
		if (this.PreloadInBackground)
		{
			base.StartCoroutine(this.preloadPools());
		}
		else
		{
			IEnumerator enumerator = this.preloadPools();
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
			}
		}
	}

	// Token: 0x0600197E RID: 6526 RVA: 0x00077698 File Offset: 0x00075898
	public void AddPool(string name, GameObject prefab)
	{
		if (this.objectPools.Any((dfPoolManager.ObjectPool p) => p.PoolName == name))
		{
			throw new Exception("Duplicate key: " + name);
		}
		if (prefab.activeSelf)
		{
			prefab.SetActive(false);
		}
		dfPoolManager.ObjectPool objectPool = new dfPoolManager.ObjectPool
		{
			Prefab = prefab,
			PoolName = name
		};
		this.objectPools.Add(objectPool);
	}

	// Token: 0x0600197F RID: 6527 RVA: 0x00077720 File Offset: 0x00075920
	private IEnumerator preloadPools()
	{
		this.poolsPreloaded = true;
		int totalItems = 0;
		for (int j = 0; j < this.objectPools.Count; j++)
		{
			totalItems += this.objectPools[j].InitialPoolSize;
		}
		if (this.LoadingStarted != null)
		{
			this.LoadingStarted();
		}
		int currentItem = 0;
		for (int i = 0; i < this.objectPools.Count; i++)
		{
			this.objectPools[i].Preload(delegate
			{
				if (this.LoadingProgress != null)
				{
					this.LoadingProgress(totalItems, currentItem);
				}
				currentItem++;
			});
			yield return null;
		}
		if (this.LoadingComplete != null)
		{
			this.LoadingComplete();
		}
		yield break;
	}

	// Token: 0x17000560 RID: 1376
	public dfPoolManager.ObjectPool this[string name]
	{
		get
		{
			for (int i = 0; i < this.objectPools.Count; i++)
			{
				if (this.objectPools[i].PoolName == name)
				{
					return this.objectPools[i];
				}
			}
			throw new KeyNotFoundException("Object pool not found: " + name);
		}
	}

	// Token: 0x040013F0 RID: 5104
	public bool AutoPreload = true;

	// Token: 0x040013F1 RID: 5105
	public bool PreloadInBackground = true;

	// Token: 0x040013F2 RID: 5106
	[SerializeField]
	private List<dfPoolManager.ObjectPool> objectPools = new List<dfPoolManager.ObjectPool>();

	// Token: 0x040013F3 RID: 5107
	private bool poolsPreloaded;

	// Token: 0x02000450 RID: 1104
	public enum LimitReachedAction
	{
		// Token: 0x040013F5 RID: 5109
		Nothing,
		// Token: 0x040013F6 RID: 5110
		Error,
		// Token: 0x040013F7 RID: 5111
		Recycle
	}

	// Token: 0x02000451 RID: 1105
	// (Invoke) Token: 0x06001982 RID: 6530
	public delegate void PoolManagerLoadingEvent();

	// Token: 0x02000452 RID: 1106
	// (Invoke) Token: 0x06001986 RID: 6534
	public delegate void PoolManagerProgressEvent(int TotalItems, int Current);

	// Token: 0x02000453 RID: 1107
	[Serializable]
	public class ObjectPool
	{
		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x0600198A RID: 6538 RVA: 0x000777D8 File Offset: 0x000759D8
		// (set) Token: 0x0600198B RID: 6539 RVA: 0x000777E0 File Offset: 0x000759E0
		public string PoolName
		{
			get
			{
				return this.poolName;
			}
			set
			{
				this.poolName = value;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x0600198C RID: 6540 RVA: 0x000777EC File Offset: 0x000759EC
		// (set) Token: 0x0600198D RID: 6541 RVA: 0x000777F4 File Offset: 0x000759F4
		public dfPoolManager.LimitReachedAction LimitReached
		{
			get
			{
				return this.limitType;
			}
			set
			{
				this.limitType = value;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x0600198E RID: 6542 RVA: 0x00077800 File Offset: 0x00075A00
		// (set) Token: 0x0600198F RID: 6543 RVA: 0x00077808 File Offset: 0x00075A08
		public GameObject Prefab
		{
			get
			{
				return this.prefab;
			}
			set
			{
				this.prefab = value;
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001990 RID: 6544 RVA: 0x00077814 File Offset: 0x00075A14
		// (set) Token: 0x06001991 RID: 6545 RVA: 0x0007781C File Offset: 0x00075A1C
		public int MaxInstances
		{
			get
			{
				return this.maxInstances;
			}
			set
			{
				this.maxInstances = value;
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001992 RID: 6546 RVA: 0x00077828 File Offset: 0x00075A28
		// (set) Token: 0x06001993 RID: 6547 RVA: 0x00077830 File Offset: 0x00075A30
		public int InitialPoolSize
		{
			get
			{
				return this.initialPoolSize;
			}
			set
			{
				this.initialPoolSize = value;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001994 RID: 6548 RVA: 0x0007783C File Offset: 0x00075A3C
		// (set) Token: 0x06001995 RID: 6549 RVA: 0x00077844 File Offset: 0x00075A44
		public bool AllowReparenting
		{
			get
			{
				return this.allowReparenting;
			}
			set
			{
				this.allowReparenting = value;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001996 RID: 6550 RVA: 0x00077850 File Offset: 0x00075A50
		public int Available
		{
			get
			{
				if (this.maxInstances == -1)
				{
					return int.MaxValue;
				}
				return Mathf.Max(this.pool.Count, this.maxInstances);
			}
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0007787C File Offset: 0x00075A7C
		public void Clear()
		{
			while (this.spawned.Count > 0)
			{
				this.pool.Enqueue(this.spawned.Dequeue());
			}
			for (int i = 0; i < this.pool.Count; i++)
			{
				GameObject gameObject = this.pool[i];
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
			this.pool.Clear();
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x000778F0 File Offset: 0x00075AF0
		public GameObject Spawn(Transform parent, Vector3 position, Quaternion rotation, bool activate)
		{
			GameObject gameObject = this.Spawn(position, rotation, activate);
			gameObject.transform.parent = parent;
			return gameObject;
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x00077918 File Offset: 0x00075B18
		public GameObject Spawn(Vector3 position, Quaternion rotation)
		{
			return this.Spawn(position, rotation, true);
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x00077924 File Offset: 0x00075B24
		public GameObject Spawn(Vector3 position, Quaternion rotation, bool activate)
		{
			GameObject gameObject = this.Spawn(false);
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
			if (activate)
			{
				gameObject.SetActive(true);
			}
			return gameObject;
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x00077960 File Offset: 0x00075B60
		public GameObject Spawn(bool activate)
		{
			if (this.pool.Count > 0)
			{
				GameObject gameObject = this.pool.Dequeue();
				this.spawnInstance(gameObject, activate);
				return gameObject;
			}
			if (this.maxInstances == -1 || this.spawned.Count < this.maxInstances)
			{
				GameObject gameObject2 = this.Instantiate();
				this.spawnInstance(gameObject2, activate);
				return gameObject2;
			}
			if (this.limitType == dfPoolManager.LimitReachedAction.Nothing)
			{
				return null;
			}
			if (this.limitType == dfPoolManager.LimitReachedAction.Error)
			{
				throw new Exception(string.Format("The {0} object pool has already allocated its limit of {1} objects", this.PoolName, this.MaxInstances));
			}
			GameObject gameObject3 = this.spawned.Dequeue();
			this.spawnInstance(gameObject3, activate);
			return gameObject3;
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x00077A18 File Offset: 0x00075C18
		public void Despawn(GameObject instance)
		{
			if (!this.spawned.Remove(instance))
			{
				return;
			}
			dfPooledObject component = instance.GetComponent<dfPooledObject>();
			if (component != null)
			{
				component.OnDespawned();
			}
			instance.SetActive(false);
			this.pool.Enqueue(instance);
			if (this.allowReparenting && dfPoolManager.Pool != null)
			{
				instance.transform.parent = dfPoolManager.Pool.transform;
			}
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00077A94 File Offset: 0x00075C94
		internal void Preload()
		{
			this.Preload(null);
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x00077AA0 File Offset: 0x00075CA0
		internal void Preload(Action callback)
		{
			if (this.prefab.activeSelf)
			{
				this.prefab.SetActive(false);
			}
			int num = ((this.maxInstances != -1) ? this.maxInstances : int.MaxValue);
			int num2 = Mathf.Min(this.initialPoolSize, num);
			while (this.pool.Count + this.spawned.Count < num2)
			{
				this.pool.Add(this.Instantiate());
				if (callback != null)
				{
					callback();
				}
			}
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x00077B34 File Offset: 0x00075D34
		private void spawnInstance(GameObject instance, bool activate)
		{
			this.spawned.Enqueue(instance);
			dfPooledObject component = instance.GetComponent<dfPooledObject>();
			if (component != null)
			{
				component.OnSpawned();
			}
			if (activate)
			{
				instance.SetActive(true);
			}
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x00077B74 File Offset: 0x00075D74
		private GameObject Instantiate()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab);
			gameObject.name = string.Format("{0} {1}", this.PoolName, this.pool.Count + 1);
			if (this.allowReparenting)
			{
				gameObject.transform.parent = dfPoolManager.Pool.transform;
			}
			dfPooledObject dfPooledObject = gameObject.GetComponent<dfPooledObject>();
			if (dfPooledObject == null)
			{
				dfPooledObject = gameObject.AddComponent<dfPooledObject>();
			}
			dfPooledObject.Pool = this;
			return gameObject;
		}

		// Token: 0x040013F8 RID: 5112
		private dfList<GameObject> pool = dfList<GameObject>.Obtain();

		// Token: 0x040013F9 RID: 5113
		private dfList<GameObject> spawned = dfList<GameObject>.Obtain();

		// Token: 0x040013FA RID: 5114
		[SerializeField]
		private string poolName = string.Empty;

		// Token: 0x040013FB RID: 5115
		[SerializeField]
		private dfPoolManager.LimitReachedAction limitType;

		// Token: 0x040013FC RID: 5116
		[SerializeField]
		private GameObject prefab;

		// Token: 0x040013FD RID: 5117
		[SerializeField]
		private int maxInstances = -1;

		// Token: 0x040013FE RID: 5118
		[SerializeField]
		private int initialPoolSize;

		// Token: 0x040013FF RID: 5119
		[SerializeField]
		private bool allowReparenting = true;
	}
}
