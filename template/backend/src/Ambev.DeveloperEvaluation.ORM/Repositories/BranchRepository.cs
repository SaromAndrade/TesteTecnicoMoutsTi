using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly DefaultContext _context;

        public BranchRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<List<Branch>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Branchs.OrderBy(b => b.Name).ToListAsync(cancellationToken);
        }

        public async Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Branchs.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
    }
}
