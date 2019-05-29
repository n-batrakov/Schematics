using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Schematics.Core
{
    public interface IQuery
    {
        IEntity Entity { get; }
    }
    
    public interface IMetadataFeature : IFeature
    {
        IEnumerable<string> GetAvailableEntities();
        IEntity GetEntity(string name);
    }
    
    public interface IQueryFeature : IFeature
    {
        Task<IEnumerable<Instance>> QueryAsync(IQuery query, CancellationToken token);
    }
}