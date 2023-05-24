using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000E3C RID: 3644
public class CameraController : BraveBehaviour
{
	// Token: 0x06004D1B RID: 19739 RVA: 0x001A6470 File Offset: 0x001A4670
	public void ClearPlayerCache()
	{
		this.m_player = null;
	}

	// Token: 0x17000AD3 RID: 2771
	// (get) Token: 0x06004D1C RID: 19740 RVA: 0x001A647C File Offset: 0x001A467C
	public float CurrentZOffset
	{
		get
		{
			if (this.IsPerspectiveMode)
			{
				return base.transform.position.y - 40f;
			}
			return this.z_Offset;
		}
	}

	// Token: 0x17000AD4 RID: 2772
	// (get) Token: 0x06004D1D RID: 19741 RVA: 0x001A64B4 File Offset: 0x001A46B4
	public bool IsCurrentlyZoomIntermediate
	{
		get
		{
			return this.CurrentZoomScale != this.OverrideZoomScale;
		}
	}

	// Token: 0x06004D1E RID: 19742 RVA: 0x001A64C8 File Offset: 0x001A46C8
	public void SetZoomScaleImmediate(float zoomScale)
	{
		this.OverrideZoomScale = zoomScale;
		this.CurrentZoomScale = zoomScale;
		if (Pixelator.HasInstance)
		{
			Pixelator.Instance.NUM_MACRO_PIXELS_HORIZONTAL = (int)((float)BraveCameraUtility.H_PIXELS / this.CurrentZoomScale).Quantize(2f);
			Pixelator.Instance.NUM_MACRO_PIXELS_VERTICAL = (int)((float)BraveCameraUtility.V_PIXELS / this.CurrentZoomScale).Quantize(2f);
		}
	}

	// Token: 0x17000AD5 RID: 2773
	// (get) Token: 0x06004D1F RID: 19743 RVA: 0x001A6534 File Offset: 0x001A4734
	public Vector3 ScreenShakeVector
	{
		get
		{
			return this.screenShakeAmount;
		}
	}

	// Token: 0x17000AD6 RID: 2774
	// (get) Token: 0x06004D20 RID: 19744 RVA: 0x001A653C File Offset: 0x001A473C
	public float ScreenShakeVibration
	{
		get
		{
			return this.m_screenShakeVibration;
		}
	}

	// Token: 0x06004D21 RID: 19745 RVA: 0x001A6544 File Offset: 0x001A4744
	public void UpdateScreenShakeVibration(float newVibration)
	{
		if (this.m_screenShakeVibrationDirty)
		{
			this.m_screenShakeVibration = 0f;
			this.m_screenShakeVibrationDirty = false;
		}
		this.m_screenShakeVibration = Mathf.Max(this.m_screenShakeVibration, newVibration);
	}

	// Token: 0x06004D22 RID: 19746 RVA: 0x001A6578 File Offset: 0x001A4778
	public void MarkScreenShakeVibrationDirty()
	{
		this.m_screenShakeVibrationDirty = true;
	}

	// Token: 0x17000AD7 RID: 2775
	// (get) Token: 0x06004D23 RID: 19747 RVA: 0x001A6584 File Offset: 0x001A4784
	public bool ManualControl
	{
		get
		{
			return this.m_manualControl;
		}
	}

	// Token: 0x17000AD8 RID: 2776
	// (get) Token: 0x06004D24 RID: 19748 RVA: 0x001A658C File Offset: 0x001A478C
	// (set) Token: 0x06004D25 RID: 19749 RVA: 0x001A6594 File Offset: 0x001A4794
	public bool PreventAimLook { get; set; }

	// Token: 0x17000AD9 RID: 2777
	// (get) Token: 0x06004D26 RID: 19750 RVA: 0x001A65A0 File Offset: 0x001A47A0
	public Camera Camera
	{
		get
		{
			if (!this.m_camera && this)
			{
				this.m_camera = base.GetComponent<Camera>();
			}
			return this.m_camera;
		}
	}

	// Token: 0x17000ADA RID: 2778
	// (get) Token: 0x06004D27 RID: 19751 RVA: 0x001A65D0 File Offset: 0x001A47D0
	private float m_deltaTime
	{
		get
		{
			return GameManager.INVARIANT_DELTA_TIME;
		}
	}

	// Token: 0x17000ADB RID: 2779
	// (get) Token: 0x06004D28 RID: 19752 RVA: 0x001A65D8 File Offset: 0x001A47D8
	// (set) Token: 0x06004D29 RID: 19753 RVA: 0x001A65E0 File Offset: 0x001A47E0
	public bool LockX { get; set; }

	// Token: 0x17000ADC RID: 2780
	// (get) Token: 0x06004D2A RID: 19754 RVA: 0x001A65EC File Offset: 0x001A47EC
	// (set) Token: 0x06004D2B RID: 19755 RVA: 0x001A65F4 File Offset: 0x001A47F4
	public bool LockY { get; set; }

	// Token: 0x17000ADD RID: 2781
	// (get) Token: 0x06004D2C RID: 19756 RVA: 0x001A6600 File Offset: 0x001A4800
	// (set) Token: 0x06004D2D RID: 19757 RVA: 0x001A6608 File Offset: 0x001A4808
	public bool LockToRoom { get; set; }

	// Token: 0x17000ADE RID: 2782
	// (get) Token: 0x06004D2E RID: 19758 RVA: 0x001A6614 File Offset: 0x001A4814
	// (set) Token: 0x06004D2F RID: 19759 RVA: 0x001A661C File Offset: 0x001A481C
	public bool PreventFuseBombAimOffset { get; set; }

	// Token: 0x17000ADF RID: 2783
	// (get) Token: 0x06004D30 RID: 19760 RVA: 0x001A6628 File Offset: 0x001A4828
	public static Vector3 PLATFORM_CAMERA_OFFSET
	{
		get
		{
			if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11)
			{
				return Vector3.zero;
			}
			return new Vector3(0.03125f, 0.03125f, 0f);
		}
	}

	// Token: 0x06004D31 RID: 19761 RVA: 0x001A6650 File Offset: 0x001A4850
	private void Awake()
	{
		BraveTime.CacheDeltaTimeForFrame();
	}

	// Token: 0x06004D32 RID: 19762 RVA: 0x001A6658 File Offset: 0x001A4858
	private void Start()
	{
		this.m_camera = base.GetComponent<Camera>();
		this.FINAL_CAMERA_POSITION_OFFSET = CameraController.PLATFORM_CAMERA_OFFSET;
		if (this.m_player == null)
		{
			this.m_player = GameManager.Instance.PrimaryPlayer;
		}
		this.screenShakeAmount = new Vector3(0f, 0f, 0f);
	}

	// Token: 0x06004D33 RID: 19763 RVA: 0x001A66B8 File Offset: 0x001A48B8
	public void AddFocusPoint(GameObject go)
	{
		if (!this.m_focusObjects.Contains(go))
		{
			this.m_focusObjects.Add(go);
		}
	}

	// Token: 0x06004D34 RID: 19764 RVA: 0x001A66D8 File Offset: 0x001A48D8
	public void AddFocusPoint(SpeculativeRigidbody specRigidbody)
	{
		if (!this.m_focusObjects.Contains(specRigidbody))
		{
			this.m_focusObjects.Add(specRigidbody);
		}
	}

	// Token: 0x06004D35 RID: 19765 RVA: 0x001A66F8 File Offset: 0x001A48F8
	public void RemoveFocusPoint(GameObject go)
	{
		this.m_focusObjects.Remove(go);
	}

	// Token: 0x06004D36 RID: 19766 RVA: 0x001A6708 File Offset: 0x001A4908
	public void RemoveFocusPoint(SpeculativeRigidbody specRigidbody)
	{
		this.m_focusObjects.Remove(specRigidbody);
	}

	// Token: 0x17000AE0 RID: 2784
	// (get) Token: 0x06004D37 RID: 19767 RVA: 0x001A6718 File Offset: 0x001A4918
	public static bool SuperSmoothCamera
	{
		get
		{
			return GameManager.Options.SuperSmoothCamera;
		}
	}

	// Token: 0x06004D38 RID: 19768 RVA: 0x001A6724 File Offset: 0x001A4924
	private Vector2 GetPlayerPosition(PlayerController targetPlayer)
	{
		if (targetPlayer.IsPrimaryPlayer)
		{
			return (!this.UseOverridePlayerOnePosition) ? ((!CameraController.SuperSmoothCamera) ? targetPlayer.CenterPosition : targetPlayer.SmoothedCameraCenter) : this.OverridePlayerOnePosition;
		}
		return (!this.UseOverridePlayerTwoPosition) ? ((!CameraController.SuperSmoothCamera) ? targetPlayer.CenterPosition : targetPlayer.SmoothedCameraCenter) : this.OverridePlayerTwoPosition;
	}

	// Token: 0x17000AE1 RID: 2785
	// (get) Token: 0x06004D39 RID: 19769 RVA: 0x001A67A0 File Offset: 0x001A49A0
	private bool UseMouseAim
	{
		get
		{
			return Application.platform != RuntimePlatform.PS4 && Application.platform != RuntimePlatform.XboxOne && GameManager.Options.mouseAimLook && BraveInput.GetInstanceForPlayer(0).IsKeyboardAndMouse(false) && !this.LockToRoom;
		}
	}

	// Token: 0x06004D3A RID: 19770 RVA: 0x001A67F8 File Offset: 0x001A49F8
	public Vector2 GetCoreCurrentBasePosition()
	{
		if (this.m_player == null)
		{
			this.m_player = GameManager.Instance.PrimaryPlayer;
		}
		Vector2 zero = Vector2.zero;
		int num = 0;
		if (GameManager.Instance.AllPlayers.Length < 2)
		{
			if (this.m_player == null)
			{
				return Vector2.zero;
			}
			BraveMathCollege.WeightedAverage(this.GetPlayerPosition(this.m_player), ref zero, ref num);
		}
		else
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i].gameObject.activeSelf)
				{
					if (!GameManager.Instance.AllPlayers[i].IgnoredByCamera)
					{
						if (!GameManager.Instance.AllPlayers[i].IsGhost)
						{
							BraveMathCollege.WeightedAverage(this.GetPlayerPosition(GameManager.Instance.AllPlayers[i]), ref zero, ref num);
						}
					}
				}
			}
			if (num > 1)
			{
				num = 1;
			}
		}
		for (int j = 0; j < this.m_focusObjects.Count; j++)
		{
			if (this.m_focusObjects[j] is GameObject)
			{
				BraveMathCollege.WeightedAverage((this.m_focusObjects[j] as GameObject).transform.position, ref zero, ref num);
			}
			else if (this.m_focusObjects[j] is SpeculativeRigidbody)
			{
				BraveMathCollege.WeightedAverage((this.m_focusObjects[j] as SpeculativeRigidbody).GetUnitCenter(ColliderType.HitBox), ref zero, ref num);
			}
		}
		return zero;
	}

	// Token: 0x06004D3B RID: 19771 RVA: 0x001A69A4 File Offset: 0x001A4BA4
	public Vector2 GetIdealCameraPosition()
	{
		Vector2 coreCurrentBasePosition = this.GetCoreCurrentBasePosition();
		return coreCurrentBasePosition + this.GetCoreOffset(coreCurrentBasePosition, false, true);
	}

	// Token: 0x06004D3C RID: 19772 RVA: 0x001A69CC File Offset: 0x001A4BCC
	private Vector2 GetCoreOffset(Vector2 currentBasePosition, bool isUpdate, bool allowAimOffset)
	{
		if (this.UseMouseAim)
		{
			Vector2 vector = Vector2.zero;
			if (allowAimOffset && GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER)
			{
				Vector2 vector2 = this.m_camera.ScreenToWorldPoint(Input.mousePosition).XY();
				Vector2 vector3 = vector2 - currentBasePosition;
				vector3 = new Vector2(vector3.x / this.m_camera.aspect, vector3.y);
				vector3.x = Mathf.Clamp(vector3.x, this.m_camera.orthographicSize * -1.5f, this.m_camera.orthographicSize * 1.5f);
				vector3.y = Mathf.Clamp(vector3.y, this.m_camera.orthographicSize * -1.5f, this.m_camera.orthographicSize * 1.5f);
				float num = Mathf.Lerp(0f, 0.33333f, Mathf.Pow(Mathf.Clamp01((vector3.magnitude - 1f) / (this.m_camera.orthographicSize - 1f)), 0.5f));
				vector = vector3 * num;
			}
			if (vector.magnitude < 0.015625f)
			{
				vector = Vector2.zero;
			}
			return vector;
		}
		Vector2 vector4 = Vector2.zero;
		GungeonActions activeActions = BraveInput.GetInstanceForPlayer(0).ActiveActions;
		if (Minimap.Instance && !Minimap.Instance.IsFullscreen && activeActions != null)
		{
			Vector2 vector5 = activeActions.Aim.Vector;
			vector4 = ((vector5.magnitude <= 0.1f) ? Vector2.zero : (vector5.normalized * this.controllerCamera.ModifiedAimContribution));
			if (vector4.y > 0f && this.PreventFuseBombAimOffset)
			{
				vector4.y = 0f;
			}
		}
		float num2;
		if (vector4 == Vector2.zero)
		{
			num2 = this.controllerCamera.AimContributionSlowTime;
		}
		else if (this.m_lastAimOffset != Vector2.zero && Mathf.Abs(BraveMathCollege.ClampAngle180(vector4.ToAngle() - this.m_lastAimOffset.ToAngle())) > 135f)
		{
			num2 = this.controllerCamera.AimContributionFastTime;
		}
		else
		{
			num2 = this.controllerCamera.AimContributionTime;
		}
		Vector2 vector6 = Vector2.SmoothDamp(this.m_lastAimOffset, vector4, ref this.m_aimOffsetVelocity, num2, 20f, this.m_deltaTime);
		if (isUpdate)
		{
			this.m_lastAimOffset = vector6;
		}
		Vector2 vector7 = currentBasePosition;
		if (this.controllerCamera.state == CameraController.ControllerCameraState.RoomLock)
		{
			Rect cameraBoundingRect = GameManager.Instance.PrimaryPlayer.CurrentRoom.cameraBoundingRect;
			cameraBoundingRect.yMin += 1f;
			cameraBoundingRect.height += 2f;
			vector7 = this.GetBoundedCameraPositionInRect(cameraBoundingRect, currentBasePosition, ref vector6);
		}
		if (this.controllerCamera.UseAimContribution && this.OverrideZoomScale == 1f && GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER)
		{
			vector7 += vector6;
		}
		Vector2 vector8 = vector7 - currentBasePosition;
		if (vector8.magnitude < 0.015625f)
		{
			vector8 = Vector2.zero;
		}
		return vector8;
	}

	// Token: 0x06004D3D RID: 19773 RVA: 0x001A6D14 File Offset: 0x001A4F14
	private void Update()
	{
		tk2dSpriteAnimator.CameraPositionThisFrame = base.transform.position.XY();
		if (this.m_screenShakeVibrationDirty)
		{
			this.m_screenShakeVibration = 0f;
			this.m_screenShakeVibrationDirty = false;
		}
	}

	// Token: 0x06004D3E RID: 19774 RVA: 0x001A6D48 File Offset: 0x001A4F48
	private void AdjustRecoverySpeedFoyer()
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER && this.OverrideRecoverySpeed > 0f)
		{
			if (this.OverrideRecoverySpeed >= 20f)
			{
				this.OverrideRecoverySpeed = -1f;
			}
			else
			{
				this.OverrideRecoverySpeed += BraveTime.DeltaTime * 3f;
			}
		}
	}

	// Token: 0x06004D3F RID: 19775 RVA: 0x001A6DB0 File Offset: 0x001A4FB0
	private void LateUpdate()
	{
		this.controllerCamera.forceTimer = Mathf.Max(0f, this.controllerCamera.forceTimer - BraveTime.DeltaTime);
		this.m_terminateNextContinuousScreenShake = false;
		for (int i = 0; i < this.activeContinuousShakes.Count; i++)
		{
			this.activeContinuousShakes[i].MoveNext();
		}
		if (GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		if (Pixelator.Instance && (this.CurrentZoomScale != this.OverrideZoomScale || Pixelator.Instance.NUM_MACRO_PIXELS_HORIZONTAL != (int)((float)BraveCameraUtility.H_PIXELS / this.CurrentZoomScale).Quantize(2f)))
		{
			this.CurrentZoomScale = Mathf.MoveTowards(this.CurrentZoomScale, this.OverrideZoomScale, 0.5f * GameManager.INVARIANT_DELTA_TIME);
			float aspect = this.m_camera.aspect;
			int h_PIXELS = BraveCameraUtility.H_PIXELS;
			int v_PIXELS = BraveCameraUtility.V_PIXELS;
			Pixelator.Instance.NUM_MACRO_PIXELS_HORIZONTAL = (int)((float)h_PIXELS / this.CurrentZoomScale).Quantize(2f);
			Pixelator.Instance.NUM_MACRO_PIXELS_VERTICAL = (int)((float)v_PIXELS / this.CurrentZoomScale).Quantize(2f);
		}
		if (!this.m_manualControl)
		{
			Vector2 vector = ((!this.m_isTrackingPlayer) ? this.previousBasePosition : this.GetCoreCurrentBasePosition());
			if (!this.UseMouseAim && this.controllerCamera.forceTimer <= 0f)
			{
				bool flag = GameManager.Instance.PrimaryPlayer != null && GameManager.Instance.PrimaryPlayer.CurrentRoom != null && GameManager.Instance.PrimaryPlayer.CurrentRoom.IsSealed;
				if ((this.controllerCamera.state == CameraController.ControllerCameraState.FollowPlayer || this.controllerCamera.state == CameraController.ControllerCameraState.Off) && flag)
				{
					this.controllerCamera.state = CameraController.ControllerCameraState.RoomLock;
					this.controllerCamera.isTransitioning = true;
					this.controllerCamera.transitionDuration = this.controllerCamera.ToRoomLockTime;
					this.controllerCamera.transitionStart = base.transform.position;
					this.controllerCamera.transitionTimer = 0f;
				}
				else if ((this.controllerCamera.state == CameraController.ControllerCameraState.RoomLock || this.controllerCamera.state == CameraController.ControllerCameraState.Off) && !flag)
				{
					this.controllerCamera.state = CameraController.ControllerCameraState.FollowPlayer;
					this.controllerCamera.isTransitioning = true;
					this.controllerCamera.transitionDuration = this.controllerCamera.EndRoomLockTime;
					this.controllerCamera.transitionStart = base.transform.position;
					this.controllerCamera.transitionTimer = 0f;
				}
			}
			Vector2 coreOffset = this.GetCoreOffset(vector, true, this.m_isTrackingPlayer);
			vector += coreOffset;
			this.previousBasePosition = vector;
			Vector2 vector2 = vector;
			if (!this.UseMouseAim && this.controllerCamera.isTransitioning)
			{
				this.controllerCamera.transitionTimer += this.m_deltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Min(this.controllerCamera.transitionTimer / this.controllerCamera.transitionDuration, 1f));
				vector2 = Vector2.Lerp(this.controllerCamera.transitionStart, vector, num);
				if (this.controllerCamera.transitionTimer > this.controllerCamera.transitionDuration)
				{
					this.controllerCamera.isTransitioning = false;
				}
			}
			else if (this.m_isRecoveringFromManualControl)
			{
				Vector2 vector3 = base.transform.PositionVector2() - this.FINAL_CAMERA_POSITION_OFFSET.XY();
				float num2 = Vector2.Distance(vector2, vector3);
				this.AdjustRecoverySpeedFoyer();
				float num3 = ((this.OverrideRecoverySpeed <= 0f) ? 20f : this.OverrideRecoverySpeed);
				if (num2 > num3 * this.m_deltaTime)
				{
					vector2 = vector3 + (vector2 - vector3).normalized * num3 * this.m_deltaTime;
				}
				else
				{
					this.m_isRecoveringFromManualControl = false;
					this.OverrideRecoverySpeed = -1f;
				}
			}
			if (this.UseMouseAim)
			{
				this.controllerCamera.state = CameraController.ControllerCameraState.Off;
				this.controllerCamera.isTransitioning = false;
			}
			Vector3 vector4 = this.screenShakeAmount * ScreenShakeSettings.GLOBAL_SHAKE_MULTIPLIER * GameManager.Options.ScreenShakeMultiplier;
			Vector3 vector5 = ((vector4.magnitude <= 5f) ? vector4 : (vector4.normalized * 5f));
			if (float.IsNaN(vector5.x) || float.IsInfinity(vector5.x))
			{
				vector5.x = 0f;
			}
			if (float.IsNaN(vector5.y) || float.IsInfinity(vector5.y))
			{
				vector5.y = 0f;
			}
			if (float.IsNaN(vector5.z) || float.IsInfinity(vector5.z))
			{
				vector5.z = 0f;
			}
			if (GameManager.Instance.IsPaused)
			{
				vector5 = Vector3.zero;
			}
			Vector3 vector6 = vector2.ToVector3ZUp(this.CurrentZOffset) + vector5;
			vector6 += this.FINAL_CAMERA_POSITION_OFFSET;
			if (this.LockX)
			{
				vector6.x = base.transform.position.x;
			}
			if (this.LockY)
			{
				vector6.y = base.transform.position.y;
			}
			base.transform.position = vector6;
		}
		else
		{
			if (this.controllerCamera != null)
			{
				this.controllerCamera.isTransitioning = false;
				this.controllerCamera.transitionStart = base.transform.position;
			}
			this.GetCoreOffset(this.GetCoreCurrentBasePosition(), true, true);
			Vector2 vector7 = this.OverridePosition.XY();
			if (this.m_isLerpingToManualControl)
			{
				Vector2 vector8 = base.transform.PositionVector2() - this.FINAL_CAMERA_POSITION_OFFSET.XY();
				float num4 = Vector2.Distance(vector7, vector8);
				this.AdjustRecoverySpeedFoyer();
				float num5 = ((this.OverrideRecoverySpeed <= 0f) ? 20f : this.OverrideRecoverySpeed);
				if (num4 > num5 * this.m_deltaTime)
				{
					vector7 = vector8 + (vector7 - vector8).normalized * num5 * this.m_deltaTime;
				}
				else
				{
					this.m_isLerpingToManualControl = false;
				}
			}
			Vector3 vector9 = ((this.screenShakeAmount.magnitude <= 5f) ? this.screenShakeAmount : (this.screenShakeAmount.normalized * 5f));
			float screenShakeMultiplier = GameManager.Options.ScreenShakeMultiplier;
			Vector3 vector10 = vector7.ToVector3ZUp(this.CurrentZOffset) + vector9 * ScreenShakeSettings.GLOBAL_SHAKE_MULTIPLIER * screenShakeMultiplier + this.FINAL_CAMERA_POSITION_OFFSET;
			if (this.LockX)
			{
				vector10.x = base.transform.position.x;
			}
			if (this.LockY)
			{
				vector10.y = base.transform.position.y;
			}
			if (float.IsNaN(vector10.x) || float.IsNaN(vector10.y))
			{
				Debug.LogWarning("THERE'S NaNS IN THEM THAR HILLS");
				vector10 = this.GetCoreCurrentBasePosition();
			}
			base.transform.position = vector10;
		}
		if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.VERY_LOW || GameManager.Options.CurrentPreferredScalingMode == GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST)
		{
			base.transform.position = base.transform.position.Quantize(0.0625f) + CameraController.PLATFORM_CAMERA_OFFSET;
		}
		Ray ray = Camera.main.ViewportPointToRay(new Vector2(0f, 0f));
		Plane plane = new Plane(Vector3.back, Vector3.zero);
		float num6;
		plane.Raycast(ray, out num6);
		this.m_cachedMinPos = ray.GetPoint(num6);
		ray = Camera.main.ViewportPointToRay(new Vector2(1f, 1f));
		plane.Raycast(ray, out num6);
		this.m_cachedMaxPos = ray.GetPoint(num6);
		this.m_cachedSize = this.m_cachedMaxPos - this.m_cachedMinPos;
		CameraController.m_cachedCameraMin = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f));
		CameraController.m_cachedCameraMax = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f));
		if (this.OnFinishedFrame != null)
		{
			this.OnFinishedFrame();
		}
	}

	// Token: 0x17000AE2 RID: 2786
	// (get) Token: 0x06004D40 RID: 19776 RVA: 0x001A76D8 File Offset: 0x001A58D8
	// (set) Token: 0x06004D41 RID: 19777 RVA: 0x001A76F8 File Offset: 0x001A58F8
	public bool IsLerping
	{
		get
		{
			return (!this.m_manualControl) ? this.m_isRecoveringFromManualControl : this.m_isLerpingToManualControl;
		}
		set
		{
			if (this.m_manualControl)
			{
				this.m_isLerpingToManualControl = true;
			}
			else
			{
				this.m_isRecoveringFromManualControl = true;
			}
		}
	}

	// Token: 0x06004D42 RID: 19778 RVA: 0x001A7718 File Offset: 0x001A5918
	public void SetManualControl(bool manualControl, bool shouldLerp = true)
	{
		this.m_manualControl = manualControl;
		if (this.m_manualControl)
		{
			this.m_isLerpingToManualControl = shouldLerp;
		}
		else
		{
			this.m_isRecoveringFromManualControl = shouldLerp;
		}
	}

	// Token: 0x06004D43 RID: 19779 RVA: 0x001A7740 File Offset: 0x001A5940
	public void ForceUpdateControllerCameraState(CameraController.ControllerCameraState newState)
	{
		this.controllerCamera.state = newState;
		this.controllerCamera.isTransitioning = false;
		this.controllerCamera.forceTimer = 6f;
	}

	// Token: 0x06004D44 RID: 19780 RVA: 0x001A776C File Offset: 0x001A596C
	public void UpdateOverridePosition(Vector3 newOverridePosition, float duration)
	{
		base.StartCoroutine(this.UpdateOverridePosition_CR(newOverridePosition, duration));
	}

	// Token: 0x06004D45 RID: 19781 RVA: 0x001A7780 File Offset: 0x001A5980
	public IEnumerator UpdateOverridePosition_CR(Vector3 newOverridePosition, float duration)
	{
		float ela = 0f;
		Vector3 startOverride = this.OverridePosition;
		while (ela < duration)
		{
			ela += BraveTime.DeltaTime;
			this.OverridePosition = Vector3.Lerp(startOverride, newOverridePosition, ela / duration);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004D46 RID: 19782 RVA: 0x001A77AC File Offset: 0x001A59AC
	public Vector2 GetAimContribution()
	{
		if (this.m_manualControl || BraveInput.GetInstanceForPlayer(0).IsKeyboardAndMouse(false))
		{
			return Vector2.zero;
		}
		if (this.controllerCamera.UseAimContribution && this.OverrideZoomScale == 1f && GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER)
		{
			return this.m_lastAimOffset;
		}
		return Vector2.zero;
	}

	// Token: 0x06004D47 RID: 19783 RVA: 0x001A7818 File Offset: 0x001A5A18
	public void ResetAimContribution()
	{
		this.m_lastAimOffset = Vector2.zero;
		this.m_aimOffsetVelocity = Vector2.zero;
	}

	// Token: 0x06004D48 RID: 19784 RVA: 0x001A7830 File Offset: 0x001A5A30
	public void ForceToPlayerPosition(PlayerController p)
	{
		base.transform.position = BraveUtility.QuantizeVector(p.transform.position.WithZ(this.CurrentZOffset), (float)PhysicsEngine.Instance.PixelsPerUnit) + new Vector3(0.03125f, 0.03125f, 0f);
		if (this.controllerCamera != null)
		{
			this.controllerCamera.isTransitioning = false;
			this.controllerCamera.transitionStart = base.transform.position;
		}
	}

	// Token: 0x06004D49 RID: 19785 RVA: 0x001A78BC File Offset: 0x001A5ABC
	public void ForceToPlayerPosition(PlayerController p, Vector3 prevPlayerPosition)
	{
		Vector3 vector = base.transform.position - prevPlayerPosition;
		Vector3 vector2 = p.transform.position + vector;
		base.transform.position = BraveUtility.QuantizeVector(vector2.WithZ(this.CurrentZOffset), (float)PhysicsEngine.Instance.PixelsPerUnit) + new Vector3(0.03125f, 0.03125f, 0f);
		if (this.controllerCamera != null)
		{
			this.controllerCamera.isTransitioning = false;
			this.controllerCamera.transitionStart = base.transform.position;
		}
	}

	// Token: 0x06004D4A RID: 19786 RVA: 0x001A7960 File Offset: 0x001A5B60
	public void AssignBoundingPolygon(RoomHandlerBoundingPolygon p)
	{
	}

	// Token: 0x06004D4B RID: 19787 RVA: 0x001A7964 File Offset: 0x001A5B64
	public void StartTrackingPlayer()
	{
		this.m_isTrackingPlayer = true;
	}

	// Token: 0x06004D4C RID: 19788 RVA: 0x001A7970 File Offset: 0x001A5B70
	public void StopTrackingPlayer()
	{
		this.m_isRecoveringFromManualControl = true;
		this.m_isTrackingPlayer = false;
	}

	// Token: 0x06004D4D RID: 19789 RVA: 0x001A7980 File Offset: 0x001A5B80
	public void DoScreenShake(ScreenShakeSettings shakesettings, Vector2? shakeOrigin, bool isPlayerGun = false)
	{
		float num = shakesettings.magnitude;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Options.CoopScreenShakeReduction)
		{
			num *= 0.3f;
		}
		if (isPlayerGun)
		{
			num *= 0.75f;
		}
		bool flag = shakesettings.vibrationType != ScreenShakeSettings.VibrationType.None;
		if (shakesettings.vibrationType == ScreenShakeSettings.VibrationType.Simple)
		{
			BraveInput.DoVibrationForAllPlayers(shakesettings.simpleVibrationTime, shakesettings.simpleVibrationStrength);
			flag = false;
		}
		base.StartCoroutine(this.HandleScreenShake(num, shakesettings.speed, shakesettings.time, shakesettings.falloff, shakesettings.direction, shakeOrigin, flag));
	}

	// Token: 0x06004D4E RID: 19790 RVA: 0x001A7A20 File Offset: 0x001A5C20
	public void DoScreenShake(float magnitude, float shakeSpeed, float time, float falloffTime, Vector2? shakeOrigin)
	{
		float num = magnitude;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Options.CoopScreenShakeReduction)
		{
			num *= 0.3f;
		}
		base.StartCoroutine(this.HandleScreenShake(num, shakeSpeed, time, falloffTime, Vector2.zero, shakeOrigin, true));
	}

	// Token: 0x06004D4F RID: 19791 RVA: 0x001A7A70 File Offset: 0x001A5C70
	public void DoGunScreenShake(ScreenShakeSettings shakesettings, Vector2 dir, Vector2? shakeOrigin, PlayerController playerOwner = null)
	{
		float num = shakesettings.magnitude;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Options.CoopScreenShakeReduction)
		{
			num *= 0.3f;
		}
		if (playerOwner)
		{
			num *= 0.75f;
		}
		bool flag = shakesettings.vibrationType != ScreenShakeSettings.VibrationType.None;
		if (playerOwner)
		{
			if (shakesettings.vibrationType == ScreenShakeSettings.VibrationType.Auto)
			{
				playerOwner.DoScreenShakeVibration(shakesettings.time, shakesettings.magnitude);
				flag = false;
			}
			else if (shakesettings.vibrationType == ScreenShakeSettings.VibrationType.Simple)
			{
				playerOwner.DoVibration(shakesettings.simpleVibrationTime, shakesettings.simpleVibrationStrength);
				flag = false;
			}
		}
		base.StartCoroutine(this.HandleScreenShake(num, shakesettings.speed, shakesettings.time, shakesettings.falloff, dir, shakeOrigin, flag));
	}

	// Token: 0x17000AE3 RID: 2787
	// (get) Token: 0x06004D50 RID: 19792 RVA: 0x001A7B44 File Offset: 0x001A5D44
	public Vector2 MinVisiblePoint
	{
		get
		{
			return this.m_cachedMinPos;
		}
	}

	// Token: 0x17000AE4 RID: 2788
	// (get) Token: 0x06004D51 RID: 19793 RVA: 0x001A7B4C File Offset: 0x001A5D4C
	public Vector2 MaxVisiblePoint
	{
		get
		{
			return this.m_cachedMaxPos;
		}
	}

	// Token: 0x06004D52 RID: 19794 RVA: 0x001A7B54 File Offset: 0x001A5D54
	public bool PointIsVisible(Vector2 flatPoint)
	{
		return flatPoint.x > this.m_cachedMinPos.x && flatPoint.x < this.m_cachedMaxPos.x && flatPoint.y > this.m_cachedMinPos.y && flatPoint.y < this.m_cachedMaxPos.y;
	}

	// Token: 0x06004D53 RID: 19795 RVA: 0x001A7BC0 File Offset: 0x001A5DC0
	public bool PointIsVisible(Vector2 flatPoint, float percentBuffer)
	{
		Vector2 vector = this.m_cachedSize * percentBuffer;
		return flatPoint.x > this.m_cachedMinPos.x - vector.x && flatPoint.x < this.m_cachedMaxPos.x + vector.x && flatPoint.y > this.m_cachedMinPos.y - vector.y && flatPoint.y < this.m_cachedMaxPos.y + vector.y;
	}

	// Token: 0x06004D54 RID: 19796 RVA: 0x001A7C58 File Offset: 0x001A5E58
	public void DoContinuousScreenShake(ScreenShakeSettings shakesettings, Component source, bool isPlayerGun = false)
	{
		float num = shakesettings.magnitude;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Options.CoopScreenShakeReduction)
		{
			num *= 0.3f;
		}
		if (isPlayerGun)
		{
			num *= 0.75f;
		}
		bool flag = shakesettings.vibrationType != ScreenShakeSettings.VibrationType.None;
		if (shakesettings.vibrationType == ScreenShakeSettings.VibrationType.Simple)
		{
			BraveInput.DoVibrationForAllPlayers(shakesettings.simpleVibrationTime, shakesettings.simpleVibrationStrength);
			flag = false;
		}
		IEnumerator enumerator = this.HandleContinuousScreenShake(num, shakesettings.speed, shakesettings.direction, source, flag);
		if (this.continuousShakeMap.ContainsKey(source))
		{
			Debug.LogWarning("Overwriting previous screen shake for " + source, source);
			this.StopContinuousScreenShake(source);
		}
		this.continuousShakeMap.Add(source, enumerator);
		this.activeContinuousShakes.Add(enumerator);
	}

	// Token: 0x06004D55 RID: 19797 RVA: 0x001A7D28 File Offset: 0x001A5F28
	public void DoDelayedScreenShake(ScreenShakeSettings s, float delay, Vector2? shakeOrigin)
	{
		base.StartCoroutine(this.HandleDelayedScreenShake(s, delay, shakeOrigin));
	}

	// Token: 0x06004D56 RID: 19798 RVA: 0x001A7D3C File Offset: 0x001A5F3C
	public void StopContinuousScreenShake(Component source)
	{
		if (this.continuousShakeMap.ContainsKey(source))
		{
			IEnumerator enumerator = this.continuousShakeMap[source];
			this.m_terminateNextContinuousScreenShake = true;
			enumerator.MoveNext();
			this.continuousShakeMap.Remove(source);
			this.activeContinuousShakes.Remove(enumerator);
		}
	}

	// Token: 0x06004D57 RID: 19799 RVA: 0x001A7D90 File Offset: 0x001A5F90
	public Vector3 DoFrameScreenShake(float magnitude, float shakeSpeed, Vector2 direction, Vector3 lastShakeAmount, float elapsedTime)
	{
		this.screenShakeAmount -= lastShakeAmount;
		if (direction == Vector2.zero)
		{
			float num = Mathf.PerlinNoise(0.3141567f + elapsedTime * shakeSpeed / 1.073f, 0.1156832f + elapsedTime * shakeSpeed / 4.8127f) * 2f - 1f;
			float num2 = Mathf.PerlinNoise(0.7159354f + elapsedTime * shakeSpeed / 2.3727f, 0.9315825f + elapsedTime * shakeSpeed / 0.9812f) * 2f - 1f;
			Vector2 vector = new Vector2(num, num2);
			float num3 = magnitude - Mathf.PingPong(elapsedTime * shakeSpeed, magnitude) / magnitude * magnitude;
			Vector2 vector2 = vector.normalized * num3;
			this.screenShakeAmount += new Vector3(vector2.x, vector2.y, 0f);
			BraveInput.DoSustainedScreenShakeVibration(magnitude);
			return new Vector3(vector2.x, vector2.y, 0f);
		}
		float num4 = Mathf.PingPong(elapsedTime * shakeSpeed, magnitude);
		Vector2 vector3 = new Vector2(num4 * direction.x, num4 * direction.y);
		this.screenShakeAmount += new Vector3(vector3.x, vector3.y, 0f);
		BraveInput.DoSustainedScreenShakeVibration(magnitude);
		return new Vector3(vector3.x, vector3.y, 0f);
	}

	// Token: 0x06004D58 RID: 19800 RVA: 0x001A7F08 File Offset: 0x001A6108
	public void ClearFrameScreenShake(Vector3 lastShakeAmount)
	{
		this.screenShakeAmount -= lastShakeAmount;
	}

	// Token: 0x06004D59 RID: 19801 RVA: 0x001A7F1C File Offset: 0x001A611C
	private IEnumerator HandleContinuousScreenShake(float magnitude, float shakeSpeed, Vector2 direction, Component source, bool useCameraVibration)
	{
		float t = 0f;
		Vector3 lastScreenShakeAmount = Vector3.zero;
		Vector2 vector;
		if (direction != Vector2.zero)
		{
			vector = direction.normalized;
		}
		else
		{
			Vector2 vector2 = new Vector2(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f);
			vector = vector2.normalized;
		}
		Vector2 baseDirection = vector;
		magnitude *= 0.5f;
		while (!this.m_terminateNextContinuousScreenShake)
		{
			this.screenShakeAmount -= lastScreenShakeAmount;
			t += this.m_deltaTime * this.CurrentStickyFriction;
			float currentMagnitude = Mathf.PingPong(t * shakeSpeed, magnitude);
			Vector2 contribution = new Vector2(currentMagnitude * baseDirection.x, currentMagnitude * baseDirection.y);
			this.screenShakeAmount += new Vector3(contribution.x, contribution.y, 0f);
			lastScreenShakeAmount = new Vector3(contribution.x, contribution.y, 0f);
			if (useCameraVibration)
			{
				this.UpdateScreenShakeVibration(magnitude);
			}
			yield return null;
		}
		this.screenShakeAmount -= lastScreenShakeAmount;
		this.m_terminateNextContinuousScreenShake = false;
		yield break;
	}

	// Token: 0x06004D5A RID: 19802 RVA: 0x001A7F54 File Offset: 0x001A6154
	private IEnumerator HandleDelayedScreenShake(ScreenShakeSettings sss, float delay, Vector2? origin)
	{
		yield return new WaitForSeconds(delay);
		this.DoScreenShake(sss, origin, false);
		yield break;
	}

	// Token: 0x06004D5B RID: 19803 RVA: 0x001A7F84 File Offset: 0x001A6184
	private IEnumerator HandleScreenShake(float magnitude, float shakeSpeed, float time, float falloffTime, Vector2 direction, Vector2? origin, bool useCameraVibration)
	{
		if (origin != null)
		{
			Vector2 vector = BraveUtility.ScreenCenterWorldPoint();
			float num = Vector2.Distance(origin.Value, vector);
			if (num > this.screenShakeDist)
			{
				yield break;
			}
			float num2 = Mathf.Clamp01(this.screenShakeCurve.Evaluate(num / this.screenShakeDist));
			magnitude *= num2;
			shakeSpeed *= num2;
		}
		if (magnitude == 0f)
		{
			yield break;
		}
		float t = 0f;
		Vector3 lastScreenShakeAmount = Vector3.zero;
		if (direction == Vector2.zero)
		{
			Vector4 randoms = new Vector4(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
			magnitude *= 0.5f;
			shakeSpeed *= 3.75f;
			while (t < time + falloffTime)
			{
				if (!GameManager.Instance.IsPaused)
				{
					this.screenShakeAmount -= lastScreenShakeAmount;
					float num3 = magnitude;
					if (t > time)
					{
						if (falloffTime <= 0f)
						{
							num3 = 0f;
						}
						else
						{
							num3 = Mathf.Sqrt(1f - (t - time) / falloffTime) * magnitude;
						}
					}
					float num4 = Mathf.PerlinNoise(randoms.x + t * shakeSpeed / 1.073f, randoms.y + t * shakeSpeed / 4.8127f) * 2f - 1f;
					float num5 = Mathf.PerlinNoise(randoms.z + t * shakeSpeed / 2.3727f, randoms.w + t * shakeSpeed / 0.9812f) * 2f - 1f;
					Vector2 vector2 = new Vector2(num4, num5);
					float num6 = num3 - Mathf.PingPong(t * shakeSpeed, magnitude) / magnitude * num3;
					Vector2 vector3 = vector2.normalized * num6;
					if (float.IsNaN(vector3.x) || float.IsNaN(vector3.y))
					{
						yield break;
					}
					this.screenShakeAmount += new Vector3(vector3.x, vector3.y, 0f);
					lastScreenShakeAmount = new Vector3(vector3.x, vector3.y, 0f);
					if (useCameraVibration)
					{
						this.UpdateScreenShakeVibration(num3);
					}
					t += this.m_deltaTime * this.CurrentStickyFriction;
				}
				yield return null;
			}
			this.screenShakeAmount -= lastScreenShakeAmount;
		}
		else
		{
			Vector2 baseDirection = direction.normalized;
			magnitude *= 0.5f;
			while (t < time + falloffTime)
			{
				if (!GameManager.Instance.IsPaused)
				{
					this.screenShakeAmount -= lastScreenShakeAmount;
					float num7 = magnitude;
					if (t > time)
					{
						if (falloffTime <= 0f)
						{
							num7 = 0f;
						}
						else
						{
							num7 = Mathf.Sqrt(1f - (t - time) / falloffTime) * magnitude;
						}
					}
					float num8 = Mathf.Clamp01(Mathf.PingPong(t * shakeSpeed, magnitude) / magnitude);
					float num9 = Mathf.Lerp(num7, -num7, num8);
					Vector2 vector4 = baseDirection * num9;
					if (float.IsNaN(vector4.x) || float.IsNaN(vector4.y))
					{
						yield break;
					}
					this.screenShakeAmount += new Vector3(vector4.x, vector4.y, 0f);
					lastScreenShakeAmount = new Vector3(vector4.x, vector4.y, 0f);
					if (useCameraVibration)
					{
						this.UpdateScreenShakeVibration(num7);
					}
					t += this.m_deltaTime * this.CurrentStickyFriction;
				}
				yield return null;
			}
			this.screenShakeAmount -= lastScreenShakeAmount;
		}
		yield break;
	}

	// Token: 0x06004D5C RID: 19804 RVA: 0x001A7FD4 File Offset: 0x001A61D4
	private Vector2 GetBoundedCameraPositionInRect(Rect rect, Vector2 focalPos, ref Vector2 aimOffset)
	{
		Vector2 vector = focalPos;
		Vector2 vector2 = this.m_camera.ViewportToWorldPoint(Vector2.zero);
		Vector2 vector3 = this.m_camera.ViewportToWorldPoint(Vector2.one);
		Rect rect2 = new Rect(vector2.x, vector2.y, vector3.x - vector2.x, vector3.y - vector2.y);
		rect2.center = focalPos;
		float num = this.controllerCamera.VisibleBorder / this.controllerCamera.ModifiedAimContribution;
		if (rect2.width > rect.width)
		{
			float num2 = Mathf.Max(1f, this.controllerCamera.VisibleBorder - (rect2.width - rect.width) / 2f);
			if (rect2.center.x < rect.center.x)
			{
				float num3 = (rect.center.x - rect2.center.x) / (rect.width / 2f);
				vector.x = rect.center.x - this.controllerCamera.BorderBumperCurve.Evaluate(num3) * num2;
				aimOffset.x = (1f - num3) * aimOffset.x + num3 * aimOffset.x * num;
			}
			else if (rect2.center.x > rect.center.x)
			{
				float num4 = (rect2.center.x - rect.center.x) / (rect.width / 2f);
				vector.x = rect.center.x + this.controllerCamera.BorderBumperCurve.Evaluate(num4) * num2;
				aimOffset.x = (1f - num4) * aimOffset.x + num4 * aimOffset.x * num;
			}
		}
		else if (rect2.xMin < rect.xMin)
		{
			float num5 = (rect.xMin - rect2.xMin) / (rect2.width / 2f);
			vector.x = rect.xMin - this.controllerCamera.BorderBumperCurve.Evaluate(num5) * this.controllerCamera.VisibleBorder + rect2.width / 2f;
			aimOffset.x = (1f - num5) * aimOffset.x + num5 * aimOffset.x * num;
		}
		else if (rect2.xMax > rect.xMax)
		{
			float num6 = (rect2.xMax - rect.xMax) / (rect2.width / 2f);
			vector.x = rect.xMax + this.controllerCamera.BorderBumperCurve.Evaluate(num6) * this.controllerCamera.VisibleBorder - rect2.width / 2f;
			aimOffset.x = (1f - num6) * aimOffset.x + num6 * aimOffset.x * num;
		}
		if (rect2.height > rect.height)
		{
			float num7 = Mathf.Max(1f, this.controllerCamera.VisibleBorder - (rect2.height - rect.height) / 2f);
			if (rect2.center.y < rect.center.y)
			{
				float num8 = (rect.center.y - rect2.center.y) / (rect.height / 2f);
				vector.y = rect.center.y - this.controllerCamera.BorderBumperCurve.Evaluate(num8) * num7;
				aimOffset.y = (1f - num8) * aimOffset.y + num8 * aimOffset.y * num;
			}
			else if (rect2.center.y > rect.center.y)
			{
				float num9 = (rect2.center.y - rect.center.y) / (rect.height / 2f);
				vector.y = rect.center.y + this.controllerCamera.BorderBumperCurve.Evaluate(num9) * num7;
				aimOffset.y = (1f - num9) * aimOffset.y + num9 * aimOffset.y * num;
			}
		}
		else if (rect2.yMin < rect.yMin)
		{
			float num10 = (rect.yMin - rect2.yMin) / (rect2.height / 2f);
			vector.y = rect.yMin - this.controllerCamera.BorderBumperCurve.Evaluate(num10) * this.controllerCamera.VisibleBorder + rect2.height / 2f;
			aimOffset.y = (1f - num10) * aimOffset.y + num10 * aimOffset.y * num;
		}
		else if (rect2.yMax > rect.yMax)
		{
			float num11 = (rect2.yMax - rect.yMax) / (rect2.height / 2f);
			vector.y = rect.yMax + this.controllerCamera.BorderBumperCurve.Evaluate(num11) * this.controllerCamera.VisibleBorder - rect2.height / 2f;
			aimOffset.y = (1f - num11) * aimOffset.y + num11 * aimOffset.y * num;
		}
		return vector;
	}

	// Token: 0x06004D5D RID: 19805 RVA: 0x001A85DC File Offset: 0x001A67DC
	public static Vector2 CameraToWorld(float x, float y)
	{
		return new Vector2(Mathf.Lerp(CameraController.m_cachedCameraMin.x, CameraController.m_cachedCameraMax.x, x), Mathf.Lerp(CameraController.m_cachedCameraMin.y, CameraController.m_cachedCameraMax.y, y));
	}

	// Token: 0x06004D5E RID: 19806 RVA: 0x001A8618 File Offset: 0x001A6818
	public static Vector2 CameraToWorld(Vector2 point)
	{
		return new Vector2(Mathf.Lerp(CameraController.m_cachedCameraMin.x, CameraController.m_cachedCameraMax.x, point.x), Mathf.Lerp(CameraController.m_cachedCameraMin.y, CameraController.m_cachedCameraMax.y, point.y));
	}

	// Token: 0x06004D5F RID: 19807 RVA: 0x001A866C File Offset: 0x001A686C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04004345 RID: 17221
	public CameraController.ControllerCamSettings controllerCamera;

	// Token: 0x04004346 RID: 17222
	private const float c_screenShakeClamp = 5f;

	// Token: 0x04004347 RID: 17223
	public float screenShakeDist;

	// Token: 0x04004348 RID: 17224
	[CurveRange(0f, 0f, 1f, 1f)]
	public AnimationCurve screenShakeCurve;

	// Token: 0x04004349 RID: 17225
	private PlayerController m_player;

	// Token: 0x0400434A RID: 17226
	[SerializeField]
	private float z_Offset = -10f;

	// Token: 0x0400434B RID: 17227
	public bool IsPerspectiveMode;

	// Token: 0x0400434C RID: 17228
	[HideInInspector]
	public float CurrentStickyFriction = 1f;

	// Token: 0x0400434D RID: 17229
	[HideInInspector]
	public Vector3 OverridePosition;

	// Token: 0x0400434E RID: 17230
	[HideInInspector]
	public bool UseOverridePlayerOnePosition;

	// Token: 0x0400434F RID: 17231
	[HideInInspector]
	public Vector2 OverridePlayerOnePosition;

	// Token: 0x04004350 RID: 17232
	[HideInInspector]
	public bool UseOverridePlayerTwoPosition;

	// Token: 0x04004351 RID: 17233
	[HideInInspector]
	public Vector2 OverridePlayerTwoPosition;

	// Token: 0x04004352 RID: 17234
	[NonSerialized]
	public float OverrideZoomScale = 1f;

	// Token: 0x04004353 RID: 17235
	[NonSerialized]
	public float CurrentZoomScale = 1f;

	// Token: 0x04004354 RID: 17236
	private float m_screenShakeVibration;

	// Token: 0x04004355 RID: 17237
	private bool m_screenShakeVibrationDirty;

	// Token: 0x04004356 RID: 17238
	private Vector3 screenShakeAmount = Vector3.zero;

	// Token: 0x04004357 RID: 17239
	private Vector2 previousBasePosition;

	// Token: 0x04004358 RID: 17240
	private Dictionary<Component, IEnumerator> continuousShakeMap = new Dictionary<Component, IEnumerator>();

	// Token: 0x04004359 RID: 17241
	private List<IEnumerator> activeContinuousShakes = new List<IEnumerator>();

	// Token: 0x0400435A RID: 17242
	private bool m_isTrackingPlayer = true;

	// Token: 0x0400435B RID: 17243
	private bool m_manualControl;

	// Token: 0x0400435C RID: 17244
	private bool m_isLerpingToManualControl;

	// Token: 0x0400435D RID: 17245
	private bool m_isRecoveringFromManualControl;

	// Token: 0x0400435E RID: 17246
	private Vector2 m_lastAimOffset = Vector2.zero;

	// Token: 0x0400435F RID: 17247
	private Vector2 m_aimOffsetVelocity = Vector2.zero;

	// Token: 0x04004361 RID: 17249
	private Vector3 m_currentVelocity;

	// Token: 0x04004362 RID: 17250
	private Camera m_camera;

	// Token: 0x04004363 RID: 17251
	[NonSerialized]
	public float OverrideRecoverySpeed = -1f;

	// Token: 0x04004364 RID: 17252
	private const float RECOVERY_SPEED = 20f;

	// Token: 0x04004369 RID: 17257
	[NonSerialized]
	public Vector3 FINAL_CAMERA_POSITION_OFFSET;

	// Token: 0x0400436A RID: 17258
	public Action OnFinishedFrame;

	// Token: 0x0400436B RID: 17259
	private List<UnityEngine.Object> m_focusObjects = new List<UnityEngine.Object>();

	// Token: 0x0400436C RID: 17260
	private Vector2 m_cachedMinPos;

	// Token: 0x0400436D RID: 17261
	private Vector2 m_cachedMaxPos;

	// Token: 0x0400436E RID: 17262
	private Vector2 m_cachedSize;

	// Token: 0x0400436F RID: 17263
	private static Vector2 m_cachedCameraMin;

	// Token: 0x04004370 RID: 17264
	private static Vector2 m_cachedCameraMax;

	// Token: 0x04004371 RID: 17265
	private const float COOP_REDUCTION = 0.3f;

	// Token: 0x04004372 RID: 17266
	private const float c_newScreenShakeModeScalar = 0.5f;

	// Token: 0x04004373 RID: 17267
	private bool m_terminateNextContinuousScreenShake;

	// Token: 0x02000E3D RID: 3645
	[Serializable]
	public class ControllerCamSettings
	{
		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06004D61 RID: 19809 RVA: 0x001A86EC File Offset: 0x001A68EC
		public bool UseAimContribution
		{
			get
			{
				return GameManager.Options.controllerAimLookMultiplier > 0f && !GameManager.Instance.MainCameraController.PreventAimLook;
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06004D62 RID: 19810 RVA: 0x001A8718 File Offset: 0x001A6918
		public float ModifiedAimContribution
		{
			get
			{
				return this.AimContribution * GameManager.Options.controllerAimLookMultiplier;
			}
		}

		// Token: 0x04004374 RID: 17268
		public float VisibleBorder = 4f;

		// Token: 0x04004375 RID: 17269
		public AnimationCurve BorderBumperCurve;

		// Token: 0x04004376 RID: 17270
		public float ToHallwayTime = 1.5f;

		// Token: 0x04004377 RID: 17271
		public float ToRoomTime = 1.5f;

		// Token: 0x04004378 RID: 17272
		public float ToRoomLockTime = 1f;

		// Token: 0x04004379 RID: 17273
		public float EndRoomLockTime = 2f;

		// Token: 0x0400437A RID: 17274
		public float AimContribution = 5f;

		// Token: 0x0400437B RID: 17275
		public float AimContributionTime = 0.5f;

		// Token: 0x0400437C RID: 17276
		public float AimContributionFastTime = 0.25f;

		// Token: 0x0400437D RID: 17277
		public float AimContributionSlowTime = 1f;

		// Token: 0x0400437E RID: 17278
		[NonSerialized]
		public CameraController.ControllerCameraState state;

		// Token: 0x0400437F RID: 17279
		[NonSerialized]
		public bool isTransitioning;

		// Token: 0x04004380 RID: 17280
		[NonSerialized]
		public float transitionTimer;

		// Token: 0x04004381 RID: 17281
		[NonSerialized]
		public float transitionDuration;

		// Token: 0x04004382 RID: 17282
		[NonSerialized]
		public Vector2 transitionStart;

		// Token: 0x04004383 RID: 17283
		[NonSerialized]
		public float forceTimer;

		// Token: 0x04004384 RID: 17284
		[NonSerialized]
		public RoomHandler exitRoomOne;

		// Token: 0x04004385 RID: 17285
		[NonSerialized]
		public RoomHandler exitRoomTwo;
	}

	// Token: 0x02000E3E RID: 3646
	public enum ControllerCameraState
	{
		// Token: 0x04004387 RID: 17287
		FollowPlayer,
		// Token: 0x04004388 RID: 17288
		RoomLock,
		// Token: 0x04004389 RID: 17289
		Off
	}
}
