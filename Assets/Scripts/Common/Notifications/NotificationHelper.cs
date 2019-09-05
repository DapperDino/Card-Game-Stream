using System;

namespace CardGame.Common.Notifications
{
    public static class NotificationHelper
    {
        public static string PrepareNotification<T>() => PrepareNotification(typeof(T));
        public static string PrepareNotification(Type type) => $"{type.Name}.OnPrepare";

        public static string PerformNotification<T>() => PerformNotification(typeof(T));
        public static string PerformNotification(Type type) => $"{type.Name}.OnPerform";

        public static string ValidateNotification<T>() => ValidateNotification(typeof(T));
        public static string ValidateNotification(Type type) => $"{type.Name}.OnValidate";

        public static string CancelNotification<T>() => CancelNotification(typeof(T));
        public static string CancelNotification(Type type) => $"{type.Name}.OnCancel";
    }
}
