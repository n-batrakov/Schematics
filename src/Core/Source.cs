using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Schematics.Core
{
    public static class FeatureCollectionExtensions
    {
        public static TFeature GetFeature<TFeature>(this IFeatureCollection features) where TFeature : IFeature
        {
            return (TFeature)features.GetFeature(typeof(TFeature));
        }

        public static bool TryGetFeature<TFeature>(this IFeatureCollection features, out TFeature feature) where TFeature : IFeature
        {
            var key = typeof(TFeature);
            if (features.HasFeature(key))
            {
                feature = (TFeature) features.GetFeature(key);
                return true;
            }
            else
            {
                feature = default;
                return false;
            }
        }
    }

    public class FeatureCollection : IFeatureCollection
    {
        private IReadOnlyCollection<IFeature> Source { get; }

        public FeatureCollection(IEnumerable<IFeature> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            
            Source = source.ToArray();
            EnsureNoDuplicateFeatures(Source);
        }

        public object GetFeature(Type featureType)
        {
            if (featureType == null) throw new ArgumentNullException(nameof(featureType));
            
            var feature = Source.FirstOrDefault(featureType.IsInstanceOfType);

            if (feature == null)
            {
                throw new FeatureNotFoundException(featureType);
            }
            else
            {
                return feature;
            }
        }

        public bool HasFeature(Type featureType)
        {
            if (featureType == null) throw new ArgumentNullException(nameof(featureType));
            
            return Source.Any(featureType.IsInstanceOfType);
        }

        private static void EnsureNoDuplicateFeatures(IReadOnlyCollection<IFeature> features)
        {
            var featureTypes = features.Select(x => x.GetType()).Distinct().ToArray();

            if (featureTypes.Length != features.Count)
            {
                throw new DuplicateFeaturesException();
            }
        }

        public IEnumerator<IFeature> GetEnumerator() => Source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => Source.Count;
    }
}