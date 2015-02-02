using System;

using Dilemma.Common;
using Dilemma.Security.AccessFilters.ByEnum;

namespace Dilemma.Security.AccessFilters
{
    public static class ServerRoleControllerAction
    {
        public static ControllerAction GetControllerAction(ServerRole serverRole)
        {
            switch (serverRole)
            {
                case ServerRole.Offline:
                    return new ControllerAction("Offline", "Index");
                case ServerRole.Moderation:
                    return new ControllerAction("Moderation", "Index");
                case ServerRole.Public:
                    return new ControllerAction("Home", "Index");
                case ServerRole.QuestionSeeder:
                    return new ControllerAction("Question", "Seeder");
                default:
                    throw new ArgumentOutOfRangeException("serverRole");
            }
        }
    }
}