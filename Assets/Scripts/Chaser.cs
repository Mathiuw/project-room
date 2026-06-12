using UnityEngine;

namespace MaiNull
{
	public class Chaser : MonoBehaviour, IDamageable
	{
		[SerializeField] private int maxHealth = 60;
		
		public Health Health { get; private set; }

		private void Awake()
		{
			Health = new Health(maxHealth);
			Health.OnDie += OnDie;
		}
		
		private void OnDie()
		{
			Destroy(gameObject);
		}
	}
}
