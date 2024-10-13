using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
	public abstract void Execute();
	public SkillActivationType SkillActivationType;
}

public enum SkillActivationType
{
	Update,
	Start,
}