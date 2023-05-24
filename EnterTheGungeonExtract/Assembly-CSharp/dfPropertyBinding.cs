using System;
using UnityEngine;

// Token: 0x02000383 RID: 899
[AddComponentMenu("Daikon Forge/Data Binding/Property Binding")]
[Serializable]
public class dfPropertyBinding : MonoBehaviour, IDataBindingComponent
{
	// Token: 0x1700034F RID: 847
	// (get) Token: 0x06000F25 RID: 3877 RVA: 0x000468F0 File Offset: 0x00044AF0
	public bool IsBound
	{
		get
		{
			return this.isBound;
		}
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x000468F8 File Offset: 0x00044AF8
	public virtual void OnEnable()
	{
		if (!this.AutoBind || this.DataSource == null || this.DataTarget == null)
		{
			return;
		}
		if (!this.isBound && this.DataSource.IsValid && this.DataTarget.IsValid)
		{
			this.Bind();
		}
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x00046958 File Offset: 0x00044B58
	public virtual void Start()
	{
		if (!this.AutoBind || this.DataSource == null || this.DataTarget == null)
		{
			return;
		}
		if (!this.isBound && this.DataSource.IsValid && this.DataTarget.IsValid)
		{
			this.Bind();
		}
	}

	// Token: 0x06000F28 RID: 3880 RVA: 0x000469B8 File Offset: 0x00044BB8
	public virtual void OnDisable()
	{
		if (this.AutoUnbind)
		{
			this.Unbind();
		}
	}

	// Token: 0x06000F29 RID: 3881 RVA: 0x000469CC File Offset: 0x00044BCC
	public virtual void OnDestroy()
	{
		this.Unbind();
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x000469D4 File Offset: 0x00044BD4
	public virtual void Update()
	{
		if (this.sourceProperty == null || this.targetProperty == null)
		{
			return;
		}
		if (this.sourceProperty.HasChanged)
		{
			this.targetProperty.Value = this.formatValue(this.sourceProperty.Value);
			this.sourceProperty.ClearChangedFlag();
		}
		else if (this.TwoWay && this.targetProperty.HasChanged)
		{
			this.sourceProperty.Value = this.targetProperty.Value;
			this.targetProperty.ClearChangedFlag();
		}
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x00046A70 File Offset: 0x00044C70
	public static dfPropertyBinding Bind(Component sourceComponent, string sourceProperty, Component targetComponent, string targetProperty)
	{
		return dfPropertyBinding.Bind(sourceComponent.gameObject, sourceComponent, sourceProperty, targetComponent, targetProperty);
	}

	// Token: 0x06000F2C RID: 3884 RVA: 0x00046A84 File Offset: 0x00044C84
	public static dfPropertyBinding Bind(GameObject hostObject, Component sourceComponent, string sourceProperty, Component targetComponent, string targetProperty)
	{
		if (hostObject == null)
		{
			throw new ArgumentNullException("hostObject");
		}
		if (sourceComponent == null)
		{
			throw new ArgumentNullException("sourceComponent");
		}
		if (targetComponent == null)
		{
			throw new ArgumentNullException("targetComponent");
		}
		if (string.IsNullOrEmpty(sourceProperty))
		{
			throw new ArgumentNullException("sourceProperty");
		}
		if (string.IsNullOrEmpty(targetProperty))
		{
			throw new ArgumentNullException("targetProperty");
		}
		dfPropertyBinding dfPropertyBinding = hostObject.AddComponent<dfPropertyBinding>();
		dfPropertyBinding.DataSource = new dfComponentMemberInfo
		{
			Component = sourceComponent,
			MemberName = sourceProperty
		};
		dfPropertyBinding.DataTarget = new dfComponentMemberInfo
		{
			Component = targetComponent,
			MemberName = targetProperty
		};
		dfPropertyBinding.Bind();
		return dfPropertyBinding;
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x00046B48 File Offset: 0x00044D48
	public virtual bool CanSynchronize()
	{
		return this.DataSource != null && this.DataTarget != null && (this.DataSource.IsValid || this.DataTarget.IsValid) && this.DataTarget.GetMemberType() == this.DataSource.GetMemberType();
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x00046BB0 File Offset: 0x00044DB0
	public virtual void Bind()
	{
		if (this.isBound)
		{
			return;
		}
		if (!this.DataSource.IsValid || !this.DataTarget.IsValid)
		{
			Debug.LogError(string.Format("Invalid data binding configuration - Source:{0}, Target:{1}", this.DataSource, this.DataTarget));
			return;
		}
		this.sourceProperty = this.DataSource.GetProperty();
		this.targetProperty = this.DataTarget.GetProperty();
		this.isBound = this.sourceProperty != null && this.targetProperty != null;
		if (this.isBound)
		{
			if (this.targetProperty.PropertyType == typeof(string) && this.sourceProperty.PropertyType != typeof(string))
			{
				this.useFormatString = !string.IsNullOrEmpty(this.FormatString);
			}
			this.targetProperty.Value = this.formatValue(this.sourceProperty.Value);
		}
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x00046CB8 File Offset: 0x00044EB8
	public virtual void Unbind()
	{
		if (!this.isBound)
		{
			return;
		}
		this.sourceProperty = null;
		this.targetProperty = null;
		this.isBound = false;
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x00046CDC File Offset: 0x00044EDC
	private object formatValue(object value)
	{
		try
		{
			if (this.useFormatString && !string.IsNullOrEmpty(this.FormatString))
			{
				return string.Format(this.FormatString, value);
			}
		}
		catch (FormatException ex)
		{
			Debug.LogError(ex, this);
			if (Application.isPlaying)
			{
				base.enabled = false;
			}
		}
		return value;
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x00046D4C File Offset: 0x00044F4C
	public override string ToString()
	{
		string text = ((this.DataSource == null || !(this.DataSource.Component != null)) ? "[null]" : this.DataSource.Component.GetType().Name);
		string text2 = ((this.DataSource == null || string.IsNullOrEmpty(this.DataSource.MemberName)) ? "[null]" : this.DataSource.MemberName);
		string text3 = ((this.DataTarget == null || !(this.DataTarget.Component != null)) ? "[null]" : this.DataTarget.Component.GetType().Name);
		string text4 = ((this.DataTarget == null || string.IsNullOrEmpty(this.DataTarget.MemberName)) ? "[null]" : this.DataTarget.MemberName);
		return string.Format("Bind {0}.{1} -> {2}.{3}", new object[] { text, text2, text3, text4 });
	}

	// Token: 0x04000E89 RID: 3721
	public dfComponentMemberInfo DataSource;

	// Token: 0x04000E8A RID: 3722
	public dfComponentMemberInfo DataTarget;

	// Token: 0x04000E8B RID: 3723
	public string FormatString;

	// Token: 0x04000E8C RID: 3724
	public bool TwoWay;

	// Token: 0x04000E8D RID: 3725
	public bool AutoBind = true;

	// Token: 0x04000E8E RID: 3726
	public bool AutoUnbind = true;

	// Token: 0x04000E8F RID: 3727
	protected dfObservableProperty sourceProperty;

	// Token: 0x04000E90 RID: 3728
	protected dfObservableProperty targetProperty;

	// Token: 0x04000E91 RID: 3729
	protected bool isBound;

	// Token: 0x04000E92 RID: 3730
	protected bool useFormatString;
}
