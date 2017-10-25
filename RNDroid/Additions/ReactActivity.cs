using System;

namespace Com.Facebook.React
{
    public abstract partial class ReactActivity
    {
        new public int CheckSelfPermission(string permission)
        {
            return (int)base.CheckSelfPermission(permission);
        }

        new public int CheckPermission(string permission, int pid, int uid)
        {
            return (int)base.CheckPermission(permission, pid, uid);
        }
    }
}
