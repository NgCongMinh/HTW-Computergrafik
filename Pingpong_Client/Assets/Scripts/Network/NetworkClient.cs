using System;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;

namespace Network
{
    public class NetworkClient : SocketIOComponent
    {
        [Header("Network Client")] [SerializeField]
        private Transform networkContainer;

        [SerializeField] private GameObject playerPrefab;

        private Dictionary<string, NetworkIdentity> serverObjects;

        public static string ClientId { get; private set; }

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

                ClientId = getPlayerId(e.data);
                Debug.LogFormat("Our client's Id ({0})", ClientId);
            });

            On("spawn", (e) =>
            {
                string id = getPlayerId(e.data);

                Debug.Log("SPAWN " + id);

                GameObject gameObject = Instantiate(playerPrefab, networkContainer);
                gameObject.name = string.Format("Player ({0})", id);

                gameObject.transform.localPosition = new Vector3(
                    e.data["position"]["x"].f,
                    e.data["position"]["y"].f,
                    e.data["position"]["z"].f
                );

                NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
                networkIdentity.SetControllerId(id);
                networkIdentity.SetSocket(this);

                serverObjects.Add(id, networkIdentity);
            });

            On("disconnectClient", (e) =>
            {
                Debug.Log("DISCONNECT");

                string id = getPlayerId(e.data);

                GameObject gameObject = serverObjects[id].gameObject;
                Destroy(gameObject);
                serverObjects.Remove(id);
            });

            On("updateClientPosition", (e) =>
            {
                string id = getPlayerId(e.data);

                float x = Convert.ToSingle(e.data["position"]["x"].str);
                float y = Convert.ToSingle(e.data["position"]["y"].str);
                float z = Convert.ToSingle(e.data["position"]["z"].str);

                NetworkIdentity networkIdentity = serverObjects[id];

                networkIdentity.transform.localPosition = new Vector3(x, y, z);
            });
        }

        private string getPlayerId(JSONObject data)
        {
            return data["id"].ToString().Replace("\"", "");
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
        public string x;

        public string y;

        public string z;
    }
}