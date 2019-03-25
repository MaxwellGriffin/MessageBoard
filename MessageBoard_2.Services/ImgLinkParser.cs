using System;
using System.Collections.Generic;

namespace MessageBoard_2.Services
{
    public class ImgLinkParser
    {
        int state;
        List<string> tokz = new List<string>();

        public string Parse(string input, bool first = false)
        {
            var tok = "";
            for (int i = 0; i < input.Length; i++)
            {
                tok += input[i];
                switch (state)
                {
                    case 0:
                        if (tok.EndsWith("[") && !(i < input.Length-1 && (input[i+1]=='[')) && !(i > 0 && (input[i-1]=='[')))
                        {
                            tokz.Add(tok.Substring(0, tok.Length - 1));
                            state = 1;
                            tok = "";
                        }
                        break;
                    case 1:
                        if (tok == "img")
                        {
                            state = 2;
                            i++;
                            tok = "";
                        }
                        else if (tok == "a")
                        {
                            state = 3;
                            i++;
                            tok = "";
                        }
                        else if (tok == "style]" && first)
                        {
                            state = 5;
                            i++;
                            tokz.Add("<style>");
                            tok = "";
                        }
                        break;
                    case 2: // Image Parsing
                        if (tok.EndsWith("]"))
                        {
                            var url = tok.Substring(0, tok.Length - 1);
                            var html = $"<img src=\"{url}\" style=\"max-width:500px;\">";
                            tokz.Add(html);
                            tok = "";
                            state = 0;
                        }
                        break;
                    case 3: // Link parsing
                        if (tok.EndsWith("]"))
                        {
                            var url = tok.Substring(0, tok.Length - 1);
                            if (!url.StartsWith("http")) url = "http://" + url;
                            var html = $"<a href=\"{url}\">";
                            tokz.Add(html);
                            tok = "";
                            state = 4;
                        }
                        break;
                    case 4:
                        if (tok.EndsWith("[/a]"))
                        {
                            state = 0;
                            var text = tok.Substring(0, tok.Length - 4);
                            tokz.Add(text + "</a>");
                            tok = "";
                        }
                        break;
                    case 5:
                        if (tok.EndsWith("[/style]"))
                        {
                            var text = tok.Substring(0, tok.Length - ("[/style]").Length);
                            tokz.Add(text + "</style");
                            tok = "";
                            state = 0;
                        }
                        break;
                }
            }
            if (tok != "") tokz.Add(tok);
            tok = "";
            var str = "";
            foreach (string tokk in tokz) str += tokk;
            tokz.Clear();
            return str.Replace("[[", "[");
        }
    }
}