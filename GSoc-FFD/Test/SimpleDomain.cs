using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * WindInflow.cs
 * Copyright 2016 Lukas Bystricky <lb13f@my.fsu.edu>
 *
 * This work is licensed under the GNU GPL license version 2 or later.
 */
 
namespace FastFluidSolver
{
    /// <summary>
    /// Domain with an exponential wind profile on the inflow (x = 0), 
    /// 0 velocity on the ground (z = 0) and all other boundaries marked as outflow.
    /// </summary>
    public class SimpleDomain : Domain
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Nx">Number of cells (including ghost cells) in x direction</param>
        /// <param name="Ny">Number of cells (including ghost cells) in x direction</param>
        /// <param name="Nz">Number of cells (including ghost cells) in x direction</param>
        /// <param name="length_x">Length of domain in x direction (not including ghost cells)</param>
        /// <param name="length_y">Length of domain in y direction (not including ghost cells)</param>
        /// <param name="length_z">Length of domain in z direction (not including ghost cells)</param>
        public SimpleDomain(int Nx, int Ny, int Nz, double length_x, 
            double length_y, double length_z)
        {
            this.Nx = Nx;
            this.Ny = Ny;
            this.Nz = Nz;

            this.length_x = length_x;
            this.length_y = length_y;
            this.length_z = length_z;

            hx = length_x / (Nx - 2);
            hy = length_y / (Ny - 2);
            hz = length_z / (Nz - 2);

            boundary_cells = new int[Nx, Ny, Nz];
            obstacle_cells = new int[Nx, Ny, Nz];
            boundary_normal_x = new int[Nx, Ny, Nz];
            boundary_normal_y = new int[Nx, Ny, Nz];
            boundary_normal_z = new int[Nx, Ny, Nz];
            boundary_u = new double[Nx - 1, Ny, Nz];
            boundary_v = new double[Nx, Ny - 1, Nz];
            boundary_w = new double[Nx, Ny, Nz - 1];

            outflow_boundary_x = new int[Nx, Ny, Nz];
            outflow_boundary_y = new int[Nx, Ny, Nz];
            outflow_boundary_z = new int[Nx, Ny, Nz];
        }
    }
}
