using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	// Token: 0x02000848 RID: 2120
	[Serializable]
	public class PrefabPool
	{
		// Token: 0x06002E72 RID: 11890 RVA: 0x000F0BD8 File Offset: 0x000EEDD8
		public PrefabPool(Transform prefab)
		{
			this.prefab = prefab;
			this.prefabGO = prefab.gameObject;
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x000F0C44 File Offset: 0x000EEE44
		public PrefabPool()
		{
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002E74 RID: 11892 RVA: 0x000F0C9C File Offset: 0x000EEE9C
		public bool logMessages
		{
			get
			{
				if (this.forceLoggingSilent)
				{
					return false;
				}
				if (this.spawnPool.logMessages)
				{
					return this.spawnPool.logMessages;
				}
				return this._logMessages;
			}
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000F0CD0 File Offset: 0x000EEED0
		internal void inspectorInstanceConstructor()
		{
			this.prefabGO = this.prefab.gameObject;
			this._spawned = new List<Transform>();
			this._despawned = new List<Transform>();
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000F0CFC File Offset: 0x000EEEFC
		internal void SelfDestruct()
		{
			this.prefab = null;
			this.prefabGO = null;
			this.spawnPool = null;
			foreach (Transform transform in this._despawned)
			{
				if (transform != null)
				{
					UnityEngine.Object.Destroy(transform.gameObject);
				}
			}
			foreach (Transform transform2 in this._spawned)
			{
				if (transform2 != null)
				{
					UnityEngine.Object.Destroy(transform2.gameObject);
				}
			}
			this._spawned.Clear();
			this._despawned.Clear();
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06002E77 RID: 11895 RVA: 0x000F0DF0 File Offset: 0x000EEFF0
		public List<Transform> spawned
		{
			get
			{
				return this._spawned;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002E78 RID: 11896 RVA: 0x000F0DF8 File Offset: 0x000EEFF8
		public List<Transform> despawned
		{
			get
			{
				return this._despawned;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002E79 RID: 11897 RVA: 0x000F0E00 File Offset: 0x000EF000
		public int totalCount
		{
			get
			{
				int num = 0;
				num += this._spawned.Count;
				return num + this._despawned.Count;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06002E7A RID: 11898 RVA: 0x000F0E2C File Offset: 0x000EF02C
		// (set) Token: 0x06002E7B RID: 11899 RVA: 0x000F0E34 File Offset: 0x000EF034
		internal bool preloaded
		{
			get
			{
				return this._preloaded;
			}
			private set
			{
				this._preloaded = value;
			}
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x000F0E40 File Offset: 0x000EF040
		internal bool DespawnInstance(Transform xform)
		{
			return this.DespawnInstance(xform, true);
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x000F0E4C File Offset: 0x000EF04C
		internal void RemoveInstance(Transform xform)
		{
			this._spawned.Remove(xform);
			this._despawned.Remove(xform);
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x000F0E68 File Offset: 0x000EF068
		internal bool DespawnInstance(Transform xform, bool sendEventMessage)
		{
			if (this.logMessages)
			{
				Debug.Log(string.Format("SpawnPool {0} ({1}): Despawning '{2}'", this.spawnPool.poolName, this.prefab.name, xform.name));
			}
			this._spawned.Remove(xform);
			this._despawned.Add(xform);
			if (sendEventMessage)
			{
				xform.gameObject.BroadcastMessage("OnDespawned", this.spawnPool, SendMessageOptions.DontRequireReceiver);
			}
			PoolManagerUtils.SetActive(xform.gameObject, false);
			if (!this.cullingActive && this.cullDespawned && this.totalCount > this.cullAbove)
			{
				this.cullingActive = true;
				this.spawnPool.StartCoroutine(this.CullDespawned());
			}
			return true;
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000F0F30 File Offset: 0x000EF130
		internal IEnumerator CullDespawned()
		{
			if (this.logMessages)
			{
				Debug.Log(string.Format("SpawnPool {0} ({1}): CULLING TRIGGERED! Waiting {2}sec to begin checking for despawns...", this.spawnPool.poolName, this.prefab.name, this.cullDelay));
			}
			yield return new WaitForSeconds((float)this.cullDelay);
			while (this.totalCount > this.cullAbove)
			{
				for (int i = 0; i < this.cullMaxPerPass; i++)
				{
					if (this.totalCount <= this.cullAbove)
					{
						break;
					}
					if (this._despawned.Count > 0)
					{
						Transform transform = this._despawned[0];
						this._despawned.RemoveAt(0);
						UnityEngine.Object.Destroy(transform.gameObject);
						if (this.logMessages)
						{
							Debug.Log(string.Format("SpawnPool {0} ({1}): CULLING to {2} instances. Now at {3}.", new object[]
							{
								this.spawnPool.poolName,
								this.prefab.name,
								this.cullAbove,
								this.totalCount
							}));
						}
					}
					else if (this.logMessages)
					{
						Debug.Log(string.Format("SpawnPool {0} ({1}): CULLING waiting for despawn. Checking again in {2}sec", this.spawnPool.poolName, this.prefab.name, this.cullDelay));
						break;
					}
				}
				yield return new WaitForSeconds((float)this.cullDelay);
			}
			if (this.logMessages)
			{
				Debug.Log(string.Format("SpawnPool {0} ({1}): CULLING FINISHED! Stopping", this.spawnPool.poolName, this.prefab.name));
			}
			this.cullingActive = false;
			yield return null;
			yield break;
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x000F0F4C File Offset: 0x000EF14C
		internal Transform SpawnInstance(Vector3 pos, Quaternion rot)
		{
			SpawnManager.LastPrefabPool = this;
			if (this.limitInstances && this.limitFIFO && this._spawned.Count >= this.limitAmount)
			{
				Transform transform = this._spawned[0];
				if (this.logMessages)
				{
					Debug.Log(string.Format("SpawnPool {0} ({1}): LIMIT REACHED! FIFO=True. Calling despawning for {2}...", this.spawnPool.poolName, this.prefab.name, transform));
				}
				this.DespawnInstance(transform);
				this.spawnPool._spawned.Remove(transform);
			}
			Transform transform2;
			if (this._despawned.Count == 0)
			{
				transform2 = this.SpawnNew(pos, rot);
			}
			else
			{
				transform2 = null;
				while (transform2 == null)
				{
					if (this._despawned.Count == 0)
					{
						transform2 = this.SpawnNew(pos, rot);
					}
					else
					{
						transform2 = this._despawned[0];
						this._despawned.RemoveAt(0);
						if (transform2 != null)
						{
							this._spawned.Add(transform2);
						}
					}
				}
				if (this.logMessages)
				{
					Debug.Log(string.Format("SpawnPool {0} ({1}): respawning '{2}'.", this.spawnPool.poolName, this.prefab.name, transform2.name));
				}
				transform2.position = pos;
				transform2.rotation = rot;
				PoolManagerUtils.SetActive(transform2.gameObject, true);
			}
			return transform2;
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x000F10B4 File Offset: 0x000EF2B4
		public Transform SpawnNew()
		{
			return this.SpawnNew(Vector3.zero, Quaternion.identity);
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x000F10C8 File Offset: 0x000EF2C8
		public Transform SpawnNew(Vector3 pos, Quaternion rot)
		{
			if (this.limitInstances && this.totalCount >= this.limitAmount)
			{
				if (this.logMessages)
				{
					Debug.Log(string.Format("SpawnPool {0} ({1}): LIMIT REACHED! Not creating new instances! (Returning null)", this.spawnPool.poolName, this.prefab.name));
				}
				return null;
			}
			if (pos == Vector3.zero)
			{
				pos = this.spawnPool.group.position;
			}
			if (rot == Quaternion.identity)
			{
				rot = this.spawnPool.group.rotation;
			}
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.prefab, pos, rot);
			this.nameInstance(transform);
			if (!this.spawnPool.dontReparent)
			{
				transform.parent = this.spawnPool.group;
			}
			if (this.spawnPool.matchPoolScale)
			{
				transform.localScale = Vector3.one;
			}
			if (this.spawnPool.matchPoolLayer)
			{
				this.SetRecursively(transform, this.spawnPool.gameObject.layer);
			}
			this._spawned.Add(transform);
			if (this.logMessages)
			{
				Debug.Log(string.Format("SpawnPool {0} ({1}): Spawned new instance '{2}'.", this.spawnPool.poolName, this.prefab.name, transform.name));
			}
			return transform;
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x000F1224 File Offset: 0x000EF424
		private void SetRecursively(Transform xform, int layer)
		{
			xform.gameObject.layer = layer;
			IEnumerator enumerator = xform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					this.SetRecursively(transform, layer);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000F1294 File Offset: 0x000EF494
		internal void AddUnpooled(Transform inst, bool despawn)
		{
			this.nameInstance(inst);
			if (despawn)
			{
				PoolManagerUtils.SetActive(inst.gameObject, false);
				this._despawned.Add(inst);
			}
			else
			{
				this._spawned.Add(inst);
			}
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x000F12CC File Offset: 0x000EF4CC
		internal void PreloadInstances()
		{
			if (this.preloaded)
			{
				Debug.Log(string.Format("SpawnPool {0} ({1}): Already preloaded! You cannot preload twice. If you are running this through code, make sure it isn't also defined in the Inspector.", this.spawnPool.poolName, this.prefab.name));
				return;
			}
			if (this.prefab == null)
			{
				Debug.LogError(string.Format("SpawnPool {0} ({1}): Prefab cannot be null.", this.spawnPool.poolName, this.prefab.name));
				return;
			}
			if (this.limitInstances && this.preloadAmount > this.limitAmount)
			{
				Debug.LogWarning(string.Format("SpawnPool {0} ({1}): You turned ON 'Limit Instances' and entered a 'Limit Amount' greater than the 'Preload Amount'! Setting preload amount to limit amount.", this.spawnPool.poolName, this.prefab.name));
				this.preloadAmount = this.limitAmount;
			}
			if (this.cullDespawned && this.preloadAmount > this.cullAbove)
			{
				Debug.LogWarning(string.Format("SpawnPool {0} ({1}): You turned ON Culling and entered a 'Cull Above' threshold greater than the 'Preload Amount'! This will cause the culling feature to trigger immediatly, which is wrong conceptually. Only use culling for extreme situations. See the docs.", this.spawnPool.poolName, this.prefab.name));
			}
			if (this.preloadTime)
			{
				if (this.preloadFrames > this.preloadAmount)
				{
					Debug.LogWarning(string.Format("SpawnPool {0} ({1}): Preloading over-time is on but the frame duration is greater than the number of instances to preload. The minimum spawned per frame is 1, so the maximum time is the same as the number of instances. Changing the preloadFrames value...", this.spawnPool.poolName, this.prefab.name));
					this.preloadFrames = this.preloadAmount;
				}
				this.spawnPool.StartCoroutine(this.PreloadOverTime());
			}
			else
			{
				this.forceLoggingSilent = true;
				while (this.totalCount < this.preloadAmount)
				{
					Transform transform = this.SpawnNew();
					this.DespawnInstance(transform, false);
				}
				this.forceLoggingSilent = false;
			}
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x000F1468 File Offset: 0x000EF668
		private IEnumerator PreloadOverTime()
		{
			yield return new WaitForSeconds(this.preloadDelay);
			int amount = this.preloadAmount - this.totalCount;
			if (amount <= 0)
			{
				yield break;
			}
			int remainder = amount % this.preloadFrames;
			int numPerFrame = amount / this.preloadFrames;
			this.forceLoggingSilent = true;
			for (int i = 0; i < this.preloadFrames; i++)
			{
				int numThisFrame = numPerFrame;
				if (i == this.preloadFrames - 1)
				{
					numThisFrame += remainder;
				}
				for (int j = 0; j < numThisFrame; j++)
				{
					Transform inst = this.SpawnNew();
					if (inst != null)
					{
						this.DespawnInstance(inst, false);
					}
					yield return null;
				}
				if (this.totalCount > this.preloadAmount)
				{
					break;
				}
			}
			this.forceLoggingSilent = false;
			yield break;
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x000F1484 File Offset: 0x000EF684
		public bool Contains(Transform transform)
		{
			if (this.prefabGO == null)
			{
				Debug.LogError(string.Format("SpawnPool {0}: PrefabPool.prefabGO is null", this.spawnPool.poolName));
			}
			bool flag = this._spawned.Contains(transform);
			return flag || this._despawned.Contains(transform);
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x000F14E8 File Offset: 0x000EF6E8
		private void nameInstance(Transform instance)
		{
			instance.name += (this.totalCount + 1).ToString("#000");
		}

		// Token: 0x04001F5D RID: 8029
		public Transform prefab;

		// Token: 0x04001F5E RID: 8030
		internal GameObject prefabGO;

		// Token: 0x04001F5F RID: 8031
		public int preloadAmount = 1;

		// Token: 0x04001F60 RID: 8032
		public bool preloadTime;

		// Token: 0x04001F61 RID: 8033
		public int preloadFrames = 2;

		// Token: 0x04001F62 RID: 8034
		public float preloadDelay;

		// Token: 0x04001F63 RID: 8035
		public bool limitInstances;

		// Token: 0x04001F64 RID: 8036
		public int limitAmount = 100;

		// Token: 0x04001F65 RID: 8037
		public bool limitFIFO;

		// Token: 0x04001F66 RID: 8038
		public bool cullDespawned;

		// Token: 0x04001F67 RID: 8039
		public int cullAbove = 50;

		// Token: 0x04001F68 RID: 8040
		public int cullDelay = 60;

		// Token: 0x04001F69 RID: 8041
		public int cullMaxPerPass = 5;

		// Token: 0x04001F6A RID: 8042
		public bool _logMessages;

		// Token: 0x04001F6B RID: 8043
		private bool forceLoggingSilent;

		// Token: 0x04001F6C RID: 8044
		public SpawnPool spawnPool;

		// Token: 0x04001F6D RID: 8045
		private bool cullingActive;

		// Token: 0x04001F6E RID: 8046
		internal List<Transform> _spawned = new List<Transform>();

		// Token: 0x04001F6F RID: 8047
		internal List<Transform> _despawned = new List<Transform>();

		// Token: 0x04001F70 RID: 8048
		private bool _preloaded;
	}
}
