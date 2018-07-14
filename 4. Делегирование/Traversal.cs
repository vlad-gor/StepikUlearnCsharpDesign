// Вставьте сюда финальное содержимое файла Traversal.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.TreeTraversal
{
	public static class Traversal
	{
		public static IEnumerable<int> GetBinaryTreeValues(BinaryTree<int> tree)
		{
			return Travel<BinaryTree<int>, int>
			(
				tree,
				t => new BinaryTree<int>[] { t.Left, t.Right }.Where(e => e != null),
				t => t != null,
				t => t.Value
			);
			throw new NotImplementedException();
		}

		public static IEnumerable<Job> GetEndJobs(Job tree)
		{
			return Travel
			(
				tree,
				j => j.Subjobs,
				j => j.Subjobs.Count == 0,
				j => j
			);
		}

		public static IEnumerable<Product> GetProducts(ProductCategory tree)
		{
			return Travel<ProductCategory, IEnumerable<Product>>
			(
				tree,
				j => j.Categories,
				j => j.Products.Count > 0,
				j =>  j.Products
			).SelectMany(t=>t);
		}

		public static IEnumerable<Tout> Travel<Tin, Tout>(Tin root, Func<Tin, IEnumerable<Tin>> childrenSelector, Func<Tin, bool> filter, Func<Tin, Tout> resultSelector)
		{
			if (filter(root))
				yield return resultSelector(root);

			var b = childrenSelector(root);

			foreach (var item in childrenSelector(root))
			{
				foreach (var item2 in Travel(item, childrenSelector, filter, resultSelector))
				{
				 	yield return item2;
				}
			}
		}
	}
}