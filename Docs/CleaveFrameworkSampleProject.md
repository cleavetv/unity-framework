# Sample Project

## Character_Paperdoll

This project creates a small paperdoll system you might find in any RPG type game with items and character stats.  

   * Use an interface and base class implementation along with the Injector to create dynamic objects at runtime: `(IPaperdoll -> Paperdoll -> PlayerPaperdoll/MonsterPaperdoll)`.  
   * Use Bindings to create a Paperdoll stat container with restrictive item slots.  
   * Use Commands to equip items into the various paperdoll slots.
   * Use the Injector and Factory classes inside of SceneView to construct a scene in various ways depending on your needs.
