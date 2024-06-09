using System.Text;

namespace Extract
{
    /// <summary>
    /// Represents the metadata associated with a single Page.
    /// Includes all Revisions associated with this Page.
    /// </summary>
    internal class Page : List<Revision>
    {
        public long? Id { get; set; }
        public string? Title { get; set; }
        public long? NameSpaceId { get; set; }
        
        public string PageLines()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{Id}\t{NameSpaceId}\t{Title}");
            return sb.ToString();
        }

        public string RevisionLines()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var revision in this)
            {
                // Strip tabs for easier ETL
                var commentWithoutTabs = revision.Comment?.Replace("\t", " ");
                sb.AppendLine($"{revision.RevisionId}\t{Id}\t{revision.ParentRevisionId}\t{revision.TimeStampUtc}\t{revision.ContributorId}\t{revision.MinorRevision}\t{revision.TextBytes}\t{commentWithoutTabs}");   
            }
            return sb.ToString();
        }
    }
}
