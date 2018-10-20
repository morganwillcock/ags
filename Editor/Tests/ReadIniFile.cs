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

        [Fact]
        public void GetBadValue()
        {
            IniFile ini = new IniFile(filename);
            string expected = "";
            string actual = ini.GetValue("misc", "no exist");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetBadSection()
        {
            IniFile ini = new IniFile(filename);
            string expected = "";
            string actual = ini.GetValue("no exist", "");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DumpFile()
        {
            IniFile ini = new IniFile(filename);
            string expected = @"[sound]
digiid=-1
midiid=-1
digiwin=-1
midiwin=-1
digiindx=0
midiindx=0
digiwinindx=0
midiwinindx=0

[misc]
game_width=1600
game_height=1200
gamecolordepth=32
antialias=0
notruecolor=0
cachemax=131072
user_data_dir=
shared_data_dir=
titletext=My Game Title

[graphics]
driver=Software
windowed=0
screen_def=max
game_scale_fs=max_round
game_scale_win=max_round
filter=stdscale
vsync=1
render_at_screenres=0

[language]
translation=

[mouse]
auto_lock=0
speed=1
";
            string actual = ini.ToString();
            Assert.Equal(expected, actual);
        }
    }
}
