using System;
using UnityEngine;

// Token: 0x0200037D RID: 893
[AddComponentMenu("Daikon Forge/Data Binding/Event-Driven Property Binding")]
[Serializable]
public class dfEventDrivenPropertyBinding : dfPropertyBinding
{
	// Token: 0x06000EF2 RID: 3826 RVA: 0x00045BFC File Offset: 0x00043DFC
	public override void Update()
	{
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x00045C00 File Offset: 0x00043E00
	public static dfEventDrivenPropertyBinding Bind(Component sourceComponent, string sourceProperty, string sourceEvent, Component targetComponent, string targetProperty, string targetEvent)
	{
		return dfEventDrivenPropertyBinding.Bind(sourceComponent.gameObject, sourceComponent, sourceProperty, sourceEvent, targetComponent, targetProperty, targetEvent);
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x00045C18 File Offset: 0x00043E18
	public static dfEventDrivenPropertyBinding Bind(GameObject hostObject, Component sourceComponent, string sourceProperty, string sourceEvent, Component targetComponent, string targetProperty, string targetEvent)
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
		if (string.IsNullOrEmpty(sourceEvent))
		{
			throw new ArgumentNullException("sourceEvent");
		}
		dfEventDrivenPropertyBinding dfEventDrivenPropertyBinding = hostObject.AddComponent<dfEventDrivenPropertyBinding>();
		dfEventDrivenPropertyBinding.DataSource = new dfComponentMemberInfo
		{
			Component = sourceComponent,
			MemberName = sourceProperty
		};
		dfEventDrivenPropertyBinding.DataTarget = new dfComponentMemberInfo
		{
			Component = targetComponent,
			MemberName = targetProperty
		};
		dfEventDrivenPropertyBinding.SourceEventName = sourceEvent;
		dfEventDrivenPropertyBinding.TargetEventName = targetEvent;
		dfEventDrivenPropertyBinding.Bind();
		return dfEventDrivenPropertyBinding;
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x00045D04 File Offset: 0x00043F04
	public override void Bind()
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
		if (this.sourceProperty != null && this.targetProperty != null)
		{
			if (!string.IsNullOrEmpty(this.SourceEventName) && this.SourceEventName.Trim() != string.Empty)
			{
				this.bindSourceEvent();
			}
			if (!string.IsNullOrEmpty(this.TargetEventName) && this.TargetEventName.Trim() != string.Empty)
			{
				this.bindTargetEvent();
			}
			else if (this.targetProperty.PropertyType == typeof(string) && this.sourceProperty.PropertyType != typeof(string))
			{
				this.useFormatString = !string.IsNullOrEmpty(this.FormatString);
			}
			this.MirrorSourceProperty();
			this.isBound = this.sourceEventBinding != null;
		}
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x00045E58 File Offset: 0x00044058
	public override void Unbind()
	{
		if (!this.isBound)
		{
			return;
		}
		this.isBound = false;
		if (this.sourceEventBinding != null)
		{
			this.sourceEventBinding.Unbind();
			UnityEngine.Object.Destroy(this.sourceEventBinding);
			this.sourceEventBinding = null;
		}
		if (this.targetEventBinding != null)
		{
			this.targetEventBinding.Unbind();
			UnityEngine.Object.Destroy(this.targetEventBinding);
			this.targetEventBinding = null;
		}
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x00045ED4 File Offset: 0x000440D4
	public void MirrorSourceProperty()
	{
		this.targetProperty.Value = this.formatValue(this.sourceProperty.Value);
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x00045EF4 File Offset: 0x000440F4
	public void MirrorTargetProperty()
	{
		this.sourceProperty.Value = this.targetProperty.Value;
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x00045F0C File Offset: 0x0004410C
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

	// Token: 0x06000EFA RID: 3834 RVA: 0x00045F7C File Offset: 0x0004417C
	private void bindSourceEvent()
	{
		this.sourceEventBinding = base.gameObject.AddComponent<dfEventBinding>();
		this.sourceEventBinding.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset;
		this.sourceEventBinding.DataSource = new dfComponentMemberInfo
		{
			Component = this.DataSource.Component,
			MemberName = this.SourceEventName
		};
		this.sourceEventBinding.DataTarget = new dfComponentMemberInfo
		{
			Component = this,
			MemberName = "MirrorSourceProperty"
		};
		this.sourceEventBinding.Bind();
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x00046008 File Offset: 0x00044208
	private void bindTargetEvent()
	{
		this.targetEventBinding = base.gameObject.AddComponent<dfEventBinding>();
		this.targetEventBinding.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset;
		this.targetEventBinding.DataSource = new dfComponentMemberInfo
		{
			Component = this.DataTarget.Component,
			MemberName = this.TargetEventName
		};
		this.targetEventBinding.DataTarget = new dfComponentMemberInfo
		{
			Component = this,
			MemberName = "MirrorTargetProperty"
		};
		this.targetEventBinding.Bind();
	}

	// Token: 0x06000EFC RID: 3836 RVA: 0x00046094 File Offset: 0x00044294
	public override string ToString()
	{
		string text = ((this.DataSource == null || !(this.DataSource.Component != null)) ? "[null]" : this.DataSource.Component.GetType().Name);
		string text2 = ((this.DataSource == null || string.IsNullOrEmpty(this.DataSource.MemberName)) ? "[null]" : this.DataSource.MemberName);
		string text3 = ((this.DataTarget == null || !(this.DataTarget.Component != null)) ? "[null]" : this.DataTarget.Component.GetType().Name);
		string text4 = ((this.DataTarget == null || string.IsNullOrEmpty(this.DataTarget.MemberName)) ? "[null]" : this.DataTarget.MemberName);
		return string.Format("Bind {0}.{1} -> {2}.{3}", new object[] { text, text2, text3, text4 });
	}

	// Token: 0x04000E75 RID: 3701
	public string SourceEventName;

	// Token: 0x04000E76 RID: 3702
	public string TargetEventName;

	// Token: 0x04000E77 RID: 3703
	protected dfEventBinding sourceEventBinding;

	// Token: 0x04000E78 RID: 3704
	protected dfEventBinding targetEventBinding;
}
