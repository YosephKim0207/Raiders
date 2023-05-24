using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x02000375 RID: 885
[Serializable]
public class dfComponentMemberInfo
{
	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06000E98 RID: 3736 RVA: 0x00044AC0 File Offset: 0x00042CC0
	public bool IsValid
	{
		get
		{
			return this.Component != null && !string.IsNullOrEmpty(this.MemberName) && this.Component.GetType().GetMember(this.MemberName).FirstOrDefault<MemberInfo>() != null;
		}
	}

	// Token: 0x06000E99 RID: 3737 RVA: 0x00044B20 File Offset: 0x00042D20
	public Type GetMemberType()
	{
		Type type = this.Component.GetType();
		MemberInfo memberInfo = type.GetMember(this.MemberName).FirstOrDefault<MemberInfo>();
		if (memberInfo == null)
		{
			throw new MissingMemberException("Member not found: " + type.Name + "." + this.MemberName);
		}
		if (memberInfo is FieldInfo)
		{
			return ((FieldInfo)memberInfo).FieldType;
		}
		if (memberInfo is PropertyInfo)
		{
			return ((PropertyInfo)memberInfo).PropertyType;
		}
		if (memberInfo is MethodInfo)
		{
			return ((MethodInfo)memberInfo).ReturnType;
		}
		if (memberInfo is EventInfo)
		{
			return ((EventInfo)memberInfo).EventHandlerType;
		}
		throw new InvalidCastException("Invalid member type: " + memberInfo.GetMemberType());
	}

	// Token: 0x06000E9A RID: 3738 RVA: 0x00044BE8 File Offset: 0x00042DE8
	public MethodInfo GetMethod()
	{
		return this.Component.GetType().GetMember(this.MemberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault<MemberInfo>() as MethodInfo;
	}

	// Token: 0x06000E9B RID: 3739 RVA: 0x00044C1C File Offset: 0x00042E1C
	public dfObservableProperty GetProperty()
	{
		Type type = this.Component.GetType();
		MemberInfo memberInfo = this.Component.GetType().GetMember(this.MemberName).FirstOrDefault<MemberInfo>();
		if (memberInfo == null)
		{
			throw new MissingMemberException("Member not found: " + type.Name + "." + this.MemberName);
		}
		if (!(memberInfo is FieldInfo) && !(memberInfo is PropertyInfo))
		{
			throw new InvalidCastException("Member " + this.MemberName + " is not an observable field or property");
		}
		return new dfObservableProperty(this.Component, memberInfo);
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x00044CB8 File Offset: 0x00042EB8
	public override string ToString()
	{
		string text = ((!(this.Component != null)) ? "[Missing ComponentType]" : this.Component.GetType().Name);
		string text2 = (string.IsNullOrEmpty(this.MemberName) ? "[Missing MemberName]" : this.MemberName);
		return string.Format("{0}.{1}", text, text2);
	}

	// Token: 0x04000E59 RID: 3673
	public Component Component;

	// Token: 0x04000E5A RID: 3674
	public string MemberName;
}
