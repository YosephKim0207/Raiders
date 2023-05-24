using System;
using System.Collections.Generic;
using System.Linq;
using InControl;
using UnityEngine;

// Token: 0x020003D1 RID: 977
[AddComponentMenu("Daikon Forge/User Interface/Input Manager")]
[Serializable]
public class dfInputManager : MonoBehaviour
{
	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x0600133B RID: 4923 RVA: 0x00058558 File Offset: 0x00056758
	public static IList<dfInputManager> ActiveInstances
	{
		get
		{
			return dfInputManager.activeInstances;
		}
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x0600133C RID: 4924 RVA: 0x00058560 File Offset: 0x00056760
	public static dfControl ControlUnderMouse
	{
		get
		{
			return dfInputManager.controlUnderMouse;
		}
	}

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x0600133D RID: 4925 RVA: 0x00058568 File Offset: 0x00056768
	// (set) Token: 0x0600133E RID: 4926 RVA: 0x00058570 File Offset: 0x00056770
	public Camera RenderCamera
	{
		get
		{
			return this.renderCamera;
		}
		set
		{
			this.renderCamera = value;
		}
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x0600133F RID: 4927 RVA: 0x0005857C File Offset: 0x0005677C
	// (set) Token: 0x06001340 RID: 4928 RVA: 0x00058584 File Offset: 0x00056784
	public bool UseTouch
	{
		get
		{
			return this.useTouch;
		}
		set
		{
			this.useTouch = value;
		}
	}

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x06001341 RID: 4929 RVA: 0x00058590 File Offset: 0x00056790
	// (set) Token: 0x06001342 RID: 4930 RVA: 0x00058598 File Offset: 0x00056798
	public bool UseMouse
	{
		get
		{
			return this.useMouse;
		}
		set
		{
			this.useMouse = value;
		}
	}

	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x06001343 RID: 4931 RVA: 0x000585A4 File Offset: 0x000567A4
	// (set) Token: 0x06001344 RID: 4932 RVA: 0x000585AC File Offset: 0x000567AC
	public bool UseJoystick
	{
		get
		{
			return this.useJoystick;
		}
		set
		{
			this.useJoystick = value;
		}
	}

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x06001345 RID: 4933 RVA: 0x000585B8 File Offset: 0x000567B8
	// (set) Token: 0x06001346 RID: 4934 RVA: 0x000585C0 File Offset: 0x000567C0
	public KeyCode JoystickClickButton
	{
		get
		{
			return this.joystickClickButton;
		}
		set
		{
			this.joystickClickButton = value;
		}
	}

	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x06001347 RID: 4935 RVA: 0x000585CC File Offset: 0x000567CC
	// (set) Token: 0x06001348 RID: 4936 RVA: 0x000585D4 File Offset: 0x000567D4
	public string HorizontalAxis
	{
		get
		{
			return this.horizontalAxis;
		}
		set
		{
			this.horizontalAxis = value;
		}
	}

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x06001349 RID: 4937 RVA: 0x000585E0 File Offset: 0x000567E0
	// (set) Token: 0x0600134A RID: 4938 RVA: 0x000585E8 File Offset: 0x000567E8
	public string VerticalAxis
	{
		get
		{
			return this.verticalAxis;
		}
		set
		{
			this.verticalAxis = value;
		}
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x0600134B RID: 4939 RVA: 0x000585F4 File Offset: 0x000567F4
	// (set) Token: 0x0600134C RID: 4940 RVA: 0x000585FC File Offset: 0x000567FC
	public IInputAdapter Adapter
	{
		get
		{
			return this.adapter;
		}
		set
		{
			this.adapter = value ?? new dfInputManager.DefaultInput(this.renderCamera);
		}
	}

	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x0600134D RID: 4941 RVA: 0x00058618 File Offset: 0x00056818
	// (set) Token: 0x0600134E RID: 4942 RVA: 0x00058620 File Offset: 0x00056820
	public bool RetainFocus
	{
		get
		{
			return this.retainFocus;
		}
		set
		{
			this.retainFocus = value;
		}
	}

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x0600134F RID: 4943 RVA: 0x0005862C File Offset: 0x0005682C
	// (set) Token: 0x06001350 RID: 4944 RVA: 0x00058634 File Offset: 0x00056834
	public IDFTouchInputSource TouchInputSource
	{
		get
		{
			return this.touchInputSource;
		}
		set
		{
			this.touchInputSource = value;
		}
	}

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x06001351 RID: 4945 RVA: 0x00058640 File Offset: 0x00056840
	// (set) Token: 0x06001352 RID: 4946 RVA: 0x00058648 File Offset: 0x00056848
	public float HoverStartDelay
	{
		get
		{
			return this.hoverStartDelay;
		}
		set
		{
			this.hoverStartDelay = value;
		}
	}

	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x06001353 RID: 4947 RVA: 0x00058654 File Offset: 0x00056854
	// (set) Token: 0x06001354 RID: 4948 RVA: 0x0005865C File Offset: 0x0005685C
	public float HoverNotificationFrequency
	{
		get
		{
			return this.hoverNotifactionFrequency;
		}
		set
		{
			this.hoverNotifactionFrequency = value;
		}
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x00058668 File Offset: 0x00056868
	public void Awake()
	{
		base.useGUILayout = false;
	}

	// Token: 0x06001356 RID: 4950 RVA: 0x00058674 File Offset: 0x00056874
	public void Start()
	{
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x00058678 File Offset: 0x00056878
	public void OnDisable()
	{
		dfInputManager.activeInstances.Remove(this);
		dfControl activeControl = dfGUIManager.ActiveControl;
		if (activeControl != null && activeControl.transform.IsChildOf(base.transform))
		{
			dfGUIManager.SetFocus(null, true);
		}
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x000586C0 File Offset: 0x000568C0
	public void OnEnable()
	{
		dfInputManager.activeInstances.Add(this);
		this.mouseHandler = new dfInputManager.MouseInputManager();
		if (this.useTouch)
		{
			this.touchHandler = new dfInputManager.TouchInputManager(this);
		}
		if (this.adapter == null)
		{
			Component component = (from c in base.GetComponents(typeof(MonoBehaviour))
				where c != null && c.GetType() != null && typeof(IInputAdapter).IsAssignableFrom(c.GetType())
				select c).FirstOrDefault<Component>();
			this.adapter = ((IInputAdapter)component) ?? new dfInputManager.DefaultInput(this.renderCamera);
		}
		Input.simulateMouseWithTouches = !this.useTouch;
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x0005876C File Offset: 0x0005696C
	public void Update()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.guiManager == null)
		{
			this.guiManager = base.GetComponent<dfGUIManager>();
			if (this.guiManager == null)
			{
				Debug.LogWarning("No associated dfGUIManager instance", this);
				base.enabled = false;
				return;
			}
		}
		dfControl activeControl = dfGUIManager.ActiveControl;
		bool flag = true;
		if (flag && this.useTouch && this.processTouchInput())
		{
			return;
		}
		if (this.useMouse)
		{
			if (BraveInput.PrimaryPlayerInstance != null && BraveInput.PrimaryPlayerInstance.ActiveActions != null)
			{
				Vector2 vector = this.adapter.GetMousePosition() - this.mousePositionLastFrame;
				bool flag2 = BraveInput.PrimaryPlayerInstance.ActiveActions.LastInputType == BindingSourceType.MouseBindingSource || (BraveInput.PrimaryPlayerInstance.ActiveActions.LastInputType == BindingSourceType.KeyBindingSource && vector.magnitude > float.Epsilon) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0);
				if (flag2)
				{
					this.processMouseInput();
				}
			}
			else
			{
				this.processMouseInput();
			}
		}
		if (activeControl == null)
		{
			return;
		}
		if (this.processKeyboard())
		{
			return;
		}
		this.mousePositionLastFrame = this.adapter.GetMousePosition();
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x000588BC File Offset: 0x00056ABC
	private void processJoystick()
	{
		try
		{
			dfControl activeControl = dfGUIManager.ActiveControl;
			if (!(activeControl == null) && activeControl.transform.IsChildOf(base.transform))
			{
				float axis = this.adapter.GetAxis(this.horizontalAxis);
				float axis2 = this.adapter.GetAxis(this.verticalAxis);
				if (Mathf.Abs(axis) < 0.5f && Mathf.Abs(axis2) <= 0.5f)
				{
					this.lastAxisCheck = BraveTime.DeltaTime - this.axisPollingInterval;
				}
				if (Time.realtimeSinceStartup - this.lastAxisCheck > this.axisPollingInterval)
				{
					if (Mathf.Abs(axis) >= 0.5f)
					{
						this.lastAxisCheck = Time.realtimeSinceStartup;
						KeyCode keyCode = ((axis <= 0f) ? KeyCode.LeftArrow : KeyCode.RightArrow);
						activeControl.OnKeyDown(new dfKeyEventArgs(activeControl, keyCode, false, false, false));
					}
					if (Mathf.Abs(axis2) >= 0.5f)
					{
						this.lastAxisCheck = Time.realtimeSinceStartup;
						KeyCode keyCode2 = ((axis2 <= 0f) ? KeyCode.DownArrow : KeyCode.UpArrow);
						activeControl.OnKeyDown(new dfKeyEventArgs(activeControl, keyCode2, false, false, false));
					}
				}
				if (this.joystickClickButton != KeyCode.None)
				{
					bool keyDown = this.adapter.GetKeyDown(this.joystickClickButton);
					if (keyDown)
					{
						Vector3 center = activeControl.GetCenter();
						Camera camera = activeControl.GetCamera();
						Ray ray = camera.ScreenPointToRay(camera.WorldToScreenPoint(center));
						dfMouseEventArgs dfMouseEventArgs = new dfMouseEventArgs(activeControl, dfMouseButtons.Left, 0, ray, center, 0f);
						activeControl.OnMouseDown(dfMouseEventArgs);
						this.buttonDownTarget = activeControl;
					}
					bool keyUp = this.adapter.GetKeyUp(this.joystickClickButton);
					if (keyUp)
					{
						if (this.buttonDownTarget == activeControl)
						{
							activeControl.DoClick();
						}
						Vector3 center2 = activeControl.GetCenter();
						Camera camera2 = activeControl.GetCamera();
						Ray ray2 = camera2.ScreenPointToRay(camera2.WorldToScreenPoint(center2));
						dfMouseEventArgs dfMouseEventArgs2 = new dfMouseEventArgs(activeControl, dfMouseButtons.Left, 0, ray2, center2, 0f);
						activeControl.OnMouseUp(dfMouseEventArgs2);
						this.buttonDownTarget = null;
					}
				}
				for (KeyCode keyCode3 = KeyCode.Joystick1Button0; keyCode3 <= KeyCode.Joystick1Button19; keyCode3++)
				{
					bool keyDown2 = this.adapter.GetKeyDown(keyCode3);
					if (keyDown2)
					{
						activeControl.OnKeyDown(new dfKeyEventArgs(activeControl, keyCode3, false, false, false));
					}
				}
			}
		}
		catch (UnityException ex)
		{
			Debug.LogError(ex.ToString(), this);
			this.useJoystick = false;
		}
	}

	// Token: 0x0600135B RID: 4955 RVA: 0x00058B5C File Offset: 0x00056D5C
	private void processKeyEvent(EventType eventType, KeyCode keyCode, EventModifiers modifiers)
	{
		dfControl activeControl = dfGUIManager.ActiveControl;
		if (activeControl == null || !activeControl.IsEnabled || !activeControl.transform.IsChildOf(base.transform))
		{
			return;
		}
		bool flag = (modifiers & EventModifiers.Control) == EventModifiers.Control || (modifiers & EventModifiers.Command) == EventModifiers.Command;
		bool flag2 = (modifiers & EventModifiers.Shift) == EventModifiers.Shift;
		bool flag3 = (modifiers & EventModifiers.Alt) == EventModifiers.Alt;
		dfKeyEventArgs dfKeyEventArgs = new dfKeyEventArgs(activeControl, keyCode, flag, flag2, flag3);
		if (keyCode >= KeyCode.Space && keyCode <= KeyCode.Z)
		{
			char c = (char)keyCode;
			dfKeyEventArgs.Character = ((!flag2) ? char.ToLower(c) : char.ToUpper(c));
		}
		if (eventType == EventType.KeyDown)
		{
			activeControl.OnKeyDown(dfKeyEventArgs);
		}
		else if (eventType == EventType.KeyUp)
		{
			activeControl.OnKeyUp(dfKeyEventArgs);
		}
		if (dfKeyEventArgs.Used || eventType == EventType.KeyUp)
		{
			return;
		}
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x00058C38 File Offset: 0x00056E38
	private bool processKeyboard()
	{
		dfControl activeControl = dfGUIManager.ActiveControl;
		if (activeControl == null || string.IsNullOrEmpty(Input.inputString) || !activeControl.transform.IsChildOf(base.transform))
		{
			return false;
		}
		foreach (char c in Input.inputString)
		{
			if (c != '\b' && c != '\n')
			{
				KeyCode keyCode = (KeyCode)c;
				activeControl.OnKeyPress(new dfKeyEventArgs(activeControl, keyCode, false, false, false)
				{
					Character = c
				});
			}
		}
		return true;
	}

	// Token: 0x0600135D RID: 4957 RVA: 0x00058CD8 File Offset: 0x00056ED8
	private bool processTouchInput()
	{
		if (this.touchInputSource == null)
		{
			foreach (dfTouchInputSourceComponent dfTouchInputSourceComponent in (from x in base.GetComponents<dfTouchInputSourceComponent>()
				orderby x.Priority descending
				select x).ToArray<dfTouchInputSourceComponent>())
			{
				if (dfTouchInputSourceComponent.enabled)
				{
					this.touchInputSource = dfTouchInputSourceComponent.Source;
					if (this.touchInputSource != null)
					{
						break;
					}
				}
			}
			if (this.touchInputSource == null)
			{
				this.touchInputSource = dfMobileTouchInputSource.Instance;
			}
		}
		this.touchInputSource.Update();
		if (this.touchInputSource.TouchCount == 0)
		{
			return false;
		}
		this.touchHandler.Process(base.transform, this.renderCamera, this.touchInputSource, this.retainFocus);
		return true;
	}

	// Token: 0x0600135E RID: 4958 RVA: 0x00058DBC File Offset: 0x00056FBC
	public bool TestRectUnderMouseEquipment(Bounds b)
	{
		if (!this.renderCamera.enabled)
		{
			return false;
		}
		Vector2 mousePosition = this.adapter.GetMousePosition();
		Vector2 rectSize = BraveCameraUtility.GetRectSize();
		float num = (float)Screen.width * rectSize.x / 2f;
		float num2 = (float)Screen.height * rectSize.y / 2f;
		Vector2 vector = mousePosition - new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
		Vector2 vector2 = vector / ((float)Screen.height / 2f);
		Vector2 vector3 = vector2 * this.renderCamera.orthographicSize;
		Vector3 vector4 = new Vector3(num / 2f / num2 * this.renderCamera.orthographicSize, 0f, 0f);
		Ray ray = new Ray(this.renderCamera.transform.position + vector3.ToVector3ZUp(0f) + vector4, this.renderCamera.transform.forward);
		Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 5f);
		return b.IntersectRay(ray);
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x00058EF8 File Offset: 0x000570F8
	private Vector2 PreprocessUltrawideAmmonomiconMousePosition(Vector2 inPosition)
	{
		Vector2 vector = inPosition;
		Rect rect = BraveCameraUtility.GetRect();
		float num = (float)Screen.height * rect.height;
		float num2 = (float)Screen.width * rect.width;
		float aspect = BraveCameraUtility.ASPECT;
		vector.x -= (float)Screen.width * (1f - rect.width) / 2f;
		vector.y -= (float)Screen.height * (1f - rect.height) / 2f;
		if (aspect > 1.7777778f)
		{
			float num3 = num * 1.7777778f;
			vector.x -= (num2 - num3) / 2f;
			vector.x = Mathf.Lerp(0f, 1920f, Mathf.Clamp01(vector.x / num3));
			vector.y = Mathf.Lerp(0f, 1080f, vector.y / num);
		}
		else
		{
			float num4 = num2 * 0.5625f;
			vector.y -= (num - num4) / 2f;
			vector.x = Mathf.Lerp(0f, 1920f, vector.x / num2);
			vector.y = Mathf.Lerp(0f, 1080f, Mathf.Clamp01(vector.y / num4));
		}
		return vector;
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x00059064 File Offset: 0x00057264
	private void processMouseInput()
	{
		if (this.guiManager == null)
		{
			return;
		}
		Vector2 vector = this.adapter.GetMousePosition();
		if (AmmonomiconController.HasInstance)
		{
			if (AmmonomiconController.Instance.CurrentRightPageRenderer != null && AmmonomiconController.Instance.CurrentRightPageRenderer.guiManager == this.guiManager)
			{
				vector = this.PreprocessUltrawideAmmonomiconMousePosition(vector);
				vector.x -= 960f;
			}
			else if (AmmonomiconController.Instance.CurrentLeftPageRenderer != null && AmmonomiconController.Instance.CurrentLeftPageRenderer.guiManager == this.guiManager)
			{
				vector = this.PreprocessUltrawideAmmonomiconMousePosition(vector);
			}
		}
		Ray ray = this.renderCamera.ScreenPointToRay(vector);
		dfInputManager.controlUnderMouse = dfGUIManager.HitTestAll(vector);
		if (dfInputManager.controlUnderMouse != null && !dfInputManager.controlUnderMouse.transform.IsChildOf(base.transform))
		{
			dfInputManager.controlUnderMouse = null;
		}
		this.mouseHandler.ProcessInput(this, this.adapter, ray, dfInputManager.controlUnderMouse, this.retainFocus);
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x00059198 File Offset: 0x00057398
	internal static int raycastHitSorter(RaycastHit lhs, RaycastHit rhs)
	{
		return lhs.distance.CompareTo(rhs.distance);
	}

	// Token: 0x06001362 RID: 4962 RVA: 0x000591BC File Offset: 0x000573BC
	internal dfControl clipCast(RaycastHit[] hits)
	{
		if (hits == null || hits.Length == 0)
		{
			return null;
		}
		dfControl dfControl = null;
		dfControl modalControl = dfGUIManager.GetModalControl();
		for (int i = hits.Length - 1; i >= 0; i--)
		{
			RaycastHit raycastHit = hits[i];
			dfControl component = raycastHit.transform.GetComponent<dfControl>();
			bool flag = component == null || (modalControl != null && !component.transform.IsChildOf(modalControl.transform)) || !component.enabled || dfInputManager.combinedOpacity(component) <= 0.01f || !component.IsEnabled || !component.IsVisible || !component.transform.IsChildOf(base.transform);
			if (!flag)
			{
				if (dfInputManager.isInsideClippingRegion(raycastHit.point, component) && (dfControl == null || component.RenderOrder > dfControl.RenderOrder))
				{
					dfControl = component;
				}
			}
		}
		return dfControl;
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x000592D4 File Offset: 0x000574D4
	internal static bool isInsideClippingRegion(Vector3 point, dfControl control)
	{
		while (control != null)
		{
			Plane[] array = ((!control.ClipChildren) ? null : control.GetClippingPlanes());
			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (!array[i].GetSide(point))
					{
						return false;
					}
				}
			}
			control = control.Parent;
		}
		return true;
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x0005934C File Offset: 0x0005754C
	private static float combinedOpacity(dfControl control)
	{
		float num = 1f;
		while (control != null)
		{
			num *= control.Opacity;
			control = control.Parent;
		}
		return num;
	}

	// Token: 0x0400109B RID: 4251
	private static dfControl controlUnderMouse = null;

	// Token: 0x0400109C RID: 4252
	private static dfList<dfInputManager> activeInstances = new dfList<dfInputManager>();

	// Token: 0x0400109D RID: 4253
	[SerializeField]
	protected Camera renderCamera;

	// Token: 0x0400109E RID: 4254
	[SerializeField]
	protected bool useTouch = true;

	// Token: 0x0400109F RID: 4255
	[SerializeField]
	protected bool useMouse = true;

	// Token: 0x040010A0 RID: 4256
	[SerializeField]
	protected bool useJoystick;

	// Token: 0x040010A1 RID: 4257
	[SerializeField]
	protected KeyCode joystickClickButton = KeyCode.Joystick1Button1;

	// Token: 0x040010A2 RID: 4258
	[SerializeField]
	protected string horizontalAxis = "Horizontal";

	// Token: 0x040010A3 RID: 4259
	[SerializeField]
	protected string verticalAxis = "Vertical";

	// Token: 0x040010A4 RID: 4260
	[SerializeField]
	protected float axisPollingInterval = 0.15f;

	// Token: 0x040010A5 RID: 4261
	[SerializeField]
	protected bool retainFocus;

	// Token: 0x040010A6 RID: 4262
	[SerializeField]
	[HideInInspector]
	protected int touchClickRadius = 125;

	// Token: 0x040010A7 RID: 4263
	[SerializeField]
	protected float hoverStartDelay = 0.25f;

	// Token: 0x040010A8 RID: 4264
	[SerializeField]
	protected float hoverNotifactionFrequency = 0.1f;

	// Token: 0x040010A9 RID: 4265
	private IDFTouchInputSource touchInputSource;

	// Token: 0x040010AA RID: 4266
	private dfInputManager.TouchInputManager touchHandler;

	// Token: 0x040010AB RID: 4267
	private dfInputManager.MouseInputManager mouseHandler;

	// Token: 0x040010AC RID: 4268
	private dfGUIManager guiManager;

	// Token: 0x040010AD RID: 4269
	private dfControl buttonDownTarget;

	// Token: 0x040010AE RID: 4270
	private IInputAdapter adapter;

	// Token: 0x040010AF RID: 4271
	private float lastAxisCheck;

	// Token: 0x040010B0 RID: 4272
	private Vector2 mousePositionLastFrame;

	// Token: 0x020003D2 RID: 978
	private class TouchInputManager
	{
		// Token: 0x06001368 RID: 4968 RVA: 0x000593D4 File Offset: 0x000575D4
		private TouchInputManager()
		{
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x000593F4 File Offset: 0x000575F4
		public TouchInputManager(dfInputManager manager)
		{
			this.manager = manager;
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0005941C File Offset: 0x0005761C
		internal void Process(Transform transform, Camera renderCamera, IDFTouchInputSource input, bool retainFocusSetting)
		{
			dfInputManager.controlUnderMouse = null;
			IList<dfTouchInfo> touches = input.Touches;
			for (int i = 0; i < touches.Count; i++)
			{
				dfTouchInfo dfTouchInfo = touches[i];
				dfControl dfControl = dfGUIManager.HitTestAll(dfTouchInfo.position);
				if (dfControl != null && dfControl.transform.IsChildOf(this.manager.transform))
				{
					dfInputManager.controlUnderMouse = dfControl;
				}
				if (dfInputManager.controlUnderMouse == null && dfTouchInfo.phase == TouchPhase.Began)
				{
					if (!retainFocusSetting && this.untracked.Count == 0)
					{
						dfControl activeControl = dfGUIManager.ActiveControl;
						if (activeControl != null && activeControl.transform.IsChildOf(this.manager.transform))
						{
							activeControl.Unfocus();
						}
					}
					this.untracked.Add(dfTouchInfo.fingerId);
				}
				else if (this.untracked.Contains(dfTouchInfo.fingerId))
				{
					if (dfTouchInfo.phase == TouchPhase.Ended)
					{
						this.untracked.Remove(dfTouchInfo.fingerId);
					}
				}
				else
				{
					Ray ray = renderCamera.ScreenPointToRay(dfTouchInfo.position);
					dfInputManager.TouchInputManager.TouchRaycast info = new dfInputManager.TouchInputManager.TouchRaycast(dfInputManager.controlUnderMouse, dfTouchInfo, ray);
					dfInputManager.TouchInputManager.ControlTouchTracker controlTouchTracker = this.tracked.FirstOrDefault((dfInputManager.TouchInputManager.ControlTouchTracker x) => x.IsTrackingFinger(info.FingerID));
					if (controlTouchTracker != null)
					{
						controlTouchTracker.Process(info);
					}
					else
					{
						bool flag = false;
						for (int j = 0; j < this.tracked.Count; j++)
						{
							if (this.tracked[j].Process(info))
							{
								flag = true;
								break;
							}
						}
						if (!flag && dfInputManager.controlUnderMouse != null)
						{
							if (!this.tracked.Any((dfInputManager.TouchInputManager.ControlTouchTracker x) => x.control == dfInputManager.controlUnderMouse))
							{
								dfInputManager.TouchInputManager.ControlTouchTracker controlTouchTracker2 = new dfInputManager.TouchInputManager.ControlTouchTracker(this.manager, dfInputManager.controlUnderMouse);
								this.tracked.Add(controlTouchTracker2);
								controlTouchTracker2.Process(info);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0005966C File Offset: 0x0005786C
		private dfControl clipCast(Transform transform, RaycastHit[] hits)
		{
			if (hits == null || hits.Length == 0)
			{
				return null;
			}
			dfControl dfControl = null;
			dfControl modalControl = dfGUIManager.GetModalControl();
			for (int i = hits.Length - 1; i >= 0; i--)
			{
				RaycastHit raycastHit = hits[i];
				dfControl component = raycastHit.transform.GetComponent<dfControl>();
				bool flag = component == null || (modalControl != null && !component.transform.IsChildOf(modalControl.transform)) || !component.enabled || !component.IsEnabled || !component.IsVisible || !component.transform.IsChildOf(transform);
				if (!flag)
				{
					if (this.isInsideClippingRegion(raycastHit, component) && (dfControl == null || component.RenderOrder > dfControl.RenderOrder))
					{
						dfControl = component;
					}
				}
			}
			return dfControl;
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x00059768 File Offset: 0x00057968
		private bool isInsideClippingRegion(RaycastHit hit, dfControl control)
		{
			Vector3 point = hit.point;
			while (control != null)
			{
				Plane[] array = ((!control.ClipChildren) ? null : control.GetClippingPlanes());
				if (array != null && array.Length > 0)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (!array[i].GetSide(point))
						{
							return false;
						}
					}
				}
				control = control.Parent;
			}
			return true;
		}

		// Token: 0x040010B3 RID: 4275
		private List<dfInputManager.TouchInputManager.ControlTouchTracker> tracked = new List<dfInputManager.TouchInputManager.ControlTouchTracker>();

		// Token: 0x040010B4 RID: 4276
		private List<int> untracked = new List<int>();

		// Token: 0x040010B5 RID: 4277
		private dfInputManager manager;

		// Token: 0x020003D3 RID: 979
		private class ControlTouchTracker
		{
			// Token: 0x0600136E RID: 4974 RVA: 0x000597FC File Offset: 0x000579FC
			public ControlTouchTracker(dfInputManager manager, dfControl control)
			{
				this.manager = manager;
				this.control = control;
				this.controlStartPosition = control.transform.position;
			}

			// Token: 0x1700043B RID: 1083
			// (get) Token: 0x0600136F RID: 4975 RVA: 0x0005983C File Offset: 0x00057A3C
			public bool IsDragging
			{
				get
				{
					return this.dragState == dfDragDropState.Dragging;
				}
			}

			// Token: 0x1700043C RID: 1084
			// (get) Token: 0x06001370 RID: 4976 RVA: 0x00059848 File Offset: 0x00057A48
			public int TouchCount
			{
				get
				{
					return this.touches.Count;
				}
			}

			// Token: 0x06001371 RID: 4977 RVA: 0x00059858 File Offset: 0x00057A58
			public bool IsTrackingFinger(int fingerID)
			{
				return this.touches.ContainsKey(fingerID);
			}

			// Token: 0x06001372 RID: 4978 RVA: 0x00059868 File Offset: 0x00057A68
			public bool Process(dfInputManager.TouchInputManager.TouchRaycast info)
			{
				if (this.IsDragging)
				{
					if (!this.capture.Contains(info.FingerID))
					{
						return false;
					}
					if (info.Phase == TouchPhase.Stationary)
					{
						return true;
					}
					if (info.Phase == TouchPhase.Canceled)
					{
						this.control.OnDragEnd(new dfDragEventArgs(this.control, dfDragDropState.Cancelled, this.dragData, info.ray, info.position));
						this.dragState = dfDragDropState.None;
						this.touches.Clear();
						this.capture.Clear();
						return true;
					}
					if (info.Phase != TouchPhase.Ended)
					{
						return true;
					}
					if (info.control == null || info.control == this.control)
					{
						this.control.OnDragEnd(new dfDragEventArgs(this.control, dfDragDropState.CancelledNoTarget, this.dragData, info.ray, info.position));
						this.dragState = dfDragDropState.None;
						this.touches.Clear();
						this.capture.Clear();
						return true;
					}
					dfDragEventArgs dfDragEventArgs = new dfDragEventArgs(info.control, dfDragDropState.Dragging, this.dragData, info.ray, info.position);
					info.control.OnDragDrop(dfDragEventArgs);
					if (!dfDragEventArgs.Used || dfDragEventArgs.State != dfDragDropState.Dropped)
					{
						dfDragEventArgs.State = dfDragDropState.Cancelled;
					}
					dfDragEventArgs dfDragEventArgs2 = new dfDragEventArgs(this.control, dfDragEventArgs.State, this.dragData, info.ray, info.position);
					dfDragEventArgs2.Target = info.control;
					this.control.OnDragEnd(dfDragEventArgs2);
					this.dragState = dfDragDropState.None;
					this.touches.Clear();
					this.capture.Clear();
					return true;
				}
				else if (!this.touches.ContainsKey(info.FingerID))
				{
					if (info.control != this.control)
					{
						return false;
					}
					this.touches[info.FingerID] = info;
					if (this.touches.Count == 1)
					{
						this.control.OnMouseEnter(info);
						if (info.Phase == TouchPhase.Began)
						{
							this.capture.Add(info.FingerID);
							this.controlStartPosition = this.control.transform.position;
							this.control.OnMouseDown(info);
							if (Event.current != null)
							{
								Event.current.Use();
							}
						}
						return true;
					}
					if (info.Phase == TouchPhase.Began)
					{
						List<dfTouchInfo> activeTouches = this.getActiveTouches();
						dfTouchEventArgs dfTouchEventArgs = new dfTouchEventArgs(this.control, activeTouches, info.ray);
						this.control.OnMultiTouch(dfTouchEventArgs);
					}
					return true;
				}
				else
				{
					if (info.Phase == TouchPhase.Canceled || info.Phase == TouchPhase.Ended)
					{
						TouchPhase phase = info.Phase;
						dfInputManager.TouchInputManager.TouchRaycast touchRaycast = this.touches[info.FingerID];
						this.touches.Remove(info.FingerID);
						if (this.touches.Count == 0 && phase != TouchPhase.Canceled)
						{
							if (this.capture.Contains(info.FingerID))
							{
								if (this.canFireClickEvent(info, touchRaycast) && info.control == this.control)
								{
									if (info.touch.tapCount > 1)
									{
										this.control.OnDoubleClick(info);
									}
									else
									{
										this.control.OnClick(info);
									}
								}
								info.control = this.control;
								if (this.control)
								{
									this.control.OnMouseUp(info);
								}
							}
							this.capture.Remove(info.FingerID);
							return true;
						}
						this.capture.Remove(info.FingerID);
						if (this.touches.Count == 1)
						{
							this.control.OnMultiTouchEnd();
							return true;
						}
					}
					if (this.touches.Count > 1)
					{
						List<dfTouchInfo> activeTouches2 = this.getActiveTouches();
						dfTouchEventArgs dfTouchEventArgs2 = new dfTouchEventArgs(this.control, activeTouches2, info.ray);
						this.control.OnMultiTouch(dfTouchEventArgs2);
						return true;
					}
					if (!this.IsDragging && info.Phase == TouchPhase.Stationary)
					{
						if (info.control == this.control)
						{
							this.control.OnMouseHover(info);
							return true;
						}
						return false;
					}
					else
					{
						bool flag = this.capture.Contains(info.FingerID) && this.dragState == dfDragDropState.None && info.Phase == TouchPhase.Moved;
						if (flag)
						{
							dfDragEventArgs dfDragEventArgs3 = info;
							this.control.OnDragStart(dfDragEventArgs3);
							if (dfDragEventArgs3.State == dfDragDropState.Dragging && dfDragEventArgs3.Used)
							{
								this.dragState = dfDragDropState.Dragging;
								this.dragData = dfDragEventArgs3.Data;
								return true;
							}
							this.dragState = dfDragDropState.Denied;
						}
						if (info.control != this.control && !this.capture.Contains(info.FingerID))
						{
							info.control = this.control;
							this.control.OnMouseLeave(info);
							this.touches.Remove(info.FingerID);
							return true;
						}
						info.control = this.control;
						this.control.OnMouseMove(info);
						return true;
					}
				}
			}

			// Token: 0x06001373 RID: 4979 RVA: 0x00059DC4 File Offset: 0x00057FC4
			private bool canFireClickEvent(dfInputManager.TouchInputManager.TouchRaycast info, dfInputManager.TouchInputManager.TouchRaycast touch)
			{
				if (this.control == null)
				{
					return false;
				}
				float num = this.control.PixelsToUnits();
				Vector3 vector = this.controlStartPosition / num;
				Vector3 vector2 = this.control.transform.position / num;
				return Vector3.Distance(vector, vector2) <= 1f;
			}

			// Token: 0x06001374 RID: 4980 RVA: 0x00059E28 File Offset: 0x00058028
			private List<dfTouchInfo> getActiveTouches()
			{
				dfInputManager.TouchInputManager.ControlTouchTracker.<getActiveTouches>c__AnonStorey0 <getActiveTouches>c__AnonStorey = new dfInputManager.TouchInputManager.ControlTouchTracker.<getActiveTouches>c__AnonStorey0();
				IList<dfTouchInfo> list = this.manager.touchInputSource.Touches;
				<getActiveTouches>c__AnonStorey.result = this.touches.Select((KeyValuePair<int, dfInputManager.TouchInputManager.TouchRaycast> x) => x.Value.touch).ToList<dfTouchInfo>();
				int i = 0;
				while (i < <getActiveTouches>c__AnonStorey.result.Count)
				{
					bool flag = false;
					int num = 0;
					while (i < list.Count)
					{
						if (list[num].fingerId == <getActiveTouches>c__AnonStorey.result[i].fingerId)
						{
							flag = true;
							break;
						}
						num++;
					}
					if (flag)
					{
						<getActiveTouches>c__AnonStorey.result[i] = list.First((dfTouchInfo x) => x.fingerId == <getActiveTouches>c__AnonStorey.result[i].fingerId);
						i++;
					}
					else
					{
						<getActiveTouches>c__AnonStorey.result.RemoveAt(i);
					}
				}
				return <getActiveTouches>c__AnonStorey.result;
			}

			// Token: 0x040010B7 RID: 4279
			public readonly dfControl control;

			// Token: 0x040010B8 RID: 4280
			public readonly Dictionary<int, dfInputManager.TouchInputManager.TouchRaycast> touches = new Dictionary<int, dfInputManager.TouchInputManager.TouchRaycast>();

			// Token: 0x040010B9 RID: 4281
			public readonly List<int> capture = new List<int>();

			// Token: 0x040010BA RID: 4282
			private dfInputManager manager;

			// Token: 0x040010BB RID: 4283
			private Vector3 controlStartPosition;

			// Token: 0x040010BC RID: 4284
			private dfDragDropState dragState;

			// Token: 0x040010BD RID: 4285
			private object dragData;
		}

		// Token: 0x020003D6 RID: 982
		private class TouchRaycast
		{
			// Token: 0x06001379 RID: 4985 RVA: 0x00059FB0 File Offset: 0x000581B0
			public TouchRaycast(dfControl control, dfTouchInfo touch, Ray ray)
			{
				this.control = control;
				this.touch = touch;
				this.ray = ray;
				this.position = touch.position;
			}

			// Token: 0x1700043D RID: 1085
			// (get) Token: 0x0600137A RID: 4986 RVA: 0x00059FDC File Offset: 0x000581DC
			public int FingerID
			{
				get
				{
					return this.touch.fingerId;
				}
			}

			// Token: 0x1700043E RID: 1086
			// (get) Token: 0x0600137B RID: 4987 RVA: 0x00059FEC File Offset: 0x000581EC
			public TouchPhase Phase
			{
				get
				{
					return this.touch.phase;
				}
			}

			// Token: 0x0600137C RID: 4988 RVA: 0x00059FFC File Offset: 0x000581FC
			public static implicit operator dfTouchEventArgs(dfInputManager.TouchInputManager.TouchRaycast touch)
			{
				return new dfTouchEventArgs(touch.control, touch.touch, touch.ray);
			}

			// Token: 0x0600137D RID: 4989 RVA: 0x0005A024 File Offset: 0x00058224
			public static implicit operator dfDragEventArgs(dfInputManager.TouchInputManager.TouchRaycast touch)
			{
				return new dfDragEventArgs(touch.control, dfDragDropState.None, null, touch.ray, touch.position);
			}

			// Token: 0x040010C2 RID: 4290
			public dfControl control;

			// Token: 0x040010C3 RID: 4291
			public dfTouchInfo touch;

			// Token: 0x040010C4 RID: 4292
			public Ray ray;

			// Token: 0x040010C5 RID: 4293
			public Vector2 position;
		}
	}

	// Token: 0x020003D8 RID: 984
	private class MouseInputManager
	{
		// Token: 0x06001381 RID: 4993 RVA: 0x0005A090 File Offset: 0x00058290
		public void ProcessInput(dfInputManager manager, IInputAdapter adapter, Ray ray, dfControl control, bool retainFocusSetting)
		{
			Vector2 mousePosition = adapter.GetMousePosition();
			this.buttonsDown = dfMouseButtons.None;
			this.buttonsReleased = dfMouseButtons.None;
			this.buttonsPressed = dfMouseButtons.None;
			dfInputManager.MouseInputManager.getMouseButtonInfo(adapter, ref this.buttonsDown, ref this.buttonsReleased, ref this.buttonsPressed);
			float num = adapter.GetAxis("Mouse ScrollWheel");
			if (!Mathf.Approximately(num, 0f))
			{
				num = Mathf.Sign(num) * Mathf.Max(1f, Mathf.Abs(num));
			}
			this.mouseMoveDelta = mousePosition - this.lastPosition;
			this.lastPosition = mousePosition;
			if (this.dragState == dfDragDropState.Dragging)
			{
				if (this.buttonsReleased != dfMouseButtons.None)
				{
					if (control != null && control != this.activeControl)
					{
						dfDragEventArgs dfDragEventArgs = new dfDragEventArgs(control, dfDragDropState.Dragging, this.dragData, ray, mousePosition);
						control.OnDragDrop(dfDragEventArgs);
						if (!dfDragEventArgs.Used || dfDragEventArgs.State == dfDragDropState.Dragging)
						{
							dfDragEventArgs.State = dfDragDropState.Cancelled;
						}
						dfDragEventArgs = new dfDragEventArgs(this.activeControl, dfDragEventArgs.State, dfDragEventArgs.Data, ray, mousePosition);
						dfDragEventArgs.Target = control;
						this.activeControl.OnDragEnd(dfDragEventArgs);
					}
					else
					{
						dfDragDropState dfDragDropState = ((!(control == null)) ? dfDragDropState.Cancelled : dfDragDropState.CancelledNoTarget);
						dfDragEventArgs dfDragEventArgs2 = new dfDragEventArgs(this.activeControl, dfDragDropState, this.dragData, ray, mousePosition);
						this.activeControl.OnDragEnd(dfDragEventArgs2);
					}
					this.dragState = dfDragDropState.None;
					this.lastDragControl = null;
					this.activeControl = null;
					this.lastClickTime = 0f;
					this.lastHoverTime = 0f;
					this.lastPosition = mousePosition;
					return;
				}
				if (control == this.activeControl)
				{
					return;
				}
				if (control != this.lastDragControl)
				{
					if (this.lastDragControl != null)
					{
						dfDragEventArgs dfDragEventArgs3 = new dfDragEventArgs(this.lastDragControl, this.dragState, this.dragData, ray, mousePosition);
						this.lastDragControl.OnDragLeave(dfDragEventArgs3);
					}
					if (control != null)
					{
						dfDragEventArgs dfDragEventArgs4 = new dfDragEventArgs(control, this.dragState, this.dragData, ray, mousePosition);
						control.OnDragEnter(dfDragEventArgs4);
					}
					this.lastDragControl = control;
					return;
				}
				if (control != null && this.mouseMoveDelta.magnitude > 1f)
				{
					dfDragEventArgs dfDragEventArgs5 = new dfDragEventArgs(control, this.dragState, this.dragData, ray, mousePosition);
					control.OnDragOver(dfDragEventArgs5);
				}
				return;
			}
			else
			{
				if (this.buttonsPressed != dfMouseButtons.None)
				{
					this.lastHoverTime = Time.realtimeSinceStartup + manager.hoverStartDelay;
					if (this.activeControl != null)
					{
						if (this.activeControl.transform.IsChildOf(manager.transform))
						{
							this.activeControl.OnMouseDown(new dfMouseEventArgs(this.activeControl, this.buttonsPressed, 0, ray, mousePosition, num));
						}
					}
					else if (control == null || control.transform.IsChildOf(manager.transform))
					{
						this.setActive(manager, control, mousePosition, ray);
						if (control != null)
						{
							dfGUIManager.SetFocus(control, true);
							control.OnMouseDown(new dfMouseEventArgs(control, this.buttonsPressed, 0, ray, mousePosition, num));
						}
						else if (!retainFocusSetting)
						{
							dfControl dfControl = dfGUIManager.ActiveControl;
							if (dfControl != null && dfControl.transform.IsChildOf(manager.transform))
							{
								dfControl.Unfocus();
							}
						}
					}
					if (this.buttonsReleased == dfMouseButtons.None)
					{
						return;
					}
				}
				if (this.buttonsReleased == dfMouseButtons.None)
				{
					if (this.activeControl != null && this.activeControl == control && this.mouseMoveDelta.magnitude == 0f && Time.realtimeSinceStartup - this.lastHoverTime > manager.hoverNotifactionFrequency)
					{
						this.activeControl.OnMouseHover(new dfMouseEventArgs(this.activeControl, this.buttonsDown, 0, ray, mousePosition, num));
						this.lastHoverTime = Time.realtimeSinceStartup;
					}
					if (this.buttonsDown == dfMouseButtons.None)
					{
						if (num != 0f && control != null)
						{
							this.setActive(manager, control, mousePosition, ray);
							control.OnMouseWheel(new dfMouseEventArgs(control, this.buttonsDown, 0, ray, mousePosition, num));
							return;
						}
						this.setActive(manager, control, mousePosition, ray);
					}
					else if (this.buttonsDown != dfMouseButtons.None && this.activeControl != null)
					{
						if (!(control != null) || control.RenderOrder > this.activeControl.RenderOrder)
						{
						}
						if (this.mouseMoveDelta.magnitude >= 2f && (this.buttonsDown & (dfMouseButtons.Left | dfMouseButtons.Right)) != dfMouseButtons.None && this.dragState != dfDragDropState.Denied)
						{
							dfDragEventArgs dfDragEventArgs6 = new dfDragEventArgs(this.activeControl)
							{
								Position = mousePosition
							};
							this.activeControl.OnDragStart(dfDragEventArgs6);
							if (dfDragEventArgs6.State == dfDragDropState.Dragging)
							{
								this.dragState = dfDragDropState.Dragging;
								this.dragData = dfDragEventArgs6.Data;
								return;
							}
							this.dragState = dfDragDropState.Denied;
						}
					}
					if (this.activeControl != null && this.mouseMoveDelta.magnitude >= 1f)
					{
						dfMouseEventArgs dfMouseEventArgs = new dfMouseEventArgs(this.activeControl, this.buttonsDown, 0, ray, mousePosition, num)
						{
							MoveDelta = this.mouseMoveDelta
						};
						this.activeControl.OnMouseMove(dfMouseEventArgs);
					}
					return;
				}
				this.lastHoverTime = Time.realtimeSinceStartup + manager.hoverStartDelay;
				if (this.activeControl == null)
				{
					this.setActive(manager, control, mousePosition, ray);
					return;
				}
				if (this.activeControl == control && this.buttonsDown == dfMouseButtons.None)
				{
					float num2 = this.activeControl.PixelsToUnits();
					Vector3 vector = this.activeControlPosition / num2;
					Vector3 vector2 = this.activeControl.transform.position / num2;
					if (Vector3.Distance(vector, vector2) <= 1f)
					{
						if (Time.realtimeSinceStartup - this.lastClickTime < 0.25f)
						{
							this.lastClickTime = 0f;
							this.activeControl.OnDoubleClick(new dfMouseEventArgs(this.activeControl, this.buttonsReleased, 1, ray, mousePosition, num));
						}
						else
						{
							this.lastClickTime = Time.realtimeSinceStartup;
							this.activeControl.OnClick(new dfMouseEventArgs(this.activeControl, this.buttonsReleased, 1, ray, mousePosition, num));
						}
					}
				}
				this.activeControl.OnMouseUp(new dfMouseEventArgs(this.activeControl, this.buttonsReleased, 0, ray, mousePosition, num));
				if (this.buttonsDown == dfMouseButtons.None && this.activeControl != control)
				{
					this.setActive(manager, null, mousePosition, ray);
				}
				return;
			}
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0005A760 File Offset: 0x00058960
		private static void getMouseButtonInfo(IInputAdapter adapter, ref dfMouseButtons buttonsDown, ref dfMouseButtons buttonsReleased, ref dfMouseButtons buttonsPressed)
		{
			for (int i = 0; i < 3; i++)
			{
				if (adapter.GetMouseButton(i))
				{
					buttonsDown |= (dfMouseButtons)(1 << i);
				}
				if (adapter.GetMouseButtonUp(i))
				{
					buttonsReleased |= (dfMouseButtons)(1 << i);
				}
				if (adapter.GetMouseButtonDown(i))
				{
					buttonsPressed |= (dfMouseButtons)(1 << i);
				}
			}
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0005A7C4 File Offset: 0x000589C4
		private void setActive(dfInputManager manager, dfControl control, Vector2 position, Ray ray)
		{
			if (this.activeControl != null && this.activeControl != control)
			{
				this.activeControl.OnMouseLeave(new dfMouseEventArgs(this.activeControl)
				{
					Position = position,
					Ray = ray
				});
			}
			if (control != null && control != this.activeControl)
			{
				this.lastClickTime = 0f;
				this.lastHoverTime = Time.realtimeSinceStartup + manager.hoverStartDelay;
				control.OnMouseEnter(new dfMouseEventArgs(control)
				{
					Position = position,
					Ray = ray
				});
			}
			this.activeControl = control;
			this.activeControlPosition = ((!(control != null)) ? (Vector3.one * float.MinValue) : control.transform.position);
			this.lastPosition = position;
			this.dragState = dfDragDropState.None;
		}

		// Token: 0x040010C7 RID: 4295
		private const string scrollAxisName = "Mouse ScrollWheel";

		// Token: 0x040010C8 RID: 4296
		private const float DOUBLECLICK_TIME = 0.25f;

		// Token: 0x040010C9 RID: 4297
		private const int DRAG_START_DELTA = 2;

		// Token: 0x040010CA RID: 4298
		private dfControl activeControl;

		// Token: 0x040010CB RID: 4299
		private Vector3 activeControlPosition;

		// Token: 0x040010CC RID: 4300
		private Vector2 lastPosition = Vector2.one * -2.1474836E+09f;

		// Token: 0x040010CD RID: 4301
		private Vector2 mouseMoveDelta = Vector2.zero;

		// Token: 0x040010CE RID: 4302
		private float lastClickTime;

		// Token: 0x040010CF RID: 4303
		private float lastHoverTime;

		// Token: 0x040010D0 RID: 4304
		private dfDragDropState dragState;

		// Token: 0x040010D1 RID: 4305
		private object dragData;

		// Token: 0x040010D2 RID: 4306
		private dfControl lastDragControl;

		// Token: 0x040010D3 RID: 4307
		private dfMouseButtons buttonsDown;

		// Token: 0x040010D4 RID: 4308
		private dfMouseButtons buttonsReleased;

		// Token: 0x040010D5 RID: 4309
		private dfMouseButtons buttonsPressed;
	}

	// Token: 0x020003D9 RID: 985
	private class DefaultInput : IInputAdapter
	{
		// Token: 0x06001384 RID: 4996 RVA: 0x0005A8B8 File Offset: 0x00058AB8
		public DefaultInput(Camera renderCam)
		{
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x0005A8C0 File Offset: 0x00058AC0
		public bool GetKeyDown(KeyCode key)
		{
			return Input.GetKeyDown(key);
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x0005A8C8 File Offset: 0x00058AC8
		public bool GetKeyUp(KeyCode key)
		{
			return Input.GetKeyUp(key);
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0005A8D0 File Offset: 0x00058AD0
		public float GetAxis(string axisName)
		{
			return Input.GetAxis(axisName);
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x0005A8D8 File Offset: 0x00058AD8
		public Vector2 GetMousePosition()
		{
			return Input.mousePosition;
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0005A8E4 File Offset: 0x00058AE4
		public bool GetMouseButton(int button)
		{
			return Input.GetMouseButton(button);
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0005A8EC File Offset: 0x00058AEC
		public bool GetMouseButtonDown(int button)
		{
			return Input.GetMouseButtonDown(button);
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0005A8F4 File Offset: 0x00058AF4
		public bool GetMouseButtonUp(int button)
		{
			return Input.GetMouseButtonUp(button);
		}
	}
}
