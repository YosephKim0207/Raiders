using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x0200037A RID: 890
[AddComponentMenu("Daikon Forge/Data Binding/Event Binding")]
[Serializable]
public class dfEventBinding : MonoBehaviour, IDataBindingComponent
{
	// Token: 0x17000349 RID: 841
	// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x000451B0 File Offset: 0x000433B0
	public bool IsBound
	{
		get
		{
			return this.isBound;
		}
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x000451B8 File Offset: 0x000433B8
	public void OnEnable()
	{
		if (this.AutoBind && this.DataSource != null && !this.isBound && this.DataSource.IsValid && this.DataTarget.IsValid)
		{
			this.Bind();
		}
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x0004520C File Offset: 0x0004340C
	public void Start()
	{
		if (this.AutoBind && this.DataSource != null && !this.isBound && this.DataSource.IsValid && this.DataTarget.IsValid)
		{
			this.Bind();
		}
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x00045260 File Offset: 0x00043460
	public void OnDisable()
	{
		if (this.AutoUnbind)
		{
			this.Unbind();
		}
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x00045274 File Offset: 0x00043474
	public void OnDestroy()
	{
		this.Unbind();
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x0004527C File Offset: 0x0004347C
	public void Bind()
	{
		if (this.isBound || this.DataSource == null)
		{
			return;
		}
		if (!this.DataSource.IsValid || !this.DataTarget.IsValid)
		{
			Debug.LogError(string.Format("Invalid event binding configuration - Source:{0}, Target:{1}", this.DataSource, this.DataTarget));
			return;
		}
		this.sourceComponent = this.DataSource.Component;
		this.targetComponent = this.DataTarget.Component;
		MethodInfo method = this.DataTarget.GetMethod();
		if (method == null)
		{
			Debug.LogError("Event handler not found: " + this.targetComponent.GetType().Name + "." + this.DataTarget.MemberName);
			return;
		}
		if (this.bindToEventProperty(method))
		{
			this.isBound = true;
			return;
		}
		if (this.bindToEventField(method))
		{
			this.isBound = true;
			return;
		}
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x00045368 File Offset: 0x00043568
	public void Unbind()
	{
		if (!this.isBound)
		{
			return;
		}
		this.isBound = false;
		if (this.eventField != null)
		{
			Delegate @delegate = (Delegate)this.eventField.GetValue(this.sourceComponent);
			Delegate delegate2 = Delegate.Remove(@delegate, this.eventDelegate);
			this.eventField.SetValue(this.sourceComponent, delegate2);
		}
		else if (this.eventInfo != null)
		{
			MethodInfo removeMethod = this.eventInfo.GetRemoveMethod();
			removeMethod.Invoke(this.sourceComponent, new object[] { this.eventDelegate });
		}
		this.eventInfo = null;
		this.eventField = null;
		this.eventDelegate = null;
		this.handlerProxy = null;
		this.sourceComponent = null;
		this.targetComponent = null;
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x0004542C File Offset: 0x0004362C
	public override string ToString()
	{
		string text = ((this.DataSource == null || !(this.DataSource.Component != null)) ? "[null]" : this.DataSource.Component.GetType().Name);
		string text2 = ((this.DataSource == null || string.IsNullOrEmpty(this.DataSource.MemberName)) ? "[null]" : this.DataSource.MemberName);
		string text3 = ((this.DataTarget == null || !(this.DataTarget.Component != null)) ? "[null]" : this.DataTarget.Component.GetType().Name);
		string text4 = ((this.DataTarget == null || string.IsNullOrEmpty(this.DataTarget.MemberName)) ? "[null]" : this.DataTarget.MemberName);
		return string.Format("Bind {0}.{1} -> {2}.{3}", new object[] { text, text2, text3, text4 });
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x00045548 File Offset: 0x00043748
	[dfEventProxy]
	[HideInInspector]
	public void NotificationEventProxy()
	{
		this.callProxyEventHandler(new object[0]);
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x00045558 File Offset: 0x00043758
	[HideInInspector]
	[dfEventProxy]
	public void GenericCallbackProxy(object sender)
	{
		this.callProxyEventHandler(new object[] { sender });
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x0004556C File Offset: 0x0004376C
	[HideInInspector]
	[dfEventProxy]
	public void AnimationEventProxy(dfTweenPlayableBase tween)
	{
		this.callProxyEventHandler(new object[] { tween });
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x00045580 File Offset: 0x00043780
	[dfEventProxy]
	[HideInInspector]
	public void MouseEventProxy(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.callProxyEventHandler(new object[] { control, mouseEvent });
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x00045598 File Offset: 0x00043798
	[dfEventProxy]
	[HideInInspector]
	public void KeyEventProxy(dfControl control, dfKeyEventArgs keyEvent)
	{
		this.callProxyEventHandler(new object[] { control, keyEvent });
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x000455B0 File Offset: 0x000437B0
	[dfEventProxy]
	[HideInInspector]
	public void DragEventProxy(dfControl control, dfDragEventArgs dragEvent)
	{
		this.callProxyEventHandler(new object[] { control, dragEvent });
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x000455C8 File Offset: 0x000437C8
	[dfEventProxy]
	[HideInInspector]
	public void ChildControlEventProxy(dfControl container, dfControl child)
	{
		this.callProxyEventHandler(new object[] { container, child });
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x000455E0 File Offset: 0x000437E0
	[dfEventProxy]
	[HideInInspector]
	public void FocusEventProxy(dfControl control, dfFocusEventArgs args)
	{
		this.callProxyEventHandler(new object[] { control, args });
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x000455F8 File Offset: 0x000437F8
	[HideInInspector]
	[dfEventProxy]
	public void PropertyChangedProxy(dfControl control, int value)
	{
		this.callProxyEventHandler(new object[] { control, value });
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x00045614 File Offset: 0x00043814
	[HideInInspector]
	[dfEventProxy]
	public void PropertyChangedProxy(dfControl control, float value)
	{
		this.callProxyEventHandler(new object[] { control, value });
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x00045630 File Offset: 0x00043830
	[dfEventProxy]
	[HideInInspector]
	public void PropertyChangedProxy(dfControl control, bool value)
	{
		this.callProxyEventHandler(new object[] { control, value });
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x0004564C File Offset: 0x0004384C
	[HideInInspector]
	[dfEventProxy]
	public void PropertyChangedProxy(dfControl control, string value)
	{
		this.callProxyEventHandler(new object[] { control, value });
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x00045664 File Offset: 0x00043864
	[HideInInspector]
	[dfEventProxy]
	public void PropertyChangedProxy(dfControl control, Vector2 value)
	{
		this.callProxyEventHandler(new object[] { control, value });
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x00045680 File Offset: 0x00043880
	[HideInInspector]
	[dfEventProxy]
	public void PropertyChangedProxy(dfControl control, Vector3 value)
	{
		this.callProxyEventHandler(new object[] { control, value });
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x0004569C File Offset: 0x0004389C
	[dfEventProxy]
	[HideInInspector]
	public void PropertyChangedProxy(dfControl control, Vector4 value)
	{
		this.callProxyEventHandler(new object[] { control, value });
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x000456B8 File Offset: 0x000438B8
	[dfEventProxy]
	[HideInInspector]
	public void PropertyChangedProxy(dfControl control, Quaternion value)
	{
		this.callProxyEventHandler(new object[] { control, value });
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x000456D4 File Offset: 0x000438D4
	[HideInInspector]
	[dfEventProxy]
	public void PropertyChangedProxy(dfControl control, dfButton.ButtonState value)
	{
		this.callProxyEventHandler(new object[] { control, value });
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x000456F0 File Offset: 0x000438F0
	[dfEventProxy]
	[HideInInspector]
	public void PropertyChangedProxy(dfControl control, dfPivotPoint value)
	{
		this.callProxyEventHandler(new object[] { control, value });
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x0004570C File Offset: 0x0004390C
	[HideInInspector]
	[dfEventProxy]
	public void PropertyChangedProxy(dfControl control, Texture value)
	{
		this.callProxyEventHandler(new object[] { control, value });
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x00045724 File Offset: 0x00043924
	[dfEventProxy]
	[HideInInspector]
	public void PropertyChangedProxy(dfControl control, Texture2D value)
	{
		this.callProxyEventHandler(new object[] { control, value });
	}

	// Token: 0x06000EE4 RID: 3812 RVA: 0x0004573C File Offset: 0x0004393C
	[HideInInspector]
	[dfEventProxy]
	public void PropertyChangedProxy(dfControl control, Material value)
	{
		this.callProxyEventHandler(new object[] { control, value });
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x00045754 File Offset: 0x00043954
	[HideInInspector]
	[dfEventProxy]
	public void SystemEventHandlerProxy(object sender, EventArgs args)
	{
		this.callProxyEventHandler(new object[] { sender, args });
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x0004576C File Offset: 0x0004396C
	private bool bindToEventField(MethodInfo eventHandler)
	{
		this.eventField = dfEventBinding.getField(this.sourceComponent, this.DataSource.MemberName);
		if (this.eventField == null)
		{
			return false;
		}
		try
		{
			MethodInfo method = this.eventField.FieldType.GetMethod("Invoke");
			ParameterInfo[] parameters = method.GetParameters();
			ParameterInfo[] parameters2 = eventHandler.GetParameters();
			if (parameters.Length == parameters2.Length && method.ReturnType == eventHandler.ReturnType)
			{
				this.eventDelegate = Delegate.CreateDelegate(this.eventField.FieldType, this.targetComponent, eventHandler, true);
			}
			else
			{
				this.eventDelegate = this.createEventProxyDelegate(this.targetComponent, this.eventField.FieldType, parameters, eventHandler);
			}
			Delegate @delegate = Delegate.Combine(this.eventDelegate, (Delegate)this.eventField.GetValue(this.sourceComponent));
			this.eventField.SetValue(this.sourceComponent, @delegate);
		}
		catch (Exception ex)
		{
			base.enabled = false;
			string text = string.Format("Event binding failed - Failed to create event handler for {0} ({1}) - {2}", this.DataSource, eventHandler, ex.ToString());
			Debug.LogError(text, this);
			return false;
		}
		return true;
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x000458B0 File Offset: 0x00043AB0
	private bool bindToEventProperty(MethodInfo eventHandler)
	{
		this.eventInfo = this.sourceComponent.GetType().GetEvent(this.DataSource.MemberName);
		if (this.eventInfo == null)
		{
			return false;
		}
		try
		{
			Type eventHandlerType = this.eventInfo.EventHandlerType;
			MethodInfo addMethod = this.eventInfo.GetAddMethod();
			MethodInfo method = eventHandlerType.GetMethod("Invoke");
			ParameterInfo[] parameters = method.GetParameters();
			ParameterInfo[] parameters2 = eventHandler.GetParameters();
			if (parameters.Length == parameters2.Length && method.ReturnType == eventHandler.ReturnType)
			{
				this.eventDelegate = Delegate.CreateDelegate(eventHandlerType, this.targetComponent, eventHandler, true);
			}
			else
			{
				this.eventDelegate = this.createEventProxyDelegate(this.targetComponent, eventHandlerType, parameters, eventHandler);
			}
			addMethod.Invoke(this.DataSource.Component, new object[] { this.eventDelegate });
		}
		catch (Exception ex)
		{
			base.enabled = false;
			string text = string.Format("Event binding failed - Failed to create event handler for {0} ({1}) - {2}", this.DataSource, eventHandler, ex.ToString());
			Debug.LogError(text, this);
			return false;
		}
		return true;
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x000459E4 File Offset: 0x00043BE4
	private void callProxyEventHandler(params object[] arguments)
	{
		if (this.handlerProxy == null)
		{
			return;
		}
		if (this.handlerParameters.Length == 0)
		{
			arguments = null;
		}
		object obj = this.handlerProxy.Invoke(this.targetComponent, arguments);
		if (!(obj is IEnumerator))
		{
			return;
		}
		if (this.targetComponent is MonoBehaviour)
		{
			((MonoBehaviour)this.targetComponent).StartCoroutine((IEnumerator)obj);
		}
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x00045A54 File Offset: 0x00043C54
	private static FieldInfo getField(Component component, string fieldName)
	{
		if (component == null)
		{
			throw new ArgumentNullException("component");
		}
		return component.GetType().GetAllFields().FirstOrDefault((FieldInfo f) => f.Name == fieldName);
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x00045AA4 File Offset: 0x00043CA4
	private Delegate createEventProxyDelegate(object target, Type delegateType, ParameterInfo[] eventParams, MethodInfo eventHandler)
	{
		MethodInfo methodInfo = (from m in typeof(dfEventBinding).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			where m.IsDefined(typeof(dfEventProxyAttribute), true) && this.signatureIsCompatible(eventParams, m.GetParameters())
			select m).FirstOrDefault<MethodInfo>();
		if (methodInfo == null)
		{
			return null;
		}
		this.handlerProxy = eventHandler;
		this.handlerParameters = eventHandler.GetParameters();
		return Delegate.CreateDelegate(delegateType, this, methodInfo, true);
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x00045B18 File Offset: 0x00043D18
	private bool signatureIsCompatible(ParameterInfo[] lhs, ParameterInfo[] rhs)
	{
		if (lhs == null || rhs == null)
		{
			return false;
		}
		if (lhs.Length != rhs.Length)
		{
			return false;
		}
		for (int i = 0; i < lhs.Length; i++)
		{
			if (!this.areTypesCompatible(lhs[i], rhs[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x00045B68 File Offset: 0x00043D68
	private bool areTypesCompatible(ParameterInfo lhs, ParameterInfo rhs)
	{
		return lhs.ParameterType.Equals(rhs.ParameterType) || lhs.ParameterType.IsAssignableFrom(rhs.ParameterType);
	}

	// Token: 0x04000E66 RID: 3686
	public dfComponentMemberInfo DataSource;

	// Token: 0x04000E67 RID: 3687
	public dfComponentMemberInfo DataTarget;

	// Token: 0x04000E68 RID: 3688
	public bool AutoBind = true;

	// Token: 0x04000E69 RID: 3689
	public bool AutoUnbind = true;

	// Token: 0x04000E6A RID: 3690
	private bool isBound;

	// Token: 0x04000E6B RID: 3691
	private Component sourceComponent;

	// Token: 0x04000E6C RID: 3692
	private Component targetComponent;

	// Token: 0x04000E6D RID: 3693
	private EventInfo eventInfo;

	// Token: 0x04000E6E RID: 3694
	private FieldInfo eventField;

	// Token: 0x04000E6F RID: 3695
	private Delegate eventDelegate;

	// Token: 0x04000E70 RID: 3696
	private MethodInfo handlerProxy;

	// Token: 0x04000E71 RID: 3697
	private ParameterInfo[] handlerParameters;
}
