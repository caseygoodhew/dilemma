namespace Dilemma.Web.ViewModels
{
    public class ModerationHistoryViewModel<T> where T : class
    {
        public ModerationHistoryViewModel(T history, SidebarViewModel sidebar)
        {
            History = history;
            Sidebar = sidebar;
        }
        
        public T History { get; set; }

        public SidebarViewModel Sidebar { get; set; }
    }
}