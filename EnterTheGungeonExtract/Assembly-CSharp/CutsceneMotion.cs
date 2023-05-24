using System;
using UnityEngine;

// Token: 0x02001029 RID: 4137
public class CutsceneMotion
{
	// Token: 0x06005AB8 RID: 23224 RVA: 0x0022A3D8 File Offset: 0x002285D8
	public CutsceneMotion(Transform t, Vector2? targetPosition, float s, float z = 0f)
	{
		this.transform = t;
		this.lerpStart = t.position.XY();
		this.lerpEnd = targetPosition;
		this.lerpProgress = 0f;
		this.speed = s;
		this.zOffset = z;
	}

	// Token: 0x0400541C RID: 21532
	public Transform transform;

	// Token: 0x0400541D RID: 21533
	public CameraController camera;

	// Token: 0x0400541E RID: 21534
	public Vector2 lerpStart;

	// Token: 0x0400541F RID: 21535
	public Vector2? lerpEnd;

	// Token: 0x04005420 RID: 21536
	public float lerpProgress;

	// Token: 0x04005421 RID: 21537
	public float speed;

	// Token: 0x04005422 RID: 21538
	public float zOffset;

	// Token: 0x04005423 RID: 21539
	public bool isSmoothStepped = true;
}
