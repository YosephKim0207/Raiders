using System;
using System.Linq;
using System.Reflection;

// Token: 0x02000380 RID: 896
public class dfObservableProperty : IObservableValue
{
	// Token: 0x06000F08 RID: 3848 RVA: 0x00046560 File Offset: 0x00044760
	internal dfObservableProperty(object target, string memberName)
	{
		MemberInfo memberInfo = target.GetType().GetMember(memberName, BindingFlags.Instance | BindingFlags.Public).FirstOrDefault<MemberInfo>();
		if (memberInfo == null)
		{
			throw new ArgumentException("Invalid property or field name: " + memberName, "memberName");
		}
		this.initMember(target, memberInfo);
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x000465AC File Offset: 0x000447AC
	internal dfObservableProperty(object target, FieldInfo field)
	{
		this.initField(target, field);
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x000465BC File Offset: 0x000447BC
	internal dfObservableProperty(object target, PropertyInfo property)
	{
		this.initProperty(target, property);
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x000465CC File Offset: 0x000447CC
	internal dfObservableProperty(object target, MemberInfo member)
	{
		this.initMember(target, member);
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06000F0C RID: 3852 RVA: 0x000465DC File Offset: 0x000447DC
	public Type PropertyType
	{
		get
		{
			if (this.fieldInfo != null)
			{
				return this.fieldInfo.FieldType;
			}
			return this.propertyInfo.PropertyType;
		}
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06000F0D RID: 3853 RVA: 0x00046600 File Offset: 0x00044800
	// (set) Token: 0x06000F0E RID: 3854 RVA: 0x00046608 File Offset: 0x00044808
	public object Value
	{
		get
		{
			return this.getter();
		}
		set
		{
			this.lastValue = value;
			this.setter(value);
			this.hasChanged = false;
		}
	}

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x06000F0F RID: 3855 RVA: 0x00046620 File Offset: 0x00044820
	public bool HasChanged
	{
		get
		{
			if (this.hasChanged)
			{
				return true;
			}
			object obj = this.getter();
			if (object.ReferenceEquals(obj, this.lastValue))
			{
				this.hasChanged = false;
			}
			else if (obj == null || this.lastValue == null)
			{
				this.hasChanged = true;
			}
			else
			{
				this.hasChanged = !obj.Equals(this.lastValue);
			}
			return this.hasChanged;
		}
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x00046698 File Offset: 0x00044898
	public void ClearChangedFlag()
	{
		this.hasChanged = false;
		this.lastValue = this.getter();
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x000466B0 File Offset: 0x000448B0
	private void initMember(object target, MemberInfo member)
	{
		if (member is FieldInfo)
		{
			this.initField(target, (FieldInfo)member);
		}
		else
		{
			this.initProperty(target, (PropertyInfo)member);
		}
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x000466DC File Offset: 0x000448DC
	private void initField(object target, FieldInfo field)
	{
		this.target = target;
		this.fieldInfo = field;
		this.Value = this.getter();
	}

	// Token: 0x06000F13 RID: 3859 RVA: 0x000466F8 File Offset: 0x000448F8
	private void initProperty(object target, PropertyInfo property)
	{
		this.target = target;
		this.propertyInfo = property;
		this.propertyGetter = property.GetGetMethod();
		this.propertySetter = property.GetSetMethod();
		this.canWrite = this.propertySetter != null;
		this.Value = this.getter();
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x0004674C File Offset: 0x0004494C
	private object getter()
	{
		if (this.propertyInfo != null)
		{
			return this.getPropertyValue();
		}
		return this.getFieldValue();
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x00046768 File Offset: 0x00044968
	private void setter(object value)
	{
		if (this.propertyInfo != null)
		{
			this.setPropertyValue(value);
		}
		else
		{
			this.setFieldValue(value);
		}
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x00046788 File Offset: 0x00044988
	private object getPropertyValue()
	{
		return this.propertyGetter.Invoke(this.target, null);
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x0004679C File Offset: 0x0004499C
	private void setPropertyValue(object value)
	{
		if (!this.canWrite)
		{
			return;
		}
		if (this.propertyType == null)
		{
			this.propertyType = this.propertyInfo.PropertyType;
		}
		if (value == null || this.propertyType.IsAssignableFrom(value.GetType()))
		{
			dfObservableProperty.tempArray[0] = value;
		}
		else
		{
			dfObservableProperty.tempArray[0] = Convert.ChangeType(value, this.propertyType);
		}
		this.propertySetter.Invoke(this.target, dfObservableProperty.tempArray);
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x00046824 File Offset: 0x00044A24
	private void setFieldValue(object value)
	{
		if (this.fieldInfo.IsLiteral)
		{
			return;
		}
		if (this.propertyType == null)
		{
			this.propertyType = this.fieldInfo.FieldType;
		}
		if (value == null || this.propertyType.IsAssignableFrom(value.GetType()))
		{
			this.fieldInfo.SetValue(this.target, value);
		}
		else
		{
			object obj = Convert.ChangeType(value, this.propertyType);
			this.fieldInfo.SetValue(this.target, obj);
		}
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x000468B0 File Offset: 0x00044AB0
	private void setFieldValueNOP(object value)
	{
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x000468B4 File Offset: 0x00044AB4
	private object getFieldValue()
	{
		return this.fieldInfo.GetValue(this.target);
	}

	// Token: 0x04000E7F RID: 3711
	private static object[] tempArray = new object[1];

	// Token: 0x04000E80 RID: 3712
	private object lastValue;

	// Token: 0x04000E81 RID: 3713
	private bool hasChanged;

	// Token: 0x04000E82 RID: 3714
	private object target;

	// Token: 0x04000E83 RID: 3715
	private FieldInfo fieldInfo;

	// Token: 0x04000E84 RID: 3716
	private PropertyInfo propertyInfo;

	// Token: 0x04000E85 RID: 3717
	private MethodInfo propertyGetter;

	// Token: 0x04000E86 RID: 3718
	private MethodInfo propertySetter;

	// Token: 0x04000E87 RID: 3719
	private Type propertyType;

	// Token: 0x04000E88 RID: 3720
	private bool canWrite;

	// Token: 0x02000381 RID: 897
	// (Invoke) Token: 0x06000F1D RID: 3869
	private delegate object ValueGetter();

	// Token: 0x02000382 RID: 898
	// (Invoke) Token: 0x06000F21 RID: 3873
	private delegate void ValueSetter(object value);
}
