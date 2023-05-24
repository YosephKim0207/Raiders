using System;
using DaikonForge.Tween;
using UnityEngine;

// Token: 0x02000512 RID: 1298
public static class TweenComponentExtensions
{
	// Token: 0x06001EFD RID: 7933 RVA: 0x0008C3CC File Offset: 0x0008A5CC
	public static Tween<float> TweenAlpha(this Component component)
	{
		if (component is TextMesh)
		{
			return ((TextMesh)component).TweenAlpha();
		}
		if (component is GUIText)
		{
			return ((GUIText)component).material.TweenAlpha();
		}
		if (component.GetComponent<Renderer>() is SpriteRenderer)
		{
			return ((SpriteRenderer)component.GetComponent<Renderer>()).TweenAlpha();
		}
		if (component.GetComponent<Renderer>() == null)
		{
			throw new NullReferenceException("Component does not have a Renderer assigned");
		}
		Material material = component.GetComponent<Renderer>().material;
		if (material == null)
		{
			throw new NullReferenceException("Component does not have a Material assigned");
		}
		return material.TweenAlpha();
	}

	// Token: 0x06001EFE RID: 7934 RVA: 0x0008C474 File Offset: 0x0008A674
	public static Tween<Color> TweenColor(this Component component)
	{
		if (component is TextMesh)
		{
			return ((TextMesh)component).TweenColor();
		}
		if (component is GUIText)
		{
			return ((GUIText)component).material.TweenColor();
		}
		if (component.GetComponent<Renderer>() is SpriteRenderer)
		{
			return ((SpriteRenderer)component.GetComponent<Renderer>()).TweenColor();
		}
		if (component.GetComponent<Renderer>() == null)
		{
			throw new NullReferenceException("Component does not have a Renderer assigned");
		}
		Material material = component.GetComponent<Renderer>().material;
		if (material == null)
		{
			throw new NullReferenceException("Component does not have a Material assigned");
		}
		return material.TweenColor();
	}

	// Token: 0x06001EFF RID: 7935 RVA: 0x0008C51C File Offset: 0x0008A71C
	public static Tween<float> TweenPath(this Component component, IPathIterator path)
	{
		return component.TweenPath(path, true);
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x0008C528 File Offset: 0x0008A728
	public static Tween<float> TweenPath(this Component component, IPathIterator path, bool orientToPath)
	{
		if (component == null)
		{
			throw new ArgumentNullException("component");
		}
		return component.transform.TweenPath(path);
	}

	// Token: 0x06001F01 RID: 7937 RVA: 0x0008C550 File Offset: 0x0008A750
	public static TweenShake ShakePosition(this Component component)
	{
		return component.transform.ShakePosition();
	}

	// Token: 0x06001F02 RID: 7938 RVA: 0x0008C560 File Offset: 0x0008A760
	public static TweenShake ShakePosition(this Component component, bool localPosition)
	{
		return component.transform.ShakePosition(localPosition);
	}

	// Token: 0x06001F03 RID: 7939 RVA: 0x0008C570 File Offset: 0x0008A770
	public static Tween<Vector3> TweenScaleFrom(this Component component, Vector3 startScale)
	{
		return component.TweenScale().SetStartValue(startScale);
	}

	// Token: 0x06001F04 RID: 7940 RVA: 0x0008C580 File Offset: 0x0008A780
	public static Tween<Vector3> TweenScaleTo(this Component component, Vector3 endScale)
	{
		return component.transform.TweenScale().SetEndValue(endScale);
	}

	// Token: 0x06001F05 RID: 7941 RVA: 0x0008C594 File Offset: 0x0008A794
	public static Tween<Vector3> TweenScale(this Component component)
	{
		return component.transform.TweenScale();
	}

	// Token: 0x06001F06 RID: 7942 RVA: 0x0008C5A4 File Offset: 0x0008A7A4
	public static Tween<Vector3> TweenRotateFrom(this Component component, Vector3 startRotation)
	{
		return component.TweenRotateFrom(startRotation, true, false);
	}

	// Token: 0x06001F07 RID: 7943 RVA: 0x0008C5B0 File Offset: 0x0008A7B0
	public static Tween<Vector3> TweenRotateFrom(this Component component, Vector3 startRotation, bool useShortestPath)
	{
		return component.TweenRotateFrom(startRotation, useShortestPath, false);
	}

	// Token: 0x06001F08 RID: 7944 RVA: 0x0008C5BC File Offset: 0x0008A7BC
	public static Tween<Vector3> TweenRotateFrom(this Component component, Vector3 startRotation, bool useShortestPath, bool useLocalRotation)
	{
		return component.transform.TweenRotation(useShortestPath, useLocalRotation).SetStartValue(startRotation);
	}

	// Token: 0x06001F09 RID: 7945 RVA: 0x0008C5D4 File Offset: 0x0008A7D4
	public static Tween<Vector3> TweenRotateTo(this Component component, Vector3 endRotation)
	{
		return component.TweenRotateTo(endRotation, true, false);
	}

	// Token: 0x06001F0A RID: 7946 RVA: 0x0008C5E0 File Offset: 0x0008A7E0
	public static Tween<Vector3> TweenRotateTo(this Component component, Vector3 endRotation, bool useShortestPath)
	{
		return component.TweenRotateTo(endRotation, useShortestPath, false);
	}

	// Token: 0x06001F0B RID: 7947 RVA: 0x0008C5EC File Offset: 0x0008A7EC
	public static Tween<Vector3> TweenRotateTo(this Component component, Vector3 endRotation, bool useShortestPath, bool useLocalRotation)
	{
		return component.transform.TweenRotation(useShortestPath, useLocalRotation).SetEndValue(endRotation);
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x0008C604 File Offset: 0x0008A804
	public static Tween<Vector3> TweenRotation(this Component component)
	{
		return component.transform.TweenRotation(true, false);
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x0008C614 File Offset: 0x0008A814
	public static Tween<Vector3> TweenRotation(this Component component, bool useShortestPath)
	{
		return component.transform.TweenRotation(useShortestPath, false);
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x0008C624 File Offset: 0x0008A824
	public static Tween<Vector3> TweenRotation(this Component component, bool useShortestPath, bool useLocalRotation)
	{
		return component.transform.TweenRotation(useShortestPath, useLocalRotation);
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x0008C634 File Offset: 0x0008A834
	public static Tween<Vector3> TweenMoveFrom(this Component component, Vector3 startPosition)
	{
		return component.TweenMoveFrom(startPosition, false);
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x0008C640 File Offset: 0x0008A840
	public static Tween<Vector3> TweenMoveFrom(this Component component, Vector3 startPosition, bool useLocalPosition)
	{
		return component.TweenPosition(useLocalPosition).SetStartValue(startPosition);
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x0008C650 File Offset: 0x0008A850
	public static Tween<Vector3> TweenMoveTo(this Component component, Vector3 endPosition)
	{
		return component.TweenMoveTo(endPosition, false);
	}

	// Token: 0x06001F12 RID: 7954 RVA: 0x0008C65C File Offset: 0x0008A85C
	public static Tween<Vector3> TweenMoveTo(this Component component, Vector3 endPosition, bool useLocalPosition)
	{
		return component.TweenPosition(useLocalPosition).SetEndValue(endPosition);
	}

	// Token: 0x06001F13 RID: 7955 RVA: 0x0008C66C File Offset: 0x0008A86C
	public static Tween<Vector3> TweenPosition(this Component component)
	{
		return component.transform.TweenPosition(false);
	}

	// Token: 0x06001F14 RID: 7956 RVA: 0x0008C67C File Offset: 0x0008A87C
	public static Tween<Vector3> TweenPosition(this Component component, bool useLocalPosition)
	{
		return component.transform.TweenPosition(useLocalPosition);
	}
}
