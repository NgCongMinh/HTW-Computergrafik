using SocketIO;
using UnityEngine;

namespace Network
{
    public class NetworkIdentity : MonoBehaviour
    {
        [Header("Helpful Values")] [SerializeField]
        private string id;

        [SerializeField]
        private bool isControlling;

        private SocketIOComponent socket;

        // Start is called before the first frame update
        public void Awake()
        {
            isControlling = false;
        }

        public void SetControllerId(string id)
        {
            this.id = id;
            isControlling = (NetworkClient.ClientId == id) ? true : false;
        }

        public void SetSocket(SocketIOComponent socket)
        {
            this.socket = socket;
        }

        public string GetId()
        {
            return id;
        }

        public bool IsControlling()
        {
            return isControlling;
        }
        
        public SocketIOComponent GetSocket()
        {
            return socket;
        }
        
    }
}