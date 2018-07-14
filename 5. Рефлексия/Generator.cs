// Вставьте сюда финальное содержимое файла Generator.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.Randomness
{
    public class Generator<T> where T : new()
    {
        public Dictionary<string, IContinousDistribution> distributions = new Dictionary<string, IContinousDistribution>();
        public PropertyInfo[] properties;

        public Generator()
        {
            properties = typeof(T).GetProperties();
        }

        public T Generate(Random rnd)
        {
            T sourceObject = new T();
            foreach (var property in properties)
            {
                if (distributions.ContainsKey(property.Name))
                {
                   property.SetValue(sourceObject, distributions[property.Name].Generate(rnd));
                }
                else
                {
                    var attribute = property.GetCustomAttributes(false).OfType<FromDistributionAttribute>().FirstOrDefault();

                    if (attribute != null)
                    {
                        property.SetValue(sourceObject, attribute.Distribution.Generate(rnd));
                    }
                }
            }
            return sourceObject;
        }
    }

    public static class GeneratorExtensions
    {
        public static Config<T> For<T>(this Generator<T> generator, Expression<Func<T, double>> handler) where T: new()
        {   
            if (handler.Body is MemberExpression a)
            {
                var changingProperty = a.Member.Name;
                var propertyNames = generator.properties.Where(property => property.Name == changingProperty).Select(property => property.Name);

                if (propertyNames.Count() > 0)
                   return new Config<T>(propertyNames, generator);
                else throw new ArgumentException();
            }
           throw new ArgumentException();
        }

        public static Generator<T> Set<T>(this Config<T> config, IContinousDistribution distribution) where T: new()
        {
            foreach (var property in config.propertyNames)
            {
                if (config.generator.distributions.ContainsKey(property))
                    config.generator.distributions[property] = distribution;
                else config.generator.distributions.Add(property, distribution);
            }
            return config.generator;
        }

        public class Config<T> where T: new()
        {
            public IEnumerable<string> propertyNames;
            public Generator<T> generator;

            public Config(IEnumerable<string> propertyNames, Generator<T> generator)
            {
                this.propertyNames = propertyNames;
                this.generator = generator;
            }
        }
    }

    public class FromDistributionAttribute : Attribute
    {
        public IContinousDistribution Distribution { get; set; }
        public FromDistributionAttribute(Type type, params object[] parameters)
        {
            if (parameters.Length > 2) throw new ArgumentException($"{type} can't have wrong arguments.");
           Distribution = (IContinousDistribution)Activator.CreateInstance(type, parameters);
        }
    }
}