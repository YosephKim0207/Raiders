using System;
using UnityEngine;

// Token: 0x02000EA5 RID: 3749
public class ObjectStampOptions : MonoBehaviour
{
	// Token: 0x06004F5C RID: 20316 RVA: 0x001B8584 File Offset: 0x001B6784
	public Vector3 GetPositionOffset()
	{
		return new Vector3(UnityEngine.Random.Range(this.xPositionRange.x, this.xPositionRange.y), UnityEngine.Random.Range(this.yPositionRange.x, this.yPositionRange.y));
	}

	// Token: 0x040046CF RID: 18127
	public Vector2 xPositionRange;

	// Token: 0x040046D0 RID: 18128
	public Vector2 yPositionRange;
}
