using System;
using UnityEngine;

// Token: 0x02000463 RID: 1123
[AddComponentMenu("Daikon Forge/Examples/General/Follow Object")]
public class dfFollowObject : MonoBehaviour
{
	// Token: 0x060019F9 RID: 6649 RVA: 0x000792A0 File Offset: 0x000774A0
	private void OnEnable()
	{
		if (this.mainCamera == null)
		{
			this.mainCamera = Camera.main;
			if (this.mainCamera == null)
			{
				Debug.LogError("dfFollowObject component is unable to determine which camera is the MainCamera", base.gameObject);
				base.enabled = false;
				return;
			}
		}
		this.myControl = base.GetComponent<dfControl>();
		if (this.myControl == null)
		{
			Debug.LogError("No dfControl component on this GameObject: " + base.gameObject.name, base.gameObject);
			base.enabled = false;
		}
		if (this.myControl == null || this.attach == null)
		{
			Debug.LogWarning("Configuration incomplete: " + base.name);
			base.enabled = false;
			return;
		}
		this.followTransform = this.attach.transform;
		this.manager = this.myControl.GetManager();
		dfFollowObjectSorter.Register(this);
		CameraController mainCameraController = GameManager.Instance.MainCameraController;
		mainCameraController.OnFinishedFrame = (Action)Delegate.Remove(mainCameraController.OnFinishedFrame, new Action(this.OnMainCameraFinishedFrame));
		CameraController mainCameraController2 = GameManager.Instance.MainCameraController;
		mainCameraController2.OnFinishedFrame = (Action)Delegate.Combine(mainCameraController2.OnFinishedFrame, new Action(this.OnMainCameraFinishedFrame));
	}

	// Token: 0x060019FA RID: 6650 RVA: 0x000793F4 File Offset: 0x000775F4
	private void OnMainCameraFinishedFrame()
	{
	}

	// Token: 0x060019FB RID: 6651 RVA: 0x000793F8 File Offset: 0x000775F8
	private void OnDisable()
	{
		if (GameManager.HasInstance)
		{
			dfFollowObjectSorter.Unregister(this);
			CameraController mainCameraController = GameManager.Instance.MainCameraController;
			mainCameraController.OnFinishedFrame = (Action)Delegate.Remove(mainCameraController.OnFinishedFrame, new Action(this.OnMainCameraFinishedFrame));
		}
	}

	// Token: 0x060019FC RID: 6652 RVA: 0x00079438 File Offset: 0x00077638
	private void OnDestroy()
	{
		if (GameManager.HasInstance && GameManager.Instance.MainCameraController)
		{
			CameraController mainCameraController = GameManager.Instance.MainCameraController;
			mainCameraController.OnFinishedFrame = (Action)Delegate.Remove(mainCameraController.OnFinishedFrame, new Action(this.OnMainCameraFinishedFrame));
		}
	}

	// Token: 0x060019FD RID: 6653 RVA: 0x00079490 File Offset: 0x00077690
	public void ForceUpdate()
	{
		this.OnEnable();
		this.Update();
	}

	// Token: 0x060019FE RID: 6654 RVA: 0x000794A0 File Offset: 0x000776A0
	public static Vector3 ConvertWorldSpaces(Vector3 inPoint, Camera inCamera, Camera outCamera)
	{
		Vector3 vector = inCamera.WorldToViewportPoint(inPoint);
		return outCamera.ViewportToWorldPoint(vector);
	}

	// Token: 0x060019FF RID: 6655 RVA: 0x000794BC File Offset: 0x000776BC
	private void Update()
	{
		if (!this.followTransform)
		{
			base.enabled = false;
			return;
		}
		base.transform.position = dfFollowObject.ConvertWorldSpaces(this.followTransform.position + this.offset, this.mainCamera, this.manager.RenderCamera).WithZ(0f);
		base.transform.position = base.transform.position.QuantizeFloor(this.myControl.PixelsToUnits() / (Pixelator.Instance.ScaleTileScale / Pixelator.Instance.CurrentTileScale));
	}

	// Token: 0x06001A00 RID: 6656 RVA: 0x00079560 File Offset: 0x00077760
	private Vector2 getAnchoredControlPosition()
	{
		float height = this.myControl.Height;
		float num = this.myControl.Width / 2f;
		float width = this.myControl.Width;
		float num2 = this.myControl.Height / 2f;
		Vector2 vector = default(Vector2);
		switch (this.anchor)
		{
		case dfPivotPoint.TopLeft:
			vector.x = width;
			vector.y = height;
			break;
		case dfPivotPoint.TopCenter:
			vector.x = num;
			vector.y = height;
			break;
		case dfPivotPoint.TopRight:
			vector.x = 0f;
			vector.y = height;
			break;
		case dfPivotPoint.MiddleLeft:
			vector.x = width;
			vector.y = num2;
			break;
		case dfPivotPoint.MiddleCenter:
			vector.x = num;
			vector.y = num2;
			break;
		case dfPivotPoint.MiddleRight:
			vector.x = 0f;
			vector.y = num2;
			break;
		case dfPivotPoint.BottomLeft:
			vector.x = width;
			vector.y = 0f;
			break;
		case dfPivotPoint.BottomCenter:
			vector.x = num;
			vector.y = 0f;
			break;
		case dfPivotPoint.BottomRight:
			vector.x = 0f;
			vector.y = 0f;
			break;
		}
		return vector;
	}

	// Token: 0x0400145B RID: 5211
	public Camera mainCamera;

	// Token: 0x0400145C RID: 5212
	public GameObject attach;

	// Token: 0x0400145D RID: 5213
	public dfPivotPoint anchor = dfPivotPoint.MiddleCenter;

	// Token: 0x0400145E RID: 5214
	public Vector3 offset;

	// Token: 0x0400145F RID: 5215
	public float hideDistance = 20f;

	// Token: 0x04001460 RID: 5216
	public float fadeDistance = 15f;

	// Token: 0x04001461 RID: 5217
	public bool constantScale;

	// Token: 0x04001462 RID: 5218
	public bool stickToScreenEdge;

	// Token: 0x04001463 RID: 5219
	[HideInInspector]
	public bool overrideVisibility = true;

	// Token: 0x04001464 RID: 5220
	private Transform followTransform;

	// Token: 0x04001465 RID: 5221
	private dfControl myControl;

	// Token: 0x04001466 RID: 5222
	private dfGUIManager manager;
}
