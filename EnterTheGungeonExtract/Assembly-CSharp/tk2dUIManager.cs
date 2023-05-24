using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C1E RID: 3102
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIManager")]
public class tk2dUIManager : MonoBehaviour
{
	// Token: 0x17000A16 RID: 2582
	// (get) Token: 0x060042B1 RID: 17073 RVA: 0x00159284 File Offset: 0x00157484
	public static tk2dUIManager Instance
	{
		get
		{
			if (tk2dUIManager.instance == null)
			{
				tk2dUIManager.instance = UnityEngine.Object.FindObjectOfType(typeof(tk2dUIManager)) as tk2dUIManager;
				if (tk2dUIManager.instance == null)
				{
					GameObject gameObject = new GameObject("tk2dUIManager");
					tk2dUIManager.instance = gameObject.AddComponent<tk2dUIManager>();
				}
			}
			return tk2dUIManager.instance;
		}
	}

	// Token: 0x17000A17 RID: 2583
	// (get) Token: 0x060042B2 RID: 17074 RVA: 0x001592E8 File Offset: 0x001574E8
	public static tk2dUIManager Instance__NoCreate
	{
		get
		{
			return tk2dUIManager.instance;
		}
	}

	// Token: 0x17000A18 RID: 2584
	// (get) Token: 0x060042B3 RID: 17075 RVA: 0x001592F0 File Offset: 0x001574F0
	// (set) Token: 0x060042B4 RID: 17076 RVA: 0x001592F8 File Offset: 0x001574F8
	public Camera UICamera
	{
		get
		{
			return this.uiCamera;
		}
		set
		{
			this.uiCamera = value;
		}
	}

	// Token: 0x060042B5 RID: 17077 RVA: 0x00159304 File Offset: 0x00157504
	public Camera GetUICameraForControl(GameObject go)
	{
		int num = 1 << go.layer;
		int count = tk2dUIManager.allCameras.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dUICamera tk2dUICamera = tk2dUIManager.allCameras[i];
			if ((tk2dUICamera.FilteredMask & num) != 0)
			{
				return tk2dUICamera.HostCamera;
			}
		}
		UnityEngine.Debug.LogError("Unable to find UI camera for " + go.name);
		return null;
	}

	// Token: 0x060042B6 RID: 17078 RVA: 0x00159378 File Offset: 0x00157578
	public static void RegisterCamera(tk2dUICamera cam)
	{
		tk2dUIManager.allCameras.Add(cam);
	}

	// Token: 0x060042B7 RID: 17079 RVA: 0x00159388 File Offset: 0x00157588
	public static void UnregisterCamera(tk2dUICamera cam)
	{
		tk2dUIManager.allCameras.Remove(cam);
	}

	// Token: 0x17000A19 RID: 2585
	// (get) Token: 0x060042B8 RID: 17080 RVA: 0x00159398 File Offset: 0x00157598
	// (set) Token: 0x060042B9 RID: 17081 RVA: 0x001593A0 File Offset: 0x001575A0
	public bool InputEnabled
	{
		get
		{
			return this.inputEnabled;
		}
		set
		{
			if (this.inputEnabled && !value)
			{
				this.SortCameras();
				this.inputEnabled = value;
				if (this.useMultiTouch)
				{
					this.CheckMultiTouchInputs();
				}
				else
				{
					this.CheckInputs();
				}
			}
			else
			{
				this.inputEnabled = value;
			}
		}
	}

	// Token: 0x17000A1A RID: 2586
	// (get) Token: 0x060042BA RID: 17082 RVA: 0x001593F4 File Offset: 0x001575F4
	public tk2dUIItem PressedUIItem
	{
		get
		{
			if (!this.useMultiTouch)
			{
				return this.pressedUIItem;
			}
			if (this.pressedUIItems.Length > 0)
			{
				return this.pressedUIItems[this.pressedUIItems.Length - 1];
			}
			return null;
		}
	}

	// Token: 0x17000A1B RID: 2587
	// (get) Token: 0x060042BB RID: 17083 RVA: 0x0015942C File Offset: 0x0015762C
	public tk2dUIItem[] PressedUIItems
	{
		get
		{
			return this.pressedUIItems;
		}
	}

	// Token: 0x17000A1C RID: 2588
	// (get) Token: 0x060042BC RID: 17084 RVA: 0x00159434 File Offset: 0x00157634
	// (set) Token: 0x060042BD RID: 17085 RVA: 0x0015943C File Offset: 0x0015763C
	public bool UseMultiTouch
	{
		get
		{
			return this.useMultiTouch;
		}
		set
		{
			if (this.useMultiTouch != value && this.inputEnabled)
			{
				this.InputEnabled = false;
				this.useMultiTouch = value;
				this.InputEnabled = true;
			}
			else
			{
				this.useMultiTouch = value;
			}
		}
	}

	// Token: 0x1400009F RID: 159
	// (add) Token: 0x060042BE RID: 17086 RVA: 0x00159478 File Offset: 0x00157678
	// (remove) Token: 0x060042BF RID: 17087 RVA: 0x001594B0 File Offset: 0x001576B0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnAnyPress;

	// Token: 0x140000A0 RID: 160
	// (add) Token: 0x060042C0 RID: 17088 RVA: 0x001594E8 File Offset: 0x001576E8
	// (remove) Token: 0x060042C1 RID: 17089 RVA: 0x00159520 File Offset: 0x00157720
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnInputUpdate;

	// Token: 0x140000A1 RID: 161
	// (add) Token: 0x060042C2 RID: 17090 RVA: 0x00159558 File Offset: 0x00157758
	// (remove) Token: 0x060042C3 RID: 17091 RVA: 0x00159590 File Offset: 0x00157790
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<float> OnScrollWheelChange;

	// Token: 0x060042C4 RID: 17092 RVA: 0x001595C8 File Offset: 0x001577C8
	private void SortCameras()
	{
		this.sortedCameras.Clear();
		int count = tk2dUIManager.allCameras.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dUICamera tk2dUICamera = tk2dUIManager.allCameras[i];
			if (tk2dUICamera != null)
			{
				this.sortedCameras.Add(tk2dUICamera);
			}
		}
		this.sortedCameras.Sort((tk2dUICamera a, tk2dUICamera b) => b.GetComponent<Camera>().depth.CompareTo(a.GetComponent<Camera>().depth));
	}

	// Token: 0x060042C5 RID: 17093 RVA: 0x0015964C File Offset: 0x0015784C
	private void Awake()
	{
		if (tk2dUIManager.instance == null)
		{
			tk2dUIManager.instance = this;
			if (tk2dUIManager.instance.transform.childCount != 0)
			{
				UnityEngine.Debug.LogError("You should not attach anything to the tk2dUIManager object. The tk2dUIManager will not get destroyed between scene switches and any children will persist as well.");
			}
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}
		else if (tk2dUIManager.instance != this)
		{
			UnityEngine.Debug.Log("Discarding unnecessary tk2dUIManager instance.");
			if (this.uiCamera != null)
			{
				this.HookUpLegacyCamera(this.uiCamera);
				this.uiCamera = null;
			}
			UnityEngine.Object.Destroy(this);
			return;
		}
		tk2dUITime.Init();
		this.Setup();
	}

	// Token: 0x060042C6 RID: 17094 RVA: 0x001596F8 File Offset: 0x001578F8
	private void HookUpLegacyCamera(Camera cam)
	{
		if (cam.GetComponent<tk2dUICamera>() == null)
		{
			tk2dUICamera tk2dUICamera = cam.gameObject.AddComponent<tk2dUICamera>();
			tk2dUICamera.AssignRaycastLayerMask(this.raycastLayerMask);
		}
	}

	// Token: 0x060042C7 RID: 17095 RVA: 0x00159730 File Offset: 0x00157930
	private void Start()
	{
		if (this.uiCamera != null)
		{
			UnityEngine.Debug.Log("It is no longer necessary to hook up a camera to the tk2dUIManager. You can simply attach a tk2dUICamera script to the cameras that interact with UI.");
			this.HookUpLegacyCamera(this.uiCamera);
			this.uiCamera = null;
		}
		if (tk2dUIManager.allCameras.Count == 0)
		{
			UnityEngine.Debug.LogError("Unable to find any tk2dUICameras, and no cameras are connected to the tk2dUIManager. You will not be able to interact with the UI.");
		}
	}

	// Token: 0x060042C8 RID: 17096 RVA: 0x00159784 File Offset: 0x00157984
	private void Setup()
	{
		if (!this.areHoverEventsTracked)
		{
			this.checkForHovers = false;
		}
	}

	// Token: 0x060042C9 RID: 17097 RVA: 0x00159798 File Offset: 0x00157998
	private void Update()
	{
		tk2dUITime.Update();
		if (this.inputEnabled)
		{
			this.SortCameras();
			if (this.useMultiTouch)
			{
				this.CheckMultiTouchInputs();
			}
			else
			{
				this.CheckInputs();
			}
			if (this.OnInputUpdate != null)
			{
				this.OnInputUpdate();
			}
			if (this.OnScrollWheelChange != null)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (axis != 0f)
				{
					this.OnScrollWheelChange(axis);
				}
			}
		}
	}

	// Token: 0x060042CA RID: 17098 RVA: 0x0015981C File Offset: 0x00157A1C
	private void CheckInputs()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		this.primaryTouch = default(tk2dUITouch);
		this.secondaryTouch = default(tk2dUITouch);
		this.resultTouch = default(tk2dUITouch);
		this.hitUIItem = null;
		if (this.inputEnabled)
		{
			if (Input.touchCount > 0)
			{
				foreach (Touch touch in Input.touches)
				{
					if (touch.phase == TouchPhase.Began)
					{
						this.primaryTouch = new tk2dUITouch(touch);
						flag = true;
						flag3 = true;
					}
					else if (this.pressedUIItem != null && touch.fingerId == this.firstPressedUIItemTouch.fingerId)
					{
						this.secondaryTouch = new tk2dUITouch(touch);
						flag2 = true;
					}
				}
				this.checkForHovers = false;
			}
			else if (Input.GetMouseButtonDown(0))
			{
				this.primaryTouch = new tk2dUITouch(TouchPhase.Began, 9999, Input.mousePosition, Vector2.zero, 0f);
				flag = true;
				flag3 = true;
			}
			else if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
			{
				Vector2 vector = Vector2.zero;
				TouchPhase touchPhase = TouchPhase.Moved;
				if (this.pressedUIItem != null)
				{
					vector = this.firstPressedUIItemTouch.position - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				}
				if (Input.GetMouseButtonUp(0))
				{
					touchPhase = TouchPhase.Ended;
				}
				else if (vector == Vector2.zero)
				{
					touchPhase = TouchPhase.Stationary;
				}
				this.secondaryTouch = new tk2dUITouch(touchPhase, 9999, Input.mousePosition, vector, tk2dUITime.deltaTime);
				flag2 = true;
			}
		}
		if (flag)
		{
			this.resultTouch = this.primaryTouch;
		}
		else if (flag2)
		{
			this.resultTouch = this.secondaryTouch;
		}
		if (flag || flag2)
		{
			this.hitUIItem = this.RaycastForUIItem(this.resultTouch.position);
			if (this.resultTouch.phase == TouchPhase.Began)
			{
				if (this.pressedUIItem != null)
				{
					this.pressedUIItem.CurrentOverUIItem(this.hitUIItem);
					if (this.pressedUIItem != this.hitUIItem)
					{
						this.pressedUIItem.Release();
						this.pressedUIItem = null;
					}
					else
					{
						this.firstPressedUIItemTouch = this.resultTouch;
					}
				}
				if (this.hitUIItem != null)
				{
					this.hitUIItem.Press(this.resultTouch);
				}
				this.pressedUIItem = this.hitUIItem;
				this.firstPressedUIItemTouch = this.resultTouch;
			}
			else if (this.resultTouch.phase == TouchPhase.Ended)
			{
				if (this.pressedUIItem != null)
				{
					this.pressedUIItem.CurrentOverUIItem(this.hitUIItem);
					this.pressedUIItem.UpdateTouch(this.resultTouch);
					this.pressedUIItem.Release();
					this.pressedUIItem = null;
				}
			}
			else if (this.pressedUIItem != null)
			{
				this.pressedUIItem.CurrentOverUIItem(this.hitUIItem);
				this.pressedUIItem.UpdateTouch(this.resultTouch);
			}
		}
		else if (this.pressedUIItem != null)
		{
			this.pressedUIItem.CurrentOverUIItem(null);
			this.pressedUIItem.Release();
			this.pressedUIItem = null;
		}
		if (this.checkForHovers)
		{
			if (this.inputEnabled)
			{
				if (!flag && !flag2 && this.hitUIItem == null && !Input.GetMouseButton(0))
				{
					this.hitUIItem = this.RaycastForUIItem(Input.mousePosition);
				}
				else if (Input.GetMouseButton(0))
				{
					this.hitUIItem = null;
				}
			}
			if (this.hitUIItem != null)
			{
				if (this.hitUIItem.isHoverEnabled)
				{
					if (!this.hitUIItem.HoverOver(this.overUIItem) && this.overUIItem != null)
					{
						this.overUIItem.HoverOut(this.hitUIItem);
					}
					this.overUIItem = this.hitUIItem;
				}
				else if (this.overUIItem != null)
				{
					this.overUIItem.HoverOut(null);
				}
			}
			else if (this.overUIItem != null)
			{
				this.overUIItem.HoverOut(null);
			}
		}
		if (flag3 && this.OnAnyPress != null)
		{
			this.OnAnyPress();
		}
	}

	// Token: 0x060042CB RID: 17099 RVA: 0x00159CE8 File Offset: 0x00157EE8
	private void CheckMultiTouchInputs()
	{
		bool flag = false;
		this.touchCounter = 0;
		if (this.inputEnabled)
		{
			if (Input.touchCount > 0)
			{
				foreach (Touch touch in Input.touches)
				{
					if (this.touchCounter >= 5)
					{
						break;
					}
					this.allTouches[this.touchCounter] = new tk2dUITouch(touch);
					this.touchCounter++;
				}
			}
			else if (Input.GetMouseButtonDown(0))
			{
				this.allTouches[this.touchCounter] = new tk2dUITouch(TouchPhase.Began, 9999, Input.mousePosition, Vector2.zero, 0f);
				this.mouseDownFirstPos = Input.mousePosition;
				this.touchCounter++;
			}
			else if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
			{
				Vector2 vector = this.mouseDownFirstPos - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				TouchPhase touchPhase = TouchPhase.Moved;
				if (Input.GetMouseButtonUp(0))
				{
					touchPhase = TouchPhase.Ended;
				}
				else if (vector == Vector2.zero)
				{
					touchPhase = TouchPhase.Stationary;
				}
				this.allTouches[this.touchCounter] = new tk2dUITouch(touchPhase, 9999, Input.mousePosition, vector, tk2dUITime.deltaTime);
				this.touchCounter++;
			}
		}
		for (int j = 0; j < this.touchCounter; j++)
		{
			this.pressedUIItems[j] = this.RaycastForUIItem(this.allTouches[j].position);
		}
		for (int k = 0; k < this.prevPressedUIItemList.Count; k++)
		{
			this.prevPressedItem = this.prevPressedUIItemList[k];
			if (this.prevPressedItem != null)
			{
				int fingerId = this.prevPressedItem.Touch.fingerId;
				bool flag2 = false;
				for (int l = 0; l < this.touchCounter; l++)
				{
					this.currTouch = this.allTouches[l];
					if (this.currTouch.fingerId == fingerId)
					{
						flag2 = true;
						this.currPressedItem = this.pressedUIItems[l];
						if (this.currTouch.phase == TouchPhase.Began)
						{
							this.prevPressedItem.CurrentOverUIItem(this.currPressedItem);
							if (this.prevPressedItem != this.currPressedItem)
							{
								this.prevPressedItem.Release();
								this.prevPressedUIItemList.RemoveAt(k);
								k--;
							}
						}
						else if (this.currTouch.phase == TouchPhase.Ended)
						{
							this.prevPressedItem.CurrentOverUIItem(this.currPressedItem);
							this.prevPressedItem.UpdateTouch(this.currTouch);
							this.prevPressedItem.Release();
							this.prevPressedUIItemList.RemoveAt(k);
							k--;
						}
						else
						{
							this.prevPressedItem.CurrentOverUIItem(this.currPressedItem);
							this.prevPressedItem.UpdateTouch(this.currTouch);
						}
						break;
					}
				}
				if (!flag2)
				{
					this.prevPressedItem.CurrentOverUIItem(null);
					this.prevPressedItem.Release();
					this.prevPressedUIItemList.RemoveAt(k);
					k--;
				}
			}
		}
		for (int m = 0; m < this.touchCounter; m++)
		{
			this.currPressedItem = this.pressedUIItems[m];
			this.currTouch = this.allTouches[m];
			if (this.currTouch.phase == TouchPhase.Began)
			{
				if (this.currPressedItem != null)
				{
					bool flag3 = this.currPressedItem.Press(this.currTouch);
					if (flag3)
					{
						this.prevPressedUIItemList.Add(this.currPressedItem);
					}
				}
				flag = true;
			}
		}
		if (flag && this.OnAnyPress != null)
		{
			this.OnAnyPress();
		}
	}

	// Token: 0x060042CC RID: 17100 RVA: 0x0015A140 File Offset: 0x00158340
	private tk2dUIItem RaycastForUIItem(Vector2 screenPos)
	{
		int count = this.sortedCameras.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dUICamera tk2dUICamera = this.sortedCameras[i];
			this.ray = tk2dUICamera.HostCamera.ScreenPointToRay(screenPos);
			if (Physics.Raycast(this.ray, out this.hit, tk2dUICamera.HostCamera.farClipPlane, tk2dUICamera.FilteredMask))
			{
				return this.hit.collider.GetComponent<tk2dUIItem>();
			}
		}
		return null;
	}

	// Token: 0x060042CD RID: 17101 RVA: 0x0015A1D0 File Offset: 0x001583D0
	public void OverrideClearAllChildrenPresses(tk2dUIItem item)
	{
		if (this.useMultiTouch)
		{
			for (int i = 0; i < this.pressedUIItems.Length; i++)
			{
				tk2dUIItem tk2dUIItem = this.pressedUIItems[i];
				if (tk2dUIItem != null && item.CheckIsUIItemChildOfMe(tk2dUIItem))
				{
					tk2dUIItem.CurrentOverUIItem(item);
				}
			}
		}
		else if (this.pressedUIItem != null && item.CheckIsUIItemChildOfMe(this.pressedUIItem))
		{
			this.pressedUIItem.CurrentOverUIItem(item);
		}
	}

	// Token: 0x040034F5 RID: 13557
	public static double version = 1.0;

	// Token: 0x040034F6 RID: 13558
	public static int releaseId = 0;

	// Token: 0x040034F7 RID: 13559
	private static tk2dUIManager instance;

	// Token: 0x040034F8 RID: 13560
	[SerializeField]
	private Camera uiCamera;

	// Token: 0x040034F9 RID: 13561
	private static List<tk2dUICamera> allCameras = new List<tk2dUICamera>();

	// Token: 0x040034FA RID: 13562
	private List<tk2dUICamera> sortedCameras = new List<tk2dUICamera>();

	// Token: 0x040034FB RID: 13563
	public LayerMask raycastLayerMask = -1;

	// Token: 0x040034FC RID: 13564
	private bool inputEnabled = true;

	// Token: 0x040034FD RID: 13565
	public bool areHoverEventsTracked = true;

	// Token: 0x040034FE RID: 13566
	private tk2dUIItem pressedUIItem;

	// Token: 0x040034FF RID: 13567
	private tk2dUIItem overUIItem;

	// Token: 0x04003500 RID: 13568
	private tk2dUITouch firstPressedUIItemTouch;

	// Token: 0x04003501 RID: 13569
	private bool checkForHovers = true;

	// Token: 0x04003502 RID: 13570
	[SerializeField]
	private bool useMultiTouch;

	// Token: 0x04003503 RID: 13571
	private const int MAX_MULTI_TOUCH_COUNT = 5;

	// Token: 0x04003504 RID: 13572
	private tk2dUITouch[] allTouches = new tk2dUITouch[5];

	// Token: 0x04003505 RID: 13573
	private List<tk2dUIItem> prevPressedUIItemList = new List<tk2dUIItem>();

	// Token: 0x04003506 RID: 13574
	private tk2dUIItem[] pressedUIItems = new tk2dUIItem[5];

	// Token: 0x04003507 RID: 13575
	private int touchCounter;

	// Token: 0x04003508 RID: 13576
	private Vector2 mouseDownFirstPos = Vector2.zero;

	// Token: 0x04003509 RID: 13577
	private const string MOUSE_WHEEL_AXES_NAME = "Mouse ScrollWheel";

	// Token: 0x0400350A RID: 13578
	private tk2dUITouch primaryTouch = default(tk2dUITouch);

	// Token: 0x0400350B RID: 13579
	private tk2dUITouch secondaryTouch = default(tk2dUITouch);

	// Token: 0x0400350C RID: 13580
	private tk2dUITouch resultTouch = default(tk2dUITouch);

	// Token: 0x0400350D RID: 13581
	private tk2dUIItem hitUIItem;

	// Token: 0x0400350E RID: 13582
	private RaycastHit hit;

	// Token: 0x0400350F RID: 13583
	private Ray ray;

	// Token: 0x04003510 RID: 13584
	private tk2dUITouch currTouch;

	// Token: 0x04003511 RID: 13585
	private tk2dUIItem currPressedItem;

	// Token: 0x04003512 RID: 13586
	private tk2dUIItem prevPressedItem;
}
