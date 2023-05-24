using System;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x0200055D RID: 1373
	public class fiGUIContent
	{
		// Token: 0x060020AA RID: 8362 RVA: 0x00090BB0 File Offset: 0x0008EDB0
		public fiGUIContent()
			: this(string.Empty, string.Empty, null)
		{
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x00090BC4 File Offset: 0x0008EDC4
		public fiGUIContent(string text)
			: this(text, string.Empty, null)
		{
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x00090BD4 File Offset: 0x0008EDD4
		public fiGUIContent(string text, string tooltip)
			: this(text, tooltip, null)
		{
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00090BE0 File Offset: 0x0008EDE0
		public fiGUIContent(string text, string tooltip, Texture image)
		{
			this._text = text;
			this._tooltip = tooltip;
			this._image = image;
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00090C00 File Offset: 0x0008EE00
		public fiGUIContent(Texture image)
			: this(string.Empty, string.Empty, image)
		{
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x00090C14 File Offset: 0x0008EE14
		public fiGUIContent(Texture image, string tooltip)
			: this(string.Empty, tooltip, image)
		{
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060020B0 RID: 8368 RVA: 0x00090C24 File Offset: 0x0008EE24
		public GUIContent AsGUIContent
		{
			get
			{
				return new GUIContent(this._text, this._image, this._tooltip);
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060020B1 RID: 8369 RVA: 0x00090C40 File Offset: 0x0008EE40
		public bool IsEmpty
		{
			get
			{
				return string.IsNullOrEmpty(this._text) && string.IsNullOrEmpty(this._tooltip) && !(this._image != null);
			}
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x00090C7C File Offset: 0x0008EE7C
		public static implicit operator GUIContent(fiGUIContent label)
		{
			if (label == null)
			{
				return GUIContent.none;
			}
			return label.AsGUIContent;
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x00090C90 File Offset: 0x0008EE90
		public static implicit operator fiGUIContent(string text)
		{
			return new fiGUIContent
			{
				_text = text
			};
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x00090CAC File Offset: 0x0008EEAC
		public static implicit operator fiGUIContent(GUIContent label)
		{
			return new fiGUIContent
			{
				_text = label.text,
				_tooltip = label.tooltip,
				_image = label.image
			};
		}

		// Token: 0x040017B3 RID: 6067
		public static fiGUIContent Empty = new fiGUIContent();

		// Token: 0x040017B4 RID: 6068
		private string _text;

		// Token: 0x040017B5 RID: 6069
		private string _tooltip;

		// Token: 0x040017B6 RID: 6070
		private Texture _image;
	}
}
