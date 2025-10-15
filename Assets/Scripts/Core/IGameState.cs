using UnityEngine;

namespace BasketLegend.Core
{
    public interface IGameState
    {
        void Enter();
        void Update();
        void Exit();
    }
}