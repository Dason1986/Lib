using Library;
using Library.ComponentModel.Test; 
using NUnit.Framework;
using System;

namespace TestPj.Test
{
    [TestFixture(Category = "編號")]
    public class IdentityGeneratorTest
    {
        [Test]
        public void NewSequentialGuidTset()
        {
            Console.WriteLine(IdentityGenerator.NewGuid());
            Console.WriteLine(IdentityGenerator.NewGuid());
            Console.WriteLine(IdentityGenerator.NewGuid());
            CodeTimer.Time("NewGuid", ConstValue.Times99999, () =>
              {
                  IdentityGenerator.NewGuid();
              });
        }
    }
}