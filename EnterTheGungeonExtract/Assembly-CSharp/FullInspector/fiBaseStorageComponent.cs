using System;
using System.Collections.Generic;
using FullInspector.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000612 RID: 1554
	[AddComponentMenu("")]
	public abstract class fiBaseStorageComponent<T> : MonoBehaviour, fiIEditorOnlyTag, ISerializationCallbackReceiver
	{
		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x0600244B RID: 9291 RVA: 0x0009DD94 File Offset: 0x0009BF94
		public IDictionary<UnityEngine.Object, T> Data
		{
			get
			{
				if (this._data == null)
				{
					this._data = new Dictionary<UnityEngine.Object, T>();
				}
				return this._data;
			}
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x0009DDB4 File Offset: 0x0009BFB4
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this._keys == null || this._values == null)
			{
				return;
			}
			this._data = new Dictionary<UnityEngine.Object, T>();
			for (int i = 0; i < Math.Min(this._keys.Count, this._values.Count); i++)
			{
				if (!object.ReferenceEquals(this._keys[i], null))
				{
					this.Data[this._keys[i]] = this._values[i];
				}
			}
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x0009DE4C File Offset: 0x0009C04C
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this._data == null)
			{
				this._keys = null;
				this._values = null;
				return;
			}
			this._keys = new List<UnityEngine.Object>(this._data.Count);
			this._values = new List<T>(this._data.Count);
			foreach (KeyValuePair<UnityEngine.Object, T> keyValuePair in this._data)
			{
				this._keys.Add(keyValuePair.Key);
				this._values.Add(keyValuePair.Value);
			}
		}

		// Token: 0x0400191E RID: 6430
		[SerializeField]
		private List<UnityEngine.Object> _keys;

		// Token: 0x0400191F RID: 6431
		[SerializeField]
		private List<T> _values;

		// Token: 0x04001920 RID: 6432
		private IDictionary<UnityEngine.Object, T> _data;
	}
}
