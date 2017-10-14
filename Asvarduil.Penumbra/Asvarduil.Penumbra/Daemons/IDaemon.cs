using System;

namespace Asvarduil.Penumbra.Daemons
{
    public interface IDaemon
    {
        string Name { get; }
        DateTime LastRan { get; set; }
        int Period { get; }

        void OnInvoked();
    }
}
