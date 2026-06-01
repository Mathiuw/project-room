using UnityEngine;

namespace MaiNull
{
    public class PickableCard : Pickable
    {
        [SerializeField] private CardData card;
        
        public override string readName => card ? card.name : "CardData Not Found";

        public override void Interact(Transform interactor)
        {
            base.Interact(interactor);
            CardInventory.AddCard(card);
            Destroy(gameObject);
        }
    }
}