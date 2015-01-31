using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smoke
{
    /// <summary>
    /// Enum to denote the version of a version of the framework protrocol
    /// </summary>
    public enum ProtocolVersion
    {
        Unknown = 0,
        v0_1    = 1
        // v0_2 = 2,        // Reserved
        // v0_3 = 3,        // Reserved
        // v1_0 = 10,       // Reserved
        // v1_1 = 11,       // Reserved
        // Client = 100     // Reserved

        // Not sure if this is the best way to indicate version, enum isn't extendable by clients 
        // creating new message protocols.
    }
}
