using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace AU.Com.BushLife.Utils
{
    [TestFixture]
    public class LanguateUtilsTest
    {
        [DebuggerDisplay("{Name}:{QuantityRequired}")]
        public class Item
        {
            public string Name { get; set; }
            public int QuantityRequired { get; set; }
            public override bool Equals(object obj)
            {
                var rhs = obj as Item;
                if (rhs == null)
                    return false;
                return this.Name == rhs.Name
                    && this.QuantityRequired == rhs.QuantityRequired;
            }
            public override int GetHashCode()
            {
                return LanguageUtils.RSHash(Name, QuantityRequired);
            }
        }

        private IEnumerable<object[]> PartitionFunctionData
        {
            get
            {
                #region Test 1: Second item too large to fit
                yield return new object[]
                {
                    new List<Item>()
                    {
                        new Item() { Name = "1", QuantityRequired = 20 },
                        new Item() { Name = "2", QuantityRequired = 40 },
                    },
                    #region Expected Results
                    new List<List<Item>>()
                    {
                        new List<Item>() { 
                        new Item() { Name = "1", QuantityRequired = 20 } },
                        new List<Item>() {new Item() { Name = "Split", QuantityRequired = 20 } },
                        new List<Item>() {new Item() { Name = "2", QuantityRequired = 20 } },
                    }
                    #endregion
                };
                #endregion
                #region Test 2: Single item fits
                yield return new object[]
                {
                    new List<Item>()
                    {
                        new Item() { Name = "1", QuantityRequired = 20 },
                    },
                    #region Expected Results
                    new List<List<Item>>()
                    {
                        new List<Item>() {new Item() { Name = "1", QuantityRequired = 20 } },
                    }
                    #endregion
                };
                #endregion
                #region Test 3: Single item fits easily
                yield return new object[]
                {
                    new List<Item>()
                    {
                        new Item() { Name = "1", QuantityRequired = 19 },
                    },
                    #region Expected Results
                    new List<List<Item>>()
                    {
                        new List<Item>() {new Item() { Name = "1", QuantityRequired = 19 } },
                    }
                    #endregion
                };
                #endregion
                #region Test 4: Single item does not fit
                yield return new object[]
                {
                    new List<Item>()
                    {
                        new Item() { Name = "1", QuantityRequired = 22 },
                    },
                    #region Expected Results
                    new List<List<Item>>()
                    {
                        new List<Item>() {new Item() { Name = "Split", QuantityRequired = 20 } },
                        new List<Item>() {new Item() { Name = "1", QuantityRequired = 2 } },
                    }
                    #endregion
                };
                #endregion
                #region Test 5: Multiple items in single group
                yield return new object[]
                {
                    new List<Item>()
                    {
                        new Item() { Name = "1", QuantityRequired = 10 },
                        new Item() { Name = "2", QuantityRequired = 10 },
                    },
                    #region Expected Results
                    new List<List<Item>>()
                    {
                        new List<Item>() {
                            new Item() { Name = "1", QuantityRequired = 10 },
                            new Item() { Name = "2", QuantityRequired = 10 } 
                        },
                    }
                    #endregion
                };
                #endregion
                #region Test 6: Multiple items in second group
                yield return new object[]
                {
                    new List<Item>()
                    {
                        new Item() { Name = "1", QuantityRequired = 30 },
                        new Item() { Name = "2", QuantityRequired = 10 },
                    },
                    #region Expected Results
                    new List<List<Item>>()
                    {
                        new List<Item>() {new Item() { Name = "Split", QuantityRequired = 20 } },
                        new List<Item>() {
                            new Item() { Name = "1", QuantityRequired = 10 },
                            new Item() { Name = "2", QuantityRequired = 10 },
                        },
                    }
                    #endregion
                };
                #endregion
                #region Test 7: Multiple items in both groups
                yield return new object[]
                {
                    new List<Item>()
                    {
                        new Item() { Name = "1", QuantityRequired = 10 },
                        new Item() { Name = "2", QuantityRequired = 20 },
                        new Item() { Name = "3", QuantityRequired = 20 },
                    },
                    #region Expected Results
                    new List<List<Item>>()
                    {
                        new List<Item>() {
                            new Item() { Name = "1", QuantityRequired = 10 },
                            new Item() { Name = "Split", QuantityRequired = 10 },
                        },
                        new List<Item>() {
                            new Item() { Name = "2", QuantityRequired = 10 },
                            new Item() { Name = "Split", QuantityRequired = 10 },
                        },
                        new List<Item>() {
                            new Item() { Name = "3", QuantityRequired = 10 },
                        },
                    }
                    #endregion
                };
                #endregion
            }
        }

        [Test]
        [Factory("PartitionFunctionData")]
        public void ParitionFunctionTest(IEnumerable<Item> parts,
            IEnumerable<IEnumerable<Item>> expectedResults)
        {
            var result = parts.Partition(
                (l, i) => {
                    var alreadyAdded = l.Sum(x => x.QuantityRequired);
                    return (alreadyAdded + i.QuantityRequired >= 20) 
                        ? (20 - alreadyAdded) 
                        : i.QuantityRequired;
                },
                (i, c) => i.QuantityRequired <= c,
                (i, c) =>
                {
                    var child = new Item() { Name = "Split", QuantityRequired = c };
                    i.QuantityRequired -= c;
                    return child;
                }).ToList();
            Assert.AreElementsEqual(expectedResults, result);
        }
    }
}
