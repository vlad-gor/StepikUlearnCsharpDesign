using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public readonly string ProductName;
        public readonly MessageType Type;
        public readonly MessageTopic Topic;

        public Category(string name, MessageType type, MessageTopic topic)
        {
            ProductName = name;
            Type = type;
            Topic = topic;
        }

        public static bool operator <(Category first, Category second) => first.CompareTo(second) < 0;
        public static bool operator <=(Category first, Category second) => first.CompareTo(second) <= 0;
        public static bool operator >(Category first, Category second) => first.CompareTo(second) > 0;
        public static bool operator >=(Category first, Category second) => first.CompareTo(second) >= 0;

        public int CompareTo(object obj)
        {
            if (!(obj is Category)) return 1;   // ?? 
            var categoryObj = (Category)obj;
            var nameComparison =
                (string.Compare(ProductName, categoryObj.ProductName, StringComparison.InvariantCulture));
            var typeComparison = Type.CompareTo(categoryObj.Type);
            var topicComparison = Topic.CompareTo(categoryObj.Topic);

            if (nameComparison != 0) return nameComparison;
            if (typeComparison != 0) return typeComparison;
            if (topicComparison != 0) return topicComparison;
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Category) || obj.GetHashCode() != this.GetHashCode()) return false;
            var categoryObj = (Category)obj;
            return ProductName == categoryObj.ProductName && Type == categoryObj.Type && Topic == categoryObj.Topic;
        }

        public override int GetHashCode()
        {
            if (ProductName == null) return base.GetHashCode();
            var hash = 13;
            hash = (hash * 7) + ProductName.GetHashCode();
            hash = (hash * 7) + Topic.GetHashCode();
            hash = (hash * 7) + Type.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return ProductName + "." + Type + "." + Topic;
        }
    }
}