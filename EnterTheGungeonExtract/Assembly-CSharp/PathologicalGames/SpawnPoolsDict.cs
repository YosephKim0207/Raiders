using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	// Token: 0x0200083F RID: 2111
	public class SpawnPoolsDict : IDictionary<string, SpawnPool>, ICollection<KeyValuePair<string, SpawnPool>>, IEnumerable<KeyValuePair<string, SpawnPool>>, IEnumerable
	{
		// Token: 0x06002DFC RID: 11772 RVA: 0x000EF418 File Offset: 0x000ED618
		public void AddOnCreatedDelegate(string poolName, SpawnPoolsDict.OnCreatedDelegate createdDelegate)
		{
			if (!this.onCreatedDelegates.ContainsKey(poolName))
			{
				this.onCreatedDelegates.Add(poolName, createdDelegate);
				return;
			}
			Dictionary<string, SpawnPoolsDict.OnCreatedDelegate> dictionary;
			(dictionary = this.onCreatedDelegates)[poolName] = (SpawnPoolsDict.OnCreatedDelegate)Delegate.Combine(dictionary[poolName], createdDelegate);
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x000EF468 File Offset: 0x000ED668
		public void RemoveOnCreatedDelegate(string poolName, SpawnPoolsDict.OnCreatedDelegate createdDelegate)
		{
			if (!this.onCreatedDelegates.ContainsKey(poolName))
			{
				throw new KeyNotFoundException("No OnCreatedDelegates found for pool name '" + poolName + "'.");
			}
			Dictionary<string, SpawnPoolsDict.OnCreatedDelegate> dictionary;
			(dictionary = this.onCreatedDelegates)[poolName] = (SpawnPoolsDict.OnCreatedDelegate)Delegate.Remove(dictionary[poolName], createdDelegate);
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x000EF4C0 File Offset: 0x000ED6C0
		public SpawnPool Create(string poolName)
		{
			GameObject gameObject = new GameObject(poolName + "Pool");
			return gameObject.AddComponent<SpawnPool>();
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x000EF4E4 File Offset: 0x000ED6E4
		public SpawnPool Create(string poolName, GameObject owner)
		{
			if (!this.assertValidPoolName(poolName))
			{
				return null;
			}
			string name = owner.gameObject.name;
			SpawnPool spawnPool;
			try
			{
				owner.gameObject.name = poolName;
				spawnPool = owner.AddComponent<SpawnPool>();
			}
			finally
			{
				owner.gameObject.name = name;
			}
			return spawnPool;
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x000EF544 File Offset: 0x000ED744
		private bool assertValidPoolName(string poolName)
		{
			string text = poolName.Replace("Pool", string.Empty);
			if (text != poolName)
			{
				string text2 = string.Format("'{0}' has the word 'Pool' in it. This word is reserved for GameObject defaul naming. The pool name has been changed to '{1}'", poolName, text);
				Debug.LogWarning(text2);
				poolName = text;
			}
			if (this.ContainsKey(poolName))
			{
				Debug.Log(string.Format("A pool with the name '{0}' already exists", poolName));
				return false;
			}
			return true;
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x000EF5A4 File Offset: 0x000ED7A4
		public override string ToString()
		{
			string[] array = new string[this._pools.Count];
			this._pools.Keys.CopyTo(array, 0);
			return string.Format("[{0}]", string.Join(", ", array));
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x000EF5EC File Offset: 0x000ED7EC
		public bool Destroy(string poolName)
		{
			SpawnPool spawnPool;
			if (!this._pools.TryGetValue(poolName, out spawnPool))
			{
				Debug.LogError(string.Format("PoolManager: Unable to destroy '{0}'. Not in PoolManager", poolName));
				return false;
			}
			UnityEngine.Object.Destroy(spawnPool.gameObject);
			return true;
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x000EF62C File Offset: 0x000ED82C
		public void DestroyAll()
		{
			foreach (KeyValuePair<string, SpawnPool> keyValuePair in this._pools)
			{
				UnityEngine.Object.Destroy(keyValuePair.Value);
			}
			this._pools.Clear();
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x000EF698 File Offset: 0x000ED898
		internal void Add(SpawnPool spawnPool)
		{
			if (this.ContainsKey(spawnPool.poolName))
			{
				Debug.LogError(string.Format("A pool with the name '{0}' already exists. This should only happen if a SpawnPool with this name is added to a scene twice.", spawnPool.poolName));
				return;
			}
			this._pools.Add(spawnPool.poolName, spawnPool);
			if (this.onCreatedDelegates.ContainsKey(spawnPool.poolName))
			{
				this.onCreatedDelegates[spawnPool.poolName](spawnPool);
			}
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x000EF70C File Offset: 0x000ED90C
		public void Add(string key, SpawnPool value)
		{
			string text = "SpawnPools add themselves to PoolManager.Pools when created, so there is no need to Add() them explicitly. Create pools using PoolManager.Pools.Create() or add a SpawnPool component to a GameObject.";
			throw new NotImplementedException(text);
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x000EF728 File Offset: 0x000ED928
		internal bool Remove(SpawnPool spawnPool)
		{
			if (!this.ContainsKey(spawnPool.poolName))
			{
				Debug.LogError(string.Format("PoolManager: Unable to remove '{0}'. Pool not in PoolManager", spawnPool.poolName));
				return false;
			}
			this._pools.Remove(spawnPool.poolName);
			return true;
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000EF768 File Offset: 0x000ED968
		public bool Remove(string poolName)
		{
			string text = "SpawnPools can only be destroyed, not removed and kept alive outside of PoolManager. There are only 2 legal ways to destroy a SpawnPool: Destroy the GameObject directly, if you have a reference, or use PoolManager.Destroy(string poolName).";
			throw new NotImplementedException(text);
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002E08 RID: 11784 RVA: 0x000EF784 File Offset: 0x000ED984
		public int Count
		{
			get
			{
				return this._pools.Count;
			}
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x000EF794 File Offset: 0x000ED994
		public bool ContainsKey(string poolName)
		{
			return this._pools.ContainsKey(poolName);
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x000EF7A4 File Offset: 0x000ED9A4
		public bool TryGetValue(string poolName, out SpawnPool spawnPool)
		{
			return this._pools.TryGetValue(poolName, out spawnPool);
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x000EF7B4 File Offset: 0x000ED9B4
		public bool Contains(KeyValuePair<string, SpawnPool> item)
		{
			string text = "Use PoolManager.Pools.Contains(string poolName) instead.";
			throw new NotImplementedException(text);
		}

		// Token: 0x17000886 RID: 2182
		public SpawnPool this[string key]
		{
			get
			{
				SpawnPool spawnPool;
				try
				{
					spawnPool = this._pools[key];
				}
				catch (KeyNotFoundException)
				{
					string text = string.Format("A Pool with the name '{0}' not found. \nPools={1}", key, this.ToString());
					throw new KeyNotFoundException(text);
				}
				return spawnPool;
			}
			set
			{
				string text = "Cannot set PoolManager.Pools[key] directly. SpawnPools add themselves to PoolManager.Pools when created, so there is no need to set them explicitly. Create pools using PoolManager.Pools.Create() or add a SpawnPool component to a GameObject.";
				throw new NotImplementedException(text);
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002E0E RID: 11790 RVA: 0x000EF838 File Offset: 0x000EDA38
		public ICollection<string> Keys
		{
			get
			{
				string text = "If you need this, please request it.";
				throw new NotImplementedException(text);
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002E0F RID: 11791 RVA: 0x000EF854 File Offset: 0x000EDA54
		public ICollection<SpawnPool> Values
		{
			get
			{
				string text = "If you need this, please request it.";
				throw new NotImplementedException(text);
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06002E10 RID: 11792 RVA: 0x000EF870 File Offset: 0x000EDA70
		private bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002E11 RID: 11793 RVA: 0x000EF874 File Offset: 0x000EDA74
		bool ICollection<KeyValuePair<string, SpawnPool>>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x000EF878 File Offset: 0x000EDA78
		public void Add(KeyValuePair<string, SpawnPool> item)
		{
			string text = "SpawnPools add themselves to PoolManager.Pools when created, so there is no need to Add() them explicitly. Create pools using PoolManager.Pools.Create() or add a SpawnPool component to a GameObject.";
			throw new NotImplementedException(text);
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x000EF894 File Offset: 0x000EDA94
		public void Clear()
		{
			string text = "Use PoolManager.Pools.DestroyAll() instead.";
			throw new NotImplementedException(text);
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x000EF8B0 File Offset: 0x000EDAB0
		private void CopyTo(KeyValuePair<string, SpawnPool>[] array, int arrayIndex)
		{
			string text = "PoolManager.Pools cannot be copied";
			throw new NotImplementedException(text);
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000EF8CC File Offset: 0x000EDACC
		void ICollection<KeyValuePair<string, SpawnPool>>.CopyTo(KeyValuePair<string, SpawnPool>[] array, int arrayIndex)
		{
			string text = "PoolManager.Pools cannot be copied";
			throw new NotImplementedException(text);
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x000EF8E8 File Offset: 0x000EDAE8
		public bool Remove(KeyValuePair<string, SpawnPool> item)
		{
			string text = "SpawnPools can only be destroyed, not removed and kept alive outside of PoolManager. There are only 2 legal ways to destroy a SpawnPool: Destroy the GameObject directly, if you have a reference, or use PoolManager.Destroy(string poolName).";
			throw new NotImplementedException(text);
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x000EF904 File Offset: 0x000EDB04
		public IEnumerator<KeyValuePair<string, SpawnPool>> GetEnumerator()
		{
			return this._pools.GetEnumerator();
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x000EF918 File Offset: 0x000EDB18
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._pools.GetEnumerator();
		}

		// Token: 0x04001F2B RID: 7979
		internal Dictionary<string, SpawnPoolsDict.OnCreatedDelegate> onCreatedDelegates = new Dictionary<string, SpawnPoolsDict.OnCreatedDelegate>();

		// Token: 0x04001F2C RID: 7980
		private Dictionary<string, SpawnPool> _pools = new Dictionary<string, SpawnPool>();

		// Token: 0x02000840 RID: 2112
		// (Invoke) Token: 0x06002E1A RID: 11802
		public delegate void OnCreatedDelegate(SpawnPool pool);
	}
}
