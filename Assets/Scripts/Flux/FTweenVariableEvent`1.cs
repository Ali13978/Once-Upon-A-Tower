using System;
using System.Reflection;
using UnityEngine;

namespace Flux
{
	public abstract class FTweenVariableEvent<T> : FTweenEvent<T> where T : FTweenBase
	{
		public const BindingFlags VARIABLE_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		[SerializeField]
		[HideInInspector]
		private string _className;

		[SerializeField]
		[HideInInspector]
		private string _variableName;

		private Type _classType;

		private object _scriptReference;

		private FieldInfo _fieldInfo;

		private PropertyInfo _propertyInfo;

		public string ClassName
		{
			get
			{
				return _className;
			}
			set
			{
				_className = value;
				OnInit();
			}
		}

		public string VariableName
		{
			get
			{
				return _variableName;
			}
			set
			{
				_variableName = value;
				OnInit();
			}
		}

		protected override void OnInit()
		{
			if (_className == null)
			{
				return;
			}
			_classType = Type.GetType(_className);
			_fieldInfo = null;
			_propertyInfo = null;
			if (_classType == null)
			{
				return;
			}
			_scriptReference = Owner.GetComponent(_classType);
			if (_scriptReference != null && _variableName != null)
			{
				_fieldInfo = _classType.GetField(_variableName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (_fieldInfo == null)
				{
					_propertyInfo = _classType.GetProperty(_variableName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
			}
		}

		protected override void ApplyProperty(float t)
		{
			if (_fieldInfo != null)
			{
				if (_fieldInfo.IsStatic)
				{
					_fieldInfo.SetValue(null, GetValueAt(t));
				}
				else
				{
					_fieldInfo.SetValue(_scriptReference, GetValueAt(t));
				}
			}
			else if (_propertyInfo != null)
			{
				_propertyInfo.SetValue(_scriptReference, GetValueAt(t), null);
			}
		}

		protected abstract object GetValueAt(float t);
	}
}
