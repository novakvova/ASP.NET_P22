using Core.Interfaces;
using Quartz;

namespace WebApiPizushi.Jobs;

public class DbSeedJob(IDbSeederService dbSeeder) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await dbSeeder.SeedData();
    }
}
