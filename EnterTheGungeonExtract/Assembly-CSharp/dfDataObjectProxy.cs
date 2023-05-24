using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x02000377 RID: 887
[AddComponentMenu("Daikon Forge/Data Binding/Proxy Data Object")]
[Serializable]
public class dfDataObjectProxy : MonoBehaviour, IDataBindingComponent
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000EB2 RID: 3762 RVA: 0x00044F04 File Offset: 0x00043104
	// (remove) Token: 0x06000EB3 RID: 3763 RVA: 0x00044F3C File Offset: 0x0004313C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfDataObjectProxy.DataObjectChangedHandler DataChanged;

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x00044F74 File Offset: 0x00043174
	public bool IsBound
	{
		get
		{
			return this.data != null;
		}
	}

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x00044F84 File Offset: 0x00043184
	// (set) Token: 0x06000EB6 RID: 3766 RVA: 0x00044F8C File Offset: 0x0004318C
	public string TypeName
	{
		get
		{
			return this.typeName;
		}
		set
		{
			if (this.typeName != value)
			{
				this.typeName = value;
				this.Data = null;
			}
		}
	}

	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x00044FB0 File Offset: 0x000431B0
	public Type DataType
	{
		get
		{
			return this.getTypeFromName(this.typeName);
		}
	}

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x00044FC0 File Offset: 0x000431C0
	// (set) Token: 0x06000EB9 RID: 3769 RVA: 0x00044FC8 File Offset: 0x000431C8
	public object Data
	{
		get
		{
			return this.data;
		}
		set
		{
			if (!object.ReferenceEquals(value, this.data))
			{
				this.data = value;
				if (value != null)
				{
					this.typeName = value.GetType().Name;
				}
				if (this.DataChanged != null)
				{
					this.DataChanged(value);
				}
			}
		}
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x0004501C File Offset: 0x0004321C
	public void Start()
	{
		if (this.DataType == null)
		{
			UnityEngine.Debug.LogError("Unable to retrieve System.Type reference for type: " + this.TypeName);
		}
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x0004504C File Offset: 0x0004324C
	public Type GetPropertyType(string propertyName)
	{
		Type dataType = this.DataType;
		if (dataType == null)
		{
			return null;
		}
		MemberInfo memberInfo = dataType.GetMember(propertyName, BindingFlags.Instance | BindingFlags.Public).FirstOrDefault<MemberInfo>();
		if (memberInfo is FieldInfo)
		{
			return ((FieldInfo)memberInfo).FieldType;
		}
		if (memberInfo is PropertyInfo)
		{
			return ((PropertyInfo)memberInfo).PropertyType;
		}
		return null;
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x000450A8 File Offset: 0x000432A8
	public dfObservableProperty GetProperty(string PropertyName)
	{
		if (this.data == null)
		{
			return null;
		}
		return new dfObservableProperty(this.data, PropertyName);
	}

	// Token: 0x06000EBD RID: 3773 RVA: 0x000450C4 File Offset: 0x000432C4
	private Type getTypeFromName(string nameOfType)
	{
		if (nameOfType == null)
		{
			throw new ArgumentNullException("nameOfType");
		}
		Type[] types = base.GetType().GetAssembly().GetTypes();
		return types.FirstOrDefault((Type t) => t.Name == nameOfType);
	}

	// Token: 0x06000EBE RID: 3774 RVA: 0x0004511C File Offset: 0x0004331C
	private static Type getTypeFromQualifiedName(string typeName)
	{
		Type type = Type.GetType(typeName);
		if (type != null)
		{
			return type;
		}
		if (typeName.IndexOf('.') == -1)
		{
			return null;
		}
		string text = typeName.Substring(0, typeName.IndexOf('.'));
		Assembly assembly = Assembly.Load(new AssemblyName(text));
		if (assembly == null)
		{
			return null;
		}
		return assembly.GetType(typeName);
	}

	// Token: 0x06000EBF RID: 3775 RVA: 0x00045174 File Offset: 0x00043374
	public void Bind()
	{
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x00045178 File Offset: 0x00043378
	public void Unbind()
	{
	}

	// Token: 0x04000E63 RID: 3683
	[SerializeField]
	protected string typeName;

	// Token: 0x04000E64 RID: 3684
	private object data;

	// Token: 0x02000378 RID: 888
	// (Invoke) Token: 0x06000EC2 RID: 3778
	[dfEventCategory("Data Changed")]
	public delegate void DataObjectChangedHandler(object data);
}
