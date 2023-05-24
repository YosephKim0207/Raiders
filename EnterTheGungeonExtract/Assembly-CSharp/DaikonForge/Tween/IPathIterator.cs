using System;
using UnityEngine;

namespace DaikonForge.Tween
{
	// Token: 0x0200052B RID: 1323
	public interface IPathIterator
	{
		// Token: 0x06001FC9 RID: 8137
		Vector3 GetPosition(float time);

		// Token: 0x06001FCA RID: 8138
		Vector3 GetTangent(float time);
	}
}
