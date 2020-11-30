﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public interface ISearchRunner
    {
        string Name { get; }
        bool Disabled { get; }
        Task<long> Run(string query);
    }
}
