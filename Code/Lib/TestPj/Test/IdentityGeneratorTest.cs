using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Library.Test;
using NUnit.Framework;

namespace TestPj.Test
{
    [TestFixture]
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
