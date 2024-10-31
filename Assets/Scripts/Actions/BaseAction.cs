using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
	// Priority or utility value can be used for decision-making
	public float Priority { get; protected set; }
	// Action's name
	public string ActionName { get { return GetType().Name; } }

	public virtual void Initialize()
	{
		// Common initialization code
		return;
	}

	// check for both current state excecutaion and gene abilities
	public abstract bool CanExecute(AgentState agentState);

	public abstract void Execute();

	public virtual void Finish()
	{
		// Common cleanup code
		return;
	}
}