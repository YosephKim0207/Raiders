using System;
using System.Runtime.InteropServices;

// Token: 0x0200179C RID: 6044
public static class ProfileUtils
{
	// Token: 0x06008D78 RID: 36216 RVA: 0x003B86AC File Offset: 0x003B68AC
	public static int GetMonoCollectionCount()
	{
		return ProfileUtils.mono_gc_collection_count(0);
	}

	// Token: 0x06008D79 RID: 36217 RVA: 0x003B86B4 File Offset: 0x003B68B4
	public static uint GetMonoHeapSize()
	{
		return (uint)ProfileUtils.mono_gc_get_heap_size();
	}

	// Token: 0x06008D7A RID: 36218 RVA: 0x003B86BC File Offset: 0x003B68BC
	public static uint GetMonoUsedHeapSize()
	{
		return (uint)ProfileUtils.mono_gc_get_used_size();
	}

	// Token: 0x06008D7B RID: 36219
	[DllImport("mono")]
	private static extern int mono_gc_collection_count(int generation);

	// Token: 0x06008D7C RID: 36220
	[DllImport("mono")]
	private static extern long mono_gc_get_heap_size();

	// Token: 0x06008D7D RID: 36221
	[DllImport("mono")]
	private static extern long mono_gc_get_used_size();
}
