using System.IO;
using Xunit;
using AGS.Editor.Utils;

namespace Tests
{
    public class WriteIniFile
    {
        static string filename = "./Resources/acsetup.cfg";
        static string temppath = Path.GetTempPath();

        [Fact]
        public void WriteFile()
        {
            string writepath = Path.Combine(temppath, Path.GetRandomFileName());
            File.Copy(filename, writepath, false);
            IniFile ini;

            ini = new IniFile(writepath);
            string expected = ini.ToString();

            // just write it with no changes
            ini.Commit();

            ini = new IniFile(writepath);
            string actual = ini.ToString();

            Assert.Equal(expected, actual);
            File.Delete(writepath);
        }
    }
}

