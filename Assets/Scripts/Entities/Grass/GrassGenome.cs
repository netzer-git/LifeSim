public struct GrassGenome
{
	// learning
	public float explorationRate; // exploration rate
	// health and energy
	public float health; // start and max amount of health, replenish on long rests
	public float idleEnergyConsumption; // energy consumed when idle
	public float sleepEnergyGain; // energy gained when sleeping
	// eat
	public float biteStrength; // amount of food for each bite
	public float biteEnergyCost; // energy cost for each bite
	public float stomachSize; // amount of maximum food in the stomach
	public float satietyDecreaseRate;    // Rate at which hunger increases over time
	// move
	public float moveSpeed; // movement speed
	public float moveEnergyCost; // energy cost for a move
	// look
	public float sightRadius; // field of view depth
	// rotate
	public float rotationSpeed; // rotation speed
	// birth
	public float birthEnergyCost; // cost in energy for birth
	public float mutationRate; // mutation rate
}