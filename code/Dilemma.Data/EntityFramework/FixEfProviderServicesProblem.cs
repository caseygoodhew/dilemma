namespace Dilemma.Data.EntityFramework
{
    /// <summary>
    /// This class forces the project to find the <see cref="System.Data.Entity.SqlServer.SqlProviderServices.Instance"/> 
    /// </summary>
    internal static class FixEfProviderServicesProblem
    {
        private static void Fix()
        {
            // The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            // for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            // Make sure the provider assembly is available to the running application. 
            // See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}
