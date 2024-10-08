﻿using Microsoft.EntityFrameworkCore;
using NotificationSystem.Entities;
using NotificationSystem.Senders;

namespace NotificationSystem.Repositories;

public sealed class OutboxRepository(AppDbContext context) : IOutboxRepository
{
    private const int MAX_RETRY = 3;
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<FailedNotification>> GetPendingNotifications()
    {
        return await _context.FailedNotifications
            .AsNoTracking()
            .Where(x => x.IsReProcessSuccess == false)
            .Where(x => x.RetryCount < MAX_RETRY)
            .ToListAsync();
    }

    public async Task IncreaseFailedCount(FailedNotification entry)
    {
        await _context.FailedNotifications
            .Where(x => x.Id == entry.Id)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(y => y.RetryCount, entry.RetryCount + 1));
    }

    public async Task MarkAsProcessed(FailedNotification entry)
    {
        await _context.FailedNotifications
            .Where(x => x.Id == entry.Id)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(y => y.IsReProcessSuccess, true));
    }

    public async Task SaveFailedNotification(NotificationType type, string message, User user, string errorMessage)
    {
        FailedNotification failedNotification = new()
        {
            UserId = user.Id,
            NotificationType = type,
            Message = message,
            IsReProcessSuccess = false
        };
        await _context.FailedNotifications.AddAsync(failedNotification);
        await _context.SaveChangesAsync();
    }
}
