namespace Backend.Modules.Users.Domain.Entities;
public class Role
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; set; }

    public Role(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void SetName(string name) => Name = name;
}