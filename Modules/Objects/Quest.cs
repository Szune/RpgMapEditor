namespace RpgMapEditor.Modules.Objects
{
    public class Quest
    {
        public string Name;
        public string Text;
        public bool Accepted;
        public bool Completed;
        public int Id;

        public Quest()
        {
            Id = -1;
        }

        public Quest(int id, string name, string text, bool accepted = true, bool completed = false)
        {
            Id = id;
            Name = name;
            Text = text;
            Accepted = accepted;
            Completed = completed;
        }
        
        public void Complete()
        {
            Completed = true;
        }

        public bool IsCompleted()
        {
            return Completed;
        }

        public void Accept()
        {
            Accepted = true;
        }

        public bool IsAccepted()
        {
            return Accepted;
        }
    }
}
