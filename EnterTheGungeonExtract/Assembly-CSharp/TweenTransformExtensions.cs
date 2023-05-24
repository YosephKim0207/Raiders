using System;
using DaikonForge.Tween;
using DaikonForge.Tween.Interpolation;
using UnityEngine;

// Token: 0x02000513 RID: 1299
public static class TweenTransformExtensions
{
	// Token: 0x06001F15 RID: 7957 RVA: 0x0008C68C File Offset: 0x0008A88C
	public static TweenShake ShakePosition(this Transform transform)
	{
		return transform.ShakePosition(false);
	}

	// Token: 0x06001F16 RID: 7958 RVA: 0x0008C698 File Offset: 0x0008A898
	public static TweenShake ShakePosition(this Transform transform, bool localPosition)
	{
		return TweenShake.Obtain().SetStartValue((!localPosition) ? transform.position : transform.localPosition).SetShakeMagnitude(0.1f)
			.SetDuration(1f)
			.SetShakeSpeed(10f)
			.OnExecute(delegate(Vector3 result)
			{
				if (localPosition)
				{
					transform.localPosition = result;
				}
				else
				{
					transform.position = result;
				}
			});
	}

	// Token: 0x06001F17 RID: 7959 RVA: 0x0008C718 File Offset: 0x0008A918
	public static Tween<float> TweenPath(this Transform transform, IPathIterator path)
	{
		return transform.TweenPath(path, true);
	}

	// Token: 0x06001F18 RID: 7960 RVA: 0x0008C724 File Offset: 0x0008A924
	public static Tween<float> TweenPath(this Transform transform, IPathIterator path, bool orientToPath)
	{
		if (transform == null)
		{
			throw new ArgumentNullException("transform");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		Tween<float> tween = null;
		tween = Tween<float>.Obtain().SetStartValue(0f).SetEndValue(1f)
			.SetEasing(new TweenEasingCallback(TweenEasingFunctions.Linear))
			.OnExecute(delegate(float time)
			{
				transform.position = path.GetPosition(time);
				if (orientToPath)
				{
					Vector3 tangent = path.GetTangent(time);
					if (tween.PlayDirection == TweenDirection.Forward)
					{
						transform.forward = tangent;
					}
					else
					{
						transform.forward = -tangent;
					}
				}
			});
		return tween;
	}

	// Token: 0x06001F19 RID: 7961 RVA: 0x0008C7E0 File Offset: 0x0008A9E0
	public static Tween<Vector3> TweenScaleFrom(this Transform transform, Vector3 startScale)
	{
		return transform.TweenScale().SetStartValue(startScale);
	}

	// Token: 0x06001F1A RID: 7962 RVA: 0x0008C7F0 File Offset: 0x0008A9F0
	public static Tween<Vector3> TweenScaleTo(this Transform transform, Vector3 endScale)
	{
		return transform.TweenScale().SetEndValue(endScale);
	}

	// Token: 0x06001F1B RID: 7963 RVA: 0x0008C800 File Offset: 0x0008AA00
	public static Tween<Vector3> TweenScale(this Transform transform)
	{
		return Tween<Vector3>.Obtain().SetStartValue(transform.localScale).SetEndValue(transform.localScale)
			.SetDuration(1f)
			.OnExecute(delegate(Vector3 currentValue)
			{
				transform.localScale = currentValue;
			});
	}

	// Token: 0x06001F1C RID: 7964 RVA: 0x0008C85C File Offset: 0x0008AA5C
	public static Tween<Vector3> TweenRotateFrom(this Transform transform, Vector3 startRotation)
	{
		return transform.TweenRotateFrom(startRotation, true, false);
	}

	// Token: 0x06001F1D RID: 7965 RVA: 0x0008C868 File Offset: 0x0008AA68
	public static Tween<Vector3> TweenRotateFrom(this Transform transform, Vector3 startRotation, bool useShortestPath)
	{
		return transform.TweenRotateFrom(startRotation, useShortestPath, false);
	}

	// Token: 0x06001F1E RID: 7966 RVA: 0x0008C874 File Offset: 0x0008AA74
	public static Tween<Vector3> TweenRotateFrom(this Transform transform, Vector3 startRotation, bool useShortestPath, bool useLocalRotation)
	{
		return transform.TweenRotation(useShortestPath, useLocalRotation).SetStartValue(startRotation);
	}

	// Token: 0x06001F1F RID: 7967 RVA: 0x0008C884 File Offset: 0x0008AA84
	public static Tween<Vector3> TweenRotateTo(this Transform transform, Vector3 endRotation)
	{
		return transform.TweenRotateTo(endRotation, true, false);
	}

	// Token: 0x06001F20 RID: 7968 RVA: 0x0008C890 File Offset: 0x0008AA90
	public static Tween<Vector3> TweenRotateTo(this Transform transform, Vector3 endRotation, bool useShortestPath)
	{
		return transform.TweenRotateTo(endRotation, useShortestPath, false);
	}

	// Token: 0x06001F21 RID: 7969 RVA: 0x0008C89C File Offset: 0x0008AA9C
	public static Tween<Vector3> TweenRotateTo(this Transform transform, Vector3 endRotation, bool useShortestPath, bool useLocalRotation)
	{
		return transform.TweenRotation(useShortestPath, useLocalRotation).SetEndValue(endRotation);
	}

	// Token: 0x06001F22 RID: 7970 RVA: 0x0008C8AC File Offset: 0x0008AAAC
	public static Tween<Vector3> TweenRotation(this Transform transform)
	{
		return transform.TweenRotation(true, false);
	}

	// Token: 0x06001F23 RID: 7971 RVA: 0x0008C8B8 File Offset: 0x0008AAB8
	public static Tween<Vector3> TweenRotation(this Transform transform, bool useShortestPath)
	{
		return transform.TweenRotation(useShortestPath, false);
	}

	// Token: 0x06001F24 RID: 7972 RVA: 0x0008C8C4 File Offset: 0x0008AAC4
	public static Tween<Vector3> TweenRotation(this Transform transform, bool useShortestPath, bool useLocalRotation)
	{
		Interpolator<Vector3> interpolator = ((!useShortestPath) ? Vector3Interpolator.Default : EulerInterpolator.Default);
		Vector3 vector = ((!useLocalRotation) ? transform.eulerAngles : transform.localEulerAngles);
		TweenAssignmentCallback<Vector3> tweenAssignmentCallback;
		if (useLocalRotation)
		{
			tweenAssignmentCallback = delegate(Vector3 localValue)
			{
				transform.localEulerAngles = localValue;
			};
		}
		else
		{
			tweenAssignmentCallback = delegate(Vector3 globalValue)
			{
				transform.eulerAngles = globalValue;
			};
		}
		return Tween<Vector3>.Obtain().SetStartValue(vector).SetEndValue(vector)
			.SetDuration(1f)
			.SetInterpolator(interpolator)
			.OnExecute(tweenAssignmentCallback);
	}

	// Token: 0x06001F25 RID: 7973 RVA: 0x0008C968 File Offset: 0x0008AB68
	public static Tween<Vector3> TweenMoveFrom(this Transform transform, Vector3 startPosition)
	{
		return transform.TweenMoveFrom(startPosition, false);
	}

	// Token: 0x06001F26 RID: 7974 RVA: 0x0008C974 File Offset: 0x0008AB74
	public static Tween<Vector3> TweenMoveFrom(this Transform transform, Vector3 startPosition, bool useLocalPosition)
	{
		return transform.TweenPosition(useLocalPosition).SetStartValue(startPosition);
	}

	// Token: 0x06001F27 RID: 7975 RVA: 0x0008C984 File Offset: 0x0008AB84
	public static Tween<Vector3> TweenMoveTo(this Transform transform, Vector3 endPosition)
	{
		return transform.TweenMoveTo(endPosition, false);
	}

	// Token: 0x06001F28 RID: 7976 RVA: 0x0008C990 File Offset: 0x0008AB90
	public static Tween<Vector3> TweenMoveTo(this Transform transform, Vector3 endPosition, bool useLocalPosition)
	{
		return transform.TweenPosition(useLocalPosition).SetEndValue(endPosition);
	}

	// Token: 0x06001F29 RID: 7977 RVA: 0x0008C9A0 File Offset: 0x0008ABA0
	public static Tween<Vector3> TweenPosition(this Transform transform)
	{
		return transform.TweenPosition(false);
	}

	// Token: 0x06001F2A RID: 7978 RVA: 0x0008C9AC File Offset: 0x0008ABAC
	public static Tween<Vector3> TweenPosition(this Transform transform, bool useLocalPosition)
	{
		Vector3 vector = ((!useLocalPosition) ? transform.position : transform.localPosition);
		TweenAssignmentCallback<Vector3> tweenAssignmentCallback;
		if (useLocalPosition)
		{
			tweenAssignmentCallback = delegate(Vector3 localValue)
			{
				transform.localPosition = localValue;
			};
		}
		else
		{
			tweenAssignmentCallback = delegate(Vector3 globalValue)
			{
				transform.position = globalValue;
			};
		}
		return Tween<Vector3>.Obtain().SetStartValue(vector).SetEndValue(vector)
			.SetDuration(1f)
			.OnExecute(tweenAssignmentCallback);
	}
}
