using System;

namespace Health.API.Model
{
    public class PortalUser
    {
        public string LoginId { get { return "hp@olto.dev"; } }

        public string Name { get { return "Patel, Hiten"; } }
        public string Role { get { return "Portal User"; } }

        public static PortalUser Get(string loginId) {
            return new PortalUser();
        }
        public static bool Valid(string LoginId, string passcode) {
            PortalUser user = PortalUser.Get(LoginId);
            if (user.LoginId.Equals(LoginId, StringComparison.InvariantCultureIgnoreCase)) {
                return true;
            }
            return false;
        }
    }
}
