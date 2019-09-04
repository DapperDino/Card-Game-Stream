using System.Collections.Generic;
using UnityEngine;
using Handler = System.Action<object, object>;

using SenderTable = System.Collections.Generic.Dictionary<object, System.Collections.Generic.List<System.Action<object, object>>>;

namespace CardGame.Common.Notifications
{
    public class NotificationCenter
    {
        private readonly Dictionary<string, SenderTable> table = new Dictionary<string, SenderTable>();
        private readonly HashSet<List<Handler>> invoking = new HashSet<List<Handler>>();

        public static NotificationCenter Instance { get; } = new NotificationCenter();

        public void Clean()
        {
            string[] notKeys = new string[table.Keys.Count];

            table.Keys.CopyTo(notKeys, 0);

            for (int i = notKeys.Length - 1; i >= 0; --i)
            {
                string notificationName = notKeys[i];
                SenderTable senderTable = table[notificationName];

                object[] senKeys = new object[senderTable.Keys.Count];
                senderTable.Keys.CopyTo(senKeys, 0);

                for (int j = senKeys.Length - 1; j >= 0; --j)
                {
                    object sender = senKeys[j];
                    List<Handler> handlers = senderTable[sender];

                    if (handlers.Count == 0) { senderTable.Remove(sender); }
                }

                if (senderTable.Count == 0) { table.Remove(notificationName); }
            }
        }

        public void AddObserver(Handler handler, string notificationName) => AddObserver(handler, notificationName, null);

        public void AddObserver(Handler handler, string notificationName, object sender)
        {
            if (handler == null)
            {
                Debug.LogError($"Can't add a null event handler for the notification: {notificationName}");
                return;
            }

            if (string.IsNullOrEmpty(notificationName))
            {
                Debug.LogError("Can't observe an unamed notification");
                return;
            }

            if (!table.ContainsKey(notificationName)) { table.Add(notificationName, new SenderTable()); }

            SenderTable subTable = table[notificationName];

            object key = sender ?? (this);

            if (!subTable.ContainsKey(key)) { subTable.Add(key, new List<Handler>()); }

            List<Handler> list = subTable[key];

            if (!list.Contains(handler))
            {
                if (invoking.Contains(list)) { subTable[key] = list = new List<Handler>(list); }

                list.Add(handler);
            }
        }

        public void RemoveObserver(Handler handler, string notificationName)
        {
            RemoveObserver(handler, notificationName, null);
        }

        private void RemoveObserver(Handler handler, string notificationName, object sender)
        {
            if (handler == null)
            {
                Debug.LogError($"Can't remove a null event handler for the notification: {notificationName}");
                return;
            }

            if (string.IsNullOrEmpty(notificationName))
            {
                Debug.LogError("A notification name is required to stop observation");
                return;
            }

            if (!table.ContainsKey(notificationName)) { return; }

            SenderTable subTable = table[notificationName];

            object key = sender ?? (this);

            if (!subTable.ContainsKey(key)) { return; }

            List<Handler> list = subTable[key];
            int index = list.IndexOf(handler);

            if (index != -1)
            {
                if (invoking.Contains(list))
                {
                    subTable[key] = list = new List<Handler>(list);
                }

                list.RemoveAt(index);
            }
        }

        public void PostNotification(string notificationName)
        {
            PostNotification(notificationName, null);
        }

        public void PostNotification(string notificationName, object sender)
        {
            PostNotification(notificationName, sender, null);
        }

        public void PostNotification(string notificationName, object sender, object e)
        {
            if (string.IsNullOrEmpty(notificationName))
            {
                Debug.LogError("A notification name is required");
                return;
            }

            if (!table.ContainsKey(notificationName)) { return; }

            SenderTable subTable = table[notificationName];

            if (sender != null && subTable.ContainsKey(sender))
            {
                List<Handler> handlers = subTable[sender];

                invoking.Add(handlers);

                for (int i = 0; i < handlers.Count; ++i)
                {
                    handlers[i](sender, e);
                }

                invoking.Remove(handlers);
            }

            if (subTable.ContainsKey(this))
            {
                List<Handler> handlers = subTable[this];

                invoking.Add(handlers);

                for (int i = 0; i < handlers.Count; ++i)
                {
                    handlers[i](sender, e);
                }

                invoking.Remove(handlers);
            }
        }
    }

    public static class NotificationCenterExtensions
    {
        public static void PostNotification(this object obj, string notificationName) => NotificationCenter.Instance.PostNotification(notificationName, obj);
        public static void PostNotification(this object obj, string notificationName, object e) => NotificationCenter.Instance.PostNotification(notificationName, obj, e);
        public static void AddObserver(this object obj, Handler handler, string notificationName) => NotificationCenter.Instance.AddObserver(handler, notificationName);
        public static void AddObserver(this object obj, Handler handler, string notificationName, object sender) => NotificationCenter.Instance.AddObserver(handler, notificationName, sender);
        public static void RemoveObserver(this object obj, Handler handler, string notificationName) => NotificationCenter.Instance.RemoveObserver(handler, notificationName);
        public static void RemoveObserver(this object obj, Handler handler, string notificationName, object sender) => NotificationCenter.Instance.RemoveObserver(handler, notificationName, sender);
    }
}