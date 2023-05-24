using System;
using System.Collections.Generic;

// Token: 0x02000415 RID: 1045
internal class dfTempArray<T>
{
	// Token: 0x060017BD RID: 6077 RVA: 0x000717D0 File Offset: 0x0006F9D0
	public static void Clear()
	{
		dfTempArray<T>.cache.Clear();
	}

	// Token: 0x060017BE RID: 6078 RVA: 0x000717DC File Offset: 0x0006F9DC
	public static T[] Obtain(int length)
	{
		return dfTempArray<T>.Obtain(length, 128);
	}

	// Token: 0x060017BF RID: 6079 RVA: 0x000717EC File Offset: 0x0006F9EC
	public static T[] Obtain(int length, int maxCacheSize)
	{
		object obj = dfTempArray<T>.cache;
		T[] array3;
		lock (obj)
		{
			for (int i = 0; i < dfTempArray<T>.cache.Count; i++)
			{
				T[] array = dfTempArray<T>.cache[i];
				if (array.Length == length)
				{
					if (i > 0)
					{
						dfTempArray<T>.cache.RemoveAt(i);
						dfTempArray<T>.cache.Insert(0, array);
					}
					return array;
				}
			}
			if (dfTempArray<T>.cache.Count >= maxCacheSize)
			{
				dfTempArray<T>.cache.RemoveAt(dfTempArray<T>.cache.Count - 1);
			}
			T[] array2 = new T[length];
			dfTempArray<T>.cache.Insert(0, array2);
			array3 = array2;
		}
		return array3;
	}

	// Token: 0x04001315 RID: 4885
	private static List<T[]> cache = new List<T[]>(32);
}
