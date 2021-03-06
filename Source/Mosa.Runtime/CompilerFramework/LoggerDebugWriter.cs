﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
    class LoggerDebugWriter : ILoggerWriter
    {
        public void WriteLine(string s)
        {
            Debug.Write(s);
        }
        public void Indent()
        {
            Debug.Indent();
        }
        public void Unindent()
        {
            Debug.Unindent();
        }
    }
}
