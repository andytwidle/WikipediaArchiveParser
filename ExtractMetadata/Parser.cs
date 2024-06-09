using System.Text.RegularExpressions;

namespace Extract
{
    /// <summary>
    /// Program for extracting page and revision level metadata from Wiki dump pages.  
    /// Compatible with archive dump from Febrauary 2024.
    /// </summary>
    public class Parser
    {
        // Regexes for parsing Wiki dump pages
        readonly Regex PageStartExpr = new("^\\s*<page>", RegexOptions.Compiled);
        readonly Regex PageEndExpr = new("^\\s*</page>", RegexOptions.Compiled);
        readonly Regex RevisionStartExpr = new("^\\s*<revision>", RegexOptions.Compiled);
        readonly Regex RevisionEndExpr = new("^\\s*</revision>", RegexOptions.Compiled);
        readonly Regex ContributorStartExpr = new("^\\s*<contributor>", RegexOptions.Compiled);
        readonly Regex ContributorEndExpr = new("^\\s*</contributor>", RegexOptions.Compiled);

        readonly Regex TimeStampExpr = new("^\\s*<timestamp>(.+)</timestamp>", RegexOptions.Compiled);
        readonly Regex ParentRevisionIdExpr = new("^\\s*<parentid>(\\d+)</parentid>", RegexOptions.Compiled);
        readonly Regex IdExpr = new("^\\s*<id>(\\d+)</id>", RegexOptions.Compiled);
        
        readonly Regex NameSpaceExpr = new("^\\s*<ns>(\\d+)</ns>", RegexOptions.Compiled);
        readonly Regex MinorExpr = new("^\\s*<minor", RegexOptions.Compiled);
        readonly Regex TextExpr = new("^\\s*<text\\s*bytes\\s*=\"(\\d+)\"", RegexOptions.Compiled);
        readonly Regex CommentExpr = new("^\\s*<comment>(.*)</comment>", RegexOptions.Compiled);
        readonly Regex TitleExpr = new("^\\s*<title>(.*)</title>", RegexOptions.Compiled);

        /// <summary>
        /// States for the parsing state machine
        /// </summary>
        public enum ParseState
        {
            SearchingForPage,
            InPage,
            InRevision,
            InContributor,
        };

        /// <summary>
        /// Parses a single wiki dump XML file containing multiple pages. 
        /// </summary>
        /// <param name="inputFile">input XML File to parse</param>
        /// <param name="outRevisionsFile">output file for revision metedata</param>
        /// <param name="outPagesFile">output file for page metadata</param>
        public void Run(string inputFile, string outRevisionsFile, string outPagesFile)
        {
            long lineNumber = 0;

            var pages = new List<Page>();
            string? line;
            
            ParseState state = ParseState.SearchingForPage;
            using var reader = new StreamReader(inputFile);
            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;
                switch (state)
                {
                    case ParseState.SearchingForPage:
                        state = ParseForPage(pages, line, state);
                        break;
                    case ParseState.InPage:
                        state = ParsePageLine(pages.Last(), line, state);
                        break;
                    case ParseState.InRevision:
                        state = ParseRevisionLine(pages.Last(), line, state);
                        break;
                    case ParseState.InContributor:
                        state = ParseInContributorLine(pages.Last(), line, state);
                        break;
                }
            }

            DumpPages(outPagesFile, pages);
            DumpRevisions(outRevisionsFile, pages);
            Console.WriteLine(pages.Select(pg => pg.Count()).Sum());
            Console.WriteLine(pages.Count);

        }
        
        /// <summary>
        /// Dumps revision level metadata to outFile
        /// </summary>
        private static void DumpRevisions(string outFile, List<Page> pages)
        {
            using var writer = new StreamWriter(outFile);
            foreach (Page page in pages)
            {
                writer.Write(page.RevisionLines());
            }

        }

        /// <summary>
        /// Dumps page level metadata to outFile
        /// </summary>
        private static void DumpPages(string outFile, List<Page> pages)
        {
            using var writer = new StreamWriter(outFile);
            foreach (Page page in pages)
            {
                writer.Write(page.PageLines());
            }
            
        }

        /// <summary>
        /// Executes parse logic for lines in Contributor elements
        /// </summary>
        /// <returns>state || InRevision</returns>
        private ParseState ParseInContributorLine(Page page, string line, ParseState state)
        {
            var revision = page.Last();
            if (IdExpr.IsMatch(line))
            {
                revision.ContributorId = long.Parse(IdExpr.Match(line).Result("$1"));
            }
            else if (ContributorEndExpr.IsMatch(line))
            {
                revision.ContributorId ??= Revision.AnonymousContributor;
                state = ParseState.InRevision;
            }

            return state;
        }

        /// <summary>
        /// Executes parse logic for liens in Revision elements
        /// </summary>
        /// <returns>state || InPage</returns>
        private ParseState ParseRevisionLine(Page page, string line, ParseState state)
        {
            var revision = page.Last();
            if (RevisionEndExpr.IsMatch(line))
            {
                revision.ParentRevisionId ??= Revision.InitialRevision;
                state = ParseState.InPage;
            }
            else if (MinorExpr.IsMatch(line))
            {
                revision.MinorRevision = true;
            }
            else if (IdExpr.IsMatch(line))
            {
                revision.RevisionId = long.Parse(IdExpr.Match(line).Result("$1"));
            }
            else if (ParentRevisionIdExpr.IsMatch(line))
            {
                revision.ParentRevisionId = long.Parse(ParentRevisionIdExpr.Match(line).Result("$1"));
            }
            else if (TimeStampExpr.IsMatch(line))
            {
                revision.TimeStampUtc = TimeStampExpr.Match(line).Result("$1");
            }
            else if (ContributorStartExpr.IsMatch(line))
            {
                state = ParseState.InContributor;
            }
            else if (TextExpr.IsMatch(line))
            {
                revision.TextBytes = long.Parse(TextExpr.Match(line).Result("$1"));
            }
            else if (CommentExpr.IsMatch(line))
            {
                revision.Comment = CommentExpr.Match(line).Result("$1");
            }

            return state;
        }

        /// <summary>
        /// Executes parse logic for lines outside of Page elements
        /// </summary>
        /// <returns>state || InPage</returns>
        private ParseState ParseForPage(List<Page> pages, string line, ParseState state)
        {
            if (PageStartExpr.IsMatch(line))
            {
                pages.Add(new Page());
                state = ParseState.InPage;
            }
            return state;
        }

        /// <summary>
        /// Executes parse logic for lines inside of Page elements
        /// </summary>
        /// <returns>state || InRevision || SearchingForPage</returns>
        private ParseState ParsePageLine(Page page, string line, ParseState state)
        {
            if (IdExpr.IsMatch(line))
            {
                page.Id = long.Parse(IdExpr.Match(line).Result("$1"));
            }
            else if (TitleExpr.IsMatch(line))
            {
                page.Title = TitleExpr.Match(line).Result("$1");
            }
            else if (RevisionStartExpr.IsMatch(line))
            {
                page.Add(new Revision());
                state = ParseState.InRevision;
            }
            else if (PageEndExpr.IsMatch(line))
            {
                state = ParseState.SearchingForPage;
            }
            else if (NameSpaceExpr.IsMatch(line))
            {
                page.NameSpaceId = long.Parse(NameSpaceExpr.Match(line).Result("$1"));
            }

            return state;
        }
    }
}
