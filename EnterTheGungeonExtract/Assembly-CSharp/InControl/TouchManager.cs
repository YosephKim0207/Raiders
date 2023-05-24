using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000769 RID: 1897
	[ExecuteInEditMode]
	public class TouchManager : SingletonMonoBehavior<TouchManager, InControlManager>
	{
		// Token: 0x06002A40 RID: 10816 RVA: 0x000C0374 File Offset: 0x000BE574
		protected TouchManager()
		{
		}

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06002A41 RID: 10817 RVA: 0x000C0394 File Offset: 0x000BE594
		// (remove) Token: 0x06002A42 RID: 10818 RVA: 0x000C03C8 File Offset: 0x000BE5C8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnSetup;

		// Token: 0x06002A43 RID: 10819 RVA: 0x000C03FC File Offset: 0x000BE5FC
		private void OnEnable()
		{
			InControlManager component = base.GetComponent<InControlManager>();
			if (component == null)
			{
				UnityEngine.Debug.LogError("Touch Manager component can only be added to the InControl Manager object.");
				UnityEngine.Object.DestroyImmediate(this);
				return;
			}
			if (!base.EnforceSingletonComponent())
			{
				UnityEngine.Debug.LogWarning("There is already a Touch Manager component on this game object.");
				return;
			}
			this.touchControls = base.GetComponentsInChildren<TouchControl>(true);
			if (Application.isPlaying)
			{
				InputManager.OnSetup += this.Setup;
				InputManager.OnUpdateDevices += this.UpdateDevice;
				InputManager.OnCommitDevices += this.CommitDevice;
			}
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x000C0490 File Offset: 0x000BE690
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				InputManager.OnSetup -= this.Setup;
				InputManager.OnUpdateDevices -= this.UpdateDevice;
				InputManager.OnCommitDevices -= this.CommitDevice;
			}
			this.Reset();
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x000C04E0 File Offset: 0x000BE6E0
		private void Setup()
		{
			this.UpdateScreenSize(this.GetCurrentScreenSize());
			this.CreateDevice();
			this.CreateTouches();
			if (TouchManager.OnSetup != null)
			{
				TouchManager.OnSetup();
				TouchManager.OnSetup = null;
			}
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x000C0514 File Offset: 0x000BE714
		private void Reset()
		{
			this.device = null;
			this.mouseTouch = null;
			this.cachedTouches = null;
			this.activeTouches = null;
			this.readOnlyActiveTouches = null;
			this.touchControls = null;
			TouchManager.OnSetup = null;
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x000C0548 File Offset: 0x000BE748
		private IEnumerator UpdateScreenSizeAtEndOfFrame()
		{
			yield return new WaitForEndOfFrame();
			this.UpdateScreenSize(this.GetCurrentScreenSize());
			yield return null;
			yield break;
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x000C0564 File Offset: 0x000BE764
		private void Update()
		{
			Vector2 currentScreenSize = this.GetCurrentScreenSize();
			if (!this.isReady)
			{
				base.StartCoroutine(this.UpdateScreenSizeAtEndOfFrame());
				this.UpdateScreenSize(currentScreenSize);
				this.isReady = true;
				return;
			}
			if (this.screenSize != currentScreenSize)
			{
				this.UpdateScreenSize(currentScreenSize);
			}
			if (TouchManager.OnSetup != null)
			{
				TouchManager.OnSetup();
				TouchManager.OnSetup = null;
			}
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x000C05D4 File Offset: 0x000BE7D4
		private void CreateDevice()
		{
			this.device = new TouchInputDevice();
			this.device.AddControl(InputControlType.LeftStickLeft, "LeftStickLeft");
			this.device.AddControl(InputControlType.LeftStickRight, "LeftStickRight");
			this.device.AddControl(InputControlType.LeftStickUp, "LeftStickUp");
			this.device.AddControl(InputControlType.LeftStickDown, "LeftStickDown");
			this.device.AddControl(InputControlType.RightStickLeft, "RightStickLeft");
			this.device.AddControl(InputControlType.RightStickRight, "RightStickRight");
			this.device.AddControl(InputControlType.RightStickUp, "RightStickUp");
			this.device.AddControl(InputControlType.RightStickDown, "RightStickDown");
			this.device.AddControl(InputControlType.DPadUp, "DPadUp");
			this.device.AddControl(InputControlType.DPadDown, "DPadDown");
			this.device.AddControl(InputControlType.DPadLeft, "DPadLeft");
			this.device.AddControl(InputControlType.DPadRight, "DPadRight");
			this.device.AddControl(InputControlType.LeftTrigger, "LeftTrigger");
			this.device.AddControl(InputControlType.RightTrigger, "RightTrigger");
			this.device.AddControl(InputControlType.LeftBumper, "LeftBumper");
			this.device.AddControl(InputControlType.RightBumper, "RightBumper");
			for (InputControlType inputControlType = InputControlType.Action1; inputControlType <= InputControlType.Action4; inputControlType++)
			{
				this.device.AddControl(inputControlType, inputControlType.ToString());
			}
			this.device.AddControl(InputControlType.Menu, "Menu");
			for (InputControlType inputControlType2 = InputControlType.Button0; inputControlType2 <= InputControlType.Button19; inputControlType2++)
			{
				this.device.AddControl(inputControlType2, inputControlType2.ToString());
			}
			InputManager.AttachDevice(this.device);
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x000C0798 File Offset: 0x000BE998
		private void UpdateDevice(ulong updateTick, float deltaTime)
		{
			this.UpdateTouches(updateTick, deltaTime);
			this.SubmitControlStates(updateTick, deltaTime);
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x000C07AC File Offset: 0x000BE9AC
		private void CommitDevice(ulong updateTick, float deltaTime)
		{
			this.CommitControlStates(updateTick, deltaTime);
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000C07B8 File Offset: 0x000BE9B8
		private void SubmitControlStates(ulong updateTick, float deltaTime)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.SubmitControlState(updateTick, deltaTime);
				}
			}
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x000C080C File Offset: 0x000BEA0C
		private void CommitControlStates(ulong updateTick, float deltaTime)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.CommitControlState(updateTick, deltaTime);
				}
			}
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x000C0860 File Offset: 0x000BEA60
		private void UpdateScreenSize(Vector2 currentScreenSize)
		{
			this.touchCamera.rect = new Rect(0f, 0f, 0.99f, 1f);
			this.touchCamera.rect = new Rect(0f, 0f, 1f, 1f);
			this.screenSize = currentScreenSize;
			this.halfScreenSize = this.screenSize / 2f;
			this.viewSize = this.ConvertViewToWorldPoint(Vector2.one) * 0.02f;
			this.percentToWorld = Mathf.Min(this.viewSize.x, this.viewSize.y);
			this.halfPercentToWorld = this.percentToWorld / 2f;
			if (this.touchCamera != null)
			{
				this.halfPixelToWorld = this.touchCamera.orthographicSize / this.screenSize.y;
				this.pixelToWorld = this.halfPixelToWorld * 2f;
			}
			if (this.touchControls != null)
			{
				int num = this.touchControls.Length;
				for (int i = 0; i < num; i++)
				{
					this.touchControls[i].ConfigureControl();
				}
			}
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x000C0994 File Offset: 0x000BEB94
		private void CreateTouches()
		{
			this.cachedTouches = new TouchPool();
			this.mouseTouch = new Touch();
			this.mouseTouch.fingerId = Touch.FingerID_Mouse;
			this.activeTouches = new List<Touch>(32);
			this.readOnlyActiveTouches = new ReadOnlyCollection<Touch>(this.activeTouches);
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x000C09E8 File Offset: 0x000BEBE8
		private void UpdateTouches(ulong updateTick, float deltaTime)
		{
			this.activeTouches.Clear();
			this.cachedTouches.FreeEndedTouches();
			if (this.mouseTouch.SetWithMouseData(updateTick, deltaTime))
			{
				this.activeTouches.Add(this.mouseTouch);
			}
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				Touch touch2 = this.cachedTouches.FindOrCreateTouch(touch.fingerId);
				touch2.SetWithTouchData(touch, updateTick, deltaTime);
				this.activeTouches.Add(touch2);
			}
			int count = this.cachedTouches.Touches.Count;
			for (int j = 0; j < count; j++)
			{
				Touch touch3 = this.cachedTouches.Touches[j];
				if (touch3.phase != TouchPhase.Ended && touch3.updateTick != updateTick)
				{
					touch3.phase = TouchPhase.Ended;
					this.activeTouches.Add(touch3);
				}
			}
			this.InvokeTouchEvents();
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x000C0AE4 File Offset: 0x000BECE4
		private void SendTouchBegan(Touch touch)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.TouchBegan(touch);
				}
			}
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x000C0B38 File Offset: 0x000BED38
		private void SendTouchMoved(Touch touch)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.TouchMoved(touch);
				}
			}
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x000C0B8C File Offset: 0x000BED8C
		private void SendTouchEnded(Touch touch)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.TouchEnded(touch);
				}
			}
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x000C0BE0 File Offset: 0x000BEDE0
		private void InvokeTouchEvents()
		{
			int count = this.activeTouches.Count;
			if (this.enableControlsOnTouch && count > 0 && !this.controlsEnabled)
			{
				TouchManager.Device.RequestActivation();
				this.controlsEnabled = true;
			}
			for (int i = 0; i < count; i++)
			{
				Touch touch = this.activeTouches[i];
				switch (touch.phase)
				{
				case TouchPhase.Began:
					this.SendTouchBegan(touch);
					break;
				case TouchPhase.Moved:
					this.SendTouchMoved(touch);
					break;
				case TouchPhase.Ended:
					this.SendTouchEnded(touch);
					break;
				case TouchPhase.Canceled:
					this.SendTouchEnded(touch);
					break;
				}
			}
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x000C0C9C File Offset: 0x000BEE9C
		private bool TouchCameraIsValid()
		{
			return !(this.touchCamera == null) && !Utility.IsZero(this.touchCamera.orthographicSize) && (!Utility.IsZero(this.touchCamera.rect.width) || !Utility.IsZero(this.touchCamera.rect.height)) && (!Utility.IsZero(this.touchCamera.pixelRect.width) || !Utility.IsZero(this.touchCamera.pixelRect.height));
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x000C0D4C File Offset: 0x000BEF4C
		private Vector3 ConvertScreenToWorldPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ScreenToWorldPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x000C0DA4 File Offset: 0x000BEFA4
		private Vector3 ConvertViewToWorldPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ViewportToWorldPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x000C0DFC File Offset: 0x000BEFFC
		private Vector3 ConvertScreenToViewPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ScreenToViewportPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x000C0E54 File Offset: 0x000BF054
		private Vector2 GetCurrentScreenSize()
		{
			if (this.TouchCameraIsValid())
			{
				return new Vector2((float)this.touchCamera.pixelWidth, (float)this.touchCamera.pixelHeight);
			}
			return new Vector2((float)Screen.width, (float)Screen.height);
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06002A5A RID: 10842 RVA: 0x000C0E90 File Offset: 0x000BF090
		// (set) Token: 0x06002A5B RID: 10843 RVA: 0x000C0E98 File Offset: 0x000BF098
		public bool controlsEnabled
		{
			get
			{
				return this._controlsEnabled;
			}
			set
			{
				if (this._controlsEnabled != value)
				{
					int num = this.touchControls.Length;
					for (int i = 0; i < num; i++)
					{
						this.touchControls[i].enabled = value;
					}
					this._controlsEnabled = value;
				}
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06002A5C RID: 10844 RVA: 0x000C0EE4 File Offset: 0x000BF0E4
		public static ReadOnlyCollection<Touch> Touches
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.readOnlyActiveTouches;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06002A5D RID: 10845 RVA: 0x000C0EF0 File Offset: 0x000BF0F0
		public static int TouchCount
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.activeTouches.Count;
			}
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000C0F04 File Offset: 0x000BF104
		public static Touch GetTouch(int touchIndex)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.activeTouches[touchIndex];
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x000C0F18 File Offset: 0x000BF118
		public static Touch GetTouchByFingerId(int fingerId)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.cachedTouches.FindTouch(fingerId);
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000C0F2C File Offset: 0x000BF12C
		public static Vector3 ScreenToWorldPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.ConvertScreenToWorldPoint(point);
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000C0F3C File Offset: 0x000BF13C
		public static Vector3 ViewToWorldPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.ConvertViewToWorldPoint(point);
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000C0F4C File Offset: 0x000BF14C
		public static Vector3 ScreenToViewPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.ConvertScreenToViewPoint(point);
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000C0F5C File Offset: 0x000BF15C
		public static float ConvertToWorld(float value, TouchUnitType unitType)
		{
			return value * ((unitType != TouchUnitType.Pixels) ? TouchManager.PercentToWorld : TouchManager.PixelToWorld);
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000C0F78 File Offset: 0x000BF178
		public static Rect PercentToWorldRect(Rect rect)
		{
			return new Rect((rect.xMin - 50f) * TouchManager.ViewSize.x, (rect.yMin - 50f) * TouchManager.ViewSize.y, rect.width * TouchManager.ViewSize.x, rect.height * TouchManager.ViewSize.y);
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x000C0FEC File Offset: 0x000BF1EC
		public static Rect PixelToWorldRect(Rect rect)
		{
			return new Rect(Mathf.Round(rect.xMin - TouchManager.HalfScreenSize.x) * TouchManager.PixelToWorld, Mathf.Round(rect.yMin - TouchManager.HalfScreenSize.y) * TouchManager.PixelToWorld, Mathf.Round(rect.width) * TouchManager.PixelToWorld, Mathf.Round(rect.height) * TouchManager.PixelToWorld);
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x000C1064 File Offset: 0x000BF264
		public static Rect ConvertToWorld(Rect rect, TouchUnitType unitType)
		{
			return (unitType != TouchUnitType.Pixels) ? TouchManager.PercentToWorldRect(rect) : TouchManager.PixelToWorldRect(rect);
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002A67 RID: 10855 RVA: 0x000C1080 File Offset: 0x000BF280
		public static Camera Camera
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.touchCamera;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x000C108C File Offset: 0x000BF28C
		public static InputDevice Device
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.device;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06002A69 RID: 10857 RVA: 0x000C1098 File Offset: 0x000BF298
		public static Vector3 ViewSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.viewSize;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06002A6A RID: 10858 RVA: 0x000C10A4 File Offset: 0x000BF2A4
		public static float PercentToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.percentToWorld;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06002A6B RID: 10859 RVA: 0x000C10B0 File Offset: 0x000BF2B0
		public static float HalfPercentToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.halfPercentToWorld;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06002A6C RID: 10860 RVA: 0x000C10BC File Offset: 0x000BF2BC
		public static float PixelToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.pixelToWorld;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06002A6D RID: 10861 RVA: 0x000C10C8 File Offset: 0x000BF2C8
		public static float HalfPixelToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.halfPixelToWorld;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06002A6E RID: 10862 RVA: 0x000C10D4 File Offset: 0x000BF2D4
		public static Vector2 ScreenSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.screenSize;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06002A6F RID: 10863 RVA: 0x000C10E0 File Offset: 0x000BF2E0
		public static Vector2 HalfScreenSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.halfScreenSize;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06002A70 RID: 10864 RVA: 0x000C10EC File Offset: 0x000BF2EC
		public static TouchManager.GizmoShowOption ControlsShowGizmos
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.controlsShowGizmos;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06002A71 RID: 10865 RVA: 0x000C10F8 File Offset: 0x000BF2F8
		// (set) Token: 0x06002A72 RID: 10866 RVA: 0x000C1104 File Offset: 0x000BF304
		public static bool ControlsEnabled
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.controlsEnabled;
			}
			set
			{
				SingletonMonoBehavior<TouchManager, InControlManager>.Instance.controlsEnabled = value;
			}
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x000C1114 File Offset: 0x000BF314
		public static implicit operator bool(TouchManager instance)
		{
			return instance != null;
		}

		// Token: 0x04001D46 RID: 7494
		[Space(10f)]
		public Camera touchCamera;

		// Token: 0x04001D47 RID: 7495
		public TouchManager.GizmoShowOption controlsShowGizmos = TouchManager.GizmoShowOption.Always;

		// Token: 0x04001D48 RID: 7496
		[HideInInspector]
		public bool enableControlsOnTouch;

		// Token: 0x04001D49 RID: 7497
		[HideInInspector]
		[SerializeField]
		private bool _controlsEnabled = true;

		// Token: 0x04001D4A RID: 7498
		[HideInInspector]
		public int controlsLayer = 5;

		// Token: 0x04001D4C RID: 7500
		private InputDevice device;

		// Token: 0x04001D4D RID: 7501
		private Vector3 viewSize;

		// Token: 0x04001D4E RID: 7502
		private Vector2 screenSize;

		// Token: 0x04001D4F RID: 7503
		private Vector2 halfScreenSize;

		// Token: 0x04001D50 RID: 7504
		private float percentToWorld;

		// Token: 0x04001D51 RID: 7505
		private float halfPercentToWorld;

		// Token: 0x04001D52 RID: 7506
		private float pixelToWorld;

		// Token: 0x04001D53 RID: 7507
		private float halfPixelToWorld;

		// Token: 0x04001D54 RID: 7508
		private TouchControl[] touchControls;

		// Token: 0x04001D55 RID: 7509
		private TouchPool cachedTouches;

		// Token: 0x04001D56 RID: 7510
		private List<Touch> activeTouches;

		// Token: 0x04001D57 RID: 7511
		private ReadOnlyCollection<Touch> readOnlyActiveTouches;

		// Token: 0x04001D58 RID: 7512
		private Vector2 lastMousePosition;

		// Token: 0x04001D59 RID: 7513
		private bool isReady;

		// Token: 0x04001D5A RID: 7514
		private Touch mouseTouch;

		// Token: 0x0200076A RID: 1898
		public enum GizmoShowOption
		{
			// Token: 0x04001D5C RID: 7516
			Never,
			// Token: 0x04001D5D RID: 7517
			WhenSelected,
			// Token: 0x04001D5E RID: 7518
			UnlessPlaying,
			// Token: 0x04001D5F RID: 7519
			Always
		}
	}
}
