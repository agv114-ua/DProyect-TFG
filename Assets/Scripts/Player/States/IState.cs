using UnityEngine;

public interface IState
{
    void Enter(); // Se ejecuta al entrar en el estado
    void Execute(); // Se ejecuta en cada frame mientras se está en el estado
    void Exit(); // Se ejecuta al salir del estado
}
