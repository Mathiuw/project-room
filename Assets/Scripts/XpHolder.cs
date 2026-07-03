using UnityEngine;

namespace MaiNull
{
	public class XpHolder
	{
		private readonly AnimationCurve _experienceCurve;
		private uint _currenLevel;
		private int _totalExperience = 0;
        private int _previousLevelsExperience, _nextLevelsExperience;

        public XpHolder(AnimationCurve experienceCurve, uint currenLevel)
        {
	        _experienceCurve = experienceCurve;
	        _currenLevel = currenLevel;
        }

        public void AddExperience(int amount)
		{
			_totalExperience += amount;
			CheckForLevelUp();
		}

        private void CheckForLevelUp()
        {
            if (_totalExperience >= _nextLevelsExperience)
            {
				LevelUp();
            }
        }

        private void LevelUp()
        {
			_currenLevel++;
			_previousLevelsExperience = (int)_experienceCurve.Evaluate(_currenLevel);
			_nextLevelsExperience = (int)_experienceCurve.Evaluate(_currenLevel + 1);
        }
    }
}