using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Single;
using Thorus.PluginsApi;

namespace Thorus.JetPlugin
{
    public class Implementor : IJetPlugin
    {
        public DenseMatrix[] GetJetDeviations(DenseMatrix u, DenseMatrix v)
        {
            return null;
        }
    }
}
