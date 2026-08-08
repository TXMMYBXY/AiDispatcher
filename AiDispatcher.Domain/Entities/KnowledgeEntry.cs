namespace AiDispatcher.Domain.Entities;

public class KnowledgeEntry
{
    public int Id { get; set; }
    public byte[] Content { get; set; }
    public string SourceLink { get; set; }

    public void CalculateRelevance(string query)
    {
        
    }
}