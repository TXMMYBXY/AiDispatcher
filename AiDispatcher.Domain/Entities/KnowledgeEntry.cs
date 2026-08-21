namespace AiDispatcher.Domain.Entities;

public class KnowledgeEntry
{
    public Guid Id { get; private set; }
    public byte[] Content { get; private set; }
    public string SourceLink { get; private set; }
    public double? RelevanceScore { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public KnowledgeEntry() { }

    public KnowledgeEntry(byte[] content, string sourceLink)
    {
        if (content == null || content.Length == 0)
            throw new ArgumentException("Content cannot be empty", nameof(content));
        if (string.IsNullOrWhiteSpace(sourceLink))
            throw new ArgumentException("SourceLink cannot be empty", nameof(sourceLink));

        Id = Guid.NewGuid();
        Content = content;
        SourceLink = sourceLink;
        CreatedAt = DateTime.UtcNow;
    }

    public void CalculateRelevance(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            RelevanceScore = 0;
            return;
        }

        var queryWords = query.ToLower().Split([' ', '\t', '\n'], StringSplitOptions.RemoveEmptyEntries);
        var sourceLower = SourceLink.ToLower();
        
        int matches = queryWords.Count(word => sourceLower.Contains(word));
        RelevanceScore = queryWords.Length > 0 ? (double)matches / queryWords.Length : 0;
    }

    public bool IsRelevant(double threshold = 0.3) => RelevanceScore.HasValue && RelevanceScore.Value >= threshold;
}