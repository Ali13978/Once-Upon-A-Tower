using System;
using System.Reflection;
using UnityEngine;

namespace Flux
{
	[FEvent("Script/Call Function")]
	public class FCallFunctionEvent : FEvent
	{
		public const BindingFlags METHOD_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		[SerializeField]
		[Tooltip("If false, it gets called every tick")]
		private bool _callOnlyOnTrigger = true;

		[SerializeField]
		[HideInInspector]
		private string _className;

		[SerializeField]
		[HideInInspector]
		private string _methodName;

		private Type _classType;

		private object _scriptReference;

		private MethodInfo _methodInfo;

		public bool CallOnlyOnTrigger
		{
			get
			{
				return _callOnlyOnTrigger;
			}
			set
			{
				_callOnlyOnTrigger = value;
			}
		}

		public string ClassName
		{
			get
			{
				return _className;
			}
			set
			{
				_className = value;
			}
		}

		public string MethodName
		{
			get
			{
				return _methodName;
			}
			set
			{
				_methodName = value;
			}
		}

		protected override void OnInit()
		{
			_classType = GetType(_className);
			if (_classType != null)
			{
				_scriptReference = Owner.GetComponent(_classType);
				if (_scriptReference != null)
				{
					_methodInfo = _classType.GetMethod(_methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
			}
		}

		protected override void OnTrigger(float timeSinceTrigger)
		{
			CallFunction();
		}

		protected override void OnUpdateEvent(float timeSinceTrigger)
		{
			if (!_callOnlyOnTrigger)
			{
				CallFunction();
			}
		}

		private Type GetType(string className)
		{
			Type type = Type.GetType(className);
			if (type == null)
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i != assemblies.Length; i++)
				{
					type = assemblies[i].GetType(className);
					if (type != null)
					{
						break;
					}
				}
			}
			return type;
		}

		private void CallFunction()
		{
			if (_methodInfo != null)
			{
				if (_methodInfo.IsStatic)
				{
					_methodInfo.Invoke(null, null);
				}
				else
				{
					_methodInfo.Invoke(_scriptReference, null);
				}
			}
		}
	}
}
