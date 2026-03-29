using System;
using UnityEngine;

namespace MaiNull
{
	public class XPHolder : MonoBehaviour
	{
		[Header("Experience")]
		[SerializeField] private AnimationCurve experienceCurve;
		
		private uint currenLevel = 1;
		private int totalExperience = 0;
        private int previousLevelsExperience, nextLevelsExperience;

		public void AddExperience(int amount)
		{
			totalExperience += amount;
			CheckForLevelUp();
		}

        private void CheckForLevelUp()
        {
            if (totalExperience >= nextLevelsExperience)
            {
				LevelUp();
            }
        }

        private void LevelUp()
        {
			currenLevel++;
			previousLevelsExperience = (int)experienceCurve.Evaluate(currenLevel);
			nextLevelsExperience = (int)experienceCurve.Evaluate(currenLevel + 1);
        }
    }
}