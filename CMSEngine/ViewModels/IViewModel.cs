namespace CMSEngine.ViewModels
{
    public interface IViewModel
    {
        /// <summary>
        /// Used to recognize when the ViewModel is being passed to a modal or not
        /// </summary>
        bool IsDialog { get; set; }
    }
}
