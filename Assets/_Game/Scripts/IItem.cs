public interface IItem
{
    string ItemName { get; }
    void Activate();
    void Deactivate();
}