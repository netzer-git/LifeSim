public struct GrassGenome
{
	// health and energy
	public float energy; // start and max amount of energy, replenish on eating. everything costs energy
	public float health; // start and max amount of health, replenish on long rests
	public float idleEnergyConsumption; // energy consumed when idle
	public float sleepEnergyGain; // energy gained when sleeping
	// eat
	public float biteStrength; // amount of food for each bite
	public float biteEnergyCost; // energy cost for each bite
	public float nutrientConsumption; // amount of food that turns into energy
	public float hungerIncreaseRate;    // Rate at which hunger increases over time
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