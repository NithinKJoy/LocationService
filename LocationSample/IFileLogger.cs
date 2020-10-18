using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LocationSample
{
    public interface IFileLogger
    {
        void LogInformation(string value);
    }
}
