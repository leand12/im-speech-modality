﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speechModality
{
    public class SpeechEventArg
    {
        public string Text { get; set; }
        public double Confidence { get; set; }
        public string Out { get; set; }
        public bool Final { get; set; }
    }
}
