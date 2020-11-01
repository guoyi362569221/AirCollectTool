namespace CNEMC.Repository
{
    using Env.CnemcPublish.DAL;
    using Env.CnemcPublish.RiaServices;
    using System;
    using System.Collections.Generic;
    using OpenRiaServices.DomainServices.Client;

    public class CityRepository : RepositoryBase<City>
    {
        public CityRepository(EnvCnemcPublishDomainContext service) : base(service)
        {
        }

        public virtual void GetCities(Action<IEnumerable<City>> completedAction)
        {
            try
            {
                EntityQuery<City> citiesQuery = base.Service.GetCitiesQuery();
                base.ProcessCollection(completedAction, citiesQuery);
            }
            catch (Exception e)
            {

            }
           
        }

        public virtual void GetCitiesByPid(Action<IEnumerable<City>> completedAction, int pid)
        {
            try
            {
                EntityQuery<City> citiesByPidQuery = base.Service.GetCitiesByPidQuery(pid);
                base.ProcessCollection(completedAction, citiesByPidQuery);

            }
            catch (Exception e)
            {

            }
            
        }

        public virtual void GetPid(Action<int> completedAction, string CityName)
        {
            try
            {
                base.Service.GetPid(CityName).Completed += (delegate (object sender, EventArgs e) {
                    InvokeOperation<int> operation = sender as InvokeOperation<int>;
                    if (operation != null)
                    {
                        completedAction(operation.Value);
                    }
                });
            }
            catch (Exception e)
            {

            }
           
        }
    }
}

