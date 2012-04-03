using OwlAPI.Structure.Meta;

namespace OwlAPI.Structure
{
    public interface IEdge : IDBObject
    {
        #region Properties

        IGraph Graph { get; set; }

        IVertex Source { get; }

        IVertex Target { get; }

        #endregion
    }
}
