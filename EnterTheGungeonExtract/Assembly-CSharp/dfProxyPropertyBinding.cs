using System;
using UnityEngine;

// Token: 0x02000384 RID: 900
[AddComponentMenu("Daikon Forge/Data Binding/Proxy Property Binding")]
[Serializable]
public class dfProxyPropertyBinding : MonoBehaviour, IDataBindingComponent
{
	// Token: 0x17000350 RID: 848
	// (get) Token: 0x06000F33 RID: 3891 RVA: 0x00046E70 File Offset: 0x00045070
	public bool IsBound
	{
		get
		{
			return this.isBound;
		}
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x00046E78 File Offset: 0x00045078
	public void Awake()
	{
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x00046E7C File Offset: 0x0004507C
	public void OnEnable()
	{
		if (!this.isBound && this.IsDataSourceValid() && this.DataTarget.IsValid)
		{
			this.Bind();
		}
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x00046EAC File Offset: 0x000450AC
	public void Start()
	{
		if (!this.isBound && this.IsDataSourceValid() && this.DataTarget.IsValid)
		{
			this.Bind();
		}
	}

	// Token: 0x06000F37 RID: 3895 RVA: 0x00046EDC File Offset: 0x000450DC
	public void OnDisable()
	{
		this.Unbind();
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x00046EE4 File Offset: 0x000450E4
	public void Update()
	{
		if (this.sourceProperty == null || this.targetProperty == null)
		{
			return;
		}
		if (this.sourceProperty.HasChanged)
		{
			this.targetProperty.Value = this.sourceProperty.Value;
			this.sourceProperty.ClearChangedFlag();
		}
		else if (this.TwoWay && this.targetProperty.HasChanged)
		{
			this.sourceProperty.Value = this.targetProperty.Value;
			this.targetProperty.ClearChangedFlag();
		}
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x00046F7C File Offset: 0x0004517C
	public void Bind()
	{
		if (this.isBound)
		{
			return;
		}
		if (!this.IsDataSourceValid())
		{
			Debug.LogError(string.Format("Invalid data binding configuration - Source:{0}, Target:{1}", this.DataSource, this.DataTarget));
			return;
		}
		if (!this.DataTarget.IsValid)
		{
			Debug.LogError(string.Format("Invalid data binding configuration - Source:{0}, Target:{1}", this.DataSource, this.DataTarget));
			return;
		}
		dfDataObjectProxy dfDataObjectProxy = this.DataSource.Component as dfDataObjectProxy;
		this.sourceProperty = dfDataObjectProxy.GetProperty(this.DataSource.MemberName);
		this.targetProperty = this.DataTarget.GetProperty();
		this.isBound = this.sourceProperty != null && this.targetProperty != null;
		if (this.isBound)
		{
			this.targetProperty.Value = this.sourceProperty.Value;
		}
		this.attachEvent();
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x00047068 File Offset: 0x00045268
	public void Unbind()
	{
		if (!this.isBound)
		{
			return;
		}
		this.detachEvent();
		this.sourceProperty = null;
		this.targetProperty = null;
		this.isBound = false;
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x00047094 File Offset: 0x00045294
	private bool IsDataSourceValid()
	{
		return this.DataSource != null || this.DataSource.Component != null || !string.IsNullOrEmpty(this.DataSource.MemberName) || (this.DataSource.Component as dfDataObjectProxy).Data != null;
	}

	// Token: 0x06000F3C RID: 3900 RVA: 0x000470F8 File Offset: 0x000452F8
	private void attachEvent()
	{
		if (this.eventsAttached)
		{
			return;
		}
		this.eventsAttached = true;
		dfDataObjectProxy dfDataObjectProxy = this.DataSource.Component as dfDataObjectProxy;
		if (dfDataObjectProxy != null)
		{
			dfDataObjectProxy.DataChanged += this.handle_DataChanged;
		}
	}

	// Token: 0x06000F3D RID: 3901 RVA: 0x00047148 File Offset: 0x00045348
	private void detachEvent()
	{
		if (!this.eventsAttached)
		{
			return;
		}
		this.eventsAttached = false;
		dfDataObjectProxy dfDataObjectProxy = this.DataSource.Component as dfDataObjectProxy;
		if (dfDataObjectProxy != null)
		{
			dfDataObjectProxy.DataChanged -= this.handle_DataChanged;
		}
	}

	// Token: 0x06000F3E RID: 3902 RVA: 0x00047198 File Offset: 0x00045398
	private void handle_DataChanged(object data)
	{
		this.Unbind();
		if (this.IsDataSourceValid())
		{
			this.Bind();
		}
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x000471B4 File Offset: 0x000453B4
	public override string ToString()
	{
		string text = ((this.DataSource == null || !(this.DataSource.Component != null)) ? "[null]" : this.DataSource.Component.GetType().Name);
		string text2 = ((this.DataSource == null || string.IsNullOrEmpty(this.DataSource.MemberName)) ? "[null]" : this.DataSource.MemberName);
		string text3 = ((this.DataTarget == null || !(this.DataTarget.Component != null)) ? "[null]" : this.DataTarget.Component.GetType().Name);
		string text4 = ((this.DataTarget == null || string.IsNullOrEmpty(this.DataTarget.MemberName)) ? "[null]" : this.DataTarget.MemberName);
		return string.Format("Bind {0}.{1} -> {2}.{3}", new object[] { text, text2, text3, text4 });
	}

	// Token: 0x04000E93 RID: 3731
	public dfComponentMemberInfo DataSource;

	// Token: 0x04000E94 RID: 3732
	public dfComponentMemberInfo DataTarget;

	// Token: 0x04000E95 RID: 3733
	public bool TwoWay;

	// Token: 0x04000E96 RID: 3734
	private dfObservableProperty sourceProperty;

	// Token: 0x04000E97 RID: 3735
	private dfObservableProperty targetProperty;

	// Token: 0x04000E98 RID: 3736
	private bool isBound;

	// Token: 0x04000E99 RID: 3737
	private bool eventsAttached;
}
