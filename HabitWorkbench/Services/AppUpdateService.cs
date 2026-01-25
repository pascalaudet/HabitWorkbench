using System;
using System.Threading;
using System.Threading.Tasks;
using Velopack;
using Velopack.Sources;

namespace HabitWorkbench.Services
{
    /// <summary>
    /// Silent updater:
    /// - On startup: check + download updates (no restart)
    /// - On close: if update is pending restart, apply + restart
    /// </summary>
    public sealed class AppUpdateService
    {
        private readonly UpdateManager _um;
        private int _started;

        // TODO: set these
        //private const string RepoUrl = "https://github.com/pascalaudet/HabitWorkbench";
        private const string RepoUrl = "pascalaudet/HabitWorkbench";
        private const bool IncludePrereleases = false;

        public AppUpdateService()
        {
            // GitHub Releases as update source (free for OSS)
            var source = new GithubSource(RepoUrl, accessToken: null, prerelease: IncludePrereleases);
            _um = new UpdateManager(source);
        }

        /// <summary>
        /// Call once on startup. Safe to call repeatedly; it will only run once.
        /// </summary>
        public void StartBackgroundCheck()
        {
            if (Interlocked.Exchange(ref _started, 1) == 1)
                return;

            _ = Task.Run(async () =>
            {
                try
                {
                    var updateInfo = await _um.CheckForUpdatesAsync().ConfigureAwait(false);
                    if (updateInfo is null)
                        return;

                    // Download, but DO NOT restart/apply now.
                    await _um.DownloadUpdatesAsync(updateInfo).ConfigureAwait(false);
                    // At this point, UpdatePendingRestart should be set.
                }
                catch
                {
                    // Deliberately swallow: no UI, no support burden.
                    // (Optional: add debug logging in DEBUG builds.)
                }
            });
        }

        /// <summary>
        /// Call during shutdown/close. If an update is ready, apply and restart.
        /// </summary>
        public void ApplyIfPendingRestart()
        {
            try
            {
                var pending = _um.UpdatePendingRestart;
                if (pending is null)
                    return;

                _um.ApplyUpdatesAndRestart(pending);
            }
            catch
            {
                // Same philosophy: silent failure.
            }
        }
    }
}
