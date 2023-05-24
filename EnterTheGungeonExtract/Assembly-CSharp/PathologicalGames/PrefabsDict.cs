using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	// Token: 0x0200084B RID: 2123
	public class PrefabsDict : IDictionary<string, Transform>, ICollection<KeyValuePair<string, Transform>>, IEnumerable<KeyValuePair<string, Transform>>, IEnumerable
	{
		// Token: 0x06002E96 RID: 11926 RVA: 0x000F1A28 File Offset: 0x000EFC28
		public override string ToString()
		{
			string[] array = new string[this._prefabs.Count];
			this._prefabs.Keys.CopyTo(array, 0);
			return string.Format("[{0}]", string.Join(", ", array));
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x000F1A70 File Offset: 0x000EFC70
		internal void _Add(string prefabName, Transform prefab)
		{
			this._prefabs.Add(prefabName, prefab);
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x000F1A80 File Offset: 0x000EFC80
		internal bool _Remove(string prefabName)
		{
			return this._prefabs.Remove(prefabName);
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x000F1A90 File Offset: 0x000EFC90
		internal void _Clear()
		{
			this._prefabs.Clear();
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002E9A RID: 11930 RVA: 0x000F1AA0 File Offset: 0x000EFCA0
		public int Count
		{
			get
			{
				return this._prefabs.Count;
			}
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x000F1AB0 File Offset: 0x000EFCB0
		public bool ContainsKey(string prefabName)
		{
			return this._prefabs.ContainsKey(prefabName);
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x000F1AC0 File Offset: 0x000EFCC0
		public bool TryGetValue(string prefabName, out Transform prefab)
		{
			return this._prefabs.TryGetValue(prefabName, out prefab);
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x000F1AD0 File Offset: 0x000EFCD0
		public void Add(string key, Transform value)
		{
			throw new NotImplementedException("Read-Only");
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x000F1ADC File Offset: 0x000EFCDC
		public bool Remove(string prefabName)
		{
			throw new NotImplementedException("Read-Only");
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x000F1AE8 File Offset: 0x000EFCE8
		public bool Contains(KeyValuePair<string, Transform> item)
		{
			string text = "Use Contains(string prefabName) instead.";
			throw new NotImplementedException(text);
		}

		// Token: 0x170008A5 RID: 2213
		public Transform this[string key]
		{
			get
			{
				Transform transform;
				try
				{
					transform = this._prefabs[key];
				}
				catch (KeyNotFoundException)
				{
					string text = string.Format("A Prefab with the name '{0}' not found. \nPrefabs={1}", key, this.ToString());
					throw new KeyNotFoundException(text);
				}
				return transform;
			}
			set
			{
				throw new NotImplementedException("Read-only.");
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06002EA2 RID: 11938 RVA: 0x000F1B5C File Offset: 0x000EFD5C
		public ICollection<string> Keys
		{
			get
			{
				return this._prefabs.Keys;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06002EA3 RID: 11939 RVA: 0x000F1B6C File Offset: 0x000EFD6C
		public ICollection<Transform> Values
		{
			get
			{
				return this._prefabs.Values;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06002EA4 RID: 11940 RVA: 0x000F1B7C File Offset: 0x000EFD7C
		private bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002EA5 RID: 11941 RVA: 0x000F1B80 File Offset: 0x000EFD80
		bool ICollection<KeyValuePair<string, Transform>>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x000F1B84 File Offset: 0x000EFD84
		public void Add(KeyValuePair<string, Transform> item)
		{
			throw new NotImplementedException("Read-only");
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x000F1B90 File Offset: 0x000EFD90
		public void Clear()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x000F1B98 File Offset: 0x000EFD98
		private void CopyTo(KeyValuePair<string, Transform>[] array, int arrayIndex)
		{
			string text = "Cannot be copied";
			throw new NotImplementedException(text);
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x000F1BB4 File Offset: 0x000EFDB4
		void ICollection<KeyValuePair<string, Transform>>.CopyTo(KeyValuePair<string, Transform>[] array, int arrayIndex)
		{
			string text = "Cannot be copied";
			throw new NotImplementedException(text);
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x000F1BD0 File Offset: 0x000EFDD0
		public bool Remove(KeyValuePair<string, Transform> item)
		{
			throw new NotImplementedException("Read-only");
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x000F1BDC File Offset: 0x000EFDDC
		public IEnumerator<KeyValuePair<string, Transform>> GetEnumerator()
		{
			return this._prefabs.GetEnumerator();
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x000F1BF0 File Offset: 0x000EFDF0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._prefabs.GetEnumerator();
		}

		// Token: 0x04001F80 RID: 8064
		private Dictionary<string, Transform> _prefabs = new Dictionary<string, Transform>();
	}
}
