using System.ComponentModel.Design;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
	public abstract void Execute();
	// public SkillActivationType activationType = SkillActivationType.None;
}

public enum SkillActivationType
{
	None,
	Update,
	Start,
	OnCollide,
	Active
}