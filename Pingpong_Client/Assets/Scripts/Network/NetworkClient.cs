using System;
using System.Collections.Generic;
using Model;
using SocketIO;
using UnityEngine;
using Utils;

namespace Network
{
    public class NetworkClient : SocketIOComponent
    {
        [Header("Network Client")] [SerializeField]
        private Transform networkContainer;

        [SerializeField] private GameObject playerPrefab;

        [SerializeField] public GameObject ballPrefab;

        private Dictionary<string, NetworkIdentity> serverObjects;

        [Header("Game Start Handler")] public GameObject ownPlayerSpawnPoint;

        public GameObject enemyPlayerSpawnPoint;

        public static string ClientId { get; private set; }

        private void Awake()
        {
            Settings settings = SettingsReader.ReadSettings();
            url = settings.serverSetting.getAddress();
            base.Awake();
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            Initialize();
            SetupEvents();
        }

        private void Initialize()
        {
            serverObjects = new Dictionary<string, NetworkIdentity>();
        }

        private void SetupEvents()
        {
            On("open", (e) => { Debug.Log("Connection made to server"); });

            On("register", (e) =>
            {
                Debug.Log("REGISTER");

                ClientId = GetPlayerId(e.data);
                Debug.LogFormat("Our client's Id ({0})", ClientId);
            });

            On("playerRejected", (e) =>
            {
                // TODO 
            });

            On("spawn", (e) =>
            {
                string id = GetPlayerId(e.data);
                Debug.Log("SPAWN " + id);

                GameObject gameObject = Instantiate(playerPrefab);
                gameObject.name = string.Format("Player ({0})", id);

                // only two players
                if (serverObjects.Count >= 2)
                {
                    return;
                }

                SpawnPlayer(gameObject, id);

                NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
                networkIdentity.SetControllerId(id);
                networkIdentity.SetSocket(this);

                serverObjects.Add(id, networkIdentity);
            });

            On("disconnectClient", (e) =>
            {
                Debug.Log("DISCONNECT");

                string id = GetPlayerId(e.data);

                GameObject gameObject = serverObjects[id].gameObject;
                Destroy(gameObject);
                serverObjects.Remove(id);
            });

            On("updateClientPosition", (e) =>
            {
                Debug.Log("updateClientPosition");
                //Debug.Log(e.data);

                string id = GetPlayerId(e.data);

                if(!IsOwnPlayer(id)){
                    Debug.Log("enemy : " + e.data);
                }

                float x = Convert.ToSingle(e.data["position"]["x"].str.Replace(",", "."));
                float y = Convert.ToSingle(e.data["position"]["y"].str.Replace(",", "."));
                float z = Convert.ToSingle(e.data["position"]["z"].str.Replace(",", "."));

                if(!IsOwnPlayer(id)){
                    Debug.Log("enemy : " + e.data);
                    Debug.Log("x : " + x);
                    Debug.Log("y : " + y);
                }

                NetworkIdentity networkIdentity = serverObjects[id];

                PositionPlayer(id, networkIdentity, x, y, z);
            });

            On("updateBallPosition", (e) =>
            {
                Debug.Log("updateBallPosition");

                BallPosition ballPosition = JsonUtility.FromJson<BallPosition>(e.data.ToString());

                string id = GetPlayerId(e.data);

                // adapt position from enemy
                GameObject ball = Instantiate(ballPrefab);
                Rigidbody ballBody = ball.GetComponent<Rigidbody>();

                if (IsOwnPlayer(id))
                {
                    ballBody.velocity = new Vector3(
                        ConvertToFloat(ballPosition.velocity.x),
                        ConvertToFloat(ballPosition.velocity.y),
                        ConvertToFloat(ballPosition.velocity.z)
                    );
                    ball.transform.position = new Vector3(
                        ConvertToFloat(ballPosition.position.x),
                        ConvertToFloat(ballPosition.position.y),
                        ConvertToFloat(ballPosition.position.z)
                    );
                }
                else
                {
                    ballBody.velocity = new Vector3(
                        (-1) * ConvertToFloat(ballPosition.velocity.x),
                        ConvertToFloat(ballPosition.velocity.y),
                        (-1) * ConvertToFloat(ballPosition.velocity.z)
                    );
                    ball.transform.position = new Vector3(
                        (-1) * ConvertToFloat(ballPosition.position.x),
                        ConvertToFloat(ballPosition.position.y),
                        (-1) * ConvertToFloat(ballPosition.position.z)
                    );
                }
            });
        }

        private void SpawnPlayer(GameObject gameObject, string playerId)
        {
            // place own player in front of the camera
            Vector3 position = IsOwnPlayer(playerId)
                ? ownPlayerSpawnPoint.transform.position
                : enemyPlayerSpawnPoint.transform.position;

            gameObject.transform.position = position;
            Debug.Log("Spawn Player Position " + playerId + " : " + JsonUtility.ToJson(gameObject.transform.position));

            Color playerColor = IsOwnPlayer(playerId) ? new Color(0, 0, 1, 0.5f) : new Color(1, 0, 0, 0.5f);
            gameObject.GetComponent<MeshRenderer>().material.color = playerColor;

            // camera position
            if (IsOwnPlayer(playerId))
            {
                Camera.main.transform.position =
                    gameObject.transform.position - gameObject.transform.forward * 20 + gameObject.transform.up * 3;
            }
        }

        private void PositionPlayer(string playerId, NetworkIdentity player, float x, float y, float z)
        {
            Vector3 position = IsOwnPlayer(playerId) ? new Vector3(x, y, z) : new Vector3((-1) * x, y, (-1) * z);

            if(!IsOwnPlayer(playerId)){
                //Debug.Log("enemy : " + JsonUtility.ToJson(position));
            }

            player.transform.position = position;
        }

        private string GetPlayerId(JSONObject data)
        {
            return data["id"].ToString().Replace("\"", "");
        }

        private bool IsOwnPlayer(string playerId)
        {
            return ClientId == playerId;
        }

        private float ConvertToFloat(string value)
        {
            return Convert.ToSingle(value);
        }
    }

    [Serializable]
    public class Player
    {
        public string id;

        public Position position;
    }

    [Serializable]
    public class Position
    {
        public Position(string x, string y, string z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public string x;

        public string y;

        public string z;
    }

    public class BallPosition
    {
        public string id;

        public Position position;

        public Position velocity;
    }
}