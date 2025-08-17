using BankApp.Infrastructure.Data;
using BankAppDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Infrastructure.Repositories
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly BankDbContext _context;

        public OutboxRepository(BankDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OutboxMessage message)
        {
            _context.OutboxMessages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OutboxMessage>> GetUnprocessedAsync()
        {
            return await _context.OutboxMessages
                .Where(x => !x.IsProcessed)
                .ToListAsync();
        }

        public async Task MarkAsProcessedAsync(int id)
        {
            var msg = await _context.OutboxMessages.FindAsync(id);
            if (msg != null)
            {
                msg.IsProcessed = true;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<OutboxMessage>> GetUnprocessedMessagesAsync()
        {
            return await _context.OutboxMessages
                .Where(x => !x.IsProcessed)
                .ToListAsync();
        }

       

    }

}
