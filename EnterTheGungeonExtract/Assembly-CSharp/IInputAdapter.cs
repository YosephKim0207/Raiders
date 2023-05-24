using System;
using UnityEngine;

// Token: 0x020003DA RID: 986
public interface IInputAdapter
{
	// Token: 0x0600138C RID: 5004
	bool GetKeyDown(KeyCode key);

	// Token: 0x0600138D RID: 5005
	bool GetKeyUp(KeyCode key);

	// Token: 0x0600138E RID: 5006
	float GetAxis(string axisName);

	// Token: 0x0600138F RID: 5007
	Vector2 GetMousePosition();

	// Token: 0x06001390 RID: 5008
	bool GetMouseButton(int button);

	// Token: 0x06001391 RID: 5009
	bool GetMouseButtonDown(int button);

	// Token: 0x06001392 RID: 5010
	bool GetMouseButtonUp(int button);
}
