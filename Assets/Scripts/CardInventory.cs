using System;
using System.Collections.Generic;
using MaiNull.Singleton;

namespace MaiNull
{
    public class CardInventory : Singleton<CardInventory>
    {
        private static List<CardData> _cards;

        public static List<CardData> Cards => _cards;

        public static event Action<CardData> OnCardAdded;
        
        public static event Action<CardData> OnCardRemoved;
        
        public static void AddCard(CardData card)
        {
            _cards.Add(card);
            OnCardAdded?.Invoke(card);
        }

        public static void RemoveCard(CardData card)
        {
            _cards.Remove(card);
            OnCardRemoved?.Invoke(card);
        }
    }
}