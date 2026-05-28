using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.IRepositories
{
    public interface ICartRepository
    {

        public Task<Cart?> GetByGuestCartIdAsync(Guid guestCartId);

        public Task AddAsync (Cart cart);

        public Task UpdateAsync (Cart cart);

        public Task SaveChangesAsync();

        public Task<Cart?> FindAsync(int cartId);
    }
}
