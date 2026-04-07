namespace MaiNull
{
    public interface IState
    {
        void Tick();

        void OnEnter();

        void OnExit();
    }
}
