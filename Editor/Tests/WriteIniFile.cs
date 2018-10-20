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

        [Fact]
        public void SetNothing()
        {
            string writepath = Path.Combine(temppath, Path.GetRandomFileName());
            File.Copy(filename, writepath, false);
            IniFile ini;

            ini = new IniFile(writepath);
            string expected = ini.ToString();

            ini.SetValue("", "", "");
            ini.Commit();

            ini = new IniFile(writepath);
            string actual = ini.ToString();

            Assert.Equal(expected, actual);
            File.Delete(writepath);
        }

        [Fact]
        public void SetValue()
        {
            string writepath = Path.Combine(temppath, Path.GetRandomFileName());
            File.Copy(filename, writepath, false);
            IniFile ini;

            ini = new IniFile(writepath);
            ini.SetValue("misc", "titletext", "New Game Title");
            ini.Commit();

            ini = new IniFile(writepath);
            string expected = "New Game Title";
            string actual = ini.GetValue("misc", "titletext");

            Assert.Equal(expected, actual);
            File.Delete(writepath);
        }

        [Fact]
        public void SetEmptyValue()
        {
            string writepath = Path.Combine(temppath, Path.GetRandomFileName());
            File.Copy(filename, writepath, false);
            IniFile ini;

            ini = new IniFile(writepath);
            ini.SetValue("misc", "titletext", "");
            ini.Commit();

            ini = new IniFile(writepath);
            string expected = "";
            string actual = ini.GetValue("misc", "titletext");

            Assert.Equal(expected, actual);
            File.Delete(writepath);
        }

        [Fact]
        public void SetValueInNewKey()
        {
            string writepath = Path.Combine(temppath, Path.GetRandomFileName());
            File.Copy(filename, writepath, false);
            IniFile ini;

            ini = new IniFile(writepath);
            ini.SetValue("misc", "newkey", "newvalue");
            ini.Commit();

            ini = new IniFile(writepath);
            string expected = "newvalue";
            string actual = ini.GetValue("misc", "newkey");

            Assert.Equal(expected, actual);
            File.Delete(writepath);
        }

        [Fact]
        public void SetEmptyValueInNewKey()
        {
            string writepath = Path.Combine(temppath, Path.GetRandomFileName());
            File.Copy(filename, writepath, false);
            IniFile ini;

            ini = new IniFile(writepath);
            ini.SetValue("misc", "newkey", "");
            ini.Commit();

            ini = new IniFile(writepath);
            string expected = "";
            string actual = ini.GetValue("misc", "newkey");

            Assert.Equal(expected, actual);
            File.Delete(writepath);
        }

        [Fact]
        public void SetValueInNewSection()
        {
            string writepath = Path.Combine(temppath, Path.GetRandomFileName());
            File.Copy(filename, writepath, false);
            IniFile ini;

            ini = new IniFile(writepath);
            ini.SetValue("newsection", "newkey", "newvalue");
            ini.Commit();

            ini = new IniFile(writepath);
            string expected = "newvalue";
            string actual = ini.GetValue("newsection", "newkey");

            Assert.Equal(expected, actual);
            File.Delete(writepath);
        }

        [Fact]
        public void SetEmptyValueInNewSection()
        {
            string writepath = Path.Combine(temppath, Path.GetRandomFileName());
            File.Copy(filename, writepath, false);
            IniFile ini;

            ini = new IniFile(writepath);
            ini.SetValue("newsection", "newkey", "");
            ini.Commit();

            ini = new IniFile(writepath);
            string expected = "";
            string actual = ini.GetValue("newsection", "newkey");

            Assert.Equal(expected, actual);
            File.Delete(writepath);
        }

        [Fact]
        public void DeleteSection()
        {
            string writepath = Path.Combine(temppath, Path.GetRandomFileName());
            File.Copy(filename, writepath, false);
            IniFile ini;

            ini = new IniFile(writepath);
            ini.DeleteSection("misc");
            ini.Commit();

            ini = new IniFile(writepath);
            string expected = "";
            string actual = ini.GetValue("misc", "titletext");

            Assert.Equal(expected, actual);
            File.Delete(writepath);
        }

        [Fact]
        public void DeleteKey()
        {
            string writepath = Path.Combine(temppath, Path.GetRandomFileName());
            File.Copy(filename, writepath, false);
            IniFile ini;

            ini = new IniFile(writepath);
            ini.DeleteKey("misc", "titletext");
            ini.Commit();

            ini = new IniFile(writepath);
            string expected = "";
            string actual = ini.GetValue("misc", "titletext");

            Assert.Equal(expected, actual);
            File.Delete(writepath);
        }

        [Fact]
        public void NewFile()
        {
            string writepath = Path.Combine(temppath, Path.GetRandomFileName());
            IniFile ini;

            ini = new IniFile(writepath);
            ini.SetValue("My Section", "My Key", "My Value");
            ini.Commit();

            ini = new IniFile(writepath);
            string expected = "My Value";
            string actual = ini.GetValue("My Section", "My Key");

            Assert.Equal(expected, actual);
            File.Delete(writepath);
        }

        [Fact]
        public void NewFileWithEmptySection()
        {
            string writepath = Path.Combine(temppath, Path.GetRandomFileName());
            IniFile ini;

            ini = new IniFile(writepath);
            ini.SetValue("My Section", "", "");
            ini.Commit();

            ini = new IniFile(writepath);
            string expected = "";
            string actual = ini.ToString();

            Assert.Equal(expected, actual);
            File.Delete(writepath);
        }

        [Fact]
        public void NewFileWithEmptiedSection()
        {
            string writepath = Path.Combine(temppath, Path.GetRandomFileName());
            IniFile ini;

            ini = new IniFile(writepath);
            ini.SetValue("My Section", "My Key", "My Value");
            ini.DeleteKey("My Section", "My Key");
            ini.Commit();

            ini = new IniFile(writepath);
            string expected = "";
            string actual = ini.ToString();

            Assert.Equal(expected, actual);
            File.Delete(writepath);
        }
    }
}

