using System;
using DaikonForge.Tween;
using UnityEngine;

// Token: 0x02000519 RID: 1305
public static class Tween
{
	// Token: 0x06001F37 RID: 7991 RVA: 0x0008CB48 File Offset: 0x0008AD48
	public static Tween<Color> Color(SpriteRenderer renderer)
	{
		return renderer.TweenColor();
	}

	// Token: 0x06001F38 RID: 7992 RVA: 0x0008CB50 File Offset: 0x0008AD50
	public static Tween<float> Alpha(SpriteRenderer renderer)
	{
		return renderer.TweenAlpha();
	}

	// Token: 0x06001F39 RID: 7993 RVA: 0x0008CB58 File Offset: 0x0008AD58
	public static Tween<Color> Color(Material material)
	{
		return material.TweenColor();
	}

	// Token: 0x06001F3A RID: 7994 RVA: 0x0008CB60 File Offset: 0x0008AD60
	public static Tween<float> Alpha(Material material)
	{
		return material.TweenAlpha();
	}

	// Token: 0x06001F3B RID: 7995 RVA: 0x0008CB68 File Offset: 0x0008AD68
	public static Tween<Vector3> Position(Transform transform)
	{
		return Tween.Position(transform, false);
	}

	// Token: 0x06001F3C RID: 7996 RVA: 0x0008CB74 File Offset: 0x0008AD74
	public static Tween<Vector3> Position(Transform transform, bool useLocalPosition)
	{
		return transform.TweenPosition(useLocalPosition);
	}

	// Token: 0x06001F3D RID: 7997 RVA: 0x0008CB80 File Offset: 0x0008AD80
	public static Tween<Vector3> MoveTo(Transform transform, Vector3 endPosition)
	{
		return Tween.MoveTo(transform, endPosition, false);
	}

	// Token: 0x06001F3E RID: 7998 RVA: 0x0008CB8C File Offset: 0x0008AD8C
	public static Tween<Vector3> MoveTo(Transform transform, Vector3 endPosition, bool useLocalPosition)
	{
		return transform.TweenMoveTo(endPosition, useLocalPosition);
	}

	// Token: 0x06001F3F RID: 7999 RVA: 0x0008CB98 File Offset: 0x0008AD98
	public static Tween<Vector3> MoveFrom(Transform transform, Vector3 startPosition)
	{
		return transform.TweenMoveFrom(startPosition, false);
	}

	// Token: 0x06001F40 RID: 8000 RVA: 0x0008CBA4 File Offset: 0x0008ADA4
	public static Tween<Vector3> MoveFrom(Transform transform, Vector3 startPosition, bool useLocalPosition)
	{
		return transform.TweenMoveFrom(startPosition, useLocalPosition);
	}

	// Token: 0x06001F41 RID: 8001 RVA: 0x0008CBB0 File Offset: 0x0008ADB0
	public static Tween<Vector3> RotateFrom(Transform transform, Vector3 startRotation)
	{
		return Tween.RotateFrom(transform, startRotation, true, false);
	}

	// Token: 0x06001F42 RID: 8002 RVA: 0x0008CBBC File Offset: 0x0008ADBC
	public static Tween<Vector3> RotateFrom(Transform transform, Vector3 startRotation, bool useShortestPath)
	{
		return Tween.RotateFrom(transform, startRotation, useShortestPath, false);
	}

	// Token: 0x06001F43 RID: 8003 RVA: 0x0008CBC8 File Offset: 0x0008ADC8
	public static Tween<Vector3> RotateFrom(Transform transform, Vector3 startRotation, bool useShortestPath, bool useLocalRotation)
	{
		return transform.TweenRotateFrom(startRotation, useShortestPath, useLocalRotation);
	}

	// Token: 0x06001F44 RID: 8004 RVA: 0x0008CBD4 File Offset: 0x0008ADD4
	public static Tween<Vector3> RotateTo(Transform transform, Vector3 endRotation)
	{
		return Tween.RotateTo(transform, endRotation, true, false);
	}

	// Token: 0x06001F45 RID: 8005 RVA: 0x0008CBE0 File Offset: 0x0008ADE0
	public static Tween<Vector3> RotateTo(Transform transform, Vector3 endRotation, bool useShortestPath)
	{
		return Tween.RotateTo(transform, endRotation, useShortestPath, false);
	}

	// Token: 0x06001F46 RID: 8006 RVA: 0x0008CBEC File Offset: 0x0008ADEC
	public static Tween<Vector3> RotateTo(Transform transform, Vector3 endRotation, bool useShortestPath, bool useLocalRotation)
	{
		return transform.TweenRotateTo(endRotation, useShortestPath, useLocalRotation);
	}

	// Token: 0x06001F47 RID: 8007 RVA: 0x0008CBF8 File Offset: 0x0008ADF8
	public static Tween<Vector3> Rotation(Transform transform)
	{
		return transform.TweenRotation();
	}

	// Token: 0x06001F48 RID: 8008 RVA: 0x0008CC00 File Offset: 0x0008AE00
	public static Tween<Vector3> Rotation(Transform transform, bool useShortestPath)
	{
		return Tween.Rotation(transform, useShortestPath, false);
	}

	// Token: 0x06001F49 RID: 8009 RVA: 0x0008CC0C File Offset: 0x0008AE0C
	public static Tween<Vector3> Rotation(Transform transform, bool useShortestPath, bool useLocalRotation)
	{
		return transform.TweenRotation(useShortestPath, useLocalRotation);
	}

	// Token: 0x06001F4A RID: 8010 RVA: 0x0008CC18 File Offset: 0x0008AE18
	public static Tween<Vector3> ScaleFrom(Transform transform, Vector3 startScale)
	{
		return Tween.Scale(transform).SetStartValue(startScale);
	}

	// Token: 0x06001F4B RID: 8011 RVA: 0x0008CC28 File Offset: 0x0008AE28
	public static Tween<Vector3> ScaleTo(Transform transform, Vector3 endScale)
	{
		return Tween.Scale(transform).SetEndValue(endScale);
	}

	// Token: 0x06001F4C RID: 8012 RVA: 0x0008CC38 File Offset: 0x0008AE38
	public static Tween<Vector3> Scale(Transform transform)
	{
		return transform.TweenScale();
	}

	// Token: 0x06001F4D RID: 8013 RVA: 0x0008CC40 File Offset: 0x0008AE40
	public static TweenShake Shake(Transform transform)
	{
		return Tween.Shake(transform, false);
	}

	// Token: 0x06001F4E RID: 8014 RVA: 0x0008CC4C File Offset: 0x0008AE4C
	public static TweenShake Shake(Transform transform, bool localPosition)
	{
		return transform.ShakePosition(localPosition);
	}

	// Token: 0x06001F4F RID: 8015 RVA: 0x0008CC58 File Offset: 0x0008AE58
	public static Tween<T> NamedProperty<T>(object target, string propertyName)
	{
		return TweenNamedProperty<T>.Obtain(target, propertyName);
	}
}
