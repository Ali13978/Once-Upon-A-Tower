using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMPro
{
	[AddComponentMenu("UI/TextMeshPro - Input Field", 11)]
	public class TMP_InputField : Selectable, IUpdateSelectedHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, ISubmitHandler, ICanvasElement, IEventSystemHandler
	{
		public enum ContentType
		{
			Standard,
			Autocorrected,
			IntegerNumber,
			DecimalNumber,
			Alphanumeric,
			Name,
			EmailAddress,
			Password,
			Pin,
			Custom
		}

		public enum InputType
		{
			Standard,
			AutoCorrect,
			Password
		}

		public enum CharacterValidation
		{
			None,
			Integer,
			Decimal,
			Alphanumeric,
			Name,
			EmailAddress
		}

		public enum LineType
		{
			SingleLine,
			MultiLineSubmit,
			MultiLineNewline
		}

		public delegate char OnValidateInput(string text, int charIndex, char addedChar);

		[Serializable]
		public class SubmitEvent : UnityEvent<string>
		{
		}

		[Serializable]
		public class OnChangeEvent : UnityEvent<string>
		{
		}

		protected enum EditState
		{
			Continue,
			Finish
		}

		protected TouchScreenKeyboard m_Keyboard;

		private static readonly char[] kSeparators = new char[6]
		{
			' ',
			'.',
			',',
			'\t',
			'\r',
			'\n'
		};

		[SerializeField]
		protected RectTransform m_TextViewport;

		[SerializeField]
		protected TMP_Text m_TextComponent;

		protected RectTransform m_TextComponentRectTransform;

		[SerializeField]
		protected Graphic m_Placeholder;

		[SerializeField]
		private ContentType m_ContentType;

		[SerializeField]
		private InputType m_InputType;

		[SerializeField]
		private char m_AsteriskChar = '*';

		[SerializeField]
		private TouchScreenKeyboardType m_KeyboardType;

		[SerializeField]
		private LineType m_LineType;

		[SerializeField]
		private bool m_HideMobileInput;

		[SerializeField]
		private CharacterValidation m_CharacterValidation;

		[SerializeField]
		private int m_CharacterLimit;

		[SerializeField]
		private SubmitEvent m_OnEndEdit = new SubmitEvent();

		[SerializeField]
		private SubmitEvent m_OnSubmit = new SubmitEvent();

		[SerializeField]
		private SubmitEvent m_OnFocusLost = new SubmitEvent();

		[SerializeField]
		private OnChangeEvent m_OnValueChanged = new OnChangeEvent();

		[SerializeField]
		private OnValidateInput m_OnValidateInput;

		[SerializeField]
		private Color m_CaretColor = new Color(10f / 51f, 10f / 51f, 10f / 51f, 1f);

		[SerializeField]
		private bool m_CustomCaretColor;

		[SerializeField]
		private Color m_SelectionColor = new Color(56f / 85f, 206f / 255f, 1f, 64f / 85f);

		[SerializeField]
		protected string m_Text = string.Empty;

		[SerializeField]
		[Range(0f, 4f)]
		private float m_CaretBlinkRate = 0.85f;

		[SerializeField]
		[Range(1f, 5f)]
		private int m_CaretWidth = 1;

		[SerializeField]
		private bool m_ReadOnly;

		[SerializeField]
		private bool m_RichText = true;

		protected int m_StringPosition;

		protected int m_StringSelectPosition;

		protected int m_CaretPosition;

		protected int m_CaretSelectPosition;

		private RectTransform caretRectTrans;

		protected UIVertex[] m_CursorVerts;

		private CanvasRenderer m_CachedInputRenderer;

		[NonSerialized]
		protected Mesh m_Mesh;

		private bool m_AllowInput;

		private bool m_HasLostFocus;

		private bool m_ShouldActivateNextUpdate;

		private bool m_UpdateDrag;

		private bool m_DragPositionOutOfBounds;

		private const float kHScrollSpeed = 0.05f;

		private const float kVScrollSpeed = 0.1f;

		protected bool m_CaretVisible;

		private Coroutine m_BlinkCoroutine;

		private float m_BlinkStartTime;

		protected int m_DrawStart;

		protected int m_DrawEnd;

		private Coroutine m_DragCoroutine;

		private string m_OriginalText = string.Empty;

		private bool m_WasCanceled;

		private bool m_HasDoneFocusTransition;

		private bool m_isLastKeyBackspace;

		private const string kEmailSpecialCharacters = "!#$%&'*+-/=?^_`{|}~";

		private bool isCaretInsideTag;

		private Event m_ProcessingEvent = new Event();

		protected Mesh mesh
		{
			get
			{
				if (m_Mesh == null)
				{
					m_Mesh = new Mesh();
				}
				return m_Mesh;
			}
		}

		public bool shouldHideMobileInput
		{
			get
			{
				switch (Application.platform)
				{
				case RuntimePlatform.IPhonePlayer:
				case RuntimePlatform.Android:
				case RuntimePlatform.TizenPlayer:
				case RuntimePlatform.tvOS:
					return m_HideMobileInput;
				default:
					return true;
				}
			}
			set
			{
				SetPropertyUtility.SetStruct(ref m_HideMobileInput, value);
			}
		}

		public string text
		{
			get
			{
				return m_Text;
			}
			set
			{
				if (!(text == value))
				{
					m_Text = value;
					if (m_Keyboard != null)
					{
						m_Keyboard.text = m_Text;
					}
					if (m_StringPosition > m_Text.Length)
					{
						m_StringPosition = (m_StringSelectPosition = m_Text.Length);
					}
					SendOnValueChangedAndUpdateLabel();
				}
			}
		}

		public bool isFocused => m_AllowInput;

		public float caretBlinkRate
		{
			get
			{
				return m_CaretBlinkRate;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CaretBlinkRate, value) && m_AllowInput)
				{
					SetCaretActive();
				}
			}
		}

		public int caretWidth
		{
			get
			{
				return m_CaretWidth;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CaretWidth, value))
				{
					MarkGeometryAsDirty();
				}
			}
		}

		public RectTransform textViewport
		{
			get
			{
				return m_TextViewport;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_TextViewport, value);
			}
		}

		public TMP_Text textComponent
		{
			get
			{
				return m_TextComponent;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_TextComponent, value);
			}
		}

		public Graphic placeholder
		{
			get
			{
				return m_Placeholder;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_Placeholder, value);
			}
		}

		public Color caretColor
		{
			get
			{
				return (!customCaretColor) ? textComponent.color : m_CaretColor;
			}
			set
			{
				if (SetPropertyUtility.SetColor(ref m_CaretColor, value))
				{
					MarkGeometryAsDirty();
				}
			}
		}

		public bool customCaretColor
		{
			get
			{
				return m_CustomCaretColor;
			}
			set
			{
				if (m_CustomCaretColor != value)
				{
					m_CustomCaretColor = value;
					MarkGeometryAsDirty();
				}
			}
		}

		public Color selectionColor
		{
			get
			{
				return m_SelectionColor;
			}
			set
			{
				if (SetPropertyUtility.SetColor(ref m_SelectionColor, value))
				{
					MarkGeometryAsDirty();
				}
			}
		}

		public SubmitEvent onEndEdit
		{
			get
			{
				return m_OnEndEdit;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnEndEdit, value);
			}
		}

		public SubmitEvent onSubmit
		{
			get
			{
				return m_OnSubmit;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnSubmit, value);
			}
		}

		public SubmitEvent onFocusLost
		{
			get
			{
				return m_OnFocusLost;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnFocusLost, value);
			}
		}

		public OnChangeEvent onValueChanged
		{
			get
			{
				return m_OnValueChanged;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnValueChanged, value);
			}
		}

		public OnValidateInput onValidateInput
		{
			get
			{
				return m_OnValidateInput;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnValidateInput, value);
			}
		}

		public int characterLimit
		{
			get
			{
				return m_CharacterLimit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CharacterLimit, Math.Max(0, value)))
				{
					UpdateLabel();
				}
			}
		}

		public ContentType contentType
		{
			get
			{
				return m_ContentType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_ContentType, value))
				{
					EnforceContentType();
				}
			}
		}

		public LineType lineType
		{
			get
			{
				return m_LineType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_LineType, value))
				{
					SetTextComponentWrapMode();
				}
				SetToCustomIfContentTypeIsNot(ContentType.Standard, ContentType.Autocorrected);
			}
		}

		public InputType inputType
		{
			get
			{
				return m_InputType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_InputType, value))
				{
					SetToCustom();
				}
			}
		}

		public TouchScreenKeyboardType keyboardType
		{
			get
			{
				return m_KeyboardType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_KeyboardType, value))
				{
					SetToCustom();
				}
			}
		}

		public CharacterValidation characterValidation
		{
			get
			{
				return m_CharacterValidation;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CharacterValidation, value))
				{
					SetToCustom();
				}
			}
		}

		public bool readOnly
		{
			get
			{
				return m_ReadOnly;
			}
			set
			{
				m_ReadOnly = value;
			}
		}

		public bool richText
		{
			get
			{
				return m_RichText;
			}
			set
			{
				m_RichText = value;
				SetTextComponentRichTextMode();
			}
		}

		public bool multiLine => m_LineType == LineType.MultiLineNewline || lineType == LineType.MultiLineSubmit;

		public char asteriskChar
		{
			get
			{
				return m_AsteriskChar;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_AsteriskChar, value))
				{
					UpdateLabel();
				}
			}
		}

		public bool wasCanceled => m_WasCanceled;

		protected int caretPositionInternal
		{
			get
			{
				return m_CaretPosition + Input.compositionString.Length;
			}
			set
			{
				m_CaretPosition = value;
				ClampPos(ref m_CaretPosition);
			}
		}

		protected int stringPositionInternal
		{
			get
			{
				return m_StringPosition + Input.compositionString.Length;
			}
			set
			{
				m_StringPosition = value;
				ClampPos(ref m_StringPosition);
			}
		}

		protected int caretSelectPositionInternal
		{
			get
			{
				return m_CaretSelectPosition + Input.compositionString.Length;
			}
			set
			{
				m_CaretSelectPosition = value;
				ClampPos(ref m_CaretSelectPosition);
			}
		}

		protected int stringSelectPositionInternal
		{
			get
			{
				return m_StringSelectPosition + Input.compositionString.Length;
			}
			set
			{
				m_StringSelectPosition = value;
				ClampPos(ref m_StringSelectPosition);
			}
		}

		private bool hasSelection => stringPositionInternal != stringSelectPositionInternal;

		public int caretPosition
		{
			get
			{
				return m_StringSelectPosition + Input.compositionString.Length;
			}
			set
			{
				selectionAnchorPosition = value;
				selectionFocusPosition = value;
			}
		}

		public int selectionAnchorPosition
		{
			get
			{
				m_StringPosition = GetStringIndexFromCaretPosition(m_CaretPosition);
				return m_StringPosition + Input.compositionString.Length;
			}
			set
			{
				if (Input.compositionString.Length == 0)
				{
					m_CaretPosition = value;
					ClampPos(ref m_CaretPosition);
				}
			}
		}

		public int selectionFocusPosition
		{
			get
			{
				m_StringSelectPosition = GetStringIndexFromCaretPosition(m_CaretSelectPosition);
				return m_StringSelectPosition + Input.compositionString.Length;
			}
			set
			{
				if (Input.compositionString.Length == 0)
				{
					m_CaretSelectPosition = value;
					ClampPos(ref m_CaretSelectPosition);
				}
			}
		}

		private static string clipboard
		{
			get
			{
				return GUIUtility.systemCopyBuffer;
			}
			set
			{
				GUIUtility.systemCopyBuffer = value;
			}
		}

		protected TMP_InputField()
		{
		}

		protected void ClampPos(ref int pos)
		{
			if (pos < 0)
			{
				pos = 0;
			}
			else if (pos > text.Length)
			{
				pos = text.Length;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (m_Text == null)
			{
				m_Text = string.Empty;
			}
			m_DrawStart = 0;
			m_DrawEnd = m_Text.Length;
			if (m_CachedInputRenderer != null)
			{
				m_CachedInputRenderer.SetMaterial(Graphic.defaultGraphicMaterial, Texture2D.whiteTexture);
			}
			if (m_TextComponent != null)
			{
				m_TextComponent.RegisterDirtyVerticesCallback(MarkGeometryAsDirty);
				m_TextComponent.RegisterDirtyVerticesCallback(UpdateLabel);
				UpdateLabel();
			}
		}

		protected override void OnDisable()
		{
			m_BlinkCoroutine = null;
			DeactivateInputField();
			if (m_TextComponent != null)
			{
				m_TextComponent.UnregisterDirtyVerticesCallback(MarkGeometryAsDirty);
				m_TextComponent.UnregisterDirtyVerticesCallback(UpdateLabel);
			}
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (m_CachedInputRenderer != null)
			{
				m_CachedInputRenderer.Clear();
			}
			if (m_Mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(m_Mesh);
			}
			m_Mesh = null;
			base.OnDisable();
		}

		private IEnumerator CaretBlink()
		{
			m_CaretVisible = true;
			yield return null;
			while (isFocused && m_CaretBlinkRate > 0f)
			{
				float blinkPeriod = 1f / m_CaretBlinkRate;
				bool blinkState = (Time.unscaledTime - m_BlinkStartTime) % blinkPeriod < blinkPeriod / 2f;
				if (m_CaretVisible != blinkState)
				{
					m_CaretVisible = blinkState;
					if (!hasSelection)
					{
						MarkGeometryAsDirty();
					}
				}
				yield return null;
			}
			m_BlinkCoroutine = null;
		}

		private void SetCaretVisible()
		{
			if (m_AllowInput)
			{
				m_CaretVisible = true;
				m_BlinkStartTime = Time.unscaledTime;
				SetCaretActive();
			}
		}

		private void SetCaretActive()
		{
			if (!m_AllowInput)
			{
				return;
			}
			if (m_CaretBlinkRate > 0f)
			{
				if (m_BlinkCoroutine == null)
				{
					m_BlinkCoroutine = StartCoroutine(CaretBlink());
				}
			}
			else
			{
				m_CaretVisible = true;
			}
		}

		protected void OnFocus()
		{
			SelectAll();
		}

		protected void SelectAll()
		{
			stringPositionInternal = text.Length;
			stringSelectPositionInternal = 0;
		}

		public void MoveTextEnd(bool shift)
		{
			int length = text.Length;
			if (shift)
			{
				stringSelectPositionInternal = length;
			}
			else
			{
				stringPositionInternal = length;
				stringSelectPositionInternal = stringPositionInternal;
			}
			UpdateLabel();
		}

		public void MoveTextStart(bool shift)
		{
			int num = 0;
			if (shift)
			{
				stringSelectPositionInternal = num;
			}
			else
			{
				stringPositionInternal = num;
				stringSelectPositionInternal = stringPositionInternal;
			}
			UpdateLabel();
		}

		private bool InPlaceEditing()
		{
			return !TouchScreenKeyboard.isSupported;
		}

		protected virtual void LateUpdate()
		{
			if (m_ShouldActivateNextUpdate)
			{
				if (!isFocused)
				{
					ActivateInputFieldInternal();
					m_ShouldActivateNextUpdate = false;
					return;
				}
				m_ShouldActivateNextUpdate = false;
			}
			if (InPlaceEditing() || !isFocused)
			{
				return;
			}
			AssignPositioningIfNeeded();
			if (m_Keyboard == null || !m_Keyboard.active)
			{
				if (m_Keyboard != null)
				{
					if (!m_ReadOnly)
					{
						this.text = m_Keyboard.text;
					}
					if (m_Keyboard.wasCanceled)
					{
						m_WasCanceled = true;
					}
				}
				OnDeselect(null);
				return;
			}
			string text = m_Keyboard.text;
			if (m_Text != text)
			{
				if (m_ReadOnly)
				{
					m_Keyboard.text = m_Text;
				}
				else
				{
					m_Text = string.Empty;
					for (int i = 0; i < text.Length; i++)
					{
						char c = text[i];
						if (c == '\r' || c == '\u0003')
						{
							c = '\n';
						}
						if (onValidateInput != null)
						{
							c = onValidateInput(m_Text, m_Text.Length, c);
						}
						else if (characterValidation != 0)
						{
							c = Validate(m_Text, m_Text.Length, c);
						}
						if (lineType == LineType.MultiLineSubmit && c == '\n')
						{
							m_Keyboard.text = m_Text;
							OnDeselect(null);
							return;
						}
						if (c != 0)
						{
							m_Text += c;
						}
					}
					if (characterLimit > 0 && m_Text.Length > characterLimit)
					{
						m_Text = m_Text.Substring(0, characterLimit);
					}
					int num2 = stringPositionInternal = (stringSelectPositionInternal = m_Text.Length);
					if (m_Text != text)
					{
						m_Keyboard.text = m_Text;
					}
					SendOnValueChangedAndUpdateLabel();
				}
			}
			if (m_Keyboard.done)
			{
				if (m_Keyboard.wasCanceled)
				{
					m_WasCanceled = true;
				}
				OnDeselect(null);
			}
		}

		protected int GetCharacterIndexFromPosition(Vector2 pos)
		{
			return 0;
		}

		private bool MayDrag(PointerEventData eventData)
		{
			return IsActive() && IsInteractable() && eventData.button == PointerEventData.InputButton.Left && m_TextComponent != null && m_Keyboard == null;
		}

		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (MayDrag(eventData))
			{
				m_UpdateDrag = true;
			}
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			if (MayDrag(eventData))
			{
				CaretPosition cursor;
				int cursorIndexFromPosition = TMP_TextUtilities.GetCursorIndexFromPosition(m_TextComponent, eventData.position, eventData.pressEventCamera, out cursor);
				switch (cursor)
				{
				case CaretPosition.Left:
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition);
					break;
				case CaretPosition.Right:
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1;
					break;
				}
				caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
				MarkGeometryAsDirty();
				m_DragPositionOutOfBounds = !RectTransformUtility.RectangleContainsScreenPoint(textViewport, eventData.position, eventData.pressEventCamera);
				if (m_DragPositionOutOfBounds && m_DragCoroutine == null)
				{
					m_DragCoroutine = StartCoroutine(MouseDragOutsideRect(eventData));
				}
				eventData.Use();
			}
		}

		private IEnumerator MouseDragOutsideRect(PointerEventData eventData)
		{
			while (m_UpdateDrag && m_DragPositionOutOfBounds)
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(textViewport, eventData.position, eventData.pressEventCamera, out Vector2 localMousePos);
				Rect rect = textViewport.rect;
				if (multiLine)
				{
					if (localMousePos.y > rect.yMax)
					{
						MoveUp(shift: true, goToFirstChar: true);
					}
					else if (localMousePos.y < rect.yMin)
					{
						MoveDown(shift: true, goToLastChar: true);
					}
				}
				else if (localMousePos.x < rect.xMin)
				{
					MoveLeft(shift: true, ctrl: false);
				}
				else if (localMousePos.x > rect.xMax)
				{
					MoveRight(shift: true, ctrl: false);
				}
				UpdateLabel();
				float delay = (!multiLine) ? 0.05f : 0.1f;
				yield return new WaitForSeconds(delay);
			}
			m_DragCoroutine = null;
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (MayDrag(eventData))
			{
				m_UpdateDrag = false;
			}
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (!MayDrag(eventData))
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(base.gameObject, eventData);
			bool allowInput = m_AllowInput;
			base.OnPointerDown(eventData);
			if (!InPlaceEditing() && (m_Keyboard == null || !m_Keyboard.active))
			{
				OnSelect(eventData);
				return;
			}
			if (allowInput)
			{
				CaretPosition cursor;
				int cursorIndexFromPosition = TMP_TextUtilities.GetCursorIndexFromPosition(m_TextComponent, eventData.position, eventData.pressEventCamera, out cursor);
				int num3;
				switch (cursor)
				{
				case CaretPosition.Left:
					num3 = (stringPositionInternal = (stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition)));
					break;
				case CaretPosition.Right:
					num3 = (stringPositionInternal = (stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1));
					break;
				}
				num3 = (caretPositionInternal = (caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal)));
			}
			UpdateLabel();
			eventData.Use();
		}

		protected EditState KeyPressed(Event evt)
		{
			EventModifiers modifiers = evt.modifiers;
			RuntimePlatform platform = Application.platform;
			bool flag = (platform != 0 && platform != RuntimePlatform.OSXPlayer) ? ((modifiers & EventModifiers.Control) != EventModifiers.None) : ((modifiers & EventModifiers.Command) != EventModifiers.None);
			bool flag2 = (modifiers & EventModifiers.Shift) != EventModifiers.None;
			bool flag3 = (modifiers & EventModifiers.Alt) != EventModifiers.None;
			bool flag4 = flag && !flag3 && !flag2;
			switch (evt.keyCode)
			{
			case KeyCode.Backspace:
				Backspace();
				return EditState.Continue;
			case KeyCode.Delete:
				ForwardSpace();
				return EditState.Continue;
			case KeyCode.Home:
				MoveTextStart(flag2);
				return EditState.Continue;
			case KeyCode.End:
				MoveTextEnd(flag2);
				return EditState.Continue;
			case KeyCode.A:
				if (flag4)
				{
					SelectAll();
					return EditState.Continue;
				}
				break;
			case KeyCode.C:
				if (flag4)
				{
					if (inputType != InputType.Password)
					{
						clipboard = GetSelectedString();
					}
					else
					{
						clipboard = string.Empty;
					}
					return EditState.Continue;
				}
				break;
			case KeyCode.V:
				if (flag4)
				{
					Append(clipboard);
					return EditState.Continue;
				}
				break;
			case KeyCode.X:
				if (flag4)
				{
					if (inputType != InputType.Password)
					{
						clipboard = GetSelectedString();
					}
					else
					{
						clipboard = string.Empty;
					}
					Delete();
					SendOnValueChangedAndUpdateLabel();
					return EditState.Continue;
				}
				break;
			case KeyCode.LeftArrow:
				MoveLeft(flag2, flag);
				return EditState.Continue;
			case KeyCode.RightArrow:
				MoveRight(flag2, flag);
				return EditState.Continue;
			case KeyCode.UpArrow:
				MoveUp(flag2);
				return EditState.Continue;
			case KeyCode.DownArrow:
				MoveDown(flag2);
				return EditState.Continue;
			case KeyCode.Return:
			case KeyCode.KeypadEnter:
				if (lineType != LineType.MultiLineNewline)
				{
					return EditState.Finish;
				}
				break;
			case KeyCode.Escape:
				m_WasCanceled = true;
				return EditState.Finish;
			}
			char c = evt.character;
			if (!multiLine && (c == '\t' || c == '\r' || c == '\n'))
			{
				return EditState.Continue;
			}
			if (c == '\r' || c == '\u0003')
			{
				c = '\n';
			}
			if (IsValidChar(c))
			{
				Append(c);
			}
			if (c == '\0' && Input.compositionString.Length > 0)
			{
				UpdateLabel();
			}
			return EditState.Continue;
		}

		private bool IsValidChar(char c)
		{
			switch (c)
			{
			case '\u007f':
				return false;
			case '\t':
			case '\n':
				return true;
			default:
				return m_TextComponent.font.HasCharacter(c, searchFallbacks: true);
			}
		}

		public void ProcessEvent(Event e)
		{
			KeyPressed(e);
		}

		public virtual void OnUpdateSelected(BaseEventData eventData)
		{
			if (!isFocused)
			{
				return;
			}
			bool flag = false;
			while (Event.PopEvent(m_ProcessingEvent))
			{
				if (m_ProcessingEvent.rawType == EventType.KeyDown)
				{
					flag = true;
					EditState editState = KeyPressed(m_ProcessingEvent);
					if (editState == EditState.Finish)
					{
						DeactivateInputField();
						break;
					}
				}
				EventType type = m_ProcessingEvent.type;
				if (type == EventType.ValidateCommand || type == EventType.ExecuteCommand)
				{
					string commandName = m_ProcessingEvent.commandName;
					if (commandName != null && commandName == "SelectAll")
					{
						SelectAll();
						flag = true;
					}
				}
			}
			if (flag)
			{
				UpdateLabel();
			}
			eventData.Use();
		}

		private string GetSelectedString()
		{
			if (!hasSelection)
			{
				return string.Empty;
			}
			int num = stringPositionInternal;
			int num2 = stringSelectPositionInternal;
			if (num > num2)
			{
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			return text.Substring(num, num2 - num);
		}

		private int FindtNextWordBegin()
		{
			if (stringSelectPositionInternal + 1 >= text.Length)
			{
				return text.Length;
			}
			int num = text.IndexOfAny(kSeparators, stringSelectPositionInternal + 1);
			if (num == -1)
			{
				return text.Length;
			}
			return num + 1;
		}

		private void MoveRight(bool shift, bool ctrl)
		{
			if (hasSelection && !shift)
			{
				int num3 = stringPositionInternal = (stringSelectPositionInternal = Mathf.Max(stringPositionInternal, stringSelectPositionInternal));
				num3 = (caretPositionInternal = (this.caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal)));
				return;
			}
			int caretSelectPositionInternal = this.caretSelectPositionInternal;
			int num5 = (!ctrl) ? (stringSelectPositionInternal + 1) : FindtNextWordBegin();
			if (!shift)
			{
				int num3 = stringSelectPositionInternal = (stringPositionInternal = num5);
				num3 = (this.caretSelectPositionInternal = (caretPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal)));
			}
			else
			{
				stringSelectPositionInternal = num5;
				this.caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
			}
			isCaretInsideTag = (caretSelectPositionInternal == this.caretSelectPositionInternal);
			UnityEngine.Debug.Log("Caret is " + ((!isCaretInsideTag) ? " [Not Inside Tag]" : " [Inside Tag]"));
		}

		private int FindtPrevWordBegin()
		{
			if (stringSelectPositionInternal - 2 < 0)
			{
				return 0;
			}
			int num = text.LastIndexOfAny(kSeparators, stringSelectPositionInternal - 2);
			if (num == -1)
			{
				return 0;
			}
			return num + 1;
		}

		private void MoveLeft(bool shift, bool ctrl)
		{
			if (hasSelection && !shift)
			{
				int num3 = stringPositionInternal = (stringSelectPositionInternal = Mathf.Min(stringPositionInternal, stringSelectPositionInternal));
				num3 = (caretPositionInternal = (this.caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal)));
				return;
			}
			int caretSelectPositionInternal = this.caretSelectPositionInternal;
			int num5 = (!ctrl) ? (stringSelectPositionInternal - 1) : FindtPrevWordBegin();
			if (!shift)
			{
				int num3 = stringSelectPositionInternal = (stringPositionInternal = num5);
				num3 = (this.caretSelectPositionInternal = (caretPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal)));
			}
			else
			{
				stringSelectPositionInternal = num5;
				this.caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
			}
			isCaretInsideTag = (caretSelectPositionInternal == this.caretSelectPositionInternal);
			UnityEngine.Debug.Log("Caret is " + ((!isCaretInsideTag) ? " [Not Inside Tag]" : " [Inside Tag]"));
		}

		private int LineUpCharacterPosition(int originalPos, bool goToFirstChar)
		{
			if (originalPos >= m_TextComponent.textInfo.characterCount)
			{
				originalPos--;
			}
			TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = tMP_CharacterInfo.lineNumber;
			if (lineNumber - 1 < 0)
			{
				return (!goToFirstChar) ? originalPos : 0;
			}
			int num = m_TextComponent.textInfo.lineInfo[lineNumber].firstCharacterIndex - 1;
			for (int i = m_TextComponent.textInfo.lineInfo[lineNumber - 1].firstCharacterIndex; i < num; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[i];
				float num2 = (tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin) / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num2 >= 0f && num2 <= 1f)
				{
					if (num2 < 0.5f)
					{
						return i;
					}
					return i + 1;
				}
			}
			return num;
		}

		private int LineDownCharacterPosition(int originalPos, bool goToLastChar)
		{
			if (originalPos >= m_TextComponent.textInfo.characterCount)
			{
				return text.Length;
			}
			TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = tMP_CharacterInfo.lineNumber;
			if (lineNumber + 1 >= m_TextComponent.textInfo.lineCount)
			{
				return (!goToLastChar) ? originalPos : (m_TextComponent.textInfo.characterCount - 1);
			}
			int lastCharacterIndex = m_TextComponent.textInfo.lineInfo[lineNumber + 1].lastCharacterIndex;
			for (int i = m_TextComponent.textInfo.lineInfo[lineNumber + 1].firstCharacterIndex; i < lastCharacterIndex; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[i];
				float num = (tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin) / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num >= 0f && num <= 1f)
				{
					if (num < 0.5f)
					{
						return i;
					}
					return i + 1;
				}
			}
			return lastCharacterIndex;
		}

		private void MoveDown(bool shift)
		{
			MoveDown(shift, goToLastChar: true);
		}

		private void MoveDown(bool shift, bool goToLastChar)
		{
			if (hasSelection && !shift)
			{
				int num3 = caretPositionInternal = (caretSelectPositionInternal = Mathf.Max(caretPositionInternal, caretSelectPositionInternal));
			}
			int num4 = (!multiLine) ? text.Length : LineDownCharacterPosition(caretSelectPositionInternal, goToLastChar);
			if (shift)
			{
				caretSelectPositionInternal = num4;
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
			}
			else
			{
				int num3 = caretSelectPositionInternal = (caretPositionInternal = num4);
				num3 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal)));
			}
		}

		private void MoveUp(bool shift)
		{
			MoveUp(shift, goToFirstChar: true);
		}

		private void MoveUp(bool shift, bool goToFirstChar)
		{
			if (hasSelection && !shift)
			{
				int num3 = caretPositionInternal = (caretSelectPositionInternal = Mathf.Min(caretPositionInternal, caretSelectPositionInternal));
			}
			int num4 = multiLine ? LineUpCharacterPosition(caretSelectPositionInternal, goToFirstChar) : 0;
			if (shift)
			{
				caretSelectPositionInternal = num4;
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
			}
			else
			{
				int num3 = caretSelectPositionInternal = (caretPositionInternal = num4);
				num3 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal)));
			}
		}

		private void Delete()
		{
			if (!m_ReadOnly && stringPositionInternal != stringSelectPositionInternal)
			{
				if (stringPositionInternal < stringSelectPositionInternal)
				{
					m_Text = text.Substring(0, stringPositionInternal) + text.Substring(stringSelectPositionInternal, text.Length - stringSelectPositionInternal);
					stringSelectPositionInternal = stringPositionInternal;
				}
				else
				{
					m_Text = text.Substring(0, stringSelectPositionInternal) + text.Substring(stringPositionInternal, text.Length - stringPositionInternal);
					stringPositionInternal = stringSelectPositionInternal;
				}
			}
		}

		private void ForwardSpace()
		{
			if (!m_ReadOnly)
			{
				if (hasSelection)
				{
					Delete();
					SendOnValueChangedAndUpdateLabel();
				}
				else if (stringPositionInternal < text.Length)
				{
					m_Text = text.Remove(stringPositionInternal, 1);
					SendOnValueChangedAndUpdateLabel();
				}
			}
		}

		private void Backspace()
		{
			if (!m_ReadOnly)
			{
				if (hasSelection)
				{
					Delete();
					SendOnValueChangedAndUpdateLabel();
				}
				else if (stringPositionInternal > 0)
				{
					m_Text = text.Remove(stringPositionInternal - 1, 1);
					stringSelectPositionInternal = --stringPositionInternal;
					m_isLastKeyBackspace = true;
					SendOnValueChangedAndUpdateLabel();
				}
			}
		}

		private void Insert(char c)
		{
			if (!m_ReadOnly)
			{
				string text = c.ToString();
				Delete();
				if (characterLimit <= 0 || this.text.Length < characterLimit)
				{
					m_Text = this.text.Insert(m_StringPosition, text);
					stringSelectPositionInternal = (stringPositionInternal += text.Length);
					SendOnValueChanged();
				}
			}
		}

		private void SendOnValueChangedAndUpdateLabel()
		{
			SendOnValueChanged();
			UpdateLabel();
		}

		private void SendOnValueChanged()
		{
			if (onValueChanged != null)
			{
				onValueChanged.Invoke(text);
			}
		}

		protected void SendOnSubmit()
		{
			if (onEndEdit != null)
			{
				onEndEdit.Invoke(m_Text);
			}
		}

		protected void SendOnFocusLost()
		{
			if (onFocusLost != null)
			{
				onFocusLost.Invoke(m_Text);
			}
		}

		protected virtual void Append(string input)
		{
			if (m_ReadOnly || !InPlaceEditing())
			{
				return;
			}
			int i = 0;
			for (int length = input.Length; i < length; i++)
			{
				char c = input[i];
				if (c >= ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\n')
				{
					Append(c);
				}
			}
		}

		protected virtual void Append(char input)
		{
			if (!m_ReadOnly && InPlaceEditing())
			{
				if (onValidateInput != null)
				{
					input = onValidateInput(text, stringPositionInternal, input);
				}
				else if (characterValidation != 0)
				{
					input = Validate(text, stringPositionInternal, input);
				}
				if (input != 0)
				{
					Insert(input);
				}
			}
		}

		protected void UpdateLabel()
		{
			if (m_TextComponent != null && m_TextComponent.font != null)
			{
				string text = (Input.compositionString.Length <= 0) ? this.text : (this.text.Substring(0, m_StringPosition) + Input.compositionString + this.text.Substring(m_StringPosition));
				string str = (inputType != InputType.Password) ? text : new string(asteriskChar, text.Length);
				bool flag = string.IsNullOrEmpty(text);
				if (m_Placeholder != null)
				{
					m_Placeholder.enabled = flag;
				}
				if (!m_AllowInput)
				{
					m_DrawStart = 0;
					m_DrawEnd = m_Text.Length;
				}
				if (!flag)
				{
					SetCaretVisible();
				}
				m_TextComponent.text = str + "\u200b";
				MarkGeometryAsDirty();
			}
		}

		private int GetCaretPositionFromStringIndex(int stringIndex)
		{
			int characterCount = m_TextComponent.textInfo.characterCount;
			for (int i = 0; i < characterCount; i++)
			{
				if (m_TextComponent.textInfo.characterInfo[i].index >= stringIndex)
				{
					return i;
				}
			}
			return characterCount;
		}

		private int GetStringIndexFromCaretPosition(int caretPosition)
		{
			return m_TextComponent.textInfo.characterInfo[caretPosition].index;
		}

		public void ForceLabelUpdate()
		{
			UpdateLabel();
		}

		private void MarkGeometryAsDirty()
		{
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
		}

		public virtual void Rebuild(CanvasUpdate update)
		{
			if (update == CanvasUpdate.LatePreRender)
			{
				UpdateGeometry();
			}
		}

		public virtual void LayoutComplete()
		{
		}

		public virtual void GraphicUpdateComplete()
		{
		}

		private void UpdateGeometry()
		{
			if (shouldHideMobileInput)
			{
				if (m_CachedInputRenderer == null && m_TextComponent != null)
				{
					GameObject gameObject = new GameObject(base.transform.name + " Input Caret");
					gameObject.hideFlags = HideFlags.DontSave;
					gameObject.transform.SetParent(m_TextComponent.transform.parent);
					gameObject.transform.SetAsFirstSibling();
					gameObject.layer = base.gameObject.layer;
					caretRectTrans = gameObject.AddComponent<RectTransform>();
					m_CachedInputRenderer = gameObject.AddComponent<CanvasRenderer>();
					m_CachedInputRenderer.SetMaterial(Graphic.defaultGraphicMaterial, Texture2D.whiteTexture);
					gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
					AssignPositioningIfNeeded();
				}
				if (!(m_CachedInputRenderer == null))
				{
					OnFillVBO(mesh);
					m_CachedInputRenderer.SetMesh(mesh);
				}
			}
		}

		private void AssignPositioningIfNeeded()
		{
			if (m_TextComponent != null && caretRectTrans != null && (caretRectTrans.localPosition != m_TextComponent.rectTransform.localPosition || caretRectTrans.localRotation != m_TextComponent.rectTransform.localRotation || caretRectTrans.localScale != m_TextComponent.rectTransform.localScale || caretRectTrans.anchorMin != m_TextComponent.rectTransform.anchorMin || caretRectTrans.anchorMax != m_TextComponent.rectTransform.anchorMax || caretRectTrans.anchoredPosition != m_TextComponent.rectTransform.anchoredPosition || caretRectTrans.sizeDelta != m_TextComponent.rectTransform.sizeDelta || caretRectTrans.pivot != m_TextComponent.rectTransform.pivot))
			{
				caretRectTrans.localPosition = m_TextComponent.rectTransform.localPosition;
				caretRectTrans.localRotation = m_TextComponent.rectTransform.localRotation;
				caretRectTrans.localScale = m_TextComponent.rectTransform.localScale;
				caretRectTrans.anchorMin = m_TextComponent.rectTransform.anchorMin;
				caretRectTrans.anchorMax = m_TextComponent.rectTransform.anchorMax;
				caretRectTrans.anchoredPosition = m_TextComponent.rectTransform.anchoredPosition;
				caretRectTrans.sizeDelta = m_TextComponent.rectTransform.sizeDelta;
				caretRectTrans.pivot = m_TextComponent.rectTransform.pivot;
			}
		}

		private void OnFillVBO(Mesh vbo)
		{
			using (VertexHelper vertexHelper = new VertexHelper())
			{
				if (!isFocused)
				{
					vertexHelper.FillMesh(vbo);
				}
				else
				{
					if (!hasSelection)
					{
						GenerateCaret(vertexHelper, Vector2.zero);
					}
					else
					{
						GenerateHightlight(vertexHelper, Vector2.zero);
					}
					vertexHelper.FillMesh(vbo);
				}
			}
		}

		private void GenerateCaret(VertexHelper vbo, Vector2 roundingOffset)
		{
			if (m_CaretVisible)
			{
				if (m_CursorVerts == null)
				{
					CreateCursorVerts();
				}
				float num = m_CaretWidth;
				int characterCount = m_TextComponent.textInfo.characterCount;
				Vector2 vector = Vector2.zero;
				float num2 = 0f;
				caretPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal);
				TMP_CharacterInfo tMP_CharacterInfo;
				if (caretPositionInternal == 0)
				{
					tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[0];
					vector = new Vector2(tMP_CharacterInfo.origin, tMP_CharacterInfo.descender);
					num2 = tMP_CharacterInfo.ascender - tMP_CharacterInfo.descender;
				}
				else if (caretPositionInternal < characterCount)
				{
					tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[caretPositionInternal];
					vector = new Vector2(tMP_CharacterInfo.origin, tMP_CharacterInfo.descender);
					num2 = tMP_CharacterInfo.ascender - tMP_CharacterInfo.descender;
				}
				else
				{
					tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[characterCount - 1];
					vector = new Vector2(tMP_CharacterInfo.xAdvance, tMP_CharacterInfo.descender);
					num2 = tMP_CharacterInfo.ascender - tMP_CharacterInfo.descender;
				}
				AdjustRectTransformRelativeToViewport(vector, num2, tMP_CharacterInfo.isVisible);
				float num3 = vector.y + num2;
				float y = num3 - Mathf.Min(num2, m_TextComponent.rectTransform.rect.height);
				m_CursorVerts[0].position = new Vector3(vector.x, y, 0f);
				m_CursorVerts[1].position = new Vector3(vector.x, num3, 0f);
				m_CursorVerts[2].position = new Vector3(vector.x + num, num3, 0f);
				m_CursorVerts[3].position = new Vector3(vector.x + num, y, 0f);
				m_CursorVerts[0].color = caretColor;
				m_CursorVerts[1].color = caretColor;
				m_CursorVerts[2].color = caretColor;
				m_CursorVerts[3].color = caretColor;
				vbo.AddUIVertexQuad(m_CursorVerts);
				int height = Screen.height;
				vector.y = (float)height - vector.y;
				Input.compositionCursorPos = vector;
			}
		}

		private void CreateCursorVerts()
		{
			m_CursorVerts = new UIVertex[4];
			for (int i = 0; i < m_CursorVerts.Length; i++)
			{
				m_CursorVerts[i] = UIVertex.simpleVert;
				m_CursorVerts[i].uv0 = Vector2.zero;
			}
		}

		private void GenerateHightlight(VertexHelper vbo, Vector2 roundingOffset)
		{
			TMP_TextInfo textInfo = m_TextComponent.textInfo;
			caretPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal);
			caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
			UnityEngine.Debug.Log("StringPosition:" + stringPositionInternal + "  StringSelectPosition:" + stringSelectPositionInternal);
			float num = 0f;
			Vector2 startPosition;
			if (caretSelectPositionInternal < textInfo.characterCount)
			{
				startPosition = new Vector2(textInfo.characterInfo[caretSelectPositionInternal].origin, textInfo.characterInfo[caretSelectPositionInternal].descender);
				num = textInfo.characterInfo[caretSelectPositionInternal].ascender - textInfo.characterInfo[caretSelectPositionInternal].descender;
			}
			else
			{
				startPosition = new Vector2(textInfo.characterInfo[caretSelectPositionInternal - 1].xAdvance, textInfo.characterInfo[caretSelectPositionInternal - 1].descender);
				num = textInfo.characterInfo[caretSelectPositionInternal - 1].ascender - textInfo.characterInfo[caretSelectPositionInternal - 1].descender;
			}
			AdjustRectTransformRelativeToViewport(startPosition, num, isCharVisible: true);
			int num2 = Mathf.Max(0, caretPositionInternal);
			int num3 = Mathf.Max(0, caretSelectPositionInternal);
			if (num2 > num3)
			{
				int num4 = num2;
				num2 = num3;
				num3 = num4;
			}
			num3--;
			int num5 = textInfo.characterInfo[num2].lineNumber;
			int lastCharacterIndex = textInfo.lineInfo[num5].lastCharacterIndex;
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.uv0 = Vector2.zero;
			simpleVert.color = selectionColor;
			for (int i = num2; i <= num3 && i < textInfo.characterCount; i++)
			{
				if (i != lastCharacterIndex && i != num3)
				{
					continue;
				}
				TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[num2];
				TMP_CharacterInfo tMP_CharacterInfo2 = textInfo.characterInfo[i];
				Vector2 vector = new Vector2(tMP_CharacterInfo.origin, tMP_CharacterInfo.ascender);
				Vector2 vector2 = new Vector2(tMP_CharacterInfo2.xAdvance, tMP_CharacterInfo2.descender);
				Vector2 min = m_TextViewport.rect.min;
				Vector2 max = m_TextViewport.rect.max;
				Vector2 anchoredPosition = m_TextComponent.rectTransform.anchoredPosition;
				float num6 = anchoredPosition.x + vector.x - min.x;
				if (num6 < 0f)
				{
					vector.x -= num6;
				}
				Vector2 anchoredPosition2 = m_TextComponent.rectTransform.anchoredPosition;
				float num7 = anchoredPosition2.y + vector2.y - min.y;
				if (num7 < 0f)
				{
					vector2.y -= num7;
				}
				float x = max.x;
				Vector2 anchoredPosition3 = m_TextComponent.rectTransform.anchoredPosition;
				float num8 = x - (anchoredPosition3.x + vector2.x);
				if (num8 < 0f)
				{
					vector2.x += num8;
				}
				float y = max.y;
				Vector2 anchoredPosition4 = m_TextComponent.rectTransform.anchoredPosition;
				float num9 = y - (anchoredPosition4.y + vector.y);
				if (num9 < 0f)
				{
					vector.y += num9;
				}
				Vector2 anchoredPosition5 = m_TextComponent.rectTransform.anchoredPosition;
				if (!(anchoredPosition5.y + vector.y < min.y))
				{
					Vector2 anchoredPosition6 = m_TextComponent.rectTransform.anchoredPosition;
					if (!(anchoredPosition6.y + vector2.y > max.y))
					{
						int currentVertCount = vbo.currentVertCount;
						simpleVert.position = new Vector3(vector.x, vector2.y, 0f);
						vbo.AddVert(simpleVert);
						simpleVert.position = new Vector3(vector2.x, vector2.y, 0f);
						vbo.AddVert(simpleVert);
						simpleVert.position = new Vector3(vector2.x, vector.y, 0f);
						vbo.AddVert(simpleVert);
						simpleVert.position = new Vector3(vector.x, vector.y, 0f);
						vbo.AddVert(simpleVert);
						vbo.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
						vbo.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
					}
				}
				num2 = i + 1;
				num5++;
				if (num5 < textInfo.lineCount)
				{
					lastCharacterIndex = textInfo.lineInfo[num5].lastCharacterIndex;
				}
			}
		}

		private void AdjustRectTransformRelativeToViewport(Vector2 startPosition, float height, bool isCharVisible)
		{
			float xMin = m_TextViewport.rect.xMin;
			float xMax = m_TextViewport.rect.xMax;
			float num = xMax;
			Vector2 anchoredPosition = m_TextComponent.rectTransform.anchoredPosition;
			float num2 = anchoredPosition.x + startPosition.x;
			Vector4 margin = m_TextComponent.margin;
			float num3 = num - (num2 + margin.z);
			if (num3 < 0f && (!multiLine || (multiLine && isCharVisible)))
			{
				m_TextComponent.rectTransform.anchoredPosition += new Vector2(num3, 0f);
				AssignPositioningIfNeeded();
			}
			Vector2 anchoredPosition2 = m_TextComponent.rectTransform.anchoredPosition;
			float num4 = anchoredPosition2.x + startPosition.x;
			Vector4 margin2 = m_TextComponent.margin;
			float num5 = num4 - margin2.x - xMin;
			if (num5 < 0f)
			{
				m_TextComponent.rectTransform.anchoredPosition += new Vector2(0f - num5, 0f);
				AssignPositioningIfNeeded();
			}
			if (m_LineType != 0)
			{
				float yMax = m_TextViewport.rect.yMax;
				Vector2 anchoredPosition3 = m_TextComponent.rectTransform.anchoredPosition;
				float num6 = yMax - (anchoredPosition3.y + startPosition.y + height);
				if (num6 < -0.0001f)
				{
					m_TextComponent.rectTransform.anchoredPosition += new Vector2(0f, num6);
					AssignPositioningIfNeeded();
				}
				Vector2 anchoredPosition4 = m_TextComponent.rectTransform.anchoredPosition;
				float num7 = anchoredPosition4.y + startPosition.y - m_TextViewport.rect.yMin;
				if (num7 < 0f)
				{
					m_TextComponent.rectTransform.anchoredPosition -= new Vector2(0f, num7);
					AssignPositioningIfNeeded();
				}
			}
			if (!m_isLastKeyBackspace)
			{
				return;
			}
			Vector2 anchoredPosition5 = m_TextComponent.rectTransform.anchoredPosition;
			float num8 = anchoredPosition5.x + m_TextComponent.textInfo.characterInfo[0].origin;
			Vector4 margin3 = m_TextComponent.margin;
			float num9 = num8 - margin3.x;
			Vector2 anchoredPosition6 = m_TextComponent.rectTransform.anchoredPosition;
			float num10 = anchoredPosition6.x + m_TextComponent.textInfo.characterInfo[m_TextComponent.textInfo.characterCount - 1].origin;
			Vector4 margin4 = m_TextComponent.margin;
			float num11 = num10 + margin4.z;
			Vector2 anchoredPosition7 = m_TextComponent.rectTransform.anchoredPosition;
			if (anchoredPosition7.x + startPosition.x <= xMin + 0.0001f)
			{
				if (num9 < xMin)
				{
					float x = Mathf.Min((xMax - xMin) / 2f, xMin - num9);
					m_TextComponent.rectTransform.anchoredPosition += new Vector2(x, 0f);
					AssignPositioningIfNeeded();
				}
			}
			else if (num11 < xMax && num9 < xMin)
			{
				float x2 = Mathf.Min(xMax - num11, xMin - num9);
				m_TextComponent.rectTransform.anchoredPosition += new Vector2(x2, 0f);
				AssignPositioningIfNeeded();
			}
			m_isLastKeyBackspace = false;
		}

		protected char Validate(string text, int pos, char ch)
		{
			if (characterValidation == CharacterValidation.None || !base.enabled)
			{
				return ch;
			}
			if (characterValidation == CharacterValidation.Integer || characterValidation == CharacterValidation.Decimal)
			{
				bool flag = pos == 0 && text.Length > 0 && text[0] == '-';
				bool flag2 = stringPositionInternal == 0 || stringSelectPositionInternal == 0;
				if (!flag)
				{
					if (ch >= '0' && ch <= '9')
					{
						return ch;
					}
					if (ch == '-' && (pos == 0 || flag2))
					{
						return ch;
					}
					if (ch == '.' && characterValidation == CharacterValidation.Decimal && !text.Contains("."))
					{
						return ch;
					}
				}
			}
			else if (characterValidation == CharacterValidation.Alphanumeric)
			{
				if (ch >= 'A' && ch <= 'Z')
				{
					return ch;
				}
				if (ch >= 'a' && ch <= 'z')
				{
					return ch;
				}
				if (ch >= '0' && ch <= '9')
				{
					return ch;
				}
			}
			else if (characterValidation == CharacterValidation.Name)
			{
				char c = (text.Length <= 0) ? ' ' : text[Mathf.Clamp(pos, 0, text.Length - 1)];
				char c2 = (text.Length <= 0) ? '\n' : text[Mathf.Clamp(pos + 1, 0, text.Length - 1)];
				if (char.IsLetter(ch))
				{
					if (char.IsLower(ch) && c == ' ')
					{
						return char.ToUpper(ch);
					}
					if (char.IsUpper(ch) && c != ' ' && c != '\'')
					{
						return char.ToLower(ch);
					}
					return ch;
				}
				switch (ch)
				{
				case '\'':
					if (c != ' ' && c != '\'' && c2 != '\'' && !text.Contains("'"))
					{
						return ch;
					}
					break;
				case ' ':
					if (c != ' ' && c != '\'' && c2 != ' ' && c2 != '\'')
					{
						return ch;
					}
					break;
				}
			}
			else if (characterValidation == CharacterValidation.EmailAddress)
			{
				if (ch >= 'A' && ch <= 'Z')
				{
					return ch;
				}
				if (ch >= 'a' && ch <= 'z')
				{
					return ch;
				}
				if (ch >= '0' && ch <= '9')
				{
					return ch;
				}
				if (ch == '@' && text.IndexOf('@') == -1)
				{
					return ch;
				}
				if ("!#$%&'*+-/=?^_`{|}~".IndexOf(ch) != -1)
				{
					return ch;
				}
				if (ch == '.')
				{
					char c3 = (text.Length <= 0) ? ' ' : text[Mathf.Clamp(pos, 0, text.Length - 1)];
					char c4 = (text.Length <= 0) ? '\n' : text[Mathf.Clamp(pos + 1, 0, text.Length - 1)];
					if (c3 != '.' && c4 != '.')
					{
						return ch;
					}
				}
			}
			return '\0';
		}

		public void ActivateInputField()
		{
			if (!(m_TextComponent == null) && !(m_TextComponent.font == null) && IsActive() && IsInteractable())
			{
				if (isFocused && m_Keyboard != null && !m_Keyboard.active)
				{
					m_Keyboard.active = true;
					m_Keyboard.text = m_Text;
				}
				m_HasLostFocus = false;
				m_ShouldActivateNextUpdate = true;
			}
		}

		private void ActivateInputFieldInternal()
		{
			if (EventSystem.current == null)
			{
				return;
			}
			if (EventSystem.current.currentSelectedGameObject != base.gameObject)
			{
				EventSystem.current.SetSelectedGameObject(base.gameObject);
			}
			if (TouchScreenKeyboard.isSupported)
			{
				if (Input.touchSupported)
				{
					TouchScreenKeyboard.hideInput = shouldHideMobileInput;
				}
				m_Keyboard = ((inputType != InputType.Password) ? TouchScreenKeyboard.Open(m_Text, keyboardType, inputType == InputType.AutoCorrect, multiLine) : TouchScreenKeyboard.Open(m_Text, keyboardType, autocorrection: false, multiLine, secure: true));
				MoveTextEnd(shift: false);
			}
			else
			{
				Input.imeCompositionMode = IMECompositionMode.On;
				OnFocus();
			}
			m_AllowInput = true;
			m_OriginalText = text;
			m_WasCanceled = false;
			SetCaretVisible();
			UpdateLabel();
		}

		public override void OnSelect(BaseEventData eventData)
		{
			UnityEngine.Debug.Log("OnSelect()");
			base.OnSelect(eventData);
			ActivateInputField();
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				ActivateInputField();
			}
		}

		public void DeactivateInputField()
		{
			if (!m_AllowInput)
			{
				return;
			}
			m_HasDoneFocusTransition = false;
			m_AllowInput = false;
			if (m_Placeholder != null)
			{
				m_Placeholder.enabled = string.IsNullOrEmpty(m_Text);
			}
			if (m_TextComponent != null && IsInteractable())
			{
				if (m_WasCanceled)
				{
					text = m_OriginalText;
				}
				if (m_Keyboard != null)
				{
					m_Keyboard.active = false;
					m_Keyboard = null;
				}
				m_StringPosition = (m_StringSelectPosition = 0);
				m_TextComponent.rectTransform.localPosition = Vector3.zero;
				if (caretRectTrans != null)
				{
					caretRectTrans.localPosition = Vector3.zero;
				}
				SendOnSubmit();
				if (m_HasLostFocus)
				{
					SendOnFocusLost();
				}
				Input.imeCompositionMode = IMECompositionMode.Auto;
			}
			MarkGeometryAsDirty();
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			UnityEngine.Debug.Log("OnDeselect()");
			m_HasLostFocus = true;
			DeactivateInputField();
			base.OnDeselect(eventData);
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			UnityEngine.Debug.Log("OnSubmit()");
			if (IsActive() && IsInteractable() && !isFocused)
			{
				m_ShouldActivateNextUpdate = true;
			}
		}

		private void EnforceContentType()
		{
			switch (contentType)
			{
			case ContentType.Standard:
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.Default;
				m_CharacterValidation = CharacterValidation.None;
				break;
			case ContentType.Autocorrected:
				m_InputType = InputType.AutoCorrect;
				m_KeyboardType = TouchScreenKeyboardType.Default;
				m_CharacterValidation = CharacterValidation.None;
				break;
			case ContentType.IntegerNumber:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.NumberPad;
				m_CharacterValidation = CharacterValidation.Integer;
				break;
			case ContentType.DecimalNumber:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
				m_CharacterValidation = CharacterValidation.Decimal;
				break;
			case ContentType.Alphanumeric:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.ASCIICapable;
				m_CharacterValidation = CharacterValidation.Alphanumeric;
				break;
			case ContentType.Name:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.Default;
				m_CharacterValidation = CharacterValidation.Name;
				break;
			case ContentType.EmailAddress:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.EmailAddress;
				m_CharacterValidation = CharacterValidation.EmailAddress;
				break;
			case ContentType.Password:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Password;
				m_KeyboardType = TouchScreenKeyboardType.Default;
				m_CharacterValidation = CharacterValidation.None;
				break;
			case ContentType.Pin:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Password;
				m_KeyboardType = TouchScreenKeyboardType.NumberPad;
				m_CharacterValidation = CharacterValidation.Integer;
				break;
			}
		}

		private void SetTextComponentWrapMode()
		{
			if (!(m_TextComponent == null))
			{
				if (m_LineType == LineType.SingleLine)
				{
					m_TextComponent.enableWordWrapping = false;
				}
				else
				{
					m_TextComponent.enableWordWrapping = true;
				}
			}
		}

		private void SetTextComponentRichTextMode()
		{
			if (!(m_TextComponent == null))
			{
				m_TextComponent.richText = m_RichText;
			}
		}

		private void SetToCustomIfContentTypeIsNot(params ContentType[] allowedContentTypes)
		{
			if (contentType == ContentType.Custom)
			{
				return;
			}
			for (int i = 0; i < allowedContentTypes.Length; i++)
			{
				if (contentType == allowedContentTypes[i])
				{
					return;
				}
			}
			contentType = ContentType.Custom;
		}

		private void SetToCustom()
		{
			if (contentType != ContentType.Custom)
			{
				contentType = ContentType.Custom;
			}
		}

		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			if (m_HasDoneFocusTransition)
			{
				state = SelectionState.Highlighted;
			}
			else if (state == SelectionState.Pressed)
			{
				m_HasDoneFocusTransition = true;
			}
			base.DoStateTransition(state, instant);
		}

		Transform ICanvasElement.transform
		{ get {
			return base.transform;
		} }

		bool ICanvasElement.IsDestroyed()
		{
			return IsDestroyed();
		}
	}
}
