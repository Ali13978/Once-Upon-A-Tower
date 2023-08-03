using Rewired.Integration.UnityUI;
using Rewired.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	[AddComponentMenu("")]
	public class CalibrationWindow : Window
	{
		public enum ButtonIdentifier
		{
			Done,
			Cancel,
			Default,
			Calibrate
		}

		private const float minSensitivityOtherAxes = 0.1f;

		private const float maxDeadzone = 0.8f;

		[SerializeField]
		private RectTransform rightContentContainer;

		[SerializeField]
		private RectTransform valueDisplayGroup;

		[SerializeField]
		private RectTransform calibratedValueMarker;

		[SerializeField]
		private RectTransform rawValueMarker;

		[SerializeField]
		private RectTransform calibratedZeroMarker;

		[SerializeField]
		private RectTransform deadzoneArea;

		[SerializeField]
		private Slider deadzoneSlider;

		[SerializeField]
		private Slider zeroSlider;

		[SerializeField]
		private Slider sensitivitySlider;

		[SerializeField]
		private Toggle invertToggle;

		[SerializeField]
		private RectTransform axisScrollAreaContent;

		[SerializeField]
		private Button doneButton;

		[SerializeField]
		private Button calibrateButton;

		[SerializeField]
		private Text doneButtonLabel;

		[SerializeField]
		private Text cancelButtonLabel;

		[SerializeField]
		private Text defaultButtonLabel;

		[SerializeField]
		private Text deadzoneSliderLabel;

		[SerializeField]
		private Text zeroSliderLabel;

		[SerializeField]
		private Text sensitivitySliderLabel;

		[SerializeField]
		private Text invertToggleLabel;

		[SerializeField]
		private Text calibrateButtonLabel;

		[SerializeField]
		private GameObject axisButtonPrefab;

		private Joystick joystick;

		private string origCalibrationData;

		private int selectedAxis = -1;

		private AxisCalibrationData origSelectedAxisCalibrationData;

		private float displayAreaWidth;

		private List<Button> axisButtons;

		private Dictionary<int, Action<int>> buttonCallbacks;

		private int playerId;

		private RewiredStandaloneInputModule rewiredStandaloneInputModule;

		private int menuHorizActionId = -1;

		private int menuVertActionId = -1;

		private float minSensitivity;

		private bool axisSelected
		{
			get
			{
				if (joystick == null)
				{
					return false;
				}
				if (selectedAxis < 0 || selectedAxis >= joystick.calibrationMap.axisCount)
				{
					return false;
				}
				return true;
			}
		}

		private AxisCalibration axisCalibration
		{
			get
			{
				if (!axisSelected)
				{
					return null;
				}
				return joystick.calibrationMap.GetAxis(selectedAxis);
			}
		}

		public override void Initialize(int id, Func<int, bool> isFocusedCallback)
		{
			if (rightContentContainer == null || valueDisplayGroup == null || calibratedValueMarker == null || rawValueMarker == null || calibratedZeroMarker == null || deadzoneArea == null || deadzoneSlider == null || sensitivitySlider == null || zeroSlider == null || invertToggle == null || axisScrollAreaContent == null || doneButton == null || calibrateButton == null || axisButtonPrefab == null || doneButtonLabel == null || cancelButtonLabel == null || defaultButtonLabel == null || deadzoneSliderLabel == null || zeroSliderLabel == null || sensitivitySliderLabel == null || invertToggleLabel == null || calibrateButtonLabel == null)
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: All inspector values must be assigned!");
				return;
			}
			axisButtons = new List<Button>();
			buttonCallbacks = new Dictionary<int, Action<int>>();
			doneButtonLabel.text = ControlMapper.GetLanguage().done;
			cancelButtonLabel.text = ControlMapper.GetLanguage().cancel;
			defaultButtonLabel.text = ControlMapper.GetLanguage().default_;
			deadzoneSliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_deadZoneSliderLabel;
			zeroSliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_zeroSliderLabel;
			sensitivitySliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_sensitivitySliderLabel;
			invertToggleLabel.text = ControlMapper.GetLanguage().calibrateWindow_invertToggleLabel;
			calibrateButtonLabel.text = ControlMapper.GetLanguage().calibrateWindow_calibrateButtonLabel;
			base.Initialize(id, isFocusedCallback);
		}

		public void SetJoystick(int playerId, Joystick joystick)
		{
			if (!base.initialized)
			{
				return;
			}
			this.playerId = playerId;
			this.joystick = joystick;
			if (joystick == null)
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: Joystick cannot be null!");
				return;
			}
			float num = 0f;
			for (int i = 0; i < joystick.axisCount; i++)
			{
				int index = i;
				GameObject gameObject = UITools.InstantiateGUIObject<Button>(axisButtonPrefab, axisScrollAreaContent, "Axis" + i);
				Button button = gameObject.GetComponent<Button>();
				button.onClick.AddListener(delegate
				{
					OnAxisSelected(index, button);
				});
				Text componentInSelfOrChildren = UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
				if (componentInSelfOrChildren != null)
				{
					componentInSelfOrChildren.text = joystick.AxisElementIdentifiers[i].name;
				}
				if (num == 0f)
				{
					num = UnityTools.GetComponentInSelfOrChildren<LayoutElement>(gameObject).minHeight;
				}
				axisButtons.Add(button);
			}
			float spacing = axisScrollAreaContent.GetComponent<VerticalLayoutGroup>().spacing;
			RectTransform rectTransform = axisScrollAreaContent;
			Vector2 sizeDelta = axisScrollAreaContent.sizeDelta;
			float x = sizeDelta.x;
			float a = (float)joystick.axisCount * (num + spacing) - spacing;
			Vector2 sizeDelta2 = axisScrollAreaContent.sizeDelta;
			rectTransform.sizeDelta = new Vector2(x, Mathf.Max(a, sizeDelta2.y));
			origCalibrationData = joystick.calibrationMap.ToXmlString();
			Vector2 sizeDelta3 = rightContentContainer.sizeDelta;
			displayAreaWidth = sizeDelta3.x;
			rewiredStandaloneInputModule = base.gameObject.transform.root.GetComponentInChildren<RewiredStandaloneInputModule>();
			if (rewiredStandaloneInputModule != null)
			{
				menuHorizActionId = ReInput.mapping.GetActionId(rewiredStandaloneInputModule.horizontalAxis);
				menuVertActionId = ReInput.mapping.GetActionId(rewiredStandaloneInputModule.verticalAxis);
			}
			if (joystick.axisCount > 0)
			{
				SelectAxis(0);
			}
			base.defaultUIElement = doneButton.gameObject;
			RefreshControls();
			Redraw();
		}

		public void SetButtonCallback(ButtonIdentifier buttonIdentifier, Action<int> callback)
		{
			if (base.initialized && callback != null)
			{
				if (buttonCallbacks.ContainsKey((int)buttonIdentifier))
				{
					buttonCallbacks[(int)buttonIdentifier] = callback;
				}
				else
				{
					buttonCallbacks.Add((int)buttonIdentifier, callback);
				}
			}
		}

		public override void Cancel()
		{
			if (!base.initialized)
			{
				return;
			}
			if (joystick != null)
			{
				joystick.ImportCalibrationMapFromXmlString(origCalibrationData);
			}
			if (!buttonCallbacks.TryGetValue(1, out Action<int> value))
			{
				if (cancelCallback != null)
				{
					cancelCallback();
				}
			}
			else
			{
				value(base.id);
			}
		}

		protected override void Update()
		{
			if (base.initialized)
			{
				base.Update();
				UpdateDisplay();
			}
		}

		public void OnDone()
		{
			if (base.initialized && buttonCallbacks.TryGetValue(0, out Action<int> value))
			{
				value(base.id);
			}
		}

		public void OnCancel()
		{
			Cancel();
		}

		public void OnRestoreDefault()
		{
			if (base.initialized && joystick != null)
			{
				joystick.calibrationMap.Reset();
				RefreshControls();
				Redraw();
			}
		}

		public void OnCalibrate()
		{
			if (base.initialized && buttonCallbacks.TryGetValue(3, out Action<int> value))
			{
				value(selectedAxis);
			}
		}

		public void OnInvert(bool state)
		{
			if (base.initialized && axisSelected)
			{
				axisCalibration.invert = state;
			}
		}

		public void OnZeroValueChange(float value)
		{
			if (base.initialized && axisSelected)
			{
				axisCalibration.calibratedZero = value;
				RedrawCalibratedZero();
			}
		}

		public void OnZeroCancel()
		{
			if (base.initialized && axisSelected)
			{
				axisCalibration.calibratedZero = origSelectedAxisCalibrationData.zero;
				RedrawCalibratedZero();
				RefreshControls();
			}
		}

		public void OnDeadzoneValueChange(float value)
		{
			if (base.initialized && axisSelected)
			{
				axisCalibration.deadZone = Mathf.Clamp(value, 0f, 0.8f);
				if (value > 0.8f)
				{
					deadzoneSlider.value = 0.8f;
				}
				RedrawDeadzone();
			}
		}

		public void OnDeadzoneCancel()
		{
			if (base.initialized && axisSelected)
			{
				axisCalibration.deadZone = origSelectedAxisCalibrationData.deadZone;
				RedrawDeadzone();
				RefreshControls();
			}
		}

		public void OnSensitivityValueChange(float value)
		{
			if (base.initialized && axisSelected)
			{
				axisCalibration.sensitivity = Mathf.Clamp(value, minSensitivity, float.PositiveInfinity);
				if (value < minSensitivity)
				{
					sensitivitySlider.value = minSensitivity;
				}
			}
		}

		public void OnSensitivityCancel(float value)
		{
			if (base.initialized && axisSelected)
			{
				axisCalibration.sensitivity = origSelectedAxisCalibrationData.sensitivity;
				RefreshControls();
			}
		}

		public void OnAxisScrollRectScroll(Vector2 pos)
		{
			if (base.initialized)
			{
			}
		}

		private void OnAxisSelected(int axisIndex, Button button)
		{
			if (base.initialized && joystick != null)
			{
				SelectAxis(axisIndex);
				RefreshControls();
				Redraw();
			}
		}

		private void UpdateDisplay()
		{
			RedrawValueMarkers();
		}

		private void Redraw()
		{
			RedrawCalibratedZero();
			RedrawValueMarkers();
		}

		private void RefreshControls()
		{
			if (!axisSelected)
			{
				deadzoneSlider.value = 0f;
				zeroSlider.value = 0f;
				sensitivitySlider.value = 0f;
				invertToggle.isOn = false;
			}
			else
			{
				deadzoneSlider.value = axisCalibration.deadZone;
				zeroSlider.value = axisCalibration.calibratedZero;
				sensitivitySlider.value = axisCalibration.sensitivity;
				invertToggle.isOn = axisCalibration.invert;
			}
		}

		private void RedrawDeadzone()
		{
			if (axisSelected)
			{
				float num = displayAreaWidth * axisCalibration.deadZone;
				RectTransform rectTransform = deadzoneArea;
				float x = num;
				Vector2 sizeDelta = deadzoneArea.sizeDelta;
				rectTransform.sizeDelta = new Vector2(x, sizeDelta.y);
				RectTransform rectTransform2 = deadzoneArea;
				float calibratedZero = axisCalibration.calibratedZero;
				Vector3 localPosition = deadzoneArea.parent.localPosition;
				float x2 = calibratedZero * (0f - localPosition.x);
				Vector2 anchoredPosition = deadzoneArea.anchoredPosition;
				rectTransform2.anchoredPosition = new Vector2(x2, anchoredPosition.y);
			}
		}

		private void RedrawCalibratedZero()
		{
			if (axisSelected)
			{
				RectTransform rectTransform = calibratedZeroMarker;
				float calibratedZero = axisCalibration.calibratedZero;
				Vector3 localPosition = deadzoneArea.parent.localPosition;
				float x = calibratedZero * (0f - localPosition.x);
				Vector2 anchoredPosition = calibratedZeroMarker.anchoredPosition;
				rectTransform.anchoredPosition = new Vector2(x, anchoredPosition.y);
				RedrawDeadzone();
			}
		}

		private void RedrawValueMarkers()
		{
			if (!axisSelected)
			{
				RectTransform rectTransform = calibratedValueMarker;
				Vector2 anchoredPosition = calibratedValueMarker.anchoredPosition;
				rectTransform.anchoredPosition = new Vector2(0f, anchoredPosition.y);
				RectTransform rectTransform2 = rawValueMarker;
				Vector2 anchoredPosition2 = rawValueMarker.anchoredPosition;
				rectTransform2.anchoredPosition = new Vector2(0f, anchoredPosition2.y);
			}
			else
			{
				float axis = joystick.GetAxis(selectedAxis);
				float num = Mathf.Clamp(joystick.GetAxisRaw(selectedAxis), -1f, 1f);
				RectTransform rectTransform3 = calibratedValueMarker;
				float x = displayAreaWidth * 0.5f * axis;
				Vector2 anchoredPosition3 = calibratedValueMarker.anchoredPosition;
				rectTransform3.anchoredPosition = new Vector2(x, anchoredPosition3.y);
				RectTransform rectTransform4 = rawValueMarker;
				float x2 = displayAreaWidth * 0.5f * num;
				Vector2 anchoredPosition4 = rawValueMarker.anchoredPosition;
				rectTransform4.anchoredPosition = new Vector2(x2, anchoredPosition4.y);
			}
		}

		private void SelectAxis(int index)
		{
			if (index < 0 || index >= axisButtons.Count || axisButtons[index] == null)
			{
				return;
			}
			axisButtons[index].interactable = false;
			axisButtons[index].Select();
			for (int i = 0; i < axisButtons.Count; i++)
			{
				if (i != index)
				{
					axisButtons[i].interactable = true;
				}
			}
			selectedAxis = index;
			origSelectedAxisCalibrationData = axisCalibration.GetData();
			SetMinSensitivity();
		}

		public override void TakeInputFocus()
		{
			base.TakeInputFocus();
			if (selectedAxis >= 0)
			{
				SelectAxis(selectedAxis);
			}
			RefreshControls();
			Redraw();
		}

		private void SetMinSensitivity()
		{
			if (!axisSelected)
			{
				return;
			}
			minSensitivity = 0.1f;
			if (rewiredStandaloneInputModule != null)
			{
				if (IsMenuAxis(menuHorizActionId, selectedAxis))
				{
					GetAxisButtonDeadZone(playerId, menuHorizActionId, ref minSensitivity);
				}
				else if (IsMenuAxis(menuVertActionId, selectedAxis))
				{
					GetAxisButtonDeadZone(playerId, menuVertActionId, ref minSensitivity);
				}
			}
		}

		private bool IsMenuAxis(int actionId, int axisIndex)
		{
			if (rewiredStandaloneInputModule == null)
			{
				return false;
			}
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			int count = allPlayers.Count;
			for (int i = 0; i < count; i++)
			{
				IList<JoystickMap> maps = allPlayers[i].controllers.maps.GetMaps<JoystickMap>(joystick.id);
				if (maps == null)
				{
					continue;
				}
				int count2 = maps.Count;
				for (int j = 0; j < count2; j++)
				{
					IList<ActionElementMap> axisMaps = maps[j].AxisMaps;
					if (axisMaps == null)
					{
						continue;
					}
					int count3 = axisMaps.Count;
					for (int k = 0; k < count3; k++)
					{
						ActionElementMap actionElementMap = axisMaps[k];
						if (actionElementMap.actionId == actionId && actionElementMap.elementIndex == axisIndex)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private void GetAxisButtonDeadZone(int playerId, int actionId, ref float value)
		{
			InputAction action = ReInput.mapping.GetAction(actionId);
			if (action != null)
			{
				int behaviorId = action.behaviorId;
				InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
				if (inputBehavior != null)
				{
					value = inputBehavior.buttonDeadZone + 0.1f;
				}
			}
		}
	}
}
