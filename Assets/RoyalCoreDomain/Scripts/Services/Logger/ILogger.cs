using System;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;

namespace RoyalCoreDomain.Scripts.Services.Logger
{
    public interface ILogger
    {
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogException(Exception exception);
        void LogTopic(string message, LogTopicType logTopicType = LogTopicType.Temp, string callerFilePath = "", string callerMemberName = "");
    }
}