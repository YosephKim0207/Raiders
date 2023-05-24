using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200048A RID: 1162
public class DEMO_PictureManipulation : MonoBehaviour
{
	// Token: 0x06001AAA RID: 6826 RVA: 0x0007CB18 File Offset: 0x0007AD18
	public void Start()
	{
		this.control = base.GetComponent<dfTextureSprite>();
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x0007CB28 File Offset: 0x0007AD28
	protected void OnMouseUp()
	{
		this.isMouseDown = false;
	}

	// Token: 0x06001AAC RID: 6828 RVA: 0x0007CB34 File Offset: 0x0007AD34
	protected void OnMouseDown()
	{
		this.isMouseDown = true;
		this.control.BringToFront();
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x0007CB48 File Offset: 0x0007AD48
	public IEnumerator OnFlickGesture(dfGestureBase gesture)
	{
		return this.handleMomentum(gesture);
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x0007CB54 File Offset: 0x0007AD54
	public IEnumerator OnDoubleTapGesture()
	{
		return this.resetZoom();
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x0007CB5C File Offset: 0x0007AD5C
	public void OnRotateGestureBegin(dfRotateGesture gesture)
	{
		this.rotateAroundPoint(gesture.StartPosition, gesture.AngleDelta * 0.5f);
	}

	// Token: 0x06001AB0 RID: 6832 RVA: 0x0007CB78 File Offset: 0x0007AD78
	public void OnRotateGestureUpdate(dfRotateGesture gesture)
	{
		this.rotateAroundPoint(gesture.CurrentPosition, gesture.AngleDelta * 0.5f);
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x0007CB94 File Offset: 0x0007AD94
	public void OnResizeGestureUpdate(dfResizeGesture gesture)
	{
		this.zoomToPoint(gesture.StartPosition, gesture.SizeDelta * 1f);
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x0007CBB0 File Offset: 0x0007ADB0
	public void OnPanGestureStart(dfPanGesture gesture)
	{
		this.control.BringToFront();
		this.control.RelativePosition += gesture.Delta.Scale(1f, -1f);
	}

	// Token: 0x06001AB3 RID: 6835 RVA: 0x0007CBF0 File Offset: 0x0007ADF0
	public void OnPanGestureMove(dfPanGesture gesture)
	{
		this.control.RelativePosition += gesture.Delta.Scale(1f, -1f);
	}

	// Token: 0x06001AB4 RID: 6836 RVA: 0x0007CC24 File Offset: 0x0007AE24
	public void OnMouseWheel(dfControl sender, dfMouseEventArgs args)
	{
		this.zoomToPoint(args.Position, Mathf.Sign(args.WheelDelta) * 75f);
	}

	// Token: 0x06001AB5 RID: 6837 RVA: 0x0007CC44 File Offset: 0x0007AE44
	private IEnumerator resetZoom()
	{
		Vector2 controlSize = this.control.Size;
		Vector2 screenSize = this.control.GetManager().GetScreenSize();
		dfAnimatedVector2 animatedSize = new dfAnimatedVector2(this.control.Size, controlSize, 0.2f);
		if (controlSize.x >= screenSize.x - 10f || controlSize.y >= screenSize.y - 10f)
		{
			animatedSize.EndValue = DEMO_PictureManipulation.fitImage(screenSize.x * 0.75f, screenSize.y * 0.75f, this.control.Width, this.control.Height);
		}
		else
		{
			animatedSize.EndValue = DEMO_PictureManipulation.fitImage(screenSize.x, screenSize.y, this.control.Width, this.control.Height);
		}
		Vector3 endPosition = new Vector3(screenSize.x - animatedSize.EndValue.x, screenSize.y - animatedSize.EndValue.y, 0f) * 0.5f;
		dfAnimatedVector3 animatedPosition = new dfAnimatedVector3(this.control.RelativePosition, endPosition, animatedSize.Length);
		dfAnimatedQuaternion animatedRotation = new dfAnimatedQuaternion(this.control.transform.rotation, Quaternion.identity, animatedSize.Length);
		while (!animatedSize.IsDone)
		{
			this.control.Size = animatedSize;
			this.control.RelativePosition = animatedPosition;
			this.control.transform.rotation = animatedRotation;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001AB6 RID: 6838 RVA: 0x0007CC60 File Offset: 0x0007AE60
	private IEnumerator handleMomentum(dfGestureBase gesture)
	{
		this.isMouseDown = false;
		Vector3 direction = (gesture.CurrentPosition - gesture.StartPosition) * this.control.PixelsToUnits();
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(this.control.GetCamera());
		float startTime = Time.realtimeSinceStartup;
		while (!this.isMouseDown)
		{
			float timeNow = Time.realtimeSinceStartup;
			float elapsed = timeNow - startTime;
			if (elapsed > 1f)
			{
				break;
			}
			this.control.transform.position += direction * BraveTime.DeltaTime * 10f * (1f - elapsed);
			yield return null;
		}
		if (!GeometryUtility.TestPlanesAABB(planes, this.control.GetComponent<Collider>().bounds))
		{
			this.control.enabled = false;
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
		yield break;
	}

	// Token: 0x06001AB7 RID: 6839 RVA: 0x0007CC84 File Offset: 0x0007AE84
	private void rotateAroundPoint(Vector2 point, float delta)
	{
		Transform transform = this.control.transform;
		Vector3[] corners = this.control.GetCorners();
		Plane plane = new Plane(corners[0], corners[1], corners[2]);
		Ray ray = this.control.GetCamera().ScreenPointToRay(point);
		float num = 0f;
		plane.Raycast(ray, out num);
		Vector3 point2 = ray.GetPoint(num);
		Vector3 vector = new Vector3(0f, 0f, delta);
		Vector3 vector2 = transform.eulerAngles + vector;
		Vector3 vector3 = this.rotatePointAroundPivot(transform.position, point2, vector);
		transform.position = vector3;
		transform.eulerAngles = vector2;
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x0007CD4C File Offset: 0x0007AF4C
	private Vector3 rotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		Vector3 vector = Quaternion.Euler(angles) * (point - pivot);
		return vector + pivot;
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x0007CD74 File Offset: 0x0007AF74
	private void zoomToPoint(Vector2 point, float delta)
	{
		Transform transform = this.control.transform;
		Vector3[] corners = this.control.GetCorners();
		float num = this.control.PixelsToUnits();
		Plane plane = new Plane(corners[0], corners[1], corners[2]);
		Ray ray = this.control.GetCamera().ScreenPointToRay(point);
		float num2 = 0f;
		plane.Raycast(ray, out num2);
		Vector3 point2 = ray.GetPoint(num2);
		Vector2 vector = this.control.Size * num;
		Vector3 vector2 = transform.position - point2;
		Vector3 vector3 = new Vector3(vector2.x / vector.x, vector2.y / vector.y);
		float num3 = this.control.Height / this.control.Width;
		float num4 = this.control.Width + delta;
		float num5 = num4 * num3;
		if (num4 < 256f || num5 < 256f)
		{
			return;
		}
		this.control.Size = new Vector2(num4, num5);
		Vector3 vector4 = new Vector3(this.control.Width * vector3.x * num, this.control.Height * vector3.y * num, vector2.z);
		transform.position += vector4 - vector2;
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x0007CEFC File Offset: 0x0007B0FC
	private static Vector2 fitImage(float maxWidth, float maxHeight, float imageWidth, float imageHeight)
	{
		float num = maxWidth / imageWidth;
		float num2 = maxHeight / imageHeight;
		float num3 = Mathf.Min(num, num2);
		return new Vector2(Mathf.Floor(imageWidth * num3), Mathf.Ceil(imageHeight * num3));
	}

	// Token: 0x04001504 RID: 5380
	private dfTextureSprite control;

	// Token: 0x04001505 RID: 5381
	private bool isMouseDown;
}
