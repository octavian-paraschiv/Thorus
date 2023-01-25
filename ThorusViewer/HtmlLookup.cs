using Majestic12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThorusViewer
{
    public class HtmlLookup : IDisposable
    {
        HTMLparser _parser = null;

        public HtmlLookup(string doc)
        {
            if (string.IsNullOrEmpty(doc) == false)
            {
                _parser = new HTMLparser();
                _parser.Init(doc);
            }
        }

        public void Dispose()
        {
            if (_parser != null)
            {
                _parser.Close();
                _parser.Dispose();
                _parser = null;
            }
        }

        public List<string> GetElements(string tagType, string param, string lookupText, string optional = null)
        {
            List<string> elements = new List<string>();

            if (_parser != null)
            {
                _parser.Reset();

                while (true)
                {
                    string elem = GetNextElement(tagType, param, lookupText, optional);
                    if (string.IsNullOrEmpty(elem))
                        break;

                    elements.Add(elem);
                }
            }

            return elements;
        }

        private string GetNextElement(string tagType, string param, string lookupText, string optional = null)
        {
            if (_parser != null)
            {
                HTMLchunk chunk = null;

                int i = 0;

                while ((chunk = _parser.ParseNext()) != null)
                {
                    if (chunk.sTag != tagType)
                        continue;
                    if (chunk.oType != HTMLchunkType.OpenTag)
                        continue;
                    if (chunk.oParams == null || chunk.oParams.Count < 1)
                        continue;

                    string elem = chunk.oParams[param]?.ToString();
                    if (string.IsNullOrEmpty(elem))
                        continue;

                    if (elem.ToUpperInvariant().Contains(lookupText.ToUpperInvariant()))
                        return elem;
                }
            }

            return string.Empty;
        }
    }
}