using System.Collections.Generic;
using System.Linq;

namespace Terminal3WebAPI.Hubs
{
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, string> _connections =
            new Dictionary<T, string>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
               string connection;
                if (!_connections.TryGetValue(key, out connection))
                {
                    _connections.Add(key, connectionId);
                }
            }
        }

        public string GetConnections(T key)
        {
            string connection;
            if (_connections.TryGetValue(key, out connection))
            {
                return connection;
            }

            return string.Empty;
        }

        public void Remove(T key)
        {
            lock (_connections)
            {
                _connections.Remove(key);
            }

        }
    }
}