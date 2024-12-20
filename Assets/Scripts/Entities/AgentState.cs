﻿using System.Collections.Generic;

public enum SatietyLevel { Low = 1, Medium = 2, High = 3 }
public enum EnergyLevel { Low = 1, Medium = 2, High = 3 }
public enum DetectedObjectType { Food, Predator, Mate, None }

public class AgentStateActionPair
{
	public AgentState state;
	public BaseAction action;

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
			return false;

		AgentStateActionPair other = (AgentStateActionPair)obj;
		return state.Equals(other.state) && action.ActionName == other.action.ActionName;
	}

	public override int GetHashCode()
	{
		int hash = 17;
		hash = hash * 31 + state.GetHashCode();
		hash = hash * 31 + action.ActionName.GetHashCode();
		return hash;
	}
}

public class AgentState
{
	public SatietyLevel satietyLevel;
	public EnergyLevel energyLevel;
	public List<DetectedObjectType> detectedObjectsTypes;

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
			return false;

		AgentState other = (AgentState)obj;
		return satietyLevel == other.satietyLevel && energyLevel == other.energyLevel && SameObjects(detectedObjectsTypes, other.detectedObjectsTypes);
	}

	private bool SameObjects(List<DetectedObjectType> a, List<DetectedObjectType> b)
	{
		if (a.Count != b.Count)
			return false;
		foreach (var item in a)
		{
			if (!b.Contains(item))
				return false;
		}
		return true;
	}

	public override int GetHashCode()
	{
		int hash = 13;
		hash = hash * 7 + satietyLevel.GetHashCode();
		hash = hash * 7 + energyLevel.GetHashCode();
		foreach (var obj in detectedObjectsTypes)
		{
			hash = hash * 7 + obj.GetHashCode();
		}
		return hash;
	}
}

