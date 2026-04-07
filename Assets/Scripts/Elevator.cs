using MaiNull.Singleton;
using UnityEngine;

namespace MaiNull
{
    public class Elevator : MonoBehaviour
    {
        [SerializeField] ElevatorPanel panelInside;

        void Start() 
        {
            panelInside.onButtomPress += EndGame;
        }

        void EndGame() => GameManager.Instance.SceneTransition(2);
    }
}