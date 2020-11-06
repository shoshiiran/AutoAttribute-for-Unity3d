﻿/* Author: Oran Bar
 * Summary: This attribute automatically assigns a class variable to one of the gameobject's component. 
 * It basically acts as an automatic GetComponent on Awake.
 * Using [Auto(true)], the behaviour can be extendend to act like an AddOrGetComponent: The component will be added if it is not found, instead of an error being thrown.
 * 
 * usage example
 * 
 * public class Foo
 * {
 *		[Auto] public BoxCollier myBoxCollier;	//This assigns the variable to the BoxColider attached on your object
 *		[Auto(true)] public Camera myCamera;	//If this GameObject has no camera, since we wrote (true), the component will be automayically added and referenced.
 *		
 *		//Methods...
 * }
 * 
 */

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class AutoAttribute : Attribute, IAutoAttribute {

	private const string MonoBehaviourNameColor = "green";	//Changeme

	private bool logMissingAsError = true;

	private Component targetComponent;

	public AutoAttribute(bool logMissingAsError = true)
	{
		this.logMissingAsError = logMissingAsError;
	}

	/// <Summary>
	///	Executes the call to fetch the component and assign it to the variable with [Auto*]
	/// </Summary>
	public bool Execute(MonoBehaviour mb, Type componentType, Action<MonoBehaviour, object> SetVariableType)
	{
		GameObject go = mb.gameObject;

		Component componentToReference = go.GetComponent(componentType);
		if (componentToReference == null)
		{
			LogMissingComponent(mb, componentType, go);
			
			return false;
		}

		SetVariableType(mb, componentToReference);
		
		return true;
	}

	
	private void LogMissingComponent(MonoBehaviour mb, Type componentType, GameObject go){
		string errorMessage = string.Format("[Auto]: <color={3}><b>{1}</b></color> couldn't find <color=#cc3300><b>{0}</b></color> on <color=#e68a00>{2}</color>",
						componentType.Name, mb.GetType().Name, go.name, MonoBehaviourNameColor);

		if (logMissingAsError)
		{
			//Logging an error during PostProcessScene halts the build.
			Debug.LogError(errorMessage, mb);
		}
		else
		{
			Debug.LogWarning("<color=red>" + errorMessage + "</color>", mb);
		}
	}
}