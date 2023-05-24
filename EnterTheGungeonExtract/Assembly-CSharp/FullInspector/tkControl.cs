using System;
using System.Collections.Generic;
using System.Reflection;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x0200066C RID: 1644
	public abstract class tkControl<T, TContext> : tkIControl
	{
		// Token: 0x06002585 RID: 9605 RVA: 0x000A0CF8 File Offset: 0x0009EEF8
		protected fiGraphMetadata GetInstanceMetadata(fiGraphMetadata metadata)
		{
			return metadata.Enter(this._uniqueId).Metadata;
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002586 RID: 9606 RVA: 0x000A0D1C File Offset: 0x0009EF1C
		public Type ContextType
		{
			get
			{
				return typeof(TContext);
			}
		}

		// Token: 0x06002587 RID: 9607
		protected abstract T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata);

		// Token: 0x06002588 RID: 9608
		protected abstract float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata);

		// Token: 0x06002589 RID: 9609 RVA: 0x000A0D28 File Offset: 0x0009EF28
		public virtual bool ShouldShow(T obj, TContext context, fiGraphMetadata metadata)
		{
			return true;
		}

		// Token: 0x170006F3 RID: 1779
		// (set) Token: 0x0600258A RID: 9610 RVA: 0x000A0D2C File Offset: 0x0009EF2C
		public tkStyle<T, TContext> Style
		{
			set
			{
				this.Styles = new List<tkStyle<T, TContext>> { value };
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x0600258B RID: 9611 RVA: 0x000A0D50 File Offset: 0x0009EF50
		// (set) Token: 0x0600258C RID: 9612 RVA: 0x000A0D70 File Offset: 0x0009EF70
		public List<tkStyle<T, TContext>> Styles
		{
			get
			{
				if (this._styles == null)
				{
					this._styles = new List<tkStyle<T, TContext>>();
				}
				return this._styles;
			}
			set
			{
				this._styles = value;
			}
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x000A0D7C File Offset: 0x0009EF7C
		public T Edit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
		{
			if (this.Styles == null)
			{
				return this.DoEdit(rect, obj, context, metadata);
			}
			for (int i = 0; i < this.Styles.Count; i++)
			{
				this.Styles[i].Activate(obj, context);
			}
			T t = this.DoEdit(rect, obj, context, metadata);
			for (int j = 0; j < this.Styles.Count; j++)
			{
				this.Styles[j].Deactivate(obj, context);
			}
			return t;
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000A0E0C File Offset: 0x0009F00C
		public object Edit(Rect rect, object obj, object context, fiGraphMetadata metadata)
		{
			return this.Edit(rect, (T)((object)obj), (TContext)((object)context), metadata);
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000A0E28 File Offset: 0x0009F028
		public float GetHeight(T obj, TContext context, fiGraphMetadata metadata)
		{
			if (this.Styles == null)
			{
				return this.DoGetHeight(obj, context, metadata);
			}
			for (int i = 0; i < this.Styles.Count; i++)
			{
				this.Styles[i].Activate(obj, context);
			}
			float num = this.DoGetHeight(obj, context, metadata);
			for (int j = 0; j < this.Styles.Count; j++)
			{
				this.Styles[j].Deactivate(obj, context);
			}
			return num;
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000A0EB4 File Offset: 0x0009F0B4
		public float GetHeight(object obj, object context, fiGraphMetadata metadata)
		{
			return this.GetHeight((T)((object)obj), (TContext)((object)context), metadata);
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000A0ECC File Offset: 0x0009F0CC
		void tkIControl.InitializeId(ref int nextId)
		{
			this._uniqueId = nextId++;
			foreach (tkIControl tkIControl in this.NonMemberChildControls)
			{
				tkIControl.InitializeId(ref nextId);
			}
			for (Type type = base.GetType(); type != null; type = type.Resolve().BaseType)
			{
				foreach (MemberInfo memberInfo in type.GetDeclaredMembers())
				{
					Type type2;
					if (tkControl<T, TContext>.TryGetMemberType(memberInfo, out type2))
					{
						if (typeof(tkIControl).IsAssignableFrom(type2))
						{
							tkIControl tkIControl2;
							if (tkControl<T, TContext>.TryReadValue<tkIControl>(memberInfo, this, out tkIControl2))
							{
								if (tkIControl2 != null)
								{
									tkIControl2.InitializeId(ref nextId);
								}
							}
						}
						else if (typeof(IEnumerable<tkIControl>).IsAssignableFrom(type2))
						{
							IEnumerable<tkIControl> enumerable;
							if (tkControl<T, TContext>.TryReadValue<IEnumerable<tkIControl>>(memberInfo, this, out enumerable))
							{
								if (enumerable != null)
								{
									foreach (tkIControl tkIControl3 in enumerable)
									{
										tkIControl3.InitializeId(ref nextId);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002592 RID: 9618 RVA: 0x000A1054 File Offset: 0x0009F254
		protected virtual IEnumerable<tkIControl> NonMemberChildControls
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000A1070 File Offset: 0x0009F270
		private static bool TryReadValue<TValue>(MemberInfo member, object context, out TValue value)
		{
			if (member is FieldInfo)
			{
				value = (TValue)((object)((FieldInfo)member).GetValue(context));
				return true;
			}
			if (member is PropertyInfo)
			{
				value = (TValue)((object)((PropertyInfo)member).GetValue(context, null));
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x000A10D8 File Offset: 0x0009F2D8
		private static bool TryGetMemberType(MemberInfo member, out Type memberType)
		{
			if (member is FieldInfo)
			{
				memberType = ((FieldInfo)member).FieldType;
				return true;
			}
			if (member is PropertyInfo)
			{
				memberType = ((PropertyInfo)member).PropertyType;
				return true;
			}
			memberType = null;
			return false;
		}

		// Token: 0x0400199D RID: 6557
		private int _uniqueId;

		// Token: 0x0400199E RID: 6558
		private List<tkStyle<T, TContext>> _styles;
	}
}
