using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Test.TestExtensions
{
    public static class AssertException
    {
        //ncrunch: no coverage start
        public static T Throws<T>(Action action) where T : Exception
        {
            try
            {
                action();
                Assert.Fail("Should throw {0}", typeof(T));
            }
            catch (T ex)
            {
                return ex;
            }
            catch (Exception ex)
            {
                Assert.Fail("Should throw {0}, actually threw {1}", typeof(T), ex.GetType());
            }

            return null;
        }
        //ncrunch: no coverage end
    }
}
