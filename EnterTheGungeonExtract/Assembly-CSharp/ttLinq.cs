using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02001838 RID: 6200
public static class ttLinq
{
	// Token: 0x060092C9 RID: 37577 RVA: 0x003E0040 File Offset: 0x003DE240
	public static TSource ttAggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
	{
		TSource tsource = default(TSource);
		for (int i = 0; i < source.Count<TSource>(); i++)
		{
			tsource = func(tsource, source.ElementAt(i));
		}
		return tsource;
	}

	// Token: 0x060092CA RID: 37578 RVA: 0x003E0080 File Offset: 0x003DE280
	public static TAccumulate ttAggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
	{
		TAccumulate taccumulate = seed;
		for (int i = 0; i < source.Count<TSource>(); i++)
		{
			taccumulate = func(taccumulate, source.ElementAt(i));
		}
		return taccumulate;
	}

	// Token: 0x060092CB RID: 37579 RVA: 0x003E00B8 File Offset: 0x003DE2B8
	public static TResult ttAggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
	{
		TAccumulate taccumulate = seed;
		for (int i = 0; i < source.Count<TSource>(); i++)
		{
			taccumulate = func(taccumulate, source.ElementAt(i));
		}
		return resultSelector(taccumulate);
	}

	// Token: 0x060092CC RID: 37580 RVA: 0x003E00F8 File Offset: 0x003DE2F8
	public static TSource ttLast<TSource>(this IEnumerable<TSource> source)
	{
		int num = source.Count<TSource>() - 1;
		return source.ElementAt(num);
	}

	// Token: 0x060092CD RID: 37581 RVA: 0x003E0118 File Offset: 0x003DE318
	public static List<TSource> ttOrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable
	{
		TSource[] array = source.ToArray<TSource>();
		TKey[] array2 = array.Select(keySelector).ToArray<TKey>();
		Array.Sort<TKey, TSource>(array2, array);
		return array.ToList<TSource>();
	}

	// Token: 0x060092CE RID: 37582 RVA: 0x003E0148 File Offset: 0x003DE348
	public static List<TSource> ttThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		IOrderedEnumerable<TSource> orderedEnumerable = source.CreateOrderedEnumerable<TKey>(keySelector, Comparer<TKey>.Default, false);
		return orderedEnumerable.ToList<TSource>();
	}

	// Token: 0x060092CF RID: 37583 RVA: 0x003E016C File Offset: 0x003DE36C
	public static List<TSource> ttOrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable
	{
		TSource[] array = source.ToArray<TSource>();
		TKey[] array2 = array.Select(keySelector).ToArray<TKey>();
		Array.Sort<TKey, TSource>(array2, array);
		return array.Reverse<TSource>().ToList<TSource>();
	}

	// Token: 0x060092D0 RID: 37584 RVA: 0x003E01A0 File Offset: 0x003DE3A0
	public static TResult[] ttSelect<TSource, TResult>(this IList<TSource> source, Func<TSource, TResult> selector)
	{
		TResult[] array = new TResult[source.Count];
		for (int i = 0; i < source.Count; i++)
		{
			array[i] = selector(source[i]);
		}
		return array;
	}

	// Token: 0x060092D1 RID: 37585 RVA: 0x003E01E8 File Offset: 0x003DE3E8
	public static int ttSum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> func)
	{
		int num = 0;
		for (int i = 0; i < source.Count<TSource>(); i++)
		{
			num += func(source.ElementAt(i));
		}
		return num;
	}

	// Token: 0x060092D2 RID: 37586 RVA: 0x003E0220 File Offset: 0x003DE420
	public static float ttSum<TSource>(this IEnumerable<TSource> source, Func<TSource, float> func)
	{
		float num = 0f;
		for (int i = 0; i < source.Count<TSource>(); i++)
		{
			num += func(source.ElementAt(i));
		}
		return num;
	}

	// Token: 0x060092D3 RID: 37587 RVA: 0x003E025C File Offset: 0x003DE45C
	public static double ttSum<TSource>(this IEnumerable<TSource> source, Func<TSource, double> func)
	{
		double num = 0.0;
		for (int i = 0; i < source.Count<TSource>(); i++)
		{
			num += func(source.ElementAt(i));
		}
		return num;
	}

	// Token: 0x060092D4 RID: 37588 RVA: 0x003E029C File Offset: 0x003DE49C
	public static List<TSource> ttWhere<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> func)
	{
		List<TSource> list = new List<TSource>();
		foreach (TSource tsource in source)
		{
			if (func(tsource))
			{
				list.Add(tsource);
			}
		}
		return list;
	}
}
