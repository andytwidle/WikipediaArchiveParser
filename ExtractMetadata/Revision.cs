namespace Extract
{
    /// <summary>
    /// Represents the metadata associated with a single revision
    /// </summary>
    internal class Revision
    {
        public const long InitialRevision = -1;
        public const long AnonymousContributor = -1;
        
        public long? RevisionId = null;
        public string? TimeStampUtc = null;
        public long? ContributorId;
        public long? ParentRevisionId = null;
        public bool MinorRevision = false;
        public long? TextBytes = null;
        public string? Comment;
    }
}
