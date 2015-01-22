using System;

namespace Dilemma.Data.EntityFramework
{
    /// <summary>
    /// Thrown when two reference type variables do not correspond to the same object.
    /// </summary>
    [Serializable]
    public class AreNotSameException : Exception
    {
    }
}
