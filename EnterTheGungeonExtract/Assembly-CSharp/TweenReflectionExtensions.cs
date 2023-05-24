using System;
using DaikonForge.Tween;
using DaikonForge.Tween.Interpolation;

// Token: 0x02000508 RID: 1288
public static class TweenReflectionExtensions
{
	// Token: 0x06001EE9 RID: 7913 RVA: 0x0008C06C File Offset: 0x0008A26C
	public static Tween<T> TweenProperty<T>(this object target, string propertyName)
	{
		return TweenNamedProperty<T>.Obtain(target, propertyName);
	}

	// Token: 0x06001EEA RID: 7914 RVA: 0x0008C078 File Offset: 0x0008A278
	public static Tween<T> TweenProperty<T>(this object target, string propertyName, Interpolator<T> interpolator)
	{
		return TweenNamedProperty<T>.Obtain(target, propertyName, interpolator);
	}
}
