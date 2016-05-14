using System;

namespace Cron.Interfaces
{
    public interface IFireTimeEvaluator
    {
        DateTimeOffset? NextFire();
    }
}
