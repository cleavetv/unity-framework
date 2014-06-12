using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CleaveFramework.Interfaces;

namespace __GameDevTest
{
    class Foo : IUpdateable
    {
        public int Value = 5;
        public string Word = "Hello World";

        public void Update()
        {
            Value++;
        }
    }
}
