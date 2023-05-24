using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001737 RID: 5943
public class AmmonomiconInstanceManager : MonoBehaviour
{
	// Token: 0x17001491 RID: 5265
	// (get) Token: 0x06008A36 RID: 35382 RVA: 0x00399090 File Offset: 0x00397290
	// (set) Token: 0x06008A37 RID: 35383 RVA: 0x00399098 File Offset: 0x00397298
	public int CurrentlySelectedTabIndex
	{
		get
		{
			return this.m_currentlySelectedBookmark;
		}
		set
		{
			this.m_currentlySelectedBookmark = value;
		}
	}

	// Token: 0x17001492 RID: 5266
	// (get) Token: 0x06008A38 RID: 35384 RVA: 0x003990A4 File Offset: 0x003972A4
	public dfGUIManager GuiManager
	{
		get
		{
			if (this.m_manager == null)
			{
				this.m_manager = base.GetComponent<dfGUIManager>();
			}
			return this.m_manager;
		}
	}

	// Token: 0x17001493 RID: 5267
	// (get) Token: 0x06008A39 RID: 35385 RVA: 0x003990CC File Offset: 0x003972CC
	public bool BookmarkHasFocus
	{
		get
		{
			for (int i = 0; i < this.bookmarks.Length; i++)
			{
				if (this.bookmarks[i].IsFocused)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x06008A3A RID: 35386 RVA: 0x00399108 File Offset: 0x00397308
	public void Open()
	{
		this.m_currentlySelectedBookmark = 0;
		base.StartCoroutine(this.HandleOpenAmmonomicon());
	}

	// Token: 0x06008A3B RID: 35387 RVA: 0x00399120 File Offset: 0x00397320
	public void Close()
	{
		for (int i = 0; i < this.bookmarks.Length; i++)
		{
			this.bookmarks[i].Disable();
		}
	}

	// Token: 0x06008A3C RID: 35388 RVA: 0x00399154 File Offset: 0x00397354
	public void LateUpdate()
	{
		if (dfGUIManager.ActiveControl == null && this.bookmarks != null && this.bookmarks[this.m_currentlySelectedBookmark] != null)
		{
			this.bookmarks[this.m_currentlySelectedBookmark].ForceFocus();
		}
	}

	// Token: 0x06008A3D RID: 35389 RVA: 0x003991A8 File Offset: 0x003973A8
	public void OpenDeath()
	{
		this.m_currentlySelectedBookmark = this.bookmarks.Length - 1;
		base.StartCoroutine(this.HandleOpenAmmonomiconDeath());
	}

	// Token: 0x06008A3E RID: 35390 RVA: 0x003991C8 File Offset: 0x003973C8
	public IEnumerator InvariantWait(float t)
	{
		float elapsed = 0f;
		while (elapsed < t)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008A3F RID: 35391 RVA: 0x003991E4 File Offset: 0x003973E4
	public IEnumerator HandleOpenAmmonomiconDeath()
	{
		int currentBookmark = 0;
		while (currentBookmark < this.bookmarks.Length)
		{
			this.bookmarks[currentBookmark].TriggerAppearAnimation();
			if (currentBookmark != this.bookmarks.Length - 1)
			{
				this.bookmarks[currentBookmark].Disable();
			}
			currentBookmark++;
			yield return base.StartCoroutine(this.InvariantWait(0.1f));
		}
		this.m_currentlySelectedBookmark = this.bookmarks.Length - 1;
		this.bookmarks[this.m_currentlySelectedBookmark].IsCurrentPage = true;
		yield break;
	}

	// Token: 0x06008A40 RID: 35392 RVA: 0x00399200 File Offset: 0x00397400
	public IEnumerator HandleOpenAmmonomicon()
	{
		dfGUIManager.SetFocus(null, true);
		int currentBookmark = 0;
		this.bookmarks[this.m_currentlySelectedBookmark].IsCurrentPage = true;
		while (currentBookmark < this.bookmarks.Length - 1)
		{
			if (!AmmonomiconController.Instance.IsOpen)
			{
				yield break;
			}
			this.bookmarks[currentBookmark].TriggerAppearAnimation();
			currentBookmark++;
			yield return base.StartCoroutine(this.InvariantWait(0.05f));
		}
		this.bookmarks[this.m_currentlySelectedBookmark].IsCurrentPage = true;
		yield break;
	}

	// Token: 0x040090BB RID: 37051
	public AmmonomiconBookmarkController[] bookmarks;

	// Token: 0x040090BC RID: 37052
	private int m_currentlySelectedBookmark;

	// Token: 0x040090BD RID: 37053
	private dfGUIManager m_manager;
}
