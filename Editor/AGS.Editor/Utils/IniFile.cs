﻿using System;
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
                builder.AppendLine(String.Format("[{0}]", section));

                foreach (string key in data[section].Keys)
                {
                    builder.AppendLine(String.Format("{0} = {1}", key, data[section][key]));
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
    }
}