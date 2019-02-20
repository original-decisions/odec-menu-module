using odec.Framework.Generic;

namespace odec.Server.Model.Menu.Abstractions.Filters
{
    public abstract class MenuFilterBase<TMenuItemGroup>: FilterBase
    {
        protected MenuFilterBase()
        {
            InitHierarchy = true;
        }

        public TMenuItemGroup MenuItemGroup { get; set; }
        public bool InitHierarchy { get; set; }
    }
}
