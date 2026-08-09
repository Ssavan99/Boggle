using System;
using System.Threading;
using System.Threading.Tasks;
using Boggle.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Boggle.Services
{
    /// <summary>
    /// Periodically drops games nobody has touched for a while.
    ///
    /// Games live only in the in-memory <see cref="Server"/> singleton, and
    /// nothing removed them before: every "New Game" click leaked a room for the
    /// lifetime of the process. Left alone that grows without bound and, because
    /// game ids are drawn from a fixed space, eventually makes id allocation fail.
    ///
    /// A client that is still polling getGameState keeps its game alive, so an
    /// active table is never collected.
    /// </summary>
    public class StaleGameCleanupService : BackgroundService
    {
        private static readonly TimeSpan SweepInterval = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan IdleTimeout = TimeSpan.FromMinutes(30);

        private readonly ILogger<StaleGameCleanupService> logger;

        public StaleGameCleanupService(ILogger<StaleGameCleanupService> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(SweepInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                try
                {
                    Server srv = Server.getInstance();
                    int removed = srv.removeStaleGames(IdleTimeout);
                    if (removed > 0)
                    {
                        logger.LogInformation(
                            "Removed {Removed} idle game(s); {Remaining} still active.",
                            removed, srv.getGameCount());
                    }
                }
                catch (Exception ex)
                {
                    // A failed sweep must never take the host down; try again next tick.
                    logger.LogError(ex, "Stale game sweep failed.");
                }
            }
        }
    }
}
