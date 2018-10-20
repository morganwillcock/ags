using System;
using System.Collections.Generic;
using Xunit;
using AGS.Editor.Utils;

namespace Tests
{
    public class ReadIniFile
    {
        static string filename = "./Resources/acsetup.cfg";

        [Fact]
        public void GetValue()
        {
            IniFile ini = new IniFile(filename);
            string expected = "My Game Title";
            string actual = ini.GetValue("misc", "titletext");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetEmptyValue()
        {
            IniFile ini = new IniFile(filename);
            string expected = "";
            string actual = ini.GetValue("misc", "user_data_dir");
            Assert.Equal(expected, actual);
        }
    }
}
