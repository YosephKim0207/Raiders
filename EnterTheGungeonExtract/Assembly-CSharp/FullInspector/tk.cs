using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FullInspector.Internal;
using FullSerializer;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000635 RID: 1589
	public class tk<T, TContext>
	{
		// Token: 0x060024A3 RID: 9379 RVA: 0x0009EB40 File Offset: 0x0009CD40
		public static tk<T, TContext>.Value<TValue> Val<TValue>(tk<T, TContext>.Value<TValue>.GeneratorNoContext generator)
		{
			return generator;
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x0009EB48 File Offset: 0x0009CD48
		public static tk<T, TContext>.Value<TValue> Val<TValue>(tk<T, TContext>.Value<TValue>.Generator generator)
		{
			return generator;
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x0009EB50 File Offset: 0x0009CD50
		public static tk<T, TContext>.Value<TValue> Val<TValue>(TValue value)
		{
			return value;
		}

		// Token: 0x02000636 RID: 1590
		public class Box : tkControl<T, TContext>
		{
			// Token: 0x060024A6 RID: 9382 RVA: 0x0009EB58 File Offset: 0x0009CD58
			public Box(tkControl<T, TContext> control)
			{
				this._control = control;
			}

			// Token: 0x060024A7 RID: 9383 RVA: 0x0009EB68 File Offset: 0x0009CD68
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				GUI.Box(rect, string.Empty);
				return this._control.Edit(rect, obj, context, metadata);
			}

			// Token: 0x060024A8 RID: 9384 RVA: 0x0009EB88 File Offset: 0x0009CD88
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				return this._control.GetHeight(obj, context, metadata);
			}

			// Token: 0x04001931 RID: 6449
			[ShowInInspector]
			private readonly tkControl<T, TContext> _control;
		}

		// Token: 0x02000637 RID: 1591
		public class Button : tkControl<T, TContext>
		{
			// Token: 0x060024A9 RID: 9385 RVA: 0x0009EB98 File Offset: 0x0009CD98
			public Button(string methodName)
			{
				InspectedMethod foundMethod = null;
				foreach (InspectedMethod inspectedMethod in InspectedType.Get(typeof(T)).GetMethods(InspectedMemberFilters.All))
				{
					if (inspectedMethod.Method.Name == methodName)
					{
						foundMethod = inspectedMethod;
					}
				}
				if (foundMethod != null)
				{
					this._label = foundMethod.DisplayLabel;
					this._enabled = true;
					this._onClick = delegate(T o, TContext c)
					{
						foundMethod.Invoke(o);
					};
				}
				else
				{
					Debug.LogError("Unable to find method " + methodName + " on " + typeof(T).CSharpName());
					this._label = new fiGUIContent(methodName + " (unable to find on " + typeof(T).CSharpName() + ")");
					this._enabled = false;
					this._onClick = delegate(T o, TContext c)
					{
					};
				}
			}

			// Token: 0x060024AA RID: 9386 RVA: 0x0009ECF4 File Offset: 0x0009CEF4
			public Button(tk<T, TContext>.Value<fiGUIContent> label, Action<T, TContext> onClick)
			{
				this._enabled = true;
				this._label = label;
				this._onClick = onClick;
			}

			// Token: 0x060024AB RID: 9387 RVA: 0x0009ED14 File Offset: 0x0009CF14
			public Button(fiGUIContent label, Action<T, TContext> onClick)
				: this(tk<T, TContext>.Val<fiGUIContent>(label), onClick)
			{
			}

			// Token: 0x060024AC RID: 9388 RVA: 0x0009ED24 File Offset: 0x0009CF24
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				fiLateBindings.EditorGUI.BeginDisabledGroup(!this._enabled);
				if (GUI.Button(rect, this._label.GetCurrentValue(obj, context)))
				{
					this._onClick(obj, context);
				}
				fiLateBindings.EditorGUI.EndDisabledGroup();
				return obj;
			}

			// Token: 0x060024AD RID: 9389 RVA: 0x0009ED74 File Offset: 0x0009CF74
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				return 18f;
			}

			// Token: 0x04001932 RID: 6450
			[ShowInInspector]
			private readonly tk<T, TContext>.Value<fiGUIContent> _label;

			// Token: 0x04001933 RID: 6451
			private readonly bool _enabled;

			// Token: 0x04001934 RID: 6452
			private readonly Action<T, TContext> _onClick;
		}

		// Token: 0x02000639 RID: 1593
		public class CenterVertical : tkControl<T, TContext>
		{
			// Token: 0x060024B1 RID: 9393 RVA: 0x0009ED9C File Offset: 0x0009CF9C
			public CenterVertical(tkControl<T, TContext> centered)
			{
				this._centered = centered;
			}

			// Token: 0x060024B2 RID: 9394 RVA: 0x0009EDAC File Offset: 0x0009CFAC
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				float num = rect.height - this._centered.GetHeight(obj, context, metadata);
				rect.y += num / 2f;
				rect.height -= num;
				return this._centered.Edit(rect, obj, context, metadata);
			}

			// Token: 0x060024B3 RID: 9395 RVA: 0x0009EE08 File Offset: 0x0009D008
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				return this._centered.GetHeight(obj, context, metadata);
			}

			// Token: 0x04001937 RID: 6455
			[ShowInInspector]
			private readonly tkControl<T, TContext> _centered;
		}

		// Token: 0x0200063A RID: 1594
		public class Color : tk<T, TContext>.ColorIf
		{
			// Token: 0x060024B4 RID: 9396 RVA: 0x0009EE18 File Offset: 0x0009D018
			public Color(tk<T, TContext>.Value<UnityEngine.Color> color)
				: base(tk<T, TContext>.Val<bool>((T o) => true), color)
			{
			}
		}

		// Token: 0x0200063B RID: 1595
		public class ColorIf : tk<T, TContext>.ConditionalStyle
		{
			// Token: 0x060024B6 RID: 9398 RVA: 0x0009EE48 File Offset: 0x0009D048
			public ColorIf(tk<T, TContext>.Value<bool> shouldActivate, tk<T, TContext>.Value<UnityEngine.Color> color)
				: base(new Func<T, TContext, bool>(shouldActivate.GetCurrentValue), delegate(T obj, TContext context)
				{
					UnityEngine.Color color2 = GUI.color;
					GUI.color = color.GetCurrentValue(obj, context);
					return color2;
				}, delegate(T obj, TContext context, object state)
				{
					GUI.color = (UnityEngine.Color)state;
				})
			{
			}

			// Token: 0x060024B7 RID: 9399 RVA: 0x0009EEA4 File Offset: 0x0009D0A4
			public ColorIf(tk<T, TContext>.Value<bool>.Generator shouldActivate, tk<T, TContext>.Value<UnityEngine.Color> color)
				: this(tk<T, TContext>.Val<bool>(shouldActivate), color)
			{
			}

			// Token: 0x060024B8 RID: 9400 RVA: 0x0009EEB4 File Offset: 0x0009D0B4
			public ColorIf(tk<T, TContext>.Value<bool>.GeneratorNoContext shouldActivate, tk<T, TContext>.Value<UnityEngine.Color> color)
				: this(tk<T, TContext>.Val<bool>(shouldActivate), color)
			{
			}
		}

		// Token: 0x0200063D RID: 1597
		public class Comment : tkControl<T, TContext>
		{
			// Token: 0x060024BC RID: 9404 RVA: 0x0009EF08 File Offset: 0x0009D108
			public Comment(tk<T, TContext>.Value<string> comment, CommentType commentType)
			{
				this._comment = comment;
				this._commentType = commentType;
			}

			// Token: 0x060024BD RID: 9405 RVA: 0x0009EF20 File Offset: 0x0009D120
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				string currentValue = this._comment.GetCurrentValue(obj, context);
				fiLateBindings.EditorGUI.HelpBox(rect, currentValue, this._commentType);
				return obj;
			}

			// Token: 0x060024BE RID: 9406 RVA: 0x0009EF4C File Offset: 0x0009D14C
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				string currentValue = this._comment.GetCurrentValue(obj, context);
				return (float)fiCommentUtility.GetCommentHeight(currentValue, this._commentType);
			}

			// Token: 0x0400193B RID: 6459
			private readonly tk<T, TContext>.Value<string> _comment;

			// Token: 0x0400193C RID: 6460
			private readonly CommentType _commentType;
		}

		// Token: 0x0200063E RID: 1598
		public class ConditionalStyle : tkStyle<T, TContext>
		{
			// Token: 0x060024BF RID: 9407 RVA: 0x0009EF78 File Offset: 0x0009D178
			public ConditionalStyle(Func<T, TContext, bool> shouldActivate, Func<T, TContext, object> activate, Action<T, TContext, object> deactivate)
			{
				this._shouldActivate = shouldActivate;
				this._activate = activate;
				this._deactivate = deactivate;
			}

			// Token: 0x060024C0 RID: 9408 RVA: 0x0009EFAC File Offset: 0x0009D1AC
			public override void Activate(T obj, TContext context)
			{
				bool flag = this._shouldActivate(obj, context);
				this._activatedStack.Push(flag);
				if (flag)
				{
					this._activationStateStack.Push(this._activate(obj, context));
				}
			}

			// Token: 0x060024C1 RID: 9409 RVA: 0x0009EFF4 File Offset: 0x0009D1F4
			public override void Deactivate(T obj, TContext context)
			{
				bool flag = this._activatedStack.Pop();
				if (flag)
				{
					this._deactivate(obj, context, this._activationStateStack.Pop());
				}
			}

			// Token: 0x0400193D RID: 6461
			private readonly Func<T, TContext, bool> _shouldActivate;

			// Token: 0x0400193E RID: 6462
			private readonly Func<T, TContext, object> _activate;

			// Token: 0x0400193F RID: 6463
			private readonly Action<T, TContext, object> _deactivate;

			// Token: 0x04001940 RID: 6464
			private readonly fiStackValue<bool> _activatedStack = new fiStackValue<bool>();

			// Token: 0x04001941 RID: 6465
			private readonly fiStackValue<object> _activationStateStack = new fiStackValue<object>();
		}

		// Token: 0x0200063F RID: 1599
		public class Context : tkControl<T, TContext>
		{
			// Token: 0x060024C3 RID: 9411 RVA: 0x0009F040 File Offset: 0x0009D240
			public tkControl<T, TContext> With(tkControl<T, TContext> control)
			{
				this._control = control;
				return this;
			}

			// Token: 0x170006E4 RID: 1764
			// (get) Token: 0x060024C4 RID: 9412 RVA: 0x0009F04C File Offset: 0x0009D24C
			public T Data
			{
				get
				{
					return this._data.Value;
				}
			}

			// Token: 0x060024C5 RID: 9413 RVA: 0x0009F05C File Offset: 0x0009D25C
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				this._data.Push(obj);
				obj = this._control.Edit(rect, obj, context, metadata);
				this._data.Pop();
				return obj;
			}

			// Token: 0x060024C6 RID: 9414 RVA: 0x0009F08C File Offset: 0x0009D28C
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				this._data.Push(obj);
				float height = this._control.GetHeight(obj, context, metadata);
				this._data.Pop();
				return height;
			}

			// Token: 0x04001942 RID: 6466
			[ShowInInspector]
			private tkControl<T, TContext> _control;

			// Token: 0x04001943 RID: 6467
			[ShowInInspector]
			private readonly fiStackValue<T> _data = new fiStackValue<T>();
		}

		// Token: 0x02000640 RID: 1600
		public class DefaultInspector : tkControl<T, TContext>
		{
			// Token: 0x060024C8 RID: 9416 RVA: 0x0009F0EC File Offset: 0x0009D2EC
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				return (T)((object)fiLateBindings.PropertyEditor.EditSkipUntilNot(new Type[] { this.type_fitkControlPropertyEditor, this.type_IObjectPropertyEditor }, typeof(T), typeof(T).Resolve(), rect, GUIContent.none, obj, new fiGraphMetadataChild
				{
					Metadata = metadata
				}));
			}

			// Token: 0x060024C9 RID: 9417 RVA: 0x0009F154 File Offset: 0x0009D354
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				return fiLateBindings.PropertyEditor.GetElementHeightSkipUntilNot(new Type[] { this.type_fitkControlPropertyEditor, this.type_IObjectPropertyEditor }, typeof(T), typeof(T).Resolve(), GUIContent.none, obj, new fiGraphMetadataChild
				{
					Metadata = metadata
				});
			}

			// Token: 0x04001944 RID: 6468
			private readonly Type type_fitkControlPropertyEditor = TypeCache.FindType("FullInspector.Internal.tkControlPropertyEditor");

			// Token: 0x04001945 RID: 6469
			private readonly Type type_IObjectPropertyEditor = TypeCache.FindType("FullInspector.Modules.Common.IObjectPropertyEditor");
		}

		// Token: 0x02000641 RID: 1601
		public class DisableHierarchyMode : tkControl<T, TContext>
		{
			// Token: 0x060024CA RID: 9418 RVA: 0x0009F1B4 File Offset: 0x0009D3B4
			public DisableHierarchyMode(tkControl<T, TContext> childControl)
			{
				this._childControl = childControl;
			}

			// Token: 0x060024CB RID: 9419 RVA: 0x0009F1C4 File Offset: 0x0009D3C4
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				fiLateBindings.fiEditorGUI.PushHierarchyMode(false);
				T t = this._childControl.Edit(rect, obj, context, metadata);
				fiLateBindings.fiEditorGUI.PopHierarchyMode();
				return t;
			}

			// Token: 0x060024CC RID: 9420 RVA: 0x0009F1F0 File Offset: 0x0009D3F0
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				return this._childControl.GetHeight(obj, context, metadata);
			}

			// Token: 0x04001946 RID: 6470
			private tkControl<T, TContext> _childControl;
		}

		// Token: 0x02000642 RID: 1602
		public class Empty : tkControl<T, TContext>
		{
			// Token: 0x060024CD RID: 9421 RVA: 0x0009F200 File Offset: 0x0009D400
			public Empty()
				: this(0f)
			{
			}

			// Token: 0x060024CE RID: 9422 RVA: 0x0009F214 File Offset: 0x0009D414
			public Empty(tk<T, TContext>.Value<float> height)
			{
				this._height = height;
			}

			// Token: 0x060024CF RID: 9423 RVA: 0x0009F224 File Offset: 0x0009D424
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				return obj;
			}

			// Token: 0x060024D0 RID: 9424 RVA: 0x0009F228 File Offset: 0x0009D428
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				return this._height.GetCurrentValue(obj, context);
			}

			// Token: 0x04001947 RID: 6471
			[ShowInInspector]
			private readonly tk<T, TContext>.Value<float> _height;
		}

		// Token: 0x02000643 RID: 1603
		public class EnabledIf : tk<T, TContext>.ConditionalStyle
		{
			// Token: 0x060024D1 RID: 9425 RVA: 0x0009F248 File Offset: 0x0009D448
			public EnabledIf(tk<T, TContext>.Value<bool> isEnabled)
				: base((T o, TContext c) => !isEnabled.GetCurrentValue(o, c), delegate(T obj, TContext context)
				{
					fiLateBindings.EditorGUI.BeginDisabledGroup(true);
					return null;
				}, delegate(T obj, TContext context, object state)
				{
					fiLateBindings.EditorGUI.EndDisabledGroup();
				})
			{
			}

			// Token: 0x060024D2 RID: 9426 RVA: 0x0009F2B0 File Offset: 0x0009D4B0
			public EnabledIf(tk<T, TContext>.Value<bool>.Generator isEnabled)
				: this(tk<T, TContext>.Val<bool>(isEnabled))
			{
			}

			// Token: 0x060024D3 RID: 9427 RVA: 0x0009F2C0 File Offset: 0x0009D4C0
			public EnabledIf(tk<T, TContext>.Value<bool>.GeneratorNoContext isEnabled)
				: this(tk<T, TContext>.Val<bool>(isEnabled))
			{
			}
		}

		// Token: 0x02000645 RID: 1605
		public class Foldout : tkControl<T, TContext>
		{
			// Token: 0x060024D8 RID: 9432 RVA: 0x0009F300 File Offset: 0x0009D500
			public Foldout(fiGUIContent label, tkControl<T, TContext> control)
				: this(label, FontStyle.Normal, control)
			{
			}

			// Token: 0x060024D9 RID: 9433 RVA: 0x0009F30C File Offset: 0x0009D50C
			public Foldout(fiGUIContent label, FontStyle fontStyle, tkControl<T, TContext> control)
				: this(label, fontStyle, true, control)
			{
			}

			// Token: 0x060024DA RID: 9434 RVA: 0x0009F318 File Offset: 0x0009D518
			public Foldout(fiGUIContent label, FontStyle fontStyle, bool defaultToExpanded, tkControl<T, TContext> control)
			{
				this._label = label;
				this._foldoutStyle = new GUIStyle(fiLateBindings.EditorStyles.foldout)
				{
					fontStyle = fontStyle
				};
				this._defaultToExpanded = defaultToExpanded;
				this._control = control;
			}

			// Token: 0x170006E5 RID: 1765
			// (get) Token: 0x060024DB RID: 9435 RVA: 0x0009F35C File Offset: 0x0009D55C
			// (set) Token: 0x060024DC RID: 9436 RVA: 0x0009F368 File Offset: 0x0009D568
			[ShowInInspector]
			public bool IndentChildControl
			{
				get
				{
					return !this._doNotIndentChildControl;
				}
				set
				{
					this._doNotIndentChildControl = !value;
				}
			}

			// Token: 0x060024DD RID: 9437 RVA: 0x0009F374 File Offset: 0x0009D574
			private tkFoldoutMetadata GetMetadata(fiGraphMetadata metadata)
			{
				bool flag;
				tkFoldoutMetadata persistentMetadata = base.GetInstanceMetadata(metadata).GetPersistentMetadata<tkFoldoutMetadata>(out flag);
				if (flag)
				{
					persistentMetadata.IsExpanded = this._defaultToExpanded;
				}
				return persistentMetadata;
			}

			// Token: 0x060024DE RID: 9438 RVA: 0x0009F3A4 File Offset: 0x0009D5A4
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				tkFoldoutMetadata metadata2 = this.GetMetadata(metadata);
				if (this.HierarchyMode != null)
				{
					fiLateBindings.fiEditorGUI.PushHierarchyMode(this.HierarchyMode.Value);
				}
				Rect rect2 = rect;
				rect2.height = fiLateBindings.EditorGUIUtility.singleLineHeight;
				metadata2.IsExpanded = fiLateBindings.EditorGUI.Foldout(rect2, metadata2.IsExpanded, this._label, true, this._foldoutStyle);
				if (metadata2.IsExpanded)
				{
					float num = fiLateBindings.EditorGUIUtility.singleLineHeight + fiLateBindings.EditorGUIUtility.standardVerticalSpacing;
					Rect rect3 = rect;
					if (this.IndentChildControl)
					{
						rect3.x += 15f;
						rect3.width -= 15f;
					}
					rect3.y += num;
					rect3.height -= num;
					obj = this._control.Edit(rect3, obj, context, metadata);
				}
				if (this.HierarchyMode != null)
				{
					fiLateBindings.fiEditorGUI.PopHierarchyMode();
				}
				return obj;
			}

			// Token: 0x060024DF RID: 9439 RVA: 0x0009F49C File Offset: 0x0009D69C
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				tkFoldoutMetadata metadata2 = this.GetMetadata(metadata);
				float num = fiLateBindings.EditorGUIUtility.singleLineHeight;
				if (metadata2.IsExpanded)
				{
					num += fiLateBindings.EditorGUIUtility.standardVerticalSpacing;
					num += this._control.GetHeight(obj, context, metadata);
				}
				return num;
			}

			// Token: 0x0400194B RID: 6475
			private readonly GUIStyle _foldoutStyle;

			// Token: 0x0400194C RID: 6476
			[ShowInInspector]
			private readonly fiGUIContent _label;

			// Token: 0x0400194D RID: 6477
			[ShowInInspector]
			private readonly tkControl<T, TContext> _control;

			// Token: 0x0400194E RID: 6478
			[ShowInInspector]
			private readonly bool _defaultToExpanded;

			// Token: 0x0400194F RID: 6479
			private bool _doNotIndentChildControl;

			// Token: 0x04001950 RID: 6480
			public bool? HierarchyMode;
		}

		// Token: 0x02000646 RID: 1606
		public class HorizontalGroup : tkControl<T, TContext>, IEnumerable
		{
			// Token: 0x170006E6 RID: 1766
			// (get) Token: 0x060024E1 RID: 9441 RVA: 0x0009F4F0 File Offset: 0x0009D6F0
			protected override IEnumerable<tkIControl> NonMemberChildControls
			{
				get
				{
					foreach (tk<T, TContext>.HorizontalGroup.SectionItem item in this._items)
					{
						yield return item.Rule;
					}
					yield break;
				}
			}

			// Token: 0x060024E2 RID: 9442 RVA: 0x0009F514 File Offset: 0x0009D714
			public void Add(tkControl<T, TContext> rule)
			{
				this.InternalAdd(false, 0f, 1f, rule);
			}

			// Token: 0x060024E3 RID: 9443 RVA: 0x0009F528 File Offset: 0x0009D728
			public void Add(bool matchParentHeight, tkControl<T, TContext> rule)
			{
				this.InternalAdd(matchParentHeight, 0f, 1f, rule);
			}

			// Token: 0x060024E4 RID: 9444 RVA: 0x0009F53C File Offset: 0x0009D73C
			public void Add(float width)
			{
				this.InternalAdd(false, width, 0f, tk<T, TContext>.HorizontalGroup.DefaultRule);
			}

			// Token: 0x060024E5 RID: 9445 RVA: 0x0009F550 File Offset: 0x0009D750
			public void Add(float width, tkControl<T, TContext> rule)
			{
				this.InternalAdd(false, width, 0f, rule);
			}

			// Token: 0x060024E6 RID: 9446 RVA: 0x0009F560 File Offset: 0x0009D760
			private void InternalAdd(bool matchParentHeight, float width, float fillStrength, tkControl<T, TContext> rule)
			{
				if (width < 0f)
				{
					throw new ArgumentException("width must be >= 0");
				}
				if (fillStrength < 0f)
				{
					throw new ArgumentException("fillStrength must be >= 0");
				}
				this._items.Add(new tk<T, TContext>.HorizontalGroup.SectionItem
				{
					MatchParentHeight = matchParentHeight,
					MinWidth = width,
					FillStrength = fillStrength,
					Rule = rule
				});
			}

			// Token: 0x060024E7 RID: 9447 RVA: 0x0009F5D0 File Offset: 0x0009D7D0
			private void DoLayout(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				for (int i = 0; i < this._items.Count; i++)
				{
					tk<T, TContext>.HorizontalGroup.SectionItem sectionItem = this._items[i];
					sectionItem.Layout_IsFlexible = sectionItem.FillStrength > 0f;
					this._items[i] = sectionItem;
				}
				for (;;)
				{
					IL_4B:
					float num = 0f;
					float num2 = 0f;
					for (int j = 0; j < this._items.Count; j++)
					{
						tk<T, TContext>.HorizontalGroup.SectionItem sectionItem2 = this._items[j];
						if (sectionItem2.Rule.ShouldShow(obj, context, metadata))
						{
							if (sectionItem2.Layout_IsFlexible)
							{
								num2 += sectionItem2.FillStrength;
							}
							else
							{
								num += sectionItem2.MinWidth;
							}
						}
					}
					float num3 = rect.width - num;
					for (int k = 0; k < this._items.Count; k++)
					{
						tk<T, TContext>.HorizontalGroup.SectionItem sectionItem3 = this._items[k];
						if (sectionItem3.Rule.ShouldShow(obj, context, metadata))
						{
							if (sectionItem3.Layout_IsFlexible)
							{
								sectionItem3.Layout_FlexibleWidth = num3 * sectionItem3.FillStrength / num2;
								this._items[k] = sectionItem3;
								if (sectionItem3.Layout_FlexibleWidth < sectionItem3.MinWidth)
								{
									sectionItem3.Layout_IsFlexible = false;
									this._items[k] = sectionItem3;
									goto IL_4B;
								}
							}
						}
					}
					break;
				}
			}

			// Token: 0x060024E8 RID: 9448 RVA: 0x0009F760 File Offset: 0x0009D960
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				this.DoLayout(rect, obj, context, metadata);
				for (int i = 0; i < this._items.Count; i++)
				{
					tk<T, TContext>.HorizontalGroup.SectionItem sectionItem = this._items[i];
					if (sectionItem.Rule.ShouldShow(obj, context, metadata))
					{
						float layout_Width = sectionItem.Layout_Width;
						Rect rect2 = rect;
						rect2.width = layout_Width;
						if (!sectionItem.MatchParentHeight)
						{
							rect2.height = sectionItem.Rule.GetHeight(obj, context, metadata);
						}
						obj = sectionItem.Rule.Edit(rect2, obj, context, metadata);
						rect.x += layout_Width;
					}
				}
				return obj;
			}

			// Token: 0x060024E9 RID: 9449 RVA: 0x0009F814 File Offset: 0x0009DA14
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				float num = 0f;
				for (int i = 0; i < this._items.Count; i++)
				{
					tk<T, TContext>.HorizontalGroup.SectionItem sectionItem = this._items[i];
					if (sectionItem.Rule.ShouldShow(obj, context, metadata))
					{
						num = Math.Max(num, sectionItem.Rule.GetHeight(obj, context, metadata));
					}
				}
				return num;
			}

			// Token: 0x060024EA RID: 9450 RVA: 0x0009F880 File Offset: 0x0009DA80
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04001951 RID: 6481
			[ShowInInspector]
			private readonly List<tk<T, TContext>.HorizontalGroup.SectionItem> _items = new List<tk<T, TContext>.HorizontalGroup.SectionItem>();

			// Token: 0x04001952 RID: 6482
			private static readonly tkControl<T, TContext> DefaultRule = new tk<T, TContext>.VerticalGroup();

			// Token: 0x02000647 RID: 1607
			private struct SectionItem
			{
				// Token: 0x170006E7 RID: 1767
				// (get) Token: 0x060024EC RID: 9452 RVA: 0x0009F894 File Offset: 0x0009DA94
				// (set) Token: 0x060024ED RID: 9453 RVA: 0x0009F89C File Offset: 0x0009DA9C
				[ShowInInspector]
				public float MinWidth
				{
					get
					{
						return this._minWidth;
					}
					set
					{
						this._minWidth = Math.Max(value, 0f);
					}
				}

				// Token: 0x170006E8 RID: 1768
				// (get) Token: 0x060024EE RID: 9454 RVA: 0x0009F8B0 File Offset: 0x0009DAB0
				// (set) Token: 0x060024EF RID: 9455 RVA: 0x0009F8B8 File Offset: 0x0009DAB8
				[ShowInInspector]
				public float FillStrength
				{
					get
					{
						return this._fillStrength;
					}
					set
					{
						this._fillStrength = Math.Max(value, 0f);
					}
				}

				// Token: 0x170006E9 RID: 1769
				// (get) Token: 0x060024F0 RID: 9456 RVA: 0x0009F8CC File Offset: 0x0009DACC
				public float Layout_Width
				{
					get
					{
						if (this.Layout_IsFlexible)
						{
							return this.Layout_FlexibleWidth;
						}
						return this.MinWidth;
					}
				}

				// Token: 0x04001953 RID: 6483
				private float _minWidth;

				// Token: 0x04001954 RID: 6484
				private float _fillStrength;

				// Token: 0x04001955 RID: 6485
				public bool MatchParentHeight;

				// Token: 0x04001956 RID: 6486
				public tkControl<T, TContext> Rule;

				// Token: 0x04001957 RID: 6487
				public bool Layout_IsFlexible;

				// Token: 0x04001958 RID: 6488
				public float Layout_FlexibleWidth;
			}
		}

		// Token: 0x02000649 RID: 1609
		public class Indent : tkControl<T, TContext>
		{
			// Token: 0x060024F9 RID: 9465 RVA: 0x0009FA78 File Offset: 0x0009DC78
			public Indent(tkControl<T, TContext> control)
				: this(15f, control)
			{
			}

			// Token: 0x060024FA RID: 9466 RVA: 0x0009FA8C File Offset: 0x0009DC8C
			public Indent(tk<T, TContext>.Value<float> indent, tkControl<T, TContext> control)
			{
				this._indent = indent;
				this._control = control;
			}

			// Token: 0x060024FB RID: 9467 RVA: 0x0009FAA4 File Offset: 0x0009DCA4
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				float currentValue = this._indent.GetCurrentValue(obj, context);
				rect.x += currentValue;
				rect.width -= currentValue;
				return this._control.Edit(rect, obj, context, metadata);
			}

			// Token: 0x060024FC RID: 9468 RVA: 0x0009FAF0 File Offset: 0x0009DCF0
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				return this._control.GetHeight(obj, context, metadata);
			}

			// Token: 0x0400195F RID: 6495
			[ShowInInspector]
			private readonly tk<T, TContext>.Value<float> _indent;

			// Token: 0x04001960 RID: 6496
			[ShowInInspector]
			private readonly tkControl<T, TContext> _control;
		}

		// Token: 0x0200064A RID: 1610
		public class IntSlider : tkControl<T, TContext>
		{
			// Token: 0x060024FD RID: 9469 RVA: 0x0009FB00 File Offset: 0x0009DD00
			public IntSlider(tk<T, TContext>.Value<int> min, tk<T, TContext>.Value<int> max, Func<T, TContext, int> getValue, Action<T, TContext, int> setValue)
				: this(fiGUIContent.Empty, min, max, getValue, setValue)
			{
			}

			// Token: 0x060024FE RID: 9470 RVA: 0x0009FB18 File Offset: 0x0009DD18
			public IntSlider(tk<T, TContext>.Value<fiGUIContent> label, tk<T, TContext>.Value<int> min, tk<T, TContext>.Value<int> max, Func<T, TContext, int> getValue, Action<T, TContext, int> setValue)
			{
				this._label = label;
				this._min = min;
				this._max = max;
				this._getValue = getValue;
				this._setValue = setValue;
			}

			// Token: 0x060024FF RID: 9471 RVA: 0x0009FB48 File Offset: 0x0009DD48
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				int num = this._getValue(obj, context);
				int currentValue = this._min.GetCurrentValue(obj, context);
				int currentValue2 = this._max.GetCurrentValue(obj, context);
				fiLateBindings.EditorGUI.BeginChangeCheck();
				num = fiLateBindings.EditorGUI.IntSlider(rect, this._label.GetCurrentValue(obj, context), num, currentValue, currentValue2);
				if (fiLateBindings.EditorGUI.EndChangeCheck())
				{
					this._setValue(obj, context, num);
				}
				return obj;
			}

			// Token: 0x06002500 RID: 9472 RVA: 0x0009FBC4 File Offset: 0x0009DDC4
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				return fiLateBindings.EditorGUIUtility.singleLineHeight;
			}

			// Token: 0x04001961 RID: 6497
			private readonly tk<T, TContext>.Value<int> _min;

			// Token: 0x04001962 RID: 6498
			private readonly tk<T, TContext>.Value<int> _max;

			// Token: 0x04001963 RID: 6499
			private readonly Func<T, TContext, int> _getValue;

			// Token: 0x04001964 RID: 6500
			private readonly Action<T, TContext, int> _setValue;

			// Token: 0x04001965 RID: 6501
			private readonly tk<T, TContext>.Value<fiGUIContent> _label;
		}

		// Token: 0x0200064B RID: 1611
		public class Label : tkControl<T, TContext>
		{
			// Token: 0x06002501 RID: 9473 RVA: 0x0009FBCC File Offset: 0x0009DDCC
			public Label(fiGUIContent label)
				: this(label, FontStyle.Normal, null)
			{
			}

			// Token: 0x06002502 RID: 9474 RVA: 0x0009FBD8 File Offset: 0x0009DDD8
			public Label(tk<T, TContext>.Value<fiGUIContent> label)
				: this(label, FontStyle.Normal, null)
			{
			}

			// Token: 0x06002503 RID: 9475 RVA: 0x0009FBE4 File Offset: 0x0009DDE4
			public Label(tk<T, TContext>.Value<fiGUIContent>.Generator label)
				: this(label, FontStyle.Normal, null)
			{
			}

			// Token: 0x06002504 RID: 9476 RVA: 0x0009FBF0 File Offset: 0x0009DDF0
			public Label(fiGUIContent label, FontStyle fontStyle)
				: this(label, fontStyle, null)
			{
			}

			// Token: 0x06002505 RID: 9477 RVA: 0x0009FBFC File Offset: 0x0009DDFC
			public Label(tk<T, TContext>.Value<fiGUIContent> label, FontStyle fontStyle)
				: this(label, fontStyle, null)
			{
			}

			// Token: 0x06002506 RID: 9478 RVA: 0x0009FC08 File Offset: 0x0009DE08
			public Label(tk<T, TContext>.Value<fiGUIContent>.Generator label, FontStyle fontStyle)
				: this(label, fontStyle, null)
			{
			}

			// Token: 0x06002507 RID: 9479 RVA: 0x0009FC14 File Offset: 0x0009DE14
			public Label(fiGUIContent label, tkControl<T, TContext> control)
				: this(label, FontStyle.Normal, control)
			{
			}

			// Token: 0x06002508 RID: 9480 RVA: 0x0009FC20 File Offset: 0x0009DE20
			public Label(tk<T, TContext>.Value<fiGUIContent> label, tkControl<T, TContext> control)
				: this(label, FontStyle.Normal, control)
			{
			}

			// Token: 0x06002509 RID: 9481 RVA: 0x0009FC2C File Offset: 0x0009DE2C
			public Label(tk<T, TContext>.Value<fiGUIContent>.Generator label, tkControl<T, TContext> control)
				: this(label, FontStyle.Normal, control)
			{
			}

			// Token: 0x0600250A RID: 9482 RVA: 0x0009FC38 File Offset: 0x0009DE38
			public Label(fiGUIContent label, FontStyle fontStyle, tkControl<T, TContext> control)
				: this(tk<T, TContext>.Val<fiGUIContent>(label), fontStyle, control)
			{
			}

			// Token: 0x0600250B RID: 9483 RVA: 0x0009FC48 File Offset: 0x0009DE48
			public Label(tk<T, TContext>.Value<fiGUIContent> label, FontStyle fontStyle, tkControl<T, TContext> control)
			{
				this.GUIContent = label;
				this._fontStyle = fontStyle;
				this._control = control;
			}

			// Token: 0x0600250C RID: 9484 RVA: 0x0009FC68 File Offset: 0x0009DE68
			public Label(tk<T, TContext>.Value<fiGUIContent>.Generator label, FontStyle fontStyle, tkControl<T, TContext> control)
				: this(tk<T, TContext>.Val<fiGUIContent>(label), fontStyle, control)
			{
			}

			// Token: 0x0600250D RID: 9485 RVA: 0x0009FC78 File Offset: 0x0009DE78
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				fiGUIContent currentValue = this.GUIContent.GetCurrentValue(obj, context);
				Rect rect2 = rect;
				Rect rect3 = rect;
				bool flag = false;
				if (this._control != null && !currentValue.IsEmpty)
				{
					rect2.height = fiLateBindings.EditorGUIUtility.singleLineHeight;
					if (this.InlineControl)
					{
						rect2.width = fiGUI.PushLabelWidth(currentValue, rect2.width);
						flag = true;
						rect3.x += rect2.width;
						rect3.width -= rect2.width;
					}
					else
					{
						float num = rect2.height + fiLateBindings.EditorGUIUtility.standardVerticalSpacing;
						rect3.x += 15f;
						rect3.width -= 15f;
						rect3.y += num;
						rect3.height -= num;
					}
				}
				if (!currentValue.IsEmpty)
				{
					GUIStyle label = fiLateBindings.EditorStyles.label;
					FontStyle fontStyle = label.fontStyle;
					label.fontStyle = this._fontStyle;
					GUI.Label(rect2, currentValue, label);
					label.fontStyle = fontStyle;
				}
				if (this._control != null)
				{
					this._control.Edit(rect3, obj, context, metadata);
				}
				if (flag)
				{
					fiGUI.PopLabelWidth();
				}
				return obj;
			}

			// Token: 0x0600250E RID: 9486 RVA: 0x0009FDD0 File Offset: 0x0009DFD0
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				float num = 0f;
				if (!this.GUIContent.GetCurrentValue(obj, context).IsEmpty)
				{
					num += fiLateBindings.EditorGUIUtility.singleLineHeight;
				}
				if (this._control != null)
				{
					float height = this._control.GetHeight(obj, context, metadata);
					if (!this.InlineControl)
					{
						num += fiLateBindings.EditorGUIUtility.standardVerticalSpacing + height;
					}
				}
				return num;
			}

			// Token: 0x04001966 RID: 6502
			public tk<T, TContext>.Value<fiGUIContent> GUIContent;

			// Token: 0x04001967 RID: 6503
			[ShowInInspector]
			private readonly FontStyle _fontStyle;

			// Token: 0x04001968 RID: 6504
			[ShowInInspector]
			private readonly tkControl<T, TContext> _control;

			// Token: 0x04001969 RID: 6505
			public bool InlineControl;
		}

		// Token: 0x0200064C RID: 1612
		public class Margin : tkControl<T, TContext>
		{
			// Token: 0x0600250F RID: 9487 RVA: 0x0009FE34 File Offset: 0x0009E034
			public Margin(tk<T, TContext>.Value<float> margin, tkControl<T, TContext> control)
				: this(margin, margin, margin, margin, control)
			{
			}

			// Token: 0x06002510 RID: 9488 RVA: 0x0009FE44 File Offset: 0x0009E044
			public Margin(tk<T, TContext>.Value<float> left, tk<T, TContext>.Value<float> top, tkControl<T, TContext> control)
				: this(left, top, left, top, control)
			{
			}

			// Token: 0x06002511 RID: 9489 RVA: 0x0009FE54 File Offset: 0x0009E054
			public Margin(tk<T, TContext>.Value<float> left, tk<T, TContext>.Value<float> top, tk<T, TContext>.Value<float> right, tk<T, TContext>.Value<float> bottom, tkControl<T, TContext> control)
			{
				this._left = left;
				this._top = top;
				this._right = right;
				this._bottom = bottom;
				this._control = control;
			}

			// Token: 0x06002512 RID: 9490 RVA: 0x0009FE84 File Offset: 0x0009E084
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				float currentValue = this._left.GetCurrentValue(obj, context);
				float currentValue2 = this._right.GetCurrentValue(obj, context);
				float currentValue3 = this._top.GetCurrentValue(obj, context);
				float currentValue4 = this._bottom.GetCurrentValue(obj, context);
				rect.x += currentValue;
				rect.width -= currentValue + currentValue2;
				rect.y += currentValue3;
				rect.height -= currentValue3 + currentValue4;
				return this._control.Edit(rect, obj, context, metadata);
			}

			// Token: 0x06002513 RID: 9491 RVA: 0x0009FF2C File Offset: 0x0009E12C
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				float currentValue = this._top.GetCurrentValue(obj, context);
				float currentValue2 = this._bottom.GetCurrentValue(obj, context);
				return this._control.GetHeight(obj, context, metadata) + currentValue + currentValue2;
			}

			// Token: 0x0400196A RID: 6506
			[ShowInInspector]
			private readonly tk<T, TContext>.Value<float> _left;

			// Token: 0x0400196B RID: 6507
			[ShowInInspector]
			private readonly tk<T, TContext>.Value<float> _top;

			// Token: 0x0400196C RID: 6508
			[ShowInInspector]
			private readonly tk<T, TContext>.Value<float> _right;

			// Token: 0x0400196D RID: 6509
			[ShowInInspector]
			private readonly tk<T, TContext>.Value<float> _bottom;

			// Token: 0x0400196E RID: 6510
			[ShowInInspector]
			private readonly tkControl<T, TContext> _control;
		}

		// Token: 0x0200064D RID: 1613
		public class Popup : tkControl<T, TContext>
		{
			// Token: 0x06002514 RID: 9492 RVA: 0x0009FF70 File Offset: 0x0009E170
			public Popup(tk<T, TContext>.Value<fiGUIContent> label, tk<T, TContext>.Value<GUIContent[]> options, tk<T, TContext>.Value<int> currentSelection, tk<T, TContext>.Popup.OnSelectionChanged onSelectionChanged)
			{
				this._label = label;
				this._options = options;
				this._currentSelection = currentSelection;
				this._onSelectionChanged = onSelectionChanged;
			}

			// Token: 0x06002515 RID: 9493 RVA: 0x0009FF98 File Offset: 0x0009E198
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				fiGUIContent currentValue = this._label.GetCurrentValue(obj, context);
				int currentValue2 = this._currentSelection.GetCurrentValue(obj, context);
				GUIContent[] currentValue3 = this._options.GetCurrentValue(obj, context);
				int num = fiLateBindings.EditorGUI.Popup(rect, currentValue.AsGUIContent, currentValue2, currentValue3);
				if (currentValue2 != num)
				{
					obj = this._onSelectionChanged(obj, context, num);
				}
				return obj;
			}

			// Token: 0x06002516 RID: 9494 RVA: 0x000A0008 File Offset: 0x0009E208
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				return fiLateBindings.EditorGUIUtility.singleLineHeight;
			}

			// Token: 0x0400196F RID: 6511
			private readonly tk<T, TContext>.Value<fiGUIContent> _label;

			// Token: 0x04001970 RID: 6512
			private readonly tk<T, TContext>.Value<GUIContent[]> _options;

			// Token: 0x04001971 RID: 6513
			private readonly tk<T, TContext>.Value<int> _currentSelection;

			// Token: 0x04001972 RID: 6514
			private readonly tk<T, TContext>.Popup.OnSelectionChanged _onSelectionChanged;

			// Token: 0x0200064E RID: 1614
			// (Invoke) Token: 0x06002518 RID: 9496
			public delegate T OnSelectionChanged(T obj, TContext context, int selected);
		}

		// Token: 0x0200064F RID: 1615
		public class PropertyEditor : tkControl<T, TContext>
		{
			// Token: 0x0600251B RID: 9499 RVA: 0x000A0010 File Offset: 0x0009E210
			public PropertyEditor(string memberName)
			{
				this.InitializeFromMemberName(memberName);
			}

			// Token: 0x0600251C RID: 9500 RVA: 0x000A0020 File Offset: 0x0009E220
			public PropertyEditor(fiGUIContent label, string memberName)
				: this(memberName)
			{
				this._label = label;
			}

			// Token: 0x0600251D RID: 9501 RVA: 0x000A0038 File Offset: 0x0009E238
			public PropertyEditor(tk<T, TContext>.Value<fiGUIContent> label, string memberName)
				: this(memberName)
			{
				this._label = label;
			}

			// Token: 0x0600251E RID: 9502 RVA: 0x000A0048 File Offset: 0x0009E248
			public PropertyEditor(fiGUIContent label, Type fieldType, MemberInfo attributes, Func<T, TContext, object> getValue, Action<T, TContext, object> setValue)
			{
				this._label = label;
				this._fieldType = fieldType;
				this._attributes = attributes;
				this._getValue = getValue;
				this._setValue = setValue;
			}

			// Token: 0x0600251F RID: 9503 RVA: 0x000A007C File Offset: 0x0009E27C
			private void InitializeFromMemberName(string memberName)
			{
				InspectedProperty property = InspectedType.Get(typeof(T)).GetPropertyByName(memberName);
				if (property == null)
				{
					this._errorMessage = string.Concat(new string[]
					{
						"Unable to locate member `",
						memberName,
						"` on type `",
						typeof(T).CSharpName(),
						"`"
					});
					this._fieldType = typeof(T);
					this._attributes = null;
					this._getValue = (T o, TContext c) => default(T);
					this._setValue = delegate(T o, TContext c, object v)
					{
					};
					this._label = new fiGUIContent(memberName + " (unable to locate)");
					return;
				}
				this._fieldType = property.StorageType;
				this._attributes = property.MemberInfo;
				this._getValue = (T o, TContext c) => property.Read(o);
				this._setValue = delegate(T o, TContext c, object v)
				{
					property.Write(o, v);
				};
				this._label = new fiGUIContent(property.DisplayName);
			}

			// Token: 0x06002520 RID: 9504 RVA: 0x000A01D0 File Offset: 0x0009E3D0
			public static tk<T, TContext>.PropertyEditor Create<TEdited>(fiGUIContent label, MemberInfo attributes, Func<T, TContext, TEdited> getValue, Action<T, TContext, TEdited> setValue)
			{
				return new tk<T, TContext>.PropertyEditor(label, typeof(TEdited), attributes, (T o, TContext c) => getValue(o, c), delegate(T o, TContext c, object v)
				{
					setValue(o, c, (TEdited)((object)v));
				});
			}

			// Token: 0x06002521 RID: 9505 RVA: 0x000A021C File Offset: 0x0009E41C
			public static tk<T, TContext>.PropertyEditor Create<TEdited>(fiGUIContent label, Func<T, TContext, TEdited> getValue)
			{
				return new tk<T, TContext>.PropertyEditor(label, typeof(TEdited), null, (T o, TContext c) => getValue(o, c), null);
			}

			// Token: 0x06002522 RID: 9506 RVA: 0x000A0254 File Offset: 0x0009E454
			public static tk<T, TContext>.PropertyEditor Create<TEdited>(fiGUIContent label, Func<T, TContext, TEdited> getValue, Action<T, TContext, TEdited> setValue)
			{
				return new tk<T, TContext>.PropertyEditor(label, typeof(TEdited), null, (T o, TContext c) => getValue(o, c), delegate(T o, TContext c, object v)
				{
					setValue(o, c, (TEdited)((object)v));
				});
			}

			// Token: 0x06002523 RID: 9507 RVA: 0x000A02A0 File Offset: 0x0009E4A0
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				if (this._errorMessage != null)
				{
					fiLateBindings.EditorGUI.HelpBox(rect, this._errorMessage, CommentType.Error);
					return obj;
				}
				fiLateBindings.EditorGUI.BeginChangeCheck();
				fiLateBindings.EditorGUI.BeginDisabledGroup(this._setValue == null);
				object obj2 = this._getValue(obj, context);
				fiGraphMetadataChild fiGraphMetadataChild = new fiGraphMetadataChild
				{
					Metadata = base.GetInstanceMetadata(metadata)
				};
				fiGUIContent currentValue = this._label.GetCurrentValue(obj, context);
				object obj3 = fiLateBindings.PropertyEditor.Edit(this._fieldType, this._attributes, rect, currentValue, obj2, fiGraphMetadataChild, new Type[0]);
				fiLateBindings.EditorGUI.EndDisabledGroup();
				if (fiLateBindings.EditorGUI.EndChangeCheck() && this._setValue != null)
				{
					this._setValue(obj, context, obj3);
				}
				return obj;
			}

			// Token: 0x06002524 RID: 9508 RVA: 0x000A035C File Offset: 0x0009E55C
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				if (this._errorMessage != null)
				{
					return (float)fiCommentUtility.GetCommentHeight(this._errorMessage, CommentType.Error);
				}
				object obj2 = this._getValue(obj, context);
				fiGraphMetadataChild fiGraphMetadataChild = new fiGraphMetadataChild
				{
					Metadata = base.GetInstanceMetadata(metadata)
				};
				fiGUIContent currentValue = this._label.GetCurrentValue(obj, context);
				return fiLateBindings.PropertyEditor.GetElementHeight(this._fieldType, this._attributes, currentValue, obj2, fiGraphMetadataChild, new Type[0]);
			}

			// Token: 0x04001973 RID: 6515
			private MemberInfo _attributes;

			// Token: 0x04001974 RID: 6516
			private Func<T, TContext, object> _getValue;

			// Token: 0x04001975 RID: 6517
			private Action<T, TContext, object> _setValue;

			// Token: 0x04001976 RID: 6518
			private tk<T, TContext>.Value<fiGUIContent> _label;

			// Token: 0x04001977 RID: 6519
			private Type _fieldType;

			// Token: 0x04001978 RID: 6520
			private string _errorMessage;
		}

		// Token: 0x02000654 RID: 1620
		public class StyleProxy : tkControl<T, TContext>
		{
			// Token: 0x06002532 RID: 9522 RVA: 0x000A04AC File Offset: 0x0009E6AC
			public StyleProxy()
			{
			}

			// Token: 0x06002533 RID: 9523 RVA: 0x000A04B4 File Offset: 0x0009E6B4
			public StyleProxy(tkControl<T, TContext> control)
			{
				this.Control = control;
			}

			// Token: 0x06002534 RID: 9524 RVA: 0x000A04C4 File Offset: 0x0009E6C4
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				return this.Control.Edit(rect, obj, context, metadata);
			}

			// Token: 0x06002535 RID: 9525 RVA: 0x000A04D8 File Offset: 0x0009E6D8
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				return this.Control.GetHeight(obj, context, metadata);
			}

			// Token: 0x04001981 RID: 6529
			public tkControl<T, TContext> Control;
		}

		// Token: 0x02000655 RID: 1621
		public class ReadOnly : tk<T, TContext>.ReadOnlyIf
		{
			// Token: 0x06002536 RID: 9526 RVA: 0x000A04E8 File Offset: 0x0009E6E8
			public ReadOnly()
				: base(tk<T, TContext>.Val<bool>((T o) => true))
			{
			}
		}

		// Token: 0x02000656 RID: 1622
		public class ReadOnlyIf : tk<T, TContext>.ConditionalStyle
		{
			// Token: 0x06002538 RID: 9528 RVA: 0x000A0518 File Offset: 0x0009E718
			public ReadOnlyIf(tk<T, TContext>.Value<bool> isReadOnly)
				: base(new Func<T, TContext, bool>(isReadOnly.GetCurrentValue), delegate(T obj, TContext context)
				{
					fiLateBindings.EditorGUI.BeginDisabledGroup(true);
					return null;
				}, delegate(T obj, TContext context, object state)
				{
					fiLateBindings.EditorGUI.EndDisabledGroup();
				})
			{
			}

			// Token: 0x06002539 RID: 9529 RVA: 0x000A0578 File Offset: 0x0009E778
			public ReadOnlyIf(tk<T, TContext>.Value<bool>.Generator isReadOnly)
				: this(tk<T, TContext>.Val<bool>(isReadOnly))
			{
			}

			// Token: 0x0600253A RID: 9530 RVA: 0x000A0588 File Offset: 0x0009E788
			public ReadOnlyIf(tk<T, TContext>.Value<bool>.GeneratorNoContext isReadOnly)
				: this(tk<T, TContext>.Val<bool>(isReadOnly))
			{
			}
		}

		// Token: 0x02000657 RID: 1623
		public class ShowIf : tkControl<T, TContext>
		{
			// Token: 0x0600253D RID: 9533 RVA: 0x000A05AC File Offset: 0x0009E7AC
			public ShowIf(tk<T, TContext>.Value<bool> shouldDisplay, tkControl<T, TContext> control)
			{
				this._shouldDisplay = shouldDisplay;
				this._control = control;
			}

			// Token: 0x0600253E RID: 9534 RVA: 0x000A05C4 File Offset: 0x0009E7C4
			public ShowIf(tk<T, TContext>.Value<bool>.Generator shouldDisplay, tkControl<T, TContext> control)
				: this(new tk<T, TContext>.Value<bool>(shouldDisplay), control)
			{
			}

			// Token: 0x0600253F RID: 9535 RVA: 0x000A05D4 File Offset: 0x0009E7D4
			public ShowIf(tk<T, TContext>.Value<bool>.GeneratorNoContext shouldDisplay, tkControl<T, TContext> control)
				: this(new tk<T, TContext>.Value<bool>(shouldDisplay), control)
			{
			}

			// Token: 0x06002540 RID: 9536 RVA: 0x000A05E4 File Offset: 0x0009E7E4
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				return this._control.Edit(rect, obj, context, metadata);
			}

			// Token: 0x06002541 RID: 9537 RVA: 0x000A05F8 File Offset: 0x0009E7F8
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				return this._control.GetHeight(obj, context, metadata);
			}

			// Token: 0x06002542 RID: 9538 RVA: 0x000A0608 File Offset: 0x0009E808
			public override bool ShouldShow(T obj, TContext context, fiGraphMetadata metadata)
			{
				return this._shouldDisplay.GetCurrentValue(obj, context);
			}

			// Token: 0x04001985 RID: 6533
			private readonly tk<T, TContext>.Value<bool> _shouldDisplay;

			// Token: 0x04001986 RID: 6534
			[ShowInInspector]
			private readonly tkControl<T, TContext> _control;
		}

		// Token: 0x02000658 RID: 1624
		public class Slider : tkControl<T, TContext>
		{
			// Token: 0x06002543 RID: 9539 RVA: 0x000A0628 File Offset: 0x0009E828
			public Slider(tk<T, TContext>.Value<float> min, tk<T, TContext>.Value<float> max, Func<T, TContext, float> getValue, Action<T, TContext, float> setValue)
				: this(fiGUIContent.Empty, min, max, getValue, setValue)
			{
			}

			// Token: 0x06002544 RID: 9540 RVA: 0x000A0640 File Offset: 0x0009E840
			public Slider(tk<T, TContext>.Value<fiGUIContent> label, tk<T, TContext>.Value<float> min, tk<T, TContext>.Value<float> max, Func<T, TContext, float> getValue, Action<T, TContext, float> setValue)
			{
				this._label = label;
				this._min = min;
				this._max = max;
				this._getValue = getValue;
				this._setValue = setValue;
			}

			// Token: 0x06002545 RID: 9541 RVA: 0x000A0670 File Offset: 0x0009E870
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				float num = this._getValue(obj, context);
				float currentValue = this._min.GetCurrentValue(obj, context);
				float currentValue2 = this._max.GetCurrentValue(obj, context);
				fiLateBindings.EditorGUI.BeginChangeCheck();
				num = fiLateBindings.EditorGUI.Slider(rect, this._label.GetCurrentValue(obj, context), num, currentValue, currentValue2);
				if (fiLateBindings.EditorGUI.EndChangeCheck())
				{
					this._setValue(obj, context, num);
				}
				return obj;
			}

			// Token: 0x06002546 RID: 9542 RVA: 0x000A06EC File Offset: 0x0009E8EC
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				return fiLateBindings.EditorGUIUtility.singleLineHeight;
			}

			// Token: 0x04001987 RID: 6535
			private readonly tk<T, TContext>.Value<float> _min;

			// Token: 0x04001988 RID: 6536
			private readonly tk<T, TContext>.Value<float> _max;

			// Token: 0x04001989 RID: 6537
			private readonly Func<T, TContext, float> _getValue;

			// Token: 0x0400198A RID: 6538
			private readonly Action<T, TContext, float> _setValue;

			// Token: 0x0400198B RID: 6539
			private readonly tk<T, TContext>.Value<fiGUIContent> _label;
		}

		// Token: 0x02000659 RID: 1625
		public class VerticalGroup : tkControl<T, TContext>, IEnumerable
		{
			// Token: 0x06002547 RID: 9543 RVA: 0x000A06F4 File Offset: 0x0009E8F4
			public VerticalGroup()
				: this(fiLateBindings.EditorGUIUtility.standardVerticalSpacing)
			{
			}

			// Token: 0x06002548 RID: 9544 RVA: 0x000A0704 File Offset: 0x0009E904
			public VerticalGroup(float marginBetweenItems)
			{
				this._marginBetweenItems = marginBetweenItems;
			}

			// Token: 0x170006EC RID: 1772
			// (get) Token: 0x06002549 RID: 9545 RVA: 0x000A0720 File Offset: 0x0009E920
			protected override IEnumerable<tkIControl> NonMemberChildControls
			{
				get
				{
					foreach (tk<T, TContext>.VerticalGroup.SectionItem item in this._items)
					{
						yield return item.Rule;
					}
					yield break;
				}
			}

			// Token: 0x0600254A RID: 9546 RVA: 0x000A0744 File Offset: 0x0009E944
			public void Add(tkControl<T, TContext> rule)
			{
				this.InternalAdd(rule);
			}

			// Token: 0x0600254B RID: 9547 RVA: 0x000A0750 File Offset: 0x0009E950
			private void InternalAdd(tkControl<T, TContext> rule)
			{
				this._items.Add(new tk<T, TContext>.VerticalGroup.SectionItem
				{
					Rule = rule
				});
			}

			// Token: 0x0600254C RID: 9548 RVA: 0x000A077C File Offset: 0x0009E97C
			protected override T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
			{
				for (int i = 0; i < this._items.Count; i++)
				{
					tk<T, TContext>.VerticalGroup.SectionItem sectionItem = this._items[i];
					if (sectionItem.Rule.ShouldShow(obj, context, metadata))
					{
						float height = sectionItem.Rule.GetHeight(obj, context, metadata);
						Rect rect2 = rect;
						rect2.height = height;
						obj = sectionItem.Rule.Edit(rect2, obj, context, metadata);
						rect.y += height;
						rect.y += this._marginBetweenItems;
					}
				}
				return obj;
			}

			// Token: 0x0600254D RID: 9549 RVA: 0x000A0820 File Offset: 0x0009EA20
			protected override float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata)
			{
				float num = 0f;
				for (int i = 0; i < this._items.Count; i++)
				{
					tk<T, TContext>.VerticalGroup.SectionItem sectionItem = this._items[i];
					if (sectionItem.Rule.ShouldShow(obj, context, metadata))
					{
						num += sectionItem.Rule.GetHeight(obj, context, metadata);
						if (i != this._items.Count - 1)
						{
							num += this._marginBetweenItems;
						}
					}
				}
				return num;
			}

			// Token: 0x0600254E RID: 9550 RVA: 0x000A08A4 File Offset: 0x0009EAA4
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400198C RID: 6540
			[ShowInInspector]
			private readonly List<tk<T, TContext>.VerticalGroup.SectionItem> _items = new List<tk<T, TContext>.VerticalGroup.SectionItem>();

			// Token: 0x0400198D RID: 6541
			private readonly float _marginBetweenItems;

			// Token: 0x0200065A RID: 1626
			private struct SectionItem
			{
				// Token: 0x0400198E RID: 6542
				public tkControl<T, TContext> Rule;
			}
		}

		// Token: 0x0200065C RID: 1628
		public struct Value<TValue>
		{
			// Token: 0x06002557 RID: 9559 RVA: 0x000A0A3C File Offset: 0x0009EC3C
			public Value(tk<T, TContext>.Value<TValue>.Generator generator)
			{
				this._generator = generator;
				this._direct = default(TValue);
			}

			// Token: 0x06002558 RID: 9560 RVA: 0x000A0A60 File Offset: 0x0009EC60
			public Value(tk<T, TContext>.Value<TValue>.GeneratorNoContext generator)
			{
				this._generator = (T o, TContext context) => generator(o);
				this._direct = default(TValue);
			}

			// Token: 0x06002559 RID: 9561 RVA: 0x000A0A9C File Offset: 0x0009EC9C
			public TValue GetCurrentValue(T instance, TContext context)
			{
				if (this._generator == null)
				{
					return this._direct;
				}
				return this._generator(instance, context);
			}

			// Token: 0x0600255A RID: 9562 RVA: 0x000A0AC0 File Offset: 0x0009ECC0
			public static implicit operator tk<T, TContext>.Value<TValue>(TValue direct)
			{
				return new tk<T, TContext>.Value<TValue>
				{
					_generator = null,
					_direct = direct
				};
			}

			// Token: 0x0600255B RID: 9563 RVA: 0x000A0AE8 File Offset: 0x0009ECE8
			public static implicit operator tk<T, TContext>.Value<TValue>(tk<T, TContext>.Value<TValue>.Generator generator)
			{
				return new tk<T, TContext>.Value<TValue>
				{
					_generator = generator,
					_direct = default(TValue)
				};
			}

			// Token: 0x0600255C RID: 9564 RVA: 0x000A0B18 File Offset: 0x0009ED18
			public static implicit operator tk<T, TContext>.Value<TValue>(tk<T, TContext>.Value<TValue>.GeneratorNoContext generator)
			{
				return new tk<T, TContext>.Value<TValue>
				{
					_generator = (T obj, TContext context) => generator(obj),
					_direct = default(TValue)
				};
			}

			// Token: 0x0600255D RID: 9565 RVA: 0x000A0B60 File Offset: 0x0009ED60
			public static implicit operator tk<T, TContext>.Value<TValue>(Func<T, int, TValue> generator)
			{
				return default(tk<T, TContext>.Value<TValue>);
			}

			// Token: 0x0600255E RID: 9566 RVA: 0x000A0B78 File Offset: 0x0009ED78
			public static implicit operator tk<T, TContext>.Value<TValue>(Func<T, TValue> generator)
			{
				return new tk<T, TContext>.Value<TValue>
				{
					_generator = (T obj, TContext context) => generator(obj),
					_direct = default(TValue)
				};
			}

			// Token: 0x04001995 RID: 6549
			private tk<T, TContext>.Value<TValue>.Generator _generator;

			// Token: 0x04001996 RID: 6550
			private TValue _direct;

			// Token: 0x0200065D RID: 1629
			// (Invoke) Token: 0x06002560 RID: 9568
			public delegate TValue Generator(T input, TContext context);

			// Token: 0x0200065E RID: 1630
			// (Invoke) Token: 0x06002564 RID: 9572
			public delegate TValue GeneratorNoContext(T input);
		}
	}
}
