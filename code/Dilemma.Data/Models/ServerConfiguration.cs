using System.Diagnostics.CodeAnalysis;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    /// <summary>
    /// The server configuration model.
    /// </summary>
    public class ServerConfiguration
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public ServerRole ServerRole { get; set; }
    }
}
