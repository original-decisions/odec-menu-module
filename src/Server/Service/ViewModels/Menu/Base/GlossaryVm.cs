namespace odec.Menu.ViewModels
{
    public class GlossaryVm<TKey>
    {
        public TKey Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
