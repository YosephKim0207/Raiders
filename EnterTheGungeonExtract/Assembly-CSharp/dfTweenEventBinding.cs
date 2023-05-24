using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x020004C9 RID: 1225
[AddComponentMenu("Daikon Forge/Tweens/Tween Event Binding")]
[Serializable]
public class dfTweenEventBinding : MonoBehaviour
{
	// Token: 0x06001CDD RID: 7389 RVA: 0x0008727C File Offset: 0x0008547C
	private void OnEnable()
	{
		if (this.isValid())
		{
			this.Bind();
		}
	}

	// Token: 0x06001CDE RID: 7390 RVA: 0x00087290 File Offset: 0x00085490
	private void Start()
	{
		if (this.isValid())
		{
			this.Bind();
		}
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x000872A4 File Offset: 0x000854A4
	private void OnDisable()
	{
		this.Unbind();
	}

	// Token: 0x06001CE0 RID: 7392 RVA: 0x000872AC File Offset: 0x000854AC
	public void Bind()
	{
		if (this.isBound && !this.isValid())
		{
			return;
		}
		this.isBound = true;
		if (!string.IsNullOrEmpty(this.StartEvent))
		{
			this.startEventBinding = this.bindEvent(this.StartEvent, "Play");
		}
		if (!string.IsNullOrEmpty(this.StopEvent))
		{
			this.stopEventBinding = this.bindEvent(this.StopEvent, "Stop");
		}
		if (!string.IsNullOrEmpty(this.ResetEvent))
		{
			this.resetEventBinding = this.bindEvent(this.ResetEvent, "Reset");
		}
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x0008734C File Offset: 0x0008554C
	public void Unbind()
	{
		if (!this.isBound)
		{
			return;
		}
		this.isBound = false;
		if (this.startEventBinding != null)
		{
			this.startEventBinding.Unbind();
			this.startEventBinding = null;
		}
		if (this.stopEventBinding != null)
		{
			this.stopEventBinding.Unbind();
			this.stopEventBinding = null;
		}
		if (this.resetEventBinding != null)
		{
			this.resetEventBinding.Unbind();
			this.resetEventBinding = null;
		}
	}

	// Token: 0x06001CE2 RID: 7394 RVA: 0x000873D8 File Offset: 0x000855D8
	private bool isValid()
	{
		if (this.Tween == null || !(this.Tween is dfTweenComponentBase))
		{
			return false;
		}
		if (this.EventSource == null)
		{
			return false;
		}
		bool flag = string.IsNullOrEmpty(this.StartEvent) && string.IsNullOrEmpty(this.StopEvent) && string.IsNullOrEmpty(this.ResetEvent);
		if (flag)
		{
			return false;
		}
		Type type = this.EventSource.GetType();
		return (string.IsNullOrEmpty(this.StartEvent) || this.getField(type, this.StartEvent) != null) && (string.IsNullOrEmpty(this.StopEvent) || this.getField(type, this.StopEvent) != null) && (string.IsNullOrEmpty(this.ResetEvent) || this.getField(type, this.ResetEvent) != null);
	}

	// Token: 0x06001CE3 RID: 7395 RVA: 0x000874CC File Offset: 0x000856CC
	private FieldInfo getField(Type type, string fieldName)
	{
		return (from f in type.GetAllFields()
			where f.Name == fieldName
			select f).FirstOrDefault<FieldInfo>();
	}

	// Token: 0x06001CE4 RID: 7396 RVA: 0x00087504 File Offset: 0x00085704
	private void unbindEvent(FieldInfo eventField, Delegate eventDelegate)
	{
		Delegate @delegate = (Delegate)eventField.GetValue(this.EventSource);
		Delegate delegate2 = Delegate.Remove(@delegate, eventDelegate);
		eventField.SetValue(this.EventSource, delegate2);
	}

	// Token: 0x06001CE5 RID: 7397 RVA: 0x00087538 File Offset: 0x00085738
	private dfEventBinding bindEvent(string eventName, string handlerName)
	{
		if (this.Tween.GetType().GetMethod(handlerName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) == null)
		{
			throw new MissingMemberException("Method not found: " + handlerName);
		}
		dfEventBinding dfEventBinding = base.gameObject.AddComponent<dfEventBinding>();
		dfEventBinding.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset;
		dfEventBinding.DataSource = new dfComponentMemberInfo
		{
			Component = this.EventSource,
			MemberName = eventName
		};
		dfEventBinding.DataTarget = new dfComponentMemberInfo
		{
			Component = this.Tween,
			MemberName = handlerName
		};
		dfEventBinding.Bind();
		return dfEventBinding;
	}

	// Token: 0x04001643 RID: 5699
	public Component Tween;

	// Token: 0x04001644 RID: 5700
	public Component EventSource;

	// Token: 0x04001645 RID: 5701
	public string StartEvent;

	// Token: 0x04001646 RID: 5702
	public string StopEvent;

	// Token: 0x04001647 RID: 5703
	public string ResetEvent;

	// Token: 0x04001648 RID: 5704
	private bool isBound;

	// Token: 0x04001649 RID: 5705
	private dfEventBinding startEventBinding;

	// Token: 0x0400164A RID: 5706
	private dfEventBinding stopEventBinding;

	// Token: 0x0400164B RID: 5707
	private dfEventBinding resetEventBinding;
}
