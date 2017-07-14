﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;

using HtmlAgilityPack;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HtmlSpeedPack.Test
{
    [TestClass]
    public class LargeDocumentTest
    {
        private readonly Stream stream;

        public LargeDocumentTest()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            stream = executingAssembly.GetManifestResourceStream("HtmlSpeedPack.Test.en.wikipedia.org_wiki_List_of_Australian_treaties.html");
        }

        [TestMethod]
        public void ExtractLinksFromWikipediaListOfAustralianTreaties()
        {
            stream.Seek(0, SeekOrigin.Begin);

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(stream);
            var htmlAgilityPackLinks = new List<string>();

            foreach (var node in htmlDocument.DocumentNode.Descendants())
            {
                if (node.NodeType == HtmlAgilityPack.HtmlNodeType.Element && node.Name == "a")
                {
                    var hrefAttributeValue = node.Attributes["href"];
                    if (hrefAttributeValue != null)
                    {
                        htmlAgilityPackLinks.Add(HttpUtility.HtmlDecode(hrefAttributeValue.Value));
                    }
                }
            }

            stream.Seek(0, SeekOrigin.Begin);

            var htmlReader = new HtmlReader(new StreamReader(stream));
            var htmlSpeedPackLinks = new List<string>();

            while (htmlReader.Read())
            {
                if (htmlReader.NodeType == HtmlNodeType.Tag && htmlReader.Name == "a")
                {
                    var hrefAttributeValue = htmlReader.GetAttribute("href");
                    if (hrefAttributeValue != null)
                    {
                        htmlSpeedPackLinks.Add(hrefAttributeValue);
                    }
                }
            }

            CollectionAssert.AreEqual(htmlAgilityPackLinks, htmlSpeedPackLinks);
        }

        [TestMethod]
        public void ExtractTextFromWikipediaListOfAustralianTreaties()
        {
            stream.Seek(0, SeekOrigin.Begin);

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(stream);
            var htmlAgilityPackTexts = new List<string>();

            foreach (var node in htmlDocument.DocumentNode.Descendants())
            {
                if (node.NodeType == HtmlAgilityPack.HtmlNodeType.Text && node.InnerText != "</form>" && node.InnerText != "")
                {
                    htmlAgilityPackTexts.Add(HttpUtility.HtmlDecode(node.InnerText));
                }
            }

            stream.Seek(0, SeekOrigin.Begin);

            var htmlReader = new HtmlReader(new StreamReader(stream));
            var htmlSpeedPackTexts = new List<string>();

            while (htmlReader.Read())
            {
                if (htmlReader.NodeType == HtmlNodeType.Text)
                {
                    htmlSpeedPackTexts.Add(htmlReader.Text);
                }
            }

            CollectionAssert.AreEqual(htmlAgilityPackTexts, htmlSpeedPackTexts);
        }
    }
}