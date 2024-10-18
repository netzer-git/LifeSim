using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AgentController : MonoBehaviour
{
	public List<BaseSkill> skills = new List<BaseSkill>();

	private void Start()
	{
		skills.AddRange(GetComponents<BaseSkill>());
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			ExecuteSkills();
		}
	}

	public void AddSkill(BaseSkill skill)
	{
		skills.Add(skill);
		skill.transform.SetParent(transform);
	}

	public void RemoveSkill(BaseSkill skill)
	{
		skills.Remove(skill);
		Destroy(skill);
	}

	public void ExecuteSkills()
	{
		foreach (var skill in skills)
		{
			skill.Execute();
		}
	}
}