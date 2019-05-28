using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Schematics.Core
{
    public interface IMetadataProvider
    {
        IEnumerable<string> GetAvailableEntities();
        IEntity GetEntity(string name);
    }
    
    public interface IFeature  {  }
    
    
    public interface IQuery
    {
        IEntity Entity { get; }
    }
    public interface IQueryFeature : IFeature
    {
        Task<IEnumerable<Instance>> QueryAsync(IQuery query, CancellationToken token);
    }
}