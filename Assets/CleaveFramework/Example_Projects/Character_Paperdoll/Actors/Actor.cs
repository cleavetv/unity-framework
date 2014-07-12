using CleaveFramework.DependencyInjection;
using CleaveFramework.Example_Projects.Character_Paperdoll.Item;
using CleaveFramework.Example_Projects.Character_Paperdoll.Paperdoll;

namespace CleaveFramework.Example_Projects.Character_Paperdoll.Actors
{
    public class Actor
    {
        [Inject] public IPaperdoll Paperdoll;



        public Actor()
        {
            // We need to ask the injector to PerformInjections() on us because we use
            // a Transient interface and the Injector needs the type information.
            // This requirement may go away in the future.
            Injector.PerformInjections(this);
        }

        /// <summary>
        /// wrapper for Paperdoll
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="item"></param>
        public BaseItem Equip(PaperdollSlot slot, BaseItem item)
        {
            return Paperdoll.Equip(slot, item);
        }


    }
}
