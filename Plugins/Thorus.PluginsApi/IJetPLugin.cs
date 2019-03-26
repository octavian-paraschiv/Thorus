using MathNet.Numerics.LinearAlgebra.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorus.PluginsApi
{
    public interface IJetPlugin
    {
        DenseMatrix[] GetJetDeviations(DenseMatrix u, DenseMatrix v);
    }
}
