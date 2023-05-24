using System;
using UnityEngine;

namespace BraveDynamicTree
{
	// Token: 0x02000358 RID: 856
	public interface RigidbodyContainer
	{
		// Token: 0x06000D78 RID: 3448
		void RayCast(b2RayCastInput input, Func<b2RayCastInput, SpeculativeRigidbody, float> callback);

		// Token: 0x06000D79 RID: 3449
		void Query(b2AABB aabb, Func<SpeculativeRigidbody, bool> callback);

		// Token: 0x06000D7A RID: 3450
		int CreateProxy(b2AABB aabb, SpeculativeRigidbody rigidbody);

		// Token: 0x06000D7B RID: 3451
		bool MoveProxy(int proxyId, b2AABB aabb, Vector2 displacement);

		// Token: 0x06000D7C RID: 3452
		void DestroyProxy(int proxyId);
	}
}
