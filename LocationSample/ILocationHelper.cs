using System;
namespace LocationSample
{
    public interface ILocationHelper
    {
        void Start();
        void Stop();
        void PositionChanged();
        event EventHandler<LocationData> OnLocationReceived;
    }
}
