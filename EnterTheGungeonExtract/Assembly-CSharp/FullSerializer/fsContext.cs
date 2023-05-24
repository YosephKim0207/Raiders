using System;
using System.Collections.Generic;

namespace FullSerializer
{
	// Token: 0x0200059F RID: 1439
	public sealed class fsContext
	{
		// Token: 0x06002210 RID: 8720 RVA: 0x0009624C File Offset: 0x0009444C
		public void Reset()
		{
			this._contextObjects.Clear();
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x0009625C File Offset: 0x0009445C
		public void Set<T>(T obj)
		{
			this._contextObjects[typeof(T)] = obj;
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x0009627C File Offset: 0x0009447C
		public bool Has<T>()
		{
			return this._contextObjects.ContainsKey(typeof(T));
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x00096294 File Offset: 0x00094494
		public T Get<T>()
		{
			object obj;
			if (this._contextObjects.TryGetValue(typeof(T), out obj))
			{
				return (T)((object)obj);
			}
			throw new InvalidOperationException("There is no context object of type " + typeof(T));
		}

		// Token: 0x0400183A RID: 6202
		private readonly Dictionary<Type, object> _contextObjects = new Dictionary<Type, object>();
	}
}
