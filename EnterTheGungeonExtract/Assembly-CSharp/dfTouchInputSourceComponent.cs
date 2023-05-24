using System;
using UnityEngine;

// Token: 0x02000425 RID: 1061
public abstract class dfTouchInputSourceComponent : MonoBehaviour
{
	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x06001858 RID: 6232
	public abstract IDFTouchInputSource Source { get; }

	// Token: 0x0400135C RID: 4956
	public int Priority;
}
