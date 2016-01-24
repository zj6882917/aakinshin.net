using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BibTeXImporter
{
    internal enum EntryType
    {
        Inproceedings, TechReport, Book, Article, PhdThesis
    }

    internal enum Language
    {
        Russian, English
    }

    internal class Entry
    {
        public EntryType Type { get; }
        public string Key { get; }
        public Dictionary<string, string> Properties { get; }

        public Entry(EntryType type, string key, Dictionary<string, string> properties)
        {
            Type = type;
            Key = key;
            Properties = properties;
        }

        public static Entry Read(StreamReader reader)
        {
            if (reader.Peek() == -1)
                return null;
            var firstLine = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(firstLine))
                return null;
            var firstLineSplit = firstLine.Substring(1, firstLine.Length - 2).Split('{');
            var type = (EntryType)Enum.Parse(typeof(EntryType), firstLineSplit[0], true);
            var key = firstLineSplit[1];
            var properties = new Dictionary<string, string>();
            while (true)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line) || line == "}")
                    return new Entry(type, key, properties);
                var equalIndex = line.IndexOf("=", StringComparison.Ordinal);
                if (equalIndex == -1)
                    continue;
                var propertyName = line.Substring(0, equalIndex).Trim();
                var properyValue = line.Substring(equalIndex + 1).
                    Trim(' ', '{', '}', ',').
                    Replace("{\\_}", "_").
                    Replace("{\\%}", "%").
                    Replace("{\\&}", "&");
                properties[propertyName] = properyValue;
            }
        }

        public static List<Entry> ReadAll(string fileName)
        {
            var entries = new List<Entry>();
            using (var reader = new StreamReader(fileName))
            {
                while (true)
                {
                    var entry = Read(reader);
                    if (entry != null)
                        entries.Add(entry);
                    else
                        break;
                }
            }
            return entries;
        }

        public static List<Entry> ReadAll(params string[] fileNames)
        {
            var entries = new List<Entry>();
            foreach (var fileName in fileNames)
                entries.AddRange(ReadAll(fileName));
            return entries;
        }
    }

    internal class EntryAuthor
    {
        public string FirstName { get; }
        public string LastName { get; }

        private EntryAuthor(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public static EntryAuthor[] Parse(string line)
        {
            var split = line.Split(new[] { " and " }, StringSplitOptions.RemoveEmptyEntries);
            var authors = new List<EntryAuthor>();
            foreach (var item in split)
            {
                var names = item.Split(',');
                var lastName = names[0].Trim();
                var firstName = names.Length > 1 ? names[1].Trim() : "";
                if (firstName.Length == 1)
                    firstName = firstName[0] + ".";
                else if (firstName.Length == 2)
                    firstName = firstName[0] + ". " + firstName[1] + ".";
                else if (firstName.Length == 3 && firstName[1] == ' ')
                    firstName = firstName[0] + ". " + firstName[2] + ".";
                else if (firstName.Length == 4 && firstName[2] == ' ')
                    firstName = firstName.Substring(0, 2) + ". " + firstName[3] + ".";
                authors.Add(new EntryAuthor(firstName, lastName));
            }
            return authors.ToArray();
        }
    }

    internal static class EntryExtensions
    {
        public static string GetProperty(this Entry entry, string name) => entry.Properties.ContainsKey(name) ? entry.Properties[name] : "";

        public static int GetYear(this Entry entry) => int.Parse(entry.GetProperty("year"));
        public static string GetTitle(this Entry entry) => entry.GetProperty("title");
        public static string GetPublisher(this Entry entry) => entry.GetProperty("publisher");
        public static string GetAddress(this Entry entry) => entry.GetProperty("address");
        public static string GetJournal(this Entry entry) => entry.GetProperty("journal");
        public static string GetOrganization(this Entry entry) => entry.GetProperty("organization");
        public static string GetPages(this Entry entry) => entry.GetProperty("pages").Replace("--", "–");
        public static string GetVolume(this Entry entry) => entry.GetProperty("volume");
        public static string GetNumber(this Entry entry) => entry.GetProperty("number");
        public static string GetAbstract(this Entry entry) => entry.GetProperty("abstract");
        public static string GetBookTitle(this Entry entry) => entry.GetProperty("booktitle");
        public static string GetIsbn(this Entry entry) => entry.GetProperty("isbn");
        public static string GetDoi(this Entry entry) => entry.GetProperty("doi");
        public static Language GetLanguage(this Entry entry) => entry.GetProperty("language").StartsWith("ru") ? Language.Russian : Language.English;
        public static string[] GetUrls(this Entry entry) => entry.GetProperty("url").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        public static string[] GetKeywords(this Entry entry) => entry.GetProperty("keywords").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        public static EntryAuthor[] GetAuthors(this Entry entry) => EntryAuthor.Parse(entry.GetProperty("author"));

        public static IList<Entry> WithYear(this IEnumerable<Entry> entries, int year) => entries.Where(it => it.GetYear() == year).ToList();
        public static IList<Entry> WithType(this IEnumerable<Entry> entries, EntryType type) => entries.Where(it => it.Type == type).ToList();

        public static string ToHtml(this IEnumerable<EntryAuthor> authors) => "<i>" + string.Join(", ", authors.Select(it => $"{it.FirstName} {it.LastName}")) + "</i>";

        public static string ToHtml(this Entry entry)
        {
            var lang = entry.GetLanguage();
            var builder = new StringBuilder();
            builder.Append(entry.GetAuthors().ToHtml());
            builder.Append(" ");
            builder.Append($"<span title=\"{entry.GetAbstract()}\">");
            builder.Append(Resolve(lang, "“", "«"));
            builder.Append(entry.GetTitle());
            builder.Append(Resolve(lang, "”", "»"));
            builder.Append("</span>");
            builder.Append(" //");
            if (entry.GetBookTitle() != "")
                builder.Append(" " + entry.GetBookTitle() + ".");
            if (entry.GetJournal() != "")
                builder.Append(" " + entry.GetJournal() + ".");
            if (entry.GetPublisher() != "")
                builder.Append($" {Resolve(lang, "Publisher", "Издательство")}: " + entry.GetPublisher() + ".");
            if (entry.GetAddress() != "")
                builder.Append(" " + entry.GetAddress() + ".");
            if (entry.GetOrganization() != "")
                builder.Append(" " + entry.GetOrganization() + ".");
            if (entry.GetIsbn() != "")
                builder.Append(" ISBN:&nbsp;" + entry.GetIsbn() + ".");
            if (entry.GetVolume() != "")
                builder.Append($" {Resolve(lang, "Vol", "Т")}.&nbsp;" + entry.GetVolume() + ".");
            if (entry.GetNumber() != "")
                builder.Append($" {Resolve(lang, "No", "№")}&nbsp;" + entry.GetNumber() + ".");
            if (entry.GetPages() != "")
                builder.Append($" {Resolve(lang, "Pp", "Стр")}.&nbsp;" + entry.GetPages() + ".");
            if (entry.GetDoi() != "")
                builder.Append($" <a href=\"http://dx.doi.org/{entry.GetDoi()}\">DOI:&nbsp;{entry.GetDoi()}</a>");
            var urls = entry.GetUrls();
            bool isVak = entry.GetKeywords().Contains("Vak");
            if (urls.Any() || isVak)
            {
                builder.Append(" //");
                foreach (var url in urls)
                {
                    var title = Resolve(lang, "Link", "Ссылка");
                    if (url.EndsWith(".pdf"))
                        title = "Pdf";
                    else if (url.Contains("ieeexplore.ieee.org"))
                        title = "IEEE";
                    else if (url.Contains("apps.webofknowledge.com"))
                        title = "Web of Science";
                    else if (url.Contains("www.scopus.com"))
                        title = "Scopus";
                    else if (url.Contains("elibrary.ru"))
                        title = Resolve(lang, "RSCI", "РИНЦ");
                    else if (url.Contains("mathnet.ru"))
                        title = "MathNet";
                    else if (url.Contains("link.springer.com"))
                        title = "Springer";
                    else if (url.Contains("www.packtpub.com"))
                        title = "PacktPub";
                    else if (url.Contains("conf.nsc.ru") || url.Contains("uni-bielefeld.de") || url.Contains("cmb.molgen.mpg.de") || url.Contains("sites.google.com"))
                        title = Resolve(lang, "Conference site", "Сайт конференции");
                    else if (url.Contains("authorea"))
                        title = url.Substring(url.IndexOf("authorea.com", StringComparison.Ordinal)).TrimEnd('/');
                    else if (url.Contains("scholar.google.ru"))
                        title = "Google Scholar";
                    builder.AppendLine($" <a href=\"{url}\">[{title}]</a>");
                }
                if (isVak)
                    builder.AppendLine(Resolve(lang, " [VAK]", " [ВАК]"));
            }
            return builder.ToString();
        }

        public static string ToHtml(this IList<Entry> entries, Language lang = Language.English)
        {
            var builder = new StringBuilder();
            var years = entries.Select(it => it.GetYear()).Distinct().OrderByDescending(it => it);
            foreach (var year in years)
            {
                builder.AppendLine($"<h4>{year}</h4>");
                var localEntries = entries.WithYear(year);
                builder.Append(localEntries.WithType(EntryType.PhdThesis).ToHtmlSection(Resolve(lang, "Phd thesis", "Диссертационные работы")));
                builder.Append(localEntries.WithType(EntryType.Book).ToHtmlSection(Resolve(lang, "Books", "Книги")));
                builder.Append(localEntries.WithType(EntryType.Article).ToHtmlSection(Resolve(lang, "Articles", "Статьи")));
                builder.Append(localEntries.WithType(EntryType.Inproceedings).ToHtmlSection(Resolve(lang, "Inproceedings", "Тезисы")));
                builder.Append(localEntries.WithType(EntryType.TechReport).ToHtmlSection(Resolve(lang, "Technical reports", "Технические отчёты")));
            }
            return builder.ToString();
        }

        private static string ToHtmlSection(this IList<Entry> entries, string title)
        {
            if (!entries.Any())
                return "";
            var builder = new StringBuilder();
            builder.AppendLine($"  <h5>{title}</h5>");
            builder.AppendLine($"  <ul>");
            foreach (var entry in entries)
                builder.AppendLine($"  <li>{entry.ToHtml()}</li>");
            builder.AppendLine($"  </ul>");
            return builder.ToString();
        }

        private static string Resolve(Language lang, string en, string ru) => lang == Language.English ? en : ru;
    }

    public class Program
    {
        private const string MetaEnconding = "<meta charset=\"utf-8\">\n";

        public static void Main()
        {
            File.WriteAllText("bib-en.html", MetaEnconding + Entry.ReadAll("Akinshin.En.bib", "Akinshin.InRussian.bib", "Akinshin.Translation.bib").ToHtml());
            File.WriteAllText("bib-ru.html", MetaEnconding + Entry.ReadAll("Akinshin.En.bib", "Akinshin.Ru.bib", "Akinshin.Translation.bib").ToHtml(Language.Russian));
            Console.WriteLine("DONE");
        }
    }
}
