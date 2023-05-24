using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200037F RID: 895
[Obsolete("The expression binding functionality is no longer supported and may be removed in future versions of DFGUI")]
[Serializable]
public class dfExpressionPropertyBinding : MonoBehaviour, IDataBindingComponent
{
	// Token: 0x1700034A RID: 842
	// (get) Token: 0x06000EFF RID: 3839 RVA: 0x000461C0 File Offset: 0x000443C0
	public bool IsBound
	{
		get
		{
			return this.isBound;
		}
	}

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x06000F00 RID: 3840 RVA: 0x000461C8 File Offset: 0x000443C8
	// (set) Token: 0x06000F01 RID: 3841 RVA: 0x000461D0 File Offset: 0x000443D0
	public string Expression
	{
		get
		{
			return this.expression;
		}
		set
		{
			if (!string.Equals(value, this.expression))
			{
				this.Unbind();
				this.expression = value;
			}
		}
	}

	// Token: 0x06000F02 RID: 3842 RVA: 0x000461F0 File Offset: 0x000443F0
	public void OnDisable()
	{
		this.Unbind();
	}

	// Token: 0x06000F03 RID: 3843 RVA: 0x000461F8 File Offset: 0x000443F8
	public void Update()
	{
		if (this.isBound)
		{
			this.evaluate();
		}
		else
		{
			bool flag = this.DataSource != null && !string.IsNullOrEmpty(this.expression) && this.DataTarget.IsValid;
			if (flag)
			{
				this.Bind();
			}
		}
	}

	// Token: 0x06000F04 RID: 3844 RVA: 0x00046258 File Offset: 0x00044458
	public void Unbind()
	{
		if (!this.isBound)
		{
			return;
		}
		this.compiledExpression = null;
		this.targetProperty = null;
		this.isBound = false;
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x0004627C File Offset: 0x0004447C
	public void Bind()
	{
		if (this.isBound)
		{
			return;
		}
		if (this.DataSource is dfDataObjectProxy && ((dfDataObjectProxy)this.DataSource).Data == null)
		{
			return;
		}
		dfScriptEngineSettings dfScriptEngineSettings = new dfScriptEngineSettings
		{
			Constants = new Dictionary<string, object>
			{
				{
					"Application",
					typeof(Application)
				},
				{
					"Color",
					typeof(Color)
				},
				{
					"Color32",
					typeof(Color32)
				},
				{
					"Random",
					typeof(UnityEngine.Random)
				},
				{
					"Time",
					typeof(Time)
				},
				{
					"ScriptableObject",
					typeof(ScriptableObject)
				},
				{
					"Vector2",
					typeof(Vector2)
				},
				{
					"Vector3",
					typeof(Vector3)
				},
				{
					"Vector4",
					typeof(Vector4)
				},
				{
					"Quaternion",
					typeof(Quaternion)
				},
				{
					"Matrix",
					typeof(Matrix4x4)
				},
				{
					"Mathf",
					typeof(Mathf)
				}
			}
		};
		if (this.DataSource is dfDataObjectProxy)
		{
			dfDataObjectProxy dfDataObjectProxy = this.DataSource as dfDataObjectProxy;
			dfScriptEngineSettings.AddVariable(new dfScriptVariable("source", null, dfDataObjectProxy.DataType));
		}
		else
		{
			dfScriptEngineSettings.AddVariable(new dfScriptVariable("source", this.DataSource));
		}
		this.compiledExpression = dfScriptEngine.CompileExpression(this.expression, dfScriptEngineSettings);
		this.targetProperty = this.DataTarget.GetProperty();
		this.isBound = this.compiledExpression != null && this.targetProperty != null;
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x00046460 File Offset: 0x00044660
	private void evaluate()
	{
		try
		{
			object obj = this.DataSource;
			if (obj is dfDataObjectProxy)
			{
				obj = ((dfDataObjectProxy)obj).Data;
			}
			object obj2 = this.compiledExpression.DynamicInvoke(new object[] { obj });
			this.targetProperty.Value = obj2;
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x000464D0 File Offset: 0x000446D0
	public override string ToString()
	{
		string text = ((this.DataTarget == null || !(this.DataTarget.Component != null)) ? "[null]" : this.DataTarget.Component.GetType().Name);
		string text2 = ((this.DataTarget == null || string.IsNullOrEmpty(this.DataTarget.MemberName)) ? "[null]" : this.DataTarget.MemberName);
		return string.Format("Bind [expression] -> {0}.{1}", text, text2);
	}

	// Token: 0x04000E79 RID: 3705
	public Component DataSource;

	// Token: 0x04000E7A RID: 3706
	public dfComponentMemberInfo DataTarget;

	// Token: 0x04000E7B RID: 3707
	[SerializeField]
	protected string expression;

	// Token: 0x04000E7C RID: 3708
	private Delegate compiledExpression;

	// Token: 0x04000E7D RID: 3709
	private dfObservableProperty targetProperty;

	// Token: 0x04000E7E RID: 3710
	private bool isBound;
}
