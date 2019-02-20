using odec.Framework.Generic;

namespace odec.Server.Model.Menu
{
    public class MenuItem:Glossary<int>
    {
        public int? ParentId { get; set; }
        public MenuItem Parent { get; set; }
    }
}
