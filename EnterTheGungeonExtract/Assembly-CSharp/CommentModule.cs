using System;

// Token: 0x020011DB RID: 4571
[Serializable]
public struct CommentModule
{
	// Token: 0x040061CD RID: 25037
	public string stringKey;

	// Token: 0x040061CE RID: 25038
	public float duration;

	// Token: 0x040061CF RID: 25039
	public CommentModule.CommentTarget target;

	// Token: 0x040061D0 RID: 25040
	public float delay;

	// Token: 0x020011DC RID: 4572
	public enum CommentTarget
	{
		// Token: 0x040061D2 RID: 25042
		PRIMARY,
		// Token: 0x040061D3 RID: 25043
		SECONDARY,
		// Token: 0x040061D4 RID: 25044
		DOG
	}
}
