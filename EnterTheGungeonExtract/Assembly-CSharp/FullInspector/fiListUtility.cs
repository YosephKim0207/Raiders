using System;
using System.Collections;
using System.Collections.Generic;

namespace FullInspector
{
	// Token: 0x02000575 RID: 1397
	public static class fiListUtility
	{
		// Token: 0x060020FD RID: 8445 RVA: 0x000918B0 File Offset: 0x0008FAB0
		public static void Add<T>(ref IList list)
		{
			if (list.GetType().IsArray)
			{
				T[] array = (T[])list;
				Array.Resize<T>(ref array, array.Length + 1);
				list = array;
			}
			else
			{
				list.Add(default(T));
			}
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x00091900 File Offset: 0x0008FB00
		public static void InsertAt<T>(ref IList list, int index)
		{
			if (list.GetType().IsArray)
			{
				List<T> list2 = new List<T>((IList<T>)list);
				list2.Insert(index, default(T));
				list = list2.ToArray();
			}
			else
			{
				list.Insert(index, default(T));
			}
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x00091960 File Offset: 0x0008FB60
		public static void RemoveAt<T>(ref IList list, int index)
		{
			if (list.GetType().IsArray)
			{
				List<T> list2 = new List<T>((IList<T>)list);
				list2.RemoveAt(index);
				list = list2.ToArray();
			}
			else
			{
				list.RemoveAt(index);
			}
		}
	}
}
