using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AGS.Editor.Utils
{
    internal class IniFile
    {
        private string filepath;
        private Dictionary<string, Dictionary<string, string>> data;

        public IniFile(string filepath)
        {
            this.filepath = filepath;
            ReadSettings();
        }

        private void ReadSettings()
        {
            data = new Dictionary<string, Dictionary<string, string>>();

            try
            {
                using (FileStream stream = File.Open(this.filepath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        char[] delimiters = new char[1];
                        delimiters[0] = '=';
                        string section = "";
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            line = line.TrimStart();

                            if (String.IsNullOrEmpty(line))
                            {
                                continue;
                            }

                            if (line.StartsWith("["))
                            {
                                int pos = line.IndexOf(']');

                                if (pos > -1)
                                {
                                    section = line.Substring(1, pos - 1).TrimEnd();
                                }
                            }
                            else
                            {
                                string[] split = line.Split(delimiters, 2);
                                string key = split[0].TrimEnd();

                                if (split.Length > 1 && !key.StartsWith(";") && !String.IsNullOrEmpty(section))
                                {
                                    if (data.ContainsKey(section))
                                    {
                                        if (data[section].ContainsKey(key))
                                        {
                                            data[section].Remove(key);
                                        }
                                    }
                                    else
                                    {
                                        data.Add(section, new Dictionary<string, string>());
                                    }

                                    data[section].Add(key, split[1].TrimStart());
                                }
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                return;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (string section in data.Keys)
            {
                if (data[section].Count == 0)
                {
                    // don't write empty sections
                    continue;
                }

                if (builder.Length > 0)
                {
                    builder.AppendLine("");
                }

                builder.AppendLine(String.Format("[{0}]", section));

                foreach (string key in data[section].Keys)
                {
                    builder.AppendLine(String.Format("{0}={1}", key, data[section][key]));
                }
            }

            return builder.ToString();
        }

        public string GetValue(string section, string key)
        {
            string ret;

            try
            {
                ret = data[section][key];
            }
            catch
            {
                ret = "";
            }

            return ret;
        }

        public bool SetValue(string section, string key, string value)
        {
            bool ret = false;

            if (!String.IsNullOrWhiteSpace(section) && !String.IsNullOrWhiteSpace(key))
            {
                if (!data.ContainsKey(section))
                {
                    data.Add(section, new Dictionary<string, string>());
                }

                data[section][key] = value;
                ret = true;
            }

            return ret;
        }

        public bool DeleteSection(string section)
        {
            bool ret = false;

            if (data.ContainsKey(section))
            {
                ret = data.Remove(section);
            }

            return ret;
        }

        public bool DeleteKey(string section, string key)
        {
            bool ret = false;

            if (data.ContainsKey(section) && data[section].ContainsKey(key))
            {
                ret = data[section].Remove(key);
            }

            return ret;
        }

        public bool IsEmpty()
        {
            return data.Count == 0;
        }

        public void Commit()
        {
            File.WriteAllText(filepath, ToString());
        }
    }
}
