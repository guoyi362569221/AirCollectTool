namespace CNEMC.Repository
{
    using Env.CnemcPublish.RiaServices;
    using System;
    using System.Runtime.CompilerServices;

    [Obsolete("请不要使用本类实现业务,因为RepositoryBase派生的类业务实现没有回调,过程完全不可控,仅做实现业务而作参考才保留的这些类")]
    public class RepositoryFactory : IRepositoryFactory
    {
        private static IRepositoryFactory _instance;

        public AQIDataPublishHistoryRepository CreateAQIDataPublishHistoryRepository()
        {
            try
            {
                return new AQIDataPublishHistoryRepository(this.Service);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public AQIDataPublishLiveRepository CreateAQIDataPublishLiveRepository()
        {
            try
            {
                return new AQIDataPublishLiveRepository(this.Service);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public CityAQIPublishHistoryRepository CreateCityAQIPublishHistoryRepository()
        {
            try
            {
                return new CityAQIPublishHistoryRepository(this.Service);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public CityAQIPublishLiveRepository CreateCityAQIPublishLiveRepository()
        {
            try
            {
                return new CityAQIPublishLiveRepository(this.Service);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public CityDayAQIPublishHistoryRepository CreateCityDayAQIPublishHistoryRepository()
        {
            try
            {
                return new CityDayAQIPublishHistoryRepository(this.Service);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public CityDayAQIPublishLiveRepository CreateCityDayAQIPublishLiveRepository()
        {
            try
            {
                return new CityDayAQIPublishLiveRepository(this.Service);

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public CityRepository CreateCityRepository()
        {
            try
            {
                return new CityRepository(this.Service);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IAQIDataPublishHistoryRepository CreateIAQIDataPublishHistoryRepository()
        {
            try
            {
                return new IAQIDataPublishHistoryRepository(this.Service);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IAQIDataPublishLiveRepository CreateIAQIDataPublishLiveRepository()
        {
            try
            {
                return new IAQIDataPublishLiveRepository(this.Service);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ProvinceRepository CreateProvinceRepository()
        {
            try
            {
                return new ProvinceRepository(this.Service);

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public SystemConfigRepository CreateSystemConfigRepository()
        {
            try
            {
                return new SystemConfigRepository(this.Service);
            }
            catch (Exception e)
            {
                return null;
            }
            
        }

        public static IRepositoryFactory Instance
        {
            get
            {
                return (_instance ?? (_instance = new RepositoryFactory()));
            }
            set
            {
                _instance = value;
            }
        }

        public EnvCnemcPublishDomainContext Service { get; set; }
    }
}

