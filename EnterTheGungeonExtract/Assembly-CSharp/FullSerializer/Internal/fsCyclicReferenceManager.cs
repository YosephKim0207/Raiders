using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FullSerializer.Internal
{
	// Token: 0x020005B3 RID: 1459
	public class fsCyclicReferenceManager
	{
		// Token: 0x060022AF RID: 8879 RVA: 0x000990BC File Offset: 0x000972BC
		public void Enter()
		{
			this._depth++;
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x000990CC File Offset: 0x000972CC
		public bool Exit()
		{
			this._depth--;
			if (this._depth == 0)
			{
				this._objectIds = new Dictionary<object, int>(fsCyclicReferenceManager.ObjectReferenceEqualityComparator.Instance);
				this._nextId = 0;
				this._marked = new Dictionary<int, object>();
			}
			if (this._depth < 0)
			{
				this._depth = 0;
				throw new InvalidOperationException("Internal Error - Mismatched Enter/Exit");
			}
			return this._depth == 0;
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x0009913C File Offset: 0x0009733C
		public object GetReferenceObject(int id)
		{
			if (!this._marked.ContainsKey(id))
			{
				throw new InvalidOperationException("Internal Deserialization Error - Object definition has not been encountered for object with id=" + id + "; have you reordered or modified the serialized data? If this is an issue with an unmodified Full Json implementation and unmodified serialization data, please report an issue with an included test case.");
			}
			return this._marked[id];
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x00099178 File Offset: 0x00097378
		public void AddReferenceWithId(int id, object reference)
		{
			this._marked[id] = reference;
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x00099188 File Offset: 0x00097388
		public int GetReferenceId(object item)
		{
			int num;
			if (!this._objectIds.TryGetValue(item, out num))
			{
				num = this._nextId++;
				this._objectIds[item] = num;
			}
			return num;
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x000991C8 File Offset: 0x000973C8
		public bool IsReference(object item)
		{
			return this._marked.ContainsKey(this.GetReferenceId(item));
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x000991DC File Offset: 0x000973DC
		public void MarkSerialized(object item)
		{
			int referenceId = this.GetReferenceId(item);
			if (this._marked.ContainsKey(referenceId))
			{
				throw new InvalidOperationException("Internal Error - " + item + " has already been marked as serialized");
			}
			this._marked[referenceId] = item;
		}

		// Token: 0x04001869 RID: 6249
		private Dictionary<object, int> _objectIds = new Dictionary<object, int>(fsCyclicReferenceManager.ObjectReferenceEqualityComparator.Instance);

		// Token: 0x0400186A RID: 6250
		private int _nextId;

		// Token: 0x0400186B RID: 6251
		private Dictionary<int, object> _marked = new Dictionary<int, object>();

		// Token: 0x0400186C RID: 6252
		private int _depth;

		// Token: 0x020005B4 RID: 1460
		private class ObjectReferenceEqualityComparator : IEqualityComparer<object>
		{
			// Token: 0x060022B7 RID: 8887 RVA: 0x00099230 File Offset: 0x00097430
			bool IEqualityComparer<object>.Equals(object x, object y)
			{
				return object.ReferenceEquals(x, y);
			}

			// Token: 0x060022B8 RID: 8888 RVA: 0x0009923C File Offset: 0x0009743C
			int IEqualityComparer<object>.GetHashCode(object obj)
			{
				return RuntimeHelpers.GetHashCode(obj);
			}

			// Token: 0x0400186D RID: 6253
			public static readonly IEqualityComparer<object> Instance = new fsCyclicReferenceManager.ObjectReferenceEqualityComparator();
		}
	}
}
