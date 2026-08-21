namespace AiDispatcher.Domain.Entities;

public class PromptTemplate
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Template { get; private set; }
    public int VersionNumber { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public PromptTemplate() { }

    public PromptTemplate(string name, string template)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(template))
            throw new ArgumentException("Template cannot be empty", nameof(template));

        Id = Guid.NewGuid();
        Name = name;
        Template = template;
        VersionNumber = 1;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public string RenderTemplate(Dictionary<string, string> variables)
    {
        var result = Template;
        foreach (var variable in variables)
        {
            result = result.Replace($"{{{{{variable.Key}}}}}", variable.Value);
        }
        return result;
    }

    public void UpdateTemplate(string newTemplate)
    {
        if (string.IsNullOrWhiteSpace(newTemplate))
            throw new ArgumentException("Template cannot be empty", nameof(newTemplate));

        Template = newTemplate;
        VersionNumber++;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}