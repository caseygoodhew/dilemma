namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// Development user view model.
    /// </summary>
    public class DevelopmentUserViewModel
    {
        /// <summary>
        /// Gets or sets the development name of the user.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the user id of the user.
        /// </summary>
        public int? UserId { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this user is the user that is currently logged in.
        /// </summary>
        public bool IsCurrent { get; set; }
    }
}