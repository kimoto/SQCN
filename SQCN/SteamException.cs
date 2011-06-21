using System;
using System.Collections.Generic;
using System.Text;

namespace SQCN
{
    class SteamException : Exception
    {
        public SteamException( string msg )
            : base( msg )
        {
        }

        public SteamException( string msg, Exception ex )
            : base( msg, ex )
        {
        }
    }
}
