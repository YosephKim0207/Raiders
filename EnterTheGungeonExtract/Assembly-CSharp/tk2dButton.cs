using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000B99 RID: 2969
[AddComponentMenu("2D Toolkit/Deprecated/GUI/tk2dButton")]
public class tk2dButton : MonoBehaviour
{
	// Token: 0x14000082 RID: 130
	// (add) Token: 0x06003E33 RID: 15923 RVA: 0x0013AD50 File Offset: 0x00138F50
	// (remove) Token: 0x06003E34 RID: 15924 RVA: 0x0013AD88 File Offset: 0x00138F88
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event tk2dButton.ButtonHandlerDelegate ButtonPressedEvent;

	// Token: 0x14000083 RID: 131
	// (add) Token: 0x06003E35 RID: 15925 RVA: 0x0013ADC0 File Offset: 0x00138FC0
	// (remove) Token: 0x06003E36 RID: 15926 RVA: 0x0013ADF8 File Offset: 0x00138FF8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event tk2dButton.ButtonHandlerDelegate ButtonAutoFireEvent;

	// Token: 0x14000084 RID: 132
	// (add) Token: 0x06003E37 RID: 15927 RVA: 0x0013AE30 File Offset: 0x00139030
	// (remove) Token: 0x06003E38 RID: 15928 RVA: 0x0013AE68 File Offset: 0x00139068
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event tk2dButton.ButtonHandlerDelegate ButtonDownEvent;

	// Token: 0x14000085 RID: 133
	// (add) Token: 0x06003E39 RID: 15929 RVA: 0x0013AEA0 File Offset: 0x001390A0
	// (remove) Token: 0x06003E3A RID: 15930 RVA: 0x0013AED8 File Offset: 0x001390D8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event tk2dButton.ButtonHandlerDelegate ButtonUpEvent;

	// Token: 0x06003E3B RID: 15931 RVA: 0x0013AF10 File Offset: 0x00139110
	private void OnEnable()
	{
		this.buttonDown = false;
	}

	// Token: 0x06003E3C RID: 15932 RVA: 0x0013AF1C File Offset: 0x0013911C
	private void Start()
	{
		if (this.viewCamera == null)
		{
			Transform transform = base.transform;
			while (transform && transform.GetComponent<Camera>() == null)
			{
				transform = transform.parent;
			}
			if (transform && transform.GetComponent<Camera>() != null)
			{
				this.viewCamera = transform.GetComponent<Camera>();
			}
			if (this.viewCamera == null && tk2dCamera.Instance)
			{
				this.viewCamera = tk2dCamera.Instance.GetComponent<Camera>();
			}
			if (this.viewCamera == null)
			{
				this.viewCamera = Camera.main;
			}
		}
		this.sprite = base.GetComponent<tk2dBaseSprite>();
		if (this.sprite)
		{
			this.UpdateSpriteIds();
		}
		if (base.GetComponent<Collider>() == null)
		{
			BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
			Vector3 size = boxCollider.size;
			size.z = 0.2f;
			boxCollider.size = size;
		}
		if ((this.buttonDownSound != null || this.buttonPressedSound != null || this.buttonUpSound != null) && base.GetComponent<AudioSource>() == null)
		{
			AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}
	}

	// Token: 0x06003E3D RID: 15933 RVA: 0x0013B090 File Offset: 0x00139290
	public void UpdateSpriteIds()
	{
		this.buttonDownSpriteId = ((this.buttonDownSprite.Length <= 0) ? (-1) : this.sprite.GetSpriteIdByName(this.buttonDownSprite));
		this.buttonUpSpriteId = ((this.buttonUpSprite.Length <= 0) ? (-1) : this.sprite.GetSpriteIdByName(this.buttonUpSprite));
		this.buttonPressedSpriteId = ((this.buttonPressedSprite.Length <= 0) ? (-1) : this.sprite.GetSpriteIdByName(this.buttonPressedSprite));
	}

	// Token: 0x06003E3E RID: 15934 RVA: 0x0013B128 File Offset: 0x00139328
	private void PlaySound(AudioClip source)
	{
		if (base.GetComponent<AudioSource>() && source)
		{
			base.GetComponent<AudioSource>().PlayOneShot(source);
		}
	}

	// Token: 0x06003E3F RID: 15935 RVA: 0x0013B154 File Offset: 0x00139354
	private IEnumerator coScale(Vector3 defaultScale, float startScale, float endScale)
	{
		float t0 = Time.realtimeSinceStartup;
		Vector3 scale = defaultScale;
		for (float s = 0f; s < this.scaleTime; s = Time.realtimeSinceStartup - t0)
		{
			float t = Mathf.Clamp01(s / this.scaleTime);
			float scl = Mathf.Lerp(startScale, endScale, t);
			scale = defaultScale * scl;
			base.transform.localScale = scale;
			yield return 0;
		}
		base.transform.localScale = defaultScale * endScale;
		yield break;
	}

	// Token: 0x06003E40 RID: 15936 RVA: 0x0013B184 File Offset: 0x00139384
	private IEnumerator LocalWaitForSeconds(float seconds)
	{
		float t0 = Time.realtimeSinceStartup;
		for (float s = 0f; s < seconds; s = Time.realtimeSinceStartup - t0)
		{
			yield return 0;
		}
		yield break;
	}

	// Token: 0x06003E41 RID: 15937 RVA: 0x0013B1A0 File Offset: 0x001393A0
	private IEnumerator coHandleButtonPress(int fingerId)
	{
		this.buttonDown = true;
		bool buttonPressed = true;
		Vector3 defaultScale = base.transform.localScale;
		if (this.targetScale != 1f)
		{
			yield return base.StartCoroutine(this.coScale(defaultScale, 1f, this.targetScale));
		}
		this.PlaySound(this.buttonDownSound);
		if (this.buttonDownSpriteId != -1)
		{
			this.sprite.spriteId = this.buttonDownSpriteId;
		}
		if (this.ButtonDownEvent != null)
		{
			this.ButtonDownEvent(this);
		}
		for (;;)
		{
			Vector3 cursorPosition = Vector3.zero;
			bool cursorActive = true;
			if (fingerId != -1)
			{
				bool flag = false;
				for (int i = 0; i < Input.touchCount; i++)
				{
					Touch touch = Input.GetTouch(i);
					if (touch.fingerId == fingerId)
					{
						if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
						{
							break;
						}
						cursorPosition = touch.position;
						flag = true;
					}
				}
				if (!flag)
				{
					cursorActive = false;
				}
			}
			else
			{
				if (!Input.GetMouseButton(0))
				{
					cursorActive = false;
				}
				cursorPosition = Input.mousePosition;
			}
			if (!cursorActive)
			{
				break;
			}
			Ray ray = this.viewCamera.ScreenPointToRay(cursorPosition);
			RaycastHit hitInfo;
			bool colliderHit = base.GetComponent<Collider>().Raycast(ray, out hitInfo, float.PositiveInfinity);
			if (buttonPressed && !colliderHit)
			{
				if (this.targetScale != 1f)
				{
					yield return base.StartCoroutine(this.coScale(defaultScale, this.targetScale, 1f));
				}
				this.PlaySound(this.buttonUpSound);
				if (this.buttonUpSpriteId != -1)
				{
					this.sprite.spriteId = this.buttonUpSpriteId;
				}
				if (this.ButtonUpEvent != null)
				{
					this.ButtonUpEvent(this);
				}
				buttonPressed = false;
			}
			else if (!buttonPressed && colliderHit)
			{
				if (this.targetScale != 1f)
				{
					yield return base.StartCoroutine(this.coScale(defaultScale, 1f, this.targetScale));
				}
				this.PlaySound(this.buttonDownSound);
				if (this.buttonDownSpriteId != -1)
				{
					this.sprite.spriteId = this.buttonDownSpriteId;
				}
				if (this.ButtonDownEvent != null)
				{
					this.ButtonDownEvent(this);
				}
				buttonPressed = true;
			}
			if (buttonPressed && this.ButtonAutoFireEvent != null)
			{
				this.ButtonAutoFireEvent(this);
			}
			yield return 0;
		}
		if (buttonPressed)
		{
			if (this.targetScale != 1f)
			{
				yield return base.StartCoroutine(this.coScale(defaultScale, this.targetScale, 1f));
			}
			this.PlaySound(this.buttonPressedSound);
			if (this.buttonPressedSpriteId != -1)
			{
				this.sprite.spriteId = this.buttonPressedSpriteId;
			}
			if (this.targetObject)
			{
				this.targetObject.SendMessage(this.messageName);
			}
			if (this.ButtonUpEvent != null)
			{
				this.ButtonUpEvent(this);
			}
			if (this.ButtonPressedEvent != null)
			{
				this.ButtonPressedEvent(this);
			}
			if (base.gameObject.activeInHierarchy)
			{
				yield return base.StartCoroutine(this.LocalWaitForSeconds(this.pressedWaitTime));
			}
			if (this.buttonUpSpriteId != -1)
			{
				this.sprite.spriteId = this.buttonUpSpriteId;
			}
		}
		this.buttonDown = false;
		yield break;
	}

	// Token: 0x06003E42 RID: 15938 RVA: 0x0013B1C4 File Offset: 0x001393C4
	private void Update()
	{
		if (this.buttonDown)
		{
			return;
		}
		bool flag = false;
		if (Input.multiTouchEnabled)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began)
				{
					Ray ray = this.viewCamera.ScreenPointToRay(touch.position);
					RaycastHit raycastHit;
					if (base.GetComponent<Collider>().Raycast(ray, out raycastHit, 100000000f) && !Physics.Raycast(ray, raycastHit.distance - 0.01f))
					{
						base.StartCoroutine(this.coHandleButtonPress(touch.fingerId));
						flag = true;
						break;
					}
				}
			}
		}
		if (!flag && Input.GetMouseButtonDown(0))
		{
			Ray ray2 = this.viewCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit2;
			if (base.GetComponent<Collider>().Raycast(ray2, out raycastHit2, 100000000f) && !Physics.Raycast(ray2, raycastHit2.distance - 0.01f))
			{
				base.StartCoroutine(this.coHandleButtonPress(-1));
			}
		}
	}

	// Token: 0x040030D2 RID: 12498
	public Camera viewCamera;

	// Token: 0x040030D3 RID: 12499
	public string buttonDownSprite = "button_down";

	// Token: 0x040030D4 RID: 12500
	public string buttonUpSprite = "button_up";

	// Token: 0x040030D5 RID: 12501
	public string buttonPressedSprite = "button_up";

	// Token: 0x040030D6 RID: 12502
	private int buttonDownSpriteId = -1;

	// Token: 0x040030D7 RID: 12503
	private int buttonUpSpriteId = -1;

	// Token: 0x040030D8 RID: 12504
	private int buttonPressedSpriteId = -1;

	// Token: 0x040030D9 RID: 12505
	public AudioClip buttonDownSound;

	// Token: 0x040030DA RID: 12506
	public AudioClip buttonUpSound;

	// Token: 0x040030DB RID: 12507
	public AudioClip buttonPressedSound;

	// Token: 0x040030DC RID: 12508
	public GameObject targetObject;

	// Token: 0x040030DD RID: 12509
	public string messageName = string.Empty;

	// Token: 0x040030E2 RID: 12514
	private tk2dBaseSprite sprite;

	// Token: 0x040030E3 RID: 12515
	private bool buttonDown;

	// Token: 0x040030E4 RID: 12516
	public float targetScale = 1.1f;

	// Token: 0x040030E5 RID: 12517
	public float scaleTime = 0.05f;

	// Token: 0x040030E6 RID: 12518
	public float pressedWaitTime = 0.3f;

	// Token: 0x02000B9A RID: 2970
	// (Invoke) Token: 0x06003E44 RID: 15940
	public delegate void ButtonHandlerDelegate(tk2dButton source);
}
