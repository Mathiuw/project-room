using UnityEngine;

namespace MaiNull
{
    [CreateAssetMenu(fileName = "Recover_Health_Card", menuName = "Card/Consumable/Recover Health")]
    public class CardRecoverHealth : Card
    {
        public float recoverAmount;
        
        public override void ApplyCardEffect(Transform objectToApply)
        {
            if (!objectToApply.TryGetComponent(out Player player)) return;
            
            player.Health.AddHealth((int)recoverAmount);
            Debug.Log($"Recovered {recoverAmount} health to {player.name}");
        }
    }
}