using UnityEngine;

namespace MaiNull.Item
{
	[CreateAssetMenu(fileName ="new_ability", menuName = "Abilities/Ability")]
	public class AbilityData : ScriptableObject
	{
		public string abilityName = "Ability";


		public virtual void Acivate()
		{
			Debug.Log($"{abilityName} activated");
		}
	}
}