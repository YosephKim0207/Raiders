using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace InControl
{
	// Token: 0x020006B5 RID: 1717
	[AddComponentMenu("Event/InControl Input Module")]
	public class InControlInputModule : PointerInputModule
	{
		// Token: 0x06002864 RID: 10340 RVA: 0x000AB3F4 File Offset: 0x000A95F4
		protected InControlInputModule()
		{
			this.direction = new TwoAxisInputControl();
			this.direction.StateThreshold = this.analogMoveThreshold;
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06002865 RID: 10341 RVA: 0x000AB45C File Offset: 0x000A965C
		// (set) Token: 0x06002866 RID: 10342 RVA: 0x000AB464 File Offset: 0x000A9664
		public PlayerAction SubmitAction { get; set; }

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06002867 RID: 10343 RVA: 0x000AB470 File Offset: 0x000A9670
		// (set) Token: 0x06002868 RID: 10344 RVA: 0x000AB478 File Offset: 0x000A9678
		public PlayerAction CancelAction { get; set; }

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06002869 RID: 10345 RVA: 0x000AB484 File Offset: 0x000A9684
		// (set) Token: 0x0600286A RID: 10346 RVA: 0x000AB48C File Offset: 0x000A968C
		public PlayerTwoAxisAction MoveAction { get; set; }

		// Token: 0x0600286B RID: 10347 RVA: 0x000AB498 File Offset: 0x000A9698
		public override void UpdateModule()
		{
			this.lastMousePosition = this.thisMousePosition;
			this.thisMousePosition = Input.mousePosition;
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x000AB4B4 File Offset: 0x000A96B4
		public override bool IsModuleSupported()
		{
			return this.forceModuleActive || Input.mousePresent || Input.touchSupported;
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x000AB4D8 File Offset: 0x000A96D8
		public override bool ShouldActivateModule()
		{
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return false;
			}
			this.UpdateInputState();
			bool flag = false;
			flag |= this.SubmitWasPressed;
			flag |= this.CancelWasPressed;
			flag |= this.VectorWasPressed;
			if (this.allowMouseInput)
			{
				flag |= this.MouseHasMoved;
				flag |= this.MouseButtonIsPressed;
			}
			if (Input.touchCount > 0)
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x000AB550 File Offset: 0x000A9750
		public override void ActivateModule()
		{
			base.ActivateModule();
			this.thisMousePosition = Input.mousePosition;
			this.lastMousePosition = Input.mousePosition;
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x000AB5B0 File Offset: 0x000A97B0
		public override void Process()
		{
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag = this.SendVectorEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendButtonEventToSelectedObject();
				}
			}
			if (this.ProcessTouchEvents())
			{
				return;
			}
			if (this.allowMouseInput)
			{
				this.ProcessMouseEvent();
			}
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x000AB60C File Offset: 0x000A980C
		private bool ProcessTouchEvents()
		{
			int touchCount = Input.touchCount;
			for (int i = 0; i < touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.type != TouchType.Indirect)
				{
					bool flag;
					bool flag2;
					PointerEventData touchPointerEventData = base.GetTouchPointerEventData(touch, out flag, out flag2);
					this.ProcessTouchPress(touchPointerEventData, flag, flag2);
					if (!flag2)
					{
						this.ProcessMove(touchPointerEventData);
						this.ProcessDrag(touchPointerEventData);
					}
					else
					{
						base.RemovePointerData(touchPointerEventData);
					}
				}
			}
			return touchCount > 0;
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x000AB68C File Offset: 0x000A988C
		private bool SendButtonEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			if (this.SubmitWasPressed)
			{
				ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
			else if (this.SubmitWasReleased)
			{
			}
			if (this.CancelWasPressed)
			{
				ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000AB714 File Offset: 0x000A9914
		private bool SendVectorEventToSelectedObject()
		{
			if (!this.VectorWasPressed)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(this.thisVectorState.x, this.thisVectorState.y, 0.5f);
			if (axisEventData.moveDir != MoveDirection.None)
			{
				if (base.eventSystem.currentSelectedGameObject == null)
				{
					base.eventSystem.SetSelectedGameObject(base.eventSystem.firstSelectedGameObject, this.GetBaseEventData());
				}
				else
				{
					ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
				}
				this.SetVectorRepeatTimer();
			}
			return axisEventData.used;
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000AB7B8 File Offset: 0x000A99B8
		protected override void ProcessMove(PointerEventData pointerEvent)
		{
			GameObject pointerEnter = pointerEvent.pointerEnter;
			base.ProcessMove(pointerEvent);
			if (this.focusOnMouseHover && pointerEnter != pointerEvent.pointerEnter)
			{
				GameObject eventHandler = ExecuteEvents.GetEventHandler<ISelectHandler>(pointerEvent.pointerEnter);
				base.eventSystem.SetSelectedGameObject(eventHandler, pointerEvent);
			}
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000AB808 File Offset: 0x000A9A08
		private void Update()
		{
			this.direction.Filter(this.Device.Direction, Time.deltaTime);
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x000AB828 File Offset: 0x000A9A28
		private void UpdateInputState()
		{
			this.lastVectorState = this.thisVectorState;
			this.thisVectorState = Vector2.zero;
			TwoAxisInputControl twoAxisInputControl = this.MoveAction ?? this.direction;
			if (Utility.AbsoluteIsOverThreshold(twoAxisInputControl.X, this.analogMoveThreshold))
			{
				this.thisVectorState.x = Mathf.Sign(twoAxisInputControl.X);
			}
			if (Utility.AbsoluteIsOverThreshold(twoAxisInputControl.Y, this.analogMoveThreshold))
			{
				this.thisVectorState.y = Mathf.Sign(twoAxisInputControl.Y);
			}
			if (this.VectorIsReleased)
			{
				this.nextMoveRepeatTime = 0f;
			}
			if (this.VectorIsPressed)
			{
				if (this.lastVectorState == Vector2.zero)
				{
					if (Time.realtimeSinceStartup > this.lastVectorPressedTime + 0.1f)
					{
						this.nextMoveRepeatTime = Time.realtimeSinceStartup + this.moveRepeatFirstDuration;
					}
					else
					{
						this.nextMoveRepeatTime = Time.realtimeSinceStartup + this.moveRepeatDelayDuration;
					}
				}
				this.lastVectorPressedTime = Time.realtimeSinceStartup;
			}
			this.lastSubmitState = this.thisSubmitState;
			this.thisSubmitState = ((this.SubmitAction != null) ? this.SubmitAction.IsPressed : this.SubmitButton.IsPressed);
			this.lastCancelState = this.thisCancelState;
			this.thisCancelState = ((this.CancelAction != null) ? this.CancelAction.IsPressed : this.CancelButton.IsPressed);
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06002877 RID: 10359 RVA: 0x000AB9B4 File Offset: 0x000A9BB4
		// (set) Token: 0x06002876 RID: 10358 RVA: 0x000AB9A8 File Offset: 0x000A9BA8
		public InputDevice Device
		{
			get
			{
				return this.inputDevice ?? InputManager.ActiveDevice;
			}
			set
			{
				this.inputDevice = value;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06002878 RID: 10360 RVA: 0x000AB9C8 File Offset: 0x000A9BC8
		private InputControl SubmitButton
		{
			get
			{
				return this.Device.GetControl((InputControlType)this.submitButton);
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06002879 RID: 10361 RVA: 0x000AB9DC File Offset: 0x000A9BDC
		private InputControl CancelButton
		{
			get
			{
				return this.Device.GetControl((InputControlType)this.cancelButton);
			}
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x000AB9F0 File Offset: 0x000A9BF0
		private void SetVectorRepeatTimer()
		{
			this.nextMoveRepeatTime = Mathf.Max(this.nextMoveRepeatTime, Time.realtimeSinceStartup + this.moveRepeatDelayDuration);
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x0600287B RID: 10363 RVA: 0x000ABA10 File Offset: 0x000A9C10
		private bool VectorIsPressed
		{
			get
			{
				return this.thisVectorState != Vector2.zero;
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x0600287C RID: 10364 RVA: 0x000ABA24 File Offset: 0x000A9C24
		private bool VectorIsReleased
		{
			get
			{
				return this.thisVectorState == Vector2.zero;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x0600287D RID: 10365 RVA: 0x000ABA38 File Offset: 0x000A9C38
		private bool VectorHasChanged
		{
			get
			{
				return this.thisVectorState != this.lastVectorState;
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x0600287E RID: 10366 RVA: 0x000ABA4C File Offset: 0x000A9C4C
		private bool VectorWasPressed
		{
			get
			{
				return (this.VectorIsPressed && Time.realtimeSinceStartup > this.nextMoveRepeatTime) || (this.VectorIsPressed && this.lastVectorState == Vector2.zero);
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x0600287F RID: 10367 RVA: 0x000ABA8C File Offset: 0x000A9C8C
		private bool SubmitWasPressed
		{
			get
			{
				return this.thisSubmitState && this.thisSubmitState != this.lastSubmitState;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06002880 RID: 10368 RVA: 0x000ABAB0 File Offset: 0x000A9CB0
		private bool SubmitWasReleased
		{
			get
			{
				return !this.thisSubmitState && this.thisSubmitState != this.lastSubmitState;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06002881 RID: 10369 RVA: 0x000ABAD4 File Offset: 0x000A9CD4
		private bool CancelWasPressed
		{
			get
			{
				return this.thisCancelState && this.thisCancelState != this.lastCancelState;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06002882 RID: 10370 RVA: 0x000ABAF8 File Offset: 0x000A9CF8
		private bool MouseHasMoved
		{
			get
			{
				return (this.thisMousePosition - this.lastMousePosition).sqrMagnitude > 0f;
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06002883 RID: 10371 RVA: 0x000ABB28 File Offset: 0x000A9D28
		private bool MouseButtonIsPressed
		{
			get
			{
				return Input.GetMouseButtonDown(0);
			}
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x000ABB30 File Offset: 0x000A9D30
		protected bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x000ABB7C File Offset: 0x000A9D7C
		protected void ProcessMouseEvent()
		{
			this.ProcessMouseEvent(0);
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000ABB88 File Offset: 0x000A9D88
		protected void ProcessMouseEvent(int id)
		{
			PointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData(id);
			PointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
			this.ProcessMousePress(eventData);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject);
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(eventHandler, eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x000ABC6C File Offset: 0x000A9E6C
		protected void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
		{
			PointerEventData buttonData = data.buttonData;
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				base.DeselectIfSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					float num = unscaledTime - buttonData.clickTime;
					if (num < 0.3f)
					{
						buttonData.clickCount++;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					base.HandlePointerExitAndEnter(buttonData, null);
					base.HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x000ABE90 File Offset: 0x000AA090
		protected void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
		{
			GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
			if (pressed)
			{
				pointerEvent.eligibleForClick = true;
				pointerEvent.delta = Vector2.zero;
				pointerEvent.dragging = false;
				pointerEvent.useDragThreshold = true;
				pointerEvent.pressPosition = pointerEvent.position;
				pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
				base.DeselectIfSelectionChanged(gameObject, pointerEvent);
				if (pointerEvent.pointerEnter != gameObject)
				{
					base.HandlePointerExitAndEnter(pointerEvent, gameObject);
					pointerEvent.pointerEnter = gameObject;
				}
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == pointerEvent.lastPress)
				{
					float num = unscaledTime - pointerEvent.clickTime;
					if (num < 0.3f)
					{
						pointerEvent.clickCount++;
					}
					else
					{
						pointerEvent.clickCount = 1;
					}
					pointerEvent.clickTime = unscaledTime;
				}
				else
				{
					pointerEvent.clickCount = 1;
				}
				pointerEvent.pointerPress = gameObject2;
				pointerEvent.rawPointerPress = gameObject;
				pointerEvent.clickTime = unscaledTime;
				pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (released)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (pointerEvent.pointerPress == eventHandler && pointerEvent.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
				}
				else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, pointerEvent, ExecuteEvents.dropHandler);
				}
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;
				if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.dragging = false;
				pointerEvent.pointerDrag = null;
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.pointerDrag = null;
				ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
				pointerEvent.pointerEnter = null;
			}
		}

		// Token: 0x04001C26 RID: 7206
		public InControlInputModule.Button submitButton = InControlInputModule.Button.Action1;

		// Token: 0x04001C27 RID: 7207
		public InControlInputModule.Button cancelButton = InControlInputModule.Button.Action2;

		// Token: 0x04001C28 RID: 7208
		[Range(0.1f, 0.9f)]
		public float analogMoveThreshold = 0.5f;

		// Token: 0x04001C29 RID: 7209
		public float moveRepeatFirstDuration = 0.8f;

		// Token: 0x04001C2A RID: 7210
		public float moveRepeatDelayDuration = 0.1f;

		// Token: 0x04001C2B RID: 7211
		[FormerlySerializedAs("allowMobileDevice")]
		public bool forceModuleActive;

		// Token: 0x04001C2C RID: 7212
		public bool allowMouseInput = true;

		// Token: 0x04001C2D RID: 7213
		public bool focusOnMouseHover;

		// Token: 0x04001C2E RID: 7214
		private InputDevice inputDevice;

		// Token: 0x04001C2F RID: 7215
		private Vector3 thisMousePosition;

		// Token: 0x04001C30 RID: 7216
		private Vector3 lastMousePosition;

		// Token: 0x04001C31 RID: 7217
		private Vector2 thisVectorState;

		// Token: 0x04001C32 RID: 7218
		private Vector2 lastVectorState;

		// Token: 0x04001C33 RID: 7219
		private bool thisSubmitState;

		// Token: 0x04001C34 RID: 7220
		private bool lastSubmitState;

		// Token: 0x04001C35 RID: 7221
		private bool thisCancelState;

		// Token: 0x04001C36 RID: 7222
		private bool lastCancelState;

		// Token: 0x04001C37 RID: 7223
		private float nextMoveRepeatTime;

		// Token: 0x04001C38 RID: 7224
		private float lastVectorPressedTime;

		// Token: 0x04001C39 RID: 7225
		private TwoAxisInputControl direction;

		// Token: 0x020006B6 RID: 1718
		public enum Button
		{
			// Token: 0x04001C3E RID: 7230
			Action1 = 19,
			// Token: 0x04001C3F RID: 7231
			Action2,
			// Token: 0x04001C40 RID: 7232
			Action3,
			// Token: 0x04001C41 RID: 7233
			Action4
		}
	}
}
