using UnityEngine;

namespace Game
{
    public class GameHandler
    {
        
        public GameObject ownPlayerSpawnPoint;

        public GameObject enemyPlayerSpawnPoint; 
        
    }

    public enum GameAction
    {
        Register,

        PlayerRejection,

        PlayerSpawn,

        DisconnectPlayer,

        PlayerPositionUpdate,

        BallPositionUpdate
    }
}